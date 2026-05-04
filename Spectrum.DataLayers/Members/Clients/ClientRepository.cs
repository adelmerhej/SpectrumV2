using MongoDB.Bson;
using MongoDB.Driver;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Members.Clients;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.Members.Clients
{
	public class ClientRepository: IClientRepository, IDisposable
	{
		private readonly IMongoCollection<ClientModel> _clients;
		private const string CollectionName = "Clients";

		// Constructor for dependency injection
		public ClientRepository(string profileName)
		{
			var database = DatabaseFactory.GetMongoDatabase(profileName);
			_clients = database.GetCollection<ClientModel>(CollectionName);
		}

		// Interface async implementations (wrapping legacy sync methods)
		public async Task<List<ClientModel>> GetClientsAsync()
		{
			return await _clients.Find(client => true).ToListAsync();
		}

		public async Task<ClientModel> GetClientByIdAsync(string id)
		{
			var filter = Builders<ClientModel>.Filter.Eq(u => u._id, id);
			return await _clients.Find(filter).FirstOrDefaultAsync();
		}

		public async Task<ClientModel> GetClientByName(string clientName)
		{
			if (string.IsNullOrWhiteSpace(clientName)) return null;
			var pattern = "^" + Regex.Escape(clientName.Trim()) + "$"; // exact match
			var filter = Builders<ClientModel>.Filter.Regex(u => u.ClientName, new BsonRegularExpression(pattern, "i"));
			return await _clients.Find(filter).FirstOrDefaultAsync();
		}

		/// <summary>
		/// Adds a new client to the database.
		/// </summary>
		/// <returns>The newly generated Id of the client.</returns>
		public async Task<string> AddNewClientAsync(ClientModel client)
		{
			try
			{
				client.CreatedAt = DateTime.UtcNow;
				await _clients.InsertOneAsync(client);
				return client._id;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Updates an existing client document.
		/// </summary>
		public async Task<bool> UpdateClientAsync(ClientModel client)
		{
			try
			{
				var result = await _clients.ReplaceOneAsync(u => u._id == client._id, client);
				return result.IsAcknowledged && result.ModifiedCount > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<bool> DeleteClientAsync(string id)
		{
			try
			{
				var result = await _clients.DeleteOneAsync(u => u._id == id);
				return result.IsAcknowledged && result.DeletedCount > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Check if there's records for client document.
		/// </summary>
		public async Task<long> GetCountAsync()
		{
			return await _clients.CountDocumentsAsync(new BsonDocument());
		}

		#region Implementation of IDisposable
		public void Dispose()
		{
		}
		#endregion
	}
}
