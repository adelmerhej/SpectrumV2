using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using SpectrumV1.DataLayers.Common.Countries;
using SpectrumV1.DataLayers.DataAccess;
using SpectrumV1.Models.Common.Countries;
using SpectrumV1.Models.Users;
using SpectrumV1.Utilities;
using SpectrumV1.Utilities.Interfaces;
using SpectrumV1.Utilities.Layout;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpectrumV1.Views.Common.Countries
{
	public partial class ProvincesListForm : RibbonForm, IFormWithRibbon
	{
		private bool _resetMenu;
		private ProvinceEditForm _provinceEditForm;

		private ProvinceModel _provinceModel = new ProvinceModel();
		private IList<ProvinceModel> _provinces = new List<ProvinceModel>();

		private readonly ProvinceRepository _provinceRepository = new ProvinceRepository(DatabaseFactory.ProfilePrimary);

		//Init permissionvariables
		private bool _canAdd = true;
		private bool _canEdit = true;
		private bool _canDelete = true;
		private bool _canPrint = true;
		private bool _isAdmin = true;
		private bool _isProtected = true;

		#region Implementation of IFormWithRibbon

		public RibbonControl MainRibbon => rcProvinces;
		public RibbonPage DefaultPage => rpProvinces;


		#endregion

		public ProvincesListForm()
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
			gvProvinces.DoubleClick += gvProvinces_DoubleClick;
			gvProvinces.RowCellStyle += gvProvinces_RowCellStyle;

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

				_provinces = await _provinceRepository.GetProvincesAsync();
			}
			catch (Exception ex)
			{
				XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void WireUpBindings()
		{
			gcProvinces.DataSource = null;
			gcProvinces.DataSource = _provinces;
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
			ShowProvinceEditor(new ProvinceModel());
		}

		private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!_provinces.Any()) return;

			try
			{
				string currentRowId = gvProvinces.GetFocusedRowCellValue("_id").ToString();
				if (string.IsNullOrEmpty(currentRowId)) return;

				_provinceModel = _provinces.SingleOrDefault(x => x._id == currentRowId);
				if (_provinceModel == null) return;

				ShowProvinceEditor(_provinceModel);
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
			gcProvinces.ShowRibbonPrintPreview();
		}

		private async void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!CanDelete()) return;

			try
			{
				string id = gvProvinces.GetFocusedRowCellValue("_id").ToString();
				string name = gvProvinces.GetFocusedRowCellValue("ProvinceName").ToString();

				if (!string.IsNullOrEmpty(id))
				{
					if (XtraMessageBox.Show($"Are you sure you want to delete Record: `{name}`?",
							"Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
							MessageBoxDefaultButton.Button2) == DialogResult.Yes)
					{
						_provinceModel = gvProvinces.GetFocusedRow() as ProvinceModel;
						if (_provinceModel == null)
						{
							return;
						}
						_provinceModel.Deleted = true;

						//delete the record
						await _provinceRepository.DeleteProvinceAsync(_provinceModel._id);
						RcvUpdatedProvinceAsync(_provinceModel, EventArgs.Empty);
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
			// reset settings if needed
			if (XtraMessageBox.Show("This will reset Grid layout next login, to its default settings.\nAre you sure you want to continue?", "Reset Menu...",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) ==
				DialogResult.Yes)
			{
				_resetMenu = true;
				LayoutsStyle.ResetLayoutGrid(gvProvinces, CurrentUser.UserName, CurrentUser.Company);
			}
		}

		#endregion


		private async void RcvUpdatedProvinceAsync(object sender, EventArgs e)
		{
			if (sender == null) return;
			_provinceModel = sender as ProvinceModel;

			if (_provinceModel != null && (_provinceModel.LastModifiedDate == null || _provinceModel.Deleted))
			{
				await InitializeBindings();
				WireUpBindings();
			}
			else
			{
				gvProvinces.UpdateCurrentRow();
			}
		}

		private void gvProvinces_DoubleClick(object sender, EventArgs e)
		{
			if (!_provinces.Any()) return;

			try
			{
				string currentRowId = gvProvinces.GetFocusedRowCellValue("_id").ToString();
				if (string.IsNullOrEmpty(currentRowId)) return;

				_provinceModel = _provinces.SingleOrDefault(x => x._id == currentRowId);
				if (_provinceModel == null) return;

				ShowProvinceEditor(_provinceModel);
			}
			catch (Exception exception)
			{
				XtraMessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private bool CanDelete()
		{
			ProvinceModel dataBoundItem = gvProvinces.GetFocusedRow() as ProvinceModel;

			if (gvProvinces == null || gvProvinces.SelectedRowsCount == 0) return false;
			if (gvProvinces.SelectedRowsCount > 1)
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

		private void ShowProvinceEditor(ProvinceModel model)
		{
			if (_provinceEditForm == null || _provinceEditForm.IsDisposed)
			{
				_provinceEditForm = new ProvinceEditForm(model);
				_provinceEditForm.SendUpdatedProvince += RcvUpdatedProvinceAsync;
				_provinceEditForm.FormClosed += ProvinceEditForm_FormClosed;
				_provinceEditForm.Show(this);
				return;
			}

			if (_provinceEditForm.WindowState == FormWindowState.Minimized)
				_provinceEditForm.WindowState = FormWindowState.Normal;

			_provinceEditForm.Activate();
			_provinceEditForm.BringToFront();
		}

		private void ProvinceEditForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			var form = sender as ProvinceEditForm;
			if (form != null)
			{
				form.SendUpdatedProvince -= RcvUpdatedProvinceAsync;
				form.FormClosed -= ProvinceEditForm_FormClosed;
			}
			if (ReferenceEquals(_provinceEditForm, sender))
				_provinceEditForm = null;
		}

		private void gvProvinces_RowCellStyle(object sender, RowCellStyleEventArgs e)
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