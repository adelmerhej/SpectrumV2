using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using SpectrumV1.DataLayers.Members.Clients;
using SpectrumV1.Models.Members.Clients;
using SpectrumV1.Utilities;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpectrumV1.Views.Members.Clients
{
	public partial class ClientEditForm : RibbonForm
	{
		private ClientModel _clientModel = new ClientModel();

		private readonly ClientRepository _clientRepository = new ClientRepository();

		private readonly LogInfoRepository _logInfoRepository = new LogInfoRepository();

		//Init permissionvariables
		private bool _canAdd = true;
		private bool _canEdit = true;
		private bool _canDelete = true;
		private bool _canPrint = true;
		private bool _isAdmin = true;
		private bool _isProtected = true;

		public EventHandler SendUpdatedClient;

		public ClientEditForm(ClientModel model)
		{
			InitializeComponent();

			_clientModel = model;

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
			bsClient.DataSource = _clientModel;
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

			btnNew.Enabled = _isAdmin || _canAdd;
			btnSave.Enabled = _isAdmin || _canEdit;
			btnSaveAndClose.Enabled = _isAdmin || _canEdit;
			btnPrint.Enabled = _isAdmin || _canPrint;
			btnDelete.Enabled = _isAdmin || _canDelete;
		}


		#region Buttons Events

		private void btnNew_ItemClick(object sender, ItemClickEventArgs e)
		{
			_clientModel = new ClientModel();
			StartLoading();
		}

		private void btnSave_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!ValidateData()) return;
			SaveData();
		}

		private void btnSaveAndClose_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!ValidateData()) return;
			SaveData();
			Close();
		}

		private void btnRefresh_ItemClick(object sender, ItemClickEventArgs e)
		{
			StartLoading();
		}

		private void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
		{

		}

		private void btnPrint_ItemClick(object sender, ItemClickEventArgs e)
		{

		}

		private void btnMeeting_ItemClick(object sender, ItemClickEventArgs e)
		{

		}

		private void btnClose_ItemClick(object sender, ItemClickEventArgs e)
		{
			Close();
		}

		#endregion

		private async void SaveData()
		{
			try
			{
				BindingContext[bsClient].EndCurrentEdit();
				_clientModel = (ClientModel)bsClient.Current;

				if (string.IsNullOrEmpty(_clientModel._id))
				{
					_logInfoRepository.CreateLogInfo(_clientModel);

					var newCustomer = await _clientRepository.AddNewClientAsync(_clientModel);
					if (string.IsNullOrEmpty(newCustomer))
					{
						throw new Exception($"Error while saving : {txtClientName.Text}");
					}
				}
				else
				{
					_logInfoRepository.UpdateLogInfo(_clientModel);
					await _clientRepository.UpdateClientAsync(_clientModel);
				}

				SendUpdatedClient(_clientModel, EventArgs.Empty);
			}
			catch (Exception exception)
			{
				XtraMessageBox.Show(exception.Message,
					"User save error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private bool ValidateData()
		{
			var validateReturnValue = true;
			var messageNumber = 0;
			var validateMessage = new StringBuilder();

			if (txtClientName.Text == "")
			{
				messageNumber += 1;
				validateMessage.Append("\n- Client Name cannot be empty.");
				validateReturnValue = false;
				txtClientName.Focus();
			}

			if (cboCountries.Text == "")
			{
				messageNumber += 1;
				validateMessage.Append("\n- Country Name cannot be empty.");
				validateReturnValue = false;
				cboCountries.Focus();
			}

			if (cboCities.Text == "")
			{
				messageNumber += 1;
				validateMessage.Append("\n- City Region Name cannot be empty.");
				validateReturnValue = false;
				cboCities.Focus();
			}

			if (txtAddress.Text == "")
			{
				messageNumber += 1;
				validateMessage.Append("\n- Address Name cannot be empty.");
				validateReturnValue = false;
				txtAddress.Focus();
			}

			if (txtPhoneNumber1.Text == "")
			{
				messageNumber += 1;
				validateMessage.Append("\n- At least one phone number is required.");
				validateReturnValue = false;
				txtPhoneNumber1.Focus();
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