using DevExpress.XtraEditors;
using Spectrum.DataLayers.Accounting.Charts;
using Spectrum.DataLayers.Accounting.JournalType;
using Spectrum.DataLayers.Accounting.Reports.StatementReports;
using Spectrum.DataLayers.Common.Currencies;
using Spectrum.DataLayers.DataAccess;
using Spectrum.DataLayers.Users;
using Spectrum.Models.Accounting.Charts;
using Spectrum.Models.Accounting.JournalType;
using Spectrum.Models.Common.Currencies;
using Spectrum.Models.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.Accounting.Reports.Ledgers
{
    public partial class TrialBalanceForm : XtraForm
    {
        private const string _formName = "TrialBalanceForm";
        private int _formId;
        private bool _resetMenu;

        private IList<ChartModel> _charts = new List<ChartModel>();
        private IList<JournalTypeModel> _journalTypes = new List<JournalTypeModel>();
        private IList<CurrencyModel> _currencies = new List<CurrencyModel>();

        private readonly ChartRepository _chartRepository = new ChartRepository(DatabaseFactory.ProfilePrimary);
        private readonly JournalTypeRepository _journalTypeRepository = new JournalTypeRepository(DatabaseFactory.ProfilePrimary);
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

        public TrialBalanceForm()
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
                _journalTypes = await _journalTypeRepository.GetJournalTypesAsync();
                _currencies = await _currencyRepository.GetCurrenciesAsync();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WireUpBindings()
        {
            cboAccountNumberFrom.Properties.DataSource = null;
            cboAccountNumberFrom.Properties.DataSource = _charts;

            cboAccountNumberTo.Properties.DataSource = null;
            cboAccountNumberTo.Properties.DataSource = _charts;

            cboJournalTypes.Properties.DataSource = null;
            cboJournalTypes.Properties.DisplayMember = nameof(JournalTypeModel.Code);
            cboJournalTypes.Properties.ValueMember = nameof(JournalTypeModel._id);
            cboJournalTypes.Properties.DataSource = _journalTypes;

            cboCurrencies.Properties.DataSource = null;
            cboCurrencies.Properties.DisplayMember = nameof(CurrencyModel.CurrencyCode);
            cboCurrencies.Properties.ValueMember = nameof(CurrencyModel._id);
            cboCurrencies.Properties.DataSource = _currencies;
        }

        private void ApplyDefaults()
        {
            dtDateFrom.EditValue = new DateTime(DateTime.Now.Year, 1, 1);
            dtDateTo.EditValue = DateTime.Now.Date;
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;
            Close();
        }

        #endregion


        private bool ValidateForm()
        {
            var validateReturnValue = true;
            var messageNumber = 0;
            var validateMessage = new StringBuilder();

            if (cboAccountNumberFrom.Text == "")
            {
                messageNumber += 1;
                validateMessage.Append("\n- Account Number From cannot be empty.");
                validateReturnValue = false;
                cboAccountNumberFrom.Focus();
            }

            if (cboAccountNumberTo.Text == "")
            {
                messageNumber += 1;
                validateMessage.Append("\n- Account Number To cannot be empty.");
                validateReturnValue = false;
                cboAccountNumberTo.Focus();
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