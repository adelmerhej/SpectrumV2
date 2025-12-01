using SpectrumV1.Models.Members.Clients;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.Members.Clients
{
	/// <summary>
	/// Helper class to migrate ContactPerson data to the new Contacts collection
	/// </summary>
	public class ClientContactMigrationHelper
	{
		private readonly ClientRepository _clientRepository;
		private readonly ContactRepository _contactRepository;

		public ClientContactMigrationHelper(string profileName)
		{
			_clientRepository = new ClientRepository(profileName);
			_contactRepository = new ContactRepository(profileName);
		}

		/// <summary>
		/// Migrates ContactPerson from ClientModel to separate ContactModel entries
		/// </summary>
		public async Task MigrateContactPersonToContactsAsync()
		{
			try
			{
				var clients = await _clientRepository.GetClientsAsync();

				foreach (var client in clients)
				{
					// TODO: Clarify where the contact person's name is stored in ClientModel.
					// Example: If the contact person is the first contact in Contacts, use:
					// var contactPersonName = client.Contacts?.FirstOrDefault()?.ContactName;

					// Skip migration if no Contacts or no contact person name is available
					var contactPersonName = client.Contacts?.FirstOrDefault()?.ContactName;
					if (string.IsNullOrWhiteSpace(contactPersonName))
						continue;

					// Check if contact already exists for this client
					var existingContacts = await _contactRepository.GetContactsByClientIdAsync(client._id);

					// If there's already a contact with the same name, skip migration
					if (existingContacts.Any(c => c.ContactName == contactPersonName))
						continue;

					// Create new contact from contactPersonName
					var contact = new ContactModel
					{
						ClientId = client._id,
						ContactName = contactPersonName,
						Email = client.Email,
						PhoneNumber1 = client.PhoneNumber1,
						PhoneNumber2 = client.PhoneNumber2,
						IsPrimary = true,
						CreatedBy = client.CreatedBy,
						Company = client.Company,
						Branch = client.Branch
					};

					await _contactRepository.AddNewContactAsync(contact);
				}
			}
			catch (Exception ex)
			{
				throw new Exception($"Error during contact migration: {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Syncs Contacts collection to ClientModel for embedded display
		/// </summary>
		public async Task SyncContactsToClientAsync(string clientId)
		{
			try
			{
				var client = await _clientRepository.GetClientByIdAsync(clientId);
				if (client == null)
					return;

				var contacts = await _contactRepository.GetContactsByClientIdAsync(clientId);
				client.Contacts = contacts;

				await _clientRepository.UpdateClientAsync(client);
			}
			catch (Exception ex)
			{
				throw new Exception($"Error syncing contacts to client: {ex.Message}", ex);
			}
		}
	}
}
