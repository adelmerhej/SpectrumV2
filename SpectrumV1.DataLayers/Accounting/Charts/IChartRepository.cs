using SpectrumV1.Models.Accounting.Charts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.Accounting.Charts
{
	public interface IChartRepository
	{
		// CRUD Operations
		Task<List<ChartModel>> GetChartsAsync();
		Task<ChartModel> GetChartByIdAsync(string id);
		Task<string> AddNewChartAsync(ChartModel Chart);
		Task<bool> UpdateChartAsync(ChartModel Chart);
		Task<bool> DeleteChartAsync(string id);

		// A custom query example
		Task<ChartModel> GetChartByName(string name);
		Task<long> GetCountAsync();
	}
}
