using MongoDB.Bson;
using MongoDB.Driver;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Accounting.JournalType;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.Accounting.JournalType
{
    public class JournalTypeRepository : IJournalTypeRepository, IDisposable
    {
        private const string CollectionName = "JournalTypes";
        private readonly IMongoCollection<JournalTypeModel> _journalTypes;


        // Constructor for dependency injection
        public JournalTypeRepository(string profileName)
        {
            var database = DatabaseFactory.GetMongoDatabase(profileName);
            _journalTypes = database.GetCollection<JournalTypeModel>(CollectionName);
        }
        public IClientSessionHandle StartSession()
        {
            var client = _journalTypes.Database.Client;
            return client.StartSession();
        }

        public async Task<List<JournalTypeModel>> GetJournalTypesAsync()
        {
            return await _journalTypes.Find(journalType => true).ToListAsync();
        }

        public async Task<JournalTypeModel> GetJournalTypeByIdAsync(string id)
        {
            var filter = Builders<JournalTypeModel>.Filter.Eq(u => u._id, id);
            return await _journalTypes.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<JournalTypeModel> GetJournalTypeByCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return null;
            var pattern = "^" + Regex.Escape(code.Trim()) + "$"; // exact match
            var filter = Builders<JournalTypeModel>.Filter.Regex(u => u.Code, new BsonRegularExpression(pattern, "i"));
            return await _journalTypes.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<JournalTypeModel> GetDefaultJournalTypeAsync(string excludedId = null)
        {
            var filter = Builders<JournalTypeModel>.Filter.Eq(u => u.IsDefault, true);

            if (!string.IsNullOrWhiteSpace(excludedId))
            {
                filter &= Builders<JournalTypeModel>.Filter.Ne(u => u._id, excludedId);
            }

            return await _journalTypes.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Adds a new journalType to the database.
        /// </summary>
        /// <returns>The newly generated Id of the journalType.</returns>
        public async Task<string> AddNewJournalTypeAsync(JournalTypeModel journalType)
        {
            try
            {
                journalType.CreatedAt = DateTime.UtcNow;
                await _journalTypes.InsertOneAsync(journalType);
                return journalType._id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing journalType document.
        /// </summary>
        public async Task<bool> UpdateJournalTypeAsync(JournalTypeModel journalType)
        {
            try
            {
                var result = await _journalTypes.ReplaceOneAsync(u => u._id == journalType._id, journalType);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteJournalTypeAsync(string id)
        {
            try
            {
                var result = await _journalTypes.DeleteOneAsync(u => u._id == id);
                return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Check if there's records for journalType document.
        /// </summary>
        public async Task<long> GetCountAsync()
        {
            return await _journalTypes.CountDocumentsAsync(new BsonDocument());
        }

        #region Implementation of IDisposable
        public void Dispose()
        {
        }
        #endregion
    }
}
