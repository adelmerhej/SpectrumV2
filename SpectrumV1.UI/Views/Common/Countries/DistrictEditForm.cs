using DevExpress.XtraEditors;
using SpectrumV1.DataLayers.Common.Countries;
using SpectrumV1.DataLayers.DataAccess;
using SpectrumV1.Models.Common.Countries;
using SpectrumV1.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpectrumV1.Views.Common.Countries
{
	public partial class DistrictEditForm : XtraForm
	{
		private DistrictModel _districtModel = new DistrictModel();

		private ProvinceModel _provinceModel = new ProvinceModel();
		private IList<ProvinceModel> _provinces = new List<ProvinceModel>();

		private CountryModel _countryModel = new CountryModel();
		private IList<CountryModel> _countries = new List<CountryModel>();

		private RegionModel _regionModel = new RegionModel();
		private IList<RegionModel> _regions = new List<RegionModel>();

		private ContinentModel _continentModel = new ContinentModel();
		private IList<ContinentModel> _continents = new List<ContinentModel>();

		private readonly DistrictRepository _districtRepository = new DistrictRepository(DatabaseFactory.ProfilePrimary);
		private readonly ProvinceRepository _provinceRepository = new ProvinceRepository(DatabaseFactory.ProfilePrimary);
		private readonly CountryRepository _countryRepository = new CountryRepository(DatabaseFactory.ProfilePrimary);
		private readonly RegionRepository _regionRepository = new RegionRepository(DatabaseFactory.ProfilePrimary);
		private readonly ContinentRepository _continentRepository = new ContinentRepository(DatabaseFactory.ProfilePrimary);

		private readonly LogInfoRepository _logInfoRepository = new LogInfoRepository();

		//init permission variables
		private bool _canEdit = true;
		private bool _isAdmin = true;
		private bool _isProtected = true;

		public EventHandler SendUpdatedDistrict;

		public DistrictEditForm(DistrictModel model)
		{
			InitializeComponent();

			_districtModel = model;

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

				_provinces = await _provinceRepository.GetProvincesAsync();
				_countries = await _countryRepository.GetCountriesAsync();
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
			bsDistrict.DataSource = _districtModel;

			cboProvinces.Properties.DataSource = null;
			cboProvinces.Properties.DataSource = _provinces;

			cboCountries.Properties.DataSource = null;
			cboCountries.Properties.DataSource = _countries;

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
				BindingContext[bsDistrict].EndCurrentEdit();
				_districtModel = (DistrictModel)bsDistrict.Current;


				if (string.IsNullOrEmpty(_districtModel._id))
				{
					_logInfoRepository.CreateLogInfo(_districtModel);

					var newId = await _districtRepository.AddNewDistrictAsync(_districtModel);
				}
				else
				{
					_logInfoRepository.UpdateLogInfo(_districtModel);

					await _districtRepository.UpdateDistrictAsync(_districtModel);
				}

				SendUpdatedDistrict(_districtModel, EventArgs.Empty);
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

			if (cboProvinces.Text == "")
			{
				messageNumber += 1;
				validateMessage.Append("\n- Province cannot be empty.");
				validateReturnValue = false;
				cboProvinces.Focus();
			}

			if (cboCountries.Text == "")
			{
				messageNumber += 1;
				validateMessage.Append("\n- Country cannot be empty.");
				validateReturnValue = false;
				cboCountries.Focus();
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