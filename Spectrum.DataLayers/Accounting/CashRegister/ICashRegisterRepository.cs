using Spectrum.Models.Accounting.CashRegister;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.Accounting.CashRegister
{
    public interface ICashRegisterRepository
    {
        // CRUD Operations
        Task<List<CashRegisterModel>> GetCashRegistersAsync();
        Task<CashRegisterModel> GetCashRegisterByIdAsync(string id);
        Task<CashRegisterModel> GetDefaultCashRegisterAsync(string excludedId = null);
        Task<string> AddNewCashRegisterAsync(CashRegisterModel cashRegister);
        Task<bool> UpdateCashRegisterAsync(CashRegisterModel cashRegister);
        Task<bool> DeleteCashRegisterAsync(string id);

        // A custom query example
        Task<CashRegisterModel> GetCashRegisterByName(string name);
        Task<long> GetCountAsync();
    }
}
