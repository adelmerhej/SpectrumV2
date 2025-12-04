using DevExpress.Utils.Menu;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using SpectrumV1.DataLayers.Common.Countries;
using SpectrumV1.DataLayers.DataAccess;
using SpectrumV1.DataLayers.Members.Clients;
using SpectrumV1.Models.Common.Countries;
using SpectrumV1.Models.Members.Clients;
using SpectrumV1.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
	
namespace SpectrumV1.Views.Members.Clients
{
	public partial class ClientEditForm : RibbonForm
	{
		#region Fields

		private ClientModel _clientModel;
		private ContactModel _contactModel;

		private IList<CityModel> _cities = new List<CityModel>();
		private IList<CountryModel> _countries = new List<CountryModel>();
		private IList<ContactModel> _contacts = new List<ContactModel>();

		private readonly ClientRepository _clientRepository;
		private readonly CountryRepository _countryRepository;
		private readonly CityRepository _cityRepository;
		private readonly ContactRepository _contactRepository;
		private readonly LogInfoRepository _logInfoRepository;

		private bool _canAdd = true;
		private bool _canEdit = true;
		private bool _canDelete = true;
		private bool _canPrint = true;
		private bool _isAdmin = true;

		private DXMenuItem[] _menuItems;

		public EventHandler SendUpdatedClient;

		#endregion

		#region Constructor

		public ClientEditForm(ClientModel model)
		{
			InitializeComponent();

			_clientModel = model ?? new ClientModel();
			
			_clientRepository = new ClientRepository(DatabaseFactory.ProfilePrimary);
			_countryRepository = new CountryRepository(DatabaseFactory.ProfilePrimary);
			_cityRepository = new CityRepository(DatabaseFactory.ProfilePrimary);
			_contactRepository = new ContactRepository(DatabaseFactory.ProfilePrimary);
			_logInfoRepository = new LogInfoRepository();

			StartLoading();
		}

		#endregion

		#region Initialization

		private async void StartLoading()
		{
			try
			{
				await InitializeBindings();
				WireUpBindings();
				ApplyDefaults();
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
				var loadTasks = new []
				{
					LoadCitiesAsync(),
					LoadCountriesAsync(),
					LoadContactsForClientAsync()
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

		private async Task LoadContactsForClientAsync()
		{
			if (string.IsNullOrEmpty(_clientModel?._id))
			{
				_contacts = new List<ContactModel>();
			}
			else
			{
				_contacts = await _contactRepository.GetContactsByClientIdAsync(_clientModel._id);
			}
		}

		private void WireUpBindings()
		{
			bsClient.DataSource = _clientModel;

			cboCountries.Properties.DataSource = _countries;
			cboCities.Properties.DataSource = _cities;
			gcContacts.DataSource = _contacts;
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
			btnAddNewContact_ItemClick(sender, null);
		}

		private void ItemEdit_Click(object sender, EventArgs e)
		{
			btnEditContact_ItemClick(sender, null);
		}

		private void ItemDelete_Click(object sender, EventArgs e)
		{
			btnDeleteContact_ItemClick(sender, null);
		}

		#endregion

		#region Button Events

		private void btnNew_ItemClick(object sender, ItemClickEventArgs e)
		{
			_clientModel = new ClientModel();
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

		#region Contact Management

		private void btnAddNewContact_ItemClick(object sender, ItemClickEventArgs e)
		{
			OpenContactEditForm(null);
		}

		private void btnEditContact_ItemClick(object sender, ItemClickEventArgs e)
		{
			var contact = GetSelectedContact();
			if (contact != null)
			{
				OpenContactEditForm(contact);
			}
		}

		private async void btnDeleteContact_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!CanDeleteContact()) return;

			var contact = GetSelectedContact();
			if (contact == null) return;

			if (!ConfirmDelete(contact.ContactName)) return;

			try
			{
				contact.Deleted = true;
				await _contactRepository.DeleteContactAsync(contact._id);
				await RefreshContactsAsync();
			}
			catch (Exception ex)
			{
				HandleDeleteError(ex);
			}
		}

		private void gvContacts_DoubleClick(object sender, EventArgs e)
		{
			var contact = GetSelectedContact();
			if (contact != null)
			{
				OpenContactEditForm(contact);
			}
		}

		private async void gvContacts_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode != Keys.Delete) return;

			if (!ConfirmDelete("Delete row?")) return;
			if (!CanDeleteContact()) return;

			var contact = GetSelectedContact();
			if (contact == null) return;

			if (!ConfirmDelete(contact.ContactName)) return;

			try
			{
				contact.Deleted = true;
				await _contactRepository.DeleteContactAsync(contact._id);
				await RefreshContactsAsync();
			}
			catch (Exception ex)
			{
				HandleDeleteError(ex);
			}
		}

		private void OpenContactEditForm(ContactModel contact)
		{
			try
			{
				var contactEditForm = new ContactEditForm(contact, _clientModel._id);
				contactEditForm.SendUpdatedContact += RcvUpdatedContactAsync;
				contactEditForm.ShowDialog();
			}
			catch (Exception ex)
			{
				ShowError("Error opening contact form", ex);
			}
		}

		private async void RcvUpdatedContactAsync(object sender, EventArgs e)
		{
			try
			{
				await RefreshContactsAsync();
			}
			catch (Exception ex)
			{
				ShowError("Error refreshing contacts", ex);
			}
		}

		private async Task RefreshContactsAsync()
		{
			await LoadContactsForClientAsync();
			gcContacts.DataSource = null;
			gcContacts.DataSource = _contacts;
			gvContacts.RefreshData();
		}

		private ContactModel GetSelectedContact()
		{
			if (!_contacts.Any()) return null;

			try
			{
				var currentRowIdObj = gvContacts.GetFocusedRowCellValue("_id");
				if (currentRowIdObj == null) return null;

				string currentRowId = currentRowIdObj.ToString();
				if (string.IsNullOrEmpty(currentRowId)) return null;

				return _contacts.SingleOrDefault(x => x._id == currentRowId);
			}
			catch (Exception ex)
			{
				ShowError("Error getting selected contact", ex);
				return null;
			}
		}

		private bool CanDeleteContact()
		{
			var dataBoundItem = gvContacts.GetFocusedRow() as ContactModel;

			if (gvContacts == null || gvContacts.SelectedRowsCount == 0)
			{
				return false;
			}

			if (gvContacts.SelectedRowsCount > 1)
			{
				XtraMessageBox.Show("Only one record can be selected at a time, please try again",
					"Delete error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			if (dataBoundItem?.IsDefault == true)
			{
				XtraMessageBox.Show("Cannot delete system record!",
					"Delete error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			return true;
		}

		#endregion

		#region Save Operations

		private async void SaveData()
		{
			try
			{
				BindingContext[bsClient].EndCurrentEdit();
				_clientModel = (ClientModel)bsClient.Current;

				bool isNewClient = string.IsNullOrEmpty(_clientModel._id);

				if (isNewClient)
				{
					await CreateNewClientAsync();
				}
				else
				{
					await UpdateExistingClientAsync();
				}

				SendUpdatedClient?.Invoke(_clientModel, EventArgs.Empty);
			}
			catch (Exception ex)
			{
				ShowError("Error saving client", ex);
			}
		}

		private async Task CreateNewClientAsync()
		{
			_logInfoRepository.CreateLogInfo(_clientModel);

			var newClientId = await _clientRepository.AddNewClientAsync(_clientModel);
			
			if (string.IsNullOrEmpty(newClientId))
			{
				throw new Exception($"Error while saving client: {txtClientName.Text}");
			}
		}

		private async Task UpdateExistingClientAsync()
		{
			_logInfoRepository.UpdateLogInfo(_clientModel);
			await _clientRepository.UpdateClientAsync(_clientModel);
		}

		#endregion

		#region Validation

		private bool ValidateData()
		{
			var validationErrors = new List<string>();

			ValidateClientName(validationErrors);
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

		private void ValidateClientName(List<string> errors)
		{
			if (string.IsNullOrWhiteSpace(txtClientName.Text))
			{
				errors.Add("Client Name cannot be empty.");
				txtClientName.Focus();
			}
		}

		private void ValidateCountry(List<string> errors)
		{
			if (string.IsNullOrWhiteSpace(cboCountries.Text))
			{
				errors.Add("Country Name cannot be empty.");
				if (!errors.Any(e => e.Contains("Client Name")))
				{
					cboCountries.Focus();
				}
			}
		}

		private void ValidateCity(List<string> errors)
		{
			if (string.IsNullOrWhiteSpace(cboCities.Text))
			{
				errors.Add("City Region Name cannot be empty.");
				if (errors.Count == 1)
				{
					cboCities.Focus();
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
			if (string.IsNullOrWhiteSpace(txtPhoneNumber1.Text))
			{
				errors.Add("At least one phone number is required.");
				if (errors.Count == 1)
				{
					txtPhoneNumber1.Focus();
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

	}
}