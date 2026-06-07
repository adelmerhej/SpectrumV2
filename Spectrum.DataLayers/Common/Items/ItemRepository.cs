using MongoDB.Bson;
using MongoDB.Driver;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Common.Items;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.Common.Items
{
    public class ItemRepository : IItemRepository, IDisposable
    {
        private readonly IMongoCollection<ItemModel> _items;
        private const string CollectionName = "Items";

        // Constructor for dependency injection
        public ItemRepository(string profileName)
        {
            var database = DatabaseFactory.GetMongoDatabase(profileName);
            _items = database.GetCollection<ItemModel>(CollectionName);
        }

        // Interface async implementations (wrapping legacy sync methods)
        public async Task<List<ItemModel>> GetItemsAsync()
        {
            return await _items.Find(item => true).ToListAsync();
        }

        public async Task<ItemModel> GetItemByIdAsync(string id)
        {
            var filter = Builders<ItemModel>.Filter.Eq(u => u._id, id);
            return await _items.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<ItemModel> GetItemByName(string itemName)
        {
            if (string.IsNullOrWhiteSpace(itemName)) return null;
            var pattern = "^" + Regex.Escape(itemName.Trim()) + "$"; // exact match
            var filter = Builders<ItemModel>.Filter.Regex(u => u.Name, new BsonRegularExpression(pattern, "i"));
            return await _items.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<ItemModel> GetDefaultItemAsync(string excludedId = null)
        {
            var filter = Builders<ItemModel>.Filter.Eq(u => u.IsDefault, true);

            if (!string.IsNullOrWhiteSpace(excludedId))
            {
                filter &= Builders<ItemModel>.Filter.Ne(u => u._id, excludedId);
            }

            return await _items.Find(filter).FirstOrDefaultAsync();
        }   


        /// <summary>
        /// Adds a new item to the database.
        /// </summary>
        /// <returns>The newly generated Id of the item.</returns>
        public async Task<string> AddNewItemAsync(ItemModel item)
        {
            try
            {
                item.CreatedAt = DateTime.UtcNow;
                await _items.InsertOneAsync(item);
                return item._id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing item document.
        /// </summary>
        public async Task<bool> UpdateItemAsync(ItemModel item)
        {
            try
            {
                var result = await _items.ReplaceOneAsync(u => u._id == item._id, item);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            try
            {
                var result = await _items.DeleteOneAsync(u => u._id == id);
                return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Check if there's records for item document.
        /// </summary>
        public async Task<long> GetCountAsync()
        {
            return await _items.CountDocumentsAsync(new BsonDocument());
        }

        #region Implementation of IDisposable
        public void Dispose()
        {
        }
        #endregion
    }
}
