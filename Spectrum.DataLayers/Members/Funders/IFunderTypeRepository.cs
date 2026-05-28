using Spectrum.Models.Members.Funders;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.Members.Funders
{
    public interface IFunderTypeRepository
    {
        // CRUD Operations
        Task<List<FunderTypeModel>> GetFunderTypesAsync();
        Task<FunderTypeModel> GetFunderTypeByIdAsync(string id);
        Task<string> AddNewFunderTypeAsync(FunderTypeModel funderType);
        Task<bool> UpdateFunderTypeAsync(FunderTypeModel funderType);
        Task<bool> DeleteFunderTypeAsync(string id);

        // A custom query example
        Task<FunderTypeModel> GetFunderTypeByNameAsync(string name);
        Task<long> GetCountAsync();
    }
}
