using Spectrum.Models.Common.Countries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.Common.Countries.Interfaces
{
	public interface IContinentRepository
	{
		// CRUD Operations
		Task<List<ContinentModel>> GetContinentsAsync();
		Task<ContinentModel> GetContinentByIdAsync(string id);
		Task<string> AddNewContinentAsync(ContinentModel continent);
		Task<bool> UpdateContinentAsync(ContinentModel continent);
		Task<bool> DeleteContinentAsync(string id);

		// A custom query example
		Task<ContinentModel> GetContinentByName(string name);
		Task<long> GetCountAsync();
	}
}
