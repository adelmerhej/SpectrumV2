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
	public class ChartRepository : IChartRepository, IDisposable
	{
		private readonly IMongoCollection<ChartModel> _charts;
		private const string CollectionName = "Charts";

		// Constructor for dependency injection
		public ChartRepository(string profileName)
		{
			var database = DatabaseFactory.GetMongoDatabase(profileName);
			_charts = database.GetCollection<ChartModel>(CollectionName);
		}

		// Interface async implementations (wrapping legacy sync methods)
		public async Task<List<ChartModel>> GetChartsAsync()
		{
			return await _charts.Find(chart => true).ToListAsync();
		}

		public async Task<ChartModel> GetChartByIdAsync(string id)
		{
			var filter = Builders<ChartModel>.Filter.Eq(u => u._id, id);
			return await _charts.Find(filter).FirstOrDefaultAsync();
		}

		public async Task<ChartModel> GetChartByName(string chartName)
		{
			if (string.IsNullOrWhiteSpace(chartName)) return null;
			var pattern = "^" + Regex.Escape(chartName.Trim()) + "$"; // exact match
			var filter = Builders<ChartModel>.Filter.Regex(u => u.AccountName, new BsonRegularExpression(pattern, "i"));
			return await _charts.Find(filter).FirstOrDefaultAsync();
		}

		public async Task<ChartModel> GetChartByNumber(string accountNumber)
		{
			if (string.IsNullOrWhiteSpace(accountNumber)) return null;
			var pattern = "^" + Regex.Escape(accountNumber.Trim()) + "$"; // exact match
			var filter = Builders<ChartModel>.Filter.Regex(u => u.AccountNumber, new BsonRegularExpression(pattern, "i"));
			return await _charts.Find(filter).FirstOrDefaultAsync();
		}

		/// <summary>
		/// Adds a new chart to the database.
		/// </summary>
		/// <returns>The newly generated Id of the chart.</returns>
		public async Task<string> AddNewChartAsync(ChartModel chart)
		{
			try
			{
				chart.CreatedAt = DateTime.UtcNow;
				await _charts.InsertOneAsync(chart);
				return chart._id;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Updates an existing chart document.
		/// </summary>
		public async Task<bool> UpdateChartAsync(ChartModel chart)
		{
			try
			{
				var result = await _charts.ReplaceOneAsync(u => u._id == chart._id, chart);
				return result.IsAcknowledged && result.ModifiedCount > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<bool> DeleteChartAsync(string id)
		{
			try
			{
				var result = await _charts.DeleteOneAsync(u => u._id == id);
				return result.IsAcknowledged && result.DeletedCount > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Check if there's records for chart document.
		/// </summary>
		public async Task<long> GetCountAsync()
		{
			return await _charts.CountDocumentsAsync(new BsonDocument());
		}

		#region Implementation of IDisposable
		public void Dispose()
		{
		}
		#endregion
	}
}
