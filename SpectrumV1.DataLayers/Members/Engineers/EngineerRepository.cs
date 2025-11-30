using MongoDB.Bson;
using MongoDB.Driver;
using SpectrumV1.DataLayers.DataAccess;
using SpectrumV1.Models.Members.Engineers;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.Members.Engineers
{
	public class EngineerRepository : IEngineerRepository, IDisposable
	{
		private readonly IMongoCollection<EngineerModel> _engineers;
		private const string CollectionName = "Engineers";

		// Constructor for dependency injection
		public EngineerRepository(string profileName)
		{
			var database = DatabaseFactory.GetMongoDatabase(profileName);
			_engineers = database.GetCollection<EngineerModel>(CollectionName);
		}

		// Interface async implementations (wrapping legacy sync methods)
		public async Task<List<EngineerModel>> GetEngineersAsync()
		{
			return await _engineers.Find(engineer => true).ToListAsync();
		}

		public async Task<EngineerModel> GetEngineerByIdAsync(string id)
		{
			var filter = Builders<EngineerModel>.Filter.Eq(u => u._id, id);
			return await _engineers.Find(filter).FirstOrDefaultAsync();
		}

		public async Task<EngineerModel> GetEngineerByName(string engineerName)
		{
			if (string.IsNullOrWhiteSpace(engineerName)) return null;
			var pattern = "^" + Regex.Escape(engineerName.Trim()) + "$"; // exact match
			var filter = Builders<EngineerModel>.Filter.Regex(u => u.EngineerName, new BsonRegularExpression(pattern, "i"));
			return await _engineers.Find(filter).FirstOrDefaultAsync();
		}

		/// <summary>
		/// Adds a new engineer to the database.
		/// </summary>
		/// <returns>The newly generated Id of the engineer.</returns>
		public async Task<string> AddNewEngineerAsync(EngineerModel engineer)
		{
			try
			{
				engineer.CreatedAt = DateTime.UtcNow;
				await _engineers.InsertOneAsync(engineer);
				return engineer._id;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Updates an existing engineer document.
		/// </summary>
		public async Task<bool> UpdateEngineerAsync(EngineerModel engineer)
		{
			try
			{
				var result = await _engineers.ReplaceOneAsync(u => u._id == engineer._id, engineer);
				return result.IsAcknowledged && result.ModifiedCount > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<bool> DeleteEngineerAsync(string id)
		{
			try
			{
				var result = await _engineers.DeleteOneAsync(u => u._id == id);
				return result.IsAcknowledged && result.DeletedCount > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Check if there's records for engineer document.
		/// </summary>
		public async Task<long> GetCountAsync()
		{
			return await _engineers.CountDocumentsAsync(new BsonDocument());
		}

		#region Implementation of IDisposable
		public void Dispose()
		{
		}
		#endregion
	}
}
