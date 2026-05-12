using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using Spectrum.DataLayers.Common.Areas;
using Spectrum.DataLayers.Common.Countries;
using Spectrum.DataLayers.Common.Locations;
using Spectrum.DataLayers.Common.Services;
using Spectrum.DataLayers.DataAccess;
using Spectrum.DataLayers.Members.Clients;
using Spectrum.DataLayers.Projects;
using Spectrum.DataLayers.Users;
using Spectrum.Models.Common.Areas;
using Spectrum.Models.Common.Countries;
using Spectrum.Models.Common.Services;
using Spectrum.Models.HumanResources.Employees;
using Spectrum.Models.Members.Clients;
using Spectrum.Models.Projects;
using Spectrum.Models.Users;
using Spectrum.Utilities;
using Spectrum.Utilities.Enums;
using Spectrum.Views.Common.Countries;
using Spectrum.Views.Common.Services;
using Spectrum.Views.Members.Clients;
using Spectrum.Views.Members.Engineers;
using Spectrum.Views.Users;
using Spectrum.DataLayers.HumanResources.Employees;
using System;
using System.ComponentModel;
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
        private BindingList<ProjectHandoverModel> _projectHandovers = new BindingList<ProjectHandoverModel>();

        private IList<ClientModel> _clients = new List<ClientModel>();
        private ClientModel _clientModel = new ClientModel();

        private IList<EmployeeModel> _engineers = new List<EmployeeModel>();
        private EmployeeModel _engineerModel = new EmployeeModel();

        private UserModel _userModel = new UserModel();
        private IList<UserModel> _users = new List<UserModel>();

        private IList<CountryModel> _countries = new List<CountryModel>();
        private CountryModel _countryModel = new CountryModel();

        private IList<CityModel> _cities = new List<CityModel>();
        private CityModel _cityModel = new CityModel();

        private IList<AreaModel> _areas = new List<AreaModel>();
        private AreaModel _areaModel = new AreaModel();
        private IList<LocationModel> _locations = new List<LocationModel>();
        private LocationModel _locationModel = new LocationModel();

        private IList<ServiceModel> _services = new List<ServiceModel>();
        private IList<ServiceTypeModel> _serviceTypes = new List<ServiceTypeModel>();

        private readonly ProjectRepository _projectRepository = new ProjectRepository(DatabaseFactory.ProfilePrimary);
        private readonly ClientRepository _clientRepository = new ClientRepository(DatabaseFactory.ProfilePrimary);
        private readonly EmployeeRepository _engineerRepository = new EmployeeRepository(DatabaseFactory.ProfilePrimary);
        private readonly UserRepository _userRepository = new UserRepository(DatabaseFactory.ProfilePrimary);
        private readonly ServiceRepository _serviceRepository = new ServiceRepository(DatabaseFactory.ProfilePrimary);
        private readonly ServiceTypeRepository _serviceTypeRepository = new ServiceTypeRepository(DatabaseFactory.ProfilePrimary);
        private readonly CountryRepository _countryRepository = new CountryRepository(DatabaseFactory.ProfilePrimary);
        private readonly CityRepository _cityRepository = new CityRepository(DatabaseFactory.ProfilePrimary);
        private readonly LocationRepository _locationRepository = new LocationRepository(DatabaseFactory.ProfilePrimary);
        private readonly AreaRepository _areaRepository = new AreaRepository(DatabaseFactory.ProfilePrimary);

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
            try
            {
                await InitializeBindings();
                WireUpBindings();
                ApplyDefaults();
                ApplyPermissions();
            }
            catch (Exception ex)
            {
                ShowError("Error during form initialization", ex);
            }
        }

        #region InitializeBindings Methods
        private async Task InitializeBindings()
        {
            try
            {
                var loadTasks = new[]
                {
                    LoadClientsAsync(),
                    LoadEngineersAsync(),
                    LoadUsersAsync(),
                    LoadServicesAsync(),
                    LoadServiceTypesAsync(),
                    LoadLocationsAsync(),
                    LoadAreasAsync(),
                    LoadCountriesAsync(),
                    LoadCitiesAsync(),
                };

                await Task.WhenAll(loadTasks);
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading form data", ex);
            }
        }

        #region Data Loading Methods
        
        private async Task LoadClientsAsync()
        {
            _clients = await _clientRepository.GetClientsAsync();
        }
        private async Task LoadEngineersAsync()
        {
            _engineers = await _engineerRepository.GetEmployeesAsync(EmployeeType.Engineer);
        }

        private async Task LoadUsersAsync()
        {
            _users = await _userRepository.GetUsersAsync();
        }

        private async Task LoadServicesAsync()
        {
            _services = await _serviceRepository.GetServicesAsync();
        }

        private async Task LoadServiceTypesAsync()
        {
            _serviceTypes = await _serviceTypeRepository.GetServiceTypesAsync();
        }

        private async Task LoadLocationsAsync()
        {
            _locations = await _locationRepository.GetLocationsAsync();
        }

        private async Task LoadAreasAsync()
        {
            _areas = await _areaRepository.GetAreasAsync();
        }

        private async Task LoadCountriesAsync()
        {
            _countries = await _countryRepository.GetCountriesAsync();
        }

        private async Task LoadCitiesAsync()
        {
            _cities = await _cityRepository.GetCitiesAsync();
        }
        #endregion

        #endregion

        #region WireUpBindings Methods

        private void WireUpBindings()
        {
            _projectModel.Location = _projectModel.Location ?? new LocationInfoModel();
            _projectModel.ContractDetails = _projectModel.ContractDetails ?? new ContractDetailModel();
            _projectModel.ProjectHandovers = _projectModel.ProjectHandovers ?? new List<ProjectHandoverModel>();
            NormalizeLookupValues();
            InitializeProjectHandovers();

            bsProject.DataSource = _projectModel;
            bsContractDetails.DataSource = _projectModel.ContractDetails;
            bsLocationInfo.DataSource = _projectModel.Location;

            ConfigureLookupBindings();

           BindCheckedComboBoxItems(cboServicesProvided,
                _services.Select(x => x.ServiceName),
                _projectModel.ContractDetails.ServicesProvided);

            BindCheckedComboBoxItems(cboServicesType,
                _serviceTypes.Select(x => x.ServiceType),
                _projectModel.ContractDetails.ServiceTypes);

            cboAreas.Properties.DataSource = null;
            cboAreas.Properties.DisplayMember = "AreaCode";
            cboAreas.Properties.ValueMember = "AreaCode";
            cboAreas.Properties.DataSource = _areas;

            cboLocations.Properties.DataSource = null;
            cboLocations.Properties.DisplayMember = "LocationName";
            cboLocations.Properties.ValueMember = "_id";
            cboLocations.Properties.DataSource = _locations;

            cboCountries.Properties.DataSource = null;
            cboCountries.Properties.DisplayMember = "CountryName";
            cboCountries.Properties.ValueMember = "CountryName";
            cboCountries.Properties.DataSource = _countries;

            cboCities.Properties.DataSource = null;
            cboCities.Properties.DisplayMember = "CityName";
            cboCities.Properties.ValueMember = "CityName";
            cboCities.Properties.DataSource = _cities;

            if (_projectModel.Location != null && !string.IsNullOrWhiteSpace(_projectModel.Location._id))
            {
                cboLocations.EditValue = _projectModel.Location._id;
            }

            // Whenever user changes selection, update project model Location via reflection
            cboLocations.EditValueChanged += cboLocations_EditValueChanged;

            //cboStatus.Properties.Items.Clear();
            //cboStatus.Properties.Items.AddRange(Enum.GetNames(typeof(ProjectStatus)));
        }

        #endregion

        private void ConfigureLookupBindings()
        {
            cboClients.DataBindings.Clear();
            cboClients.DataBindings.Add("EditValue", bsProject, "ClientIdValue", true, DataSourceUpdateMode.OnPropertyChanged);
            cboClients.Properties.DataSource = null;
            cboClients.Properties.DisplayMember = "ClientName";
            cboClients.Properties.ValueMember = "_id";
            cboClients.Properties.DataSource = _clients;

            cboFundedBy.DataBindings.Clear();
            cboFundedBy.DataBindings.Add("EditValue", bsContractDetails, "SponsorId", true, DataSourceUpdateMode.OnPropertyChanged);
            cboFundedBy.Properties.DataSource = null;
            cboFundedBy.Properties.DisplayMember = "ClientName";
            cboFundedBy.Properties.ValueMember = "_id";
            cboFundedBy.Properties.DataSource = _clients;

            cboEngineers.DataBindings.Clear();
            cboEngineers.DataBindings.Add("EditValue", bsProject, "EngineerIdValue", true, DataSourceUpdateMode.OnPropertyChanged);
            cboEngineers.Properties.DataSource = null;
            cboEngineers.Properties.DisplayMember = "FullName";
            cboEngineers.Properties.ValueMember = "_id";
            cboEngineers.Properties.DataSource = _engineers;

            cboUsers.DataBindings.Clear();
            cboUsers.DataBindings.Add("EditValue", bsProject, "UserIdValue", true, DataSourceUpdateMode.OnPropertyChanged);
            cboUsers.Properties.DataSource = null;
            cboUsers.Properties.DisplayMember = "Username";
            cboUsers.Properties.ValueMember = "_id";
            cboUsers.Properties.DataSource = _users;

            cboPersonInCharge.DataBindings.Clear();
            cboPersonInCharge.DataBindings.Add("EditValue", bsProject, "EngineerIdValue", true, DataSourceUpdateMode.OnPropertyChanged);
            cboPersonInCharge.Properties.DataSource = null;
            cboPersonInCharge.Properties.DisplayMember = "FullName";
            cboPersonInCharge.Properties.ValueMember = "_id";
            cboPersonInCharge.Properties.DataSource = _engineers;

            cboEngineers.EditValueChanged -= cboEngineers_EditValueChanged;
            cboEngineers.EditValueChanged += cboEngineers_EditValueChanged;

        }

        private void InitializeProjectHandovers()
        {
            _projectHandovers = new BindingList<ProjectHandoverModel>(_projectModel.ProjectHandovers.ToList());
            gcJobDetails.DataSource = _projectHandovers;
        }

        private void NormalizeLookupValues()
        {
            _projectModel.ClientIdValue = ResolveClientId(_projectModel.ClientIdValue, _projectModel.ClientName);
            _projectModel.EngineerIdValue = ResolveEngineerId(_projectModel.EngineerIdValue, _projectModel.EngineerInCharge);
            _projectModel.Username = ResolveProjectUsername(_projectModel.Username);
            _projectModel.UserIdValue = ResolveUserId(_projectModel.UserIdValue, _projectModel.Username);
            _projectModel.ContractDetails.SponsorId = ResolveClientId(_projectModel.ContractDetails.SponsorId, _projectModel.ContractDetails.SponsorId);
        }

        private string ResolveProjectUsername(string username)
        {
            if (!string.IsNullOrWhiteSpace(username)) return username;
            return string.IsNullOrWhiteSpace(CurrentUser.UserName) ? null : CurrentUser.UserName;
        }

        private string ResolveClientId(string idValue, string nameValue)
        {
            if (IsValidObjectId(idValue) && _clients.Any(c => c._id == idValue)) return idValue;

            var candidateName = !string.IsNullOrWhiteSpace(nameValue) ? nameValue : idValue;
            if (string.IsNullOrWhiteSpace(candidateName)) return null;

            var client = _clients.FirstOrDefault(c => string.Equals(c.ClientName, candidateName, StringComparison.OrdinalIgnoreCase));
            return client?._id;
        }

        private string ResolveEngineerId(string idValue, string nameValue)
        {
            if (IsValidObjectId(idValue) && _engineers.Any(e => e._id == idValue)) return idValue;

            if (string.IsNullOrWhiteSpace(nameValue)) return null;

            var engineer = _engineers.FirstOrDefault(e =>
                string.Equals(e.FullName, nameValue, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(e.FirstName, nameValue, StringComparison.OrdinalIgnoreCase));

            return engineer?._id;
        }

        private string ResolveUserId(string idValue, string username)
        {
            if (IsValidObjectId(idValue) && _users.Any(u => u._id == idValue)) return idValue;
            if (string.IsNullOrWhiteSpace(username)) return null;

            var user = _users.FirstOrDefault(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));
            return user?._id;
        }

        private static bool IsValidObjectId(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length != 24) return false;

            for (var i = 0; i < value.Length; i++)
            {
                var character = value[i];
                var isDigit = character >= '0' && character <= '9';
                var isLowerHex = character >= 'a' && character <= 'f';
                var isUpperHex = character >= 'A' && character <= 'F';

                if (!isDigit && !isLowerHex && !isUpperHex)
                {
                    return false;
                }
            }

            return true;
        }

        #region ApplyDefaults and Permissions Methods
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

        #endregion

        private void cboLocations_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboLocations.EditValue == null) return;
                var id = cboLocations.EditValue.ToString();
                var selected = _locations.FirstOrDefault(x => x._id == id);
                if (selected == null) return;

                if (_projectModel.Location == null)
                {
                    _projectModel.Location = new LocationInfoModel();
                }

                _projectModel.Location._id = selected._id;
                _projectModel.Location.RawLocation = selected.LocationName;

                bsProject.ResetBindings(false);
            }
            catch
            {
                // ignore
            }
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
                BindingContext[bsContractDetails].EndCurrentEdit();
                gvJobDetails.CloseEditor();
                gvJobDetails.UpdateCurrentRow();
                _projectModel = (ProjectModel)bsProject.Current;
                _projectModel.ProjectHandovers = _projectHandovers.Where(x => x != null).ToList();

                // derive client / engineer names from selected values
                if (cboClients.EditValue != null)
                {
                    var selectedClient = _clients.FirstOrDefault(c => c._id == cboClients.EditValue.ToString());
                    if (selectedClient != null)
                    {
                        _projectModel.ClientIdValue = selectedClient._id;
                        _projectModel.ClientName = selectedClient.ClientName;
                    }
                }
                if (cboEngineers.EditValue != null)
                {
                    var selectedEngineer = _engineers.FirstOrDefault(e => e._id == cboEngineers.EditValue.ToString());
                    if (selectedEngineer != null)
                    {
                        _projectModel.EngineerIdValue = selectedEngineer._id;
                        _projectModel.EngineerInCharge = selectedEngineer.FullName;
                    }
                }
                if (cboUsers.EditValue != null)
                {
                    var selectedUser = _users.FirstOrDefault(u => u._id == cboUsers.EditValue.ToString());
                    if (selectedUser != null)
                    {
                        _projectModel.UserIdValue = selectedUser._id;
                        _projectModel.Username = selectedUser.Username;
                    }
                }
                if (cboFundedBy.EditValue != null)
                {
                    _projectModel.ContractDetails.SponsorId = cboFundedBy.EditValue.ToString();
                }

                // Save selected services and service types
                _projectModel.ContractDetails = _projectModel.ContractDetails ?? new ContractDetailModel();
                _projectModel.ContractDetails.ServicesProvided = GetCheckedItems(cboServicesProvided);
                _projectModel.ContractDetails.ServiceTypes = GetCheckedItems(cboServicesType);

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

        private static List<string> GetCheckedItems(CheckedComboBoxEdit edit)
        {
            if (edit == null) return new List<string>();
            try
            {
                var items = edit.Properties.Items;
                var checkedValues = new List<string>();
                for (var i = 0; i < items.Count; i++)
                {
                    if (!items[i].CheckState.HasFlag(CheckState.Checked)) continue;
                    var value = items[i].Value;
                    if (value == null) continue;
                    checkedValues.Add(value.ToString());
                }
                return checkedValues;
            }
            catch
            {
                return new List<string>();
            }
        }

        private static void BindCheckedComboBoxItems(CheckedComboBoxEdit edit, IEnumerable<string> availableValues, IEnumerable<string> selectedValues)
        {
            if (edit == null) return;

            try
            {
                var selected = new HashSet<string>((selectedValues ?? Enumerable.Empty<string>())
                    .Where(v => !string.IsNullOrWhiteSpace(v)));

                var values = (availableValues ?? Enumerable.Empty<string>())
                    .Where(v => !string.IsNullOrWhiteSpace(v))
                    .Concat(selected)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .OrderBy(v => v)
                    .ToList();

                edit.Properties.Items.BeginUpdate();
                try
                {
                    edit.Properties.Items.Clear();
                    foreach (var value in values)
                    {
                        edit.Properties.Items.Add(value, selected.Contains(value));
                    }
                }
                finally
                {
                    edit.Properties.Items.EndUpdate();
                }
            }
            catch
            {
                // ignore
            }
        }

        private static void SetCheckedItems(CheckedComboBoxEdit edit, IEnumerable<string> values)
        {
            if (edit == null) return;
            try
            {
                edit.Properties.Items.BeginUpdate();
                try
                {
                    var valuesSet = new HashSet<string>((values ?? Enumerable.Empty<string>()).Where(v => !string.IsNullOrWhiteSpace(v)));
                    for (var i = 0; i < edit.Properties.Items.Count; i++)
                    {
                        var itemValue = edit.Properties.Items[i].Value?.ToString();
                        edit.Properties.Items[i].CheckState = (!string.IsNullOrEmpty(itemValue) && valuesSet.Contains(itemValue))
                            ? CheckState.Checked
                            : CheckState.Unchecked;
                    }
                }
                finally
                {
                    edit.Properties.Items.EndUpdate();
                }
            }
            catch
            {
                // ignore
            }
        }

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

        private void cboLocations_AddNewValue(object sender, AddNewValueEventArgs e)
        {
            Spectrum.Views.Common.Areas.LocationEditForm frm = new Spectrum.Views.Common.Areas.LocationEditForm(new Spectrum.Models.Common.Areas.LocationModel());
            frm.SendUpdatedLocation += RcvUpdatedLocation;
            frm.ShowDialog();
        }

        private void cboServicesProvided_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.Kind != ButtonPredefines.Plus) return;
            var frm = new ServiceEditForm(new ServiceModel());
            frm.SendUpdatedService += RcvUpdatedService;
            frm.ShowDialog();
        }

        private void RcvUpdatedService(object sender, EventArgs e)
        {
            if (sender == null) return;
            var service = sender as ServiceModel;
            if (service == null) return;

         if (_services.All(x => !string.Equals(x.ServiceName, service.ServiceName, StringComparison.OrdinalIgnoreCase)))
            {
                _services.Add(service);
            }

            var selectedValues = GetCheckedItems(cboServicesProvided);
            selectedValues.Add(service.ServiceName);
            BindCheckedComboBoxItems(cboServicesProvided, _services.Select(x => x.ServiceName), selectedValues);
        }

        private void cboServicesType_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.Kind != ButtonPredefines.Plus) return;
            var frm = new ServiceTypeEditForm(new ServiceTypeModel());
            frm.SendUpdatedServiceType += RcvUpdatedServiceType;
            frm.ShowDialog();
        }

        private void RcvUpdatedServiceType(object sender, EventArgs e)
        {
            if (sender == null) return;
            var serviceType = sender as ServiceTypeModel;
            if (serviceType == null) return;

         if (_serviceTypes.All(x => !string.Equals(x.ServiceType, serviceType.ServiceType, StringComparison.OrdinalIgnoreCase)))
            {
                _serviceTypes.Add(serviceType);
            }

            var selectedValues = GetCheckedItems(cboServicesType);
            selectedValues.Add(serviceType.ServiceType);
            BindCheckedComboBoxItems(cboServicesType, _serviceTypes.Select(x => x.ServiceType), selectedValues);
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
            EngineerEditForm frm = new EngineerEditForm(new EmployeeModel());
            frm.SendUpdatedEngineer += RcvUpdatedEngineer;
            frm.ShowDialog();
        }

        private void cboAreas_AddNewValue(object sender, AddNewValueEventArgs e)
        {
            Spectrum.Views.Common.Areas.AreaEditForm frm = new Spectrum.Views.Common.Areas.AreaEditForm(new Spectrum.Models.Common.Areas.AreaModel());
            frm.SendUpdatedArea += RcvUpdatedArea;
            frm.ShowDialog();
        }

        private void RcvUpdatedArea(object sender, EventArgs e)
        {
            if (sender == null) return;
            _areaModel = sender as AreaModel;

            _areas.Add(_areaModel);

            cboAreas.Properties.DataSource = null;
            cboAreas.Properties.DataSource = _areas;
            if (_areaModel != null) cboAreas.EditValue = _areaModel.AreaCode;
        }

        private void RcvUpdatedLocation(object sender, EventArgs e)
        {
            if (sender == null) return;
            _locationModel = sender as LocationModel;

            _locations.Add(_locationModel);

            cboLocations.Properties.DataSource = null;
            cboLocations.Properties.DataSource = _locations;
            if (_locationModel != null) cboLocations.EditValue = _locationModel._id;
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
            _engineerModel = sender as EmployeeModel;

            _engineers.Add(_engineerModel);

            cboEngineers.Properties.DataSource = null;
            cboEngineers.Properties.DataSource = _engineers;
            if (_engineerModel != null) cboEngineers.EditValue = _engineerModel._id;
        }

        private void cboEngineers_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboEngineers.EditValue == null) return;

                var selectedEngineerId = cboEngineers.EditValue.ToString();
                var selectedEngineer = _engineers.FirstOrDefault(x => x._id == selectedEngineerId);
                if (selectedEngineer == null) return;

                var previousEngineerId = _projectModel.EngineerIdValue;
                var previousEngineerName = _projectModel.EngineerInCharge;
                if (!string.IsNullOrWhiteSpace(previousEngineerId) &&
                    !string.Equals(previousEngineerId, selectedEngineerId, StringComparison.OrdinalIgnoreCase) &&
                    !string.IsNullOrWhiteSpace(previousEngineerName))
                {
                    _projectHandovers.Add(new ProjectHandoverModel
                    {
                        _id = Guid.NewGuid().ToString("N"),
                        HandingOver = previousEngineerName,
                        HandingOverDate = DateTime.UtcNow
                    });
                    gvJobDetails.RefreshData();
                }

                _projectModel.EngineerIdValue = selectedEngineer._id;
                _projectModel.EngineerInCharge = selectedEngineer.FullName;
                bsProject.ResetBindings(false);
            }
            catch
            {
                // ignore
            }
        }

        private void RcvUpdatedClient(object sender, EventArgs e)
        {
            if (sender == null) return;
            _clientModel = sender as ClientModel;

            _clients.Add(_clientModel);

            cboClients.Properties.DataSource = null;
            cboClients.Properties.DisplayMember = "ClientName";
            cboClients.Properties.ValueMember = "_id";
            cboClients.Properties.DataSource = _clients;
            if (_clientModel != null) cboClients.EditValue = _clientModel._id;
        }

        private void tabDetails_Click(object sender, EventArgs e)
        {
            switch (tabDetails.SelectedPage.Name)
            {
                case "tabSellingGroup":
                    rpInvoices.Visible = true;
                    rpExpenses.Visible = false;
                    rcMain.SelectPage(rpInvoices);
                    break;

                case "tabCostGroup":
                    rpInvoices.Visible = false;
                    rpExpenses.Visible = true;
                    rcMain.SelectPage(rpExpenses);
                    break;

                default:
                    rpInvoices.Visible = false;
                    rpExpenses.Visible = false;
                    rcMain.SelectPage(rpMain);
                    break;
            }
        }

        #region Helper Methods

        private bool ConfirmDelete(string itemName)
        {
            return XtraMessageBox.Show(
                $"Are you sure you want to delete Record: `{itemName}`?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) == DialogResult.Yes;
        }

        private void HandleDeleteError(Exception ex)
        {
            string message;
            switch (ex.Message)
            {
                case "-2146233088":
                    message = "This record is linked to one or more transactions, delete all links first.";
                    break;
                default:
                    message = ex.Message;
                    break;
            }

            XtraMessageBox.Show(message, "Delete error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ShowError(string message, Exception ex)
        {
            XtraMessageBox.Show(
                $"{message}\n\nDetails: {ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        #endregion

        private void cboLocations_EditValueChanged_1(object sender, EventArgs e)
        {
            try
            {
                if (_projectModel.Location != null && !string.IsNullOrWhiteSpace(_projectModel.Location._id))
                {
                    cboLocations.EditValue = _projectModel.Location._id;
                }
            }
            catch (Exception ex)
            {
                ShowError("Error while selecting location", ex);
            }
        }
    }
}
