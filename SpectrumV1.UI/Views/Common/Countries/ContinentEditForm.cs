using DevExpress.XtraEditors;
using SpectrumV1.DataLayers.Common.Countries;
using SpectrumV1.Models.Common.Countries;
using SpectrumV1.Utilities;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpectrumV1.Views.Common.Countries
{
	public partial class ContinentEditForm : XtraForm
	{
		private ContinentModel _continentModel;
		private readonly ContinentRepository _continentRepository = new ContinentRepository();

		private readonly LogInfoRepository _logInfoRepository = new LogInfoRepository();

		private bool _canEdit = true;
		private bool _isAdmin = true;

		public EventHandler SendUpdatedContinent;

		public ContinentEditForm(ContinentModel model)
		{
			InitializeComponent();

			_continentModel = model;

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
				if (!string.IsNullOrEmpty(_continentModel?._id))
				{
					var existing = await _continentRepository.GetContinentByIdAsync(_continentModel._id);
					if (existing != null)
						_continentModel = existing;
				}
			}
			catch (Exception ex)
			{
				XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void WireUpBindings()
		{
			bsContinent.DataSource = _continentModel;
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
				BindingContext[bsContinent].EndCurrentEdit();
				_continentModel = (ContinentModel)bsContinent.Current;

				if (string.IsNullOrEmpty(_continentModel._id))
				{
					_logInfoRepository.CreateLogInfo(_continentModel);
					var newId = await _continentRepository.AddNewContinentAsync(_continentModel);
				}
				else
				{
					_logInfoRepository.UpdateLogInfo(_continentModel);
					await _continentRepository.UpdateContinentAsync(_continentModel);
				}

				SendUpdatedContinent(_continentModel, EventArgs.Empty);
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