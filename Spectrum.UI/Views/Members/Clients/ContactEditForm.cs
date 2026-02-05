using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using Spectrum.DataLayers.DataAccess;
using Spectrum.DataLayers.HumanResources.JobPositions;
using Spectrum.DataLayers.Members.Clients;
using Spectrum.Models.HumanResources.JobPositions;
using Spectrum.Models.Members.Clients;
using Spectrum.Utilities;
using Spectrum.Views.HumanResources.JobPositions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.Members.Clients
{
	public partial class ContactEditForm : XtraForm
	{
		private ContactModel _contactModel = new ContactModel();
		private JobPositionModel _jobPositionModel = new JobPositionModel();
		private IList<JobPositionModel> _jobPositions = new List<JobPositionModel>();
		private readonly string _clientId;

		private readonly ContactRepository _contactRepository = new ContactRepository(DatabaseFactory.ProfilePrimary);
		private readonly JobPositionRepository _jobPositionRepository = new JobPositionRepository(DatabaseFactory.ProfilePrimary);

		private readonly LogInfoRepository _logInfoRepository = new LogInfoRepository();

		//init permission variables
		private bool _canEdit = true;
		private bool _isAdmin = true;
		private bool _isProtected = true;

		public EventHandler SendUpdatedContact;

		public ContactEditForm(ContactModel model, string id)
		{
			InitializeComponent();

			_clientId = id;
			_contactModel = model ?? new ContactModel();
			if (string.IsNullOrEmpty(_contactModel.ClientId))
				_contactModel.ClientId = _clientId;

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

				// Only load from repository if editing existing user (has id)
				if (!string.IsNullOrEmpty(_contactModel?._id))
				{
					var existing = await _contactRepository.GetContactByIdAsync(_contactModel._id);
					if (existing != null)
					{
						_contactModel = existing;
						if (string.IsNullOrEmpty(_contactModel.ClientId))
							_contactModel.ClientId = _clientId;
					}
				}
				else
				{
					_contactModel = new ContactModel
					{
						ClientId = _clientId
					};
				}



				try
				{
					var loadTasks = new[]
					{
					Load_jobPositionsAsync(),
					LoadTitlesAsync(),
				};

					await Task.WhenAll(loadTasks);
				}
				catch (Exception ex)
				{
					throw new Exception("Error loading form data", ex);
				}
			}
			catch (Exception ex)
			{
				XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void WireUpBindings()
		{
			bsContact.DataSource = _contactModel;

			cboJobPositions.Properties.DataSource = _jobPositions;
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

		#region Loadingdata Events

		private async Task Load_jobPositionsAsync()
		{
			_jobPositions = await _jobPositionRepository.GetJobPositionsAsync();
		}

		private async Task LoadTitlesAsync()
		{
			HelperApplication.InitTitleComboBox(cboTitles.Properties);
			await Task.CompletedTask;
		}

		#endregion

		#region buttons Events

		private async void btnSave_Click(object sender, EventArgs e)
		{
			if (!ValidateData()) return;

			try
			{
				BindingContext[bsContact].EndCurrentEdit();
				_contactModel = (ContactModel)bsContact.Current;
				if (string.IsNullOrEmpty(_contactModel.ClientId))
					_contactModel.ClientId = _clientId;


				if (string.IsNullOrEmpty(_contactModel._id))
				{
					_logInfoRepository.CreateLogInfo(_contactModel);

					var newId = await _contactRepository.AddNewContactAsync(_contactModel);
				}
				else
				{
					_logInfoRepository.UpdateLogInfo(_contactModel);

					await _contactRepository.UpdateContactAsync(_contactModel);
				}

				SendUpdatedContact(_contactModel, EventArgs.Empty);
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

		#endregion

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

		private void cboJobPositions_AddNewValue(object sender, AddNewValueEventArgs e)
		{
			JobPositionEditForm frm = new JobPositionEditForm(new JobPositionModel());
			frm.SendUpdatedJobPosition += RcvUpdatedJobPositionAsync;
			frm.ShowDialog();
		}

		private void RcvUpdatedJobPositionAsync(object sender, EventArgs e)
		{
			if (sender == null) return;
			_jobPositionModel = sender as JobPositionModel;

			_jobPositions.Add(_jobPositionModel);

			cboJobPositions.Properties.DataSource = null;
			cboJobPositions.Properties.DataSource = _jobPositions;
			if (_jobPositionModel != null) cboJobPositions.EditValue = _jobPositionModel._id;
		}
	}
}