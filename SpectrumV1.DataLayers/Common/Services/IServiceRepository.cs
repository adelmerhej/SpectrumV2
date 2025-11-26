using SpectrumV1.Models.Common.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.Common.Services
{
	public interface IServiceRepository
	{
		// CRUD Operations
		Task<List<ServiceModel>> GetServicesAsync();
		Task<ServiceModel> GetServiceByIdAsync(string id);
		Task<string> AddNewServiceAsync(ServiceModel service);
		Task<bool> UpdateServiceAsync(ServiceModel service);
		Task<bool> DeleteServiceAsync(string id);

		// A custom query example
		Task<ServiceModel> GetServiceByName(string name);
		Task<long> GetCountAsync();
	}
}
