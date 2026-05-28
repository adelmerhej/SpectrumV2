using MongoDB.Bson;
using MongoDB.Driver;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Members.Funders;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.Members.Funders
{
    public class FunderTypeRepository : IFunderTypeRepository, IDisposable
    {
        private readonly IMongoCollection<FunderTypeModel> _funderTypes;
        private const string CollectionName = "FunderTypes";

        // Constructor for dependency injection
        public FunderTypeRepository(string profileName)
        {
            var database = DatabaseFactory.GetMongoDatabase(profileName);
            _funderTypes = database.GetCollection<FunderTypeModel>(CollectionName);
        }

        // Interface async implementations (wrapping legacy sync methods)
        public async Task<List<FunderTypeModel>> GetFunderTypesAsync()
        {
            return await _funderTypes.Find(funderType => true).ToListAsync();
        }

        public async Task<FunderTypeModel> GetFunderTypeByIdAsync(string id)
        {
            var filter = Builders<FunderTypeModel>.Filter.Eq(u => u._id, id);
            return await _funderTypes.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<FunderTypeModel> GetFunderTypeByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            var pattern = "^" + Regex.Escape(name.Trim()) + "$"; // exact match
            var filter = Builders<FunderTypeModel>.Filter.Regex(u => u.Type, new BsonRegularExpression(pattern, "i"));
            return await _funderTypes.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<FunderTypeModel> GetDefaultFunderTypeAsync(string excludedId = null)
        {
            var filter = Builders<FunderTypeModel>.Filter.Eq(u => u.IsDefault, true);

            if (!string.IsNullOrWhiteSpace(excludedId))
            {
                filter &= Builders<FunderTypeModel>.Filter.Ne(u => u._id, excludedId);
            }

            return await _funderTypes.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Adds a new funder type to the database.
        /// </summary>
        /// <returns>The newly generated Id of the funder type.</returns>
        public async Task<string> AddNewFunderTypeAsync(FunderTypeModel funderType)
        {
            try
            {
                funderType.CreatedAt = DateTime.UtcNow;
                await _funderTypes.InsertOneAsync(funderType);
                return funderType._id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing funder type document.
        /// </summary>
        public async Task<bool> UpdateFunderTypeAsync(FunderTypeModel funderType)
        {
            try
            {
                var result = await _funderTypes.ReplaceOneAsync(u => u._id == funderType._id, funderType);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteFunderTypeAsync(string id)
        {
            try
            {
                var result = await _funderTypes.DeleteOneAsync(u => u._id == id);
                return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Check if there's records for funder type document.
        /// </summary>
        public async Task<long> GetCountAsync()
        {
            return await _funderTypes.CountDocumentsAsync(new BsonDocument());
        }

        #region Implementation of IDisposable
        public void Dispose()
        {
        }
        #endregion
    }
}
