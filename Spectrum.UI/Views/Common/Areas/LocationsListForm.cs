using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using Spectrum.DataLayers.Common.Locations;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Common.Areas;
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

namespace Spectrum.Views.Common.Areas
{
	public partial class LocationsListForm : RibbonForm, IFormWithRibbon
	{
		private bool _resetMenu;
		private LocationEditForm _locationEditForm;

		private LocationModel _locationModel = new LocationModel();
		private IList<LocationModel> _locations = new List<LocationModel>();

		private readonly LocationRepository _locationRepository = new LocationRepository(DatabaseFactory.ProfilePrimary);

		//Init permissionvariables
		private bool _canAdd = true;
		private bool _canEdit = true;
		private bool _canDelete = true;
		private bool _canPrint = true;
		private bool _isAdmin = true;
		private bool _isProtected = true;

		#region Implementation of IFormWithRibbon

		public RibbonControl MainRibbon => rcLocations;
		public RibbonPage DefaultPage => rpLocations;


		#endregion


		public LocationsListForm()
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

				_locations = await _locationRepository.GetLocationsAsync();
			}
			catch (Exception ex)
			{
				XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void WireUpBindings()
		{
			gcLocations.DataSource = null;
			gcLocations.DataSource = _locations;
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

		private void LocationsListForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!_resetMenu)
			{
				LayoutsStyle.SaveLayoutGrid(gvLocations, CurrentUser.UserName, CurrentUser.Company);
			}
		}

		#region Buttons Event

		private void btnNew_ItemClick(object sender, ItemClickEventArgs e)
		{
			ShowLocationEditor(new LocationModel());
		}

		private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!_locations.Any()) return;

			try
			{
				string currentRowId = gvLocations.GetFocusedRowCellValue("_id").ToString();
				if (string.IsNullOrEmpty(currentRowId)) return;

				_locationModel = _locations.SingleOrDefault(x => x._id == currentRowId);
				if (_locationModel == null) return;

				ShowLocationEditor(_locationModel);
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
			gcLocations.ShowRibbonPrintPreview();
		}

		private async void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!CanDelete()) return;

			try
			{
				string id = gvLocations.GetFocusedRowCellValue("_id").ToString();
				string name = gvLocations.GetFocusedRowCellValue("LocationName").ToString();

				if (!string.IsNullOrEmpty(id))
				{
					if (XtraMessageBox.Show($"Are you sure you want to delete Record: `{name}`?",
							"Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
							MessageBoxDefaultButton.Button2) == DialogResult.Yes)
					{
						_locationModel = gvLocations.GetFocusedRow() as LocationModel;
						if (_locationModel == null)
						{
							return;
						}
						_locationModel.Deleted = true;

						//delete the record
						await _locationRepository.DeleteLocationAsync(_locationModel._id);
						RcvUpdatedLocationAsync(_locationModel, EventArgs.Empty);
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
				LayoutsStyle.ResetLayoutGrid(gvLocations, CurrentUser.UserName, CurrentUser.Company);
			}
		}

		#endregion

		private async void RcvUpdatedLocationAsync(object sender, EventArgs e)
		{
			if (sender == null) return;
			_locationModel = sender as LocationModel;

			if (_locationModel != null && (_locationModel.LastModifiedDate == null || _locationModel.Deleted))
			{
				await InitializeBindings();
				WireUpBindings();
			}
			else
			{
				gvLocations.UpdateCurrentRow();
			}
		}

		private void gvLocations_DoubleClick(object sender, EventArgs e)
		{
			if (!_locations.Any()) return;

			try
			{
				string currentRowId = gvLocations.GetFocusedRowCellValue("_id").ToString();
				if (string.IsNullOrEmpty(currentRowId)) return;

				_locationModel = _locations.SingleOrDefault(x => x._id == currentRowId);
				if (_locationModel == null) return;

				ShowLocationEditor(_locationModel);
			}
			catch (Exception exception)
			{
				XtraMessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private bool CanDelete()
		{
			LocationModel dataBoundItem = gvLocations.GetFocusedRow() as LocationModel;

			if (gvLocations == null || gvLocations.SelectedRowsCount == 0) return false;
			if (gvLocations.SelectedRowsCount > 1)
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

		private void ShowLocationEditor(LocationModel model)
		{
			if (_locationEditForm == null || _locationEditForm.IsDisposed)
			{
				_locationEditForm = new LocationEditForm(model);
				_locationEditForm.SendUpdatedLocation += RcvUpdatedLocationAsync;
				_locationEditForm.FormClosed += LocationEditForm_FormClosed;
				_locationEditForm.Show(this);
				return;
			}

			if (_locationEditForm.WindowState == FormWindowState.Minimized)
				_locationEditForm.WindowState = FormWindowState.Normal;

			_locationEditForm.Activate();
			_locationEditForm.BringToFront();
		}

		private void LocationEditForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			var form = sender as LocationEditForm;
			if (form != null)
			{
				form.SendUpdatedLocation -= RcvUpdatedLocationAsync;
				form.FormClosed -= LocationEditForm_FormClosed;
			}
			if (ReferenceEquals(_locationEditForm, sender))
				_locationEditForm = null;
		}


		private void gvLocations_RowCellStyle(object sender, RowCellStyleEventArgs e)
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