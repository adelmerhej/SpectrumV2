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
	public class RegionRepository : IRegionRepository, IDisposable
	{
		private readonly IMongoCollection<RegionModel> _regions;
		private const string CollectionName = "Regions";

		// Constructor for dependency injection
		public RegionRepository(string profileName)
		{
			var database = DatabaseFactory.GetMongoDatabase(profileName);
			_regions = database.GetCollection<RegionModel>(CollectionName);
		}

		// Interface async implementations (wrapping legacy sync methods)
		public async Task<List<RegionModel>> GetRegionsAsync()
		{
			return await _regions.Find(region => true).ToListAsync();
		}

		public async Task<RegionModel> GetRegionByIdAsync(string id)
		{
			var filter = Builders<RegionModel>.Filter.Eq(u => u._id, id);
			return await _regions.Find(filter).FirstOrDefaultAsync();
		}

		public async Task<RegionModel> GetRegionByName(string regionName)
		{
			if (string.IsNullOrWhiteSpace(regionName)) return null;
			var pattern = "^" + Regex.Escape(regionName.Trim()) + "$"; // exact match
			var filter = Builders<RegionModel>.Filter.Regex(u => u.RegionName, new BsonRegularExpression(pattern, "i"));
			return await _regions.Find(filter).FirstOrDefaultAsync();
		}

		/// <summary>
		/// Adds a new region to the database.
		/// </summary>
		/// <returns>The newly generated Id of the region.</returns>
		public async Task<string> AddNewRegionAsync(RegionModel region)
		{
			try
			{
				region.CreatedAt = DateTime.UtcNow;
				await _regions.InsertOneAsync(region);
				return region._id;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Updates an existing region document.
		/// </summary>
		public async Task<bool> UpdateRegionAsync(RegionModel region)
		{
			try
			{
				var result = await _regions.ReplaceOneAsync(u => u._id == region._id, region);
				return result.IsAcknowledged && result.ModifiedCount > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<bool> DeleteRegionAsync(string id)
		{
			try
			{
				var result = await _regions.DeleteOneAsync(u => u._id == id);
				return result.IsAcknowledged && result.DeletedCount > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Check if there's records for region document.
		/// </summary>
		public async Task<long> GetCountAsync()
		{
			return await _regions.CountDocumentsAsync(new BsonDocument());
		}

		#region Implementation of IDisposable
		public void Dispose()
		{
		}
		#endregion
	}
}
