using MongoDB.Bson;
using MongoDB.Driver;
using SpectrumV1.DataLayers.Common.Countries.Interfaces;
using SpectrumV1.DataLayers.DataAccess;
using SpectrumV1.Models.Common.Countries;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.Common.Countries
{
	public class CountryRepository : ICountryRepository, IDisposable
	{
		private readonly IMongoCollection<CountryModel> _countries;
		private const string CollectionName = "Countries";

		// Constructor for dependency injection
		public CountryRepository(string profileName)
		{
			var database = DatabaseFactory.GetMongoDatabase(profileName);
			_countries = database.GetCollection<CountryModel>(CollectionName);
		}

		// Interface async implementations (wrapping legacy sync methods)
		public async Task<List<CountryModel>> GetCountriesAsync()
		{
			return await _countries.Find(country => true).ToListAsync();
		}

		public async Task<CountryModel> GetCountryByIdAsync(string id)
		{
			var filter = Builders<CountryModel>.Filter.Eq(u => u._id, id);
			return await _countries.Find(filter).FirstOrDefaultAsync();
		}

		public async Task<CountryModel> GetCountryByName(string countryName)
		{
			if (string.IsNullOrWhiteSpace(countryName)) return null;
			var pattern = "^" + Regex.Escape(countryName.Trim()) + "$"; // exact match
			var filter = Builders<CountryModel>.Filter.Regex(u => u.CountryName, new BsonRegularExpression(pattern, "i"));
			return await _countries.Find(filter).FirstOrDefaultAsync();
		}

		/// <summary>
		/// Adds a new country to the database.
		/// </summary>
		/// <returns>The newly generated Id of the country.</returns>
		public async Task<string> AddNewCountryAsync(CountryModel country)
		{
			try
			{
				country.CreatedAt = DateTime.UtcNow;
				await _countries.InsertOneAsync(country);
				return country._id;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Updates an existing country document.
		/// </summary>
		public async Task<bool> UpdateCountryAsync(CountryModel country)
		{
			try
			{
				var result = await _countries.ReplaceOneAsync(u => u._id == country._id, country);
				return result.IsAcknowledged && result.ModifiedCount > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<bool> DeleteCountryAsync(string id)
		{
			try
			{
				var result = await _countries.DeleteOneAsync(u => u._id == id);
				return result.IsAcknowledged && result.DeletedCount > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Check if there's records for country document.
		/// </summary>
		public async Task<long> GetCountAsync()
		{
			return await _countries.CountDocumentsAsync(new BsonDocument());
		}

		#region Implementation of IDisposable
		public void Dispose()
		{
		}
		#endregion
	}
}
