using MongoDB.Bson;
using MongoDB.Driver;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Accounting.Charts;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.Accounting.Charts
{
	public class ChartDetailRepository : IChartDetailRepository, IDisposable
	{
		private readonly IMongoCollection<ChartDetailModel> _chartDetails;
		private const string CollectionName = "ChartDetails";

		// Constructor for dependency injection
		public ChartDetailRepository(string profileName)
		{
			var database = DatabaseFactory.GetMongoDatabase(profileName);
			_chartDetails = database.GetCollection<ChartDetailModel>(CollectionName);
		}

		// Interface async implementations (wrapping legacy sync methods)
		public async Task<List<ChartDetailModel>> GetChartDetailsAsync()
		{
			return await _chartDetails.Find(chartDetail => true).ToListAsync();
		}

		public async Task<ChartDetailModel> GetChartDetailByIdAsync(string id)
		{
			var filter = Builders<ChartDetailModel>.Filter.Eq(u => u._id, id);
			return await _chartDetails.Find(filter).FirstOrDefaultAsync();
		}

		public async Task<ChartDetailModel> GetChartDetailByName(string chartDetailName)
		{
			if (string.IsNullOrWhiteSpace(chartDetailName)) return null;
			var pattern = "^" + Regex.Escape(chartDetailName.Trim()) + "$"; // exact match
			var filter = Builders<ChartDetailModel>.Filter.Regex(u => u.AccountName, new BsonRegularExpression(pattern, "i"));
			return await _chartDetails.Find(filter).FirstOrDefaultAsync();
		}

		public async Task<ChartDetailModel> GetChartDetailByAccountNumber(string chartDetailName)
		{
			if (string.IsNullOrWhiteSpace(chartDetailName)) return null;
			var pattern = "^" + Regex.Escape(chartDetailName.Trim()) + "$"; // exact match
			var filter = Builders<ChartDetailModel>.Filter.Regex(u => u.AccountNumber, new BsonRegularExpression(pattern, "i"));
			return await _chartDetails.Find(filter).FirstOrDefaultAsync();
		}

		/// <summary>
		/// Adds a new chartDetail to the database.
		/// </summary>
		/// <returns>The newly generated Id of the chartDetail.</returns>
		public async Task<string> AddNewChartDetailAsync(ChartDetailModel chartDetail)
		{
			try
			{
				chartDetail.CreatedAt = DateTime.UtcNow;
				await _chartDetails.InsertOneAsync(chartDetail);
				return chartDetail._id;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Updates an existing chartDetail document.
		/// </summary>
		public async Task<bool> UpdateChartDetailAsync(ChartDetailModel chartDetail)
		{
			try
			{
				var result = await _chartDetails.ReplaceOneAsync(u => u._id == chartDetail._id, chartDetail);
				return result.IsAcknowledged && result.ModifiedCount > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<bool> DeleteChartDetailAsync(string id)
		{
			try
			{
				var result = await _chartDetails.DeleteOneAsync(u => u._id == id);
				return result.IsAcknowledged && result.DeletedCount > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Check if there's records for chartDetail document.
		/// </summary>
		public async Task<long> GetCountAsync()
		{
			return await _chartDetails.CountDocumentsAsync(new BsonDocument());
		}

		#region Implementation of IDisposable
		public void Dispose()
		{
		}
		#endregion
	}
}
