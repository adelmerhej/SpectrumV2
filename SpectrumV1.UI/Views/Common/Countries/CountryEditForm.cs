using DevExpress.XtraEditors;
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
	public partial class CountryEditForm : XtraForm
	{
		private CountryModel _countryModel = new CountryModel();

		private IList<RegionModel> _regions = new List<RegionModel>();
		private IList<ContinentModel> _continents = new List<ContinentModel>();

		private readonly CountryRepository _countryRepository = new CountryRepository();
		private readonly RegionRepository _regionRepository = new RegionRepository();
		private readonly ContinentRepository _continentRepository = new ContinentRepository();

		private readonly LogInfoRepository _logInfoRepository = new LogInfoRepository();

		//init permission variables
		private bool _canEdit = true;
		private bool _isAdmin = true;
		private bool _isProtected = true;

		public EventHandler SendUpdatedCountry;

		public CountryEditForm(CountryModel model)
		{
			InitializeComponent();

			_countryModel = model;

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

				_regions = await _regionRepository.GetRegionsAsync();
				_continents = await _continentRepository.GetContinentsAsync();
			}
			catch (Exception ex)
			{
				XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void WireUpBindings()
		{
			bsCountry.DataSource = _countryModel;

			cboRegions.Properties.DataSource = null;
			cboRegions.Properties.DataSource = _regions;

			cboContinents.Properties.DataSource = null;
			cboContinents.Properties.DataSource = _continents;
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

		private async void btnSave_Click(object sender, EventArgs e)
		{
			if (!ValidateData()) return;

			try
			{
				BindingContext[bsCountry].EndCurrentEdit();
				_countryModel = (CountryModel)bsCountry.Current;


				if (string.IsNullOrEmpty(_countryModel._id))
				{
					_logInfoRepository.CreateLogInfo(_countryModel);

					var newId = await _countryRepository.AddNewCountryAsync(_countryModel);
				}
				else
				{
					_logInfoRepository.UpdateLogInfo(_countryModel);

					await _countryRepository.UpdateCountryAsync(_countryModel);
				}

				SendUpdatedCountry(_countryModel, EventArgs.Empty);
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

			if (cboRegions.Text == "")
			{
				messageNumber += 1;
				validateMessage.Append("\n- Region cannot be empty.");
				validateReturnValue = false;
				cboRegions.Focus();
			}

			if (cboContinents.Text == "")
			{
				messageNumber += 1;
				validateMessage.Append("\n- Continent cannot be empty.");
				validateReturnValue = false;
				cboContinents.Focus();
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