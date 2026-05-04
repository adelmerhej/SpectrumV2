using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using Spectrum.DataLayers.Common.Countries;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Administration.Connections;
using Spectrum.Models.Common.Countries;
using Spectrum.Utilities;
using SpectrumV1.DataLayers.HumanResources.Candidates;
using SpectrumV1.DataLayers.HumanResources.Employees.Services;
using SpectrumV1.Models.HumanResources.Candidates;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.HumanResources.HRCVs
{
    public partial class HrCvEditForm : RibbonForm
    {
        private CandidateModel _candidateModel = new CandidateModel();
        private IList<CountryModel> _countries = new List<CountryModel>();
        private IList<CityModel> _cities = new List<CityModel>();

        private CandidateRepository _candidateRepository;

        private CountryRepository _countryRepository;
        private CityRepository _cityRepository;

        private LogInfoRepository _logInfoRepository;
        private ConnectionModel _connectionModel = new ConnectionModel();

        //Init permissionvariables
        private bool _canAdd = true;
        private bool _canEdit = true;
        private bool _canDelete = true;
        private bool _canPrint = true;
        private bool _isAdmin = true;
        private bool _isProtected = true;

        public EventHandler SendUpdatedCandidate;

        /// <summary>
        /// Required for WinForms designer support. Do not use directly.
        /// </summary>
        public HrCvEditForm()
        {
            InitializeComponent();
        }

        public HrCvEditForm(CandidateModel model)
        {
            InitializeComponent();

            _candidateModel = model ?? new CandidateModel();

            if (!DesignMode && LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                InitializeRepositories();
                StartLoading();
            }
        }

        private void InitializeRepositories()
        {
            _candidateRepository = new CandidateRepository(DatabaseFactory.ProfilePrimary);
            _countryRepository = new CountryRepository(DatabaseFactory.ProfilePrimary);
            _cityRepository = new CityRepository(DatabaseFactory.ProfilePrimary);
            _logInfoRepository = new LogInfoRepository();
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
                    LoadTitlesAsync(),
                    LoadApiKeys(),
                };

                await Task.WhenAll(loadTasks);
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading form data", ex);
            }
        }

        private async Task LoadApiKeys()
        {
            _connectionModel = DatabaseFactory.GetConnection(DatabaseFactory.ProfilePrimary);
            await Task.CompletedTask;
        }

        private async Task LoadCountriesAsync()
        {
            _countries = await _countryRepository.GetCountriesAsync();
        }

        private async Task LoadCitiesAsync()
        {
            _cities = await _cityRepository.GetCitiesAsync();
        }

        private async Task LoadTitlesAsync()
        {
            HelperApplication.InitGenderComboBox(cboGenders.Properties);
            await Task.CompletedTask;
        }
        private async Task LoadStatusAsync()
        {

        }

        private async Task LoadBloodTypeAsync()
        {

        }

        private void WireUpBindings()
        {
            RefreshCandidateBindings();

            cboNationality.Properties.DataSource = null;
            cboNationality.Properties.DataSource = _countries;

            cboCities.Properties.DataSource = null;
            cboCities.Properties.DataSource = _cities;
        }

        private void RefreshCandidateBindings()
        {
            bsCandidate.DataSource = _candidateModel;
            bsCandidate.ResetBindings(false);
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
            _candidateModel = new CandidateModel();
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

            ValidateFirstName(validationErrors);
            ValidateLastName(validationErrors);

            if (validationErrors.Any())
            {
                ShowValidationErrors(validationErrors);
                return false;
            }

            return true;
        }

        private void ValidateFirstName(List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                errors.Add("First Name cannot be empty.");
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
                BindingContext[bsCandidate].EndCurrentEdit();

                _candidateModel = bsCandidate.Current as CandidateModel;
                if (_candidateModel == null)
                {
                    throw new InvalidOperationException("Candidate data is not available for saving.");
                }

                bool isNewCandidate = string.IsNullOrEmpty(_candidateModel._id);

                if (isNewCandidate)
                {
                    _logInfoRepository.CreateLogInfo(_candidateModel);
                    await _candidateRepository.AddNewCandidateAsync(_candidateModel);
                }
                else
                {
                    _logInfoRepository.UpdateLogInfo(_candidateModel);
                    await _candidateRepository.UpdateCandidateAsync(_candidateModel);
                }

                SendUpdatedCandidate(_candidateModel, EventArgs.Empty);
                return true;
            }
            catch (Exception ex)
            {
                ShowError("Error saving employee data", ex);
                return false;
            }
        }

        private void HrCvEditForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void txtUploadCv_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select a CV Document";
                openFileDialog.Filter = "PDF Files (*.pdf)|*.pdf|Word Documents (*.docx;*.doc)|*.docx;*.doc|All Supported Files (*.pdf;*.docx;*.doc)|*.pdf;*.docx;*.doc";
                openFileDialog.FilterIndex = 3;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtUploadCv.Text = openFileDialog.FileName;
                }
            }
        }

        private async void btnParseWithAI_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtUploadCv.Text) || !System.IO.File.Exists(txtUploadCv.Text))
                {
                    XtraMessageBox.Show("Please select a valid CV file to parse.", "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                // Simulate AI parsing logic here
                var apiKey = _connectionModel.EncryptedAiApikey;
                var model = _connectionModel.AiModel ?? "gpt-4.1-mini";

                if (string.IsNullOrWhiteSpace(apiKey))
                {
                    XtraMessageBox.Show("Your API key is not configured. Please set 'AI:ApiKey' in the config settings.", "Configuration Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (var service = new CvParsingService(apiKey, model))
                {
                    var candidate = await service.ParseCvAsync(txtUploadCv.Text);

                    _candidateModel.FirstName = candidate.FirstName;
                    _candidateModel.LastName = candidate.LastName;
                    _candidateModel.DateOfBirth = candidate.DateOfBirth;
                    _candidateModel.Gender = candidate.Gender;
                    _candidateModel.Nationality = candidate.Nationality;
                    _candidateModel.Email = candidate.Email;
                    _candidateModel.Phone = candidate.Phone;
                    _candidateModel.City = candidate.City;
                    _candidateModel.Address = candidate.Address;
                    _candidateModel.Position = candidate.Position;
                    _candidateModel.YearsOfExperience = candidate.YearsOfExperience;
                    _candidateModel.Skills = candidate.Skills;
                    _candidateModel.Summary = candidate.Summary;
                }

                RefreshCandidateBindings();

            }
            catch (Exception ex)
            {
                ShowError("Parsing Error", ex);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            txtFirstName.Text = _candidateModel.FirstName;
        }
    }
}