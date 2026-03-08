using MongoDB.Bson;
using MongoDB.Driver;
using Spectrum.DataLayers.DataAccess;
using SpectrumV1.Models.Common.Roles;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.Common.Roles
{
    public class RoleRepository : IRoleRepository, IDisposable
    {
        private readonly IMongoCollection<RoleModel> _roles;
        private const string CollectionName = "Roles";

        // Constructor for dependency injection
        public RoleRepository(string profileName)
        {
            var database = DatabaseFactory.GetMongoDatabase(profileName);
            _roles = database.GetCollection<RoleModel>(CollectionName);
        }

        // Interface async implementations (wrapping legacy sync methods)
        public async Task<List<RoleModel>> GetRolesAsync()
        {
            return await _roles.Find(role => true).ToListAsync();
        }

        public async Task<RoleModel> GetRoleByIdAsync(string id)
        {
            var filter = Builders<RoleModel>.Filter.Eq(u => u._id, id);
            return await _roles.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<RoleModel> GetRoleByName(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName)) return null;
            var pattern = "^" + Regex.Escape(roleName.Trim()) + "$"; // exact match
            var filter = Builders<RoleModel>.Filter.Regex(u => u.RoleName, new BsonRegularExpression(pattern, "i"));
            return await _roles.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Adds a new role to the database.
        /// </summary>
        /// <returns>The newly generated Id of the role.</returns>
        public async Task<string> AddNewRoleAsync(RoleModel role)
        {
            try
            {
                role.CreatedAt = DateTime.UtcNow;
                await _roles.InsertOneAsync(role);
                return role._id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing role document.
        /// </summary>
        public async Task<bool> UpdateRoleAsync(RoleModel role)
        {
            try
            {
                var result = await _roles.ReplaceOneAsync(u => u._id == role._id, role);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteRoleAsync(string id)
        {
            try
            {
                var result = await _roles.DeleteOneAsync(u => u._id == id);
                return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Check if there's records for role document.
        /// </summary>
        public async Task<long> GetCountAsync()
        {
            return await _roles.CountDocumentsAsync(new BsonDocument());
        }

        #region Implementation of IDisposable
        public void Dispose()
        {
        }
        #endregion
    }
}
