using MongoDB.Bson;
using MongoDB.Driver;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Accounting.CostCenter;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.Accounting.CostCenter
{
    public class CostCenterRepository : ICostCenterRepository, IDisposable
    {
        private const string CollectionName = "CostCenters";
        private readonly IMongoCollection<CostCenterModel> _costCenters;


        // Constructor for dependency injection
        public CostCenterRepository(string profileName)
        {
            var database = DatabaseFactory.GetMongoDatabase(profileName);
            _costCenters = database.GetCollection<CostCenterModel>(CollectionName);
        }
        public IClientSessionHandle StartSession()
        {
            var client = _costCenters.Database.Client;
            return client.StartSession();
        }

        public async Task<List<CostCenterModel>> GetCostCentersAsync()
        {
            return await _costCenters.Find(costCenter => true).ToListAsync();
        }

        public async Task<CostCenterModel> GetCostCenterByIdAsync(string id)
        {
            var filter = Builders<CostCenterModel>.Filter.Eq(u => u._id, id);
            return await _costCenters.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<CostCenterModel> GetCostCenterByName(string costCenterName)
        {
            if (string.IsNullOrWhiteSpace(costCenterName)) return null;
            var pattern = "^" + Regex.Escape(costCenterName.Trim()) + "$"; // exact match
            var filter = Builders<CostCenterModel>.Filter.Regex(u => u.CostCenterName, new BsonRegularExpression(pattern, "i"));
            return await _costCenters.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<CostCenterModel> GetDefaultCostCenterAsync(string excludedId = null)
        {
            var filter = Builders<CostCenterModel>.Filter.Eq(u => u.IsDefault, true);

            if (!string.IsNullOrWhiteSpace(excludedId))
            {
                filter &= Builders<CostCenterModel>.Filter.Ne(u => u._id, excludedId);
            }

            return await _costCenters.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Adds a new costCenter to the database.
        /// </summary>
        /// <returns>The newly generated Id of the costCenter.</returns>
        public async Task<string> AddNewCostCenterAsync(CostCenterModel costCenter)
        {
            try
            {
                costCenter.CreatedAt = DateTime.UtcNow;
                await _costCenters.InsertOneAsync(costCenter);
                return costCenter._id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing costCenter document.
        /// </summary>
        public async Task<bool> UpdateCostCenterAsync(CostCenterModel costCenter)
        {
            try
            {
                var result = await _costCenters.ReplaceOneAsync(u => u._id == costCenter._id, costCenter);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteCostCenterAsync(string id)
        {
            try
            {
                var result = await _costCenters.DeleteOneAsync(u => u._id == id);
                return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Check if there's records for costCenter document.
        /// </summary>
        public async Task<long> GetCountAsync()
        {
            return await _costCenters.CountDocumentsAsync(new BsonDocument());
        }

        #region Implementation of IDisposable
        public void Dispose()
        {
        }
        #endregion
    }
}
