using DevExpress.XtraEditors;
using SpectrumV1.DataLayers.Common.Areas;
using SpectrumV1.Models.Common.Areas;
using SpectrumV1.Models.Common.Countries;
using SpectrumV1.Utilities;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpectrumV1.Views.Common.Areas
{
	public partial class AreaEditForm : XtraForm
	{
		private AreaModel _areaModel;
		private readonly AreaRepository _areaRepository = new AreaRepository();

		private readonly LogInfoRepository _logInfoRepository = new LogInfoRepository();

		private bool _canEdit = true;
		private bool _isAdmin = true;

		public EventHandler SendUpdatedArea;


		public AreaEditForm(AreaModel model)
		{
			InitializeComponent();

			_areaModel = model;

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
				if (!string.IsNullOrEmpty(_areaModel?._id))
				{
					var existing = await _areaRepository.GetAreaByIdAsync(_areaModel._id);
					if (existing != null)
						_areaModel = existing;
				}
			}
			catch (Exception ex)
			{
				XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void WireUpBindings()
		{
			bsAreas.DataSource = _areaModel;
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
				BindingContext[bsAreas].EndCurrentEdit();
				_areaModel = (AreaModel)bsAreas.Current;

				if (string.IsNullOrEmpty(_areaModel._id))
				{
					_logInfoRepository.CreateLogInfo(_areaModel);
					var newId = await _areaRepository.AddNewAreaAsync(_areaModel);
				}
				else
				{
					_logInfoRepository.UpdateLogInfo(_areaModel);
					await _areaRepository.UpdateAreaAsync (_areaModel);
				}

				SendUpdatedArea(_areaModel, EventArgs.Empty);
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