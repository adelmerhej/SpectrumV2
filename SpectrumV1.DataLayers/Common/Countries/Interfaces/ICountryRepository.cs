using SpectrumV1.Models.Common.Countries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.Common.Countries.Interfaces
{
	public interface ICountryRepository
	{
		// CRUD Operations
		Task<List<CountryModel>> GetCountriesAsync();
		Task<CountryModel> GetCountryByIdAsync(string id);
		Task<string> AddNewCountryAsync(CountryModel country);
		Task<bool> UpdateCountryAsync(CountryModel country);
		Task<bool> DeleteCountryAsync(string id);

		// A custom query example
		Task<CountryModel> GetCountryByName(string name);
		Task<long> GetCountAsync();
	}
}
