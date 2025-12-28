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
	public partial class CitiesListForm : RibbonForm, IFormWithRibbon
	{
		private bool _resetMenu;
		private CityEditForm _cityEditForm;

		private CityModel _cityModel = new CityModel();
		private IList<CityModel> _cities = new List<CityModel>();

		private readonly CityRepository _cityRepository = new CityRepository(DatabaseFactory.ProfilePrimary);

		//Init permissionvariables
		private bool _canAdd = true;
		private bool _canEdit = true;
		private bool _canDelete = true;
		private bool _canPrint = true;
		private bool _isAdmin = true;
		private bool _isProtected = true;

		#region Implementation of IFormWithRibbon

		public RibbonControl MainRibbon => rcCitiesList;
		public RibbonPage DefaultPage => rpCitiesList;


		#endregion

		public CitiesListForm()
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
			gvCities.DoubleClick += gvCities_DoubleClick;
			gvCities.RowCellStyle += gvCities_RowCellStyle;

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

				_cities = await _cityRepository.GetCitiesAsync();
			}
			catch (Exception ex)
			{
				XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void WireUpBindings()
		{
			gcCities.DataSource = null;
			gcCities.DataSource = _cities;
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
			ShowCityEditor(new CityModel());
		}

		private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!_cities.Any()) return;

			try
			{
				string currentRowId = gvCities.GetFocusedRowCellValue("_id").ToString();
				if (string.IsNullOrEmpty(currentRowId)) return;

				_cityModel = _cities.SingleOrDefault(x => x._id == currentRowId);
				if (_cityModel == null) return;

				ShowCityEditor(_cityModel);
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
			gcCities.ShowRibbonPrintPreview();
		}

		private async void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!CanDelete()) return;

			try
			{
				string id = gvCities.GetFocusedRowCellValue("_id").ToString();
				string name = gvCities.GetFocusedRowCellValue("CityName").ToString();

				if (!string.IsNullOrEmpty(id))
				{
					if (XtraMessageBox.Show($"Are you sure you want to delete Record: `{name}`?",
							"Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
							MessageBoxDefaultButton.Button2) == DialogResult.Yes)
					{
						_cityModel = gvCities.GetFocusedRow() as CityModel;
						if (_cityModel == null)
						{
							return;
						}
						_cityModel.Deleted = true;

						//delete the record
						await _cityRepository.DeleteCityAsync(_cityModel._id);
						RcvUpdatedCityAsync(_cityModel, EventArgs.Empty);
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
				LayoutsStyle.ResetLayoutGrid(gvCities, CurrentUser.UserName, CurrentUser.Company);
			}
		}

		#endregion

		private async void RcvUpdatedCityAsync(object sender, EventArgs e)
		{
			if (sender == null) return;
			_cityModel = sender as CityModel;

			if (_cityModel != null && (_cityModel.LastModifiedDate == null || _cityModel.Deleted))
			{
				await InitializeBindings();
				WireUpBindings();
			}
			else
			{
				gvCities.UpdateCurrentRow();
			}
		}

		private void gvCities_DoubleClick(object sender, EventArgs e)
		{
			if (!_cities.Any()) return;

			try
			{
				string currentRowId = gvCities.GetFocusedRowCellValue("_id").ToString();
				if (string.IsNullOrEmpty(currentRowId)) return;

				_cityModel = _cities.SingleOrDefault(x => x._id == currentRowId);
				if (_cityModel == null) return;

				ShowCityEditor(_cityModel);
			}
			catch (Exception exception)
			{
				XtraMessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private bool CanDelete()
		{
			CityModel dataBoundItem = gvCities.GetFocusedRow() as CityModel;

			if (gvCities == null || gvCities.SelectedRowsCount == 0) return false;
			if (gvCities.SelectedRowsCount > 1)
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

		private void ShowCityEditor(CityModel model)
		{
			if (_cityEditForm == null || _cityEditForm.IsDisposed)
			{
				_cityEditForm = new CityEditForm(model);
				_cityEditForm.SendUpdatedCity += RcvUpdatedCityAsync;
				_cityEditForm.FormClosed += CityEditForm_FormClosed;
				_cityEditForm.Show(this);
				return;
			}

			if (_cityEditForm.WindowState == FormWindowState.Minimized)
				_cityEditForm.WindowState = FormWindowState.Normal;

			_cityEditForm.Activate();
			_cityEditForm.BringToFront();
		}

		private void CityEditForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			var form = sender as CityEditForm;
			if (form != null)
			{
				form.SendUpdatedCity -= RcvUpdatedCityAsync;
				form.FormClosed -= CityEditForm_FormClosed;
			}
			if (ReferenceEquals(_cityEditForm, sender))
				_cityEditForm = null;
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