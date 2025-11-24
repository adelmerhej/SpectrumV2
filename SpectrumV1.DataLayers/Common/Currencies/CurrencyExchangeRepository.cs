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
	public class CurrencyExchangeRepository : ICurrencyExchangeRepository, IDisposable
	{
		private readonly IMongoCollection<CurrencyExchangeModel> _currenciesExchange;
		private const string CollectionName = "CurrenciesExchange";

		// Constructor for dependency injection
		public CurrencyExchangeRepository()
		{
			var connectionString = MongoDbDatabaseModel.BuildConnectionString();
			var url = new MongoUrl(connectionString);
			// Use database specified in connection string; fall back to "admin" then CollectionName
			var databaseName = string.IsNullOrWhiteSpace(url.DatabaseName) ? "admin" : url.DatabaseName.Trim();

			var client = new MongoClient(url); // reuse parsed settings
			var database = client.GetDatabase(databaseName);
			_currenciesExchange = database.GetCollection<CurrencyExchangeModel>(CollectionName);
		}

		// Interface async implementations (wrapping legacy sync methods)
		public async Task<List<CurrencyExchangeModel>> GetCurrenciesExchangeAsync()
		{
			return await _currenciesExchange.Find(currencyExchange => true).ToListAsync();
		}

		public async Task<CurrencyExchangeModel> GetCurrencyExchangeByIdAsync(string id)
		{
			var filter = Builders<CurrencyExchangeModel>.Filter.Eq(u => u._id, id);
			return await _currenciesExchange.Find(filter).FirstOrDefaultAsync();
		}

		public async Task<CurrencyExchangeModel> GetCurrencyExchangeByName(string currencyExchangeName)
		{
			if (string.IsNullOrWhiteSpace(currencyExchangeName)) return null;
			var pattern = "^" + Regex.Escape(currencyExchangeName.Trim()) + "$"; // exact match
			var filter = Builders<CurrencyExchangeModel>.Filter.Regex(u => u.Currency, new BsonRegularExpression(pattern, "i"));
			return await _currenciesExchange.Find(filter).FirstOrDefaultAsync();
		}

		/// <summary>
		/// Adds a new currencyExchange to the database.
		/// </summary>
		/// <returns>The newly generated Id of the currencyExchange.</returns>
		public async Task<string> AddNewCurrencyExchangeAsync(CurrencyExchangeModel currencyExchange)
		{
			try
			{
				currencyExchange.CreatedAt = DateTime.UtcNow;
				await _currenciesExchange.InsertOneAsync(currencyExchange);
				return currencyExchange._id;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Updates an existing currencyExchange document.
		/// </summary>
		public async Task<bool> UpdateCurrencyExchangeAsync(CurrencyExchangeModel currencyExchange)
		{
			try
			{
				var result = await _currenciesExchange.ReplaceOneAsync(u => u._id == currencyExchange._id, currencyExchange);
				return result.IsAcknowledged && result.ModifiedCount > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<bool> DeleteCurrencyExchangeAsync(string id)
		{
			try
			{
				var result = await _currenciesExchange.DeleteOneAsync(u => u._id == id);
				return result.IsAcknowledged && result.DeletedCount > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Check if there's records for currencyExchange document.
		/// </summary>
		public async Task<long> GetCountAsync()
		{
			return await _currenciesExchange.CountDocumentsAsync(new BsonDocument());
		}

		#region Implementation of IDisposable
		public void Dispose()
		{
		}
		#endregion
	}
}
