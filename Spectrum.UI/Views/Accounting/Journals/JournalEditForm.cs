using DevExpress.Utils.Menu;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using Spectrum.DataLayers.Accounting.Charts;
using Spectrum.DataLayers.Accounting.Journals;
using Spectrum.DataLayers.Accounting.JournalType;
using Spectrum.DataLayers.Common.Currencies;
using Spectrum.DataLayers.Common.Currencies.Interfaces;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Accounting.Charts;
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
        private IList<ChartDetailModel> _chartDetails = new List<ChartDetailModel>();
        private IList<CurrencyModel> _currencies = new List<CurrencyModel>();

        private IList<JournalTypeModel> _journalTypes = new List<JournalTypeModel>();
        private IList<IdListModel> deletedList = new List<IdListModel>();

        private readonly JournalRepository _journalRepository = new JournalRepository(DatabaseFactory.ProfilePrimary);
        private readonly JournalDetailRepository _journalDetailRepository = new JournalDetailRepository(DatabaseFactory.ProfilePrimary);

        private readonly ChartRepository _chartRepository = new ChartRepository(DatabaseFactory.ProfilePrimary);
        private readonly ChartDetailRepository _chartDetailRepository = new ChartDetailRepository(DatabaseFactory.ProfilePrimary);
        private readonly CurrencyRepository _currencyRepository = new CurrencyRepository(DatabaseFactory.ProfilePrimary);
        private readonly ICurrencyExchangeRepository _currencyExchangeRepository = new CurrencyExchangeRepository(DatabaseFactory.ProfilePrimary);
        private readonly JournalTypeRepository _journalTypeRepository = new JournalTypeRepository(DatabaseFactory.ProfilePrimary);

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
                    LoadChartDetailsAsync(),
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

        private async Task LoadChartDetailsAsync()
        {
            _chartDetails = await _chartDetailRepository.GetChartDetailsAsync();
        }

        #endregion


        private void WireUpBindings()
        {
            if (txtWorkingYear.DataBindings.Count == 0)
            {
                txtWorkingYear.DataBindings.Add("EditValue", bsJournal, nameof(JournalModel.WorkingYear), true, DataSourceUpdateMode.OnPropertyChanged);
            }

            bsJournal.DataSource = _journalModel;
            bsJournalDetails.DataSource = _journalDetails;

            cboJournalTypes.Properties.DataSource = _journalTypes;
            cboJournalTypes.Properties.DisplayMember = nameof(JournalTypeModel.Code);
            cboJournalTypes.Properties.ValueMember = nameof(JournalTypeModel._id);

            cboCurrency.Properties.DataSource = _currencies;
            cboCurrency.Properties.ValueMember = nameof(CurrencyModel._id);

            repCharts.DataSource = _chartDetails;
            repCharts.ValueMember = nameof(ChartDetailModel._id);

            repAccountNames.DataSource = _chartDetails;
            repAccountNames.ValueMember = nameof(ChartDetailModel._id);

            repCurrencies.DataSource = _currencies;
            repCurrencies.ValueMember = nameof(CurrencyModel._id);

            gvJournalDetails.InitNewRow -= gvJournalDetails_InitNewRow;
            gvJournalDetails.InitNewRow += gvJournalDetails_InitNewRow;
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
            _journalModel.JournalType = journalType != null ? journalType._id : _defaultJournalType;

            var currency = _currencies.FirstOrDefault(x => string.Equals(x.CurrencyCode, _defaultCurrency, StringComparison.OrdinalIgnoreCase));
            _journalModel.Currency = currency != null ? currency._id : _defaultCurrency;

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

        private void InitializeMenuItems()
        {
            var itemNew = new DXMenuItem("New", ItemNew_Click);
            var itemEdit = new DXMenuItem("Edit", ItemEdit_Click);
            var itemDelete = new DXMenuItem("Delete", ItemDelete_Click);
            _menuItems = new[] { itemNew, itemEdit, itemDelete };
        }

        private void ConfigureJournalDetailsGrid(bool allowEditing)
        {
            gcJournalDetails.UseEmbeddedNavigator = allowEditing;
            gvJournalDetails.OptionsBehavior.Editable = allowEditing;
            gvJournalDetails.OptionsBehavior.ReadOnly = !allowEditing;
            gvJournalDetails.OptionsBehavior.AllowAddRows = allowEditing ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;
            gvJournalDetails.OptionsBehavior.AllowDeleteRows = allowEditing ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;
            gvJournalDetails.OptionsView.NewItemRowPosition = allowEditing ? DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom : DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None;
        }

        private void gvJournalDetails_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            var journalDetail = gvJournalDetails.GetRow(e.RowHandle) as JournalDetailModel;
            if (journalDetail == null)
            {
                return;
            }

            var journalDate = _journalModel.JournalDate == default(DateTime) ? DateTime.Today : _journalModel.JournalDate;
            journalDetail.JvNo = _journalModel.JvNo;
            journalDetail.Line = _journalDetails.Any() ? _journalDetails.Max(x => x.Line) + 1 : 1;
            journalDetail.ValueDate = journalDate;
            journalDetail.Currency = _journalModel.Currency;
            journalDetail.Rate = _journalModel.Rate > 0 ? _journalModel.Rate : _defaultRate;
            journalDetail.WorkingYear = _journalModel.WorkingYear > 0 ? _journalModel.WorkingYear : journalDate.Year;

            bsJournalDetails.ResetBindings(false);
        }

        #endregion

        #region Menu Item Events

        private void ItemNew_Click(object sender, EventArgs e)
        {
            // Handle new item logic here
        }

        private void ItemEdit_Click(object sender, EventArgs e)
        {
            // Handle edit item logic here
        }

        private void ItemDelete_Click(object sender, EventArgs e)
        {
            // Handle delete item logic here
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

        private void gvJournalDetails_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;

            try
            {
                if (e.Column.FieldName == "ChartId")
                {
                    CheckChartOfAccount(sender, e);
                }

                //Currency
                if (e.Column.FieldName == "CurrencyId")
                {
                    CheckCurrency(sender, e);
                }

                //Debit Credit
                if (e.Column.FieldName == "DebCred")
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
            }
        }

        private void CheckChartOfAccount(GridView view, CellValueChangedEventArgs e)
        {
            var chartId = view.GetRowCellValue(e.RowHandle, "ChartId")?.ToString();
            var chartDetail = _chartDetails.FirstOrDefault(c => c._id == chartId);
            if (chartDetail != null)
            {
                view.SetRowCellValue(e.RowHandle, "AccountName", chartDetail.AccountName);
                view.SetRowCellValue(e.RowHandle, "CurrencyId", chartDetail.CurrencyId);
            }
        }

        #region Chart Event
        private void CheckChartOfAccount(object sender, CellValueChangedEventArgs e)
        {
            GridView chartView = repCharts.View;
            int rowHandle = chartView.FocusedRowHandle;
            string fieldName = "ChartId";
            object accountName = chartView.GetRowCellValue(rowHandle, fieldName);

            GridView view = sender as GridView;
            view.SetRowCellValue(e.RowHandle, "AccountName", accountName);

            _ = int.TryParse(view.GetRowCellValue(view.FocusedRowHandle, view.Columns["ChartId"]).ToString(), out int chartId);

        }

        private void CheckCurrency(object sender, CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;
            //get current amount
            decimal varAmount = 0;
            //get remaining balance
            decimal varBalanceAmount = 0;
            if (view.GetRowCellValue(view.FocusedRowHandle, view.Columns["CurrencyId"]) != null)
            {
                int currencyId =
                    int.Parse(view.GetRowCellValue(view.FocusedRowHandle, view.Columns["CurrencyId"]).ToString());
            }

            if (view.GetRowCellValue(view.FocusedRowHandle, view.Columns["Amount"]) != null)
            {
                varAmount = decimal.Parse(view.GetRowCellValue(view.FocusedRowHandle, view.Columns["Amount"]).ToString());
            }
        }

        private decimal CalculateRemainingBalance(object sender, CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;
            if (view.GetRowCellValue(view.FocusedRowHandle, view.Columns["DebCred"]) == null || view.GetRowCellValue(view.FocusedRowHandle, view.Columns["CurrencyId"]) == null)
            {
                return 0;
            }
            decimal total = 0;
            int currencyId = int.Parse(view.GetRowCellValue(view.FocusedRowHandle, view.Columns["CurrencyId"]).ToString());

            for (int i = 0; i < view.RowCount; i++)
            {
                if (view.GetRowCellValue(view.FocusedRowHandle, view.Columns["DebCred"]).ToString() == "D")
                {
                    total += Convert.ToDecimal(view.GetRowCellValue(i + 1, "FAmount"));
                }
                else if (view.GetRowCellValue(view.FocusedRowHandle, view.Columns["DebCred"]).ToString() == "C")
                {
                    total -= Convert.ToDecimal(view.GetRowCellValue(i + 1, "FAmount"));
                }
            }

            total = CalculateRate(2, currencyId, total);
            return total;
        }

        private decimal CalculateRate(int currencyIn, int currencyOut, decimal pValue)
        {
            decimal exchangeRate = 0;
            decimal rateValue = 0;
            decimal baseCurrencyValue = 0;

            baseCurrencyValue = GetRate(2);
            exchangeRate = GetRate(currencyIn);

            if (currencyIn == currencyOut) rateValue = pValue;
            if (currencyOut == 1 && currencyIn != currencyOut)
                rateValue = pValue * exchangeRate;
            else if (currencyOut == 2 && currencyIn != currencyOut)
                rateValue = pValue * exchangeRate / baseCurrencyValue;

            return rateValue;
        }

        private decimal GetRate(int pCurrencyId)
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

            if (view.GetRowCellValue(view.FocusedRowHandle, view.Columns["Amount"]) != null)
            {
                decimal amountValue = 0;
                {
                    decimal.TryParse(view.GetRowCellValue(view.FocusedRowHandle, view.Columns["Amount"]).ToString(),
                        out amountValue);
                }
                int curId = int.Parse(view.GetRowCellValue(view.FocusedRowHandle, view.Columns["CurrencyId"])
                    .ToString());
                view.SetRowCellValue(e.RowHandle, "LAmount", CalculateRate(curId, 1, amountValue));
                view.SetRowCellValue(e.RowHandle, "FAmount", CalculateRate(curId, 2, amountValue));
            }
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
            view.SetRowCellValue(e.RowHandle, view.Columns["JournalId"], _journalModel._id);
            view.SetRowCellValue(e.RowHandle, view.Columns["Line"], view.RowCount);
            view.SetRowCellValue(e.RowHandle, view.Columns["ValueDate"], DateTime.Now);
            view.SetRowCellValue(e.RowHandle, view.Columns["WorkingYear"], CurrentUser.WorkingYear);
            view.SetRowCellValue(e.RowHandle, view.Columns["CurrencyId"], cboCurrency.EditValue);
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
    }
}