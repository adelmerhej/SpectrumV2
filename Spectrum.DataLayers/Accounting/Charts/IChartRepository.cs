using Spectrum.Models.Accounting.Charts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.Accounting.Charts
{
    public interface IChartRepository
    {
        // CRUD Operations
        Task<List<ChartModel>> GetChartsAsync();
        Task<ChartModel> GetChartByIdAsync(string id);
        Task<ChartModel> GetDefaultChartAsync(string excludedId = null);
        Task<string> AddNewChartAsync(ChartModel Chart);
        Task<bool> UpdateChartAsync(ChartModel Chart);
        Task<bool> DeleteChartAsync(string id);

        // A custom query example
        Task<ChartModel> GetChartByName(string name);
        string GetNewSerial(string accountNumber, string company);
        bool ValidateAccountInJournal(string accountNumber);
        Task<long> GetCountAsync();
    }
}
