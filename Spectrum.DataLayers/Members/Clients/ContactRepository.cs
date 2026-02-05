using MongoDB.Bson;
using MongoDB.Driver;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Members.Clients;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.Members.Clients
{
	public class ContactRepository : IContactRepository, IDisposable
	{
		private readonly IMongoCollection<ContactModel> _contacts;
		private const string CollectionName = "Contacts";

		public ContactRepository(string profileName)
		{
			var database = DatabaseFactory.GetMongoDatabase(profileName);
			_contacts = database.GetCollection<ContactModel>(CollectionName);
		}

		public async Task<List<ContactModel>> GetContactsAsync()
		{
			return await _contacts.Find(contact => true).ToListAsync();
		}

		public async Task<ContactModel> GetContactByIdAsync(string id)
		{
			var filter = Builders<ContactModel>.Filter.Eq(c => c._id, id);
			return await _contacts.Find(filter).FirstOrDefaultAsync();
		}

		public async Task<List<ContactModel>> GetContactsByClientIdAsync(string clientId)
		{
			var filter = Builders<ContactModel>.Filter.Eq(c => c.ClientId, clientId);
			return await _contacts.Find(filter).ToListAsync();
		}

		/// <summary>
		/// Adds a new contact to the database.
		/// </summary>
		/// <returns>The newly generated Id of the contact.</returns>
		public async Task<string> AddNewContactAsync(ContactModel contact)
		{
			try
			{
				contact.CreatedAt = DateTime.UtcNow;
				await _contacts.InsertOneAsync(contact);
				return contact._id;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Updates an existing contact document.
		/// </summary>
		public async Task<bool> UpdateContactAsync(ContactModel contact)
		{
			try
			{
				var result = await _contacts.ReplaceOneAsync(c => c._id == contact._id, contact);
				return result.IsAcknowledged && result.ModifiedCount > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<bool> DeleteContactAsync(string id)
		{
			try
			{
				var result = await _contacts.DeleteOneAsync(c => c._id == id);
				return result.IsAcknowledged && result.DeletedCount > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Check if there's records for contact document.
		/// </summary>
		public async Task<long> GetCountAsync()
		{
			return await _contacts.CountDocumentsAsync(new BsonDocument());
		}

		#region Implementation of IDisposable
		public void Dispose()
		{
		}
		#endregion
	}
}
