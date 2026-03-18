using SpectrumV1.Models.Accounting.Journals;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.Accounting.Journals
{
    public interface IJournalDetailRepository
    {
        // CRUD Operations
        Task<List<JournalDetailModel>> GetJournalDetailsAsync();
        Task<JournalDetailModel> GetJournalDetailByIdAsync(string id);
        Task<string> AddNewJournalDetailAsync(JournalDetailModel jDetail);
        Task<bool> UpdateJournalDetailAsync(JournalDetailModel jDetail);
        Task<bool> DeleteJournalDetailAsync(string id);

        // A custom query example
        Task<JournalDetailModel> GetJournalDetailByJvNo(string jvNo);
        Task<long> GetCountAsync();
    }
}
