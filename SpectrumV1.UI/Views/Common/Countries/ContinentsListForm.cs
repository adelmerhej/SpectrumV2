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
	public partial class ContinentsListForm : RibbonForm, IFormWithRibbon
	{
		private bool _resetMenu;
		private ContinentEditForm _continentEditForm;

		private ContinentModel _continentModel = new ContinentModel();
		private IList<ContinentModel> _continents = new List<ContinentModel>();

		private readonly ContinentRepository _continentRepository = new ContinentRepository(DatabaseFactory.ProfilePrimary);

		//Init permissionvariables
		private bool _canAdd = true;
		private bool _canEdit = true;
		private bool _canDelete = true;
		private bool _canPrint = true;
		private bool _isAdmin = true;
		private bool _isProtected = true;

		#region Implementation of IFormWithRibbon

		public RibbonControl MainRibbon => rcContinentsList;
		public RibbonPage DefaultPage => rpContinentsList;


		#endregion

		public ContinentsListForm()
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
			gvContinents.DoubleClick += gvContinents_DoubleClick;
			gvContinents.RowCellStyle += gvContinents_RowCellStyle;

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

				_continents = await _continentRepository.GetContinentsAsync();
			}
			catch (Exception ex)
			{
				XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void WireUpBindings()
		{
			gcContinents.DataSource = null;
			gcContinents.DataSource = _continents;
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
			ShowContinentEditor(new ContinentModel());
		}

		private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!_continents.Any()) return;

			try
			{
				string currentRowId = gvContinents.GetFocusedRowCellValue("_id").ToString();
				if (string.IsNullOrEmpty(currentRowId)) return;

				_continentModel = _continents.SingleOrDefault(x => x._id == currentRowId);
				if (_continentModel == null) return;

				ShowContinentEditor(_continentModel);
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
			gcContinents.ShowRibbonPrintPreview();
		}

		private async void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!CanDelete()) return;

			try
			{
				string id = gvContinents.GetFocusedRowCellValue("_id").ToString();
				string name = gvContinents.GetFocusedRowCellValue("ContinentName").ToString();

				if (!string.IsNullOrEmpty(id))
				{
					if (XtraMessageBox.Show($"Are you sure you want to delete Record: `{name}`?",
							"Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
							MessageBoxDefaultButton.Button2) == DialogResult.Yes)
					{
						_continentModel = gvContinents.GetFocusedRow() as ContinentModel;
						if (_continentModel == null)
						{
							return;
						}
						_continentModel.Deleted = true;

						//delete the record
						await _continentRepository.DeleteContinentAsync(_continentModel._id);
						RcvUpdatedContinentAsync(_continentModel, EventArgs.Empty);
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
				LayoutsStyle.ResetLayoutGrid(gvContinents, CurrentUser.UserName, CurrentUser.Company);
			}
		}

		#endregion

		private void gvContinents_DoubleClick(object sender, EventArgs e)
		{
			if (!_continents.Any()) return;

			try
			{
				string currentRowId = gvContinents.GetFocusedRowCellValue("_id").ToString();
				if (string.IsNullOrEmpty(currentRowId)) return;

				_continentModel = _continents.SingleOrDefault(x => x._id == currentRowId);
				if (_continentModel == null) return;

				ShowContinentEditor(_continentModel);
			}
			catch (Exception exception)
			{
				XtraMessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private async void RcvUpdatedContinentAsync(object sender, EventArgs e)
		{
			if (sender == null) return;
			_continentModel = sender as ContinentModel;
			if (_continentModel == null) return;

			if (_continentModel.Deleted || _continentModel.LastModifiedDate == null)
			{
				await InitializeBindings();
				WireUpBindings();
			}
			else
			{
				gcContinents.RefreshDataSource();
				gvContinents.RefreshRow(gvContinents.FocusedRowHandle);
				gvContinents.UpdateCurrentRow();
			}
		}

		private bool CanDelete()
		{
			ContinentModel dataBoundItem = gvContinents.GetFocusedRow() as ContinentModel;

			if (gvContinents == null || gvContinents.SelectedRowsCount == 0) return false;
			if (gvContinents.SelectedRowsCount > 1)
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

		private void ShowContinentEditor(ContinentModel model)
		{
			if (_continentEditForm == null || _continentEditForm.IsDisposed)
			{
				_continentEditForm = new ContinentEditForm(model);
				_continentEditForm.SendUpdatedContinent += RcvUpdatedContinentAsync;
				_continentEditForm.FormClosed += ContinentEditForm_FormClosed;
				_continentEditForm.Show(this);
				return;
			}

			if (_continentEditForm.WindowState == FormWindowState.Minimized)
				_continentEditForm.WindowState = FormWindowState.Normal;

			_continentEditForm.Activate();
			_continentEditForm.BringToFront();
		}

		private void ContinentEditForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			var form = sender as ContinentEditForm;
			if (form != null)
			{
				form.SendUpdatedContinent -= RcvUpdatedContinentAsync;
				form.FormClosed -= ContinentEditForm_FormClosed;
			}
			if (ReferenceEquals(_continentEditForm, sender))
				_continentEditForm = null;
		}

		private void gvContinents_RowCellStyle(object sender, RowCellStyleEventArgs e)
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