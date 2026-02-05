using Spectrum.Models.Common.Currencies;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.Common.Currencies.Interfaces
{
	public interface ICurrencyRepository
	{
		// CRUD Operations
		Task<List<CurrencyModel>> GetCurrenciesAsync();
		Task<CurrencyModel> GetCurrencyByIdAsync(string id);
		Task<string> AddNewCurrencyAsync(CurrencyModel branch);
		Task<bool> UpdateCurrencyAsync(CurrencyModel branch);
		Task<bool> DeleteCurrencyAsync(string id);

		// A custom query example
		Task<CurrencyModel> GetCurrencyByName(string name);
		Task<long> GetCountAsync();
	}
}
