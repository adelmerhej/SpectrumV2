using MongoDB.Bson;
using MongoDB.Driver;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Accounting.CashRegister;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.Accounting.CashRegister
{
    public class CashRegisterRepository : ICashRegisterRepository, IDisposable
    {
        private const string CollectionName = "CashRegisters";
        private readonly IMongoCollection<CashRegisterModel> _cashRegisters;


        // Constructor for dependency injection
        public CashRegisterRepository(string profileName)
        {
            var database = DatabaseFactory.GetMongoDatabase(profileName);
            _cashRegisters = database.GetCollection<CashRegisterModel>(CollectionName);
        }
        public IClientSessionHandle StartSession()
        {
            var client = _cashRegisters.Database.Client;
            return client.StartSession();
        }

        public async Task<List<CashRegisterModel>> GetCashRegistersAsync()
        {
            return await _cashRegisters.Find(cashRegister => true).ToListAsync();
        }

        public async Task<CashRegisterModel> GetCashRegisterByIdAsync(string id)
        {
            var filter = Builders<CashRegisterModel>.Filter.Eq(u => u._id, id);
            return await _cashRegisters.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<CashRegisterModel> GetCashRegisterByName(string cashRegisterName)
        {
            if (string.IsNullOrWhiteSpace(cashRegisterName)) return null;
            var pattern = "^" + Regex.Escape(cashRegisterName.Trim()) + "$"; // exact match
            var filter = Builders<CashRegisterModel>.Filter.Regex(u => u.Name, new BsonRegularExpression(pattern, "i"));
            return await _cashRegisters.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<CashRegisterModel> GetDefaultCashRegisterAsync(string excludedId = null)
        {
            var filter = Builders<CashRegisterModel>.Filter.Eq(u => u.IsDefault, true);

            if (!string.IsNullOrWhiteSpace(excludedId))
            {
                filter &= Builders<CashRegisterModel>.Filter.Ne(u => u._id, excludedId);
            }

            return await _cashRegisters.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Adds a new cashRegister to the database.
        /// </summary>
        /// <returns>The newly generated Id of the cashRegister.</returns>
        public async Task<string> AddNewCashRegisterAsync(CashRegisterModel cashRegister)
        {
            try
            {
                cashRegister.CreatedAt = DateTime.UtcNow;
                await _cashRegisters.InsertOneAsync(cashRegister);
                return cashRegister._id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing cashRegister document.
        /// </summary>
        public async Task<bool> UpdateCashRegisterAsync(CashRegisterModel cashRegister)
        {
            try
            {
                var result = await _cashRegisters.ReplaceOneAsync(u => u._id == cashRegister._id, cashRegister);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteCashRegisterAsync(string id)
        {
            try
            {
                var result = await _cashRegisters.DeleteOneAsync(u => u._id == id);
                return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Check if there's records for cashRegister document.
        /// </summary>
        public async Task<long> GetCountAsync()
        {
            return await _cashRegisters.CountDocumentsAsync(new BsonDocument());
        }

        #region Implementation of IDisposable
        public void Dispose()
        {
        }
        #endregion
    }
}
