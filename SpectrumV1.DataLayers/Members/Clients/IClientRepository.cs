using SpectrumV1.Models.Members.Clients;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.Members.Clients
{
	public interface IClientRepository
	{
		// CRUD Operations
		Task<List<ClientModel>> GetClientsAsync();
		Task<ClientModel> GetClientByIdAsync(string id);
		Task<string> AddNewClientAsync(ClientModel client);
		Task<bool> UpdateClientAsync(ClientModel client);
		Task<bool> DeleteClientAsync(string id);

		// A custom query example
		Task<ClientModel> GetClientByName(string name);
		Task<long> GetCountAsync();
	}
}
