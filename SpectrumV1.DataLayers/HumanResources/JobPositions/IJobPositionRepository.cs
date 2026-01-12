using SpectrumV1.Models.HumanResources.JobPositions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.HumanResources.JobPositions
{
	public interface IJobPositionRepository
	{
		// CRUD Operations
		Task<List<JobPositionModel>> GetJobPositionsAsync();
		Task<JobPositionModel> GetJobPositionByIdAsync(string id);
		Task<string> AddNewJobPositionAsync(JobPositionModel jobPosition);
		Task<bool> UpdateJobPositionAsync(JobPositionModel jobPosition);
		Task<bool> DeleteJobPositionAsync(string id);

		// A custom query example
		Task<JobPositionModel> GetJobPositionByName(string name);
		Task<long> GetCountAsync();
	}
}
