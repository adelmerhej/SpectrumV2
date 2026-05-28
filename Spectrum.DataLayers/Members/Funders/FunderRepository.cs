using MongoDB.Bson;
using MongoDB.Driver;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Accounting.Banks;
using Spectrum.Models.Members.Funders;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.Members.Funders
{
    public class FunderRepository : IFunderRepository, IDisposable
    {
        private readonly IMongoCollection<FunderModel> _funders;
        private const string CollectionName = "Funders";

        // Constructor for dependency injection
        public FunderRepository(string profileName)
        {
            var database = DatabaseFactory.GetMongoDatabase(profileName);
            _funders = database.GetCollection<FunderModel>(CollectionName);
        }

        // Interface async implementations (wrapping legacy sync methods)
        public async Task<List<FunderModel>> GetFundersAsync()
        {
            return await _funders.Find(funder => true).ToListAsync();
        }

        public async Task<FunderModel> GetFunderByIdAsync(string id)
        {
            var filter = Builders<FunderModel>.Filter.Eq(u => u._id, id);
            return await _funders.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<FunderModel> GetFunderByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            var pattern = "^" + Regex.Escape(name.Trim()) + "$"; // exact match
            var filter = Builders<FunderModel>.Filter.Regex(u => u.Name, new BsonRegularExpression(pattern, "i"));
            return await _funders.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<FunderModel> GetDefaultFunderAsync(string excludedId = null)
        {
            var filter = Builders<FunderModel>.Filter.Eq(u => u.IsDefault, true);

            if (!string.IsNullOrWhiteSpace(excludedId))
            {
                filter &= Builders<FunderModel>.Filter.Ne(u => u._id, excludedId);
            }

            return await _funders.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Adds a new funder to the database.
        /// </summary>
        /// <returns>The newly generated Id of the funder.</returns>
        public async Task<string> AddNewFunderAsync(FunderModel funder)
        {
            try
            {
                funder.CreatedAt = DateTime.UtcNow;
                await _funders.InsertOneAsync(funder);
                return funder._id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing funder document.
        /// </summary>
        public async Task<bool> UpdateFunderAsync(FunderModel funder)
        {
            try
            {
                var result = await _funders.ReplaceOneAsync(u => u._id == funder._id, funder);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteFunderAsync(string id)
        {
            try
            {
                var result = await _funders.DeleteOneAsync(u => u._id == id);
                return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Check if there's records for funder document.
        /// </summary>
        public async Task<long> GetCountAsync()
        {
            return await _funders.CountDocumentsAsync(new BsonDocument());
        }

        #region Implementation of IDisposable
        public void Dispose()
        {
        }
        #endregion
    }
}
