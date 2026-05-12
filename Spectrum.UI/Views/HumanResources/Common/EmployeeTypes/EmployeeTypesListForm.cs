using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Users;
using Spectrum.Utilities;
using Spectrum.Utilities.Interfaces;
using Spectrum.Utilities.Layout;
using Spectrum.DataLayers.EmployeeTypes;
using Spectrum.Models.HumanResources.EmployeeTypes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.HumanResources.Common.EmployeeTypes
{
    public partial class EmployeeTypesListForm : RibbonForm, IFormWithRibbon
    {
        private bool _resetMenu;
        private EmployeeTypeEditForm _employeeTypeEditForm;

        private EmployeeTypeModel _employeeTypeModel = new EmployeeTypeModel();
        private IList<EmployeeTypeModel> _employeeTypes = new List<EmployeeTypeModel>();

        private readonly EmployeeTypeRepository _employeeTypeRepository = new EmployeeTypeRepository(DatabaseFactory.ProfilePrimary);

        //Init permissionvariables
        private bool _canAdd = true;
        private bool _canEdit = true;
        private bool _canDelete = true;
        private bool _canPrint = true;
        private bool _isAdmin = true;
        private bool _isProtected = true;

        #region Implementation of IFormWithRibbon

        public RibbonControl MainRibbon => rcEmployeeTypeList;
        public RibbonPage DefaultPage => rpEmployeeTypeList;


        #endregion


        public EmployeeTypesListForm()
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

                _employeeTypes = await _employeeTypeRepository.GetEmployeeTypesAsync();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WireUpBindings()
        {
            gcEmployeeTypes.DataSource = null;
            gcEmployeeTypes.DataSource = _employeeTypes;
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
            ShowEmployeeTypeEditor(new EmployeeTypeModel());
        }

        private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!_employeeTypes.Any()) return;

            try
            {
                string currentRowId = gvEmployeeTypes.GetFocusedRowCellValue("_id").ToString();
                if (string.IsNullOrEmpty(currentRowId)) return;

                _employeeTypeModel = _employeeTypes.SingleOrDefault(x => x._id == currentRowId);
                if (_employeeTypeModel == null) return;

                ShowEmployeeTypeEditor(_employeeTypeModel);
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
            gcEmployeeTypes.ShowRibbonPrintPreview();
        }

        private async void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!CanDelete()) return;

            try
            {
                string id = gvEmployeeTypes.GetFocusedRowCellValue("_id").ToString();
                string name = gvEmployeeTypes.GetFocusedRowCellValue("TypeName").ToString();

                if (!string.IsNullOrEmpty(id))
                {
                    if (XtraMessageBox.Show($"Are you sure you want to delete Record: `{name}`?",
                            "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        _employeeTypeModel = gvEmployeeTypes.GetFocusedRow() as EmployeeTypeModel;
                        if (_employeeTypeModel == null)
                        {
                            return;
                        }
                        _employeeTypeModel.Deleted = true;

                        //delete the record
                        await _employeeTypeRepository.DeleteEmployeeTypeAsync(_employeeTypeModel._id);
                        RcvUpdatedEmployeeTypeAsync(_employeeTypeModel, EventArgs.Empty);
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
                LayoutsStyle.ResetLayoutGrid(gvEmployeeTypes, CurrentUser.UserName, CurrentUser.Company);
            }

        }


        #endregion

        private void gvEmployeeTypes_DoubleClick(object sender, EventArgs e)
        {
            if (!_employeeTypes.Any()) return;

            try
            {
                string currentRowId = gvEmployeeTypes.GetFocusedRowCellValue("_id").ToString();
                if (string.IsNullOrEmpty(currentRowId)) return;

                _employeeTypeModel = _employeeTypes.SingleOrDefault(x => x._id == currentRowId);
                if (_employeeTypeModel == null) return;

                ShowEmployeeTypeEditor(_employeeTypeModel);
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void RcvUpdatedEmployeeTypeAsync(object sender, EventArgs e)
        {
            if (sender == null) return;
            _employeeTypeModel = sender as EmployeeTypeModel;
            if (_employeeTypeModel == null) return;

            if (_employeeTypeModel.Deleted || _employeeTypeModel.LastModifiedDate == null)
            {
                await InitializeBindings();
                WireUpBindings();
            }
            else
            {
                gcEmployeeTypes.RefreshDataSource();
                gvEmployeeTypes.RefreshRow(gvEmployeeTypes.FocusedRowHandle);
                gvEmployeeTypes.UpdateCurrentRow();
            }
        }

        private bool CanDelete()
        {
            EmployeeTypeModel dataBoundItem = gvEmployeeTypes.GetFocusedRow() as EmployeeTypeModel;

            if (gvEmployeeTypes == null || gvEmployeeTypes.SelectedRowsCount == 0) return false;
            if (gvEmployeeTypes.SelectedRowsCount > 1)
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

        private void ShowEmployeeTypeEditor(EmployeeTypeModel model)
        {
            if (_employeeTypeEditForm == null || _employeeTypeEditForm.IsDisposed)
            {
                _employeeTypeEditForm = new EmployeeTypeEditForm(model);
                _employeeTypeEditForm.SendUpdatedEmployeeType += RcvUpdatedEmployeeTypeAsync;
                _employeeTypeEditForm.FormClosed += EmployeeTypeEditForm_FormClosed;
                _employeeTypeEditForm.Show(this);
                return;
            }

            if (_employeeTypeEditForm.WindowState == FormWindowState.Minimized)
                _employeeTypeEditForm.WindowState = FormWindowState.Normal;

            _employeeTypeEditForm.Activate();
            _employeeTypeEditForm.BringToFront();
        }

        private void EmployeeTypeEditForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            var form = sender as EmployeeTypeEditForm;
            if (form != null)
            {
                form.SendUpdatedEmployeeType -= RcvUpdatedEmployeeTypeAsync;
                form.FormClosed -= EmployeeTypeEditForm_FormClosed;
            }
            if (ReferenceEquals(_employeeTypeEditForm, sender))
                _employeeTypeEditForm = null;
        }

        private void gvEmployeeTypes_RowCellStyle(object sender, RowCellStyleEventArgs e)
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