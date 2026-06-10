using MongoDB.Bson;
using MongoDB.Driver;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Accounting.FinancialGroup;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.Accounting.FinancialGroup
{
    public class FinancialGroupRepository : IFinancialGroupRepository, IDisposable
    {
        private const string CollectionName = "FinancialGroups";
        private readonly IMongoCollection<FinancialGroupModel> _financialGroups;


        // Constructor for dependency injection
        public FinancialGroupRepository(string profileName)
        {
            var database = DatabaseFactory.GetMongoDatabase(profileName);
            _financialGroups = database.GetCollection<FinancialGroupModel>(CollectionName);
        }
        public IClientSessionHandle StartSession()
        {
            var client = _financialGroups.Database.Client;
            return client.StartSession();
        }

        public async Task<List<FinancialGroupModel>> GetFinancialGroupsAsync()
        {
            return await _financialGroups.Find(financialGroup => true).ToListAsync();
        }

        public async Task<FinancialGroupModel> GetFinancialGroupByIdAsync(string id)
        {
            var filter = Builders<FinancialGroupModel>.Filter.Eq(u => u._id, id);
            return await _financialGroups.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<FinancialGroupModel> GetFinancialGroupByName(string financialGroupName)
        {
            if (string.IsNullOrWhiteSpace(financialGroupName)) return null;
            var pattern = "^" + Regex.Escape(financialGroupName.Trim()) + "$"; // exact match
            var filter = Builders<FinancialGroupModel>.Filter.Regex(u => u.Name, new BsonRegularExpression(pattern, "i"));
            return await _financialGroups.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<FinancialGroupModel> GetDefaultFinancialGroupAsync(string excludedId = null)
        {
            var filter = Builders<FinancialGroupModel>.Filter.Eq(u => u.IsDefault, true);

            if (!string.IsNullOrWhiteSpace(excludedId))
            {
                filter &= Builders<FinancialGroupModel>.Filter.Ne(u => u._id, excludedId);
            }

            return await _financialGroups.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Adds a new financialGroup to the database.
        /// </summary>
        /// <returns>The newly generated Id of the financialGroup.</returns>
        public async Task<string> AddNewFinancialGroupAsync(FinancialGroupModel financialGroup)
        {
            try
            {
                financialGroup.CreatedAt = DateTime.UtcNow;
                await _financialGroups.InsertOneAsync(financialGroup);
                return financialGroup._id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing financialGroup document.
        /// </summary>
        public async Task<bool> UpdateFinancialGroupAsync(FinancialGroupModel financialGroup)
        {
            try
            {
                var result = await _financialGroups.ReplaceOneAsync(u => u._id == financialGroup._id, financialGroup);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteFinancialGroupAsync(string id)
        {
            try
            {
                var result = await _financialGroups.DeleteOneAsync(u => u._id == id);
                return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Check if there's records for financialGroup document.
        /// </summary>
        public async Task<long> GetCountAsync()
        {
            return await _financialGroups.CountDocumentsAsync(new BsonDocument());
        }

        #region Implementation of IDisposable
        public void Dispose()
        {
        }
        #endregion
    }
}
