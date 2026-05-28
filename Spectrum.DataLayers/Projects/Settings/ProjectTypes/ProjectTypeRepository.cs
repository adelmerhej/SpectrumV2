using MongoDB.Bson;
using MongoDB.Driver;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Operations.Projects.Settings.ProjectTypes;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.Projects.Settings.ProjectTypes
{
    public class ProjectTypeRepository : IProjectTypeRepository, IDisposable
    {
        private const string CollectionName = "ProjectTypes";
        private readonly IMongoCollection<ProjectTypeModel> _projectTypes;

        // Constructor for dependency injection
        public ProjectTypeRepository(string profileName)
        {
            var database = DatabaseFactory.GetMongoDatabase(profileName);
            _projectTypes = database.GetCollection<ProjectTypeModel>(CollectionName);
        }
        public IClientSessionHandle StartSession()
        {
            var client = _projectTypes.Database.Client;
            return client.StartSession();
        }

        public async Task<List<ProjectTypeModel>> GetProjectTypesAsync()
        {
            return await _projectTypes.Find(project => true).ToListAsync();
        }

        public async Task<ProjectTypeModel> GetProjectTypeByIdAsync(string id)
        {
            var filter = Builders<ProjectTypeModel>.Filter.Eq(u => u._id, id);
            return await _projectTypes.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<ProjectTypeModel> GetDefaultProjectTypeAsync(string excludedId = null)
        {
            var filter = Builders<ProjectTypeModel>.Filter.Eq(u => u.IsDefault, true);

            if (!string.IsNullOrWhiteSpace(excludedId))
            {
                filter &= Builders<ProjectTypeModel>.Filter.Ne(u => u._id, excludedId);
            }

            return await _projectTypes.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<ProjectTypeModel> GetProjectTypeBySectorAsync(string projectTypeSector)
        {
            if (string.IsNullOrWhiteSpace(projectTypeSector)) return null;
            var pattern = "^" + Regex.Escape(projectTypeSector.Trim()) + "$"; // exact match
            var filter = Builders<ProjectTypeModel>.Filter.Regex(u => u.Sector, new BsonRegularExpression(pattern, "i"));
            return await _projectTypes.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Adds a new projectType to the database.
        /// </summary>
        /// <returns>The newly generated Id of the projectType.</returns>
        public async Task<string> AddNewProjectTypeAsync(ProjectTypeModel projectType)
        {
            try
            {
                projectType.CreatedAt = DateTime.UtcNow;
                await _projectTypes.InsertOneAsync(projectType);
                return projectType._id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing projectType document.
        /// </summary>
        public async Task<bool> UpdateProjectTypeAsync(ProjectTypeModel projectType)
        {
            try
            {
                var result = await _projectTypes.ReplaceOneAsync(u => u._id == projectType._id, projectType);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteProjectTypeAsync(string id)
        {
            try
            {
                var result = await _projectTypes.DeleteOneAsync(u => u._id == id);
                return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Check if there's records for projectType document.
        /// </summary>
        public async Task<long> GetCountAsync()
        {
            return await _projectTypes.CountDocumentsAsync(new BsonDocument());
        }

        #region Implementation of IDisposable
        public void Dispose()
        {
        }
        #endregion
    }
}
