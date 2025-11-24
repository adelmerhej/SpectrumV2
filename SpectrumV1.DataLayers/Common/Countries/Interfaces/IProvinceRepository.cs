using SpectrumV1.Models.Common.Countries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.Common.Countries.Interfaces
{
	public interface IProvinceRepository
	{
		// CRUD Operations
		Task<List<ProvinceModel>> GetProvincesAsync();
		Task<ProvinceModel> GetProvinceByIdAsync(string id);
		Task<string> AddNewProvinceAsync(ProvinceModel province);
		Task<bool> UpdateProvinceAsync(ProvinceModel province);
		Task<bool> DeleteProvinceAsync(string id);

		// A custom query example
		Task<ProvinceModel> GetProvinceByName(string name);
		Task<long> GetCountAsync();
	}
}
