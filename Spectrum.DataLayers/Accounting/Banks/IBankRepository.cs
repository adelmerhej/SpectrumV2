using SpectrumV1.Models.Accounting.Banks;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.Accounting.Banks
{
    public interface IBankRepository
    {
        // CRUD Operations
        Task<List<BankModel>> GetBanksAsync();
        Task<BankModel> GetBankByIdAsync(string id);
        Task<string> AddNewBankAsync(BankModel bank);
        Task<bool> UpdateBankAsync(BankModel bank);
        Task<bool> DeleteBankAsync(string id);

        // A custom query example
        Task<BankModel> GetBankByName(string name);
        Task<long> GetCountAsync();
    }
}
