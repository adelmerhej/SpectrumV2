using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using Spectrum.DataLayers.Common.Countries;
using Spectrum.DataLayers.Common.Countries.Interfaces;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Common.Countries;
using Spectrum.Models.Users;
using Spectrum.Reports.Common.Countries;
using Spectrum.Utilities;
using Spectrum.Utilities.Interfaces;
using Spectrum.Utilities.Layout;
using Spectrum.Views.Reports;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.Common.Countries
{
	public partial class DistrictsListForm : RibbonForm, IFormWithRibbon
	{
		private bool _resetMenu;
		private DistrictEditForm _districtEditForm;

		private DistrictModel _districtModel = new DistrictModel();
		private IList<DistrictModel> _districts = new List<DistrictModel>();
        private List<DistrictModel> _dataReportModels = new List<DistrictModel>();

        private readonly DistrictRepository _districtRepository = new DistrictRepository(DatabaseFactory.ProfilePrimary);

		//Init permissionvariables
		private bool _canAdd = true;
		private bool _canEdit = true;
		private bool _canDelete = true;
		private bool _canPrint = true;
		private bool _isAdmin = true;
		private bool _isProtected = true;

		#region Implementation of IFormWithRibbon

		public RibbonControl MainRibbon => rcDistricts;
		public RibbonPage DefaultPage => rpDistricts;


		#endregion

		public DistrictsListForm()
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

				_districts = await _districtRepository.GetDistrictsAsync();
			}
			catch (Exception ex)
			{
				XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void WireUpBindings()
		{
			gcDistricts.DataSource = null;
			gcDistricts.DataSource = _districts;
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


		#region Buttons Events

		private void btnNew_ItemClick(object sender, ItemClickEventArgs e)
		{
			ShowDistrictEditor(new DistrictModel());
		}

		private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!_districts.Any()) return;

			try
			{
				string currentRowId = gvDistricts.GetFocusedRowCellValue("_id").ToString();
				if (string.IsNullOrEmpty(currentRowId)) return;

				_districtModel = _districts.SingleOrDefault(x => x._id == currentRowId);
				if (_districtModel == null) return;

				ShowDistrictEditor(_districtModel);
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

		private async void btnPrint_ItemClick(object sender, ItemClickEventArgs e)
		{
            try
            {
                _dataReportModels = await _districtRepository.GetDistrictsAsync();

                var previewForm = new DocumentViewerForm();
                var report = new DistrictsListReport();

                report.DataSource = _dataReportModels;

                previewForm.Viewer.DocumentSource = report;

                previewForm.ShowDialog();
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, "Report Data Error!", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

		private async void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!CanDelete()) return;

			try
			{
				string id = gvDistricts.GetFocusedRowCellValue("_id").ToString();
				string name = gvDistricts.GetFocusedRowCellValue("DistrictName").ToString();

				if (!string.IsNullOrEmpty(id))
				{
					if (XtraMessageBox.Show($"Are you sure you want to delete Record: `{name}`?",
							"Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
							MessageBoxDefaultButton.Button2) == DialogResult.Yes)
					{
						_districtModel = gvDistricts.GetFocusedRow() as DistrictModel;
						if (_districtModel == null)
						{
							return;
						}
						_districtModel.Deleted = true;

						//delete the record
						await _districtRepository.DeleteDistrictAsync(_districtModel._id);
						RcvUpdatedDistrictAsync(_districtModel, EventArgs.Empty);
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

		#endregion


		private async void RcvUpdatedDistrictAsync(object sender, EventArgs e)
		{
			if (sender == null) return;
			_districtModel = sender as DistrictModel;

			if (_districtModel != null && (_districtModel.LastModifiedDate == null || _districtModel.Deleted))
			{
				await InitializeBindings();
				WireUpBindings();
			}
			else
			{
				gvDistricts.UpdateCurrentRow();
			}
		}

		private void gvDistricts_DoubleClick(object sender, EventArgs e)
		{
			if (!_districts.Any()) return;

			try
			{
				string currentRowId = gvDistricts.GetFocusedRowCellValue("_id").ToString();
				if (string.IsNullOrEmpty(currentRowId)) return;

				_districtModel = _districts.SingleOrDefault(x => x._id == currentRowId);
				if (_districtModel == null) return;

				ShowDistrictEditor(_districtModel);
			}
			catch (Exception exception)
			{
				XtraMessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btnResetGridStyle_ItemClick(object sender, ItemClickEventArgs e)
		{
			// reset settings if needed
			if (XtraMessageBox.Show("This will reset Grid layout next login, to its default settings.\nAre you sure you want to continue?", "Reset Menu...",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) ==
				DialogResult.Yes)
			{
				_resetMenu = true;
				LayoutsStyle.ResetLayoutGrid(gvDistricts, CurrentUser.UserName, CurrentUser.Company);
			}
		}

		private bool CanDelete()
		{
			DistrictModel dataBoundItem = gvDistricts.GetFocusedRow() as DistrictModel;

			if (gvDistricts == null || gvDistricts.SelectedRowsCount == 0) return false;
			if (gvDistricts.SelectedRowsCount > 1)
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

		private void ShowDistrictEditor(DistrictModel model)
		{
			if (_districtEditForm == null || _districtEditForm.IsDisposed)
			{
				_districtEditForm = new DistrictEditForm(model);
				_districtEditForm.SendUpdatedDistrict += RcvUpdatedDistrictAsync;
				_districtEditForm.FormClosed += DistrictEditForm_FormClosed;
				_districtEditForm.Show(this);
				return;
			}

			if (_districtEditForm.WindowState == FormWindowState.Minimized)
				_districtEditForm.WindowState = FormWindowState.Normal;

			_districtEditForm.Activate();
			_districtEditForm.BringToFront();
		}

		private void DistrictEditForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			var form = sender as DistrictEditForm;
			if (form != null)
			{
				form.SendUpdatedDistrict -= RcvUpdatedDistrictAsync;
				form.FormClosed -= DistrictEditForm_FormClosed;
			}
			if (ReferenceEquals(_districtEditForm, sender))
				_districtEditForm = null;
		}

		private void gvDistricts_RowCellStyle(object sender, RowCellStyleEventArgs e)
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