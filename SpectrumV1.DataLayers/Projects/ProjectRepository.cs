using MongoDB.Bson;
using MongoDB.Driver;
using SpectrumV1.DataLayers.DataAccess;
using SpectrumV1.Models.Projects;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.Projects
{
	public class ProjectRepository : IProjectRepository, IDisposable
	{
		private readonly IMongoCollection<ProjectModel> _projects;
		private const string CollectionName = "Projects";

		public ProjectRepository(string profileName)
		{
			var database = DatabaseFactory.GetMongoDatabase(profileName);
			_projects = database.GetCollection<ProjectModel>(CollectionName);
		}

		public async Task<List<ProjectModel>> GetProjectsAsync()
		{
			return await _projects.Find(project => true).ToListAsync();
		}

		public async Task<ProjectModel> GetProjectByIdAsync(string id)
		{
			var filter = Builders<ProjectModel>.Filter.Eq(u => u._id, id);
			return await _projects.Find(filter).FirstOrDefaultAsync();
		}

		public async Task<List<ProjectModel>> GetProjectsByClientIdAsync(string clientId)
		{
			if (string.IsNullOrWhiteSpace(clientId)) return new List<ProjectModel>();
			if (!ObjectId.TryParse(clientId, out var objectId)) return new List<ProjectModel>();
			var filter = Builders<ProjectModel>.Filter.Eq(u => u.ClientId, objectId);
			return await _projects.Find(filter).ToListAsync();
		}

		public async Task<List<ProjectModel>> GetProjectsByClientNameAsync(string clientName)
		{
			if (string.IsNullOrWhiteSpace(clientName)) return new List<ProjectModel>();
			var pattern = "^" + Regex.Escape(clientName.Trim()) + "$";
			var filter = Builders<ProjectModel>.Filter.Regex(u => u.ClientName, new BsonRegularExpression(pattern, "i"));
			return await _projects.Find(filter).ToListAsync();
		}

		public async Task<ProjectModel> GetProjectByNameAsync(string projectName)
		{
			if (string.IsNullOrWhiteSpace(projectName)) return null;
			var pattern = "^" + Regex.Escape(projectName.Trim()) + "$";
			var filter = Builders<ProjectModel>.Filter.Regex(u => u.ProjectName, new BsonRegularExpression(pattern, "i"));
			return await _projects.Find(filter).FirstOrDefaultAsync();
		}

		public async Task<string> AddNewProjectAsync(ProjectModel project)
		{
			try
			{
				project.CreatedAt = DateTime.UtcNow;
				await _projects.InsertOneAsync(project);
				return project._id;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<bool> UpdateProjectAsync(ProjectModel project)
		{
			try
			{
				var result = await _projects.ReplaceOneAsync(u => u._id == project._id, project);
				return result.IsAcknowledged && result.ModifiedCount > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<bool> DeleteProjectAsync(string id)
		{
			try
			{
				var result = await _projects.DeleteOneAsync(u => u._id == id);
				return result.IsAcknowledged && result.DeletedCount > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<long> GetCountAsync()
		{
			return await _projects.CountDocumentsAsync(new BsonDocument());
		}

		public void Dispose()
		{
		}
	}
}
