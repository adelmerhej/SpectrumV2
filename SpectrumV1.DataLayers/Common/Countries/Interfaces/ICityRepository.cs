using SpectrumV1.Models.Common.Countries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.Common.Countries.Interfaces
{
	public interface ICityRepository
	{
		// CRUD Operations
		Task<List<CityModel>> GetCitiesAsync();
		Task<CityModel> GetCityByIdAsync(string id);
		Task<string> AddNewCityAsync(CityModel city);
		Task<bool> UpdateCityAsync(CityModel city);
		Task<bool> DeleteCityAsync(string id);

		// A custom query example
		Task<CityModel> GetCityByName(string name);
		Task<long> GetCountAsync();
	}
}
