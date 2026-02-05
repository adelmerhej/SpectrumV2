using Spectrum.Models.Common.Areas;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.Common.Areas
{
	public interface ILocationRepository
	{
		Task<List<LocationModel>> GetLocationsAsync();
		Task<LocationModel> GetLocationByIdAsync(string id);
		Task<string> AddNewLocationAsync(LocationModel area);
		Task<bool> UpdateLocationAsync(LocationModel area);
		Task<bool> DeleteLocationAsync(string id);

		// A custom query example
		Task<LocationModel> GetLocationByName(string name);
		Task<long> GetCountAsync();
	}
}
