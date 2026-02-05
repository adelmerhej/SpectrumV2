using MongoDB.Bson;
using MongoDB.Driver;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Common.Areas;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.Common.Areas
{
	public class AreaRepository : IAreaRepository, IDisposable
	{
		private const string CollectionName = "Areas";
		private readonly IMongoCollection<AreaModel> _areas;


		// Constructor for dependency injection
		public AreaRepository(string profileName)
		{
			var database = DatabaseFactory.GetMongoDatabase(profileName);
			_areas = database.GetCollection<AreaModel>(CollectionName);
		}
		public IClientSessionHandle StartSession()
		{
			var client = _areas.Database.Client;
			return client.StartSession();
		}

		public async Task<List<AreaModel>> GetAreasAsync()
		{
			return await _areas.Find(area => true).ToListAsync();
		}

		public async Task<AreaModel> GetAreaByIdAsync(string id)
		{
			var filter = Builders<AreaModel>.Filter.Eq(u => u._id, id);
			return await _areas.Find(filter).FirstOrDefaultAsync();
		}

		public async Task<AreaModel> GetAreaByName(string areaName)
		{
			if (string.IsNullOrWhiteSpace(areaName)) return null;
			var pattern = "^" + Regex.Escape(areaName.Trim()) + "$"; // exact match
			var filter = Builders<AreaModel>.Filter.Regex(u => u.AreaName, new BsonRegularExpression(pattern, "i"));
			return await _areas.Find(filter).FirstOrDefaultAsync();
		}

		/// <summary>
		/// Adds a new area to the database.
		/// </summary>
		/// <returns>The newly generated Id of the area.</returns>
		public async Task<string> AddNewAreaAsync(AreaModel area)
		{
			try
			{
				area.CreatedAt = DateTime.UtcNow;
				await _areas.InsertOneAsync(area);
				return area._id;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Updates an existing area document.
		/// </summary>
		public async Task<bool> UpdateAreaAsync(AreaModel area)
		{
			try
			{
				var result = await _areas.ReplaceOneAsync(u => u._id == area._id, area);
				return result.IsAcknowledged && result.ModifiedCount > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<bool> DeleteAreaAsync(string id)
		{
			try
			{
				var result = await _areas.DeleteOneAsync(u => u._id == id);
				return result.IsAcknowledged && result.DeletedCount > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Check if there's records for area document.
		/// </summary>
		public async Task<long> GetCountAsync()
		{
			return await _areas.CountDocumentsAsync(new BsonDocument());
		}

		#region Implementation of IDisposable
		public void Dispose()
		{
		}
		#endregion
	}
}
