using DevExpress.XtraEditors;
using Spectrum.DataLayers.Common.Countries;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Common.Countries;
using Spectrum.Utilities;
using SpectrumV1.DataLayers.Accounting.Banks;
using SpectrumV1.Models.Accounting.Banks;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.Accounting.Banks
{
    public partial class BankEditForm : XtraForm
    {
        private BankModel _bankModel = new BankModel();

        private readonly BankRepository _bankRepository = new BankRepository(DatabaseFactory.ProfilePrimary);

        private readonly LogInfoRepository _logInfoRepository = new LogInfoRepository();

        //init permission variables
        private bool _canEdit = true;
        private bool _isAdmin = true;
        private bool _isProtected = true;

        public EventHandler SendUpdatedBank;

        public BankEditForm(BankModel model)
        {
            InitializeComponent();


            _bankModel = model;

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
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WireUpBindings()
        {
            bsBank.DataSource = _bankModel;
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

            btnSave.Enabled = _isAdmin || _canEdit;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateData()) return;

            try
            {
                BindingContext[bsBank].EndCurrentEdit();
                _bankModel = (BankModel)bsBank.Current;


                if (string.IsNullOrEmpty(_bankModel._id))
                {
                    _logInfoRepository.CreateLogInfo(_bankModel);

                    var newId = await _bankRepository.AddNewBankAsync(_bankModel);
                }
                else
                {
                    _logInfoRepository.UpdateLogInfo(_bankModel);

                    await _bankRepository.UpdateBankAsync(_bankModel);
                }

                SendUpdatedBank(_bankModel, EventArgs.Empty);
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