using MongoDB.Bson;
using MongoDB.Driver;
using SpectrumV1.DataLayers.Common.Currencies.Interfaces;
using SpectrumV1.DataLayers.DataAccess.Types;
using SpectrumV1.Models.Common.Currencies;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.Common.Currencies
{
	public class CurrencyRepository : ICurrencyRepository, IDisposable
	{
		private readonly IMongoCollection<CurrencyModel> _currencies;
		private const string CollectionName = "Currencies";

		// Constructor for dependency injection
		public CurrencyRepository()
		{
			var connectionString = MongoDbDatabaseModel.BuildConnectionString();
			var url = new MongoUrl(connectionString);
			// Use database specified in connection string; fall back to "admin" then CollectionName
			var databaseName = string.IsNullOrWhiteSpace(url.DatabaseName) ? "admin" : url.DatabaseName.Trim();

			var client = new MongoClient(url); // reuse parsed settings
			var database = client.GetDatabase(databaseName);
			_currencies = database.GetCollection<CurrencyModel>(CollectionName);
		}

		// Interface async implementations (wrapping legacy sync methods)
		public async Task<List<CurrencyModel>> GetCurrenciesAsync()
		{
			return await _currencies.Find(currency => true).ToListAsync();
		}

		public async Task<CurrencyModel> GetCurrencyByIdAsync(string id)
		{
			var filter = Builders<CurrencyModel>.Filter.Eq(u => u._id, id);
			return await _currencies.Find(filter).FirstOrDefaultAsync();
		}

		public async Task<CurrencyModel> GetCurrencyByName(string currencyName)
		{
			if (string.IsNullOrWhiteSpace(currencyName)) return null;
			var pattern = "^" + Regex.Escape(currencyName.Trim()) + "$"; // exact match
			var filter = Builders<CurrencyModel>.Filter.Regex(u => u.CurrencyName, new BsonRegularExpression(pattern, "i"));
			return await _currencies.Find(filter).FirstOrDefaultAsync();
		}

		/// <summary>
		/// Adds a new currency to the database.
		/// </summary>
		/// <returns>The newly generated Id of the currency.</returns>
		public async Task<string> AddNewCurrencyAsync(CurrencyModel currency)
		{
			try
			{
				currency.CreatedAt = DateTime.UtcNow;
				await _currencies.InsertOneAsync(currency);
				return currency._id;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Updates an existing currency document.
		/// </summary>
		public async Task<bool> UpdateCurrencyAsync(CurrencyModel currency)
		{
			try
			{
				var result = await _currencies.ReplaceOneAsync(u => u._id == currency._id, currency);
				return result.IsAcknowledged && result.ModifiedCount > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<bool> DeleteCurrencyAsync(string id)
		{
			try
			{
				var result = await _currencies.DeleteOneAsync(u => u._id == id);
				return result.IsAcknowledged && result.DeletedCount > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Check if there's records for currency document.
		/// </summary>
		public async Task<long> GetCountAsync()
		{
			return await _currencies.CountDocumentsAsync(new BsonDocument());
		}

		#region Implementation of IDisposable
		public void Dispose()
		{
		}
		#endregion
	}
}
