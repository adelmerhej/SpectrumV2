using DevExpress.XtraEditors;
using Spectrum.DataLayers.Accounting.FinancialGroup;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Accounting.FinancialGroup;
using Spectrum.Utilities;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.Accounting.FinancialGroup
{
    public partial class FinancialGroupEditForm : XtraForm
    {
        private FinancialGroupModel _financialGroupModel = new FinancialGroupModel();

        private readonly FinancialGroupRepository _financialGroupRepository = new FinancialGroupRepository(DatabaseFactory.ProfilePrimary);

        private readonly LogInfoRepository _logInfoRepository = new LogInfoRepository();

        //init permission variables
        private bool _canEdit = true;
        private bool _isAdmin = true;
        private bool _isProtected = true;


        public EventHandler SendUpdatedFinancialGroup;
        public FinancialGroupEditForm(FinancialGroupModel model)
        {
            InitializeComponent();

            _financialGroupModel = model ?? new FinancialGroupModel();
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
            bsFinancialGroup.DataSource = _financialGroupModel;
        }

        private async Task ApplyDefaults()
        {
            var defaultFinancialGroup = await _financialGroupRepository.GetDefaultFinancialGroupAsync(_financialGroupModel._id);
            chkIsDefault.Enabled = defaultFinancialGroup == null;
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
                BindingContext[bsFinancialGroup].EndCurrentEdit();
                _financialGroupModel = (FinancialGroupModel)bsFinancialGroup.Current;

                if (_financialGroupModel.IsDefault)
                {
                    var defaultFinancialGroup = await _financialGroupRepository.GetDefaultFinancialGroupAsync(_financialGroupModel._id);
                    if (defaultFinancialGroup != null)
                    {
                        chkIsDefault.Enabled = false;
                        XtraMessageBox.Show("A default flow type already exists. Only one record can be marked as default.",
                            @"Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }

                if (string.IsNullOrEmpty(_financialGroupModel._id))
                {
                    _logInfoRepository.CreateLogInfo(_financialGroupModel);

                    var newId = await _financialGroupRepository.AddNewFinancialGroupAsync(_financialGroupModel);
                }
                else
                {
                    _logInfoRepository.UpdateLogInfo(_financialGroupModel);
                    await _financialGroupRepository.UpdateFinancialGroupAsync(_financialGroupModel);
                }

                SendUpdatedFinancialGroup(_financialGroupModel, EventArgs.Empty);
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