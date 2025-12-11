using MongoDB.Bson;
using MongoDB.Driver;
using SpectrumV1.DataLayers.DataAccess;
using SpectrumV1.Models.Common.Departments;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.Common.Departments
{
	public class DepartmentRepository : IDepartmentRepository, IDisposable
	{
		private readonly IMongoCollection<DepartmentModel> _departments;
		private const string CollectionName = "Departments";

		// Constructor for dependency injection
		public DepartmentRepository(string profileName)
		{
			var database = DatabaseFactory.GetMongoDatabase(profileName);
			_departments = database.GetCollection<DepartmentModel>(CollectionName);
		}

		// Interface async implementations (wrapping legacy sync methods)
		public async Task<List<DepartmentModel>> GetDepartmentsAsync()
		{
			return await _departments.Find(department => true).ToListAsync();
		}

		public async Task<DepartmentModel> GetDepartmentByIdAsync(string id)
		{
			var filter = Builders<DepartmentModel>.Filter.Eq(u => u._id, id);
			return await _departments.Find(filter).FirstOrDefaultAsync();
		}

		public async Task<DepartmentModel> GetDepartmentByName(string departmentName)
		{
			if (string.IsNullOrWhiteSpace(departmentName)) return null;
			var pattern = "^" + Regex.Escape(departmentName.Trim()) + "$"; // exact match
			var filter = Builders<DepartmentModel>.Filter.Regex(u => u.DepartmentName, new BsonRegularExpression(pattern, "i"));
			return await _departments.Find(filter).FirstOrDefaultAsync();
		}

		/// <summary>
		/// Adds a new department to the database.
		/// </summary>
		/// <returns>The newly generated Id of the department.</returns>
		public async Task<string> AddNewDepartmentAsync(DepartmentModel department)
		{
			try
			{
				department.CreatedAt = DateTime.UtcNow;
				await _departments.InsertOneAsync(department);
				return department._id;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Updates an existing department document.
		/// </summary>
		public async Task<bool> UpdateDepartmentAsync(DepartmentModel department)
		{
			try
			{
				var result = await _departments.ReplaceOneAsync(u => u._id == department._id, department);
				return result.IsAcknowledged && result.ModifiedCount > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<bool> DeleteDepartmentAsync(string id)
		{
			try
			{
				var result = await _departments.DeleteOneAsync(u => u._id == id);
				return result.IsAcknowledged && result.DeletedCount > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Check if there's records for department document.
		/// </summary>
		public async Task<long> GetCountAsync()
		{
			return await _departments.CountDocumentsAsync(new BsonDocument());
		}

		#region Implementation of IDisposable
		public void Dispose()
		{
		}
		#endregion
	}
}
