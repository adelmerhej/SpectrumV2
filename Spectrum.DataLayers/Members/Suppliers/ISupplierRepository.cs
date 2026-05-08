using Spectrum.Models.Members.Suppliers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.Members.Suppliers
{
    public interface ISupplierRepository
    {
        // CRUD Operations
        Task<List<SupplierModel>> GetSuppliersAsync();
        Task<SupplierModel> GetSupplierByIdAsync(string id);
        Task<string> AddNewSupplierAsync(SupplierModel supplier);
        Task<bool> UpdateSupplierAsync(SupplierModel supplier);
        Task<bool> DeleteSupplierAsync(string id);

        // A custom query example
        Task<SupplierModel> GetSupplierByName(string name);
        Task<long> GetCountAsync();
    }
}
