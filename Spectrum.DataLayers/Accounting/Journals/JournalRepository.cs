using MongoDB.Bson;
using MongoDB.Driver;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Accounting.Journals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.Accounting.Journals
{
    public class JournalRepository : IJournalRepository, IDisposable
    {
        private readonly IMongoCollection<JournalModel> _journals;
        private const string CollectionName = "Journals";

        // Constructor for dependency injection
        public JournalRepository(string profileName)
        {
            var database = DatabaseFactory.GetMongoDatabase(profileName);
            _journals = database.GetCollection<JournalModel>(CollectionName);
        }

        // Interface async implementations (wrapping legacy sync methods)
        public async Task<List<JournalModel>> GetJournalsAsync()
        {
            return await _journals.Find(journal => true).ToListAsync();
        }
        public async Task<List<JournalModel>> GetJournalsAsync(int workingYear)
        {
            var filter = Builders<JournalModel>.Filter.Eq(journal => journal.WorkingYear, workingYear);
            return await _journals.Find(filter).ToListAsync();
        }

        public async Task<JournalModel> GetJournalByIdAsync(string id)
        {
            var filter = Builders<JournalModel>.Filter.Eq(u => u._id, id);
            return await _journals.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<JournalModel> GetJournalByJvNo(string jvNo)
        {
            if (string.IsNullOrWhiteSpace(jvNo)) return null;
            var pattern = "^" + Regex.Escape(jvNo.Trim()) + "$"; // exact match
            var filter = Builders<JournalModel>.Filter.Regex(u => u.JvNo, new BsonRegularExpression(pattern, "i"));
            return await _journals.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<string> GetNextJvNoAsync(int workingYear)
        {
            var filter = Builders<JournalModel>.Filter.Eq(journal => journal.WorkingYear, workingYear);
            var journals = await _journals.Find(filter)
                .Project(journal => journal.JvNo)
                .ToListAsync();

            var nextJvNo = journals
                .Select(ParseJvNo)
                .DefaultIfEmpty(0)
                .Max() + 1;

            return nextJvNo.ToString();
        }

        public async Task<string> GetNextReferenceAsync()
        {
            var references = await _journals.Find(journal => true)
                .Project(journal => journal.Reference)
                .ToListAsync();

            var nextReference = references
                .Select(ParseReference)
                .DefaultIfEmpty(0)
                .Max() + 1;

            return nextReference.ToString().PadLeft(6, '0');
        }

        /// <summary>
        /// Adds a new journal to the database.
        /// </summary>
        /// <returns>The newly generated Id of the journal.</returns>
        public async Task<string> AddNewJournalAsync(JournalModel journal)
        {
            try
            {
                journal.CreatedAt = DateTime.UtcNow;
                await _journals.InsertOneAsync(journal);
                return journal._id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing journal document.
        /// </summary>
        public async Task<bool> UpdateJournalAsync(JournalModel journal)
        {
            try
            {
                var result = await _journals.ReplaceOneAsync(u => u._id == journal._id, journal);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteJournalAsync(string id)
        {
            try
            {
                var result = await _journals.DeleteOneAsync(u => u._id == id);
                return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Check if there's records for journal document.
        /// </summary>
        public async Task<long> GetCountAsync()
        {
            return await _journals.CountDocumentsAsync(new BsonDocument());
        }

        private static int ParseJvNo(string jvNo)
        {
            return int.TryParse(jvNo, out var parsedJvNo) ? parsedJvNo : 0;
        }

        private static int ParseReference(string reference)
        {
            return int.TryParse(reference, out var parsedReference) ? parsedReference : 0;
        }

        #region Implementation of IDisposable
        public void Dispose()
        {
        }
        #endregion
    }
}
