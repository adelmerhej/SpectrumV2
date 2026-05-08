using SpectrumV1.Models.Members.Suppliers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.Members.Suppliers
{
    public interface ISupplierContactRepository
    {
        // CRUD Operations
        Task<List<SupplierContactModel>> GetSupplierContactsAsync();
        Task<SupplierContactModel> GetSupplierContactByIdAsync(string id);
        Task<List<SupplierContactModel>> GetSupplierContactsBySupplierIdAsync(string supplierId);
        Task<string> AddNewSupplierContactAsync(SupplierContactModel supplier);
        Task<bool> UpdateSupplierContactAsync(SupplierContactModel supplier);
        Task<bool> DeleteSupplierContactAsync(string id);
        Task<long> GetCountAsync();
    }
}
