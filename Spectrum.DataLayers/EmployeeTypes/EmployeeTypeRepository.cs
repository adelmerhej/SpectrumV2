using MongoDB.Bson;
using MongoDB.Driver;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Utilities.Enums;
using SpectrumV1.Models.HumanResources.EmployeeTypes;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.EmployeeTypes
{
    public class EmployeeTypeRepository : IEmployeeTypeRepository, IDisposable
    {
        private readonly IMongoCollection<EmployeeTypeModel> _employeeType;
        private const string CollectionName = "EmployeeTypes";

        // Constructor for dependency injection
        public EmployeeTypeRepository(string profileName)
        {
            var database = DatabaseFactory.GetMongoDatabase(profileName);
            _employeeType = database.GetCollection<EmployeeTypeModel>(CollectionName);
        }

        // Interface async implementations
        public async Task<List<EmployeeTypeModel>> GetEmployeeTypesAsync()
        {
            return await _employeeType.Find(e => true).ToListAsync();
        }

        public async Task<EmployeeTypeModel> GetEmployeeTypeByIdAsync(string id)
        {
            var filter = Builders<EmployeeTypeModel>.Filter.Eq(u => u._id, id);
            return await _employeeType.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<EmployeeTypeModel> GetEmployeeTypeByName(string employeeTypeName)
        {
            if (string.IsNullOrWhiteSpace(employeeTypeName)) return null;
            var pattern = "^" + Regex.Escape(employeeTypeName.Trim()) + "$"; // exact match
            var filter = Builders<EmployeeTypeModel>.Filter.Regex(u => u.TypeName, new BsonRegularExpression(pattern, "i"));
            return await _employeeType.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<EmployeeTypeModel> GetEmployeeTypeByType(EnumEmployeeType employeeType)
        {
            var typeName = employeeType.ToString();
            var pattern = "^" + Regex.Escape(typeName) + "$"; // exact match
            var filter = Builders<EmployeeTypeModel>.Filter.Regex(u => u.TypeName, new BsonRegularExpression(pattern, "i"));
            return await _employeeType.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Adds a new employee type to the database.
        /// </summary>
        /// <returns>The newly generated Id of the employee type.</returns>
        public async Task<string> AddNewEmployeeTypeAsync(EmployeeTypeModel employeeType)
        {
            try
            {
                employeeType.CreatedAt = DateTime.UtcNow;
                await _employeeType.InsertOneAsync(employeeType);
                return employeeType._id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing employee type document.
        /// </summary>
        public async Task<bool> UpdateEmployeeTypeAsync(EmployeeTypeModel employeeType)
        {
            try
            {
                var result = await _employeeType.ReplaceOneAsync(u => u._id == employeeType._id, employeeType);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteEmployeeTypeAsync(string id)
        {
            try
            {
                var result = await _employeeType.DeleteOneAsync(u => u._id == id);
                return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Check if there's records for employee type document.
        /// </summary>
        public async Task<long> GetCountAsync()
        {
            return await _employeeType.CountDocumentsAsync(new BsonDocument());
        }

        #region Implementation of IDisposable
        public void Dispose()
        {
        }
        #endregion
    }
}
