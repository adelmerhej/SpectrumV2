using Spectrum.Models.Accounting.JournalType;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.Accounting.JournalType
{
    public interface IJournalTypeRepository
    {
        // CRUD Operations
        Task<List<JournalTypeModel>> GetJournalTypesAsync();
        Task<JournalTypeModel> GetJournalTypeByIdAsync(string id);
        Task<JournalTypeModel> GetDefaultJournalTypeAsync(string excludedId = null);
        Task<string> AddNewJournalTypeAsync(JournalTypeModel journalType);
        Task<bool> UpdateJournalTypeAsync(JournalTypeModel journalType);
        Task<bool> DeleteJournalTypeAsync(string id);

        // A custom query example
        Task<JournalTypeModel> GetJournalTypeByCode(string code);
        Task<long> GetCountAsync();
    }
}
