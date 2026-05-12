using DevExpress.XtraEditors;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Utilities;
using Spectrum.DataLayers.Accounting.JournalType;
using Spectrum.Models.Accounting.JournalType;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.Accounting.JournalType
{
    public partial class JournalTypeEditForm : XtraForm
    {
        private JournalTypeModel _journalTypeModel = new JournalTypeModel();

        private readonly JournalTypeRepository _journalTypeRepository = new JournalTypeRepository(DatabaseFactory.ProfilePrimary);

        private readonly LogInfoRepository _logInfoRepository = new LogInfoRepository();

        //init permission variables
        private bool _canEdit = true;
        private bool _isAdmin = true;
        private bool _isProtected = true;

        public EventHandler SendUpdatedJournalType;

        public JournalTypeEditForm(JournalTypeModel model)
        {
            InitializeComponent();

            _journalTypeModel = model ?? new JournalTypeModel();
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
            bsJournalType.DataSource = _journalTypeModel;
        }

        private async Task ApplyDefaults()
        {
            var defaultJournalType = await _journalTypeRepository.GetDefaultJournalTypeAsync(_journalTypeModel._id);
            chkIsDefault.Enabled = defaultJournalType == null;
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
                BindingContext[bsJournalType].EndCurrentEdit();
                _journalTypeModel = (JournalTypeModel)bsJournalType.Current;

                if (_journalTypeModel.IsDefault)
                {
                    var defaultJournalType = await _journalTypeRepository.GetDefaultJournalTypeAsync(_journalTypeModel._id);
                    if (defaultJournalType != null)
                    {
                        chkIsDefault.Enabled = false;
                        XtraMessageBox.Show("A default journal type already exists. Only one record can be marked as default.",
                            @"Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }

                if (string.IsNullOrEmpty(_journalTypeModel._id))
                {
                    _logInfoRepository.CreateLogInfo(_journalTypeModel);

                    var newId = await _journalTypeRepository.AddNewJournalTypeAsync(_journalTypeModel);
                }
                else
                {
                    _logInfoRepository.UpdateLogInfo(_journalTypeModel);
                    await _journalTypeRepository.UpdateJournalTypeAsync(_journalTypeModel);
                }

                SendUpdatedJournalType(_journalTypeModel, EventArgs.Empty);
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

            if (txtCode.Text == "")
            {
                messageNumber += 1;
                validateMessage.Append("\n- Code cannot be empty.");
                validateReturnValue = false;
                txtCode.Focus();
            }

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