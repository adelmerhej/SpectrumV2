using DevExpress.XtraEditors;
using Spectrum.DataLayers.Accounting.Charts;
using Spectrum.DataLayers.Common.Currencies;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Accounting.Charts;
using Spectrum.Models.Common.Currencies;
using Spectrum.Models.Users;
using Spectrum.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.Accounting.Charts
{
    public partial class ChartEditForm : XtraForm
    {
        private ChartModel _chartModel = new ChartModel();
        private IList<ChartDetailModel> _chartDetails = new List<ChartDetailModel>();
        private IList<CurrencyModel> _currencies = new List<CurrencyModel>();

        private readonly ChartRepository _chartRepository = new ChartRepository(DatabaseFactory.ProfilePrimary);
        private readonly ChartDetailRepository _chartDetailRepository = new ChartDetailRepository(DatabaseFactory.ProfilePrimary);
        private readonly CurrencyRepository _currencyRepository = new CurrencyRepository(DatabaseFactory.ProfilePrimary);

        private readonly LogInfoRepository _logInfoRepository = new LogInfoRepository();

        //init permission variables
        private bool _canEdit = true;
        private bool _isAdmin = true;
        private bool _isProtected = true;

        public EventHandler SendUpdatedChartAccount;

        public ChartEditForm(ChartModel model)
        {
            InitializeComponent();

            _chartModel = model;

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

                await LoadTitlesAsync();
                //_chartDetails = await _chartDetailRepository.GetChartDetailsByChartIdAsync(_chartModel._id);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WireUpBindings()
        {
            bsChart.DataSource = _chartModel;
        }

        private void ApplyDefaults()
        {
            ApplyNumberAndAccountTypeRules(IsNewChart());
        }
        private async Task LoadTitlesAsync()
        {
            HelperApplication.InitAccountTypeComboBox(cboAccountType.Properties);
            await Task.CompletedTask;
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

            btnSave.Enabled = _isAdmin || _canEdit;
        }

        #region Buttons Event Handlers

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateData()) return;

            try
            {
                BindingContext[bsChart].EndCurrentEdit();
                _chartModel = (ChartModel)bsChart.Current;


                if (string.IsNullOrEmpty(_chartModel._id))
                {
                    _logInfoRepository.CreateLogInfo(_chartModel);

                    var newId = await _chartRepository.AddNewChartAsync(_chartModel);
                }
                else
                {
                    _logInfoRepository.UpdateLogInfo(_chartModel);

                    await _chartRepository.UpdateChartAsync(_chartModel);
                }

                SendUpdatedChartAccount?.Invoke(_chartModel, EventArgs.Empty);
                Close();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, @"Error Saving user", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        private bool ValidateData()
        {
            ApplyNumberAndAccountTypeRules(IsNewChart());

            var validateReturnValue = true;
            var messageNumber = 0;
            var validateMessage = new StringBuilder();

            if (txtNumber.Text == "")
            {
                messageNumber += 1;
                validateMessage.Append("\n- Number cannot be empty.");
                validateReturnValue = false;
                txtNumber.Focus();
            }

            if (txtSerial.Text == "" && cboAccountType.Text == "R")
            {
                messageNumber += 1;
                validateMessage.Append("\n- Serial cannot be empty if type is Regular.");
                validateReturnValue = false;
                txtSerial.Focus();
            }

            if (txtAccountName.Text == "")
            {
                messageNumber += 1;
                validateMessage.Append("\n- Account Name cannot be empty.");
                validateReturnValue = false;
                txtAccountName.Focus();
            }

            if (cboAccountType.Text == "")
            {
                messageNumber += 1;
                validateMessage.Append("\n- Account Type cannot be empty.");
                validateReturnValue = false;
                cboAccountType.Focus();
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

        private void txtSerial_EditValueChanged(object sender, EventArgs e)
        {
            if (txtSerial.Text != "")
            {
                var paddedSerial = txtSerial.Text.PadLeft(5, '0');
                if (txtSerial.Text != paddedSerial)
                {
                    txtSerial.Text = paddedSerial;
                    return;
                }
            }

            UpdateAccountNumber();
        }

        private void cboAccountType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyNumberAndAccountTypeRules(IsNewChart());
        }

        private void NewSerial()
        {
            if (txtNumber.Text.Trim().Length < 4)
            {
                txtSerial.Text = "";
                return;
            }

            txtSerial.Text = _chartRepository.GetNewSerial(txtNumber.Text, CurrentUser.Company);
        }

        private void txtNumber_EditValueChanged(object sender, EventArgs e)
        {
            ApplyNumberAndAccountTypeRules(IsNewChart());
        }

        private bool IsNewChart()
        {
            return string.IsNullOrEmpty(_chartModel._id) || _chartModel._id == "0";
        }

        private void ApplyNumberAndAccountTypeRules(bool getNewSerial)
        {
            var number = txtNumber.Text == null ? string.Empty : txtNumber.Text.Trim();

            if (number.Length < 4)
            {
                if (txtSerial.Text != string.Empty)
                    txtSerial.Text = string.Empty;

                if (cboAccountType.Text != "T")
                    cboAccountType.Text = "T";
            }

            txtSerial.Enabled = cboAccountType.Text == "R";

            if (cboAccountType.Text == "T")
            {
                if (txtSerial.Text != string.Empty)
                    txtSerial.Text = string.Empty;
            }
            else if (getNewSerial && number.Length >= 4 && cboAccountType.Text == "R")
            {
                NewSerial();
            }

            UpdateAccountNumber();
        }

        private void UpdateAccountNumber()
        {
            var number = txtNumber.Text == null ? string.Empty : txtNumber.Text.Trim();
            var serial = txtSerial.Text == null ? string.Empty : txtSerial.Text.Trim();

            txtAccountNumber.Text = string.IsNullOrEmpty(serial)
                ? number
                : $"{number} {serial}";
        }
    }
}