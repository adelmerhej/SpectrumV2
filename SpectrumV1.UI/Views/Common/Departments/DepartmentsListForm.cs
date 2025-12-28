using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using SpectrumV1.DataLayers.Common.Departments;
using SpectrumV1.DataLayers.DataAccess;
using SpectrumV1.Models.Common.Countries;
using SpectrumV1.Models.Common.Departments;
using SpectrumV1.Models.Users;
using SpectrumV1.Utilities;
using SpectrumV1.Utilities.Interfaces;
using SpectrumV1.Utilities.Layout;
using SpectrumV1.Views.Common.Countries;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpectrumV1.Views.Common.Departments
{
	public partial class DepartmentsListForm : RibbonForm, IFormWithRibbon
	{
		private bool _resetMenu;
		private DepartmentEditForm _departmentEditForm;

		private DepartmentModel _departmentModel = new DepartmentModel();
		private IList<DepartmentModel> _departments = new List<DepartmentModel>();

		private readonly DepartmentRepository _departmentRepository = new DepartmentRepository(DatabaseFactory.ProfilePrimary);

		//Init permissionvariables
		private bool _canAdd = true;
		private bool _canEdit = true;
		private bool _canDelete = true;
		private bool _canPrint = true;
		private bool _isAdmin = true;
		private bool _isProtected = true;

		#region Implementation of IFormWithRibbon

		public RibbonControl MainRibbon => rcDepartments;
		public RibbonPage DefaultPage => rpDepartments;


		#endregion

		public DepartmentsListForm()
		{
			InitializeComponent();

			// wire events
			btnNew.ItemClick += btnNew_ItemClick;
			btnEdit.ItemClick += btnEdit_ItemClick;
			btnDelete.ItemClick += btnDelete_ItemClick;
			btnPrint.ItemClick += btnPrint_ItemClick;
			btnRefresh.ItemClick += btnRefresh_ItemClick;
			btnClose.ItemClick += btnClose_ItemClick;
			btnResetGridStyle.ItemClick += btnResetGridStyle_ItemClick;
			gvDepartments.DoubleClick += gvDepartments_DoubleClick;
			gvDepartments.RowCellStyle += gvDepartments_RowCellStyle;

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

				_departments = await _departmentRepository.GetDepartmentsAsync();
			}
			catch (Exception ex)
			{
				XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void WireUpBindings()
		{
			gcDepartments.DataSource = null;
			gcDepartments.DataSource = _departments;
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

		#region Buttons Event

		private void btnNew_ItemClick(object sender, ItemClickEventArgs e)
		{
			ShowDepartmentEditor(new DepartmentModel());
		}

		private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!_departments.Any()) return;

			try
			{
				string currentRowId = gvDepartments.GetFocusedRowCellValue("_id").ToString();
				if (string.IsNullOrEmpty(currentRowId)) return;

				_departmentModel = _departments.SingleOrDefault(x => x._id == currentRowId);
				if (_departmentModel == null) return;

				ShowDepartmentEditor(_departmentModel);
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
			gcDepartments.ShowRibbonPrintPreview();
		}

		private async void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!CanDelete()) return;

			try
			{
				string id = gvDepartments.GetFocusedRowCellValue("_id").ToString();
				string name = gvDepartments.GetFocusedRowCellValue("DepartmentName").ToString();

				if (!string.IsNullOrEmpty(id))
				{
					if (XtraMessageBox.Show($"Are you sure you want to delete Record: `{name}`?",
							"Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
							MessageBoxDefaultButton.Button2) == DialogResult.Yes)
					{
						_departmentModel = gvDepartments.GetFocusedRow() as DepartmentModel;
						if (_departmentModel == null)
						{
							return;
						}
						_departmentModel.Deleted = true;

						//delete the record
						await _departmentRepository.DeleteDepartmentAsync(_departmentModel._id);
						RcvUpdatedDepartmentAsync(_departmentModel, EventArgs.Empty);
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
				LayoutsStyle.ResetLayoutGrid(gvDepartments, CurrentUser.UserName, CurrentUser.Company);
			}
		}


        #endregion

        private void gvDepartments_DoubleClick(object sender, EventArgs e)
		{
			if (!_departments.Any()) return;

			try
			{
				string currentRowId = gvDepartments.GetFocusedRowCellValue("_id").ToString();
				if (string.IsNullOrEmpty(currentRowId)) return;

				_departmentModel = _departments.SingleOrDefault(x => x._id == currentRowId);
				if (_departmentModel == null) return;

				ShowDepartmentEditor(_departmentModel);
			}
			catch (Exception exception)
			{
				XtraMessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

        private async void RcvUpdatedDepartmentAsync(object sender, EventArgs e)
		{
			if (sender == null) return;
			_departmentModel = sender as DepartmentModel;
			if (_departmentModel == null) return;

			if (_departmentModel.Deleted || _departmentModel.LastModifiedDate == null)
			{
				await InitializeBindings();
				WireUpBindings();
			}
			else
			{
				gcDepartments.RefreshDataSource();
				gvDepartments.RefreshRow(gvDepartments.FocusedRowHandle);
				gvDepartments.UpdateCurrentRow();
			}
		}

		private bool CanDelete()
		{
			DepartmentModel dataBoundItem = gvDepartments.GetFocusedRow() as DepartmentModel;

			if (gvDepartments == null || gvDepartments.SelectedRowsCount == 0) return false;
			if (gvDepartments.SelectedRowsCount > 1)
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

		private void ShowDepartmentEditor(DepartmentModel model)
		{
			if (_departmentEditForm == null || _departmentEditForm.IsDisposed)
			{
				_departmentEditForm = new DepartmentEditForm(model);
				_departmentEditForm.SendUpdatedDepartment += RcvUpdatedDepartmentAsync;
				_departmentEditForm.FormClosed += DepartmentEditForm_FormClosed;
				_departmentEditForm.Show(this);
				return;
			}

			if (_departmentEditForm.WindowState == FormWindowState.Minimized)
				_departmentEditForm.WindowState = FormWindowState.Normal;

			_departmentEditForm.Activate();
			_departmentEditForm.BringToFront();
		}

		private void DepartmentEditForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			var form = sender as DepartmentEditForm;
			if (form != null)
			{
				form.SendUpdatedDepartment -= RcvUpdatedDepartmentAsync;
				form.FormClosed -= DepartmentEditForm_FormClosed;
			}
			if (ReferenceEquals(_departmentEditForm, sender))
				_departmentEditForm = null;
		}
		private void gvDepartments_RowCellStyle(object sender, RowCellStyleEventArgs e)
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