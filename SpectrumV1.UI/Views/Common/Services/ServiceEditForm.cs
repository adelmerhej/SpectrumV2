using DevExpress.XtraEditors;
using SpectrumV1.DataLayers.Common.Services;
using SpectrumV1.Models.Common.Services;
using SpectrumV1.Utilities;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpectrumV1.Views.Common.Services
{
	public partial class ServiceEditForm : XtraForm
	{
		private ServiceModel _serviceModel;
		private readonly ServiceRepository _serviceRepository = new ServiceRepository();

		private readonly LogInfoRepository _logInfoRepository = new LogInfoRepository();

		private bool _canEdit = true;
		private bool _isAdmin = true;

		public EventHandler SendUpdatedService;

		public ServiceEditForm(ServiceModel model)
		{
			InitializeComponent();

			_serviceModel = model;

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
				// Only load from repository if editing existing user (has id)
				if (!string.IsNullOrEmpty(_serviceModel?._id))
				{
					var existing = await _serviceRepository.GetServiceByIdAsync(_serviceModel._id);
					if (existing != null)
						_serviceModel = existing;
				}
			}
			catch (Exception ex)
			{
				XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void WireUpBindings()
		{
			bsService.DataSource = _serviceModel;
		}

		private void ApplyDefaults()
		{

		}

		private void ApplyPermissions()
		{
			btnSave.Enabled = _isAdmin || _canEdit;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		private async void btnSave_Click(object sender, EventArgs e)
		{
			if (!ValidateData()) return;

			try
			{
				BindingContext[bsService].EndCurrentEdit();
				_serviceModel = (ServiceModel)bsService.Current;

				if (string.IsNullOrEmpty(_serviceModel._id))
				{
					_logInfoRepository.CreateLogInfo(_serviceModel);
					var newId = await _serviceRepository.AddNewServiceAsync(_serviceModel);
				}
				else
				{
					_logInfoRepository.UpdateLogInfo(_serviceModel);
					await _serviceRepository.UpdateServiceAsync(_serviceModel);
				}

				SendUpdatedService(_serviceModel, EventArgs.Empty);
				Close();
			}
			catch (Exception ex)
			{
				XtraMessageBox.Show(ex.Message, @"Error Saving user", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
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

			if (txtCode.Text == "")
			{
				messageNumber += 1;
				validateMessage.Append("\n- Code cannot be empty.");
				validateReturnValue = false;
				txtCode.Focus();
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