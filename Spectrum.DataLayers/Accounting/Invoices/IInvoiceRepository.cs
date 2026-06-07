using Spectrum.Models.Accounting.Invoices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.Accounting.Invoices
{
    public interface IInvoiceRepository
    {
        // CRUD Operations
        Task<List<InvoiceModel>> GetInvoicesAsync();
        Task<InvoiceModel> GetInvoiceByIdAsync(string id);
        Task<string> AddNewInvoiceAsync(InvoiceModel invoice);
        Task<bool> UpdateInvoiceAsync(InvoiceModel invoice);
        Task<bool> DeleteInvoiceAsync(string id);
        Task<long> GetCountAsync();
    }
}
