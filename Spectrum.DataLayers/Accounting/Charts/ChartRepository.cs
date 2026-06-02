using MongoDB.Bson;
using MongoDB.Driver;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Accounting.Charts;
using Spectrum.Utilities.Enums;
using Spectrum.Models.Accounting.Banks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.Accounting.Charts
{
	public class ChartRepository : IChartRepository, IDisposable
	{
		private readonly IMongoCollection<ChartModel> _charts;
		private readonly IMongoCollection<BsonDocument> _journal;
		private const string CollectionName = "Charts";
		private const string JournalCollectionName = "Journal";

		// Constructor for dependency injection
		public ChartRepository(string profileName)
		{
			var database = DatabaseFactory.GetMongoDatabase(profileName);
			_charts = database.GetCollection<ChartModel>(CollectionName);
			_journal = database.GetCollection<BsonDocument>(JournalCollectionName);
		}

		// Interface async implementations (wrapping legacy sync methods)
		public async Task<List<ChartModel>> GetChartsAsync()
		{
			return await _charts.Find(chart => true).ToListAsync();
		}

        // Interface async implementations (wrapping legacy sync methods)
        public async Task<List<ChartModel>> GetChartsAsync(int workingYear)
        {
            var filter = Builders<ChartModel>.Filter.Eq(x => x.WorkingYear, workingYear);
            return await _charts.Find(filter).ToListAsync();
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

        public async Task<List<ChartModel>> GetChartByNumberAsync(string parentNumber, string company)
        {
            if (string.IsNullOrWhiteSpace(parentNumber) || string.IsNullOrWhiteSpace(company)) return new List<ChartModel>();
            var numberPattern = "^" + Regex.Escape(parentNumber.Trim()) + "$";
            var companyPattern = "^" + Regex.Escape(company.Trim()) + "$";
            var filter = Builders<ChartModel>.Filter.And(
                Builders<ChartModel>.Filter.Regex(x => x.Number, new BsonRegularExpression(numberPattern, "i")),
                Builders<ChartModel>.Filter.Regex(x => x.Company, new BsonRegularExpression(companyPattern, "i")));
            return await _charts.Find(filter).ToListAsync();
        }

        public async Task<ChartModel> GetDefaultChartAsync(string excludedId = null)
        {
            var filter = Builders<ChartModel>.Filter.Eq(u => u.IsDefault, true);

            if (!string.IsNullOrWhiteSpace(excludedId))
            {
                filter &= Builders<ChartModel>.Filter.Ne(u => u._id, excludedId);
            }

            return await _charts.Find(filter).FirstOrDefaultAsync();
        }

        public string GetNewSerial(string accountNumber, string company)
		{
			var accountType = AccountType.R.ToString();

			if (string.IsNullOrWhiteSpace(accountNumber) || string.IsNullOrWhiteSpace(company)) return "1";

			var numberPattern = "^" + Regex.Escape(accountNumber.Trim()) + "$";
			var companyPattern = "^" + Regex.Escape(company.Trim()) + "$";

			var filter = Builders<ChartModel>.Filter.And(
				Builders<ChartModel>.Filter.Regex(x => x.Number, new BsonRegularExpression(numberPattern, "i")),
				Builders<ChartModel>.Filter.Regex(x => x.AccountType, new BsonRegularExpression("^" + accountType + "$", "i")),
				Builders<ChartModel>.Filter.Regex(x => x.Company, new BsonRegularExpression(companyPattern, "i")));

			var serials = _charts
				.Find(filter)
				.Project(x => x.Serial)
				.ToList();

			var maxSerial = serials
				.Select(serial =>
				{
					int parsedSerial;
					return int.TryParse(serial, out parsedSerial) ? parsedSerial : 0;
				})
				.DefaultIfEmpty(0)
				.Max();

			return (maxSerial + 1).ToString();
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

		public bool ValidateAccountInJournal(string accountNumber)
		{
			if (string.IsNullOrWhiteSpace(accountNumber)) return false;

			var pattern = "^" + Regex.Escape(accountNumber.Trim()) + "$";
			var filter = Builders<BsonDocument>.Filter.Regex("AccountNumber", new BsonRegularExpression(pattern, "i"));

			return _journal.Find(filter).Limit(1).Any();
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
