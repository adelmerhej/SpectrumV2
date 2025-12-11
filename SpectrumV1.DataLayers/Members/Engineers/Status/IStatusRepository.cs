using SpectrumV1.Models.Members.Engineers.Status;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.Members.Statuss.Status
{
	public interface IStatusRepository
	{
		// CRUD Operations
		Task<List<StatusModel>> GetStatusAsync();
		Task<StatusModel> GetStatusByIdAsync(string id);
		Task<string> AddNewStatusAsync(StatusModel status);
		Task<bool> UpdateStatusAsync(StatusModel status);
		Task<bool> DeleteStatusAsync(string id);

		// A custom query example
		Task<StatusModel> GetStatusByName(string name);
		Task<long> GetCountAsync();
	}
}
