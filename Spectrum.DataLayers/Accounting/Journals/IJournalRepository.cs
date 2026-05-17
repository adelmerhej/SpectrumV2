using Spectrum.Models.Accounting.Journals;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.Accounting.Journals
{
    public interface IJournalRepository
    {
        // CRUD Operations
        Task<List<JournalModel>> GetJournalsAsync();
        Task<JournalModel> GetJournalByIdAsync(string id);
        Task<string> AddNewJournalAsync(JournalModel journal);
        Task<bool> UpdateJournalAsync(JournalModel journal);
        Task<bool> DeleteJournalAsync(string id);

        // A custom query example
        Task<JournalModel> GetJournalByJvNo(string jvNo);
        Task<string> GetNextJvNoAsync(int workingYear);
        Task<long> GetCountAsync();
    }
}
