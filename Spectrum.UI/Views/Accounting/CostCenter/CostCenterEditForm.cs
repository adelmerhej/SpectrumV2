using DevExpress.XtraEditors;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Utilities;
using Spectrum.DataLayers.Accounting.CostCenter;
using Spectrum.Models.Accounting.CostCenter;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.Accounting.CostCenter
{
    public partial class CostCenterEditForm : XtraForm
    {
        private CostCenterModel _costCenterModel = new CostCenterModel();

        private readonly CostCenterRepository _costCenterRepository = new CostCenterRepository(DatabaseFactory.ProfilePrimary);

        private readonly LogInfoRepository _logInfoRepository = new LogInfoRepository();

        //init permission variables
        private bool _canEdit = true;
        private bool _isAdmin = true;
        private bool _isProtected = true;

        public EventHandler SendUpdatedCostCenter;

        public CostCenterEditForm(CostCenterModel model)
        {
            InitializeComponent();

            _costCenterModel = model ?? new CostCenterModel();

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
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WireUpBindings()
        {
            bsCostCenter.DataSource = _costCenterModel;
        }

        private async Task ApplyDefaults()
        {
            var defaultCostCenter = await _costCenterRepository.GetDefaultCostCenterAsync(_costCenterModel._id);
            chkIsDefault.Enabled = defaultCostCenter == null;
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
                BindingContext[bsCostCenter].EndCurrentEdit();
                _costCenterModel = (CostCenterModel)bsCostCenter.Current;

                if (_costCenterModel.IsDefault)
                {
                    var defaultCostCenter = await _costCenterRepository.GetDefaultCostCenterAsync(_costCenterModel._id);
                    if (defaultCostCenter != null)
                    {
                        chkIsDefault.Enabled = false;
                        XtraMessageBox.Show("A default cost center already exists. Only one record can be marked as default.",
                            @"Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }

                if (string.IsNullOrEmpty(_costCenterModel._id))
                {
                    _logInfoRepository.CreateLogInfo(_costCenterModel);

                    var newId = await _costCenterRepository.AddNewCostCenterAsync(_costCenterModel);
                }
                else
                {
                    _logInfoRepository.UpdateLogInfo(_costCenterModel);
                    await _costCenterRepository.UpdateCostCenterAsync(_costCenterModel);
                }

                SendUpdatedCostCenter(_costCenterModel, EventArgs.Empty);
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

            if (!validateReturnValue)
            {
                validateMessage.Insert(0, "The following need your attention:");
                if (messageNumber > 1) validateMessage.Replace("following", "followings");
                XtraMessageBox.Show(validateMessage + " \nPlease try again.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return validateReturnValue;
        }
    }
}