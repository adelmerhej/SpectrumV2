using DevExpress.XtraEditors;
using Spectrum.DataLayers.Accounting.Charts;
using Spectrum.DataLayers.Accounting.CostCenter;
using Spectrum.DataLayers.Accounting.FlowType;
using Spectrum.DataLayers.Accounting.JournalType;
using Spectrum.DataLayers.Accounting.Reports.StatementReports;
using Spectrum.DataLayers.Common.Currencies;
using Spectrum.DataLayers.DataAccess;
using Spectrum.DataLayers.Users;
using Spectrum.Models.Accounting.Charts;
using Spectrum.Models.Accounting.CostCenter;
using Spectrum.Models.Accounting.FlowType;
using Spectrum.Models.Accounting.JournalType;
using Spectrum.Models.Accounting.Reports.StatementReports;
using Spectrum.Models.Common.Currencies;
using Spectrum.Models.Users;
using Spectrum.Reports.Accounting.StatementReports;
using Spectrum.Views.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.Accounting.Reports.StatementReports
{
    public partial class StatementOfAccount : XtraForm
    {
        private const string _formName = "StatementOfAccount";
        private int _formId;
        private bool _resetMenu;

        private IList<ChartModel> _charts = new List<ChartModel>();
        private IList<CurrencyModel> _currencies = new List<CurrencyModel>();
        private IList<JournalTypeModel> _journalTypes = new List<JournalTypeModel>();
        private IList<CostCenterModel> _costCenters = new List<CostCenterModel>();
        private IList<FlowTypeModel> _flowTypes = new List<FlowTypeModel>();

        private readonly ChartRepository _chartRepository = new ChartRepository(DatabaseFactory.ProfilePrimary);
        private readonly StatementOfAccountRepository _statementOfAccountRepository = new StatementOfAccountRepository(DatabaseFactory.ProfilePrimary);
        private readonly CostCenterRepository _costCenterRepository = new CostCenterRepository(DatabaseFactory.ProfilePrimary);
        private readonly FlowTypeRepository _flowTypeRepository = new FlowTypeRepository(DatabaseFactory.ProfilePrimary);
        private readonly JournalTypeRepository _journalTypeRepository = new JournalTypeRepository(DatabaseFactory.ProfilePrimary);
        private readonly CurrencyRepository _currencyRepository = new CurrencyRepository(DatabaseFactory.ProfilePrimary);

        /// <summary>
        /// User Permission Role
        /// </summary>
        private IList<UserPermissionModel> _userPermission = new List<UserPermissionModel>();
        private readonly UserPermissionRepository _userPermissionRepository = new UserPermissionRepository();

        //Init permission variables
        private bool _isAdmin;
        private bool _isProtected;

        private string _defaultJournalType = "JV";
        private string _defaultCurrency = "USD";



        public StatementOfAccount()
        {
            InitializeComponent();
            cboAccountNumber.EditValueChanged += cboAccountNumber_EditValueChanged;

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
                var loadTasks = new[]
                {
                    LoadJournalTypesAsync(),
                    LoadCurrenciesAsync(),
                    LoadChartsAsync(),
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

        private void WireUpBindings()
        {
            cboAccountNumber.Properties.DataSource = null;
            cboAccountNumber.Properties.DataSource = _charts;

            cboJournalTypes.Properties.DataSource = null;
            cboJournalTypes.Properties.DataSource = _journalTypes;

            cboCurrencies.Properties.DataSource = null;
            cboCurrencies.Properties.DataSource = _currencies;

            cboCostCenters.Properties.DataSource = null;
            cboCostCenters.Properties.DataSource = _costCenters;

            cboFlowTypes.Properties.DataSource = null;
            cboFlowTypes.Properties.DataSource = _flowTypes;

        }

        private void ApplyDefaults()
        {
            cboJournalTypes.EditValue = _defaultJournalType;
            cboCurrencies.EditValue = _defaultCurrency;

            chkWithPreviousBalance.Checked = true;
            chkConsolidatePastPeriods.Checked = true;   
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

        }

        #region Buttons Event

        private async void btnPrint_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;
            //Prepare variables
            DateTime? dateFrom = null;
            DateTime? dateTo = null;
            if (dtDateFrom.EditValue != null)
            {
                dateFrom = Convert.ToDateTime(dtDateFrom.EditValue).Date;
            }

            if (dtDateTo.EditValue != null)
            {
                dateTo = Convert.ToDateTime(dtDateTo.EditValue).Date;
            }

            string chartId = "0";
            ChartModel selectedChart = cboAccountNumber.EditValue as ChartModel;

            if (selectedChart != null)
            {
                chartId = selectedChart._id;
            }
            else if (cboAccountNumber.EditValue != null)
            {
                chartId = cboAccountNumber.EditValue.ToString();
            }

            string currencyId = "0";
            if (cboCurrencies.EditValue != null) currencyId = cboCurrencies.EditValue.ToString();

            string journalTypes = "0";
            if (cboJournalTypes.EditValue != null)
            {
                journalTypes = ResolveJournalType(cboJournalTypes.EditValue);
            }

            string costCenter = "0";
            if (cboCostCenters.EditValue != null)
            {
                costCenter = ResolveCostCenter(cboCostCenters.EditValue);
            }

            string flowType = "0";
            if (cboFlowTypes.EditValue != null)
            {
                flowType = ResolveFlowType(cboFlowTypes.EditValue);
            }

            bool withPreviousBalance = chkWithPreviousBalance.Checked;
            bool showDetailed = chkShowDetailed.Checked;

            int currencyOut = rgCurrencyOut.SelectedIndex;

            int workingYear = CurrentUser.WorkingYear;

            bool consolidatePastPeriods = chkConsolidatePastPeriods.Checked;
            var isProtected = chkShowProtected.Checked;

            IList<StatementOfAccountReportModel> dataReportModels =
                await _statementOfAccountRepository.StatementOfAccountReportAsync(
                    dateFrom,
                    dateTo,
                    chartId,
                    currencyId,
                    journalTypes,
                    costCenter,
                    flowType,
                    workingYear,
                    consolidatePastPeriods,
                    isProtected);

            ChartModel chartAccount = selectedChart ?? await _chartRepository.GetChartByIdAsync(chartId);

            var previewForm = new DocumentViewerForm();
            var report = new StatementOfAccountReport();

            report.DataSource = dataReportModels;

            report.Parameters["DateFrom"].Value = dateFrom;
            report.Parameters["DateTo"].Value = dateTo;
            report.Parameters["AccountNumber"].Value = chartAccount.AccountNumber;
            report.Parameters["AccountName"].Value = chartAccount.AccountName;
            report.Parameters["CurrencyOut"].Value = currencyOut;

            previewForm.Viewer.DocumentSource = report;
            previewForm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cboAccountNumber_EditValueChanged(object sender, EventArgs e)
        {
            var selectedChart = GetSelectedChart();
            txtAccountName.Text = selectedChart != null ? selectedChart.AccountName : string.Empty;
        }

        #endregion

        private bool ValidateForm()
        {
            var validateReturnValue = true;
            var messageNumber = 0;
            var validateMessage = new StringBuilder();

            if (cboAccountNumber.Text == "")
            {
                messageNumber += 1;
                validateMessage.Append("\n- Account Number cannot be empty.");
                validateReturnValue = false;
                cboAccountNumber.Focus();
            }

            if (!validateReturnValue)
            {
                validateMessage.Insert(0, "The following need your attention:\n");
                if (messageNumber > 1) validateMessage.Replace("following", "followings");
                XtraMessageBox.Show(validateMessage + " \n\nCannot be null or empty, please try again.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return validateReturnValue;
        }

        private string ResolveJournalType(object editValue)
        {
            var selectedJournalType = editValue as JournalTypeModel;
            if (selectedJournalType != null)
            {
                return selectedJournalType.Code;
            }

            var selectedValue = editValue.ToString();
            var journalType = _journalTypes.FirstOrDefault(x =>
                string.Equals(x._id, selectedValue, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(x.Code, selectedValue, StringComparison.OrdinalIgnoreCase));

            return journalType != null ? journalType.Code : selectedValue;
        }

        private string ResolveCostCenter(object editValue)
        {
            var selectedCostCenter = editValue as CostCenterModel;
            if (selectedCostCenter != null)
            {
                return selectedCostCenter.CostCenterName;
            }

            var selectedValue = editValue.ToString();
            var costCenter = _costCenters.FirstOrDefault(x =>
                string.Equals(x._id, selectedValue, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(x.CostCenterName, selectedValue, StringComparison.OrdinalIgnoreCase));

            return costCenter != null ? costCenter.CostCenterName : selectedValue;
        }

        private string ResolveFlowType(object editValue)
        {
            var selectedFlowType = editValue as FlowTypeModel;
            if (selectedFlowType != null)
            {
                return selectedFlowType.FlowTypeName;
            }

            var selectedValue = editValue.ToString();
            var flowType = _flowTypes.FirstOrDefault(x =>
                string.Equals(x._id, selectedValue, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(x.FlowTypeName, selectedValue, StringComparison.OrdinalIgnoreCase));

            return flowType != null ? flowType.FlowTypeName : selectedValue;
        }

        private ChartModel GetSelectedChart()
        {
            var selectedChart = cboAccountNumber.EditValue as ChartModel;
            if (selectedChart != null)
            {
                return selectedChart;
            }

            var selectedValue = cboAccountNumber.EditValue as string;
            if (string.IsNullOrWhiteSpace(selectedValue))
            {
                return null;
            }

            return _charts.FirstOrDefault(x =>
                string.Equals(x._id, selectedValue, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(x.AccountNumber, selectedValue, StringComparison.OrdinalIgnoreCase));
        }
    }
}