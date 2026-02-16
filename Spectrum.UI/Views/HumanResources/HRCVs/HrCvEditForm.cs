using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using Spectrum.DataLayers.Common.Countries;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Common.Countries;
using Spectrum.Utilities;
using SpectrumV1.DataLayers.HumanResources.Candidates;
using SpectrumV1.Models.HumanResources.Candidates;
using System;
using System.Collections.Generic;
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

        private readonly CandidateRepository _candidateRepository = new CandidateRepository(DatabaseFactory.ProfilePrimary);

        private readonly CountryRepository _countryRepository = new CountryRepository(DatabaseFactory.ProfilePrimary);
        private readonly CityRepository _cityRepository = new CityRepository(DatabaseFactory.ProfilePrimary);

        private readonly LogInfoRepository _logInfoRepository = new LogInfoRepository();

        //Init permissionvariables
        private bool _canAdd = true;
        private bool _canEdit = true;
        private bool _canDelete = true;
        private bool _canPrint = true;
        private bool _isAdmin = true;
        private bool _isProtected = true;

        public EventHandler SendUpdatedCandidate;

        public HrCvEditForm(CandidateModel model)
        {
            InitializeComponent();

            _candidateModel = model ?? new CandidateModel();

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
                    LoadTitlesAsync(),
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
            bsCandidate.DataSource = _candidateModel;
            bsCandidate.ResetBindings(false);

            cboNationality.Properties.DataSource = null;
            cboNationality.Properties.DataSource = _countries;

            cboCities.Properties.DataSource = null;
            cboCities.Properties.DataSource = _cities;
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

        private async void SaveData()
        {
            try
            {
                BindingContext[bsCandidate].EndCurrentEdit();

                _candidateModel = (CandidateModel)bsCandidate.Current;

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
                SendUpdatedCandidate?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                ShowError("Error saving employee data", ex);
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
    }
}