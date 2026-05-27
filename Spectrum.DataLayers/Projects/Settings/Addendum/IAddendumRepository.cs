using Spectrum.Models.Projects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.Projects.Settings.Addendum
{
    public interface IAddendumRepository
    {
        // CRUD Operations
        Task<List<AddendumModel>> GetAddendumsAsync();
        Task<AddendumModel> GetAddendumByIdAsync(string id);
        Task<AddendumModel> GetDefaultAddendumAsync(string excludedId = null);
        Task<string> AddNewAddendumAsync(AddendumModel addendum);
        Task<bool> UpdateAddendumAsync(AddendumModel addendum);
        Task<bool> DeleteAddendumAsync(string id);

        // A custom query example
        Task<AddendumModel> GetAddendumBySequence(string sequence);
        Task<long> GetCountAsync();
    }
}
