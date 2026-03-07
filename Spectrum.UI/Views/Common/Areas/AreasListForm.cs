using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using Spectrum.DataLayers.Common.Areas;
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
	public partial class AreasListForm : RibbonForm, IFormWithRibbon
	{
		private bool _resetMenu;
		private AreaEditForm _areaEditForm;

		private AreaModel _areaModel = new AreaModel();
		private IList<AreaModel> _areas = new List<AreaModel>();

		private readonly AreaRepository _areaRepository = new AreaRepository(DatabaseFactory.ProfilePrimary);

		//Init permissionvariables
		private bool _canAdd = true;
		private bool _canEdit = true;
		private bool _canDelete = true;
		private bool _canPrint = true;
		private bool _isAdmin = true;
		private bool _isProtected = true;

		#region Implementation of IFormWithRibbon

		public RibbonControl MainRibbon => rcAreas;
		public RibbonPage DefaultPage => rpAreas;


		#endregion

		public AreasListForm()
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
			FormClosing += AreasListForm_FormClosing;
			gvAreas.DoubleClick += gvAreas_DoubleClick;
			gvAreas.RowCellStyle += gvAreas_RowCellStyle;

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

				_areas = await _areaRepository.GetAreasAsync();
			}
			catch (Exception ex)
			{
				XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void WireUpBindings()
		{
			gcAreas.DataSource = null;
			gcAreas.DataSource = _areas;
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

		private void AreasListForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!_resetMenu)
			{
				LayoutsStyle.SaveLayoutGrid(gvAreas, CurrentUser.UserName, CurrentUser.Company);
			}
		}

		#region Buttons Events

		private void btnNew_ItemClick(object sender, ItemClickEventArgs e)
		{
			ShowAreaEditor(new AreaModel());
		}

		private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!_areas.Any()) return;

			try
			{
				string currentRowId = gvAreas.GetFocusedRowCellValue("_id").ToString();
				if (string.IsNullOrEmpty(currentRowId)) return;

				_areaModel = _areas.SingleOrDefault(x => x._id == currentRowId);
				if (_areaModel == null) return;

				ShowAreaEditor(_areaModel);
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
			gcAreas.ShowRibbonPrintPreview();
		}

		private async void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!CanDelete()) return;

			try
			{
				string id = gvAreas.GetFocusedRowCellValue("_id").ToString();
				string name = gvAreas.GetFocusedRowCellValue("AreaName").ToString();

				if (!string.IsNullOrEmpty(id))
				{
					if (XtraMessageBox.Show($"Are you sure you want to delete Record: `{name}`?",
							"Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
							MessageBoxDefaultButton.Button2) == DialogResult.Yes)
					{
						_areaModel = gvAreas.GetFocusedRow() as AreaModel;
						if (_areaModel == null)
						{
							return;
						}
						_areaModel.Deleted = true;

						//delete the record
						await _areaRepository.DeleteAreaAsync(_areaModel._id);
						RcvUpdatedAreaAsync(_areaModel, EventArgs.Empty);
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
				LayoutsStyle.ResetLayoutGrid(gvAreas, CurrentUser.UserName, CurrentUser.Company);
			}
		}

		#endregion


		private async void RcvUpdatedAreaAsync(object sender, EventArgs e)
		{
			if (sender == null) return;
			_areaModel = sender as AreaModel;

			if (_areaModel != null && (_areaModel.LastModifiedDate == null || _areaModel.Deleted))
			{
				await InitializeBindings();
				WireUpBindings();
			}
			else
			{
				gvAreas.UpdateCurrentRow();
			}
		}

		private void gvAreas_DoubleClick(object sender, EventArgs e)
		{
			if (!_areas.Any()) return;

			try
			{
				string currentRowId = gvAreas.GetFocusedRowCellValue("_id").ToString();
				if (string.IsNullOrEmpty(currentRowId)) return;

				_areaModel = _areas.SingleOrDefault(x => x._id == currentRowId);
				if (_areaModel == null) return;

				ShowAreaEditor(_areaModel);
			}
			catch (Exception exception)
			{
				XtraMessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private bool CanDelete()
		{
			AreaModel dataBoundItem = gvAreas.GetFocusedRow() as AreaModel;

			if (gvAreas == null || gvAreas.SelectedRowsCount == 0) return false;
			if (gvAreas.SelectedRowsCount > 1)
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

		private void ShowAreaEditor(AreaModel model)
		{
			if (_areaEditForm == null || _areaEditForm.IsDisposed)
			{
				_areaEditForm = new AreaEditForm(model);
				_areaEditForm.SendUpdatedArea += RcvUpdatedAreaAsync;
				_areaEditForm.FormClosed += AreaEditForm_FormClosed;
				_areaEditForm.Show(this);
				return;
			}

			if (_areaEditForm.WindowState == FormWindowState.Minimized)
				_areaEditForm.WindowState = FormWindowState.Normal;

			_areaEditForm.Activate();
			_areaEditForm.BringToFront();
		}

		private void AreaEditForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			var form = sender as AreaEditForm;
			if (form != null)
			{
				//form.SendUpdatedArea -= RcvUpdatedAreaAsync;
				form.FormClosed -= AreaEditForm_FormClosed;
			}
			if (ReferenceEquals(_areaEditForm, sender))
				_areaEditForm = null;
		}


		private void gvAreas_RowCellStyle(object sender, RowCellStyleEventArgs e)
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