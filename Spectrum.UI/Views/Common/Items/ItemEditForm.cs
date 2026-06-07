using DevExpress.XtraEditors;
using Spectrum.DataLayers.Common.Items;
using Spectrum.DataLayers.Common.Services;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Common.Items;
using Spectrum.Models.Common.Services;
using Spectrum.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.Common.Items
{
    public partial class ItemEditForm : XtraForm
    {
        private ItemModel _itemModel;

        private ServiceModel _serviceModel = new ServiceModel();
        private IList<ServiceModel> _services = new List<ServiceModel>();

        private readonly ItemRepository _itemRepository = new ItemRepository(DatabaseFactory.ProfilePrimary);
        private readonly ServiceRepository _serviceRepository = new ServiceRepository(DatabaseFactory.ProfilePrimary);
        private readonly LogInfoRepository _logInfoRepository = new LogInfoRepository();

        //init permission variables
        private bool _canEdit;
        private bool _isAdmin;
        private bool _isProtected;

        public EventHandler SendUpdatedItem;

        public ItemEditForm(ItemModel model)
        {
            InitializeComponent();

            _itemModel = model ?? new ItemModel();

            StartLoading();
        }

        private async void StartLoading()
        {
            try
            {
                await InitializeBindings();
                WireUpBindings();
                ApplyDefaults();
                ApplyPermissions();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, @"Error StartLoading", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

                _services = await _serviceRepository.GetServicesAsync();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WireUpBindings()
        {
            bsItem.DataSource = _itemModel;

            cboServices.Properties.DataSource = null;
            cboServices.Properties.DataSource = _services;
        }

        private async void ApplyDefaults()
        {
            var defaultItem = await _itemRepository.GetDefaultItemAsync(_itemModel._id);
            chkIsDefault.Enabled = defaultItem == null;

            var defaultService = await _serviceRepository.GetDefaultServiceAsync(_itemModel._id);
            if (defaultService != null)
            {
                _itemModel.Service = defaultService.ServiceName;
            }

        }

        private void ApplyPermissions()
        {
            _canEdit = true;
            _isAdmin = true;
            _isProtected = true;

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
            try
            {
                if (!ValidateData()) return;

                BindingContext[bsItem].EndCurrentEdit();
                _itemModel = (ItemModel)bsItem.Current;


                if (string.IsNullOrEmpty(_itemModel._id))
                {
                    _logInfoRepository.CreateLogInfo(_itemModel);

                    _ = await _itemRepository.AddNewItemAsync(_itemModel);
                }
                else
                {
                    _logInfoRepository.UpdateLogInfo(_itemModel);

                    await _itemRepository.UpdateItemAsync(_itemModel);
                }

                SendUpdatedItem(_itemModel, EventArgs.Empty);
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

            if (cboServices.Text == "")
            {
                messageNumber += 1;
                validateMessage.Append("\n- Service cannot be empty.");
                validateReturnValue = false;
                cboServices.Focus();
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