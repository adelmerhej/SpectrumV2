using SpectrumV1.Models.Accounting.Charts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.Accounting.Charts
{
	public interface IChartDetailRepository
	{       // CRUD Operations
		Task<List<ChartDetailModel>> GetChartDetailsAsync();
		Task<ChartDetailModel> GetChartDetailByIdAsync(string id);
		Task<string> AddNewChartDetailAsync(ChartDetailModel Chart);
		Task<bool> UpdateChartDetailAsync(ChartDetailModel Chart);
		Task<bool> DeleteChartDetailAsync(string id);

		// A custom query example
		Task<ChartDetailModel> GetChartDetailByAccountNumber(string accountNumber);
		Task<long> GetCountAsync();
	}
}
