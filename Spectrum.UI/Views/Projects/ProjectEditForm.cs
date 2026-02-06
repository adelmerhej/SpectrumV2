using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using Spectrum.DataLayers.Common.Countries;
using Spectrum.DataLayers.Common.Services;
using Spectrum.DataLayers.DataAccess;
using Spectrum.DataLayers.Members.Clients;
using Spectrum.DataLayers.Members.Engineers;
using Spectrum.DataLayers.Projects;
using Spectrum.DataLayers.Users;
using Spectrum.Models.Common.Countries;
using Spectrum.Models.Common.Services;
using Spectrum.Models.Members.Clients;
using Spectrum.Models.Members.Engineers;
using Spectrum.Models.Projects;
using Spectrum.Models.Users;
using Spectrum.Utilities;
using Spectrum.Views.Common.Countries;
using Spectrum.Views.Members.Clients;
using Spectrum.Views.Members.Engineers;
using Spectrum.Views.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.Projects
{
	public partial class ProjectEditForm : RibbonForm
	{
		private ProjectModel _projectModel = new ProjectModel();
		
		private IList<ClientModel> _clients = new List<ClientModel>();
		private ClientModel _clientModel = new ClientModel();
		
		private IList<EngineerModel> _engineers = new List<EngineerModel>();
		private EngineerModel _engineerModel = new EngineerModel();

		private UserModel _userModel = new UserModel();
		private IList<UserModel> _users = new List<UserModel>();

		private IList<CountryModel> _countries = new List<CountryModel>();
		private CountryModel _countryModel = new CountryModel();

        private IList<CityModel> _cities = new List<CityModel>();
        private CityModel _cityModel = new CityModel();
        
		private IList<ServiceModel> _services = new List<ServiceModel>();
		private IList<ServiceTypeModel> _serviceTypes = new List<ServiceTypeModel>();
			
		private readonly ProjectRepository _projectRepository = new ProjectRepository(DatabaseFactory.ProfilePrimary);
		private readonly ClientRepository _clientRepository = new ClientRepository(DatabaseFactory.ProfilePrimary);
		private readonly EngineerRepository _engineerRepository = new EngineerRepository(DatabaseFactory.ProfilePrimary);
		private readonly UserRepository _userRepository = new UserRepository(DatabaseFactory.ProfilePrimary);
		private readonly ServiceRepository _serviceRepository = new ServiceRepository(DatabaseFactory.ProfilePrimary);
		private readonly ServiceTypeRepository _serviceTypeRepository = new ServiceTypeRepository(DatabaseFactory.ProfilePrimary);
		private readonly CountryRepository _countryRepository = new CountryRepository(DatabaseFactory.ProfilePrimary);
		private readonly CityRepository _cityRepository = new CityRepository(DatabaseFactory.ProfilePrimary);

		private readonly LogInfoRepository _logInfoRepository = new LogInfoRepository();

		private bool _canAdd = true;
		private bool _canEdit = true;
		private bool _canDelete = true;
		private bool _canPrint = true;
		private bool _isAdmin = true;
		private bool _isProtected = true;

		public EventHandler SendUpdatedProject;

		public ProjectEditForm(ProjectModel model)
		{
			InitializeComponent();

			_projectModel = model;
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
				_clients = await _clientRepository.GetClientsAsync();
				_engineers = await _engineerRepository.GetEngineersAsync();
				_users = await _userRepository.GetUsersAsync();
				_services = await _serviceRepository.GetServicesAsync();
				_serviceTypes = await _serviceTypeRepository.GetServiceTypesAsync();
			}
			catch (Exception ex)
			{
				XtraMessageBox.Show(ex.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void WireUpBindings()
		{
			bsProject.DataSource = _projectModel;
			cboClients.Properties.DataSource = null;
			cboClients.Properties.DataSource = _clients;

			cboEngineers.Properties.DataSource = null;
			cboEngineers.Properties.DataSource = _engineers;

			cboUsers.Properties.DataSource = null;
			cboUsers.Properties.DataSource = _users;

			cboPersonInCharge.Properties.DataSource = null;
			cboPersonInCharge.Properties.DataSource = _users;

			cboOperatingUsers.Properties.DataSource = null;
			cboOperatingUsers.Properties.DataSource = _users;

		cboServicesProvided.Properties.DataSource = null;
		cboServicesProvided.Properties.DisplayMember = "ServiceName";
		cboServicesProvided.Properties.ValueMember = "ServiceName";
		cboServicesProvided.Properties.DataSource = _services;

		cboServicesType.Properties.DataSource = null;
		cboServicesType.Properties.DisplayMember = "ServiceType";
		cboServicesType.Properties.ValueMember = "ServiceType";
		cboServicesType.Properties.DataSource = _serviceTypes;

		// Load checked items from project model
		if (_projectModel.ServicesProvided != null && _projectModel.ServicesProvided.Any())
		{
			cboServicesProvided.EditValue = string.Join(", ", _projectModel.ServicesProvided);
		}

		if (_projectModel.ServiceTypes != null && _projectModel.ServiceTypes.Any())
		{
			cboServicesType.EditValue = string.Join(", ", _projectModel.ServiceTypes);
		}

		//cboStatus.Properties.Items.Clear();
		//cboStatus.Properties.Items.AddRange(Enum.GetNames(typeof(ProjectStatus)));
	}

		private void ApplyDefaults()
		{
			if (string.IsNullOrEmpty(_projectModel._id))
			{
				_projectModel.Status = ProjectStatus.Active;
				_projectModel.ProjectDate = DateTime.Today;
			}
		}

		private void ApplyPermissions()
		{
			btnNew.Enabled = _isAdmin || _canAdd;
			btnSave.Enabled = _isAdmin || _canEdit;
			btnSaveAndClose.Enabled = _isAdmin || _canEdit;
			btnPrint.Enabled = _isAdmin || _canPrint;
			btnDelete.Enabled = _isAdmin || _canDelete;
		}

		private void btnNew_ItemClick(object sender, ItemClickEventArgs e)
		{
			_projectModel = new ProjectModel();
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

		private async void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (string.IsNullOrEmpty(_projectModel._id)) return;
			if (XtraMessageBox.Show($"Are you sure you want to delete Record: `{_projectModel.ProjectName}`?",
				"Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2) == DialogResult.Yes)
			{
			await _projectRepository.DeleteProjectAsync(_projectModel._id);
			_projectModel.Deleted = true;
			SendUpdatedProject?.Invoke(_projectModel, EventArgs.Empty);
			Close();
			}
		}

		private void btnPrint_ItemClick(object sender, ItemClickEventArgs e) { }
		private void btnClose_ItemClick(object sender, ItemClickEventArgs e) { Close(); }

		private async void SaveData()
		{
			try
			{
				BindingContext[bsProject].EndCurrentEdit();
				_projectModel = (ProjectModel)bsProject.Current;

			// derive client / engineer names from selected values
			if (cboClients.EditValue != null)
			{
				var selectedClient = _clients.FirstOrDefault(c => c._id == cboClients.EditValue.ToString());
				if (selectedClient != null) _projectModel.ClientName = selectedClient.ClientName;
			}
			if (cboEngineers.EditValue != null)
			{
				var selectedEngineer = _engineers.FirstOrDefault(e => e._id == cboEngineers.EditValue.ToString());
				if (selectedEngineer != null) _projectModel.EngineerInCharge = selectedEngineer.EngineerName;
			}

			// Save selected services and service types
			if (cboServicesProvided.EditValue != null)
			{
				var servicesString = cboServicesProvided.EditValue.ToString();
				_projectModel.ServicesProvided = servicesString.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries).ToList();
			}

			if (cboServicesType.EditValue != null)
			{
				var serviceTypesString = cboServicesType.EditValue.ToString();
				_projectModel.ServiceTypes = serviceTypesString.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries).ToList();
			}

			//if (!string.IsNullOrEmpty(cboStatus.Text))
			//{
			//	if (Enum.TryParse<ProjectStatus>(cboStatus.Text, out var st)) _projectModel.Status = st;
			//}

				if (string.IsNullOrEmpty(_projectModel._id))
				{
					_logInfoRepository.CreateLogInfo(_projectModel);
					var newId = await _projectRepository.AddNewProjectAsync(_projectModel);
					if (string.IsNullOrEmpty(newId)) throw new Exception($"Error while saving : {_projectModel.ProjectName}");
				}
			else
			{
				_logInfoRepository.UpdateLogInfo(_projectModel);
				await _projectRepository.UpdateProjectAsync(_projectModel);
			}
			SendUpdatedProject?.Invoke(_projectModel, EventArgs.Empty);
		}
		catch (Exception exception)
			{
				XtraMessageBox.Show(exception.Message, "Project save error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private bool ValidateData()
		{
			var validateReturnValue = true;
			var messageNumber = 0;
			var validateMessage = new StringBuilder();

			if (string.IsNullOrWhiteSpace(txtProjectName.Text))
			{
				messageNumber += 1; validateMessage.Append("\n- Project Name cannot be empty.");
				validateReturnValue = false; txtProjectName.Focus();
			}
			//if (cboStatus.EditValue == null)
			//{
			//	messageNumber += 1; validateMessage.Append("\n- Status cannot be empty.");
			//	validateReturnValue = false; cboStatus.Focus();
			//}

			if (!validateReturnValue)
			{
				validateMessage.Insert(0, "The following need your attention:");
				if (messageNumber > 1) validateMessage.Replace("following", "followings");
				XtraMessageBox.Show(validateMessage + " \nPlease try again.",
					"Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			return validateReturnValue;
		}


		#region Add new data
		private void cboPersonInCharge_AddNewValue(object sender, AddNewValueEventArgs e)
		{
			UserEditForm frm = new UserEditForm(new UserModel());
			frm.SendUpdatedUser += RcvUpdatedPersonInChargeAsync;
			frm.ShowDialog();
		}

		private void RcvUpdatedPersonInChargeAsync(object sender, EventArgs e)
		{
			if (sender == null) return;
			_userModel = sender as UserModel;

			_users.Add(_userModel);

			cboUsers.Properties.DataSource = null;
			cboUsers.Properties.DataSource = _users;
			if (_userModel != null) cboUsers.EditValue = _userModel._id;
		}

		private void cboOperatingUsers_AddNewValue(object sender, AddNewValueEventArgs e)
		{
			UserEditForm frm = new UserEditForm(new UserModel());
			frm.SendUpdatedUser += RcvOperatingUserAsync;
			frm.ShowDialog();
		}

		private void RcvOperatingUserAsync(object sender, EventArgs e)
		{
			if (sender == null) return;
			_userModel = sender as UserModel;

			_users.Add(_userModel);

			cboOperatingUsers.Properties.DataSource = null;
			cboOperatingUsers.Properties.DataSource = _users;
			if (_userModel != null) cboOperatingUsers.EditValue = _userModel._id;
		}

		private void cboUsers_AddNewValue(object sender, AddNewValueEventArgs e)
		{
			UserEditForm frm = new UserEditForm(new UserModel());
			frm.SendUpdatedUser += RcvUpdatedUserAsync;
			frm.ShowDialog();
		}

		private void RcvUpdatedUserAsync(object sender, EventArgs e)
		{
			if (sender == null) return;
			_userModel = sender as UserModel;

			_users.Add(_userModel);

			cboUsers.Properties.DataSource = null;
			cboUsers.Properties.DataSource = _users;
			if (_userModel != null) cboUsers.EditValue = _userModel._id;
		}


        #endregion

        private void cboCountries_AddNewValue(object sender, AddNewValueEventArgs e)
        {
            CountryEditForm frm = new CountryEditForm(new CountryModel());
            frm.SendUpdatedCountry += RcvUpdatedCountry;
            frm.ShowDialog();
        }

        private void cboCities_AddNewValue(object sender, AddNewValueEventArgs e)
        {
            CityEditForm frm = new CityEditForm(new CityModel());
            frm.SendUpdatedCity += RcvUpdatedCity;
            frm.ShowDialog();
        }

        private void RcvUpdatedCountry(object sender, EventArgs e)
        {
            if (sender == null) return;
            _countryModel = sender as CountryModel;

            _countries.Add(_countryModel);

            cboCountries.Properties.DataSource = null;
            cboCountries.Properties.DataSource = _countries;
            if (_countryModel != null) cboCountries.EditValue = _countryModel.CountryName;
        }

        private void RcvUpdatedCity(object sender, EventArgs e)
        {
            if (sender == null) return;
            _cityModel = sender as CityModel;

            _cities.Add(_cityModel);

            cboCities.Properties.DataSource = null;
            cboCities.Properties.DataSource = _cities;
            if (_cityModel != null) cboCities.EditValue = _cityModel.CityName;
        }

        private void cboEngineers_AddNewValue(object sender, AddNewValueEventArgs e)
        {
            EngineerEditForm frm = new EngineerEditForm(new EngineerModel());
            frm.SendUpdatedEngineer += RcvUpdatedEngineer;
            frm.ShowDialog();
        }

        private void cboClients_AddNewValue(object sender, AddNewValueEventArgs e)
        {
            ClientEditForm frm = new ClientEditForm(new ClientModel());
            frm.SendUpdatedClient += RcvUpdatedClient;
            frm.ShowDialog();
        }

        private void RcvUpdatedEngineer(object sender, EventArgs e)
        {
            if (sender == null) return;
            _engineerModel = sender as EngineerModel;

            _engineers.Add(_engineerModel);

            cboEngineers.Properties.DataSource = null;
            cboEngineers.Properties.DataSource = _engineers;
            if (_engineerModel != null) cboEngineers.EditValue = _engineerModel.EngineerName;
        }

        private void RcvUpdatedClient(object sender, EventArgs e)
        {
            if (sender == null) return;
            _clientModel = sender as ClientModel;

            _clients.Add(_clientModel);

            cboClients.Properties.DataSource = null;
            cboClients.Properties.DataSource = _clients;
            if (_clientModel != null) cboClients.EditValue = _clientModel.ClientName;
        }

    }
}
