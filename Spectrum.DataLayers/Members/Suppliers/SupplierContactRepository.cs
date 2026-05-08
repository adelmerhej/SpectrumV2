using MongoDB.Bson;
using MongoDB.Driver;
using Spectrum.DataLayers.DataAccess;
using SpectrumV1.Models.Members.Suppliers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.Members.Suppliers
{
    public class SupplierContactRepository : ISupplierContactRepository, IDisposable
    {
        private readonly IMongoCollection<SupplierContactModel> _suppliers;
        private const string CollectionName = "SuppliersContact";

        public SupplierContactRepository(string profileName)
        {
            var database = DatabaseFactory.GetMongoDatabase(profileName);
            _suppliers = database.GetCollection<SupplierContactModel>(CollectionName);
        }

        public async Task<List<SupplierContactModel>> GetSupplierContactsAsync()
        {
            return await _suppliers.Find(supplier => true).ToListAsync();
        }

        public async Task<SupplierContactModel> GetSupplierContactByIdAsync(string id)
        {
            var filter = Builders<SupplierContactModel>.Filter.Eq(c => c._id, id);
            return await _suppliers.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<List<SupplierContactModel>> GetSupplierContactsBySupplierIdAsync(string supplierId)
        {
            var filter = Builders<SupplierContactModel>.Filter.Eq(c => c.SupplierId, supplierId);
            return await _suppliers.Find(filter).ToListAsync();
        }

        /// <summary>
        /// Adds a new supplier to the database.
        /// </summary>
        /// <returns>The newly generated Id of the supplier.</returns>
        public async Task<string> AddNewSupplierContactAsync(SupplierContactModel supplier)
        {
            try
            {
                supplier.CreatedAt = DateTime.UtcNow;
                await _suppliers.InsertOneAsync(supplier);
                return supplier._id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing supplier document.
        /// </summary>
        public async Task<bool> UpdateSupplierContactAsync(SupplierContactModel supplier)
        {
            try
            {
                var result = await _suppliers.ReplaceOneAsync(c => c._id == supplier._id, supplier);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteSupplierContactAsync(string id)
        {
            try
            {
                var result = await _suppliers.DeleteOneAsync(c => c._id == id);
                return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Check if there's records for supplier document.
        /// </summary>
        public async Task<long> GetCountAsync()
        {
            return await _suppliers.CountDocumentsAsync(new BsonDocument());
        }

        #region Implementation of IDisposable
        public void Dispose()
        {
        }
        #endregion
    }
}
