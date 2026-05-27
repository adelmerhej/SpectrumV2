using DevExpress.XtraEditors;
using Spectrum.DataLayers.DataAccess;
using Spectrum.DataLayers.Projects.Settings.Addendum;
using Spectrum.Models.Projects;
using Spectrum.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.Projects.Settings.Addendum
{
    public partial class AddendumEditForm : XtraForm
    {
        private AddendumModel _addendumModel = new AddendumModel();
        private readonly IList<AddendumModel> _projectAddendums = new List<AddendumModel>();

        private readonly AddendumRepository _addendumRepository = new AddendumRepository(DatabaseFactory.ProfilePrimary);

        private readonly LogInfoRepository _logInfoRepository = new LogInfoRepository();

        //init permission variables
        private bool _canEdit = true;
        private bool _isAdmin = true;
        private bool _isProtected = true;

        public EventHandler SendUpdatedAddendum;

        public AddendumEditForm(AddendumModel model, IEnumerable<AddendumModel> projectAddendums = null)
        {
            InitializeComponent();

            _addendumModel = model ?? new AddendumModel();
            _projectAddendums = (projectAddendums ?? Enumerable.Empty<AddendumModel>()).ToList();

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
            bsAddendum.DataSource = _addendumModel;
        }

        private async Task ApplyDefaults()
        {
            var defaultAddendum = await _addendumRepository.GetDefaultAddendumAsync(_addendumModel._id);
            chkIsDefault.Enabled = defaultAddendum == null;

            if (string.IsNullOrWhiteSpace(_addendumModel._id))
            {
                _addendumModel.Sequence = _projectAddendums
                    .Where(x => x != null)
                    .Select(x => x.Sequence)
                    .DefaultIfEmpty(0)
                    .Max() + 1;

                dtBODDate.EditValue = DateTime.Now;
                chkActive.Checked = true;
                chkIsDefault.Checked = defaultAddendum == null;
                bsAddendum.ResetBindings(false);
            }
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
                BindingContext[bsAddendum].EndCurrentEdit();
                _addendumModel = (AddendumModel)bsAddendum.Current;

                if (_addendumModel.IsDefault)
                {
                    var defaultAddendum = await _addendumRepository.GetDefaultAddendumAsync(_addendumModel._id);
                    if (defaultAddendum != null)
                    {
                        chkIsDefault.Enabled = false;
                        XtraMessageBox.Show("A default addendum already exists. Only one record can be marked as default.",
                            @"Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }

                if (string.IsNullOrEmpty(_addendumModel._id))
                {
                    _logInfoRepository.CreateLogInfo(_addendumModel);

                    var newId = await _addendumRepository.AddNewAddendumAsync(_addendumModel);
                }
                else
                {
                    _logInfoRepository.UpdateLogInfo(_addendumModel);

                    await _addendumRepository.UpdateAddendumAsync(_addendumModel);
                }

                SendUpdatedAddendum(_addendumModel, EventArgs.Empty);
                Close();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, @"Error Saving addendum", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            if (txtSequence.Text == "")
            {
                messageNumber += 1;
                validateMessage.Append("\n- Sequence cannot be empty.");
                validateReturnValue = false;
                txtSequence.Focus();
            }

            if (txtReference.Text == "")
            {
                messageNumber += 1;
                validateMessage.Append("\n- Reference cannot be empty.");
                validateReturnValue = false;
                txtReference.Focus();
            }

            if (txtSubject.Text == "")
            {
                messageNumber += 1;
                validateMessage.Append("\n- Subject cannot be empty.");
                validateReturnValue = false;
                txtSubject.Focus();
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