using MongoDB.Bson;
using MongoDB.Driver;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Members.Suppliers;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.Members.Suppliers
{
    public class SupplierRepository : ISupplierRepository, IDisposable
    {
        private readonly IMongoCollection<SupplierModel> _suppliers;
        private const string CollectionName = "Suppliers";

        // Constructor for dependency injection
        public SupplierRepository(string profileName)
        {
            var database = DatabaseFactory.GetMongoDatabase(profileName);
            _suppliers = database.GetCollection<SupplierModel>(CollectionName);
        }

        // Interface async implementations (wrapping legacy sync methods)
        public async Task<List<SupplierModel>> GetSuppliersAsync()
        {
            return await _suppliers.Find(supplier => true).ToListAsync();
        }

        public async Task<SupplierModel> GetSupplierByIdAsync(string id)
        {
            var filter = Builders<SupplierModel>.Filter.Eq(u => u._id, id);
            return await _suppliers.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<SupplierModel> GetSupplierByName(string supplierName)
        {
            if (string.IsNullOrWhiteSpace(supplierName)) return null;
            var pattern = "^" + Regex.Escape(supplierName.Trim()) + "$"; // exact match
            var filter = Builders<SupplierModel>.Filter.Regex(u => u.SupplierName, new BsonRegularExpression(pattern, "i"));
            return await _suppliers.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Adds a new supplier to the database.
        /// </summary>
        /// <returns>The newly generated Id of the supplier.</returns>
        public async Task<string> AddNewSupplierAsync(SupplierModel supplier)
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
        public async Task<bool> UpdateSupplierAsync(SupplierModel supplier)
        {
            try
            {
                var result = await _suppliers.ReplaceOneAsync(u => u._id == supplier._id, supplier);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteSupplierAsync(string id)
        {
            try
            {
                var result = await _suppliers.DeleteOneAsync(u => u._id == id);
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
