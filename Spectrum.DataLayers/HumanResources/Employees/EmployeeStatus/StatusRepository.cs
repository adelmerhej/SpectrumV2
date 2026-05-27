using MongoDB.Bson;
using MongoDB.Driver;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.HumanResources.Employees.EmployeeStatus;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.HumanResources.Employees.EmployeeStatus
{
    public class StatusRepository : IStatusRepository, IDisposable
    {
        private readonly IMongoCollection<StatusModel> _status;
        private const string CollectionName = "Status";

        // Constructor for dependency injection
        public StatusRepository(string profileName)
        {
            var database = DatabaseFactory.GetMongoDatabase(profileName);
            _status = database.GetCollection<StatusModel>(CollectionName);
        }

        // Interface async implementations (wrapping legacy sync methods)
        public async Task<List<StatusModel>> GetStatusAsync()
        {
            return await _status.Find(status => true).ToListAsync();
        }

        public async Task<StatusModel> GetStatusByIdAsync(string id)
        {
            var filter = Builders<StatusModel>.Filter.Eq(u => u._id, id);
            return await _status.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<StatusModel> GetStatusByName(string statusName)
        {
            if (string.IsNullOrWhiteSpace(statusName)) return null;
            var pattern = "^" + Regex.Escape(statusName.Trim()) + "$"; // exact match
            var filter = Builders<StatusModel>.Filter.Regex(u => u.Status, new BsonRegularExpression(pattern, "i"));
            return await _status.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Adds a new status to the database.
        /// </summary>
        /// <returns>The newly generated Id of the status.</returns>
        public async Task<string> AddNewStatusAsync(StatusModel status)
        {
            try
            {
                status.CreatedAt = DateTime.UtcNow;
                await _status.InsertOneAsync(status);
                return status._id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing status document.
        /// </summary>
        public async Task<bool> UpdateStatusAsync(StatusModel status)
        {
            try
            {
                var result = await _status.ReplaceOneAsync(u => u._id == status._id, status);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteStatusAsync(string id)
        {
            try
            {
                var result = await _status.DeleteOneAsync(u => u._id == id);
                return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Check if there's records for status document.
        /// </summary>
        public async Task<long> GetCountAsync()
        {
            return await _status.CountDocumentsAsync(new BsonDocument());
        }

        #region Implementation of IDisposable
        public void Dispose()
        {
        }
        #endregion
    }
}
