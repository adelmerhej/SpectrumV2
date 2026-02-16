using SpectrumV1.Models.HumanResources.BloodTypes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.HumanResources.BloodTypes
{
    public interface IBloodTypeRepository
    {
        // CRUD Operations
        Task<List<BloodTypeModel>> GetBloodTypesAsync();
        Task<BloodTypeModel> GetBloodTypeByIdAsync(string id);
        Task<string> AddNewBloodTypeAsync(BloodTypeModel bloodType);
        Task<bool> UpdateBloodTypeAsync(BloodTypeModel bloodType);
        Task<bool> DeleteBloodTypeAsync(string id);

        // A custom query example
        Task<BloodTypeModel> GetBloodTypeByName(string name);
        Task<long> GetCountAsync();
    }
}
