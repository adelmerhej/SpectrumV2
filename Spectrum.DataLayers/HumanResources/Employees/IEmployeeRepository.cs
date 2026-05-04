using Spectrum.Models.HumanResources.Employees;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.HumanResources.Employees
{
    public interface IEmployeeRepository
    {
        // CRUD Operations
        Task<List<EmployeeModel>> GetEmployeesAsync();
        Task<EmployeeModel> GetEmployeeByIdAsync(string id);
        Task<string> AddNewEmployeeAsync(EmployeeModel employeePosition);
        Task<bool> UpdateEmployeeAsync(EmployeeModel employeePosition);
        Task<bool> DeleteEmployeeAsync(string id);

        // A custom query example
        Task<EmployeeModel> GetEmployeeByName(string name);
        Task<long> GetCountAsync();
        Task<int> GetLatestEmployeeNoAsync();
    }
}
