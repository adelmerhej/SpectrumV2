using MongoDB.Bson;
using MongoDB.Driver;
using SpectrumV1.DataLayers.DataAccess;
using SpectrumV1.Models.HumanResources.JobPositions;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.HumanResources.JobPositions
{
	public class JobPositionRepository : IJobPositionRepository, IDisposable
	{
		private readonly IMongoCollection<JobPositionModel> _jobPositions;
		private const string CollectionName = "JobPositions";

		// Constructor for dependency injection
		public JobPositionRepository(string profileName)
		{
			var database = DatabaseFactory.GetMongoDatabase(profileName);
			_jobPositions = database.GetCollection<JobPositionModel>(CollectionName);
		}

		// Interface async implementations (wrapping legacy sync methods)
		public async Task<List<JobPositionModel>> GetJobPositionsAsync()
		{
			return await _jobPositions.Find(jobPosition => true).ToListAsync();
		}

		public async Task<JobPositionModel> GetJobPositionByIdAsync(string id)
		{
			var filter = Builders<JobPositionModel>.Filter.Eq(u => u._id, id);
			return await _jobPositions.Find(filter).FirstOrDefaultAsync();
		}

		public async Task<JobPositionModel> GetJobPositionByName(string jobPositionName)
		{
			if (string.IsNullOrWhiteSpace(jobPositionName)) return null;
			var pattern = "^" + Regex.Escape(jobPositionName.Trim()) + "$"; // exact match
			var filter = Builders<JobPositionModel>.Filter.Regex(u => u.PositionName, new BsonRegularExpression(pattern, "i"));
			return await _jobPositions.Find(filter).FirstOrDefaultAsync();
		}

		/// <summary>
		/// Adds a new jobPosition to the database.
		/// </summary>
		/// <returns>The newly generated Id of the jobPosition.</returns>
		public async Task<string> AddNewJobPositionAsync(JobPositionModel jobPosition)
		{
			try
			{
				jobPosition.CreatedAt = DateTime.UtcNow;
				await _jobPositions.InsertOneAsync(jobPosition);
				return jobPosition._id;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Updates an existing jobPosition document.
		/// </summary>
		public async Task<bool> UpdateJobPositionAsync(JobPositionModel jobPosition)
		{
			try
			{
				var result = await _jobPositions.ReplaceOneAsync(u => u._id == jobPosition._id, jobPosition);
				return result.IsAcknowledged && result.ModifiedCount > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<bool> DeleteJobPositionAsync(string id)
		{
			try
			{
				var result = await _jobPositions.DeleteOneAsync(u => u._id == id);
				return result.IsAcknowledged && result.DeletedCount > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Check if there's records for jobPosition document.
		/// </summary>
		public async Task<long> GetCountAsync()
		{
			return await _jobPositions.CountDocumentsAsync(new BsonDocument());
		}

		#region Implementation of IDisposable
		public void Dispose()
		{
		}
		#endregion
	}
}
