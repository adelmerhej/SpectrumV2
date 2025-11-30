using MongoDB.Bson;
using MongoDB.Driver;
using SpectrumV1.DataLayers.DataAccess;
using SpectrumV1.Models.Common.Services;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.Common.Services
{
	public class ServiceRepository : IServiceRepository, IDisposable
	{
		private readonly IMongoCollection<ServiceModel> _services;
		private const string CollectionName = "Services";

		// Constructor for dependency injection
		public ServiceRepository(string profileName)
		{
			var database = DatabaseFactory.GetMongoDatabase(profileName);
			_services = database.GetCollection<ServiceModel>(CollectionName);
		}

		// Interface async implementations (wrapping legacy sync methods)
		public async Task<List<ServiceModel>> GetServicesAsync()
		{
			return await _services.Find(service => true).ToListAsync();
		}

		public async Task<ServiceModel> GetServiceByIdAsync(string id)
		{
			var filter = Builders<ServiceModel>.Filter.Eq(u => u._id, id);
			return await _services.Find(filter).FirstOrDefaultAsync();
		}

		public async Task<ServiceModel> GetServiceByName(string serviceName)
		{
			if (string.IsNullOrWhiteSpace(serviceName)) return null;
			var pattern = "^" + Regex.Escape(serviceName.Trim()) + "$"; // exact match
			var filter = Builders<ServiceModel>.Filter.Regex(u => u.ServiceName, new BsonRegularExpression(pattern, "i"));
			return await _services.Find(filter).FirstOrDefaultAsync();
		}

		/// <summary>
		/// Adds a new service to the database.
		/// </summary>
		/// <returns>The newly generated Id of the service.</returns>
		public async Task<string> AddNewServiceAsync(ServiceModel service)
		{
			try
			{
				service.CreatedAt = DateTime.UtcNow;
				await _services.InsertOneAsync(service);
				return service._id;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Updates an existing service document.
		/// </summary>
		public async Task<bool> UpdateServiceAsync(ServiceModel service)
		{
			try
			{
				var result = await _services.ReplaceOneAsync(u => u._id == service._id, service);
				return result.IsAcknowledged && result.ModifiedCount > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<bool> DeleteServiceAsync(string id)
		{
			try
			{
				var result = await _services.DeleteOneAsync(u => u._id == id);
				return result.IsAcknowledged && result.DeletedCount > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Check if there's records for service document.
		/// </summary>
		public async Task<long> GetCountAsync()
		{
			return await _services.CountDocumentsAsync(new BsonDocument());
		}

		#region Implementation of IDisposable
		public void Dispose()
		{
		}
		#endregion
	}
}
