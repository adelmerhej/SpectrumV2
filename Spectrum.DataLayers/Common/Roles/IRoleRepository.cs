using SpectrumV1.Models.Common.Roles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.Common.Roles
{
    public interface IRoleRepository
    {
        // CRUD Operations
        Task<List<RoleModel>> GetRolesAsync();
        Task<RoleModel> GetRoleByIdAsync(string id);
        Task<string> AddNewRoleAsync(RoleModel role);
        Task<bool> UpdateRoleAsync(RoleModel role);
        Task<bool> DeleteRoleAsync(string id);

        // A custom query example
        Task<RoleModel> GetRoleByName(string name);
        Task<long> GetCountAsync();
    }
}
