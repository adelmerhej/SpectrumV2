using DevExpress.Utils.Menu;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using Spectrum.DataLayers.Common.Countries;
using Spectrum.DataLayers.DataAccess;
using Spectrum.DataLayers.Members.Engineers.Status;
using Spectrum.Models.Common.Countries;
using Spectrum.Models.HumanResources.Employees;
using Spectrum.Models.Members.Engineers;
using Spectrum.Models.Members.Engineers.Status;
using Spectrum.Utilities;
using SpectrumV1.DataLayers.EmployeeTypes;
using SpectrumV1.DataLayers.HumanResources.BloodTypes;
using SpectrumV1.DataLayers.HumanResources.Employees;
using SpectrumV1.Models.HumanResources.BloodTypes;
using SpectrumV1.Models.HumanResources.EmployeeTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Spectrum.Views.HumanResources.Employees
{
    public partial class EmployeeEditForm : RibbonForm
    {
        private EmployeeModel _employeeModel = new EmployeeModel();

        private IList<CountryModel> _countries = new List<CountryModel>();
        private IList<CityModel> _cities = new List<CityModel>();
        private IList<EmployeeTypeModel> _employeeTypes = new List<EmployeeTypeModel>();
        private IList<BloodTypeModel> _bloodTypes = new List<BloodTypeModel>();

        private readonly EmployeeRepository _employeeRepository = new EmployeeRepository(DatabaseFactory.ProfilePrimary);
        private readonly CountryRepository _countryRepository = new CountryRepository(DatabaseFactory.ProfilePrimary);
        private readonly CityRepository _cityRepository = new CityRepository(DatabaseFactory.ProfilePrimary);
        private readonly EmployeeTypeRepository _employeeTypeRepository = new EmployeeTypeRepository(DatabaseFactory.ProfilePrimary);
        private readonly BloodTypeRepository _bloodTypeRepository = new BloodTypeRepository(DatabaseFactory.ProfilePrimary);
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
                ApplyDefaults();
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
            _employeeTypes = await _employeeTypeRepository.GetEmployeeTypesAsync();
        }

        private async Task LoadBloodTypeAsync()
        {
            _bloodTypes = await _bloodTypeRepository.GetBloodTypesAsync();
        }


        //_bloodTypeRepository
        private void WireUpBindings()
        {
            EnsureSubObjectsInitialized();

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

        private void ApplyDefaults()
        {

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

        private void btnSave_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (ValidateData())
            {
                SaveData();
            }
        }

        private void btnSaveAndClose_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (ValidateData())
            {
                SaveData();
                Close();
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

        private async void SaveData()
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

                _employeeModel = (EmployeeModel)bsEmployee.Current;

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
                SendUpdatedEmployee?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                ShowError("Error saving employee data", ex);
            }
        }

        private void txtDocumentLink_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select a Document";
                openFileDialog.Filter = "All Files (*.*)|*.*|PDF Files (*.pdf)|*.pdf|Word Documents (*.docx)|*.docx|Images (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
                openFileDialog.FilterIndex = 1;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtDocumentLink.Text = openFileDialog.FileName;
                }
            }
        }
    }
}