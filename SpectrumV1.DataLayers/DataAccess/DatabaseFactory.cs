using SpectrumV1.DataLayers.DataAccess.Types;
using SpectrumV1.DataLayers.Properties;
using SpectrumV1.Models.Administration.Connections;
using SpectrumV1.Utilities.Enums;
using System;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Net; // added for TLS settings

namespace SpectrumV1.DataLayers.DataAccess
{
	public class DatabaseFactory
	{
		public static IDatabase Get(string application, string databaseType, string databaseHost, int databasePort,
			string databaseName, string databaseUser, string databasePassword)
		{
			switch ((DatabaseTypes)Enum.Parse(typeof(DatabaseTypes), databaseType))
			{
				case DatabaseTypes.MySql:
					return null;

				case DatabaseTypes.MongoDb:
					return (IDatabase)new MongoDbDatabaseModel() { };

				case DatabaseTypes.SqlServer:
					return new SqlServerDatabaseModel()
					{
						Server = databaseHost,
						Database = databaseName,
						User = databaseUser,
						Password = databasePassword
					};

				default:
					return null;
			}
		}

		public static void ConnectionParamsSet(ConnectionModel connection)
		{
			Settings.Default.DatabaseType = connection.DatabaseType;
			Settings.Default.DatabaseHost = connection.DatabaseHost;
			Settings.Default.DatabasePort = connection.DatabasePort;
			Settings.Default.DatabaseName = connection.DatabaseName;
			Settings.Default.DatabaseUser = connection.DatabaseUser;
			Settings.Default.DatabasePassword = connection.DatabasePassword;
			Settings.Default.DatabaseConnectionString = connection.DatabaseConnectionString;
			Settings.Default.Save();
		}

		// New: set connection params for primary or backup
		public static void ConnectionParamsSet(ConnectionModel connection, bool isBackup)
		{
			if (!isBackup)
			{
				ConnectionParamsSet(connection);
				return;
			}

			Settings.Default.BackupDatabaseType = connection.DatabaseType;
			Settings.Default.BackupDatabaseHost = connection.DatabaseHost;
			Settings.Default.BackupDatabasePort = connection.DatabasePort;
			Settings.Default.BackupDatabaseName = connection.DatabaseName;
			Settings.Default.BackupDatabaseUser = connection.DatabaseUser;
			Settings.Default.BackupDatabasePassword = connection.DatabasePassword;
			Settings.Default.BackupDatabaseConnectionString = connection.DatabaseConnectionString;
			Settings.Default.Save();
		}

		public static ConnectionModel ConnectionParamsGet(bool secondConnection = false)
		{
			ConnectionModel connection = new ConnectionModel();

			if (!secondConnection)
			{
				connection.DatabaseType = Settings.Default.DatabaseType;
				connection.DatabaseHost = Settings.Default.DatabaseHost;
				connection.DatabasePort = Settings.Default.DatabasePort;
				connection.DatabaseName = Settings.Default.DatabaseName;
				connection.DatabaseUser = Settings.Default.DatabaseUser;
				connection.DatabasePassword = Settings.Default.DatabasePassword;
				connection.DatabaseConnectionString = Settings.Default.DatabaseConnectionString;
			}
			else
			{
				connection.DatabaseType = Settings.Default.BackupDatabaseType;
				connection.DatabaseHost = Settings.Default.BackupDatabaseHost;
				connection.DatabasePort = Settings.Default.BackupDatabasePort;
				connection.DatabaseName = Settings.Default.BackupDatabaseName;
				connection.DatabaseUser = Settings.Default.BackupDatabaseUser;
				connection.DatabasePassword = Settings.Default.BackupDatabasePassword;
				connection.DatabaseConnectionString = Settings.Default.BackupDatabaseConnectionString;
			}

			// If MongoDB and no explicit connection string, build one from parameters.
			if (string.IsNullOrWhiteSpace(connection.DatabaseConnectionString) &&
				!string.IsNullOrWhiteSpace(connection.DatabaseType) &&
				string.Equals(connection.DatabaseType, DatabaseTypes.MongoDb.ToString(), StringComparison.OrdinalIgnoreCase))
			{
				var host = string.IsNullOrWhiteSpace(connection.DatabaseHost) ? "localhost" : connection.DatabaseHost.Trim();
				var port = connection.DatabasePort > 0 ? connection.DatabasePort : 27017;
				string userInfo = null;
				if (!string.IsNullOrWhiteSpace(connection.DatabaseUser) && !string.IsNullOrWhiteSpace(connection.DatabasePassword))
					userInfo = string.Concat(Uri.EscapeDataString(connection.DatabaseUser), ":", Uri.EscapeDataString(connection.DatabasePassword), "@");
				string path = !string.IsNullOrWhiteSpace(connection.DatabaseName) ? "/" + connection.DatabaseName.Trim() : string.Empty;
				string query = userInfo != null ? "?authSource=admin" : string.Empty;
				connection.DatabaseConnectionString = $"mongodb://{(userInfo ?? string.Empty)}{host}:{port}{path}{query}";
			}
			return connection;
		}

		public static bool TestDatabaseConnection(string connectionString, string databaseName)
		{
			// Handles MongoDB SRV and standard connection strings. Uses provided databaseName override if given.
			if (string.IsNullOrWhiteSpace(connectionString)) return false;
			try
			{
				// Ensure TLS 1.2 (Atlas requires TLS; 4.7.2 usually OK but force if downgraded environment)
				ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

				var trimmed = connectionString.Trim();
				var url = new MongoUrl(trimmed);
				var client = new MongoClient(url);

				// Prefer explicit parameter, then URL database, fallback to "admin".
				var dbName = !string.IsNullOrWhiteSpace(databaseName) ? databaseName.Trim() : (string.IsNullOrWhiteSpace(url.DatabaseName) ? "admin" : url.DatabaseName);
				var db = client.GetDatabase(dbName);
				try
				{
					db.RunCommand<BsonDocument>(new BsonDocument("ping", 1));
					return true;
				}
				catch (MongoAuthenticationException authEx)
				{
					// Retry with admin if auth failed and we didn't already use admin.
					if (!string.Equals(dbName, "admin", StringComparison.OrdinalIgnoreCase))
					{
						var adminDb = client.GetDatabase("admin");
						adminDb.RunCommand<BsonDocument>(new BsonDocument("ping", 1));
						return true;
					}
					System.Diagnostics.Debug.WriteLine("MongoDB authentication failure: " + authEx.Message);
					return false;
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("MongoDB test connection failed: " + ex.Message + " | ConnString: " + connectionString);
				return false;
			}
		}

		// New: convenience method to test primary or backup connection stored in settings
		public static bool TestStoredConnection(bool isBackup)
		{
			var conn = ConnectionParamsGet(isBackup);
			return TestDatabaseConnection(conn.DatabaseConnectionString, conn.DatabaseName);
		}
	}
}
