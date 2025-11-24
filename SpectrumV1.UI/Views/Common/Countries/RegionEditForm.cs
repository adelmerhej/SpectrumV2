using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using SpectrumV1.DataLayers.Common.Countries;
using SpectrumV1.Models.Common.Countries;
using SpectrumV1.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpectrumV1.Views.Common.Countries
{
	public partial class RegionEditForm : XtraForm
	{
		private RegionModel _regionModel;
		
		private ContinentModel _continentModel = new ContinentModel();
		private IList<ContinentModel> _continents = new List<ContinentModel>();

		private readonly RegionRepository _regionRepository = new RegionRepository();
		private readonly ContinentRepository _continentRepository = new ContinentRepository();

		private readonly LogInfoRepository _logInfoRepository = new LogInfoRepository();

		private bool _canEdit = true;
		private bool _isAdmin = true;

		public EventHandler SendUpdatedRegion;

		public RegionEditForm(RegionModel model)
		{
			InitializeComponent();

			_regionModel = model;

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
				if (!string.IsNullOrEmpty(_regionModel?._id))
				{
					var existing = await _regionRepository.GetRegionByIdAsync(_regionModel._id);
					if (existing != null)
						_regionModel = existing;
				}

				_continents = await _continentRepository.GetContinentsAsync();
			}
			catch (Exception ex)
			{
				XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void WireUpBindings()
		{
			bsRegion.DataSource = _regionModel;

			cboContinents.Properties.DataSource = null;
			cboContinents.Properties.DataSource = _continents;
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
				BindingContext[bsRegion].EndCurrentEdit();
				_regionModel = (RegionModel)bsRegion.Current;

				if (string.IsNullOrEmpty(_regionModel._id))
				{
					_logInfoRepository.CreateLogInfo(_regionModel);

					var newId = await _regionRepository.AddNewRegionAsync(_regionModel);
				}
				else
				{
					_logInfoRepository.UpdateLogInfo(_regionModel);

					await _regionRepository.UpdateRegionAsync(_regionModel);
				}

				SendUpdatedRegion(_regionModel, EventArgs.Empty);
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

		private void cboContinents_AddNewValue(object sender, AddNewValueEventArgs e)
		{
			ContinentEditForm frm = new ContinentEditForm(new ContinentModel());
			frm.SendUpdatedContinent += RcvUpdatedContinent;
			frm.ShowDialog();
		}

		private void RcvUpdatedContinent(object sender, EventArgs e)
		{
			if (sender == null) return;
			_continentModel = sender as ContinentModel;

			_continents.Add(_continentModel);

			cboContinents.Properties.DataSource = null;
			cboContinents.Properties.DataSource = _continents;
			if (_continentModel != null) cboContinents.EditValue = _continentModel.ContinentName;
		}
	}
}