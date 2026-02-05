using Spectrum.Models.Common.Currencies;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.Common.Currencies.Interfaces
{
	public interface ICurrencyExchangeRepository
	{
		// CRUD Operations
		Task<List<CurrencyExchangeModel>> GetCurrenciesExchangeAsync();
		Task<CurrencyExchangeModel> GetCurrencyExchangeByIdAsync(string id);
		Task<string> AddNewCurrencyExchangeAsync(CurrencyExchangeModel branch);
		Task<bool> UpdateCurrencyExchangeAsync(CurrencyExchangeModel branch);
		Task<bool> DeleteCurrencyExchangeAsync(string id);

		// A custom query example
		Task<CurrencyExchangeModel> GetCurrencyExchangeByName(string name);
		Task<long> GetCountAsync();
	}
}
