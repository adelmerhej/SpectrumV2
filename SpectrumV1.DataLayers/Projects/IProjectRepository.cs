using SpectrumV1.Models.Projects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.Projects
{
	public interface IProjectRepository
	{
		Task<List<ProjectModel>> GetProjectsAsync();
		Task<ProjectModel> GetProjectByIdAsync(string id);
		Task<ProjectModel> GetProjectByNameAsync(string name);
		Task<string> AddNewProjectAsync(ProjectModel project);
		Task<bool> UpdateProjectAsync(ProjectModel project);
		Task<bool> DeleteProjectAsync(string id);
		Task<long> GetCountAsync();
	}
}
