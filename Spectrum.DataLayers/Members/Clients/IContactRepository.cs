using Spectrum.Models.Members.Clients;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.Members.Clients
{
	public interface IContactRepository
	{
		// CRUD Operations
		Task<List<ContactModel>> GetContactsAsync();
		Task<ContactModel> GetContactByIdAsync(string id);
		Task<List<ContactModel>> GetContactsByClientIdAsync(string clientId);
		Task<string> AddNewContactAsync(ContactModel contact);
		Task<bool> UpdateContactAsync(ContactModel contact);
		Task<bool> DeleteContactAsync(string id);
		Task<long> GetCountAsync();
	}
}
