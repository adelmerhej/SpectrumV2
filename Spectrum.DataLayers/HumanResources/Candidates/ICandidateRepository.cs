using SpectrumV1.Models.HumanResources.Candidates;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.HumanResources.Candidates
{
    public interface ICandidateRepository
    {
        // CRUD Operations
        Task<List<CandidateModel>> GetCandidatesAsync();
        Task<CandidateModel> GetCandidateByIdAsync(string id);
        Task<string> AddNewCandidateAsync(CandidateModel candidateModel);
        Task<bool> UpdateCandidateAsync(CandidateModel candidateModel);
        Task<bool> DeleteCandidateAsync(string id);

        // A custom query example
        Task<CandidateModel> GetCandidateByName(string name);
        Task<long> GetCountAsync();
    }
}
