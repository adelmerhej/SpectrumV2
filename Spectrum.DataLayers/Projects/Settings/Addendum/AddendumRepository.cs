using MongoDB.Bson;
using MongoDB.Driver;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Projects;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.Projects.Settings.Addendum
{
    public class AddendumRepository : IAddendumRepository, IDisposable
    {
        private const string CollectionName = "Addendums";
        private readonly IMongoCollection<AddendumModel> _addendums;


        // Constructor for dependency injection
        public AddendumRepository(string profileName)
        {
            var database = DatabaseFactory.GetMongoDatabase(profileName);
            _addendums = database.GetCollection<AddendumModel>(CollectionName);
        }
        public IClientSessionHandle StartSession()
        {
            var client = _addendums.Database.Client;
            return client.StartSession();
        }

        public async Task<List<AddendumModel>> GetAddendumsAsync()
        {
            return await _addendums.Find(addendum => true).ToListAsync();
        }

        public async Task<AddendumModel> GetAddendumByIdAsync(string id)
        {
            var filter = Builders<AddendumModel>.Filter.Eq(u => u._id, id);
            return await _addendums.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<AddendumModel> GetAddendumBySequence(string sequence)
        {
            if (string.IsNullOrWhiteSpace(sequence)) return null;
            var pattern = "^" + Regex.Escape(sequence.Trim()) + "$"; // exact match
            var filter = Builders<AddendumModel>.Filter.Regex(u => u.Sequence, new BsonRegularExpression(pattern, "i"));
            return await _addendums.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<AddendumModel> GetDefaultAddendumAsync(string excludedId = null)
        {
            var filter = Builders<AddendumModel>.Filter.Eq(u => u.IsDefault, true);

            if (!string.IsNullOrWhiteSpace(excludedId))
            {
                filter &= Builders<AddendumModel>.Filter.Ne(u => u._id, excludedId);
            }

            return await _addendums.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Adds a new addendum to the database.
        /// </summary>
        /// <returns>The newly generated Id of the addendum.</returns>
        public async Task<string> AddNewAddendumAsync(AddendumModel addendum)
        {
            try
            {
                addendum.CreatedAt = DateTime.UtcNow;
                await _addendums.InsertOneAsync(addendum);
                return addendum._id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing addendum document.
        /// </summary>
        public async Task<bool> UpdateAddendumAsync(AddendumModel addendum)
        {
            try
            {
                var result = await _addendums.ReplaceOneAsync(u => u._id == addendum._id, addendum);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteAddendumAsync(string id)
        {
            try
            {
                var result = await _addendums.DeleteOneAsync(u => u._id == id);
                return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Check if there's records for addendum document.
        /// </summary>
        public async Task<long> GetCountAsync()
        {
            return await _addendums.CountDocumentsAsync(new BsonDocument());
        }

        #region Implementation of IDisposable
        public void Dispose()
        {
        }
        #endregion
    }
}
