using SpectrumV1.Models.Common.Countries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.Common.Countries.Interfaces
{
	public interface IDistrictRepository
	{
		// CRUD Operations
		Task<List<DistrictModel>> GetDistrictsAsync();
		Task<DistrictModel> GetDistrictByIdAsync(string id);
		Task<string> AddNewDistrictAsync(DistrictModel district);
		Task<bool> UpdateDistrictAsync(DistrictModel district);
		Task<bool> DeleteDistrictAsync(string id);

		// A custom query example
		Task<DistrictModel> GetDistrictByName(string name);
		Task<long> GetCountAsync();
	}
}
