using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using Spectrum.DataLayers.Common.Currencies;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Common.Currencies;
using Spectrum.Models.Users;
using Spectrum.Utilities;
using Spectrum.Utilities.Interfaces;
using Spectrum.Utilities.Layout;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.Common.Currencies
{
	public partial class CurrenciesListForm : RibbonForm, IFormWithRibbon
	{
		private bool _resetMenu;
		private CurrencyEditForm _currencyEditForm;

		private CurrencyModel _currencyModel = new CurrencyModel();
		private IList<CurrencyModel> _currencies = new List<CurrencyModel>();

		private CurrencyExchangeModel _currencyExchangeModel = new CurrencyExchangeModel();
		private BindingList<CurrencyExchangeModel> _currenciesExchange = new BindingList<CurrencyExchangeModel>();

		private readonly CurrencyRepository _currencyRepository = new CurrencyRepository(DatabaseFactory.ProfilePrimary);
		private readonly CurrencyExchangeRepository _currencyExchangeRepository = new CurrencyExchangeRepository(DatabaseFactory.ProfilePrimary);
		private readonly LogInfoRepository _logInfoRepository = new LogInfoRepository();

        private DXMenuItem[] _menuItems;

        //Init permissionvariables
        private bool _canAdd = true;
		private bool _canEdit = true;
		private bool _canDelete = true;
		private bool _canPrint = true;
		private bool _isAdmin = true;
		private bool _isProtected = true;

		#region Implementation of IFormWithRibbon

		public RibbonControl MainRibbon => rcCurrenciesList;
		public RibbonPage DefaultPage => rpCurrenciesList;


		#endregion

		public CurrenciesListForm()
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
			gvCurrencies.DoubleClick += gvCurrencies_DoubleClick;
			gvCurrencies.RowCellStyle += gvCurrencies_RowCellStyle;

			StartLoading();
		}

		private async void StartLoading()
		{
			await InitializeBindings();
			WireUpBindings();
			ApplyDefaults();
			ApplyPermissions();
            SetGridPermissions();
            InitializeMenuItems();
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

				_currencies = await _currencyRepository.GetCurrenciesAsync();
				_currenciesExchange = new BindingList<CurrencyExchangeModel>(await _currencyExchangeRepository.GetCurrenciesExchangeAsync());
			}
			catch (Exception ex)
			{
				XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void WireUpBindings()
		{
			gcCurrencies.DataSource = null;
			gcCurrencies.DataSource = _currencies;

			bsExchangeCurrencies.DataSource = null;
			bsExchangeCurrencies.DataSource = _currenciesExchange;

			repCurrencies.DataSource = null;
			repCurrencies.DataSource = _currencies;
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
			btnSave.Enabled = _isAdmin || _canAdd || _canEdit || _canDelete;
		}

		#region Button Events

		private void btnNew_ItemClick(object sender, ItemClickEventArgs e)
		{
			ShowCurrencyEditor(new CurrencyModel());
		}

		private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!_currencies.Any()) return;

			try
			{
				string currentRowId = gvCurrencies.GetFocusedRowCellValue("_id").ToString();
				if (string.IsNullOrEmpty(currentRowId)) return;

				_currencyModel = _currencies.SingleOrDefault(x => x._id == currentRowId);
				if (_currencyModel == null) return;

				ShowCurrencyEditor(_currencyModel);
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
			gcCurrencies.ShowRibbonPrintPreview();
		}

		private async void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!CanDelete()) return;

			try
			{
				string id = gvCurrencies.GetFocusedRowCellValue("_id").ToString();
				string name = gvCurrencies.GetFocusedRowCellValue("CurrencyName").ToString();

				if (!string.IsNullOrEmpty(id))
				{
					if (XtraMessageBox.Show($"Are you sure you want to delete Record: `{name}`?",
							"Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
							MessageBoxDefaultButton.Button2) == DialogResult.Yes)
					{
						_currencyModel = gvCurrencies.GetFocusedRow() as CurrencyModel;
						if (_currencyModel == null)
						{
							return;
						}
						_currencyModel.Deleted = true;

						//delete the record
						await _currencyRepository.DeleteCurrencyAsync(_currencyModel._id);
						RcvUpdatedCurrencyAsync(_currencyModel, EventArgs.Empty);
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
				LayoutsStyle.ResetLayoutGrid(gvCurrencies, CurrentUser.UserName, CurrentUser.Company);
			}
		}

		#endregion

		private async void RcvUpdatedCurrencyAsync(object sender, EventArgs e)
		{
			if (sender == null) return;
			_currencyModel = sender as CurrencyModel;

			if (_currencyModel != null && (_currencyModel.LastModifiedDate == null || _currencyModel.Deleted))
			{
				await InitializeBindings();
				WireUpBindings();
			}
			else
			{
				gvCurrencies.UpdateCurrentRow();
			}
		}

		private void gvCurrencies_DoubleClick(object sender, EventArgs e)
		{
			if (!_currencies.Any()) return;

			try
			{
				string currentRowId = gvCurrencies.GetFocusedRowCellValue("_id").ToString();
				if (string.IsNullOrEmpty(currentRowId)) return;

				_currencyModel = _currencies.SingleOrDefault(x => x._id == currentRowId);
				if (_currencyModel == null) return;

				ShowCurrencyEditor(_currencyModel);
			}
			catch (Exception exception)
			{
				XtraMessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private bool CanDelete()
		{
			CurrencyModel dataBoundItem = gvCurrencies.GetFocusedRow() as CurrencyModel;

			if (gvCurrencies == null || gvCurrencies.SelectedRowsCount == 0) return false;
			if (gvCurrencies.SelectedRowsCount > 1)
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

		private void ShowCurrencyEditor(CurrencyModel model)
		{
			if (_currencyEditForm == null || _currencyEditForm.IsDisposed)
			{
				_currencyEditForm = new CurrencyEditForm(model);
				_currencyEditForm.SendUpdatedCurrency += RcvUpdatedCurrencyAsync;
				_currencyEditForm.FormClosed += CurrencyEditForm_FormClosed;
				_currencyEditForm.Show(this);
				return;
			}

			if (_currencyEditForm.WindowState == FormWindowState.Minimized)
				_currencyEditForm.WindowState = FormWindowState.Normal;

			_currencyEditForm.Activate();
			_currencyEditForm.BringToFront();
		}

		private void CurrencyEditForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			var form = sender as CurrencyEditForm;
			if (form != null)
			{
				form.SendUpdatedCurrency -= RcvUpdatedCurrencyAsync;
				form.FormClosed -= CurrencyEditForm_FormClosed;
			}
			if (ReferenceEquals(_currencyEditForm, sender))
				_currencyEditForm = null;
		}

		private void gvCurrencies_RowCellStyle(object sender, RowCellStyleEventArgs e)
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

        private void SetGridPermissions()
        {
            ConfigureGridRowPermissions(gvExchangeCurrencies, _canAdd, _canDelete);

            // Selling grid always allows add/delete per original logic
            gvExchangeCurrencies.OptionsView.NewItemRowPosition = NewItemRowPosition.Bottom;
            gvExchangeCurrencies.OptionsBehavior.AllowAddRows = DefaultBoolean.True;
            gvExchangeCurrencies.OptionsBehavior.AllowDeleteRows = DefaultBoolean.True;
        }

        private void ConfigureGridRowPermissions(GridView view, bool canAdd, bool canDelete)
        {
            view.OptionsView.NewItemRowPosition = canAdd ? NewItemRowPosition.Bottom : NewItemRowPosition.None;
            view.OptionsBehavior.AllowAddRows = canAdd ? DefaultBoolean.True : DefaultBoolean.False;
            view.OptionsBehavior.AllowDeleteRows = canDelete ? DefaultBoolean.True : DefaultBoolean.False;
        }

        private void InitializeMenuItems()
        {
            _menuItems = new[]
            {
                new DXMenuItem("Edit", ItemEdit_Click),
                new DXMenuItem("Delete", ItemDelete_Click)
            };
        }

        private void ItemEdit_Click(object sender, EventArgs e)
        {
            gvExchangeCurrencies.ShowEditor();
        }

        private void ItemDelete_Click(object sender, EventArgs e)
        {
            if (!ConfirmAction("Delete row?", "Confirmation")) return;
            
			gvExchangeCurrencies.SetRowCellValue(gvExchangeCurrencies.FocusedRowHandle, "Deleted", true);

        }

        #region UI Helper Methods

        private void ShowError(string message, string title = "Error")
        {
            XtraMessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private bool ConfirmAction(string message, string title)
        {
            return XtraMessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) == DialogResult.Yes;
        }

        #endregion

        private void gvExchangeCurrencies_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            var view = sender as GridView;
            InitializeNewExchangeRow(view, e.RowHandle);
            
        }

        private void InitializeNewExchangeRow(GridView view, int rowHandle)
        {
            view.SetRowCellValue(rowHandle, view.Columns["ExchangeDate"], DateTime.Now);
        }

        private void btnExchangeCurrencies_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

		private async void btnSave_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (gvExchangeCurrencies.FocusedRowHandle < 0) return;

			try
			{
				gvExchangeCurrencies.CloseEditor();
				gvExchangeCurrencies.UpdateCurrentRow();

				_currencyExchangeModel = gvExchangeCurrencies.GetFocusedRow() as CurrencyExchangeModel;
				if (_currencyExchangeModel == null) return;

				if (_currencyExchangeModel.Deleted)
				{
					if (!string.IsNullOrWhiteSpace(_currencyExchangeModel._id))
					{
						await _currencyExchangeRepository.DeleteCurrencyExchangeAsync(_currencyExchangeModel._id);
					}
				}
				else
				{
					if (!ValidateFocusedExchangeCurrency(_currencyExchangeModel)) return;

					if (string.IsNullOrWhiteSpace(_currencyExchangeModel._id))
					{
						_logInfoRepository.CreateLogInfo(_currencyExchangeModel);
						await _currencyExchangeRepository.AddNewCurrencyExchangeAsync(_currencyExchangeModel);
					}
					else
					{
						_logInfoRepository.UpdateLogInfo(_currencyExchangeModel);
						await _currencyExchangeRepository.UpdateCurrencyExchangeAsync(_currencyExchangeModel);
					}
				}

				await InitializeBindings();
				WireUpBindings();
			}
			catch (Exception exception)
			{
				ShowError(exception.Message, @"Error Saving Exchange Currency");
			}
		}

		private bool ValidateFocusedExchangeCurrency(CurrencyExchangeModel model)
		{
			if (model == null) return false;

			if (string.IsNullOrWhiteSpace(model.Currency))
			{
				ShowError("Currency cannot be empty.", "Validation Error");
				gvExchangeCurrencies.FocusedColumn = colCurrency;
				gvExchangeCurrencies.ShowEditor();
				return false;
			}

			if (model.Rate <= 0)
			{
				ShowError("Rate must be greater than zero.", "Validation Error");
				gvExchangeCurrencies.FocusedColumn = colRate;
				gvExchangeCurrencies.ShowEditor();
				return false;
			}

			return true;
		}

        private void btnShowDetails_CheckedChanged(object sender, ItemClickEventArgs e)
        {

        }

        private void btnEdit_ItemClick_1(object sender, ItemClickEventArgs e)
        {

        }


        #region Grid Key Events

        private void gvExchangeCurrencies_KeyUp(object sender, KeyEventArgs e)
        {
            HandleDeleteKeyPress(sender as GridView, e);
        }

        private void HandleDeleteKeyPress(GridView view, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Delete) return;
            if (!ConfirmAction("Delete row?", "Confirmation")) return;

            view?.SetRowCellValue(view.FocusedRowHandle, "Deleted", true);
        }

        #endregion
    }
}