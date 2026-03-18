using MongoDB.Bson;
using MongoDB.Driver;
using Spectrum.DataLayers.DataAccess;
using SpectrumV1.Models.Accounting.Journals;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.Accounting.Journals
{
    public class JournalDetailRepository : IJournalDetailRepository, IDisposable
    {
        private readonly IMongoCollection<JournalDetailModel> _journalDetails;
        private const string CollectionName = "JournalDetails";

        // Constructor for dependency injection
        public JournalDetailRepository(string profileName)
        {
            var database = DatabaseFactory.GetMongoDatabase(profileName);
            _journalDetails = database.GetCollection<JournalDetailModel>(CollectionName);
        }

        // Interface async implementations (wrapping legacy sync methods)
        public async Task<List<JournalDetailModel>> GetJournalDetailsAsync()
        {
            return await _journalDetails.Find(journalDetail => true).ToListAsync();
        }

        public async Task<JournalDetailModel> GetJournalDetailByIdAsync(string id)
        {
            var filter = Builders<JournalDetailModel>.Filter.Eq(u => u._id, id);
            return await _journalDetails.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<JournalDetailModel> GetJournalDetailByJvNo(string jvNo)
        {
            if (string.IsNullOrWhiteSpace(jvNo)) return null;
            var pattern = "^" + Regex.Escape(jvNo.Trim()) + "$"; // exact match
            var filter = Builders<JournalDetailModel>.Filter.Regex(u => u.JvNo, new BsonRegularExpression(pattern, "i"));
            return await _journalDetails.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Adds a new journalDetail to the database.
        /// </summary>
        /// <returns>The newly generated Id of the journalDetail.</returns>
        public async Task<string> AddNewJournalDetailAsync(JournalDetailModel journalDetail)
        {
            try
            {
                journalDetail.CreatedAt = DateTime.UtcNow;
                await _journalDetails.InsertOneAsync(journalDetail);
                return journalDetail._id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing journalDetail document.
        /// </summary>
        public async Task<bool> UpdateJournalDetailAsync(JournalDetailModel journalDetail)
        {
            try
            {
                var result = await _journalDetails.ReplaceOneAsync(u => u._id == journalDetail._id, journalDetail);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteJournalDetailAsync(string id)
        {
            try
            {
                var result = await _journalDetails.DeleteOneAsync(u => u._id == id);
                return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Check if there's records for journalDetail document.
        /// </summary>
        public async Task<long> GetCountAsync()
        {
            return await _journalDetails.CountDocumentsAsync(new BsonDocument());
        }

        #region Implementation of IDisposable
        public void Dispose()
        {
        }
        #endregion
    }
}
