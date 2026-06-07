using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using Spectrum.DataLayers.Common.Services;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Common.Services;
using Spectrum.Models.Users;
using Spectrum.Utilities;
using Spectrum.Utilities.Interfaces;
using Spectrum.Utilities.Layout;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.Common.Services
{
	public partial class ServicesListForm : RibbonForm, IFormWithRibbon
	{
		private bool _resetMenu;
		private ServiceEditForm _serviceEditForm;

		private ServiceModel _serviceModel = new ServiceModel();
		private IList<ServiceModel> _services = new List<ServiceModel>();

		private readonly ServiceRepository _serviceRepository = new ServiceRepository(DatabaseFactory.ProfilePrimary);

		//Init permissionvariables
		private bool _canAdd = true;
		private bool _canEdit = true;
		private bool _canDelete = true;
		private bool _canPrint = true;
		private bool _isAdmin = true;
		private bool _isProtected = true;

		#region Implementation of IFormWithRibbon

		public RibbonControl MainRibbon => rcServicesList;
		public RibbonPage DefaultPage => rpServicesList;


		#endregion

		public ServicesListForm()
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

				_services = await _serviceRepository.GetServicesAsync();
			}
			catch (Exception ex)
			{
				XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void WireUpBindings()
		{
			gcServices.DataSource = null;
			gcServices.DataSource = _services;
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
			ShowServiceEditor(new ServiceModel());
		}

		private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!_services.Any()) return;

			try
			{
				string currentRowId = gvServices.GetFocusedRowCellValue("_id").ToString();
				if (string.IsNullOrEmpty(currentRowId)) return;

				_serviceModel = _services.SingleOrDefault(x => x._id == currentRowId);
				if (_serviceModel == null) return;

				ShowServiceEditor(_serviceModel);
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
			gcServices.ShowRibbonPrintPreview();
		}

		private async void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!CanDelete()) return;

			try
			{
				string id = gvServices.GetFocusedRowCellValue("_id").ToString();
				string name = gvServices.GetFocusedRowCellValue("ServiceName").ToString();

				if (!string.IsNullOrEmpty(id))
				{
					if (XtraMessageBox.Show($"Are you sure you want to delete Record: `{name}`?",
							"Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
							MessageBoxDefaultButton.Button2) == DialogResult.Yes)
					{
						_serviceModel = gvServices.GetFocusedRow() as ServiceModel;
						if (_serviceModel == null)
						{
							return;
						}
						_serviceModel.Deleted = true;

						//delete the record
						await _serviceRepository.DeleteServiceAsync(_serviceModel._id);
						RcvUpdatedServiceAsync(_serviceModel, EventArgs.Empty);
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
				LayoutsStyle.ResetLayoutGrid(gvServices, CurrentUser.UserName, CurrentUser.Company);
			}
		}

		#endregion

		private void gvServices_DoubleClick(object sender, EventArgs e)
		{
			if (!_services.Any()) return;

			try
			{
				string currentRowId = gvServices.GetFocusedRowCellValue("_id").ToString();
				if (string.IsNullOrEmpty(currentRowId)) return;

				_serviceModel = _services.SingleOrDefault(x => x._id == currentRowId);
				if (_serviceModel == null) return;

				ShowServiceEditor(_serviceModel);
			}
			catch (Exception exception)
			{
				XtraMessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private async void RcvUpdatedServiceAsync(object sender, EventArgs e)
		{
			if (sender == null) return;
			_serviceModel = sender as ServiceModel;
			if (_serviceModel == null) return;

			if (_serviceModel.Deleted || _serviceModel.LastModifiedDate == null)
			{
				await InitializeBindings();
				WireUpBindings();
			}
			else
			{
				gcServices.RefreshDataSource();
				gvServices.RefreshRow(gvServices.FocusedRowHandle);
				gvServices.UpdateCurrentRow();
			}
		}

		private bool CanDelete()
		{
			ServiceModel dataBoundItem = gvServices.GetFocusedRow() as ServiceModel;

			if (gvServices == null || gvServices.SelectedRowsCount == 0) return false;
			if (gvServices.SelectedRowsCount > 1)
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

		private void ShowServiceEditor(ServiceModel model)
		{
			if (_serviceEditForm == null || _serviceEditForm.IsDisposed)
			{
				_serviceEditForm = new ServiceEditForm(model);
				_serviceEditForm.SendUpdatedService += RcvUpdatedServiceAsync;
				_serviceEditForm.FormClosed += ServiceEditForm_FormClosed;
				_serviceEditForm.Show(this);
				return;
			}

			if (_serviceEditForm.WindowState == FormWindowState.Minimized)
				_serviceEditForm.WindowState = FormWindowState.Normal;

			_serviceEditForm.Activate();
			_serviceEditForm.BringToFront();
		}

		private void ServiceEditForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			var form = sender as ServiceEditForm;
			if (form != null)
			{
				form.SendUpdatedService -= RcvUpdatedServiceAsync;
				form.FormClosed -= ServiceEditForm_FormClosed;
			}
			if (ReferenceEquals(_serviceEditForm, sender))
				_serviceEditForm = null;
		}

		private void gvServices_RowCellStyle(object sender, RowCellStyleEventArgs e)
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