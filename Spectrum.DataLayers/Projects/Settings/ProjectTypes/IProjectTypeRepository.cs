using Spectrum.Models.Operations.Projects.Settings.ProjectTypes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.Projects.Settings.ProjectTypes
{
    public interface IProjectTypeRepository
    {
        Task<List<ProjectTypeModel>> GetProjectTypesAsync();
        Task<ProjectTypeModel> GetProjectTypeByIdAsync(string id);
        Task<ProjectTypeModel> GetProjectTypeBySectorAsync(string sector);
        Task<string> AddNewProjectTypeAsync(ProjectTypeModel projectType);
        Task<bool> UpdateProjectTypeAsync(ProjectTypeModel projectType);
        Task<bool> DeleteProjectTypeAsync(string id);
        Task<long> GetCountAsync();
    }
}
