using Spectrum.Models.HumanResources.Candidates;
using System.Threading;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.HumanResources.Employees.Services
{
    public interface ICvParsingService
    {
        Task<CandidateModel> ParseCvAsync(string filePath, CancellationToken cancellationToken = default(CancellationToken));
    }
}
