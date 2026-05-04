using MongoDB.Bson;
using MongoDB.Driver;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Common.Services;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.Common.Services
{
	public class ServiceTypeRepository : IServiceTypeRepository, IDisposable
	{
		private readonly IMongoCollection<ServiceTypeModel> _serviceTypes;
		private const string CollectionName = "ServiceTypes";

		// Constructor for dependency injection
		public ServiceTypeRepository(string profileName)
		{
			var database = DatabaseFactory.GetMongoDatabase(profileName);
			_serviceTypes = database.GetCollection<ServiceTypeModel>(CollectionName);
		}

		// Interface async implementations (wrapping legacy sync methods)
		public async Task<List<ServiceTypeModel>> GetServiceTypesAsync()
		{
			return await _serviceTypes.Find(serviceType => true).ToListAsync();
		}

		public async Task<ServiceTypeModel> GetServiceTypeByIdAsync(string id)
		{
			var filter = Builders<ServiceTypeModel>.Filter.Eq(u => u._id, id);
			return await _serviceTypes.Find(filter).FirstOrDefaultAsync();
		}

		public async Task<ServiceTypeModel> GetServiceTypeByName(string serviceType)
		{
			if (string.IsNullOrWhiteSpace(serviceType)) return null;
			var pattern = "^" + Regex.Escape(serviceType.Trim()) + "$"; // exact match
			var filter = Builders<ServiceTypeModel>.Filter.Regex(u => u.ServiceType, new BsonRegularExpression(pattern, "i"));
			return await _serviceTypes.Find(filter).FirstOrDefaultAsync();
		}

		/// <summary>
		/// Adds a new service to the database.
		/// </summary>
		/// <returns>The newly generated Id of the service.</returns>
		public async Task<string> AddNewServiceTypeAsync(ServiceTypeModel serviceType)
		{
			try
			{
				serviceType.CreatedAt = DateTime.UtcNow;
				await _serviceTypes.InsertOneAsync(serviceType);
				return serviceType._id;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Updates an existing service document.
		/// </summary>
		public async Task<bool> UpdateServiceTypeAsync(ServiceTypeModel serviceType)
		{
			try
			{
				var result = await _serviceTypes.ReplaceOneAsync(u => u._id == serviceType._id, serviceType);
				return result.IsAcknowledged && result.ModifiedCount > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<bool> DeleteServiceTypeAsync(string id)
		{
			try
			{
				var result = await _serviceTypes.DeleteOneAsync(u => u._id == id);
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
			return await _serviceTypes.CountDocumentsAsync(new BsonDocument());
		}

		#region Implementation of IDisposable
		public void Dispose()
		{
		}
		#endregion
	}
}
