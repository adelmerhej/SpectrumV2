using DevExpress.XtraEditors;
using Spectrum.DataLayers.Accounting.Charts;
using Spectrum.DataLayers.Accounting.Reports.StatementReports;
using Spectrum.DataLayers.Common.Currencies;
using Spectrum.DataLayers.DataAccess;
using Spectrum.DataLayers.Users;
using Spectrum.Models.Accounting.Charts;
using Spectrum.Models.Accounting.Reports.StatementReports;
using Spectrum.Models.Common.Currencies;
using Spectrum.Models.Users;
using Spectrum.Reports.Accounting.StatementReports;
using Spectrum.Views.Reports;
using System;
using System.Collections.Generic;
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

        private readonly ChartRepository _chartRepository = new ChartRepository(DatabaseFactory.ProfilePrimary);
        private readonly CurrencyRepository _currencyRepository = new CurrencyRepository(DatabaseFactory.ProfilePrimary);
        private readonly StatementOfAccountRepository _statementOfAccountRepository = new StatementOfAccountRepository(DatabaseFactory.ProfilePrimary);

        /// <summary>
        /// User Permission Role
        /// </summary>
        private IList<UserPermissionModel> _userPermission = new List<UserPermissionModel>();
        private readonly UserPermissionRepository _userPermissionRepository = new UserPermissionRepository();

        //Init permission variables
        private bool _isAdmin;
        private bool _isProtected;

        public StatementOfAccount()
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

                _charts = await _chartRepository.GetChartsAsync();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WireUpBindings()
        {
            cboAccountNumber.Properties.DataSource = null;
            cboAccountNumber.Properties.DataSource = _charts;
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

        }

        #region Buttons Event

        private async void btnPrint_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;
            //Prepare variables
            DateTime? dateFrom = null;
            DateTime? dateTo = null;
            if (dtDateFrom.EditValue != null) dateFrom = DateTime.Parse(dtDateFrom.EditValue.ToString());
            if (dtDateTo.EditValue != null) dateTo = DateTime.Parse(dtDateTo.EditValue.ToString());

            string chartId = "0";
            if (cboAccountNumber.EditValue != null) chartId = cboAccountNumber.EditValue.ToString();

            string currencyId = "0";
            if (cboCurrencies.EditValue != null) currencyId = cboCurrencies.EditValue.ToString();

            string journalTypes = "0";
            if (cboJournalTypes.EditValue != null) journalTypes = cboJournalTypes.EditValue.ToString();

            bool withPreviousBalance = chkWithPreviousBalance.Checked;
            bool showDetailed = chkShowDetailed.Checked;

            int currencyOut = rgCurrencyOut.SelectedIndex;

            int workingYear = !chkConsolidatePastPeriods.Checked ? DateTime.Now.Year : 0;
            var isProtected = chkShowProtected.Checked;

            IList<StatementOfAccountReportModel> dataReportModels =
                await _statementOfAccountRepository.StatementOfAccountReportAsync(dateFrom, dateTo, chartId, currencyId, workingYear, isProtected);

            ChartModel chartAccount = await _chartRepository.GetChartByIdAsync(chartId);

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
    }
}