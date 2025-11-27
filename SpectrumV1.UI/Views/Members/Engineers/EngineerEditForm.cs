using DevExpress.XtraEditors;
using SpectrumV1.DataLayers.Members.Engineers;
using SpectrumV1.Models.Common.Areas;
using SpectrumV1.Models.Members.Engineers;
using SpectrumV1.Utilities;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpectrumV1.Views.Members.Engineers
{
	public partial class EngineerEditForm : XtraForm
	{
		private EngineerModel _engineerModel = new EngineerModel();
		private readonly EngineerRepository _engineerRepository = new EngineerRepository();
		private readonly LogInfoRepository _logInfoRepository = new LogInfoRepository();

		// init permission variables
		private bool _canEdit = true;
		private bool _isAdmin = true;
		private bool _isProtected = true;

		public EventHandler SendUpdatedEngineer;

		public EngineerEditForm(EngineerModel model)
		{
			InitializeComponent();

			_engineerModel = model ?? new EngineerModel();

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
				// load permissions or other async data if needed
			}
			catch (Exception ex)
			{
				XtraMessageBox.Show(ex.Message, @"Error Loading", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void WireUpBindings()
		{
			bsEngineer.DataSource = _engineerModel;
		}

		private void ApplyDefaults()
		{
			if (_engineerModel == null)
				_engineerModel = new EngineerModel();
		}

		private void ApplyPermissions()
		{
			// Enable/disable save based on permissions
			var canSave = _isAdmin || _canEdit;
			if (btnSave != null) btnSave.Enabled = canSave;
		}

		private async void btnSave_Click(object sender, EventArgs e)
		{
			if (!ValidateData()) return;

			try
			{
				BindingContext[bsEngineer].EndCurrentEdit();
				_engineerModel = (EngineerModel)bsEngineer.Current;


				if (string.IsNullOrEmpty(_engineerModel._id))
				{
					_logInfoRepository.CreateLogInfo(_engineerModel);

					var newId = await _engineerRepository.AddNewEngineerAsync(_engineerModel);
				}
				else
				{
					_logInfoRepository.UpdateLogInfo(_engineerModel);

					await _engineerRepository.UpdateEngineerAsync(_engineerModel);
				}

				SendUpdatedEngineer(_engineerModel, EventArgs.Empty);
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

			// If controls exist, validate their values; otherwise validate model
			string name = _engineerModel.EngineerName;
			string email = _engineerModel.Email;

			if (string.IsNullOrWhiteSpace(name))
			{
				messageNumber += 1;
				validateMessage.Append("\n- Engineer Name cannot be empty.");
				validateReturnValue = false;
			}

			if (!string.IsNullOrWhiteSpace(email) && !IsValidEmail(email))
			{
				messageNumber += 1;
				validateMessage.Append("\n- Email format is invalid.");
				validateReturnValue = false;
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

		private bool IsValidEmail(string email)
		{
			try
			{
				var addr = new System.Net.Mail.MailAddress(email);
				return addr.Address == email;
			}
			catch
			{
				return false;
			}
		}
	}
}