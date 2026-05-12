using SpectrumV1.Models.HumanResources.EmployeeTypes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.EmployeeTypes
{
    public interface IEmployeeTypeRepository
    {
        // CRUD Operations
        Task<List<EmployeeTypeModel>> GetEmployeeTypesAsync();
        Task<EmployeeTypeModel> GetEmployeeTypeByIdAsync(string id);
        Task<EmployeeTypeModel> GetDefaultEmployeeTypeAsync(string excludedId = null);
        Task<string> AddNewEmployeeTypeAsync(EmployeeTypeModel employeeType);
        Task<bool> UpdateEmployeeTypeAsync(EmployeeTypeModel employeeType);
        Task<bool> DeleteEmployeeTypeAsync(string id);

        // A custom query example
        Task<EmployeeTypeModel> GetEmployeeTypeByName(string name);
        Task<long> GetCountAsync();
    }
}
