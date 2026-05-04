using MongoDB.Bson;
using MongoDB.Driver;
using Spectrum.DataLayers.DataAccess;
using SpectrumV1.Models.Accounting.FlowType;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.Accounting.FlowType
{
    public class FlowTypeRepository : IFlowTypeRepository, IDisposable
    {
        private const string CollectionName = "FlowTypes";
        private readonly IMongoCollection<FlowTypeModel> _flowTypes;


        // Constructor for dependency injection
        public FlowTypeRepository(string profileName)
        {
            var database = DatabaseFactory.GetMongoDatabase(profileName);
            _flowTypes = database.GetCollection<FlowTypeModel>(CollectionName);
        }
        public IClientSessionHandle StartSession()
        {
            var client = _flowTypes.Database.Client;
            return client.StartSession();
        }

        public async Task<List<FlowTypeModel>> GetFlowTypesAsync()
        {
            return await _flowTypes.Find(flowType => true).ToListAsync();
        }

        public async Task<FlowTypeModel> GetFlowTypeByIdAsync(string id)
        {
            var filter = Builders<FlowTypeModel>.Filter.Eq(u => u._id, id);
            return await _flowTypes.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<FlowTypeModel> GetFlowTypeByName(string flowTypeName)
        {
            if (string.IsNullOrWhiteSpace(flowTypeName)) return null;
            var pattern = "^" + Regex.Escape(flowTypeName.Trim()) + "$"; // exact match
            var filter = Builders<FlowTypeModel>.Filter.Regex(u => u.FlowTypeName, new BsonRegularExpression(pattern, "i"));
            return await _flowTypes.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Adds a new flowType to the database.
        /// </summary>
        /// <returns>The newly generated Id of the flowType.</returns>
        public async Task<string> AddNewFlowTypeAsync(FlowTypeModel flowType)
        {
            try
            {
                flowType.CreatedAt = DateTime.UtcNow;
                await _flowTypes.InsertOneAsync(flowType);
                return flowType._id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing flowType document.
        /// </summary>
        public async Task<bool> UpdateFlowTypeAsync(FlowTypeModel flowType)
        {
            try
            {
                var result = await _flowTypes.ReplaceOneAsync(u => u._id == flowType._id, flowType);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteFlowTypeAsync(string id)
        {
            try
            {
                var result = await _flowTypes.DeleteOneAsync(u => u._id == id);
                return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Check if there's records for flowType document.
        /// </summary>
        public async Task<long> GetCountAsync()
        {
            return await _flowTypes.CountDocumentsAsync(new BsonDocument());
        }

        #region Implementation of IDisposable
        public void Dispose()
        {
        }
        #endregion
    }
}
