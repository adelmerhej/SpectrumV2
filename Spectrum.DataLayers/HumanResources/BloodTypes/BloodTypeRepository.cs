using MongoDB.Bson;
using MongoDB.Driver;
using Spectrum.DataLayers.DataAccess;
using SpectrumV1.Models.HumanResources.BloodTypes;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.HumanResources.BloodTypes
{
    public class BloodTypeRepository : IBloodTypeRepository, IDisposable
    {
        private readonly IMongoCollection<BloodTypeModel> _bloodType;
        private const string CollectionName = "BloodTypes";

        // Constructor for dependency injection
        public BloodTypeRepository(string profileName)
        {
            var database = DatabaseFactory.GetMongoDatabase(profileName);
            _bloodType = database.GetCollection<BloodTypeModel>(CollectionName);
        }

        // Interface async implementations (wrapping legacy sync methods)
        public async Task<List<BloodTypeModel>> GetBloodTypesAsync()
        {
            return await _bloodType.Find(employee => true).ToListAsync();
        }

        public async Task<BloodTypeModel> GetBloodTypeByIdAsync(string id)
        {
            var filter = Builders<BloodTypeModel>.Filter.Eq(u => u._id, id);
            return await _bloodType.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<BloodTypeModel> GetBloodTypeByName(string bloodTypeName)
        {
            if (string.IsNullOrWhiteSpace(bloodTypeName)) return null;
            var pattern = "^" + Regex.Escape(bloodTypeName.Trim()) + "$"; // exact match
            var filter = Builders<BloodTypeModel>.Filter.Regex(u => u.BloodTypeName, new BsonRegularExpression(pattern, "i"));
            return await _bloodType.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Adds a new employee to the database.
        /// </summary>
        /// <returns>The newly generated Id of the employee.</returns>
        public async Task<string> AddNewBloodTypeAsync(BloodTypeModel employee)
        {
            try
            {
                employee.CreatedAt = DateTime.UtcNow;
                await _bloodType.InsertOneAsync(employee);
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
        public async Task<bool> UpdateBloodTypeAsync(BloodTypeModel employee)
        {
            try
            {
                var result = await _bloodType.ReplaceOneAsync(u => u._id == employee._id, employee);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteBloodTypeAsync(string id)
        {
            try
            {
                var result = await _bloodType.DeleteOneAsync(u => u._id == id);
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
            return await _bloodType.CountDocumentsAsync(new BsonDocument());
        }

        #region Implementation of IDisposable
        public void Dispose()
        {
        }
        #endregion
    }
}
