using MongoDB.Bson;
using MongoDB.Driver;
using SpectrumV1.DataLayers.DataUtilities;
using SpectrumV1.Models.Administration.Connections;
using SpectrumV1.Utilities.Common;
using System;
using System.Net; // added for TLS settings

namespace SpectrumV1.DataLayers.DataAccess
{
	public class DatabaseFactory
	{
		// Constants for your standard profile names
		public const string ProfilePrimary = "Primary";
		public const string ProfileSecondary = "Secondary";

		/// <summary>
		/// Saves a connection configuration securely to the user's local profile.
		/// </summary>
		/// <param name="model">The UI model containing plain text data.</param>
		/// <param name="profileName">The key (e.g., "Primary", "Secondary").</param>
		public static void SaveConnection(ConnectionModel model, string profileName)
		{
			var config = ConfigManager.Load(profileName);

			var profile = new ConnectionModel
			{
				DatabaseType = model.DatabaseType,
				DatabaseHost = model.DatabaseHost,
				DatabasePort = model.DatabasePort,
				DatabaseName = model.DatabaseName,
				DatabaseUser = model.DatabaseUser,
				// ENCRYPT: Convert plain text to secure blob
				EncryptedPassword = SystemUtilities.Protect(model.DatabasePassword),
				EncryptedConnectionString = SystemUtilities.Protect(model.DatabaseConnectionString)
			};

			if (config.Connections.ContainsKey(profileName))
				config.Connections[profileName] = profile;
			else
				config.Connections.Add(profileName, profile);

			ConfigManager.Save(config, profileName);
		}

		/// <summary>
		/// Retrieves the decrypted connection model for a specific profile.
		/// </summary>
		public static ConnectionModel GetConnection(string profileName)
		{
			var config = ConfigManager.Load(profileName);

			if (!config.Connections.ContainsKey(profileName))
				return new ConnectionModel { Name = profileName }; // Return empty container

			var profile = config.Connections[profileName];

			var model = new ConnectionModel
			{
				Name = profileName,
				DatabaseType = profile.DatabaseType,
				DatabaseHost = profile.DatabaseHost,
				DatabasePort = profile.DatabasePort,
				DatabaseName = profile.DatabaseName,
				DatabaseUser = profile.DatabaseUser,
				// DECRYPT: Restore plain text for usage
				DatabasePassword = SystemUtilities.Unprotect(profile.EncryptedPassword),
				DatabaseConnectionString = SystemUtilities.Unprotect(profile.EncryptedConnectionString)
			};

			// Auto-generate MongoDB connection string if missing but parameters exist
			if (string.IsNullOrWhiteSpace(model.DatabaseConnectionString) &&
				(model.DatabaseType?.IndexOf("Mongo", StringComparison.OrdinalIgnoreCase) >= 0))
			{
				model.DatabaseConnectionString = BuildMongoConnectionString(model);
			}

			return model;
		}

		/// <summary>
		/// Gets a usable MongoDB Database object for the requested profile.
		/// </summary>
		public static IMongoDatabase GetMongoDatabase(string profileName)
		{
			var conn = GetConnection(profileName);
			if (string.IsNullOrWhiteSpace(conn.DatabaseConnectionString)) return null;

			try
			{
				// Ensure TLS 1.2+ for modern Atlas/Cloud connections
				ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

				var url = new MongoUrl(conn.DatabaseConnectionString);
				var client = new MongoClient(url);

				// Priority: Explicit DB name -> Connection String DB name -> "admin"
				var dbName = !string.IsNullOrWhiteSpace(conn.DatabaseName)
					? conn.DatabaseName
					: (url.DatabaseName ?? "admin");

				return client.GetDatabase(dbName);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Failed to create Mongo Client for {profileName}: {ex.Message}");
				return null;
			}
		}

		/// <summary>
		/// Helper to construct standard MongoDB connection strings.
		/// </summary>
		private static string BuildMongoConnectionString(ConnectionModel model)
		{
			var host = string.IsNullOrWhiteSpace(model.DatabaseHost) ? "localhost" : model.DatabaseHost.Trim();
			var port = model.DatabasePort > 0 ? model.DatabasePort : 27017;

			string userInfo = string.Empty;
			if (!string.IsNullOrWhiteSpace(model.DatabaseUser) && !string.IsNullOrWhiteSpace(model.DatabasePassword))
			{
				userInfo = $"{Uri.EscapeDataString(model.DatabaseUser)}:{Uri.EscapeDataString(model.DatabasePassword)}@";
			}

			return $"mongodb://{userInfo}{host}:{port}/{model.DatabaseName}?authSource=admin";
		}

		/// <summary>
		/// Tests connectivity. Works for both manual params (from UI) or stored params (from Disk).
		/// </summary>
		public static bool TestConnection(ConnectionModel model = null, string profileName = null)
		{
			// If model provided (e.g. from Settings UI), use it. Otherwise load from disk.
			var connToCheck = model ?? GetConnection(profileName);

			if (string.IsNullOrWhiteSpace(connToCheck.DatabaseConnectionString) &&
				string.IsNullOrWhiteSpace(connToCheck.DatabaseHost))
				return false;

			// If connection string is empty, try to build it temporarily for the test
			string connString = connToCheck.DatabaseConnectionString;
			if (string.IsNullOrWhiteSpace(connString))
				connString = BuildMongoConnectionString(connToCheck);

			try
			{
				ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
				var client = new MongoClient(connString);
				// Ping command is lightweight
				client.GetDatabase("admin").RunCommand((Command<BsonDocument>)"{ping:1}");
				return true;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Connection Test Failed: {ex.Message}");
				return false;
			}
		}
	}
}
