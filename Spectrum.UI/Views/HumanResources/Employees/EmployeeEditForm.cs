using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.WinExplorer;
using Spectrum.DataLayers.Common.Countries;
using Spectrum.DataLayers.DataAccess;
using Spectrum.DataLayers.HumanResources.JobPositions;
using Spectrum.Models.Common.Countries;
using Spectrum.Models.HumanResources.Employees;
using Spectrum.Models.HumanResources.JobPositions;
using Spectrum.Utilities;
using Spectrum.Utilities.Enums;
using Spectrum.Views.Common.Countries;
using Spectrum.Views.HumanResources.Common.BloodTypes;
using Spectrum.Views.HumanResources.Employees.Status;
using Spectrum.DataLayers.EmployeeTypes;
using Spectrum.DataLayers.HumanResources.BloodTypes;
using Spectrum.DataLayers.HumanResources.Employees;
using Spectrum.DataLayers.HumanResources.Employees.EmployeeStatus;
using Spectrum.Models.Common.Documents;
using Spectrum.Models.HumanResources.BloodTypes;
using Spectrum.Models.HumanResources.Employees.EmployeeStatus;
using Spectrum.Models.HumanResources.EmployeeTypes;
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

namespace Spectrum.Views.HumanResources.Employees
{
    public partial class EmployeeEditForm : RibbonForm
    {
        private EmployeeModel _employeeModel = new EmployeeModel();

        private IList<CountryModel> _countries = new List<CountryModel>();
        private CountryModel _countryModel = new CountryModel();

        private IList<CityModel> _cities = new List<CityModel>();
        private CityModel _cityModel = new CityModel();

        private IList<EmployeeTypeModel> _employeeTypes = new List<EmployeeTypeModel>();
        private EmployeeTypeModel _employeeTypeModel = new EmployeeTypeModel();

        private IList<BloodTypeModel> _bloodTypes = new List<BloodTypeModel>();
        private BloodTypeModel _bloodTypeModel = new BloodTypeModel();

        private IList<StatusModel> _status = new List<StatusModel>();
        private StatusModel _statusModel = new StatusModel();

        private IList<JobPositionModel> _jobPositions = new List<JobPositionModel>();
        private JobPositionModel _jobPositionModel = new JobPositionModel();
        private BindingList<DocumentModel> _documents = new BindingList<DocumentModel>();

        private readonly EmployeeRepository _employeeRepository = new EmployeeRepository(DatabaseFactory.ProfilePrimary);
        private readonly CountryRepository _countryRepository = new CountryRepository(DatabaseFactory.ProfilePrimary);
        private readonly CityRepository _cityRepository = new CityRepository(DatabaseFactory.ProfilePrimary);
        private readonly EmployeeTypeRepository _employeeTypeRepository = new EmployeeTypeRepository(DatabaseFactory.ProfilePrimary);
        private readonly BloodTypeRepository _bloodTypeRepository = new BloodTypeRepository(DatabaseFactory.ProfilePrimary);
        private readonly StatusRepository _statusRepository = new StatusRepository(DatabaseFactory.ProfilePrimary);
        private readonly JobPositionRepository _jobPositionRepository = new JobPositionRepository(DatabaseFactory.ProfilePrimary);
        private readonly LogInfoRepository _logInfoRepository = new LogInfoRepository();

        //Init permissionvariables
        private bool _canAdd = true;
        private bool _canEdit = true;
        private bool _canDelete = true;
        private bool _canPrint = true;
        private bool _isAdmin = true;
        private bool _isProtected = true;

        public EventHandler SendUpdatedEmployee;

        public EmployeeEditForm(EmployeeModel model)
        {
            InitializeComponent();

            _employeeModel = model ?? new EmployeeModel();

            StartLoading();
        }

        #region Initialization

        private async void StartLoading()
        {
            try
            {
                await InitializeBindings();
                WireUpBindings();
                await ApplyDefaultsAsync();
                ApplyPermissions();
            }
            catch (Exception ex)
            {
                ShowError("Error during form initialization", ex);
            }
        }

        private async Task InitializeBindings()
        {
            try
            {
                var loadTasks = new[]
                {
                    LoadCountriesAsync(),
                    LoadCitiesAsync(),
                    LoadStatusAsync(),
                    LoadBloodTypeAsync(),
                    LoadEmployeeTypesAsync(),
                    LoadJobPositionsAsync(),
                };

                await Task.WhenAll(loadTasks);
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading form data", ex);
            }
        }

        private async Task LoadCountriesAsync()
        {
            _countries = await _countryRepository.GetCountriesAsync();
        }

        private async Task LoadCitiesAsync()
        {
            _cities = await _cityRepository.GetCitiesAsync();
        }

        private async Task LoadStatusAsync()
        {
            _status = await _statusRepository.GetStatusAsync();
        }

        private async Task LoadBloodTypeAsync()
        {
            _bloodTypes = await _bloodTypeRepository.GetBloodTypesAsync();
        }

        private async Task LoadEmployeeTypesAsync()
        {
            _employeeTypes = await _employeeTypeRepository.GetEmployeeTypesAsync();
        }

        private async Task LoadJobPositionsAsync()
        {
            _jobPositions = await _jobPositionRepository.GetJobPositionsAsync();
        }


        //_bloodTypeRepository
        private void WireUpBindings()
        {
            EnsureSubObjectsInitialized();

            if (_employeeModel.EmployeeType != null)
            {
                _employeeModel.EmployeeType = EmployeeTypeRepository.ResolveEmployeeTypeModel(_employeeTypes, _employeeModel.EmployeeType);
            }

            bsEmployee.DataSource = _employeeModel;
            bsWorkingpermit.DataSource = _employeeModel.WorkingPermit;
            bsSyndicat.DataSource = _employeeModel.Syndicat;
            bsEmergencyContact.DataSource = _employeeModel.EmergencyContact;
            bsEmployeeContactInfo.DataSource = _employeeModel.ContactInfo;
            bsCnss.DataSource = _employeeModel.Cnss;
            bsWorkExperience.DataSource = _employeeModel.WorkExperience;
            InitializeDocumentBindings();

            cboNationality.Properties.DataSource = _countries;
            cboPlaceOfBirth.Properties.DataSource = _cities;
            cboRegistrationPlace.Properties.DataSource = _cities;
            cboEmployeeType.Properties.DataSource = _employeeTypes;
            cboBloodType.Properties.DataSource = _bloodTypes;
            cboFamilyStatus.Properties.DataSource = _status;
        }

        private void EnsureSubObjectsInitialized()
        {
            if (_employeeModel.WorkingPermit == null)
                _employeeModel.WorkingPermit = new WorkingPermitInfo();
            if (_employeeModel.Syndicat == null)
                _employeeModel.Syndicat = new SyndicatInfo();
            if (_employeeModel.EmergencyContact == null)
                _employeeModel.EmergencyContact = new EmergencyContactInfo();
            if (_employeeModel.ContactInfo == null)
                _employeeModel.ContactInfo = new EmployeeContactInfo();
            if (_employeeModel.Cnss == null)
                _employeeModel.Cnss = new CnssInfo();
            if (_employeeModel.WorkExperience == null)
                _employeeModel.WorkExperience = new WorkExperienceInfo();
            if (_employeeModel.Financial == null)
                _employeeModel.Financial = new FinancialInfo();
        }

        private async Task ApplyDefaultsAsync()
        {
            if (string.IsNullOrEmpty(_employeeModel._id))
            {
                int latestNo = await _employeeRepository.GetLatestEmployeeNoAsync();
                _employeeModel.EmployeeNo = latestNo + 1;
                bsEmployee.ResetBindings(false);
            }

            gvDocuments.GetThumbnailImage += TileView1_GetThumbnailImage;
            gvDocuments.OptionsImageLoad.RandomShow = true;
            gvDocuments.OptionsImageLoad.LoadThumbnailImagesFromDataSource = false;
            gvDocuments.OptionsImageLoad.AsyncLoad = true;

            var employeeType = EmployeeTypeRepository.ResolveEmployeeTypeModel(_employeeTypes, employeeType: EmployeeType.Employee);
            if (employeeType != null)
            {
                _employeeModel.EmployeeType = employeeType;
                bsEmployee.ResetBindings(false);
            }
        }

        private void TileView1_GetThumbnailImage(object sender, ThumbnailImageEventArgs e)
        {
            var fileName = (string)gvDocuments.GetRowCellValue(e.DataSourceIndex, colName);
            var ext = Path.GetExtension(fileName);
            e.ThumbnailImage = HelperApplication.GetFileExtensionImage(ext, IconSizeType.Large, new Size(64, 64));
        }

        private void InitializeDocumentBindings()
        {
            _documents = LoadDocumentsForEmployee();
            bsDocuments.DataSource = _documents;
        }

        private BindingList<DocumentModel> LoadDocumentsForEmployee()
        {
            var documents = new BindingList<DocumentModel>();
            var employeeId = _employeeModel?._id;
            if (string.IsNullOrWhiteSpace(employeeId))
                return documents;

            var connection = DatabaseFactory.GetConnection(DatabaseFactory.ProfilePrimary);
            var rootFolder = string.IsNullOrWhiteSpace(connection?.EmployeesDocumentsFolder)
                ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SpectrumApp", "Employees")
                : connection.EmployeesDocumentsFolder;

            if (!Directory.Exists(rootFolder))
                return documents;

            var prefix = employeeId + "_";
            var files = Directory.GetFiles(rootFolder)
                .Where(path => Path.GetFileName(path).StartsWith(prefix, StringComparison.OrdinalIgnoreCase));

            foreach (var filePath in files)
            {
                try
                {
                    documents.Add(new DocumentModel
                    {
                        DocumentName = GetDisplayDocumentName(employeeId, filePath),
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

        private static BindingList<DocumentModel> LoadDocumentsFromLink(string documentLink)
        {
            var documents = new BindingList<DocumentModel>();

            if (string.IsNullOrWhiteSpace(documentLink)) return documents;

            var filePaths = documentLink.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
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
            if (_employeeModel == null) return;

            _employeeModel.DocumentLink = string.Join(";",
                _documents.Where(x => x != null && !string.IsNullOrWhiteSpace(x.OriginPath))
                    .Select(x => x.OriginPath)
                    .Distinct(StringComparer.OrdinalIgnoreCase));
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


        #region button events

        private void btnNew_ItemClick(object sender, ItemClickEventArgs e)
        {
            _employeeModel = new EmployeeModel();
            StartLoading();
        }

        private async void btnSave_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (ValidateData())
            {
                await SaveDataAsync();
            }
        }

        private async void btnSaveAndClose_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (ValidateData())
            {
                var saved = await SaveDataAsync();
                if (saved)
                {
                    Close();
                }
            }
        }

        private void btnRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {
            StartLoading();
        }

        private void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void btnPrint_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void btnMeeting_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void btnClose_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }

        #endregion

        private void ShowError(string message, Exception ex)
        {
            XtraMessageBox.Show(
                $"{message}\n\nDetails: {ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }


        private bool ValidateData()
        {
            var validationErrors = new List<string>();

            ValidateEmployeeNo(validationErrors);
            ValidateFirstName(validationErrors);
            ValidateLastName(validationErrors);

            if (validationErrors.Any())
            {
                ShowValidationErrors(validationErrors);
                return false;
            }

            return true;
        }

        private void ValidateEmployeeNo(List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(txtEmployeeNo.Text))
            {
                errors.Add("Employee No cannot be empty.");
                txtEmployeeNo.Focus();
            }
        }

        private void ValidateFirstName(List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                errors.Add("Employee Name cannot be empty.");
                txtFirstName.Focus();
            }
        }

        private void ValidateLastName(List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                errors.Add("Last Name cannot be empty.");
                txtLastName.Focus();
            }
        }

        private void ShowValidationErrors(List<string> errors)
        {
            var messageBuilder = new StringBuilder();
            messageBuilder.AppendLine(errors.Count > 1
                ? "The followings need your attention:"
                : "The following need your attention:");

            foreach (var error in errors)
            {
                messageBuilder.AppendLine($"- {error}");
            }

            messageBuilder.AppendLine("\nPlease try again.");

            XtraMessageBox.Show(messageBuilder.ToString(),
                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private async Task<bool> SaveDataAsync()
        {
            try
            {
                BindingContext[bsEmployee].EndCurrentEdit();
                BindingContext[bsWorkingpermit].EndCurrentEdit();
                BindingContext[bsSyndicat].EndCurrentEdit();
                BindingContext[bsEmergencyContact].EndCurrentEdit();
                BindingContext[bsEmployeeContactInfo].EndCurrentEdit();
                BindingContext[bsCnss].EndCurrentEdit();
                BindingContext[bsWorkExperience].EndCurrentEdit();

                _employeeModel = bsEmployee.Current as EmployeeModel;
                if (_employeeModel == null)
                {
                    throw new InvalidOperationException("Employee data is not available for saving.");
                }

                PersistDocumentLinks();
                bool isNewEmployee = string.IsNullOrEmpty(_employeeModel._id);

                if (isNewEmployee)
                {
                    _logInfoRepository.CreateLogInfo(_employeeModel);
                    await _employeeRepository.AddNewEmployeeAsync(_employeeModel);
                }
                else
                {
                    _logInfoRepository.UpdateLogInfo(_employeeModel);
                    await _employeeRepository.UpdateEmployeeAsync(_employeeModel);
                }

                SendUpdatedEmployee(_employeeModel, EventArgs.Empty);
                return true;
            }
            catch (Exception ex)
            {
                ShowError("Error saving employee data", ex);
                return false;
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

        private void gvDocuments_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            gvDocuments.RefreshContextButtons();
        }

        private void gvDocuments_DoubleClick(object sender, EventArgs e)
        {
            OpenSelectedDocument();
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

        private void openDocuments_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            try
            {
                var employeeId = _employeeModel?._id;
                if (string.IsNullOrWhiteSpace(employeeId))
                {
                    XtraMessageBox.Show("Please save the employee record first to generate an ID for archived attachments.", "Save Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                var rootFolder = string.IsNullOrWhiteSpace(connection?.EmployeesDocumentsFolder)
                    ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SpectrumApp", "Employees")
                    : connection.EmployeesDocumentsFolder;
                Directory.CreateDirectory(rootFolder);

                foreach (var file in ofd.FileNames)
                {
                    try
                    {
                        string fileName = Path.GetFileName(file);
                        string archivedFileName = employeeId + "_" + fileName;
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

                        var newDocument = new DocumentModel
                        {
                            DocumentName = fileName,
                            OriginPath = destinationPath,
                            DocumentDate = File.GetCreationTime(destinationPath),
                            StreamedDate = DateTime.Now,
                            DocumentContent = File.ReadAllBytes(destinationPath)
                        };

                        bsDocuments.Add(newDocument);
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, @"Copy Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                PersistDocumentLinks();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Context Menu Events


        // Bloodtype
        private void cboBloodType_AddNewValue(object sender, AddNewValueEventArgs e)
        {
            BloodTypeEditForm frm = new BloodTypeEditForm(new BloodTypeModel());
            frm.SendUpdatedBloodType += RcvUpdatedBloodTypeAsync;
            frm.ShowDialog();
        }

        private void RcvUpdatedBloodTypeAsync(object sender, EventArgs e)
        {
            if (sender == null) return;
            _bloodTypeModel = sender as BloodTypeModel;

            _bloodTypes.Add(_bloodTypeModel);

            cboBloodType.Properties.DataSource = null;
            cboBloodType.Properties.DataSource = _bloodTypes;
            if (_bloodTypeModel != null) cboBloodType.EditValue = _bloodTypeModel._id;
        }

        // PlaceOfBirth
        private void cboPlaceOfBirth_AddNewValue(object sender, AddNewValueEventArgs e)
        {
            CityEditForm frm = new CityEditForm(new CityModel());
            frm.SendUpdatedCity += RcvUpdatedCityAsync;
            frm.ShowDialog();
        }

        private void RcvUpdatedCityAsync(object sender, EventArgs e)
        {
            if (sender == null) return;
            _cityModel = sender as CityModel;

            _cities.Add(_cityModel);

            cboPlaceOfBirth.Properties.DataSource = null;
            cboPlaceOfBirth.Properties.DataSource = _cities;
            if (_cityModel != null) cboPlaceOfBirth.EditValue = _cityModel._id;
        }


        // Nationality
        private void cboNationality_AddNewValue(object sender, AddNewValueEventArgs e)
        {
            CountryEditForm frm = new CountryEditForm(new CountryModel());
            frm.SendUpdatedCountry += RcvUpdatedCountryAsync;
            frm.ShowDialog();
        }

        private void RcvUpdatedCountryAsync(object sender, EventArgs e)
        {
            if (sender == null) return;
            _countryModel = sender as CountryModel;

            _countries.Add(_countryModel);

            cboNationality.Properties.DataSource = null;
            cboNationality.Properties.DataSource = _countries;
            if (_countryModel != null) cboNationality.EditValue = _countryModel._id;
        }


        // FamilyStatus
        private void cboFamilyStatus_AddNewValue(object sender, AddNewValueEventArgs e)
        {
            StatusEditForm frm = new StatusEditForm(new StatusModel());
            frm.SendUpdatedStatus += RcvUpdatedFamilyStatusAsync;
            frm.ShowDialog();
        }

        private void RcvUpdatedFamilyStatusAsync(object sender, EventArgs e)
        {
            if (sender == null) return;
            _statusModel = sender as StatusModel;

            _status.Add(_statusModel);

            cboFamilyStatus.Properties.DataSource = null;
            cboFamilyStatus.Properties.DataSource = _status;
            if (_statusModel != null) cboFamilyStatus.EditValue = _statusModel._id;
        }


        #endregion


    }
}