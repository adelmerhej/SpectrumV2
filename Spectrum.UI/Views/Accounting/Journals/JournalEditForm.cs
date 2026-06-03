using DevExpress.Data;
using DevExpress.Utils.Menu;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using Spectrum.DataLayers.Accounting.Charts;
using Spectrum.DataLayers.Accounting.CostCenter;
using Spectrum.DataLayers.Accounting.FlowType;
using Spectrum.DataLayers.Accounting.Journals;
using Spectrum.DataLayers.Accounting.JournalType;
using Spectrum.DataLayers.Common.Currencies;
using Spectrum.DataLayers.Common.Currencies.Interfaces;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Accounting.Charts;
using Spectrum.Models.Accounting.CostCenter;
using Spectrum.Models.Accounting.FlowType;
using Spectrum.Models.Accounting.Journals;
using Spectrum.Models.Accounting.JournalType;
using Spectrum.Models.Common.Currencies;
using Spectrum.Models.Users;
using Spectrum.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.Accounting.Journals
{
    public partial class JournalEditForm : RibbonForm
    {
        private JournalModel _journalModel = new JournalModel();
        private JournalDetailModel _journalDetailModel = new JournalDetailModel();
        private IList<JournalDetailModel> _journalDetails = new List<JournalDetailModel>();

        private ChartModel _chartModel = new ChartModel();
        private IList<ChartModel> _charts = new List<ChartModel>();
        private IList<CurrencyModel> _currencies = new List<CurrencyModel>();

        private IList<JournalTypeModel> _journalTypes = new List<JournalTypeModel>();
        private IList<IdListModel> deletedList = new List<IdListModel>();

        private IList<CostCenterModel> _costCenters = new List<CostCenterModel>();
        private IList<FlowTypeModel> _flowTypes = new List<FlowTypeModel>();

        private readonly JournalRepository _journalRepository = new JournalRepository(DatabaseFactory.ProfilePrimary);
        private readonly JournalDetailRepository _journalDetailRepository = new JournalDetailRepository(DatabaseFactory.ProfilePrimary);

        private readonly ChartRepository _chartRepository = new ChartRepository(DatabaseFactory.ProfilePrimary);
        private readonly ChartDetailRepository _chartDetailRepository = new ChartDetailRepository(DatabaseFactory.ProfilePrimary);
        private readonly CurrencyRepository _currencyRepository = new CurrencyRepository(DatabaseFactory.ProfilePrimary);
        private readonly CurrencyExchangeRepository _currencyExchangeRepository = new CurrencyExchangeRepository(DatabaseFactory.ProfilePrimary);
        private readonly JournalTypeRepository _journalTypeRepository = new JournalTypeRepository(DatabaseFactory.ProfilePrimary);

        private readonly CostCenterRepository _costCenterRepository = new CostCenterRepository(DatabaseFactory.ProfilePrimary);
        private readonly FlowTypeRepository _flowTypeRepository = new FlowTypeRepository(DatabaseFactory.ProfilePrimary);

        private readonly LogInfoRepository _logInfoRepository = new LogInfoRepository();

        private string _defaultJournalType = "JV";
        private string _defaultCurrency = "USD";
        private decimal _defaultRate = 89500;
        private bool _isDeleted;

        private bool _canAdd = true;
        private bool _canEdit = true;
        private bool _canDelete = true;
        private bool _canPrint = true;
        private bool _isAdmin = true;

        private DXMenuItem[] _menuItems;

        public EventHandler SendUpdatedJournal;

        #region Constructor

        public JournalEditForm(JournalModel model)
        {
            InitializeComponent();

            _journalModel = model ?? new JournalModel();

            StartLoading();
        }

        #endregion

        #region Initialization

        private async void StartLoading()
        {
            try
            {
                await InitializeBindings();
                WireUpBindings();
                await ApplyDefaultsAsync();
                ApplyPermissions();
                InitializeMenuItems();
            }
            catch (Exception ex)
            {
                ShowError("Error during form initialization", ex);
            }
        }

        #region InitializeBindings Methods

        private async Task InitializeBindings()
        {
            try
            {
                var loadTasks = new[]
                {
                    LoadJournalTypesAsync(),
                    LoadCurrenciesAsync(),
                    LoadChartsAsync(),
                    LoadJournalDetailsAsync(),
                    LoadCostCentersAsync(),
                    LoadFlowTypesAsync()
                };

                await Task.WhenAll(loadTasks);
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading form data", ex);
            }
        }

        private async Task LoadJournalTypesAsync()
        {
            _journalTypes = await _journalTypeRepository.GetJournalTypesAsync();
        }

        private async Task LoadCurrenciesAsync()
        {
            _currencies = await _currencyRepository.GetCurrenciesAsync();
        }

        private async Task LoadChartsAsync()
        {
            _charts = await _chartRepository.GetChartsAsync();
        }

        private async Task LoadCostCentersAsync()
        {
            _costCenters = await _costCenterRepository.GetCostCentersAsync();
        }

        private async Task LoadFlowTypesAsync()
        {
            _flowTypes = await _flowTypeRepository.GetFlowTypesAsync();
        }

        private async Task LoadJournalDetailsAsync()
        {
            _journalDetails = string.IsNullOrWhiteSpace(_journalModel.JvNo)
                ? new List<JournalDetailModel>()
                : await _journalDetailRepository.GetJournalDetailsByJvNoAsync(_journalModel.JvNo);
        }

        #endregion


        private void WireUpBindings()
        {
            if (txtWorkingYear.DataBindings.Count == 0)
            {
                txtWorkingYear.DataBindings.Add("EditValue", bsJournal, nameof(JournalModel.WorkingYear), true, DataSourceUpdateMode.OnPropertyChanged);
            }

            bsJournal.DataSource = _journalModel;
            BindGridWithBindingSource(gcJournalDetails, bsJournalDetails, _journalDetails);

            cboJournalTypes.Properties.DataSource = null;
            cboJournalTypes.Properties.DataSource = _journalTypes;

            cboCurrency.Properties.DataSource = null;
            cboCurrency.Properties.DataSource = _currencies;

            repCharts.DataSource = null;
            repCharts.DataSource = _charts;

            repCurrencies.DataSource = null;
            repCurrencies.DataSource = _currencies;

            repCostCenter.DataSource = null;
            repCostCenter.DataSource = _costCenters;

            repFlowType.DataSource = null;
            repFlowType.DataSource = _flowTypes;

            //gvJournalDetails.InitNewRow -= gvJournalDetails_InitNewRow;
            //gvJournalDetails.InitNewRow += gvJournalDetails_InitNewRow;
        }

        private async Task ApplyDefaultsAsync()
        {
            var isNewJournalVoucher = string.IsNullOrWhiteSpace(_journalModel._id);
            ConfigureJournalDetailsGrid(isNewJournalVoucher);

            if (!isNewJournalVoucher)
            {
                return;
            }

            var today = DateTime.Today;
            _journalModel.JournalDate = today;
            _journalModel.Rate = _defaultRate;
            _journalModel.WorkingYear = today.Year;
            _journalModel.Reference = await _journalRepository.GetNextReferenceAsync();

            var journalType = _journalTypes.FirstOrDefault(x => string.Equals(x.Code, _defaultJournalType, StringComparison.OrdinalIgnoreCase));
            _journalModel.JournalType = journalType != null ? journalType.Code : _defaultJournalType;

            var currency = _currencies.FirstOrDefault(x => string.Equals(x.CurrencyCode, _defaultCurrency, StringComparison.OrdinalIgnoreCase));
            _journalModel.Currency = currency != null ? currency.CurrencyCode : _defaultCurrency;

            bsJournal.ResetBindings(false);
            gcJournalDetails.Focus();
        }

        private void ApplyPermissions()
        {
            btnNew.Enabled = _isAdmin || _canAdd;
            btnSaveAs.Enabled = _isAdmin || _canEdit;
            btnSave.Enabled = _isAdmin || _canEdit;
            btnSaveAndClose.Enabled = _isAdmin || _canEdit;
            btnPrint.Enabled = _isAdmin || _canPrint;
            btnDelete.Enabled = _isAdmin || _canDelete;
        }

        //private void InitializeMenuItems()
        //{
        //    var itemNew = new DXMenuItem("New", ItemNew_Click);
        //    var itemEdit = new DXMenuItem("Edit", ItemEdit_Click);
        //    var itemDelete = new DXMenuItem("Delete", ItemDelete_Click);
        //    _menuItems = new[] { itemNew, itemEdit, itemDelete };
        //}

        private void InitializeMenuItems()
        {
            _menuItems = new[]
            {
                new DXMenuItem("New", ItemNew_Click),
                new DXMenuItem("Edit", ItemEdit_Click),
                new DXMenuItem("Delete", ItemDelete_Click)
            };

            gvJournalDetails.PopupMenuShowing -= gvAddendum_PopupMenuShowing;
            gvJournalDetails.PopupMenuShowing += gvAddendum_PopupMenuShowing;
        }

        private void ConfigureJournalDetailsGrid(bool allowEditing)
        {
            gcJournalDetails.UseEmbeddedNavigator = allowEditing;
            gvJournalDetails.OptionsBehavior.Editable = allowEditing;
            gvJournalDetails.OptionsBehavior.ReadOnly = !allowEditing;
            gvJournalDetails.OptionsBehavior.AllowAddRows = allowEditing ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;
            gvJournalDetails.OptionsBehavior.AllowDeleteRows = allowEditing ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;
            gvJournalDetails.OptionsView.NewItemRowPosition = allowEditing ? NewItemRowPosition.Bottom : NewItemRowPosition.None;
        }

        //private void gvJournalDetails_InitNewRow(object sender, InitNewRowEventArgs e)
        //{
        //    var journalDetail = gvJournalDetails.GetRow(e.RowHandle) as JournalDetailModel;
        //    if (journalDetail == null)
        //    {
        //        return;
        //    }

        //    var journalDate = _journalModel.JournalDate == default(DateTime) ? DateTime.Today : _journalModel.JournalDate;
        //    journalDetail.JvNo = _journalModel.JvNo;
        //    journalDetail.Line = _journalDetails.Any() ? _journalDetails.Max(x => x.Line) + 1 : 1;
        //    journalDetail.ValueDate = journalDate;
        //    journalDetail.Currency = _journalModel.Currency;
        //    journalDetail.Rate = _journalModel.Rate > 0 ? _journalModel.Rate : _defaultRate;
        //    journalDetail.WorkingYear = _journalModel.WorkingYear > 0 ? _journalModel.WorkingYear : journalDate.Year;

        //    bsJournalDetails.ResetBindings(false);
        //}

        #endregion

        #region Menu Item Events

        private void ItemNew_Click(object sender, EventArgs e)
        {
            // Handle new item logic here
        }

        private void ItemEdit_Click(object sender, EventArgs e)
        {
            // Handle edit item logic here
            gvJournalDetails.ShowEditor();
        }

        private void ItemDelete_Click(object sender, EventArgs e)
        {
            if (!ConfirmAction("Delete row?", "Confirmation")) return;

            var journalDetail = gvJournalDetails.GetFocusedRow() as JournalDetailModel;
            if (journalDetail == null)
            {
                return;
            }

            journalDetail.Deleted = true;

            if (!string.IsNullOrWhiteSpace(journalDetail._id) && deletedList.All(x => x.Id != journalDetail._id))
            {
                deletedList.Add(new IdListModel { Id = journalDetail._id });
                _isDeleted = true;
            }

            _journalDetails.Remove(journalDetail);
            bsJournalDetails.ResetBindings(false);
            EnumerateLines();
        }

        #endregion

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


        #region Button Click Events

        private void btnNew_ItemClick(object sender, ItemClickEventArgs e)
        {
            _journalModel = new JournalModel();
            StartLoading();
        }

        private void btnSaveAs_ItemClick(object sender, ItemClickEventArgs e)
        {
            bsJournal.EndEdit();
            bsJournalDetails.EndEdit();
            gvJournalDetails.CloseEditor();
            gvJournalDetails.UpdateCurrentRow();

            if (_journalModel.Locked)
            {
                XtraMessageBox.Show("Period is locked, you cannot Add, Edit or Clone a locked JV.", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _journalModel = (_journalModel.Clone() as JournalModel) ?? new JournalModel();
            _journalModel._id = null;
            _journalModel.JvNo = string.Empty;
            _journalModel.Reference = string.Empty;
            _journalModel.IsPosted = false;
            _journalModel.IsCloned = true;
            _journalModel.Locked = false;

            _journalDetails = _journalDetails
                .Select(CloneJournalDetail)
                .ToList();

            ConfigureJournalDetailsGrid(true);

            bsJournal.DataSource = _journalModel;
            bsJournal.ResetBindings(false);

            bsJournalDetails.DataSource = _journalDetails;
            bsJournalDetails.ResetBindings(false);

            gcJournalDetails.Focus();
        }

        private async void btnRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {
            await InitializeBindings();
            WireUpBindings();
        }

        private async void btnSave_ItemClick(object sender, ItemClickEventArgs e)
        {
            await SaveDataAsync();
        }

        private async void btnSaveAndClose_ItemClick(object sender, ItemClickEventArgs e)
        {
            var saved = await SaveDataAsync();
            if (saved)
            {
                Close();
            }
        }

        private void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void btnPrint_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void chkShowProtected_CheckedChanged(object sender, ItemClickEventArgs e)
        {

        }

        private void btnPrintStatement_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void btnClose_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }

        private void btnResetGridStyle_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        #endregion

        private async Task<bool> SaveDataAsync()
        {
            bsJournal.EndEdit();
            bsJournalDetails.EndEdit();
            gvJournalDetails.CloseEditor();
            gvJournalDetails.UpdateCurrentRow();

            _journalModel = bsJournal.Current as JournalModel;
            if (_journalModel == null)
            {
                return false;
            }

            if (!ValidateData())
            {
                return false;
            }

            try
            {
                BindingContext[bsJournal].EndCurrentEdit();
                BindingContext[bsJournalDetails].EndCurrentEdit();

                var isNewJournal = string.IsNullOrWhiteSpace(_journalModel._id);

                if (isNewJournal)
                {
                    _logInfoRepository.CreateLogInfo(_journalModel);

                    var workingYear = _journalModel.WorkingYear > 0 ? _journalModel.WorkingYear : DateTime.Today.Year;
                    _journalModel.WorkingYear = workingYear;
                    _journalModel.JvNo = await _journalRepository.GetNextJvNoAsync(workingYear);

                    if (string.IsNullOrWhiteSpace(_journalModel.Reference))
                    {
                        _journalModel.Reference = await _journalRepository.GetNextReferenceAsync();
                    }

                    await _journalRepository.AddNewJournalAsync(_journalModel);

                    txtJvNo.Text = _journalModel.JvNo;
                    txtReference.Text = _journalModel.Reference;
                }
                else
                {
                    _logInfoRepository.UpdateLogInfo(_journalModel);
                    await _journalRepository.UpdateJournalAsync(_journalModel);
                }

                //Update Deleted list if Any
                if (_isDeleted)
                {
                    foreach (var record in deletedList)
                    {
                        await _journalDetailRepository.DeleteJournalDetailAsync(record.Id);
                    }

                    EnumerateLines();
                    _isDeleted = false;
                }

                // UPDATE DETAILS
                foreach (var journalDetail in _journalDetails)
                {
                    journalDetail.JvNo = _journalModel.JvNo;
                    journalDetail.WorkingYear = _journalModel.WorkingYear;

                    if (string.IsNullOrEmpty(journalDetail._id))
                    {
                        _logInfoRepository.CreateLogInfo(journalDetail);
                        await _journalDetailRepository.AddNewJournalDetailAsync(journalDetail);
                    }
                    else
                    {
                        _logInfoRepository.UpdateLogInfo(journalDetail);
                        await _journalDetailRepository.UpdateJournalDetailAsync(journalDetail);
                    }
                }

                SendUpdatedJournal?.Invoke(_journalModel, EventArgs.Empty);
                bsJournal.ResetBindings(false);
                bsJournalDetails.ResetBindings(false);
                return true;

            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message,
                    "On Save error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        private bool ValidateData()
        {
            var validateReturnValue = true;
            var messageNumber = 0;
            var validateMessage = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(_journalModel._id) && txtJvNo.Text == "")
            {
                messageNumber += 1;
                validateMessage.Append("\n- JV No cannot be empty.");
                validateReturnValue = false;
                txtJvNo.Focus();
            }

            if (dtJvDate.Text == "")
            {
                messageNumber += 1;
                validateMessage.Append("\n- JV Date cannot be empty.");
                validateReturnValue = false;
                dtJvDate.Focus();
            }

            if (cboJournalTypes.Text == "")
            {
                messageNumber += 1;
                validateMessage.Append("\n- Type cannot be empty.");
                validateReturnValue = false;
                cboJournalTypes.Focus();
            }

            if (_journalModel.Locked)
            {
                messageNumber += 1;
                validateMessage.Append("\n- Period is locked, you cannot Add, Edit or Clone a locked JV.");
                validateReturnValue = false;
                dtJvDate.Focus();
            }

            if (cboCurrency.Text == "")
            {
                messageNumber += 1;
                validateMessage.Append("\n- Currency cannot be empty.");
                validateReturnValue = false;
                cboCurrency.Focus();
            }
            if (txtRate.Text == "")
            {
                messageNumber += 1;
                validateMessage.Append("\n- Rate cannot be empty.");
                validateReturnValue = false;
                txtRate.Focus();
            }

            if (txtWorkingYear.Text == "")
            {
                messageNumber += 1;
                validateMessage.Append("\n- WorkingYear cannot be empty.");
                validateReturnValue = false;
                txtWorkingYear.Focus();
            }

            if (!_journalDetails.Any())
            {
                messageNumber += 1;
                validateMessage.Append("\n- Journal should contains details.");
                validateReturnValue = false;
                gcJournalDetails.Focus();
            }

            //- Check if Balance is zero
            decimal.TryParse(txtBalanceLL.Text, out var balanceLL);
            decimal.TryParse(txtBalanceUSD.Text, out var balanceUSD);
            if (balanceLL != 0 || balanceUSD != 0)
            {
                messageNumber += 1;
                validateMessage.Append("\n- Cannot save unbalanced JV.");
                validateReturnValue = false;
            }

            if (!validateReturnValue)
            {
                validateMessage.Insert(0, "The following need your attention:");
                if (messageNumber > 1) validateMessage.Replace("following", "followings");
                XtraMessageBox.Show(validateMessage + " \nPlease try again.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return validateReturnValue;
        }

        #region Helper Methods

        private bool ConfirmDelete(string itemName)
        {
            return XtraMessageBox.Show(
                $"Are you sure you want to delete Record: `{itemName}`?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) == DialogResult.Yes;
        }

        private void HandleDeleteError(Exception ex)
        {
            string message;
            switch (ex.Message)
            {
                case "-2146233088":
                    message = "This record is linked to one or more transactions, delete all links first.";
                    break;
                default:
                    message = ex.Message;
                    break;
            }

            XtraMessageBox.Show(message, "Delete error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ShowError(string message, Exception ex)
        {
            XtraMessageBox.Show(
                $"{message}\n\nDetails: {ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        private JournalDetailModel CloneJournalDetail(JournalDetailModel source)
        {
            if (source == null)
            {
                return new JournalDetailModel();
            }

            var clonedDetail = (source.Clone() as JournalDetailModel) ?? new JournalDetailModel();
            clonedDetail._id = null;
            clonedDetail.JvNo = string.Empty;
            clonedDetail.Posted = false;
            return clonedDetail;
        }

        #endregion

        private void EnumerateLines()
        {
            for (int i = 0; i < gvJournalDetails.DataRowCount; i++)
            {
                gvJournalDetails.SetRowCellValue(i, gvJournalDetails.Columns["Line"], i + 1);
            }
        }

        private void gvJournalDetails_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;

            try
            {
                if (e.Column.FieldName == nameof(JournalDetailModel.AccountNumber))
                {
                    CheckChartOfAccount(sender, e);
                }

                //Currency
                if (e.Column.FieldName == "Currency")
                {
                    CheckCurrency(sender, e);
                }

                //Debit Credit
                if (e.Column.FieldName == "DbCr")
                {
                    CalculateRemainingBalance(sender, e);
                }

                //Rate/Amounts
                if (e.Column.FieldName == "Rate" || e.Column.FieldName == "Amount")
                {
                    CalculateAmounts(sender, e);
                }

                //Cost Center
                if (e.Column.FieldName == "CostCenter")
                {
                    CalculateCostCenter(sender, e);
                }
            }
            catch
            {
                // Handle exceptions if necessary, or log them
                var err = "";
            }
        }

        private void CheckChartOfAccount(GridView view, CellValueChangedEventArgs e)
        {
            var accountNumber = view.GetRowCellValue(e.RowHandle, nameof(JournalDetailModel.AccountNumber))?.ToString();
            var chartDetail = _charts.FirstOrDefault(c => string.Equals(c.AccountNumber, accountNumber, StringComparison.OrdinalIgnoreCase));
            if (chartDetail != null)
            {
                view.SetRowCellValue(e.RowHandle, nameof(JournalDetailModel.AccountName), chartDetail.AccountName);
            }
        }

        #region Chart Event
        private void CheckChartOfAccount(object sender, CellValueChangedEventArgs e)
        {
            var view = sender as GridView;
            if (view == null)
            {
                return;
            }

            CheckChartOfAccount(view, e);
        }

        private void CheckCurrency(object sender, CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;
            //get current amount
            decimal varAmount = 0;
            //get remaining balance
            decimal varBalanceAmount = 0;
            if (view.GetRowCellValue(view.FocusedRowHandle, view.Columns["Currency"]) != null)
            {
                string currencyId =
                    view.GetRowCellValue(view.FocusedRowHandle, view.Columns["Currency"]).ToString();
            }

            if (view.GetRowCellValue(view.FocusedRowHandle, view.Columns["Amount"]) != null)
            {
                varAmount = decimal.Parse(view.GetRowCellValue(view.FocusedRowHandle, view.Columns["Amount"]).ToString());
            }
        }

        private decimal CalculateRemainingBalance(object sender, CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;
            if (view.GetRowCellValue(view.FocusedRowHandle, view.Columns["DbCr"]) == null || view.GetRowCellValue(view.FocusedRowHandle, view.Columns["Currency"]) == null)
            {
                return 0;
            }
            decimal total = 0;
            string currencyId = view.GetRowCellValue(view.FocusedRowHandle, view.Columns["Currency"]).ToString();

            for (int i = 0; i < view.RowCount; i++)
            {
                if (view.GetRowCellValue(view.FocusedRowHandle, view.Columns["DbCr"]).ToString() == "D")
                {
                    total += Convert.ToDecimal(view.GetRowCellValue(i + 1, "FAmount"));
                }
                else if (view.GetRowCellValue(view.FocusedRowHandle, view.Columns["DbCr"]).ToString() == "C")
                {
                    total -= Convert.ToDecimal(view.GetRowCellValue(i + 1, "FAmount"));
                }
            }

            total = CalculateRate("USD", currencyId.ToString(), total);
            return total;
        }

        private decimal CalculateRate(string currencyIn, string currencyOut, decimal pValue)
        {
            decimal exchangeRate = 0;
            decimal rateValue = 0;
            decimal baseCurrencyValue = 0;

            baseCurrencyValue = GetRate("USD");
            exchangeRate = GetRate(currencyIn);

            if (currencyIn == currencyOut) rateValue = pValue;
            if (currencyOut == "LL" && currencyIn != currencyOut)
                rateValue = pValue * exchangeRate;
            else if (currencyOut == "USD" && currencyIn != currencyOut)
                rateValue = pValue * exchangeRate / baseCurrencyValue;

            return rateValue;
        }

        private decimal GetRate(string pCurrencyId)
        {
            var currencyDate = dtJvDate.DateTime;
            decimal rateValue = 0;

            //var resultRateCurrency = _services.CurrencyExchange.GetCurrencyRate(pCurrencyId, currencyDate);
            var resultRateCurrency = _currencyExchangeRepository.GetLatestExchangeByCurrencyAsync(pCurrencyId.ToString(), currencyDate).GetAwaiter().GetResult();
            if (resultRateCurrency != null) rateValue = resultRateCurrency.Rate;

            return rateValue;
        }

        private void CalculateAmounts(object sender, CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;

            if (view == null)
            {
                return;
            }

            object amountCellValue = view.GetRowCellValue(e.RowHandle, view.Columns["Amount"]);
            object currencyIdCellValue = view.GetRowCellValue(e.RowHandle, view.Columns["Currency"]);

            if (amountCellValue == null || currencyIdCellValue == null)
            {
                return;
            }

            if (!decimal.TryParse(amountCellValue.ToString(), out decimal amountValue))
            {
                return;
            }

            //if (!int.TryParse(currencyIdCellValue.ToString(), out int curId))
            //{
            //    return;
            //}

            view.SetRowCellValue(e.RowHandle, "LAmount", CalculateRate(currencyIdCellValue.ToString(), "LL", amountValue));
            view.SetRowCellValue(e.RowHandle, "FAmount", CalculateRate(currencyIdCellValue.ToString(), "USD", amountValue));
        }

        private void CalculateCostCenter(object sender, CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;

        }

        #endregion

        private void gvJournalDetails_InitNewRow_1(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;
            bool isValidAmount;

            //Set static values to Journal details
            view.SetRowCellValue(e.RowHandle, view.Columns["JvNo"], _journalModel.JvNo);
            view.SetRowCellValue(e.RowHandle, view.Columns["Line"], view.RowCount);
            view.SetRowCellValue(e.RowHandle, view.Columns["ValueDate"], DateTime.Now);
            view.SetRowCellValue(e.RowHandle, view.Columns["WorkingYear"], CurrentUser.WorkingYear);
            view.SetRowCellValue(e.RowHandle, view.Columns["Currency"], cboCurrency.EditValue);
            view.SetRowCellValue(e.RowHandle, view.Columns["Rate"], decimal.Parse(txtRate.Text));

            int currentRowVisibleIndex = view.GetDataSourceRowIndex(e.RowHandle);
            int previousRowVisibleIndex = currentRowVisibleIndex - 1;
            object row = view.GetRow(previousRowVisibleIndex);

            //After selecting Account number start setting default data
            //Description
            if (view.GetRowCellValue(previousRowVisibleIndex, view.Columns["Description"]) == null)
            {
                view.SetRowCellValue(e.RowHandle, view.Columns["Description"], txtNotes.Text.Trim());
            }
            else
            {
                view.SetRowCellValue(e.RowHandle, view.Columns["Description"], view.GetRowCellValue(previousRowVisibleIndex, view.Columns["Description"]));
            }

            // Debit/Credit flag
            if (view.GetRowCellValue(previousRowVisibleIndex, view.Columns["DbCr"]) == null)
            {
                view.SetRowCellValue(e.RowHandle, view.Columns["DbCr"], EnumDebCred.D);
            }
            else
            {
                view.SetRowCellValue(e.RowHandle, view.Columns["DbCr"],
                    (view.GetRowCellValue(previousRowVisibleIndex, view.Columns["DbCr"]).ToString() == "D") ? EnumDebCred.C : EnumDebCred.D);
            }

            // Amount
            if (view.GetRowCellValue(previousRowVisibleIndex, view.Columns["Amount"]) == null)
            {
                view.SetRowCellValue(e.RowHandle, view.Columns["Amount"], 0);
            }
            else
            {
                decimal.TryParse(view.GetRowCellValue(previousRowVisibleIndex, view.Columns["Amount"]).ToString(),
                    out decimal previousAmount);

                view.SetRowCellValue(e.RowHandle, view.Columns["Amount"], Math.Abs(previousAmount));
            }

            // LAmount
            if (view.GetRowCellValue(previousRowVisibleIndex, view.Columns["LAmount"]) == null)
            {
                view.SetRowCellValue(e.RowHandle, view.Columns["LAmount"], 0);
            }
            else
            {
                isValidAmount = decimal.TryParse(txtBalanceLL.Text, out decimal varBalAmount);
                if (isValidAmount)
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns["LAmount"], Math.Abs(varBalAmount));
                }
            }
            // FAmount
            if (view.GetRowCellValue(previousRowVisibleIndex, view.Columns["FAmount"]) == null)
            {
                view.SetRowCellValue(e.RowHandle, view.Columns["FAmount"], 0);
            }
            else
            {
                isValidAmount = decimal.TryParse(txtBalanceUSD.Text, out decimal varBalAmount);
                if (isValidAmount)
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns["FAmount"], Math.Abs(varBalAmount));
                }
            }
        }

        private void BindGridWithBindingSource<T>(GridControl grid, BindingSource bindingSource, T dataSource)
        {
            bindingSource.DataSource = dataSource;
            grid.DataSource = null;
            grid.DataSource = bindingSource;
        }

        private void gvAddendum_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (!CanManageJournal())
            {
                e.Menu = null;
                return;
            }

            if (e.MenuType != GridMenuType.Row && e.MenuType != GridMenuType.User) return;

            var view = sender as GridView;
            if (view == null) return;

            if (e.HitInfo.InRow || e.HitInfo.InRowCell)
            {
                view.FocusedRowHandle = e.HitInfo.RowHandle;
            }

            if (e.Menu == null)
            {
                e.Menu = new GridViewMenu(view);
            }

            e.Menu.Items.Clear();
            foreach (var menuItem in _menuItems)
            {
                e.Menu.Items.Add(menuItem);
            }
        }

        private bool CanManageJournal()
        {
            if (!ValidateData() || string.IsNullOrWhiteSpace(_journalModel?._id))
            {
                XtraMessageBox.Show("Finish journal first, then add details.", "Journal",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return true;
        }

        private void gvJournalDetails_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Delete)
            {
                return;
            }

            ItemDelete_Click(sender, EventArgs.Empty);
        }

        private void gvJournalDetails_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            var summaryId = Convert.ToInt32((e.Item as GridSummaryItem)?.Tag);

            if (e.SummaryProcess != CustomSummaryProcess.Finalize)
            {
                return;
            }

            decimal debitLL;
            decimal debitUSD;
            decimal creditLL;
            decimal creditUSD;
            decimal totalLL;
            decimal totalUSD;

            CalculateJournalTotals(out debitLL, out debitUSD, out creditLL, out creditUSD, out totalLL, out totalUSD);

            txtDebitLL.EditValue = debitLL;
            txtDebitUSD.EditValue = debitUSD;
            txtCreditLL.EditValue = creditLL;
            txtCreditUSD.EditValue = creditUSD;
            txtBalanceLL.EditValue = debitLL - creditLL;
            txtBalanceUSD.EditValue = debitUSD - creditUSD;

            switch (summaryId)
            {
                case 1:
                    e.TotalValue = totalLL;
                    break;
                case 2:
                    e.TotalValue = totalUSD;
                    break;
            }
        }

        private void CalculateJournalTotals(out decimal debitLL, out decimal debitUSD, out decimal creditLL, out decimal creditUSD, out decimal totalLL, out decimal totalUSD)
        {
            debitLL = 0m;
            debitUSD = 0m;
            creditLL = 0m;
            creditUSD = 0m;
            totalLL = 0m;
            totalUSD = 0m;

            if (_journalDetails == null)
            {
                return;
            }

            foreach (var journalDetail in _journalDetails)
            {
                if (journalDetail == null || journalDetail.Deleted)
                {
                    continue;
                }

                totalLL += journalDetail.LAmount;
                totalUSD += journalDetail.FAmount;

                if (string.Equals(journalDetail.DbCr, "D", StringComparison.OrdinalIgnoreCase))
                {
                    debitLL += journalDetail.LAmount;
                    debitUSD += journalDetail.FAmount;
                }
                else if (string.Equals(journalDetail.DbCr, "C", StringComparison.OrdinalIgnoreCase))
                {
                    creditLL += journalDetail.LAmount;
                    creditUSD += journalDetail.FAmount;
                }
            }

            totalLL = debitLL + creditLL;
            totalUSD = debitUSD + creditUSD;
        }

    }
}