using DevExpress.Utils.Menu;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using Spectrum.DataLayers.Common.Countries;
using Spectrum.DataLayers.DataAccess;
using Spectrum.DataLayers.Members.Engineers;
using Spectrum.DataLayers.Members.Engineers.Status;
using Spectrum.Models.Common.Countries;
using Spectrum.Models.Members.Engineers;
using Spectrum.Models.Members.Engineers.Status;
using Spectrum.Utilities;
using SpectrumV1.DataLayers.EmployeeTypes;
using SpectrumV1.Models.HumanResources.EmployeeTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.Members.Engineers
{
    public partial class EngineerEditForm : RibbonForm
    {
        private EngineerModel _engineerModel = new EngineerModel();

        private IList<CityModel> _cities = new List<CityModel>();
        private IList<CountryModel> _countries = new List<CountryModel>();
        private IList<StatusModel> _status = new List<StatusModel>();

        private readonly EngineerRepository _engineerRepository;
        private readonly CountryRepository _countryRepository;
        private readonly CityRepository _cityRepository;
        private readonly StatusRepository _statusRepository;
        private readonly LogInfoRepository _logInfoRepository;
        private readonly EmployeeTypeRepository _employeeTypeRepository;

        // init permission variables
        private bool _canAdd = true;
        private bool _canEdit = true;
        private bool _canDelete = true;
        private bool _canPrint = true;
        private bool _isAdmin = true;

        private DXMenuItem[] _menuItems;

        public EventHandler SendUpdatedEngineer;

        public EngineerEditForm(EngineerModel model)
        {
            InitializeComponent();

            _engineerModel = model ?? new EngineerModel();

            _engineerRepository = new EngineerRepository(DatabaseFactory.ProfilePrimary);
            _countryRepository = new CountryRepository(DatabaseFactory.ProfilePrimary);
            _cityRepository = new CityRepository(DatabaseFactory.ProfilePrimary);
            _statusRepository = new StatusRepository(DatabaseFactory.ProfilePrimary);
            _logInfoRepository = new LogInfoRepository();
            _employeeTypeRepository = new EmployeeTypeRepository(DatabaseFactory.ProfilePrimary);

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
                InitializeMenuItems();
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
                    LoadCitiesAsync(),
                    LoadCountriesAsync(),
                    LoadStatusAsync(),
                };

                await Task.WhenAll(loadTasks);
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading form data", ex);
            }
        }

        private async Task LoadCitiesAsync()
        {
            _cities = await _cityRepository.GetCitiesAsync();
        }

        private async Task LoadCountriesAsync()
        {
            _countries = await _countryRepository.GetCountriesAsync();
        }

        private async Task LoadStatusAsync()
        {
            _status = await _statusRepository.GetStatusAsync();
        }

        private void WireUpBindings()
        {
            bsEngineer.DataSource = _engineerModel;

            cboNationality.Properties.DataSource = _countries;
            cboPlaceOfBirth.Properties.DataSource = _cities;
        }

        private async Task ApplyDefaultsAsync()
        {
            if (_engineerModel.EmployeeType == null || _engineerModel.EmployeeType.TypeName != "Engineer")
            {
                var employeeType = await _employeeTypeRepository.GetEmployeeTypeByName("Engineer");
                if (employeeType == null)
                {
                    employeeType = new EmployeeTypeModel { TypeName = "Engineer" };
                    await _employeeTypeRepository.AddNewEmployeeTypeAsync(employeeType);
                }
                _engineerModel.EmployeeType = employeeType;
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

        private void InitializeMenuItems()
        {
            var itemNew = new DXMenuItem("New", ItemNew_Click);
            var itemEdit = new DXMenuItem("Edit", ItemEdit_Click);
            var itemDelete = new DXMenuItem("Delete", ItemDelete_Click);
            _menuItems = new[] { itemNew, itemEdit, itemDelete };
        }

        #endregion


        #region Menu Item Events

        private void ItemNew_Click(object sender, EventArgs e)
        {
            //btnAddNewContact_ItemClick(sender, null);
        }

        private void ItemEdit_Click(object sender, EventArgs e)
        {
            //btnEditContact_ItemClick(sender, null);
        }

        private void ItemDelete_Click(object sender, EventArgs e)
        {
            //btnDeleteContact_ItemClick(sender, null);
        }

        #endregion

        #region Button Events

        private void btnNew_ItemClick(object sender, ItemClickEventArgs e)
        {
            _engineerModel = new EngineerModel();
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

        #region Save Operations

        private async void SaveData()
        {
            try
            {
                BindingContext[bsEngineer].EndCurrentEdit();
                _engineerModel = (EngineerModel)bsEngineer.Current;

                bool isNewEngineer = string.IsNullOrEmpty(_engineerModel._id);

                if (isNewEngineer)
                {
                    await CreateNewEngineerAsync();
                }
                else
                {
                    await UpdateExistingEngineerAsync();
                }

                SendUpdatedEngineer?.Invoke(_engineerModel, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                ShowError("Error saving engineer", ex);
            }
        }

        private async Task CreateNewEngineerAsync()
        {
            _logInfoRepository.CreateLogInfo(_engineerModel);

            var newEngineerId = await _engineerRepository.AddNewEngineerAsync(_engineerModel);

            if (string.IsNullOrEmpty(newEngineerId))
            {
                throw new Exception($"Error while saving engineer: {txtName.Text}");
            }
        }

        private async Task UpdateExistingEngineerAsync()
        {
            _logInfoRepository.UpdateLogInfo(_engineerModel);
            await _engineerRepository.UpdateEngineerAsync(_engineerModel);
        }

        #endregion

        #region Validation

        private bool ValidateData()
        {
            var validationErrors = new List<string>();

            ValidateEngineerName(validationErrors);
            ValidateCountry(validationErrors);
            ValidateCity(validationErrors);
            ValidateAddress(validationErrors);
            ValidatePhoneNumber(validationErrors);

            if (validationErrors.Any())
            {
                ShowValidationErrors(validationErrors);
                return false;
            }

            return true;
        }

        private void ValidateEngineerName(List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                errors.Add("Engineer Name cannot be empty.");
                txtName.Focus();
            }
        }

        private void ValidateCountry(List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(cboNationality.Text))
            {
                errors.Add("Country Name cannot be empty.");
                if (!errors.Any(e => e.Contains("Country Name")))
                {
                    cboNationality.Focus();
                }
            }
        }

        private void ValidateCity(List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(cboPlaceOfBirth.Text))
            {
                errors.Add("City Region Name cannot be empty.");
                if (errors.Count == 1)
                {
                    cboPlaceOfBirth.Focus();
                }
            }
        }

        private void ValidateAddress(List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                errors.Add("Address Name cannot be empty.");
                if (errors.Count == 1)
                {
                    txtAddress.Focus();
                }
            }
        }

        private void ValidatePhoneNumber(List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(txtLocalFixPhone.Text))
            {
                errors.Add("At least one phone number is required.");
                if (errors.Count == 1)
                {
                    txtLocalFixPhone.Focus();
                }
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

        #endregion

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

        private void cboStatus_AddNewValue(object sender, AddNewValueEventArgs e)
        {
            //StatusEditForm frm = new StatusEditForm(new StatusModel());
            //frm.SendUpdatedStatus += RcvUpdatedStatusAsync;
            //frm.ShowDialog();
        }

        private void RcvUpdatedStatusAsync(object sender, EventArgs e)
        {
            if (sender == null) return;
            //var statusModel = sender as StatusModel;

            //_status.Add(statusModel);

            //cboStatus.Properties.DataSource = null;
            //cboStatus.Properties.DataSource = _status;
            //if (statusModel != null) cboStatus.EditValue = statusModel._id;
        }
    }
}