using DevExpress.XtraEditors;
using Spectrum.DataLayers.DataAccess;
using Spectrum.DataLayers.Members.Funders;
using Spectrum.Models.Members.Funders;
using Spectrum.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.Members.Funders
{
    public partial class FunderEditForm : XtraForm
    {
        private FunderModel _funderModel = new FunderModel();

        private FunderTypeModel _funderTypeModel = new FunderTypeModel();
        private IList<FunderTypeModel> _funderTypes = new List<FunderTypeModel>();

        private readonly FunderRepository _funderRepository = new FunderRepository(DatabaseFactory.ProfilePrimary);
        private readonly FunderTypeRepository _funderTypeRepository = new FunderTypeRepository(DatabaseFactory.ProfilePrimary);

        private readonly LogInfoRepository _logInfoRepository = new LogInfoRepository();

        //init permission variables
        private bool _canEdit = true;
        private bool _isAdmin = true;
        private bool _isProtected = true;

        public EventHandler SendUpdatedFunder;

        public FunderEditForm(FunderModel model)
        {
            InitializeComponent();

            _funderModel = model ?? new FunderModel();

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

                _funderTypes = await _funderTypeRepository.GetFunderTypesAsync();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WireUpBindings()
        {
            bsFunder.DataSource = _funderModel;

            cboFundersType.Properties.DataSource = null;
            cboFundersType.Properties.DataSource = _funderTypes;
        }

        private async Task ApplyDefaults()
        {
            var defaultFunder = await _funderRepository.GetDefaultFunderAsync(_funderModel._id);
            chkIsDefault.Enabled = defaultFunder == null;
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
                BindingContext[bsFunder].EndCurrentEdit();
                _funderModel = (FunderModel)bsFunder.Current;

                if (_funderModel.IsDefault)
                {
                    var defaultFunder = await _funderRepository.GetDefaultFunderAsync(_funderModel._id);
                    if (defaultFunder != null)
                    {
                        chkIsDefault.Enabled = false;
                        XtraMessageBox.Show("A default funder already exists. Only one record can be marked as default.",
                            @"Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }

                if (string.IsNullOrEmpty(_funderModel._id))
                {
                    _logInfoRepository.CreateLogInfo(_funderModel);

                    var newId = await _funderRepository.AddNewFunderAsync(_funderModel);
                }
                else
                {
                    _logInfoRepository.UpdateLogInfo(_funderModel);

                    await _funderRepository.UpdateFunderAsync(_funderModel);
                }

                SendUpdatedFunder(_funderModel, EventArgs.Empty);
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