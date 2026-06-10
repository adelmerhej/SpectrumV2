using Spectrum.Models.Accounting.FinancialGroup;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.Accounting.FinancialGroup
{
    public interface IFinancialGroupRepository
    {
        // CRUD Operations
        Task<List<FinancialGroupModel>> GetFinancialGroupsAsync();
        Task<FinancialGroupModel> GetFinancialGroupByIdAsync(string id);
        Task<FinancialGroupModel> GetDefaultFinancialGroupAsync(string excludedId = null);
        Task<string> AddNewFinancialGroupAsync(FinancialGroupModel financialGroup);
        Task<bool> UpdateFinancialGroupAsync(FinancialGroupModel financialGroup);
        Task<bool> DeleteFinancialGroupAsync(string id);

        // A custom query example
        Task<FinancialGroupModel> GetFinancialGroupByName(string name);
        Task<long> GetCountAsync();
    }
}
