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
	public class DistrictRepository : IDistrictRepository, IDisposable
	{
		private readonly IMongoCollection<DistrictModel> _districtes;
		private const string CollectionName = "Districts";

		// Constructor for dependency injection
		public DistrictRepository(string profileName)
		{
			var database = DatabaseFactory.GetMongoDatabase(profileName);
			_districtes = database.GetCollection<DistrictModel>(CollectionName);
		}

		// Interface async implementations (wrapping legacy sync methods)
		public async Task<List<DistrictModel>> GetDistrictsAsync()
		{
			return await _districtes.Find(district => true).ToListAsync();
		}

		public async Task<DistrictModel> GetDistrictByIdAsync(string id)
		{
			var filter = Builders<DistrictModel>.Filter.Eq(u => u._id, id);
			return await _districtes.Find(filter).FirstOrDefaultAsync();
		}

		public async Task<DistrictModel> GetDistrictByName(string districtName)
		{
			if (string.IsNullOrWhiteSpace(districtName)) return null;
			var pattern = "^" + Regex.Escape(districtName.Trim()) + "$"; // exact match
			var filter = Builders<DistrictModel>.Filter.Regex(u => u.DistrictName, new BsonRegularExpression(pattern, "i"));
			return await _districtes.Find(filter).FirstOrDefaultAsync();
		}

		/// <summary>
		/// Adds a new district to the database.
		/// </summary>
		/// <returns>The newly generated Id of the district.</returns>
		public async Task<string> AddNewDistrictAsync(DistrictModel district)
		{
			try
			{
				district.CreatedAt = DateTime.UtcNow;
				await _districtes.InsertOneAsync(district);
				return district._id;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Updates an existing district document.
		/// </summary>
		public async Task<bool> UpdateDistrictAsync(DistrictModel district)
		{
			try
			{
				var result = await _districtes.ReplaceOneAsync(u => u._id == district._id, district);
				return result.IsAcknowledged && result.ModifiedCount > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		public async Task<bool> DeleteDistrictAsync(string id)
		{
			try
			{
				var result = await _districtes.DeleteOneAsync(u => u._id == id);
				return result.IsAcknowledged && result.DeletedCount > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Check if there's records for district document.
		/// </summary>
		public async Task<long> GetCountAsync()
		{
			return await _districtes.CountDocumentsAsync(new BsonDocument());
		}

		#region Implementation of IDisposable
		public void Dispose()
		{
		}
		#endregion
	}
}
