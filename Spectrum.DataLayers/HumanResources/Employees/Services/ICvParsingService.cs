using Spectrum.Models.HumanResources.Candidates;
using System.Threading;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.HumanResources.Employees.Services
{
    public interface ICvParsingService
    {
        Task<CandidateModel> ParseCvAsync(string filePath, CancellationToken cancellationToken = default(CancellationToken));
        Task<CandidateModel> ParseCvTextAsync(string cvText, CancellationToken cancellationToken = default(CancellationToken));
        Task<CvPreprocessingResultModel> PreprocessCvAsync(string filePath, CancellationToken cancellationToken = default(CancellationToken));
    }

    public class CvPreprocessingResultModel
    {
        public string FilePath { get; set; }
        public string RawExtractedText { get; set; }
        public string NormalizedPreprocessedJson { get; set; }
    }
}
