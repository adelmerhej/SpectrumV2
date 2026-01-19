using SpectrumV1.Models.Common.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.Common.Services
{
	public interface IServiceTypeRepository
	{       // CRUD Operations
		Task<List<ServiceTypeModel>> GetServiceTypesAsync();
		Task<ServiceTypeModel> GetServiceTypeByIdAsync(string id);
		Task<string> AddNewServiceTypeAsync(ServiceTypeModel service);
		Task<bool> UpdateServiceTypeAsync(ServiceTypeModel service);
		Task<bool> DeleteServiceTypeAsync(string id);

		// A custom query example
		Task<ServiceTypeModel> GetServiceTypeByName(string name);
		Task<long> GetCountAsync();
	}
}
