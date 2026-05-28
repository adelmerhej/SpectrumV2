using Spectrum.Models.Members.Funders;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.Members.Funders
{
    public interface IFunderRepository
    {
        // CRUD Operations
        Task<List<FunderModel>> GetFundersAsync();
        Task<FunderModel> GetFunderByIdAsync(string id);
        Task<string> AddNewFunderAsync(FunderModel funder);
        Task<bool> UpdateFunderAsync(FunderModel funder);
        Task<bool> DeleteFunderAsync(string id);

        // A custom query example
        Task<FunderModel> GetFunderByName(string name);
        Task<long> GetCountAsync();
    }
}
