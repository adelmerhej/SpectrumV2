using SpectrumV1.Models.Common.Areas;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.Common.Areas
{
	public interface IAreaRepository
	{
		// CRUD Operations
		Task<List<AreaModel>> GetAreasAsync();
		Task<AreaModel> GetAreaByIdAsync(string id);
		Task<string> AddNewAreaAsync(AreaModel area);
		Task<bool> UpdateAreaAsync(AreaModel area);
		Task<bool> DeleteAreaAsync(string id);

		// A custom query example
		Task<AreaModel> GetAreaByName(string name);
		Task<long> GetCountAsync();
	}
}
