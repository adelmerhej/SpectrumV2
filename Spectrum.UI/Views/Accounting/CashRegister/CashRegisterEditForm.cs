using DevExpress.XtraCharts.Designer.Native;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using Spectrum.DataLayers.Accounting.CashRegister;
using Spectrum.DataLayers.Accounting.Charts;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Accounting.CashRegister;
using Spectrum.Models.Accounting.Charts;
using Spectrum.Models.Common.Countries;
using Spectrum.Models.Users;
using Spectrum.Utilities;
using Spectrum.Utilities.Enums;
using Spectrum.Views.Accounting.Charts;
using Spectrum.Views.Common.Countries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.Accounting.CashRegister
{
    public partial class CashRegisterEditForm : XtraForm
    {
        private CashRegisterModel _cashRegisterModel = new CashRegisterModel();
        private IList<ChartModel> _charts = new List<ChartModel>();

        private readonly CashRegisterRepository _cashRegisterRepository = new CashRegisterRepository(DatabaseFactory.ProfilePrimary);
        private readonly ChartRepository _chartRepository = new ChartRepository(DatabaseFactory.ProfilePrimary);

        private readonly LogInfoRepository _logInfoRepository = new LogInfoRepository();

        //init permission variables
        private bool _canEdit = true;
        private bool _isAdmin = true;
        private bool _isProtected = true;

        public EventHandler SendUpdatedCashRegister;

        public CashRegisterEditForm(CashRegisterModel model)
        {
            InitializeComponent();

            _cashRegisterModel = model ?? new CashRegisterModel();
            StartLoading();
        }

        private async void StartLoading()
        {
            await InitializeBindings();
            WireUpBindings();
            await ApplyDefaults();
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

                _charts = await _chartRepository.GetChartsAsync(ChartType.R.ToString());
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WireUpBindings()
        {
            bsCashRegister.DataSource = _cashRegisterModel;

            cboCharts.Properties.DataSource = null;
            cboCharts.Properties.DataSource = _charts;

            //UpdateAccountNameFromSelectedChart();
        }

        private void cboCharts_EditValueChanged(object sender, EventArgs e)
        {
            UpdateAccountNameFromSelectedChart();
        }

        private async void UpdateAccountNameFromSelectedChart()
        {
            var selectedChartId = cboCharts.EditValue as string;

            if (string.IsNullOrWhiteSpace(selectedChartId) || selectedChartId == null) return;

            //ChartModel selectedChart = null;
            //foreach (var chart in _charts)
            //{
            //    if (chart._id == selectedChartId)
            //    {
            //        selectedChart = chart;
            //        break;
            //    }
            //}

            ChartModel selectedChart = await _chartRepository.GetChartByAccountNumber(selectedChartId);

            var accountName = selectedChart != null ? selectedChart.AccountName : string.Empty;
            txtAccountName.EditValue = accountName;
            _cashRegisterModel.AccountName = accountName;
        }

        private async Task ApplyDefaults()
        {
            var defaultCashRegister = await _cashRegisterRepository.GetDefaultCashRegisterAsync(_cashRegisterModel._id);
            chkIsDefault.Enabled = defaultCashRegister == null;
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



        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateData()) return;

            try
            {
                BindingContext[bsCashRegister].EndCurrentEdit();
                _cashRegisterModel = (CashRegisterModel)bsCashRegister.Current;

                if (_cashRegisterModel.IsDefault)
                {
                    var defaultCashRegister = await _cashRegisterRepository.GetDefaultCashRegisterAsync(_cashRegisterModel._id);
                    if (defaultCashRegister != null)
                    {
                        chkIsDefault.Enabled = false;
                        XtraMessageBox.Show("A default cash register already exists. Only one record can be marked as default.",
                            @"Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }

                if (string.IsNullOrEmpty(_cashRegisterModel._id))
                {
                    _logInfoRepository.CreateLogInfo(_cashRegisterModel);

                    var newId = await _cashRegisterRepository.AddNewCashRegisterAsync(_cashRegisterModel);
                }
                else
                {
                    _logInfoRepository.UpdateLogInfo(_cashRegisterModel);
                    await _cashRegisterRepository.UpdateCashRegisterAsync(_cashRegisterModel);
                }

                SendUpdatedCashRegister(_cashRegisterModel, EventArgs.Empty);
                Close();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, @"Error Saving user", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private bool ValidateData()
        {
            var validateReturnValue = true;
            var messageNumber = 0;
            var validateMessage = new StringBuilder();

            if (txtName.Text == "")
            {
                messageNumber += 1;
                validateMessage.Append("\n- Name cannot be empty.");
                validateReturnValue = false;
                txtName.Focus();
            }

            if (cboCharts.Text == "")
            {
                messageNumber += 1;
                validateMessage.Append("\n- Account Ledger cannot be empty.");
                validateReturnValue = false;
                cboCharts.Focus();
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

        private async void cboCharts_AddNewValue(object sender, AddNewValueEventArgs e)
        {
            ChartEditForm frm = new ChartEditForm(new ChartModel());
            frm.SendUpdatedChartAccount += RcvUpdatedChartAsync;
            frm.ShowDialog();
        }

        private async void RcvUpdatedChartAsync(object sender, EventArgs e)
        {
            if (sender == null) return;
            ChartModel chartModel = sender as ChartModel;

            _charts.Add(chartModel);

            cboCharts.Properties.DataSource = null;
            cboCharts.Properties.DataSource = _charts;
            if (chartModel != null) cboCharts.EditValue = chartModel._id;
        }

    }
}