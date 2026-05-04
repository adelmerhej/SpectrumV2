using Spectrum.Models.Common.Countries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.Common.Countries.Interfaces
{
	public interface IRegionRepository
	{
		// CRUD Operations
		Task<List<RegionModel>> GetRegionsAsync();
		Task<RegionModel> GetRegionByIdAsync(string id);
		Task<string> AddNewRegionAsync(RegionModel region);
		Task<bool> UpdateRegionAsync(RegionModel region);
		Task<bool> DeleteRegionAsync(string id);

		// A custom query example
		Task<RegionModel> GetRegionByName(string name);
		Task<long> GetCountAsync();
	}
}
