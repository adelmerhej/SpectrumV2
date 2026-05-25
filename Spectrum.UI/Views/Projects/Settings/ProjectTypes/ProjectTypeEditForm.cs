using DevExpress.XtraEditors;
using Spectrum.DataLayers.DataAccess;
using Spectrum.DataLayers.Projects.Settings.ProjectTypes;
using Spectrum.Models.Operations.Projects.Settings.ProjectTypes;
using Spectrum.Utilities;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.Projects.Settings.ProjectTypes
{
    public partial class ProjectTypeEditForm : XtraForm
    {
        private ProjectTypeModel _projectTypeModel = new ProjectTypeModel();

        private readonly ProjectTypeRepository _projectTypeRepository = new ProjectTypeRepository(DatabaseFactory.ProfilePrimary);

        private readonly LogInfoRepository _logInfoRepository = new LogInfoRepository();

        //init permission variables
        private bool _canEdit = true;
        private bool _isAdmin = true;
        private bool _isProtected = true;

        public EventHandler SendUpdatedProjectType;

        public ProjectTypeEditForm(ProjectTypeModel model)
        {
            InitializeComponent();

            _projectTypeModel = model ?? new ProjectTypeModel();

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
            bsProjectType.DataSource = _projectTypeModel;
        }

        private async Task ApplyDefaults()
        {
            var defaultProjectType = await _projectTypeRepository.GetDefaultProjectTypeAsync(_projectTypeModel._id);
            chkIsDefault.Enabled = defaultProjectType == null;
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
                BindingContext[bsProjectType].EndCurrentEdit();
                _projectTypeModel = (ProjectTypeModel)bsProjectType.Current;

                if (_projectTypeModel.IsDefault)
                {
                    var defaultProjectType = await _projectTypeRepository.GetDefaultProjectTypeAsync(_projectTypeModel._id);
                    if (defaultProjectType != null)
                    {
                        chkIsDefault.Enabled = false;
                        XtraMessageBox.Show("A default project type already exists. Only one record can be marked as default.",
                            @"Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }

                if (string.IsNullOrEmpty(_projectTypeModel._id))
                {
                    _logInfoRepository.CreateLogInfo(_projectTypeModel);

                    var newId = await _projectTypeRepository.AddNewProjectTypeAsync(_projectTypeModel);
                }
                else
                {
                    _logInfoRepository.UpdateLogInfo(_projectTypeModel);

                    await _projectTypeRepository.UpdateProjectTypeAsync(_projectTypeModel);
                }

                SendUpdatedProjectType(_projectTypeModel, EventArgs.Empty);
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