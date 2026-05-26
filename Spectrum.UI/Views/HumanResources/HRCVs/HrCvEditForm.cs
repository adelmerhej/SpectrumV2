using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraTab;
using DevExpress.Utils.Svg;
using Spectrum.DataLayers.Common.Countries;
using Spectrum.DataLayers.DataAccess;
using Spectrum.DataLayers.EmployeeTypes;
using Spectrum.DataLayers.HumanResources.Employees;
using Spectrum.Models.Administration.Connections;
using Spectrum.Models.Common.Countries;
using Spectrum.Models.HumanResources.Employees;
using Spectrum.Models.HumanResources.EmployeeTypes;
using Spectrum.Utilities;
using Spectrum.DataLayers.HumanResources.Candidates;
using Spectrum.DataLayers.HumanResources.Employees.Services;
using Spectrum.Models.HumanResources.Candidates;
using Spectrum.Utilities.Enums;
using Spectrum.Views.Main;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;

namespace Spectrum.Views.HumanResources.HRCVs
{
    public partial class HrCvEditForm : RibbonForm
    {
        private CandidateModel _candidateModel = new CandidateModel();
        private IList<CountryModel> _countries = new List<CountryModel>();
        private IList<CityModel> _cities = new List<CityModel>();
        private IList<EmployeeTypeModel> _employeeTypes = new List<EmployeeTypeModel>();

        private CandidateRepository _candidateRepository;
        private EmployeeRepository _employeeRepository;
        private EmployeeTypeRepository _employeeTypeRepository;

        private CountryRepository _countryRepository;
        private CityRepository _cityRepository;

        private LogInfoRepository _logInfoRepository;
        private ConnectionModel _connectionModel = new ConnectionModel();

        private BarButtonItem _btnConvertToEmployee;
        private BarButtonItem _btnConvertToEngineer;
        private RibbonPageGroup _conversionRibbonGroup;
        private GridControl _workGridControl;
        private GridView _workGridView;

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
            ConfigureConversionActions();
        }

        public HrCvEditForm(CandidateModel model)
        {
            InitializeComponent();
            ConfigureConversionActions();

            _candidateModel = model ?? new CandidateModel();

            if (!DesignMode && LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                InitializeRepositories();
                StartLoading();
            }
        }

        private void ConfigureConversionActions()
        {
            if (_conversionRibbonGroup != null)
                return;

            _btnConvertToEmployee = new BarButtonItem
            {
                Caption = "Convert To Employee",
                Id = ribbonControl.MaxItemId++
            };
            try
            {
                _btnConvertToEmployee.ImageOptions.SvgImage = SvgImage.FromResources("DevExpress.DevAV.Resources.NewEmployee.svg", typeof(BarButtonItem).Assembly);
            }
            catch
            {
                _btnConvertToEmployee.ImageOptions.ImageUri.Uri = "resource://DevExpress.DevAV.Resources.NewEmployee.svg";
            }

            _btnConvertToEngineer = new BarButtonItem
            {
                Caption = "Convert To Engineer",
                Id = ribbonControl.MaxItemId++
            };
            try
            {
                _btnConvertToEngineer.ImageOptions.SvgImage = SvgImage.FromResources("DevExpress.DevAV.Resources.New.svg", typeof(BarButtonItem).Assembly);
            }
            catch
            {
                _btnConvertToEngineer.ImageOptions.ImageUri.Uri = "resource://DevExpress.DevAV.Resources.New.svg";
            }

            _btnConvertToEmployee.ItemClick += btnConvertToEmployee_ItemClick;
            _btnConvertToEngineer.ItemClick += btnConvertToEngineer_ItemClick;

            ribbonControl.Items.Add(_btnConvertToEmployee);
            ribbonControl.Items.Add(_btnConvertToEngineer);

            _conversionRibbonGroup = new RibbonPageGroup
            {
                Name = "ribbonPageGroupConversion",
                Text = "Convert"
            };

            _conversionRibbonGroup.ItemLinks.Add(_btnConvertToEmployee);
            _conversionRibbonGroup.ItemLinks.Add(_btnConvertToEngineer);

            ribbonPage1.Groups.Insert(Math.Max(0, ribbonPage1.Groups.Count - 1), _conversionRibbonGroup);
        }

        private void InitializeRepositories()
        {
            _candidateRepository = new CandidateRepository(DatabaseFactory.ProfilePrimary);
            _employeeRepository = new EmployeeRepository(DatabaseFactory.ProfilePrimary);
            _employeeTypeRepository = new EmployeeTypeRepository(DatabaseFactory.ProfilePrimary);
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
                    LoadEmployeeTypesAsync(),
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

        private async Task LoadEmployeeTypesAsync()
        {
            if (_employeeTypeRepository == null)
                return;

            _employeeTypes = await _employeeTypeRepository.GetEmployeeTypesAsync();
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
            EnsureCandidateFieldBindings();
            RefreshCandidateBindings();

            cboNationality.Properties.DataSource = null;
            cboNationality.Properties.DataSource = _countries;

            cboCities.Properties.DataSource = null;
            cboCities.Properties.DataSource = _cities;

            simpleButton1.Visible = true;
            simpleButton1.Text = "View AI Summary";

            InitializeEducationAndWorkTabs();
        }

        private void EnsureCandidateFieldBindings()
        {
            EnsureBinding(txtYearsOfExperience, "EditValue", "YearsOfExperience");
            EnsureBinding(txtSummary, "EditValue", "ReviewedSummaryTitle");
        }

        private void EnsureBinding(Control control, string propertyName, string dataMember)
        {
            if (control == null)
                return;

            var existing = control.DataBindings.Cast<Binding>()
                .FirstOrDefault(b => string.Equals(b.PropertyName, propertyName, StringComparison.OrdinalIgnoreCase));

            if (existing != null)
            {
                if (!string.Equals(existing.BindingMemberInfo.BindingMember, dataMember, StringComparison.OrdinalIgnoreCase) || existing.DataSource != bsCandidate)
                {
                    control.DataBindings.Remove(existing);
                }
                else
                {
                    return;
                }
            }

            control.DataBindings.Add(propertyName, bsCandidate, dataMember, true, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void RefreshCandidateBindings()
        {
            bsCandidate.DataSource = _candidateModel;
            bsCandidate.ResetBindings(false);

            bsEducationEntry.DataSource = _candidateModel?.Education ?? new List<EducationEntryModel>();
            bsEducationEntry.ResetBindings(false);

            bsWorkExperience.DataSource = _candidateModel?.History ?? new List<WorkExperienceModel>();
            bsWorkExperience.ResetBindings(false);

            txtSummary.EditValue = ResolveReviewedSummaryTitle();
        }

        private string ResolveReviewedSummaryTitle()
        {
            if (!string.IsNullOrWhiteSpace(_candidateModel?.ReviewedSummaryTitle))
                return _candidateModel.ReviewedSummaryTitle;

            return BuildReviewedCvTitle(_candidateModel);
        }

        private string BuildReviewedCvTitle(CandidateModel candidate)
        {
            var fullName = string.Join(" ", new[] { candidate?.FirstName, candidate?.LastName }
                .Where(x => !string.IsNullOrWhiteSpace(x)))
                .Trim();

            return string.IsNullOrWhiteSpace(fullName)
                ? "Reviewed CV Summary"
                : $"Reviewed CV Summary — {fullName}";
        }

        private void InitializeEducationAndWorkTabs()
        {
            if (_workGridControl != null)
                return;

            var parent = gridControl1.Parent;
            if (parent == null)
                return;

            var bounds = gridControl1.Bounds;
            var tab = new XtraTabControl
            {
                Name = "tabEducationWork",
                Bounds = bounds,
                Anchor = gridControl1.Anchor
            };

            var pageEducation = new XtraTabPage { Text = "Education" };
            var pageWork = new XtraTabPage { Text = "Work" };

            pageEducation.Controls.Add(gridControl1);
            gridControl1.Dock = DockStyle.Fill;

            _workGridControl = new GridControl
            {
                DataSource = bsWorkExperience,
                Dock = DockStyle.Fill,
                MenuManager = ribbonControl
            };

            _workGridView = new GridView
            {
                GridControl = _workGridControl,
                OptionsView = { ShowGroupPanel = false, ColumnAutoWidth = false }
            };

            _workGridView.Columns.AddVisible("Company", "Company");
            _workGridView.Columns.AddVisible("Position", "Position");
            _workGridView.Columns.AddVisible("Duration", "Duration");

            _workGridControl.MainView = _workGridView;
            _workGridControl.ViewCollection.Add(_workGridView);
            pageWork.Controls.Add(_workGridControl);

            tab.TabPages.AddRange(new[] { pageEducation, pageWork });
            parent.Controls.Add(tab);
            tab.BringToFront();
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

            if (_btnConvertToEmployee != null)
                _btnConvertToEmployee.Enabled = _isAdmin || _canAdd;
            if (_btnConvertToEngineer != null)
                _btnConvertToEngineer.Enabled = _isAdmin || _canAdd;
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

                var extension = Path.GetExtension(txtUploadCv.Text)?.ToLowerInvariant();
                if (extension == ".doc")
                {
                    XtraMessageBox.Show("Legacy .doc format is not supported. Please save as .docx and retry.", "Unsupported Format", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ShowBusyState("AI CV Parsing", "Pre-processing CV document...");
                CvPreprocessingResultModel preprocessing;
                using (var preprocessService = new CvParsingService("phase2-preprocess-only"))
                {
                    preprocessing = await preprocessService.PreprocessCvAsync(txtUploadCv.Text);
                }

                var aiProvider = _connectionModel?.AiSettings?.Provider;
                if (string.IsNullOrWhiteSpace(aiProvider))
                    aiProvider = "OpenAI";

                var model = ResolveAiModel(aiProvider);
                var apiKey = ResolveAiApiKey(aiProvider);

                if (string.IsNullOrWhiteSpace(apiKey))
                {
                    HideBusyState();
                    XtraMessageBox.Show("Your API key is not configured for the selected provider.", "Configuration Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                CandidateModel parsedCandidate;
                ShowBusyState("AI CV Parsing", "Generating AI summary...");
                using (var service = new CvParsingService(apiKey, model, aiProvider))
                {
                    parsedCandidate = await service.ParseCvTextAsync(preprocessing.RawExtractedText);
                }

                ShowBusyState("AI CV Parsing", "Preparing summary preview...");
                var formattedSummary = BuildFormattedSummaryDocument(parsedCandidate);
                var formattedSummaryRtf = BuildFormattedSummaryRtf(parsedCandidate);
                var mergedPreprocessedJson = MergeFormattedSummaryIntoPreprocessedJson(
                    preprocessing.NormalizedPreprocessedJson,
                    formattedSummary,
                    formattedSummaryRtf);

                HideBusyState();

                DialogResult reviewResult;
                using (var reviewForm = new AiSummaryReviewForm(formattedSummaryRtf, formattedSummary))
                {
                    reviewResult = reviewForm.ShowDialog(this);
                }

                if (reviewResult != DialogResult.OK)
                {
                    XtraMessageBox.Show("Summary preview was closed without applying changes.", "Preview Closed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                ShowBusyState("AI CV Parsing", "Applying reviewed summary to candidate...");
                ApplyParsedCandidateValues(parsedCandidate, preprocessing.RawExtractedText, mergedPreprocessedJson);
                RefreshCandidateBindings();
            }
            catch (Exception ex)
            {
                ShowError("Parsing Error", ex);
            }
            finally
            {
                HideBusyState();
            }
        }

        private void ApplyParsedCandidateValues(CandidateModel parsedCandidate, string rawExtractedText, string mergedPreprocessedJson)
        {
            if (parsedCandidate == null)
                return;

            _candidateModel.RawExtractedText = rawExtractedText;
            _candidateModel.PreprocessedJson = mergedPreprocessedJson;
            _candidateModel.FirstName = parsedCandidate.FirstName;
            _candidateModel.LastName = parsedCandidate.LastName;
            _candidateModel.DateOfBirth = parsedCandidate.DateOfBirth;
            _candidateModel.Gender = parsedCandidate.Gender;
            _candidateModel.Nationality = parsedCandidate.Nationality;
            _candidateModel.Email = parsedCandidate.Email;
            _candidateModel.Phone = parsedCandidate.Phone;
            _candidateModel.City = parsedCandidate.City;
            _candidateModel.Address = parsedCandidate.Address;
            _candidateModel.Position = parsedCandidate.Position;
            _candidateModel.YearsOfExperience = parsedCandidate.YearsOfExperience;
            _candidateModel.Skills = parsedCandidate.Skills;
            _candidateModel.Summary = parsedCandidate.Summary;
            _candidateModel.Education = parsedCandidate.Education ?? new List<EducationEntryModel>();
            _candidateModel.History = parsedCandidate.History ?? new List<WorkExperienceModel>();
            _candidateModel.Confidence = parsedCandidate.Confidence;
            _candidateModel.RawInsights = parsedCandidate.RawInsights;

            _candidateModel.ReviewedSummaryTitle = BuildReviewedCvTitle(parsedCandidate);
            txtSummary.EditValue = _candidateModel.ReviewedSummaryTitle;
        }

        private void ShowBusyState(string caption, string description)
        {
            try
            {
                if (SplashScreenManager.Default == null || !SplashScreenManager.Default.IsSplashFormVisible)
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                }

                SplashScreenManager.Default?.SetWaitFormCaption(caption);
                SplashScreenManager.Default?.SetWaitFormDescription(description);
            }
            catch
            {
            }
        }

        private void HideBusyState()
        {
            try
            {
                if (SplashScreenManager.Default != null && SplashScreenManager.Default.IsSplashFormVisible)
                {
                    SplashScreenManager.CloseForm(false);
                }
            }
            catch
            {
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                var parsedPayload = ParseSummaryPayload(_candidateModel?.PreprocessedJson);
                var rtf = parsedPayload.rtf;
                var markdown = parsedPayload.markdown;

                if (string.IsNullOrWhiteSpace(rtf) && string.IsNullOrWhiteSpace(markdown))
                {
                    XtraMessageBox.Show("No saved AI summary was found for this CV record.", "AI Summary", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (var reviewForm = new AiSummaryReviewForm(rtf, markdown))
                {
                    reviewForm.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                ShowError("AI Summary Error", ex);
            }
        }

        private (string markdown, string rtf) ParseSummaryPayload(string preprocessedJson)
        {
            if (string.IsNullOrWhiteSpace(preprocessedJson))
                return (null, null);

            var markdownToken = "\"FormattedSummaryMarkdown\":";
            var rtfToken = "\"FormattedSummaryRtf\":";

            var markdown = ExtractJsonString(preprocessedJson, markdownToken);
            var rtf = ExtractJsonString(preprocessedJson, rtfToken);

            return (markdown, rtf);
        }

        private string ExtractJsonString(string json, string token)
        {
            var tokenIndex = json.IndexOf(token, StringComparison.OrdinalIgnoreCase);
            if (tokenIndex < 0)
                return null;

            var valueStart = tokenIndex + token.Length;
            while (valueStart < json.Length && char.IsWhiteSpace(json[valueStart]))
                valueStart++;

            if (valueStart >= json.Length || json[valueStart] != '"')
                return null;

            valueStart++;
            var sb = new StringBuilder();
            var escaped = false;

            for (var i = valueStart; i < json.Length; i++)
            {
                var ch = json[i];
                if (escaped)
                {
                    switch (ch)
                    {
                        case 'n': sb.Append('\n'); break;
                        case 'r': sb.Append('\r'); break;
                        case 't': sb.Append('\t'); break;
                        case '"': sb.Append('"'); break;
                        case '\\': sb.Append('\\'); break;
                        default: sb.Append(ch); break;
                    }

                    escaped = false;
                    continue;
                }

                if (ch == '\\')
                {
                    escaped = true;
                    continue;
                }

                if (ch == '"')
                    break;

                sb.Append(ch);
            }

            return sb.ToString();
        }

        private async void btnConvertToEmployee_ItemClick(object sender, ItemClickEventArgs e)
        {
            await ConvertCandidateAsync(EmployeeType.Employee);
        }

        private async void btnConvertToEngineer_ItemClick(object sender, ItemClickEventArgs e)
        {
            await ConvertCandidateAsync(EmployeeType.Engineer);
        }

        private async Task ConvertCandidateAsync(EmployeeType employeeType)
        {
            try
            {
                BindingContext[bsCandidate].EndCurrentEdit();
                _candidateModel = bsCandidate.Current as CandidateModel ?? _candidateModel;

                if (_candidateModel == null)
                {
                    XtraMessageBox.Show("Candidate data is not available for conversion.", "Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(_candidateModel.FirstName) || string.IsNullOrWhiteSpace(_candidateModel.LastName))
                {
                    XtraMessageBox.Show("Candidate First Name and Last Name are required before conversion.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_employeeTypeRepository == null || _employeeRepository == null)
                {
                    InitializeRepositories();
                    await LoadEmployeeTypesAsync();
                }

                var targetEmployee = BuildEmployeeFromCandidate(_candidateModel, employeeType);
                _logInfoRepository.CreateLogInfo(targetEmployee);

                var latestNo = await _employeeRepository.GetLatestEmployeeNoAsync();
                targetEmployee.EmployeeNo = latestNo + 1;

                await _employeeRepository.AddNewEmployeeAsync(targetEmployee);

                XtraMessageBox.Show(
                    $"Candidate converted successfully to {employeeType}.",
                    "Conversion Complete",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                ShowError("Error converting candidate", ex);
            }
        }

        private EmployeeModel BuildEmployeeFromCandidate(CandidateModel candidate, EmployeeType employeeType)
        {
            var resolvedType = EmployeeTypeRepository.ResolveEmployeeTypeModel(_employeeTypes, employeeType: employeeType)
                ?? new EmployeeTypeModel { TypeName = employeeType.ToString() };

            return new EmployeeModel
            {
                FirstName = candidate.FirstName?.Trim(),
                LastName = candidate.LastName?.Trim(),
                DateOfBirth = candidate.DateOfBirth,
                Nationality = candidate.Nationality,
                PlaceOfBirth = candidate.City,
                Address = candidate.Address,
                ActualPosition = candidate.Position,
                YearsOfExperience = ParseYearsOfExperience(candidate.YearsOfExperience),
                HiredDate = DateTime.UtcNow,
                EmployeeType = resolvedType,
                DocumentLink = candidate.DocumentLink,
                ContactInfo = new EmployeeContactInfo
                {
                    Email = candidate.Email,
                    LocalMobileNo = candidate.Phone
                },
                WorkExperience = new WorkExperienceInfo
                {
                    WorkingPosition = candidate.Position,
                    TotalYearsOfExperience = ParseYearsOfExperience(candidate.YearsOfExperience)
                },
                Education = (candidate.Education ?? new List<EducationEntryModel>())
                    .Select(MapEducation)
                    .ToList()
            };
        }

        private EducationInfo MapEducation(EducationEntryModel entry)
        {
            return new EducationInfo
            {
                Degree = entry?.Degree,
                Specialization = entry?.Specialization,
                SchoolOrUniversity = entry?.Institution,
                GraduationYear = ParseNullableInt(entry?.Year)
            };
        }

        private int ParseYearsOfExperience(string years)
        {
            if (int.TryParse(years, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed))
                return parsed;

            return 0;
        }

        private int? ParseNullableInt(string value)
        {
            if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed))
                return parsed;

            return null;
        }

        private string ResolveAiModel(string provider)
        {
            if (string.Equals(provider, "DeepSeek", StringComparison.OrdinalIgnoreCase))
            {
                return _connectionModel?.AiSettings?.DeepSeekModel
                    ?? _connectionModel?.AiModel
                    ?? "deepseek-chat";
            }

            return _connectionModel?.AiSettings?.OpenAiModel
                ?? _connectionModel?.AiModel
                ?? "gpt-4.1-mini";
        }

        private string ResolveAiApiKey(string provider)
        {
            if (string.Equals(provider, "DeepSeek", StringComparison.OrdinalIgnoreCase))
            {
                return _connectionModel?.AiSettings?.EncryptedDeepSeekApiKey
                    ?? _connectionModel?.EncryptedAiApikey;
            }

            return _connectionModel?.AiSettings?.EncryptedOpenAiApiKey
                ?? _connectionModel?.EncryptedAiApikey;
        }

        private string BuildFormattedSummaryDocument(CandidateModel candidate)
        {
            var sb = new StringBuilder();
            sb.AppendLine("# Professional CV Summary");
            sb.AppendLine();
            sb.AppendLine("## Candidate Identity");
            sb.AppendLine($"- **Name:** {SafeValue(candidate.FirstName)} {SafeValue(candidate.LastName)}");
            sb.AppendLine($"- **Position:** {SafeValue(candidate.Position)}");
            sb.AppendLine($"- **Nationality:** {SafeValue(candidate.Nationality)}");
            sb.AppendLine();
            sb.AppendLine("## Contact Information");
            sb.AppendLine($"- **Email:** {SafeValue(candidate.Email)}");
            sb.AppendLine($"- **Phone:** {SafeValue(candidate.Phone)}");
            sb.AppendLine($"- **Address:** {SafeValue(candidate.Address)}");
            sb.AppendLine();
            sb.AppendLine("## Professional Summary");
            sb.AppendLine(SafeValue(candidate.Summary));
            sb.AppendLine();
            sb.AppendLine("## Skills");
            sb.AppendLine(SafeValue(candidate.Skills));
            sb.AppendLine();

            if (candidate.Education != null && candidate.Education.Any())
            {
                sb.AppendLine("## Education");
                foreach (var education in candidate.Education)
                {
                    sb.AppendLine($"- **{SafeValue(education.Degree)}** — {SafeValue(education.Institution)} ({SafeValue(education.Year)})");
                }
                sb.AppendLine();
            }

            if (candidate.History != null && candidate.History.Any())
            {
                sb.AppendLine("## Work History");
                foreach (var history in candidate.History)
                {
                    sb.AppendLine($"- **{SafeValue(history.Position)}** at {SafeValue(history.Company)} ({SafeValue(history.Duration)})");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private string MergeFormattedSummaryIntoPreprocessedJson(string preprocessedJson, string formattedSummary, string formattedSummaryRtf)
        {
            var timestamp = DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture);

            if (string.IsNullOrWhiteSpace(preprocessedJson) || preprocessedJson.Trim() == "{}")
            {
                return "{"
                    + "\"FormattedSummaryMarkdown\":" + ToJsonString(formattedSummary) + ","
                    + "\"FormattedSummaryRtf\":" + ToJsonString(formattedSummaryRtf) + ","
                    + "\"FormattedSummaryGeneratedAtUtc\":" + ToJsonString(timestamp)
                    + "}";
            }

            var trimmed = preprocessedJson.Trim();
            if (!trimmed.EndsWith("}"))
            {
                return preprocessedJson;
            }

            var payload = "\"FormattedSummaryMarkdown\":" + ToJsonString(formattedSummary)
                + ",\"FormattedSummaryRtf\":" + ToJsonString(formattedSummaryRtf)
                + ",\"FormattedSummaryGeneratedAtUtc\":" + ToJsonString(timestamp);

            return trimmed.Substring(0, trimmed.Length - 1) + "," + payload + "}";
        }

        private string ToJsonString(string value)
        {
            if (value == null)
                return "null";

            var escaped = value
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"")
                .Replace("\r", "\\r")
                .Replace("\n", "\\n")
                .Replace("\t", "\\t");

            return "\"" + escaped + "\"";
        }

        private string BuildFormattedSummaryRtf(CandidateModel candidate)
        {
            using (var richTextBox = new RichTextBox())
            {
                richTextBox.Clear();

                AppendHeading(richTextBox, "Professional CV Summary", 18, FontStyle.Bold);
                AppendSectionSpacing(richTextBox);

                AppendHeading(richTextBox, "Candidate Identity", 14, FontStyle.Bold);
                AppendBulletLine(richTextBox, $"Name: {SafeValue(candidate.FirstName)} {SafeValue(candidate.LastName)}");
                AppendBulletLine(richTextBox, $"Position: {SafeValue(candidate.Position)}");
                AppendBulletLine(richTextBox, $"Nationality: {SafeValue(candidate.Nationality)}");
                AppendSectionSpacing(richTextBox);

                AppendHeading(richTextBox, "Contact Information", 14, FontStyle.Bold);
                AppendBulletLine(richTextBox, $"Email: {SafeValue(candidate.Email)}");
                AppendBulletLine(richTextBox, $"Phone: {SafeValue(candidate.Phone)}");
                AppendBulletLine(richTextBox, $"Address: {SafeValue(candidate.Address)}");
                AppendSectionSpacing(richTextBox);

                AppendHeading(richTextBox, "Professional Summary", 14, FontStyle.Bold);
                AppendParagraph(richTextBox, SafeValue(candidate.Summary));
                AppendSectionSpacing(richTextBox);

                AppendHeading(richTextBox, "Skills", 14, FontStyle.Bold);
                AppendParagraph(richTextBox, SafeValue(candidate.Skills));

                if (candidate.Education != null && candidate.Education.Any())
                {
                    AppendSectionSpacing(richTextBox);
                    AppendHeading(richTextBox, "Education", 14, FontStyle.Bold);
                    foreach (var education in candidate.Education)
                    {
                        AppendBulletLine(richTextBox, $"{SafeValue(education.Degree)} — {SafeValue(education.Institution)} ({SafeValue(education.Year)})");
                    }
                }

                if (candidate.History != null && candidate.History.Any())
                {
                    AppendSectionSpacing(richTextBox);
                    AppendHeading(richTextBox, "Work History", 14, FontStyle.Bold);
                    foreach (var history in candidate.History)
                    {
                        AppendBulletLine(richTextBox, $"{SafeValue(history.Position)} at {SafeValue(history.Company)} ({SafeValue(history.Duration)})");
                    }
                }

                return richTextBox.Rtf;
            }
        }

        private void AppendHeading(RichTextBox rtb, string text, float size, FontStyle style)
        {
            rtb.SelectionStart = rtb.TextLength;
            rtb.SelectionLength = 0;
            rtb.SelectionFont = new Font("Segoe UI", size, style);
            rtb.SelectionColor = Color.Black;
            rtb.AppendText(text + Environment.NewLine);
        }

        private void AppendParagraph(RichTextBox rtb, string text)
        {
            rtb.SelectionStart = rtb.TextLength;
            rtb.SelectionLength = 0;
            rtb.SelectionFont = new Font("Segoe UI", 10f, FontStyle.Regular);
            rtb.SelectionColor = Color.Black;
            rtb.AppendText(text + Environment.NewLine);
        }

        private void AppendBulletLine(RichTextBox rtb, string text)
        {
            rtb.SelectionStart = rtb.TextLength;
            rtb.SelectionLength = 0;
            rtb.SelectionFont = new Font("Segoe UI", 10f, FontStyle.Regular);
            rtb.SelectionColor = Color.Black;
            rtb.AppendText("• " + text + Environment.NewLine);
        }

        private void AppendSectionSpacing(RichTextBox rtb)
        {
            rtb.SelectionStart = rtb.TextLength;
            rtb.SelectionLength = 0;
            rtb.AppendText(Environment.NewLine);
        }

        private string SafeValue(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? "N/A" : value.Trim();
        }
    }
}