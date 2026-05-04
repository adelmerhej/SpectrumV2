using SpectrumV1.Models.HumanResources.Candidates;
using System.Threading;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.HumanResources.Employees.Services
{
    public interface ICvParsingService
    {
        Task<CandidateModel> ParseCvAsync(string filePath, CancellationToken cancellationToken = default(CancellationToken));
    }
}
