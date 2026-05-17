using DevExpress.Utils.Menu;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using Spectrum.DataLayers.Accounting.Charts;
using Spectrum.DataLayers.Accounting.JournalType;
using Spectrum.DataLayers.Common.Currencies;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Accounting.Charts;
using Spectrum.Models.Accounting.Journals;
using Spectrum.Models.Accounting.JournalType;
using Spectrum.Models.Common.Currencies;
using Spectrum.Models.Members.Clients;
using Spectrum.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly ChartRepository _chartRepository = new ChartRepository(DatabaseFactory.ProfilePrimary);
        private readonly ChartDetailRepository _chartDetailRepository = new ChartDetailRepository(DatabaseFactory.ProfilePrimary);
        private readonly CurrencyRepository _currencyRepository = new CurrencyRepository(DatabaseFactory.ProfilePrimary);
        private readonly JournalTypeRepository _journalTypeRepository = new JournalTypeRepository(DatabaseFactory.ProfilePrimary);

        private readonly LogInfoRepository _logInfoRepository = new LogInfoRepository();

        private string _defaultJournalType = "JV";
        private string _defaultCurrency = "USD";
        private decimal _defaultRate = 89500;

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
                ApplyDefaults();
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

        private void ApplyDefaults()
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

            var journalType = _journalTypes.FirstOrDefault(x => string.Equals(x.Code, _defaultJournalType, StringComparison.OrdinalIgnoreCase));
            _journalModel.JournalType = journalType != null ? journalType._id : _defaultJournalType;

            var currency = _currencies.FirstOrDefault(x => string.Equals(x.CurrencyCode, _defaultCurrency, StringComparison.OrdinalIgnoreCase));
            _journalModel.Currency = currency != null ? currency._id : _defaultCurrency;

            bsJournal.ResetBindings(false);
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
            if (_journalModel.Locked)
            {
                XtraMessageBox.Show("Period is locked, you cannot Add, Edit or Clone a locked JV.", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _journalModel = _journalModel.Clone() as JournalModel;
            IList<JournalDetailModel> newJournalDetails = new List<JournalDetailModel>();

            foreach (var record in _journalDetails)
            {
                record.JvNo = 0;
                record.ValueDate = DateTime.Now;
                newJournalDetails.Add(record.Clone() as JournalDetailModel);
            }
            _journalDetails = newJournalDetails;

            _journalModel.Id = 0;
            _journalModel.JvNo = 0;
            _journalModel.Reference = "";

            bsJournal.DataSource = _journalModel;
            bsJournal.ResetBindings(false);

            bsJournalDetails.DataSource = _journalDetails;
            bsJournalDetails.ResetBindings(false);

            gcJournalDetails.Focus();
        }

        private void btnRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void btnSave_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void btnSaveAndClose_ItemClick(object sender, ItemClickEventArgs e)
        {

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

        #endregion
    }
}