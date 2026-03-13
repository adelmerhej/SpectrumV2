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
using SpectrumV1.DataLayers.EmployeeTypes;
using SpectrumV1.DataLayers.HumanResources.BloodTypes;
using SpectrumV1.DataLayers.HumanResources.Employees;
using SpectrumV1.DataLayers.HumanResources.Employees.EmployeeStatus;
using SpectrumV1.Models.Common.Documents;
using SpectrumV1.Models.HumanResources.BloodTypes;
using SpectrumV1.Models.HumanResources.Employees.EmployeeStatus;
using SpectrumV1.Models.HumanResources.EmployeeTypes;
using System;
using System.Collections.Generic;
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
                var matchedType = _employeeTypes.FirstOrDefault(x => x.TypeName == _employeeModel.EmployeeType);
                _employeeModel.EmployeeType = matchedType?.TypeName ?? _employeeModel.EmployeeType;
            }

            bsEmployee.DataSource = _employeeModel;
            bsWorkingpermit.DataSource = _employeeModel.WorkingPermit;
            bsSyndicat.DataSource = _employeeModel.Syndicat;
            bsEmergencyContact.DataSource = _employeeModel.EmergencyContact;
            bsEmployeeContactInfo.DataSource = _employeeModel.ContactInfo;
            bsCnss.DataSource = _employeeModel.Cnss;
            bsWorkExperience.DataSource = _employeeModel.WorkExperience;

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

        private void gvDocuments_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            gvDocuments.RefreshContextButtons();
        }

        private void openDocuments_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            try
            {
                // Select one or more files to attach
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

                // Prepare destination folder: Documents\SpectrumApp\Employees\<fullname>
                string firstName = !string.IsNullOrWhiteSpace(txtFirstName.Text)
                    ? txtFirstName.Text
                    : _employeeModel.FirstName;
                string lastName = !string.IsNullOrWhiteSpace(txtLastName.Text)
                    ? txtLastName.Text
                    : _employeeModel.LastName;

                string Sanitize(string value)
                {
                    if (string.IsNullOrWhiteSpace(value)) return "Unknown";
                    var invalid = Path.GetInvalidFileNameChars();
                    foreach (var c in invalid)
                    {
                        value = value.Replace(c.ToString(), "_");
                    }
                    return value.Trim();
                }
                string fullName = $"{firstName}_{lastName}";

                fullName = Sanitize(fullName);

                string baseFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SpectrumApp", "Employees", fullName);
                Directory.CreateDirectory(baseFolder);

                foreach (var file in ofd.FileNames)
                {
                    try
                    {
                        string fileName = Path.GetFileName(file);
                        string destinationPath = Path.Combine(baseFolder, fileName);

                        if (File.Exists(destinationPath))
                        {
                            var result = XtraMessageBox.Show($"File '{fileName}' already exists. Overwrite?", "Confirm Overwrite", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                            if (result == DialogResult.Cancel)
                            {
                                break;
                            }
                            if (result == DialogResult.No)
                            {
                                continue;
                            }
                            // Yes -> overwrite below
                        }

                        File.Copy(file, destinationPath, true);

                        // Create document model and add to binding source
                        var newDocument = new DocumentModel
                        {
                            DocumentName = Path.GetFileName(destinationPath),
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