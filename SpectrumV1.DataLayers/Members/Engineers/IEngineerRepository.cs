using SpectrumV1.Models.HumanResources.Engineers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.Members.Engineers
{
	public interface IEngineerRepository
	{
		// CRUD Operations
		Task<List<EngineerModel>> GetEngineersAsync();
		Task<EngineerModel> GetEngineerByIdAsync(string id);
		Task<string> AddNewEngineerAsync(EngineerModel engineer);
		Task<bool> UpdateEngineerAsync(EngineerModel engineer);
		Task<bool> DeleteEngineerAsync(string id);

		// A custom query example
		Task<EngineerModel> GetEngineerByName(string name);
		Task<long> GetCountAsync();
	}
}
