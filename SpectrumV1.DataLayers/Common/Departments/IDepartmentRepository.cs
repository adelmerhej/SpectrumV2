using SpectrumV1.Models.Common.Departments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.Common.Departments
{
	public interface IDepartmentRepository
	{
		// CRUD Operations
		Task<List<DepartmentModel>> GetDepartmentsAsync();
		Task<DepartmentModel> GetDepartmentByIdAsync(string id);
		Task<string> AddNewDepartmentAsync(DepartmentModel continent);
		Task<bool> UpdateDepartmentAsync(DepartmentModel continent);
		Task<bool> DeleteDepartmentAsync(string id);

		// A custom query example
		Task<DepartmentModel> GetDepartmentByName(string name);
		Task<long> GetCountAsync();
	}
}
