using Spectrum.Models.Accounting.CostCenter;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.Accounting.CostCenter
{
    public interface ICostCenterRepository
    {
        // CRUD Operations
        Task<List<CostCenterModel>> GetCostCentersAsync();
        Task<CostCenterModel> GetCostCenterByIdAsync(string id);
        Task<CostCenterModel> GetDefaultCostCenterAsync(string excludedId = null);
        Task<string> AddNewCostCenterAsync(CostCenterModel costCenter);
        Task<bool> UpdateCostCenterAsync(CostCenterModel costCenter);
        Task<bool> DeleteCostCenterAsync(string id);

        // A custom query example
        Task<CostCenterModel> GetCostCenterByName(string name);
        Task<long> GetCountAsync();
    }
}
