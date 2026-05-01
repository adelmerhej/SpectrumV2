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
using SpectrumV1.DataLayers.HumanResources.Employees;
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

        private void cboCountries_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboCountries.EditValue == null) return;
                var id = cboCountries.EditValue.ToString();
                var selected = _countries.FirstOrDefault(x => x._id == id);
                if (selected == null) return;

                // filter cities to selected country
                var filtered = _cities.Where(c => string.Equals(c.Country, selected.CountryName, StringComparison.OrdinalIgnoreCase)).ToList();
                cboCities.Properties.DataSource = null;
                cboCities.Properties.DataSource = filtered;

                // ensure project model Location.Country is set via reflection
                var locationProp = typeof(ProjectModel).GetProperty("Location");
                var locationInfoType = typeof(ProjectModel).Assembly.GetType("Spectrum.Models.Projects.LocationInfoModel");
                if (locationProp == null || locationInfoType == null) return;

                var locationInfo = locationProp.GetValue(_projectModel);
                if (locationInfo == null)
                {
                    locationInfo = Activator.CreateInstance(locationInfoType);
                    locationProp.SetValue(_projectModel, locationInfo);
                }

                locationInfoType.GetProperty("Country")?.SetValue(locationInfo, selected.CountryName);
            }
            catch
            {
                // ignore
            }
        }

        private void cboCities_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboCities.EditValue == null) return;
                var id = cboCities.EditValue.ToString();
                var selected = _cities.FirstOrDefault(x => x._id == id);
                if (selected == null) return;

                var locationProp = typeof(ProjectModel).GetProperty("Location");
                var locationInfoType = typeof(ProjectModel).Assembly.GetType("Spectrum.Models.Projects.LocationInfoModel");
                if (locationProp == null || locationInfoType == null) return;

                var locationInfo = locationProp.GetValue(_projectModel);
                if (locationInfo == null)
                {
                    locationInfo = Activator.CreateInstance(locationInfoType);
                    locationProp.SetValue(_projectModel, locationInfo);
                }

                locationInfoType.GetProperty("City")?.SetValue(locationInfo, selected.CityName);

                // if country is empty on location, set it from city.Country
                var countryProp = locationInfoType.GetProperty("Country");
                if (countryProp != null)
                {
                    var cur = countryProp.GetValue(locationInfo) as string;
                    if (string.IsNullOrEmpty(cur) && !string.IsNullOrEmpty(selected.Country))
                    {
                        // try to find matching country name
                        var countryMatch = _countries.FirstOrDefault(c => string.Equals(c.CountryName, selected.Country, StringComparison.OrdinalIgnoreCase) || string.Equals(c.CountryCode, selected.Country, StringComparison.OrdinalIgnoreCase));
                        if (countryMatch != null)
                        {
                            countryProp.SetValue(locationInfo, countryMatch.CountryName);
                            cboCountries.EditValue = countryMatch._id;
                        }
                    }
                }
            }
            catch
            {
                // ignore
            }
        }

        private void SyncContractClientFromProjectClient()
        {
            try
            {
                if (cboContractClient == null) return;
                if (cboClients == null) return;

                var editValue = cboClients.EditValue;
                if (editValue == null)
                {
                    cboContractClient.EditValue = null;
                    return;
                }

                // Preferred behavior: both combos use client _id as EditValue.
                var value = editValue.ToString();
                if (_clients == null) return;

                var client = _clients.FirstOrDefault(c => c._id == value)
                    ?? _clients.FirstOrDefault(c => string.Equals(c.ClientName, value, StringComparison.OrdinalIgnoreCase))
                    ?? _clients.FirstOrDefault(c => string.Equals(c.ClientName, cboClients.Text, StringComparison.OrdinalIgnoreCase));

                if (client != null) cboContractClient.EditValue = client._id;
            }
            catch
            {
                // ignore
            }
        }

        private void cboLocations_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboLocations.EditValue == null) return;
                var id = cboLocations.EditValue.ToString();
                var selected = _locations.FirstOrDefault(x => x._id == id);
                if (selected == null) return;

                // set project model Location (type is LocationInfoModel). Since we don't have a
                // strong reference to its definition here, populate it using reflection.
                var locationInfoType = typeof(ProjectModel).Assembly.GetType("Spectrum.Models.Projects.LocationInfoModel");
                if (locationInfoType == null) return;

                var locationInfo = Activator.CreateInstance(locationInfoType);
                // Project uses LocationInfoModel which stores RawLocation/Country/City.
                // Populate RawLocation from selected location's LocationName so reports see it.
                locationInfoType.GetProperty("RawLocation")?.SetValue(locationInfo, selected.LocationName);
                // Keep LocationCode as fallback if the info model has a matching property
                locationInfoType.GetProperty("LocationCode")?.SetValue(locationInfo, selected.LocationCode);
                // Attempt to set an _id if the info model stores it (non-standard)
                locationInfoType.GetProperty("_id")?.SetValue(locationInfo, selected._id);

                typeof(ProjectModel).GetProperty("Location")?.SetValue(_projectModel, locationInfo);
            }
            catch
            {
                // ignore
            }
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
                _engineers = await _engineerRepository.GetEmployeesAsync(EnumEmployeeType.Engineer);
                _users = await _userRepository.GetUsersAsync();
                _services = await _serviceRepository.GetServicesAsync();
                _serviceTypes = await _serviceTypeRepository.GetServiceTypesAsync();
                _locations = await _locationRepository.GetLocationsAsync();
                _areas = await _areaRepository.GetAreasAsync();
                _countries = await _countryRepository.GetCountriesAsync();
                _cities = await _cityRepository.GetCitiesAsync();
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

            // Contract management should share the same Client list as Project Information.
            // Keep it using the same key (client _id) so newly added clients are available immediately.
            cboContractClient.Properties.DataSource = null;
            cboContractClient.Properties.DataSource = _clients;

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

            cboAreas.Properties.DataSource = null;
            cboAreas.Properties.DisplayMember = "AreaCode";
            cboAreas.Properties.ValueMember = "AreaCode";
            cboAreas.Properties.DataSource = _areas;

            cboLocations.Properties.DataSource = null;
            cboLocations.Properties.DisplayMember = "LocationName";
            cboLocations.Properties.ValueMember = "_id";
            cboLocations.Properties.DataSource = _locations;

            // Countries and Cities - bind to repositories
            cboCountries.Properties.DataSource = null;
            cboCountries.Properties.DisplayMember = "CountryName";
            cboCountries.Properties.ValueMember = "_id";
            cboCountries.Properties.DataSource = _countries;

            cboCities.Properties.DataSource = null;
            cboCities.Properties.DisplayMember = "CityName";
            cboCities.Properties.ValueMember = "_id";
            cboCities.Properties.DataSource = _cities;

            // Set initial selection from project model (if any)
            try
            {
                var locProp = _projectModel.GetType().GetProperty("Location");
                if (locProp != null)
                {
                    var locVal = locProp.GetValue(_projectModel);
                    if (locVal != null)
                    {
                        var idProp = locVal.GetType().GetProperty("_id");
                        if (idProp != null)
                        {
                            var id = idProp.GetValue(locVal) as string;
                            if (!string.IsNullOrEmpty(id)) cboLocations.EditValue = id;
                        }
                        else
                        {
                            var nameProp = locVal.GetType().GetProperty("LocationName");
                            if (nameProp != null)
                            {
                                var name = nameProp.GetValue(locVal) as string;
                                if (!string.IsNullOrEmpty(name))
                                {
                                    var match = _locations.FirstOrDefault(x => x.LocationName == name);
                                    if (match != null) cboLocations.EditValue = match._id;
                                }
                            }
                            else
                            {
                                // Some LocationInfoModel instances use RawLocation instead of LocationName
                                var rawProp = locVal.GetType().GetProperty("RawLocation");
                                if (rawProp != null)
                                {
                                    var raw = rawProp.GetValue(locVal) as string;
                                    if (!string.IsNullOrEmpty(raw))
                                    {
                                        var matchRaw = _locations.FirstOrDefault(x => x.LocationName == raw);
                                        if (matchRaw != null) cboLocations.EditValue = matchRaw._id;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                // ignore any reflection errors and leave selection unset
            }

            // Whenever user changes selection, update project model Location via reflection
            cboLocations.EditValueChanged += cboLocations_EditValueChanged;

            // Ensure issuance date and year controls reflect model values
            try
            {
                if (_projectModel != null)
                {
                    if (_projectModel.IssuanceDate.HasValue)
                        dtIssuanceDate.EditValue = _projectModel.IssuanceDate.Value;

                    if (_projectModel.YearOfIssuance.HasValue)
                        dtIssuanceYear.EditValue = _projectModel.YearOfIssuance.Value;
                }
            }
            catch
            {
                // ignore
            }

            // Countries/Cities initial selection from project model (if any)
            try
            {
                var locProp2 = _projectModel.GetType().GetProperty("Location");
                if (locProp2 != null)
                {
                    var locVal2 = locProp2.GetValue(_projectModel);
                    if (locVal2 != null)
                    {
                        var countryProp = locVal2.GetType().GetProperty("Country");
                        if (countryProp != null)
                        {
                            var countryName = countryProp.GetValue(locVal2) as string;
                            if (!string.IsNullOrEmpty(countryName))
                            {
                                var match = _countries.FirstOrDefault(x => string.Equals(x.CountryName, countryName, StringComparison.OrdinalIgnoreCase));
                                if (match != null) cboCountries.EditValue = match._id;
                            }
                        }

                        var cityProp = locVal2.GetType().GetProperty("City");
                        if (cityProp != null)
                        {
                            var cityName = cityProp.GetValue(locVal2) as string;
                            if (!string.IsNullOrEmpty(cityName))
                            {
                                var matchCity = _cities.FirstOrDefault(x => string.Equals(x.CityName, cityName, StringComparison.OrdinalIgnoreCase));
                                if (matchCity != null) cboCities.EditValue = matchCity._id;
                            }
                        }
                    }
                }
            }
            catch
            {
                // ignore
            }

            // Wire countries and cities changes
            cboCountries.EditValueChanged -= cboCountries_EditValueChanged;
            cboCountries.EditValueChanged += cboCountries_EditValueChanged;

            cboCities.EditValueChanged -= cboCities_EditValueChanged;
            cboCities.EditValueChanged += cboCities_EditValueChanged;

            // Keep contract client in sync with project client.
            cboClients.EditValueChanged -= cboClients_EditValueChanged;
            cboClients.EditValueChanged += cboClients_EditValueChanged;
            SyncContractClientFromProjectClient();

            // Load checked items from project model
            SetCheckedItems(cboServicesProvided, _projectModel.ServicesProvided);
            SetCheckedItems(cboServicesType, _projectModel.ServiceTypes);

            //cboStatus.Properties.Items.Clear();
            //cboStatus.Properties.Items.AddRange(Enum.GetNames(typeof(ProjectStatus)));
        }

        private void cboClients_EditValueChanged(object sender, EventArgs e)
        {
            SyncContractClientFromProjectClient();
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
                    if (selectedEngineer != null) _projectModel.EngineerInCharge = selectedEngineer.FirstName;
                }

                // Save selected services and service types
                _projectModel.ServicesProvided = GetCheckedItems(cboServicesProvided);
                _projectModel.ServiceTypes = GetCheckedItems(cboServicesType);

                // Ensure Location.Country and Location.City are set from selected lookups
                try
                {
                    var locationProp = typeof(ProjectModel).GetProperty("Location");
                    var locationInfoType = typeof(ProjectModel).Assembly.GetType("Spectrum.Models.Projects.LocationInfoModel");
                    if (locationProp != null && locationInfoType != null)
                    {
                        var locationInfo = locationProp.GetValue(_projectModel);
                        if (locationInfo == null)
                        {
                            locationInfo = Activator.CreateInstance(locationInfoType);
                            locationProp.SetValue(_projectModel, locationInfo);
                        }

                        if (cboCountries.EditValue != null)
                        {
                            var country = _countries.FirstOrDefault(c => c._id == cboCountries.EditValue.ToString());
                            if (country != null) locationInfoType.GetProperty("Country")?.SetValue(locationInfo, country.CountryName);
                        }

                        if (cboCities.EditValue != null)
                        {
                            var city = _cities.FirstOrDefault(c => c._id == cboCities.EditValue.ToString());
                            if (city != null) locationInfoType.GetProperty("City")?.SetValue(locationInfo, city.CityName);
                        }

                        // If a location (from the Locations lookup) is selected, store its display name into RawLocation
                        if (cboLocations.EditValue != null)
                        {
                            var loc = _locations.FirstOrDefault(l => l._id == cboLocations.EditValue.ToString());
                            if (loc != null) locationInfoType.GetProperty("RawLocation")?.SetValue(locationInfo, loc.LocationName);
                        }
                    }
                }
                catch
                {
                    // ignore
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
                    // Ensure issuance date/year are propagated to the model from the UI controls
                    try
                    {
                        if (dtIssuanceDate != null && dtIssuanceDate.EditValue != null && DateTime.TryParse(dtIssuanceDate.EditValue.ToString(), out var issDate))
                        {
                            _projectModel.IssuanceDate = issDate;
                        }

                        if (dtIssuanceYear != null && dtIssuanceYear.EditValue != null)
                        {
                            if (int.TryParse(dtIssuanceYear.EditValue.ToString(), out var y)) _projectModel.YearOfIssuance = y;
                            else if (_projectModel.IssuanceDate.HasValue) _projectModel.YearOfIssuance = _projectModel.IssuanceDate.Value.Year;
                        }
                        else if (_projectModel.IssuanceDate.HasValue)
                        {
                            _projectModel.YearOfIssuance = _projectModel.IssuanceDate.Value.Year;
                        }
                    }
                    catch
                    {
                        // ignore parsing errors
                    }

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

            _services.Add(service);
            cboServicesProvided.Properties.DataSource = null;
            cboServicesProvided.Properties.DataSource = _services;

            cboServicesProvided.Properties.Items.Add(service.ServiceName, true);
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

            _serviceTypes.Add(serviceType);
            cboServicesType.Properties.DataSource = null;
            cboServicesType.Properties.DataSource = _serviceTypes;

            cboServicesType.Properties.Items.Add(serviceType.ServiceType, true);
        }

        private void RcvUpdatedCountry(object sender, EventArgs e)
        {
            if (sender == null) return;
            _countryModel = sender as CountryModel;

            _countries.Add(_countryModel);

            cboCountries.Properties.DataSource = null;
            cboCountries.Properties.DataSource = _countries;
            if (_countryModel != null) cboCountries.EditValue = _countryModel._id;
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
            if (_engineerModel != null) cboEngineers.EditValue = _engineerModel.FullName;
        }

        private void RcvUpdatedClient(object sender, EventArgs e)
        {
            if (sender == null) return;
            _clientModel = sender as ClientModel;

            _clients.Add(_clientModel);

            cboClients.Properties.DataSource = null;
            cboClients.Properties.DataSource = _clients;

            // Refresh contract management client lookup as well
            cboContractClient.Properties.DataSource = null;
            cboContractClient.Properties.DataSource = _clients;

            if (_clientModel != null)
            {
                // Prefer selecting by id (ValueMember is typically _id on these lookups)
                cboClients.EditValue = _clientModel._id;
                SyncContractClientFromProjectClient();
            }
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
    }
}
