using DevExpress.XtraEditors;
using Spectrum.DataLayers.Common.Services;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Common.Services;
using Spectrum.Utilities;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.Common.Services
{
	public partial class ServiceTypeEditForm : XtraForm
	{
		private ServiceTypeModel _serviceTypeModel = new ServiceTypeModel();

		private readonly ServiceTypeRepository _serviceTypeRepository = new ServiceTypeRepository(DatabaseFactory.ProfilePrimary);

		private readonly LogInfoRepository _logInfoRepository = new LogInfoRepository();

		private bool _canEdit = true;
		private bool _isAdmin = true;
		private bool _isProtected = true;

		public EventHandler SendUpdatedServiceType;

		public ServiceTypeEditForm(ServiceTypeModel model)
		{
			InitializeComponent();

			_serviceTypeModel = model;

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
				// reserved for future permissions/bindings
			}
			catch (Exception ex)
			{
				XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void WireUpBindings()
		{
			bsServiceType.DataSource = _serviceTypeModel;
		}

		private void ApplyDefaults()
		{
		}

		private void ApplyPermissions()
		{
			btnSave.Enabled = _isAdmin || _canEdit;
		}

		private async void btnSave_Click(object sender, EventArgs e)
		{
			if (!ValidateData()) return;

			try
			{
				BindingContext[bsServiceType].EndCurrentEdit();
				_serviceTypeModel = (ServiceTypeModel)bsServiceType.Current;

				if (string.IsNullOrEmpty(_serviceTypeModel._id))
				{
					_logInfoRepository.CreateLogInfo(_serviceTypeModel);
					await _serviceTypeRepository.AddNewServiceTypeAsync(_serviceTypeModel);
				}
				else
				{
					_logInfoRepository.UpdateLogInfo(_serviceTypeModel);
					await _serviceTypeRepository.UpdateServiceTypeAsync(_serviceTypeModel);
				}

				SendUpdatedServiceType?.Invoke(_serviceTypeModel, EventArgs.Empty);
				Close();
			}
			catch (Exception ex)
			{
				XtraMessageBox.Show(ex.Message, @"Error Saving service type", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

			if (string.IsNullOrWhiteSpace(txtType.Text))
			{
				messageNumber += 1;
				validateMessage.Append("\n- Type cannot be empty.");
				validateReturnValue = false;
				txtType.Focus();
			}

			if (string.IsNullOrWhiteSpace(txtCode.Text))
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
