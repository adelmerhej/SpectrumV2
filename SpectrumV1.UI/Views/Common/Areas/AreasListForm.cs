using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using SpectrumV1.DataLayers.Common.Areas;
using SpectrumV1.Models.Common.Areas;
using SpectrumV1.Models.Common.Countries;
using SpectrumV1.Utilities.Interfaces;
using SpectrumV1.Views.Common.Countries;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpectrumV1.Views.Common.Areas
{
	public partial class AreasListForm : RibbonForm, IFormWithRibbon
	{
		private AreaModel _areaModel = new AreaModel();
		private IList<AreaModel> _areas = new List<AreaModel>();

		private readonly AreaRepository _areaRepository = new AreaRepository();

		//Init permissionvariables
		private bool _canAdd = true;
		private bool _canEdit = true;
		private bool _canDelete = true;
		private bool _canPrint = true;
		private bool _isAdmin = true;
		private bool _isProtected = true;

		#region Implementation of IFormWithRibbon

		public RibbonControl MainRibbon => rcAreasList;
		public RibbonPage DefaultPage => rpAreasList;


		#endregion

		public AreasListForm()
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

		#region Buttons Event

		private void btnNew_ItemClick(object sender, ItemClickEventArgs e)
		{
			AreaEditForm frm = new AreaEditForm(new AreaModel());
			frm.SendUpdatedArea += RcvUpdatedAreaAsync;
			frm.ShowDialog();
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

				var areaForm = new AreaEditForm(_areaModel);
				areaForm.SendUpdatedArea += RcvUpdatedAreaAsync;
				areaForm.ShowDialog();
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
					if (XtraMessageBox.Show($"Are you sure you want to delete: `{name}`?",
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
						bool deleted = await _areaRepository.DeleteAreaAsync(id);
						if (!deleted) return;
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

		}

		private void gvAreas_RowCellStyle(object sender, RowCellStyleEventArgs e)
		{
			GridView view = sender as GridView;
			if (e.RowHandle >= 0)
			{
				bool isActive = view != null && (bool)view.GetRowCellValue(e.RowHandle, "Active");
				bool isDefault = view != null && (bool)view.GetRowCellValue(e.RowHandle, "IsDefault");
				if (isDefault)
				{
					e.Appearance.Font = new Font("Tahoma", 8, FontStyle.Bold);
				}
				if (!isActive)
				{
					e.Appearance.ForeColor = Color.Gray;
					e.Appearance.Font = new Font("Tahoma", 8, FontStyle.Italic);
				}
			}
		}

		#endregion

		private void gvAreas_DoubleClick(object sender, EventArgs e)
		{
			if (!_areas.Any()) return;

			try
			{
				string currentRowId = gvAreas.GetFocusedRowCellValue("_id").ToString();
				if (string.IsNullOrEmpty(currentRowId)) return;

				_areaModel = _areas.SingleOrDefault(x => x._id == currentRowId);
				if (_areaModel == null) return;

				var areaForm = new AreaEditForm(_areaModel);
				areaForm.SendUpdatedArea += RcvUpdatedAreaAsync;
				areaForm.ShowDialog();
			}
			catch (Exception exception)
			{
				XtraMessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private async void RcvUpdatedAreaAsync(object sender, EventArgs e)
		{
			if (sender == null) return;
			_areaModel = sender as AreaModel;
			if (_areaModel == null) return;

			if (_areaModel.Deleted || _areaModel.LastModifiedDate == null)
			{
				await InitializeBindings();
				WireUpBindings();
			}
			else
			{
				gcAreas.RefreshDataSource();
				gvAreas.RefreshRow(gvAreas.FocusedRowHandle);
				gvAreas.UpdateCurrentRow();
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
	}
}