using MongoDB.Bson;
using MongoDB.Driver;
using SpectrumV1.DataLayers.Common.Countries.Interfaces;
using SpectrumV1.DataLayers.DataAccess.Types;
using SpectrumV1.Models.Common.Countries;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.Common.Countries
{
	public class ContinentRepository : IContinentRepository, IDisposable
	{
		private readonly IMongoCollection<ContinentModel> _continentes;
		private const string CollectionName = "Continents";

		// Constructor for dependency injection
		public ContinentRepository()
		{
			var connectionString = MongoDbDatabaseModel.BuildConnectionString();
			var url = new MongoUrl(connectionString);
			// Use database specified in connection string; fall back to "admin" then CollectionName
			var databaseName = string.IsNullOrWhiteSpace(url.DatabaseName) ? "admin" : url.DatabaseName.Trim();

			var client = new MongoClient(url); // reuse parsed settings
			var database = client.GetDatabase(databaseName);
			_continentes = database.GetCollection<ContinentModel>(CollectionName);
		}

		// Interface async implementations (wrapping legacy sync methods)
		public async Task<List<ContinentModel>> GetContinentsAsync()
		{
			return await _continentes.Find(continent => true).ToListAsync();
		}

		public async Task<ContinentModel> GetContinentByIdAsync(string id)
		{
			var filter = Builders<ContinentModel>.Filter.Eq(u => u._id, id);
			return await _continentes.Find(filter).FirstOrDefaultAsync();
		}

		public async Task<ContinentModel> GetContinentByName(string continentName)
		{
			if (string.IsNullOrWhiteSpace(continentName)) return null;
			var pattern = "^" + Regex.Escape(continentName.Trim()) + "$"; // exact match
			var filter = Builders<ContinentModel>.Filter.Regex(u => u.ContinentName, new BsonRegularExpression(pattern, "i"));
			return await _continentes.Find(filter).FirstOrDefaultAsync();
		}

		/// <summary>
		/// Adds a new continent to the database.
		/// </summary>
		/// <returns>The newly generated Id of the continent.</returns>
		public async Task<string> AddNewContinentAsync(ContinentModel continent)
		{
			try
			{
				continent.CreatedAt = DateTime.UtcNow;
				await _continentes.InsertOneAsync(continent);
				return continent._id;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Updates an existing continent document.
		/// </summary>
		public async Task<bool> UpdateContinentAsync(ContinentModel continent)
		{
			try
			{
				var result = await _continentes.ReplaceOneAsync(u => u._id == continent._id, continent);
				return result.IsAcknowledged && result.ModifiedCount > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<bool> DeleteContinentAsync(string id)
		{
			try
			{
				var result = await _continentes.DeleteOneAsync(u => u._id == id);
				return result.IsAcknowledged && result.DeletedCount > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Check if there's records for continent document.
		/// </summary>
		public async Task<long> GetCountAsync()
		{
			return await _continentes.CountDocumentsAsync(new BsonDocument());
		}

		#region Implementation of IDisposable
		public void Dispose()
		{
		}
		#endregion
	}
}
