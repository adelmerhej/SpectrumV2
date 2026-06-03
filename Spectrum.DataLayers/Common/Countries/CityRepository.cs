using MongoDB.Bson;
using MongoDB.Driver;
using Spectrum.DataLayers.Common.Countries.Interfaces;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Common.Countries;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.Common.Countries
{
	public class CityRepository : ICityRepository, IDisposable
	{
		private readonly IMongoCollection<CityModel> _cities;
		private const string CollectionName = "Cities";

		// Constructor for dependency injection
		public CityRepository(string profileName)
		{
			var database = DatabaseFactory.GetMongoDatabase(profileName);
			_cities = database.GetCollection<CityModel>(CollectionName);
		}

		// Interface async implementations (wrapping legacy sync methods)
		public async Task<List<CityModel>> GetCitiesAsync()
		{
			return await _cities.Find(city => true).ToListAsync();
		}

		public async Task<CityModel> GetCityByIdAsync(string id)
		{
			var filter = Builders<CityModel>.Filter.Eq(u => u._id, id);
			return await _cities.Find(filter).FirstOrDefaultAsync();
		}

		public async Task<CityModel> GetCityByName(string cityName)
		{
			if (string.IsNullOrWhiteSpace(cityName)) return null;
			var pattern = "^" + Regex.Escape(cityName.Trim()) + "$"; // exact match
			var filter = Builders<CityModel>.Filter.Regex(u => u.CityName, new BsonRegularExpression(pattern, "i"));
			return await _cities.Find(filter).FirstOrDefaultAsync();
		}

        public async Task<CityModel> GetDefaultCityAsync(string excludedId = null)
        {
            var filter = Builders<CityModel>.Filter.Eq(u => u.IsDefault, true);

            if (!string.IsNullOrWhiteSpace(excludedId))
            {
                filter &= Builders<CityModel>.Filter.Ne(u => u._id, excludedId);
            }

            return await _cities.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Adds a new city to the database.
        /// </summary>
        /// <returns>The newly generated Id of the city.</returns>
        public async Task<string> AddNewCityAsync(CityModel city)
		{
			try
			{
				city.CreatedAt = DateTime.UtcNow;
				await _cities.InsertOneAsync(city);
				return city._id;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Updates an existing city document.
		/// </summary>
		public async Task<bool> UpdateCityAsync(CityModel city)
		{
			try
			{
				var result = await _cities.ReplaceOneAsync(u => u._id == city._id, city);
				return result.IsAcknowledged && result.ModifiedCount > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<bool> DeleteCityAsync(string id)
		{
			try
			{
				var result = await _cities.DeleteOneAsync(u => u._id == id);
				return result.IsAcknowledged && result.DeletedCount > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Check if there's records for city document.
		/// </summary>
		public async Task<long> GetCountAsync()
		{
			return await _cities.CountDocumentsAsync(new BsonDocument());
		}

		#region Implementation of IDisposable
		public void Dispose()
		{
		}
		#endregion
	}
}
