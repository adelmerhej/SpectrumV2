using MongoDB.Bson;
using MongoDB.Driver;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.HumanResources.Candidates;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.HumanResources.Candidates
{
    public class CandidateRepository : ICandidateRepository, IDisposable
    {
        private readonly IMongoCollection<CandidateModel> _candidate;
        private const string CollectionName = "Candidates";


        // Constructor for dependency injection
        public CandidateRepository(string profileName)
        {
            var database = DatabaseFactory.GetMongoDatabase(profileName);
            _candidate = database.GetCollection<CandidateModel>(CollectionName);
        }

        // Interface async implementations (wrapping legacy sync methods)
        public async Task<List<CandidateModel>> GetCandidatesAsync()
        {
            return await _candidate.Find(employee => true).ToListAsync();
        }

        public async Task<CandidateModel> GetCandidateByIdAsync(string id)
        {
            var filter = Builders<CandidateModel>.Filter.Eq(u => u._id, id);
            return await _candidate.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<CandidateModel> GetCandidateByName(string bloodTypeName)
        {
            if (string.IsNullOrWhiteSpace(bloodTypeName)) return null;
            var pattern = "^" + Regex.Escape(bloodTypeName.Trim()) + "$"; // exact match
            var filter = Builders<CandidateModel>.Filter.Regex(u => u.FirstName, new BsonRegularExpression(pattern, "i"));
            return await _candidate.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Adds a new employee to the database.
        /// </summary>
        /// <returns>The newly generated Id of the employee.</returns>
        public async Task<string> AddNewCandidateAsync(CandidateModel employee)
        {
            try
            {
                employee.CreatedAt = DateTime.UtcNow;
                await _candidate.InsertOneAsync(employee);
                return employee._id;
            }
            catch (Exception)
            {
                // preserve original stack trace
                throw;
            }
        }

        /// <summary>
        /// Updates an existing employee document.
        /// </summary>
        public async Task<bool> UpdateCandidateAsync(CandidateModel employee)
        {
            try
            {
                var result = await _candidate.ReplaceOneAsync(u => u._id == employee._id, employee);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteCandidateAsync(string id)
        {
            try
            {
                var result = await _candidate.DeleteOneAsync(u => u._id == id);
                return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Check if there's records for employee document.
        /// </summary>
        public async Task<long> GetCountAsync()
        {
            return await _candidate.CountDocumentsAsync(new BsonDocument());
        }

        #region Implementation of IDisposable
        public void Dispose()
        {
            // No unmanaged resources to dispose. If DatabaseFactory or IMongoClient requires disposal,
            // handle it here or inject a disposable client to this repository.
        }
        #endregion
    }
}
