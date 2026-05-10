using MongoDB.Bson;
using MongoDB.Driver;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.HumanResources.Employees;
using Spectrum.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.HumanResources.Employees
{
    public class EmployeeRepository : IEmployeeRepository, IDisposable
    {
        private readonly IMongoCollection<EmployeeModel> _employees;    
        private const string CollectionName = "Employees";

        // Constructor for dependency injection
        public EmployeeRepository(string profileName)
        {
            var database = DatabaseFactory.GetMongoDatabase(profileName);
            _employees = database.GetCollection<EmployeeModel>(CollectionName);
        }

        // Interface async implementations (wrapping legacy sync methods)
        public async Task<List<EmployeeModel>> GetEmployeesAsync()
        {
            return await _employees
                .Find(FilterDefinition<EmployeeModel>.Empty)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<List<EmployeeModel>> GetEmployeesAsync(EmployeeType employeeType)
        {
            var filter = Builders<EmployeeModel>.Filter.Eq(e => e.EmployeeType.TypeName, employeeType.ToString());
            return await _employees.Find(filter).ToListAsync().ConfigureAwait(false);
        }

        public async Task<EmployeeModel> GetEmployeeByIdAsync(string id)
        {
            var filter = Builders<EmployeeModel>.Filter.Eq(u => u._id, id);
            return await _employees.Find(filter).FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task<EmployeeModel> GetEmployeeByName(string employeeName)
        {
            if (string.IsNullOrWhiteSpace(employeeName)) return null;
            var pattern = "^" + Regex.Escape(employeeName.Trim()) + "$"; // exact match
            var filter = Builders<EmployeeModel>.Filter.Regex(u => u.FirstName, new BsonRegularExpression(pattern, "i"));
            return await _employees.Find(filter).FirstOrDefaultAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Adds a new employee to the database.
        /// </summary>
        /// <returns>The newly generated Id of the employee.</returns>
        public async Task<string> AddNewEmployeeAsync(EmployeeModel employee)
        {
            try
            {
                employee.CreatedAt = DateTime.UtcNow;
                await _employees.InsertOneAsync(employee).ConfigureAwait(false);
                return employee._id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing employee document.
        /// </summary>
        public async Task<bool> UpdateEmployeeAsync(EmployeeModel employee)
        {
            try
            {
                var result = await _employees.ReplaceOneAsync(u => u._id == employee._id, employee).ConfigureAwait(false);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteEmployeeAsync(string id)
        {
            try
            {
                var result = await _employees.DeleteOneAsync(u => u._id == id).ConfigureAwait(false);
                return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Check if there's records for employee document.
        /// </summary>
        public async Task<long> GetCountAsync()
        {
            return await _employees.CountDocumentsAsync(new BsonDocument()).ConfigureAwait(false);
        }

        public async Task<int> GetLatestEmployeeNoAsync()
        {
            var latestEmployee = await _employees.Find(new BsonDocument())
                                                 .SortByDescending(e => e.EmployeeNo)
                                                 .Limit(1)
                                                 .FirstOrDefaultAsync()
                                                 .ConfigureAwait(false);
            return latestEmployee?.EmployeeNo ?? 0;
        }

        #region Implementation of IDisposable
        public void Dispose()
        {
        }
        #endregion
    }
}
