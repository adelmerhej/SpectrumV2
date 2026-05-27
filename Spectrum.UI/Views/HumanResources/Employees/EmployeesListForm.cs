using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.HumanResources.Employees;
using Spectrum.Models.Users;
using Spectrum.Utilities;
using Spectrum.Utilities.Interfaces;
using Spectrum.Utilities.Layout;
using Spectrum.UI.Utilities;
using Spectrum.DataLayers.HumanResources.Employees;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Spectrum.Models.HumanResources.EmployeeTypes;

namespace Spectrum.Views.HumanResources.Employees
{
    public partial class EmployeesListForm : RibbonForm, IFormWithRibbon
    {
        private bool _resetMenu;
        private EmployeeEditForm _employeeEditForm;

        private EmployeeModel _employeeModel = new EmployeeModel();
        private IList<EmployeeModel> _employees = new List<EmployeeModel>();

        private readonly EmployeeRepository _employeeRepository = new EmployeeRepository(DatabaseFactory.ProfilePrimary);

        //Init permissionvariables
        private bool _canAdd = true;
        private bool _canEdit = true;
        private bool _canDelete = true;
        private bool _canPrint = true;
        private bool _isAdmin = true;
        private bool _isProtected = true;

        #region Implementation of IFormWithRibbon

        public RibbonControl MainRibbon => rcEmployeesList;
        public RibbonPage DefaultPage => rpEmployeesList;


        #endregion

        public EmployeesListForm()
        {
            InitializeComponent();

            StartLoading();
        }

        private async void StartLoading()
        {
            await InitializeBindings();
            WireUpBindings();
            ApplyDefaults();
            ApplyPermissions();
        }

        private async Task InitializeBindings()
        {
            try
            {
                //	//
                //	_formId = _formRepository.SelectFormByName(_formName);
                //	_userPermission = _userPermissionRepository.SelectUserPermissionById(CurrentUser.UserId, _formId);
                //	if (_userPermission is { Count: > 0 })
                //	{
                //		var isProtected = _userPermission.SingleOrDefault(x => x.ControlName == "IsProtected")?.Value;
                //		if (isProtected != null) _isProtected = (bool)isProtected;
                //	}
                //	//

				var employeesTask = _employeeRepository.GetEmployeesAsync();
				var peopleTask = PeopleDirectory.GetPeopleAsync();
				await Task.WhenAll(employeesTask, peopleTask);
				_employees = await employeesTask ?? new List<EmployeeModel>();

				// Add engineers that are not already in employees (display-only rows)
				var people = await peopleTask ?? new List<PeopleDirectory.PersonLookup>();
				var employeeKeys = new HashSet<string>(
					(_employees ?? new List<EmployeeModel>()).Select(e => NormalizeKey(e.FullName)),
					StringComparer.OrdinalIgnoreCase);

				foreach (var p in people.Where(x => x.EngineerId != null))
				{
					if (string.IsNullOrWhiteSpace(p.FullName)) continue;
					if (employeeKeys.Contains(p.Key)) continue;

					var parts = p.FullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
					var firstName = parts.Length > 0 ? parts[0] : p.FullName;
					var lastName = parts.Length > 1 ? string.Join(" ", parts.Skip(1)) : string.Empty;

					_employees.Add(new EmployeeModel
					{
						_id = p.EngineerId,
						FirstName = firstName,
						LastName = lastName,
                        EmployeeType = new EmployeeTypeModel
                        {
                            TypeName = Utilities.Enums.EmployeeType.Engineer.ToString()
                        },
						Nationality = p.Country,
						PlaceOfBirth = p.City,
						IdCardOrPassportNo = null,
						Specialization = p.Specialization,
						FamilyStatus = p.Status,
						ContactInfo = new Spectrum.Models.HumanResources.Employees.EmployeeContactInfo
						{
							Email = p.Email,
							LocalMobileNo = p.Phone1,
							AbroadMobileNo = p.Phone2,
							LocalFixPhone = p.Phone3
						}
					});
				}
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

		private static string NormalizeKey(string name)
		{
			if (string.IsNullOrWhiteSpace(name)) return null;
			var cleaned = new string(name
				.Trim()
				.ToUpperInvariant()
				.Where(ch => char.IsLetterOrDigit(ch) || char.IsWhiteSpace(ch))
				.ToArray());
			return string.Join(" ", cleaned.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
		}

        private void WireUpBindings()
        {
            gcEmployees.DataSource = null;
            gcEmployees.DataSource = _employees;
        }

        private void ApplyDefaults()
        {

        }

        private void ApplyPermissions()
        {
            //if (_userPermission == null) return;
            //if (_userPermission.Count <= 0) return;

            //var canAdd = _userPermission.SingleOrDefault(x => x.ControlName == "CanAdd")?.Value;
            //if (canAdd != null) _canAdd = (bool)canAdd;

            //var canEdit = _userPermission.SingleOrDefault(x => x.ControlName == "CanEdit")?.Value;
            //if (canEdit != null) _canEdit = (bool)canEdit;

            //var canDelete = _userPermission.SingleOrDefault(x => x.ControlName == "CanDelete")?.Value;
            //if (canDelete != null) _canDelete = (bool)canDelete;

            //var canPrint = _userPermission.SingleOrDefault(x => x.ControlName == "CanPrint")?.Value;
            //if (canPrint != null) _canPrint = (bool)canPrint;

            //var isAdmin = _userPermission.SingleOrDefault(x => x.ControlName == "IsAdmin")?.Value;
            //if (isAdmin != null) _isAdmin = (bool)isAdmin;

            btnNew.Enabled = _isAdmin || _canAdd;
            btnEdit.Enabled = _isAdmin || _canEdit;
            btnPrint.Enabled = _isAdmin || _canPrint;
            btnDelete.Enabled = _isAdmin || _canDelete;
        }


        #region Button Events

        private void btnNew_ItemClick(object sender, ItemClickEventArgs e)
        {
            ShowEmployeeEditor(new EmployeeModel());
        }

        private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!_employees.Any()) return;

            try
            {
                string currentRowId = gvEmployees.GetFocusedRowCellValue("_id").ToString();
                if (string.IsNullOrEmpty(currentRowId)) return;

                _employeeModel = _employees.SingleOrDefault(x => x._id == currentRowId);
                if (_employeeModel == null) return;

                ShowEmployeeEditor(_employeeModel);
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {
            StartLoading();
        }

        private void btnPrint_ItemClick(object sender, ItemClickEventArgs e)
        {
            gcEmployees.ShowRibbonPrintPreview();
        }

        private async void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!CanDelete()) return;

            try
            {
                string id = gvEmployees.GetFocusedRowCellValue("_id").ToString();
                string name = gvEmployees.GetFocusedRowCellValue("FullName").ToString();

                if (!string.IsNullOrEmpty(id))
                {
                    if (XtraMessageBox.Show($"Are you sure you want to delete Record: `{name}`?",
                            "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        _employeeModel = gvEmployees.GetFocusedRow() as EmployeeModel;
                        if (_employeeModel == null)
                        {
                            return;
                        }
                        _employeeModel.Deleted = true;

                        //delete the record
                        await _employeeRepository.DeleteEmployeeAsync(_employeeModel._id);
                        RcvUpdatedEmployeeAsync(_employeeModel, EventArgs.Empty);
                    }
                }
            }
            catch (Exception exception)
            {
                switch (exception.Message)
                {
                    case "-2146233088":
                        XtraMessageBox.Show("This record is linked to one or more transactions, delete all links first.",
                            "Delete error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;

                    default:
                        XtraMessageBox.Show(exception.Message,
                            "Delete error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }
        }

        private void customersRating_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void btnClose_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }

        private void btnResetGridStyle_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (XtraMessageBox.Show("This will reset Grid layout next login, to its default settings.\nAre you sure you want to continue?", "Reset Menu...",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) ==
                DialogResult.Yes)
            {
                _resetMenu = true;
                LayoutsStyle.ResetLayoutGrid(gvEmployees, CurrentUser.UserName, CurrentUser.Company);
            }
        }
        #endregion



        private async void RcvUpdatedEmployeeAsync(object sender, EventArgs e)
        {
            if (sender == null) return;
            _employeeModel = sender as EmployeeModel;

            if (_employeeModel != null && (_employeeModel.LastModifiedDate == null || _employeeModel.Deleted))
            {
                await InitializeBindings();
                WireUpBindings();
            }
            else
            {
                gvEmployees.UpdateCurrentRow();
            }
        }

        private void gvCities_DoubleClick(object sender, EventArgs e)
        {
            if (!_employees.Any()) return;

            try
            {
                string currentRowId = gvEmployees.GetFocusedRowCellValue("_id").ToString();
                if (string.IsNullOrEmpty(currentRowId)) return;

                _employeeModel = _employees.SingleOrDefault(x => x._id == currentRowId);
                if (_employeeModel == null) return;

                ShowEmployeeEditor(_employeeModel);
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool CanDelete()
        {
            EmployeeModel dataBoundItem = gvEmployees.GetFocusedRow() as EmployeeModel;

            if (gvEmployees == null || gvEmployees.SelectedRowsCount == 0) return false;
            if (gvEmployees.SelectedRowsCount > 1)
            {
                XtraMessageBox.Show("Only one record can be selected at a time, please try again",
                    "Delete error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (dataBoundItem != null && dataBoundItem.IsDefault)
            {
                XtraMessageBox.Show("Cannot delete system record!",
                    "Delete error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void ShowEmployeeEditor(EmployeeModel model)
        {
            if (_employeeEditForm == null || _employeeEditForm.IsDisposed)
            {
                _employeeEditForm = new EmployeeEditForm(model);
                _employeeEditForm.SendUpdatedEmployee += RcvUpdatedEmployeeAsync;
                _employeeEditForm.FormClosed += EmployeeEditForm_FormClosed;
                _employeeEditForm.Show(this);
                return;
            }

            if (_employeeEditForm.WindowState == FormWindowState.Minimized)
                _employeeEditForm.WindowState = FormWindowState.Normal;

            _employeeEditForm.Activate();
            _employeeEditForm.BringToFront();
        }

        private void EmployeeEditForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            var form = sender as EmployeeEditForm;
            if (form != null)
            {
                //form.SendUpdatedEmployee -= RcvUpdatedEmployeeAsync;
                form.FormClosed -= EmployeeEditForm_FormClosed;
            }
            if (ReferenceEquals(_employeeEditForm, sender))
                _employeeEditForm = null;
        }

        private void gvCities_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            var view = sender as GridView;
            if (view == null || e.RowHandle < 0) return;
            bool isActive = HelperApplication.ConvertToBool(view.GetRowCellValue(e.RowHandle, "Active")) ?? false;
            bool isDefault = HelperApplication.ConvertToBool(view.GetRowCellValue(e.RowHandle, "IsDefault")) ?? false;
            if (!isActive)
            {
                e.Appearance.ForeColor = Color.Gray;
                e.Appearance.Font = new Font("Tahoma", 8, FontStyle.Italic);
            }
            if (isDefault)
            {
                e.Appearance.Font = new Font("Tahoma", 8, FontStyle.Bold);
            }
        }
    }
}