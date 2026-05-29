using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.WinExplorer;
using Spectrum.DataLayers.Common.Areas;
using Spectrum.DataLayers.Common.Countries;
using Spectrum.DataLayers.Common.Locations;
using Spectrum.DataLayers.Common.Services;
using Spectrum.DataLayers.DataAccess;
using Spectrum.DataLayers.HumanResources.Employees;
using Spectrum.DataLayers.Members.Clients;
using Spectrum.DataLayers.Projects;
using Spectrum.DataLayers.Projects.Settings.Addendum;
using Spectrum.DataLayers.Projects.Settings.ProjectTypes;
using Spectrum.DataLayers.Users;
using Spectrum.Models.Common.Areas;
using Spectrum.Models.Common.Countries;
using Spectrum.Models.Common.Documents;
using Spectrum.Models.Common.Services;
using Spectrum.Models.HumanResources.Employees;
using Spectrum.Models.Members.Clients;
using Spectrum.Models.Operations.Projects.Settings.ProjectTypes;
using Spectrum.Models.Projects;
using Spectrum.Models.Users;
using Spectrum.Utilities;
using Spectrum.Utilities.Enums;
using Spectrum.Views.Common.Countries;
using Spectrum.Views.Common.Services;
using Spectrum.Views.Members.Clients;
using Spectrum.Views.Members.Engineers;
using Spectrum.Views.Projects.Settings.Addendum;
using Spectrum.Views.Projects.Settings.ProjectTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
        private BindingList<DocumentModel> _documents = new BindingList<DocumentModel>();

        private IList<ClientModel> _clients = new List<ClientModel>();
        private ClientModel _clientModel = new ClientModel();
        private IList<ContactModel> _contacts = new List<ContactModel>();

        private IList<EmployeeModel> _engineers = new List<EmployeeModel>();
        private EmployeeModel _engineerModel = new EmployeeModel();

        private UserModel _userModel = new UserModel();
        private IList<UserModel> _users = new List<UserModel>();

        private IList<CountryModel> _countries = new List<CountryModel>();
        private CountryModel _countryModel = new CountryModel();

        private IList<CityModel> _cities = new List<CityModel>();
        private CityModel _cityModel = new CityModel();

        private AddendumModel _addendumModel = new AddendumModel();
        private IList<AddendumModel> _addendums = new List<AddendumModel>();

        private IList<AreaModel> _areas = new List<AreaModel>();
        private AreaModel _areaModel = new AreaModel();
        private IList<LocationModel> _locations = new List<LocationModel>();
        private LocationModel _locationModel = new LocationModel();

        private IList<ServiceModel> _services = new List<ServiceModel>();
        private IList<ServiceTypeModel> _serviceTypes = new List<ServiceTypeModel>();
        private IList<ProjectTypeModel> _projectTypes = new List<ProjectTypeModel>();

        private readonly ProjectRepository _projectRepository = new ProjectRepository(DatabaseFactory.ProfilePrimary);
        private readonly ClientRepository _clientRepository = new ClientRepository(DatabaseFactory.ProfilePrimary);
        private readonly ContactRepository _contactRepository = new ContactRepository(DatabaseFactory.ProfilePrimary);
        private readonly EmployeeRepository _engineerRepository = new EmployeeRepository(DatabaseFactory.ProfilePrimary);
        private readonly UserRepository _userRepository = new UserRepository(DatabaseFactory.ProfilePrimary);
        private readonly ServiceRepository _serviceRepository = new ServiceRepository(DatabaseFactory.ProfilePrimary);
        private readonly ServiceTypeRepository _serviceTypeRepository = new ServiceTypeRepository(DatabaseFactory.ProfilePrimary);
        private readonly CountryRepository _countryRepository = new CountryRepository(DatabaseFactory.ProfilePrimary);
        private readonly CityRepository _cityRepository = new CityRepository(DatabaseFactory.ProfilePrimary);
        private readonly LocationRepository _locationRepository = new LocationRepository(DatabaseFactory.ProfilePrimary);
        private readonly AreaRepository _areaRepository = new AreaRepository(DatabaseFactory.ProfilePrimary);
        private readonly AddendumRepository _addendumRepository = new AddendumRepository(DatabaseFactory.ProfilePrimary);
        private readonly ProjectTypeRepository _projectTypeRepository = new ProjectTypeRepository(DatabaseFactory.ProfilePrimary);

        private readonly LogInfoRepository _logInfoRepository = new LogInfoRepository();

        private bool _canAdd = true;
        private bool _canEdit = true;
        private bool _canDelete = true;
        private bool _canPrint = true;
        private bool _isAdmin = true;
        private bool _isProtected = true;
        private readonly List<string> _loadWarnings = new List<string>();
        private DXMenuItem[] _addendumMenuItems;

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
                InitializeMenuItems();
                if (_loadWarnings.Any())
                {
                    XtraMessageBox.Show(
                        "Some lookup data could not be loaded. The form will remain available with partial data.\n\n" + string.Join("\n", _loadWarnings),
                        "Lookup Data Warning",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                ShowError("Error during form initialization", ex);
            }
        }

        #region InitializeBindings Methods
        private async Task InitializeBindings()
        {
            _loadWarnings.Clear();

            var loadTasks = new[]
            {
                LoadSafelyAsync("clients", LoadClientsAsync, () => _clients = new List<ClientModel>()),
                LoadSafelyAsync("contacts", LoadContactsAsync, () => _contacts = new List<ContactModel>()),
                LoadSafelyAsync("engineers", LoadEngineersAsync, () => _engineers = new List<EmployeeModel>()),
                LoadSafelyAsync("users", LoadUsersAsync, () => _users = new List<UserModel>()),
                LoadSafelyAsync("services", LoadServicesAsync, () => _services = new List<ServiceModel>()),
                LoadSafelyAsync("service types", LoadServiceTypesAsync, () => _serviceTypes = new List<ServiceTypeModel>()),
                LoadSafelyAsync("locations", LoadLocationsAsync, () => _locations = new List<LocationModel>()),
                LoadSafelyAsync("areas", LoadAreasAsync, () => _areas = new List<AreaModel>()),
                LoadSafelyAsync("countries", LoadCountriesAsync, () => _countries = new List<CountryModel>()),
                LoadSafelyAsync("cities", LoadCitiesAsync, () => _cities = new List<CityModel>()),
                LoadSafelyAsync("addendums", LoadAddendumsAsync, () => _addendums = new List<AddendumModel>()),
                LoadSafelyAsync("project types", LoadProjectTypesAsync, () => _projectTypes = new List<ProjectTypeModel>()),
            };

            await Task.WhenAll(loadTasks);
        }

        private async Task LoadSafelyAsync(string sourceName, Func<Task> loader, Action fallback)
        {
            try
            {
                await loader();
            }
            catch (Exception ex)
            {
                fallback?.Invoke();
                _loadWarnings.Add($"- {sourceName}: {ex.Message}");
            }
        }

        private void InitializeMenuItems()
        {
            _addendumMenuItems = new[]
            {
                new DXMenuItem("New", ItemNew_Click),
                new DXMenuItem("Edit", ItemEdit_Click),
                new DXMenuItem("Delete", ItemDelete_Click)
            };

            gvAddendum.PopupMenuShowing -= gvAddendum_PopupMenuShowing;
            gvAddendum.PopupMenuShowing += gvAddendum_PopupMenuShowing;
        }

        #region Data Loading Methods

        private async Task LoadClientsAsync()
        {
            _clients = await _clientRepository.GetClientsAsync();
        }

        private async Task LoadContactsAsync()
        {
            _contacts = await _contactRepository.GetContactsAsync();
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

        private async Task LoadAddendumsAsync()
        {
            await Task.CompletedTask;
            _addendums = (_projectModel.Addendums ?? new List<AddendumModel>()).ToList();
        }

        private async Task LoadProjectTypesAsync()
        {
            _projectTypes = await _projectTypeRepository.GetProjectTypesAsync();
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
            InitializeDocumentBindings();

            bsProject.DataSource = _projectModel;
            bsContractDetails.DataSource = _projectModel.ContractDetails;
            bsLocationInfo.DataSource = _projectModel.Location;

            gcAddendum.DataSource = _addendums;
            InitializeProjectHistoryTimeline();

            ConfigureLookupBindings();

            BindCheckedComboBoxItems(cboServicesProvided,
                 _services.Select(x => x.ServiceName),
                 _projectModel.ContractDetails.ServicesProvided);

            cboAreas.Properties.DataSource = null;
            cboAreas.Properties.DataSource = _areas;

            cboLocations.Properties.DataSource = null;
            cboLocations.Properties.DataSource = _locations;

            cboCountries.Properties.DataSource = null;
            cboCountries.Properties.DataSource = _countries;

            cboCities.Properties.DataSource = null;
            cboCities.Properties.DataSource = _cities;

            if (_projectModel.Location != null && !string.IsNullOrWhiteSpace(_projectModel.Location._id))
            {
                cboLocations.EditValue = _projectModel.Location._id;
            }

            // Whenever user changes selection, update project model Location via reflection
            cboLocations.EditValueChanged += cboLocations_EditValueChanged;

            //cboStatus.Properties.Items.Clear();
            //cboStatus.Properties.Items.AddRange(Enum.GetNames(typeof(ProjectStatus)));

            var fontStyle = _projectModel.IsProtected ? FontStyle.Bold : FontStyle.Regular;
            var foreColor = _projectModel.IsProtected ? Color.Red : Color.Black;
            var font = new Font("Tahoma", 8, fontStyle);

            var appearances = new[]
            {
                btnProtected.ItemAppearance.Normal,
                btnProtected.ItemAppearance.Hovered,
                btnProtected.ItemAppearance.Pressed
            };

            foreach (var appearance in appearances)
            {
                appearance.Font = font;
                appearance.ForeColor = foreColor;
            }

            HookProjectHistoryEvents();
            RefreshProjectHistoryTimeline();
        }

        #endregion

        private void ConfigureLookupBindings()
        {
            cboProjectType.DataBindings.Clear();
            cboProjectType.DataBindings.Add("EditValue", bsProject, "ProjectType", true, DataSourceUpdateMode.OnPropertyChanged);
            cboProjectType.Properties.DataSource = null;
            cboProjectType.Properties.DisplayMember = "Sector";
            cboProjectType.Properties.ValueMember = "Sector";
            cboProjectType.Properties.DataSource = _projectTypes;
            cboProjectType.AddNewValue -= cboProjectType_AddNewValue;
            cboProjectType.AddNewValue += cboProjectType_AddNewValue;

            cboClients.DataBindings.Clear();
            cboClients.DataBindings.Add("EditValue", bsProject, "ClientIdValue", true, DataSourceUpdateMode.OnPropertyChanged);
            cboClients.Properties.DataSource = null;
            cboClients.Properties.DisplayMember = "ClientName";
            cboClients.Properties.ValueMember = "_id";
            cboClients.Properties.DataSource = _clients;
            ConfigurePopupGridColumns(searchLookUpEdit1View,
                ("ClientInitial", "Initial", 60),
                ("ClientName", "Client Name", 220));

            cboFundedBy.DataBindings.Clear();
            cboFundedBy.DataBindings.Add("EditValue", bsContractDetails, "SponsorId", true, DataSourceUpdateMode.OnPropertyChanged);
            cboFundedBy.Properties.DataSource = null;
            cboFundedBy.Properties.DisplayMember = "ClientName";
            cboFundedBy.Properties.ValueMember = "_id";
            cboFundedBy.Properties.DataSource = _clients;
            ConfigurePopupGridColumns(gridView12,
                ("ClientInitial", "Initial", 60),
                ("ClientName", "Client Name", 220));

            cboEngineers.DataBindings.Clear();
            cboEngineers.DataBindings.Add("EditValue", bsProject, "EngineerIdValue", true, DataSourceUpdateMode.OnPropertyChanged);
            cboEngineers.Properties.DataSource = null;
            cboEngineers.Properties.DisplayMember = "FullName";
            cboEngineers.Properties.ValueMember = "_id";
            cboEngineers.Properties.DataSource = _engineers;
            ConfigurePopupGridColumns(gridView1,
                ("EmployeeNo", "No.", 50),
                ("FullName", "Engineer", 200));

            cboUsers.DataBindings.Clear();
            cboUsers.DataBindings.Add("EditValue", bsProject, "UserIdValue", true, DataSourceUpdateMode.OnPropertyChanged);
            cboUsers.Properties.DataSource = null;
            cboUsers.Properties.DisplayMember = "Username";
            cboUsers.Properties.ValueMember = "_id";
            cboUsers.Properties.DataSource = _users;
            ConfigurePopupGridColumns(gridView3,
                ("Username", "Username", 180));

            cboPersonInCharge.DataBindings.Clear();
            cboPersonInCharge.DataBindings.Add("EditValue", bsProject, "EngineerIdValue", true, DataSourceUpdateMode.OnPropertyChanged);
            cboPersonInCharge.Properties.DataSource = null;
            cboPersonInCharge.Properties.DisplayMember = "FullName";
            cboPersonInCharge.Properties.ValueMember = "_id";
            cboPersonInCharge.Properties.DataSource = _engineers;
            ConfigurePopupGridColumns(gridView4,
                ("EmployeeNo", "No.", 50),
                ("FullName", "Engineer", 200));

            cboEngineers.EditValueChanged -= cboEngineers_EditValueChanged;
            cboEngineers.EditValueChanged += cboEngineers_EditValueChanged;

            // cboClientContact — stores the contact name in ProjectModel.ClientContact
            cboClientContact.DataBindings.Clear();
            cboClientContact.DataBindings.Add("EditValue", bsProject, "ClientContact", true, DataSourceUpdateMode.OnPropertyChanged);
            cboClientContact.Properties.DisplayMember = "ContactName";
            cboClientContact.Properties.ValueMember = "ContactName";
            ConfigurePopupGridColumns(gridView10,
                ("ContactName", "Contact Name", 200),
                ("Email", "Email", 400));
            cboClientContact.EditValueChanged -= cboClientContact_EditValueChanged;
            cboClientContact.EditValueChanged += cboClientContact_EditValueChanged;

            // txtContactEmail — value managed manually (no DataBinding; SearchLookUpEdit
            // re-validates EditValue against DataSource on every source reassignment which
            // silently wipes the bound model value through the binding write-back path).
            txtContactEmail.DataBindings.Clear();
            txtContactEmail.Properties.DisplayMember = "Email";
            txtContactEmail.Properties.ValueMember = "Email";
            ConfigurePopupGridColumns(gridView13,
                ("ContactName", "Contact Name", 200),
                ("Email", "Email", 400));

            RefreshContactEmailSource();

            cboClients.EditValueChanged -= cboClients_EditValueChanged;
            cboClients.EditValueChanged += cboClients_EditValueChanged;
        }

        private static void ConfigurePopupGridColumns(GridView view, params (string fieldName, string caption, int width)[] columns)
        {
            view.Columns.Clear();
            view.OptionsBehavior.AutoPopulateColumns = false;
            foreach (var (fieldName, caption, width) in columns)
            {
                var col = view.Columns.AddField(fieldName);
                col.Caption = caption;
                col.Width = width;
                col.Visible = true;
            }
        }

        private void InitializeProjectHandovers()
        {
            _projectHandovers = new BindingList<ProjectHandoverModel>(_projectModel.ProjectHandovers.ToList());
            gcJobDetails.DataSource = _projectHandovers;
        }

        private void InitializeDocumentBindings()
        {
            _documents = LoadDocumentsForProject();
            gcDocuments.DataSource = _documents;
        }

        private BindingList<DocumentModel> LoadDocumentsForProject()
        {
            var documents = new BindingList<DocumentModel>();
            var projectId = _projectModel?._id;
            if (string.IsNullOrWhiteSpace(projectId))
                return documents;

            var connection = DatabaseFactory.GetConnection(DatabaseFactory.ProfilePrimary);
            var rootFolder = string.IsNullOrWhiteSpace(connection?.ProjectsDocumentsFolder)
                ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SpectrumApp", "Projects")
                : connection.ProjectsDocumentsFolder;

            if (!Directory.Exists(rootFolder))
                return documents;

            var prefix = projectId + "_";
            var files = Directory.GetFiles(rootFolder)
                .Where(path => Path.GetFileName(path).StartsWith(prefix, StringComparison.OrdinalIgnoreCase));

            foreach (var filePath in files)
            {
                try
                {
                    documents.Add(new DocumentModel
                    {
                        DocumentName = GetDisplayDocumentName(projectId, filePath),
                        OriginPath = filePath,
                        DocumentDate = File.GetCreationTime(filePath),
                        StreamedDate = DateTime.Now,
                        DocumentContent = File.ReadAllBytes(filePath)
                    });
                }
                catch
                {
                }
            }

            return documents;
        }

        private static string GetDisplayDocumentName(string recordId, string filePath)
        {
            var fileName = Path.GetFileName(filePath);
            var prefix = (recordId ?? string.Empty) + "_";
            return fileName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
                ? fileName.Substring(prefix.Length)
                : fileName;
        }

        private static BindingList<DocumentModel> LoadDocumentsFromSourceFile(string sourceFile)
        {
            var documents = new BindingList<DocumentModel>();
            if (string.IsNullOrWhiteSpace(sourceFile)) return documents;

            var filePaths = sourceFile.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x));

            foreach (var filePath in filePaths)
            {
                try
                {
                    if (!File.Exists(filePath)) continue;

                    documents.Add(new DocumentModel
                    {
                        DocumentName = Path.GetFileName(filePath),
                        OriginPath = filePath,
                        DocumentDate = File.GetCreationTime(filePath),
                        StreamedDate = DateTime.Now,
                        DocumentContent = File.ReadAllBytes(filePath)
                    });
                }
                catch
                {
                }
            }

            return documents;
        }

        private void PersistDocumentLinks()
        {
            if (_projectModel == null) return;

            _projectModel.SourceFile = string.Join(";",
                _documents.Where(x => x != null && !string.IsNullOrWhiteSpace(x.OriginPath))
                    .Select(x => x.OriginPath)
                    .Distinct(StringComparer.OrdinalIgnoreCase));
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

        private void ItemNew_Click(object sender, EventArgs e)
        {
            if (!CanManageAddendums()) return;

            _addendumModel = new AddendumModel();

            AddendumEditForm form = new AddendumEditForm(_addendumModel, _addendums);
            form.SendUpdatedAddendum += RcvUpdatedAddendum;
            form.ShowDialog();
        }

        private void ItemEdit_Click(object sender, EventArgs e)
        {
            if (!CanManageAddendums()) return;

            var addendumId = gvAddendum.GetFocusedRowCellValue("_id")?.ToString();

            if (string.IsNullOrWhiteSpace(addendumId)) return;

            _addendumModel = _addendums.FirstOrDefault(x => x._id == addendumId) ?? new AddendumModel();
            AddendumEditForm form = new AddendumEditForm(_addendumModel, _addendums);
            form.SendUpdatedAddendum += RcvUpdatedAddendum;
            form.ShowDialog();
        }

        private void ItemDelete_Click(object sender, EventArgs e)
        {
            if (!CanManageAddendums()) return;

            if (!ConfirmAction("Delete row?", "Confirmation")) return;
            gvAddendum.SetRowCellValue(gvAddendum.FocusedRowHandle, "Deleted", true);
            RefreshProjectHistoryTimeline();
        }

        private void gvAddendum_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (!CanManageAddendums())
            {
                e.Menu = null;
                return;
            }

            if (e.MenuType != GridMenuType.Row && e.MenuType != GridMenuType.User) return;

            var view = sender as GridView;
            if (view == null) return;

            if (e.HitInfo.InRow || e.HitInfo.InRowCell)
            {
                view.FocusedRowHandle = e.HitInfo.RowHandle;
            }

            if (e.Menu == null)
            {
                e.Menu = new GridViewMenu(view);
            }

            e.Menu.Items.Clear();
            foreach (var menuItem in _addendumMenuItems)
            {
                e.Menu.Items.Add(menuItem);
            }
        }

        private bool CanManageAddendums()
        {
            if (!ValidateData() || string.IsNullOrWhiteSpace(_projectModel?._id))
            {
                XtraMessageBox.Show("Finish project first, then add addendum.", "Addendum",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return true;
        }

        private bool ConfirmAction(string message, string title)
        {
            return XtraMessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) == DialogResult.Yes;
        }

        #region ApplyDefaults and Permissions Methods
        private void ApplyDefaults()
        {
            if (string.IsNullOrEmpty(_projectModel._id))
            {
                _projectModel.Status = ProjectStatus.Active;
                _projectModel.ProjectDate = DateTime.Today;
            }

            SyncIssuanceYearFromDate();

            // Wire document thumbnail/icon rendering
            gvDocuments.GetThumbnailImage += TileView1_GetThumbnailImage;
            gvDocuments.OptionsImageLoad.RandomShow = true;
            gvDocuments.OptionsImageLoad.LoadThumbnailImagesFromDataSource = false;
            gvDocuments.OptionsImageLoad.AsyncLoad = true;

            // Wire document context button events (delete bin + hover visibility)
            gvDocuments.ContextButtonClick += gvDocuments_ContextButtonClick;
            gvDocuments.ContextButtonCustomize += gvDocuments_ContextButtonCustomize;
            gvDocuments.FocusedRowChanged += gvDocuments_FocusedRowChanged;
        }

        private void TileView1_GetThumbnailImage(object sender, ThumbnailImageEventArgs e)
        {
            var fileName = (string)gvDocuments.GetRowCellValue(e.DataSourceIndex, colName);
            var ext = Path.GetExtension(fileName);
            e.ThumbnailImage = HelperApplication.GetFileExtensionImage(ext, IconSizeType.Large, new Size(64, 64));
        }

        private void ApplyPermissions()
        {
            btnNew.Enabled = _isAdmin || _canAdd;
            btnSave.Enabled = _isAdmin || _canEdit;
            btnSaveAndClose.Enabled = _isAdmin || _canEdit;
            btnPrint.Enabled = _isAdmin || _canPrint;
            btnDelete.Enabled = _isAdmin || _canDelete;
            btnProtected.Enabled = _isAdmin || _isProtected;
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

        private void dtIssuanceDate_EditValueChanged(object sender, EventArgs e)
        {
            SyncIssuanceYearFromDate();
            RefreshProjectHistoryTimeline();
        }

        private void ProjectHistoryDate_EditValueChanged(object sender, EventArgs e)
        {
            RefreshProjectHistoryTimeline();
        }

        private void InitializeProjectHistoryTimeline()
        {
            if (projectHistoryTimeline == null) return;
            projectHistoryTimeline.BorderStyle = BorderStyles.NoBorder;
            projectHistoryTimeline.Properties.ReadOnly = true;
            projectHistoryTimeline.Properties.ScrollBars = ScrollBars.Vertical;
            projectHistoryTimeline.Properties.WordWrap = false;
            projectHistoryTimeline.Properties.Appearance.BackColor = Color.White;
            projectHistoryTimeline.Properties.Appearance.Font = new Font("Segoe UI", 9F);
        }

        private void HookProjectHistoryEvents()
        {
            dtIssuanceDate.EditValueChanged -= dtIssuanceDate_EditValueChanged;
            dtIssuanceDate.EditValueChanged += dtIssuanceDate_EditValueChanged;

            dtSignatureDate.EditValueChanged -= ProjectHistoryDate_EditValueChanged;
            dtSignatureDate.EditValueChanged += ProjectHistoryDate_EditValueChanged;

            dtCompletitionDate.EditValueChanged -= ProjectHistoryDate_EditValueChanged;
            dtCompletitionDate.EditValueChanged += ProjectHistoryDate_EditValueChanged;

            dtExpiryDate.EditValueChanged -= ProjectHistoryDate_EditValueChanged;
            dtExpiryDate.EditValueChanged += ProjectHistoryDate_EditValueChanged;
        }

        private void RefreshProjectHistoryTimeline()
        {
            if (projectHistoryTimeline == null) return;

            var items = BuildProjectHistoryTimelineItems();
            projectHistoryTimeline.Text = string.Join(
                Environment.NewLine + Environment.NewLine,
                items.Select(FormatProjectHistoryTimelineItem));
        }

        private List<ProjectHistoryTimelineItem> BuildProjectHistoryTimelineItems()
        {
            var items = new List<ProjectHistoryTimelineItem>();
            var contractDetails = _projectModel?.ContractDetails ?? new ContractDetailModel();

            AddProjectHistoryItem(items, _projectModel?.IssuanceDate, "Issuance Date", _projectModel?.Reference);
            AddProjectHistoryItem(items, contractDetails.SignatureDate, "Signature Date", contractDetails.ContractNumber);

            foreach (var addendum in (_addendums ?? new List<AddendumModel>())
                         .Where(x => x != null && !x.Deleted)
                         .OrderBy(x => x.EffectiveDate ?? x.BODDate ?? DateTime.MaxValue)
                         .ThenBy(x => x.Sequence))
            {
                AddProjectHistoryItem(
                    items,
                    addendum.EffectiveDate ?? addendum.BODDate,
                    $"Addendum {addendum.Sequence}",
                    BuildAddendumHistoryDetails(addendum));
            }

            AddProjectHistoryItem(items, contractDetails.ActualCompletionDate, "Completion Date", _projectModel?.ProjectName);
            if (_projectModel?.ExpiryDate != null)
            {
                AddProjectHistoryItem(items, _projectModel.ExpiryDate, "Expiry Date", _projectModel.Reference);
            }

            if (!items.Any())
            {
                items.Add(new ProjectHistoryTimelineItem
                {
                    EventDate = string.Empty,
                    Title = "No project history available",
                    Details = "Dates and addendums will appear here once they are provided.",
                    Marker = "·"
                });
                return items;
            }

            return items
                .OrderBy(x => x.SortDate ?? DateTime.MaxValue)
                .ThenBy(x => x.SortOrder)
                .ToList();
        }

        private static void AddProjectHistoryItem(ICollection<ProjectHistoryTimelineItem> items, DateTime? date, string title, string details, int sortOrder = 0)
        {
            if (date == null) return;

            items.Add(new ProjectHistoryTimelineItem
            {
                SortDate = date,
                SortOrder = sortOrder,
                EventDate = date.Value.ToString("dd MMM yyyy"),
                Title = title,
                Details = string.IsNullOrWhiteSpace(details) ? string.Empty : details,
                Marker = "●"
            });
        }

        private static string BuildAddendumHistoryDetails(AddendumModel addendum)
        {
            var details = new List<string>();

            if (!string.IsNullOrWhiteSpace(addendum.Subject))
            {
                details.Add(addendum.Subject.Trim());
            }

            if (!string.IsNullOrWhiteSpace(addendum.DecisionNo))
            {
                details.Add($"Decision No: {addendum.DecisionNo.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(addendum.Reference))
            {
                details.Add($"Ref: {addendum.Reference.Trim()}");
            }

            return string.Join(" | ", details.Where(x => !string.IsNullOrWhiteSpace(x)));
        }

        private static string FormatProjectHistoryTimelineItem(ProjectHistoryTimelineItem item)
        {
            if (item == null) return string.Empty;

            if (string.IsNullOrWhiteSpace(item.EventDate))
            {
                return item.Title + Environment.NewLine + item.Details;
            }

            return string.IsNullOrWhiteSpace(item.Details)
                ? $"{item.Marker} {item.EventDate} - {item.Title}"
                : $"{item.Marker} {item.EventDate} - {item.Title}{Environment.NewLine}  {item.Details}";
        }

        private void SyncIssuanceYearFromDate()
        {
            try
            {
                if (dtIssuanceDate.EditValue == null || dtIssuanceDate.EditValue == DBNull.Value)
                {
                    _projectModel.YearOfIssuance = null;
                    txtIssuanceYear.EditValue = null;
                    txtIssuanceYear.Text = string.Empty;
                    bsProject.ResetBindings(false);
                    return;
                }

                var issuanceDate = dtIssuanceDate.DateTime;
                if (issuanceDate == DateTime.MinValue)
                {
                    _projectModel.YearOfIssuance = null;
                    txtIssuanceYear.EditValue = null;
                    txtIssuanceYear.Text = string.Empty;
                    bsProject.ResetBindings(false);
                    return;
                }

                _projectModel.YearOfIssuance = issuanceDate.Year;
                txtIssuanceYear.Text = issuanceDate.Year.ToString();
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
                PersistDocumentLinks();

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
                _projectModel.ContractDetails = _projectModel.ContractDetails ?? new ContractDetailModel();
                var sponsorId = cboFundedBy.EditValue?.ToString();
                _projectModel.ContractDetails.SponsorId = string.IsNullOrWhiteSpace(sponsorId) ? null : sponsorId;

                // Save selected services and service types
                _projectModel.ContractDetails.ServicesProvided = GetCheckedItems(cboServicesProvided);

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

        private void cboProjectType_AddNewValue(object sender, AddNewValueEventArgs e)
        {
            var frm = new ProjectTypeEditForm(new ProjectTypeModel());
            frm.SendUpdatedProjectType += RcvUpdatedProjectType;
            frm.ShowDialog();
        }

        private void RcvUpdatedProjectType(object sender, EventArgs e)
        {
            if (sender == null) return;
            var projectType = sender as ProjectTypeModel;
            if (projectType == null) return;

            if (_projectTypes.All(x => !string.Equals(x.Type, projectType.Type, StringComparison.OrdinalIgnoreCase)))
            {
                _projectTypes.Add(projectType);
            }

            cboProjectType.Properties.DataSource = null;
            cboProjectType.Properties.DataSource = _projectTypes;
            cboProjectType.EditValue = projectType.Type;
        }

        private void RcvUpdatedArea(object sender, EventArgs e)
        {
            if (sender == null) return;
            _areaModel = sender as AreaModel;

            _areas.Add(_areaModel);

            cboAreas.Properties.DataSource = null;
            cboAreas.Properties.DataSource = _areas;
            if (_areaModel != null) cboAreas.EditValue = _areaModel.AreaName;
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

        private void cboClients_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                // Clear stale contact values before refreshing the source
                cboClientContact.EditValue = null;
                if (_projectModel != null)
                    _projectModel.ClientContact = null;

                txtContactEmail.EditValue = null;
                if (_projectModel?.ContractDetails != null)
                    _projectModel.ContractDetails.ClientContactEmail = null;

                RefreshContactEmailSource();
            }
            catch
            {
                // ignore
            }
        }

        private void RefreshContactEmailSource()
        {
            var clientId = cboClients.EditValue?.ToString();
            var contacts = string.IsNullOrWhiteSpace(clientId)
                ? new List<ContactModel>()
                : _contacts.Where(c => c.ClientId == clientId).ToList();

            cboClientContact.Properties.DataSource = null;
            cboClientContact.Properties.DataSource = contacts;

            txtContactEmail.Properties.DataSource = null;
            txtContactEmail.Properties.DataSource = contacts;

            var savedContact = _projectModel?.ClientContact;
            if (!string.IsNullOrWhiteSpace(savedContact) &&
                contacts.Any(c => string.Equals(c.ContactName, savedContact, StringComparison.OrdinalIgnoreCase)))
            {
                cboClientContact.EditValue = savedContact;
            }

            // Restore previously saved email if it matches one of the contacts
            var savedEmail = _projectModel?.ContractDetails?.ClientContactEmail;
            if (!string.IsNullOrWhiteSpace(savedEmail) &&
                contacts.Any(c => string.Equals(c.Email, savedEmail, StringComparison.OrdinalIgnoreCase)))
            {
                txtContactEmail.EditValue = savedEmail;
            }
        }

        private void cboContactEmail_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void cboClientContact_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                var clientId = cboClients.EditValue?.ToString();
                var contactName = cboClientContact.EditValue?.ToString();

                if (string.IsNullOrWhiteSpace(contactName))
                {
                    txtContactEmail.EditValue = null;
                    if (_projectModel?.ContractDetails != null)
                        _projectModel.ContractDetails.ClientContactEmail = null;
                    return;
                }

                var contact = _contacts.FirstOrDefault(c =>
                    (string.IsNullOrWhiteSpace(clientId) || c.ClientId == clientId) &&
                    string.Equals(c.ContactName, contactName, StringComparison.OrdinalIgnoreCase));

                var email = contact?.Email;
                txtContactEmail.EditValue = email;
                if (_projectModel?.ContractDetails != null)
                    _projectModel.ContractDetails.ClientContactEmail = email;
            }
            catch
            {
                // ignore
            }
        }

        private void cboContactEmail_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (!"ViewContact".Equals(e.Button.Tag?.ToString())) return;

            try
            {
                var email = txtContactEmail.EditValue?.ToString();
                var clientId = cboClients.EditValue?.ToString();
                var selectedClient = string.IsNullOrWhiteSpace(clientId)
                    ? null
                    : _clients.FirstOrDefault(c => c._id == clientId);

                // Find the contact matching the selected email
                ContactModel contact = null;
                if (!string.IsNullOrWhiteSpace(email))
                    contact = _contacts.FirstOrDefault(c => string.Equals(c.Email, email, StringComparison.OrdinalIgnoreCase));

                if (contact == null && selectedClient == null)
                {
                    XtraMessageBox.Show(
                        "Please select a client first.",
                        "No Client Selected",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }

                contact = contact ?? new ContactModel
                {
                    ClientId = selectedClient?._id,
                    ClientName = selectedClient?.ClientName
                };

                var frm = new ContactEditForm(contact, contact.ClientId ?? selectedClient?._id, contact.ClientName ?? selectedClient?.ClientName);
                frm.SendUpdatedContact += RcvUpdatedContactFromProject;
                frm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RcvUpdatedContactFromProject(object sender, EventArgs e)
        {
            if (!(sender is ContactModel updated)) return;

            // Refresh the in-memory contacts list
            var existing = _contacts.FirstOrDefault(c => c._id == updated._id);
            if (existing != null)
            {
                var idx = _contacts.IndexOf(existing);
                _contacts[idx] = updated;
            }
            else
            {
                _contacts.Add(updated);
            }

            // Refresh the email dropdown for the current client
            RefreshContactEmailSource();

            // If the updated contact matches the currently selected email, keep it selected
            if (!string.IsNullOrWhiteSpace(updated.Email))
                txtContactEmail.EditValue = updated.Email;
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

        private void gvDocuments_ContextButtonCustomize(object sender, WinExplorerViewContextButtonCustomizeEventArgs e)
        {
            if (gvDocuments.FocusedRowHandle == e.RowHandle)
            {
                e.Item.AppearanceNormal.ForeColor = Color.White;
                e.Item.AppearanceHover.ForeColor = Color.White;
            }
        }

        private void gvDocuments_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            gvDocuments.RefreshContextButtons();
        }

        private void openDocuments_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            try
            {
                var projectId = _projectModel?._id;
                if (string.IsNullOrWhiteSpace(projectId))
                {
                    XtraMessageBox.Show("Please save the project record first to generate an ID for archived attachments.", "Save Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var ofd = new OpenFileDialog
                {
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    Multiselect = true,
                    Filter = @"All Files (*.*)|*.*" +
                             @"|PDF Portable Document Format (*.pdf)|*.pdf" +
                             @"|PNG Portable Network Graphics (*.png)|*.png" +
                             @"|JPEG File Interchange Format (*.jpg *.jpeg *jfif)|*.jpg;*.jpeg;*.jfif" +
                             @"|BMP Windows Bitmap (*.bmp)|*.bmp" +
                             @"|TIF Tagged Imaged File Format (*.tif *.tiff)|*.tif;*.tiff" +
                             @"|GIF Graphics Interchange Format (*.gif)|*.gif"
                };

                if (ofd.ShowDialog() != DialogResult.OK) return;

                var connection = DatabaseFactory.GetConnection(DatabaseFactory.ProfilePrimary);
                var rootFolder = string.IsNullOrWhiteSpace(connection?.ProjectsDocumentsFolder)
                    ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SpectrumApp", "Projects")
                    : connection.ProjectsDocumentsFolder;
                Directory.CreateDirectory(rootFolder);

                foreach (var file in ofd.FileNames)
                {
                    try
                    {
                        string fileName = Path.GetFileName(file);
                        string archivedFileName = projectId + "_" + fileName;
                        string destinationPath = Path.Combine(rootFolder, archivedFileName);

                        if (File.Exists(destinationPath))
                        {
                            var result = XtraMessageBox.Show($"File '{archivedFileName}' already exists. Overwrite?", "Confirm Overwrite", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                            if (result == DialogResult.Cancel)
                            {
                                break;
                            }
                            if (result == DialogResult.No)
                            {
                                continue;
                            }
                        }

                        File.Copy(file, destinationPath, true);

                        _documents.Add(new DocumentModel
                        {
                            DocumentName = fileName,
                            OriginPath = destinationPath,
                            DocumentDate = File.GetCreationTime(destinationPath),
                            StreamedDate = DateTime.Now,
                            DocumentContent = File.ReadAllBytes(destinationPath)
                        });
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, @"Copy Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                PersistDocumentLinks();
                gcDocuments.RefreshDataSource();
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gvDocuments_DoubleClick(object sender, EventArgs e)
        {
            OpenSelectedDocument();
        }

        private void gvDocuments_ContextButtonClick(object sender, DevExpress.Utils.ContextItemClickEventArgs e)
        {
            try
            {
                var focusedRowHandle = gvDocuments.FocusedRowHandle;
                if (focusedRowHandle < 0)
                {
                    return;
                }

                var document = gvDocuments.GetRow(focusedRowHandle) as DocumentModel;
                if (document == null || string.IsNullOrWhiteSpace(document.OriginPath))
                {
                    return;
                }

                var result = XtraMessageBox.Show(
                    $"Are you sure you want to delete '{document.DocumentName}'?\n\nThis will permanently delete the file from disk.",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);

                if (result != DialogResult.Yes)
                {
                    return;
                }

                // Delete the physical file
                if (File.Exists(document.OriginPath))
                {
                    File.Delete(document.OriginPath);
                }

                // Remove from the binding list
                _documents.RemoveAt(focusedRowHandle);

                XtraMessageBox.Show("Document deleted successfully.", "Delete Document", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (UnauthorizedAccessException ex)
            {
                XtraMessageBox.Show($"Access denied: {ex.Message}\n\nPlease check file permissions.", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException ex)
            {
                XtraMessageBox.Show($"File error: {ex.Message}\n\nThe file may be in use by another application.", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Error deleting document: {ex.Message}", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenSelectedDocument()
        {
            try
            {
                var document = gvDocuments.GetFocusedRow() as DocumentModel;
                if (document == null || string.IsNullOrWhiteSpace(document.OriginPath) || !File.Exists(document.OriginPath))
                {
                    return;
                }

                Process.Start(document.OriginPath);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Open Document", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void gvAddendum_DoubleClick(object sender, EventArgs e)
        {
            if (!CanManageAddendums()) return;

            var addendumId = gvAddendum.GetFocusedRowCellValue("_id")?.ToString();

            if (string.IsNullOrWhiteSpace(addendumId))
            {
                _addendumModel = new AddendumModel();
            }
            else
            {
                _addendumModel = _addendums.FirstOrDefault(x => x._id == addendumId) ?? new AddendumModel();
            }

            AddendumEditForm form = new AddendumEditForm(_addendumModel, _addendums);
            form.SendUpdatedAddendum += RcvUpdatedAddendum;
            form.ShowDialog();
        }

        private void RcvUpdatedAddendum(object sender, EventArgs e)
        {
            if (sender == null) return;

            _addendumModel = sender as AddendumModel;
            if (_addendumModel == null) return;

            var existingAddendum = _addendums.FirstOrDefault(x => x._id == _addendumModel._id);
            if (existingAddendum == null)
            {
                _addendums.Add(_addendumModel);
            }
            else
            {
                var index = _addendums.IndexOf(existingAddendum);
                if (index >= 0)
                {
                    _addendums[index] = _addendumModel;
                }
            }

            _projectModel.Addendums = _addendums.ToList();
            gcAddendum.DataSource = null;
            gcAddendum.DataSource = _addendums;
            gvAddendum.RefreshData();
            RefreshProjectHistoryTimeline();
        }

        private class ProjectHistoryTimelineItem
        {
            public DateTime? SortDate { get; set; }
            public int SortOrder { get; set; }
            public string EventDate { get; set; }
            public string Title { get; set; }
            public string Details { get; set; }
            public string Marker { get; set; }
        }

        private void btnProtected_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            _projectModel.IsProtected = btnProtected.Checked;

            var fontStyle = btnProtected.Checked
                ? FontStyle.Bold
                : FontStyle.Regular;

            var foreColor = btnProtected.Checked
                ? Color.Red
                : Color.Black;

            var font = new Font("Tahoma", 8, fontStyle);

            var appearances = new[]
            {
                btnProtected.ItemAppearance.Normal,
                btnProtected.ItemAppearance.Hovered,
                btnProtected.ItemAppearance.Pressed
            };

            foreach (var appearance in appearances)
            {
                appearance.Font = font;
                appearance.ForeColor = foreColor;
            }
        }


    }
}
