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
	public class ProvinceRepository : IProvinceRepository, IDisposable
	{
		private readonly IMongoCollection<ProvinceModel> _provincees;
		private const string CollectionName = "Provinces";

		// Constructor for dependency injection
		public ProvinceRepository(string profileName)
		{
			var database = DatabaseFactory.GetMongoDatabase(profileName);
			_provincees = database.GetCollection<ProvinceModel>(CollectionName);
		}

		// Interface async implementations (wrapping legacy sync methods)
		public async Task<List<ProvinceModel>> GetProvincesAsync()
		{
			return await _provincees.Find(province => true).ToListAsync();
		}

		public async Task<ProvinceModel> GetProvinceByIdAsync(string id)
		{
			var filter = Builders<ProvinceModel>.Filter.Eq(u => u._id, id);
			return await _provincees.Find(filter).FirstOrDefaultAsync();
		}

		public async Task<ProvinceModel> GetProvinceByName(string provinceName)
		{
			if (string.IsNullOrWhiteSpace(provinceName)) return null;
			var pattern = "^" + Regex.Escape(provinceName.Trim()) + "$"; // exact match
			var filter = Builders<ProvinceModel>.Filter.Regex(u => u.ProvinceName, new BsonRegularExpression(pattern, "i"));
			return await _provincees.Find(filter).FirstOrDefaultAsync();
		}

		/// <summary>
		/// Adds a new province to the database.
		/// </summary>
		/// <returns>The newly generated Id of the province.</returns>
		public async Task<string> AddNewProvinceAsync(ProvinceModel province)
		{
			try
			{
				province.CreatedAt = DateTime.UtcNow;
				await _provincees.InsertOneAsync(province);
				return province._id;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Updates an existing province document.
		/// </summary>
		public async Task<bool> UpdateProvinceAsync(ProvinceModel province)
		{
			try
			{
				var result = await _provincees.ReplaceOneAsync(u => u._id == province._id, province);
				return result.IsAcknowledged && result.ModifiedCount > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<bool> DeleteProvinceAsync(string id)
		{
			try
			{
				var result = await _provincees.DeleteOneAsync(u => u._id == id);
				return result.IsAcknowledged && result.DeletedCount > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Check if there's records for province document.
		/// </summary>
		public async Task<long> GetCountAsync()
		{
			return await _provincees.CountDocumentsAsync(new BsonDocument());
		}

		#region Implementation of IDisposable
		public void Dispose()
		{
		}
		#endregion
	}
}
