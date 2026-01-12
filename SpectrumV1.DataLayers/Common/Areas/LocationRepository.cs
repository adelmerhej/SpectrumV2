using MongoDB.Bson;
using MongoDB.Driver;
using SpectrumV1.DataLayers.Common.Areas;
using SpectrumV1.DataLayers.DataAccess;
using SpectrumV1.Models.Common.Areas;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.Common.Locations
{
	public class LocationRepository : ILocationRepository, IDisposable
	{
		private const string CollectionName = "Locations";
		private readonly IMongoCollection<LocationModel> _locations;


		// Constructor for dependency injection
		public LocationRepository(string profileName)
		{
			var database = DatabaseFactory.GetMongoDatabase(profileName);
			_locations = database.GetCollection<LocationModel>(CollectionName);
		}
		public IClientSessionHandle StartSession()
		{
			var client = _locations.Database.Client;
			return client.StartSession();
		}

		public async Task<List<LocationModel>> GetLocationsAsync()
		{
			return await _locations.Find(location => true).ToListAsync();
		}

		public async Task<LocationModel> GetLocationByIdAsync(string id)
		{
			var filter = Builders<LocationModel>.Filter.Eq(u => u._id, id);
			return await _locations.Find(filter).FirstOrDefaultAsync();
		}

		public async Task<LocationModel> GetLocationByName(string locationName)
		{
			if (string.IsNullOrWhiteSpace(locationName)) return null;
			var pattern = "^" + Regex.Escape(locationName.Trim()) + "$"; // exact match
			var filter = Builders<LocationModel>.Filter.Regex(u => u.LocationName, new BsonRegularExpression(pattern, "i"));
			return await _locations.Find(filter).FirstOrDefaultAsync();
		}

		/// <summary>
		/// Adds a new location to the database.
		/// </summary>
		/// <returns>The newly generated Id of the location.</returns>
		public async Task<string> AddNewLocationAsync(LocationModel location)
		{
			try
			{
				location.CreatedAt = DateTime.UtcNow;
				await _locations.InsertOneAsync(location);
				return location._id;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Updates an existing location document.
		/// </summary>
		public async Task<bool> UpdateLocationAsync(LocationModel location)
		{
			try
			{
				var result = await _locations.ReplaceOneAsync(u => u._id == location._id, location);
				return result.IsAcknowledged && result.ModifiedCount > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<bool> DeleteLocationAsync(string id)
		{
			try
			{
				var result = await _locations.DeleteOneAsync(u => u._id == id);
				return result.IsAcknowledged && result.DeletedCount > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Check if there's records for location document.
		/// </summary>
		public async Task<long> GetCountAsync()
		{
			return await _locations.CountDocumentsAsync(new BsonDocument());
		}

		#region Implementation of IDisposable
		public void Dispose()
		{
		}
		#endregion
	}
}
