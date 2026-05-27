using MongoDB.Bson;
using MongoDB.Driver;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Accounting.Banks;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.Accounting.Banks
{
    public class BankRepository : IBankRepository, IDisposable
    {
        private const string CollectionName = "Banks";
        private readonly IMongoCollection<BankModel> _banks;


        // Constructor for dependency injection
        public BankRepository(string profileName)
        {
            var database = DatabaseFactory.GetMongoDatabase(profileName);
            _banks = database.GetCollection<BankModel>(CollectionName);
        }
        public IClientSessionHandle StartSession()
        {
            var client = _banks.Database.Client;
            return client.StartSession();
        }

        public async Task<List<BankModel>> GetBanksAsync()
        {
            return await _banks.Find(bank => true).ToListAsync();
        }

        public async Task<BankModel> GetBankByIdAsync(string id)
        {
            var filter = Builders<BankModel>.Filter.Eq(u => u._id, id);
            return await _banks.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<BankModel> GetDefaultBankAsync(string excludedId = null)
        {
            var filter = Builders<BankModel>.Filter.Eq(u => u.IsDefault, true);

            if (!string.IsNullOrWhiteSpace(excludedId))
            {
                filter &= Builders<BankModel>.Filter.Ne(u => u._id, excludedId);
            }

            return await _banks.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<BankModel> GetBankByName(string bankName)
        {
            if (string.IsNullOrWhiteSpace(bankName)) return null;
            var pattern = "^" + Regex.Escape(bankName.Trim()) + "$"; // exact match
            var filter = Builders<BankModel>.Filter.Regex(u => u.BankName, new BsonRegularExpression(pattern, "i"));
            return await _banks.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Adds a new bank to the database.
        /// </summary>
        /// <returns>The newly generated Id of the bank.</returns>
        public async Task<string> AddNewBankAsync(BankModel bank)
        {
            try
            {
                bank.CreatedAt = DateTime.UtcNow;
                await _banks.InsertOneAsync(bank);
                return bank._id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing bank document.
        /// </summary>
        public async Task<bool> UpdateBankAsync(BankModel bank)
        {
            try
            {
                var result = await _banks.ReplaceOneAsync(u => u._id == bank._id, bank);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteBankAsync(string id)
        {
            try
            {
                var result = await _banks.DeleteOneAsync(u => u._id == id);
                return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Check if there's records for bank document.
        /// </summary>
        public async Task<long> GetCountAsync()
        {
            return await _banks.CountDocumentsAsync(new BsonDocument());
        }

        #region Implementation of IDisposable
        public void Dispose()
        {
        }
        #endregion
    }
}
