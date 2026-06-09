using MongoDB.Bson;
using MongoDB.Driver;
using Spectrum.DataLayers.DataUtilities;
using Spectrum.Models.Administration.Connections;
using Spectrum.Utilities.Common;
using System;
using System.Net; // added for TLS settings

namespace Spectrum.DataLayers.DataAccess
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
			var aiSettings = model.AiSettings ?? new AiSettingsModel();
			var connectionString = NormalizeMongoConnectionString(model.DatabaseConnectionString);

			var profile = new ConnectionModel
			{
				DatabaseType = model.DatabaseType,
				DatabaseHost = model.DatabaseHost,
				DatabasePort = model.DatabasePort,
				DatabaseName = model.DatabaseName,
				DatabaseUser = model.DatabaseUser,
				// ENCRYPT: Convert plain text to secure blob
				EncryptedPassword = SystemUtilities.Protect(model.DatabasePassword),
				EncryptedConnectionString = SystemUtilities.Protect(connectionString),

				// Legacy flat values for backward compatibility
				AiModel = model.AiModel,
				EncryptedAiApikey = SystemUtilities.Protect(model.EncryptedAiApikey),

				// Phase 1 nested AI settings
				AiSettings = new AiSettingsModel
				{
					Provider = aiSettings.Provider,
					OpenAiModel = aiSettings.OpenAiModel,
					DeepSeekModel = aiSettings.DeepSeekModel,
					EncryptedOpenAiApiKey = SystemUtilities.Protect(aiSettings.EncryptedOpenAiApiKey),
					EncryptedDeepSeekApiKey = SystemUtilities.Protect(aiSettings.EncryptedDeepSeekApiKey)
				},

				ProjectsDocumentsFolder = model.ProjectsDocumentsFolder,
				EmployeesDocumentsFolder = model.EmployeesDocumentsFolder,
				EngineersDocumentsFolder = model.EngineersDocumentsFolder,
				InvoiceTypeName = model.InvoiceTypeName,
				InvoiceJournalTypeCode = model.InvoiceJournalTypeCode,
				CustomerTypeName = model.CustomerTypeName,
				CustomerDbCr = model.CustomerDbCr,
				VatTypeName = model.VatTypeName,
				VatAccountNumber = model.VatAccountNumber,
				ExpenseTypeName = model.ExpenseTypeName,
				ExpenseJournalTypeCode = model.ExpenseJournalTypeCode,
				VendorTypeName = model.VendorTypeName,
				VendorDbCr = model.VendorDbCr,
				ExpenseVatTypeName = model.ExpenseVatTypeName,
				ExpenseVatAccountNumber = model.ExpenseVatAccountNumber,
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
			var profileAi = profile.AiSettings ?? new AiSettingsModel();

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
				DatabaseConnectionString = NormalizeMongoConnectionString(SystemUtilities.Unprotect(profile.EncryptedConnectionString)),

				// Legacy flat values (decrypted)
				AiModel = profile.AiModel,
				EncryptedAiApikey = SystemUtilities.Unprotect(profile.EncryptedAiApikey),

				// Phase 1 nested AI settings (decrypted)
				AiSettings = new AiSettingsModel
				{
					Provider = profileAi.Provider,
					OpenAiModel = profileAi.OpenAiModel,
					DeepSeekModel = profileAi.DeepSeekModel,
					EncryptedOpenAiApiKey = SystemUtilities.Unprotect(profileAi.EncryptedOpenAiApiKey),
					EncryptedDeepSeekApiKey = SystemUtilities.Unprotect(profileAi.EncryptedDeepSeekApiKey)
				},

				ProjectsDocumentsFolder = profile.ProjectsDocumentsFolder,
				EmployeesDocumentsFolder = profile.EmployeesDocumentsFolder,
				EngineersDocumentsFolder = profile.EngineersDocumentsFolder,
				InvoiceTypeName = profile.InvoiceTypeName,
				InvoiceJournalTypeCode = profile.InvoiceJournalTypeCode,
				CustomerTypeName = profile.CustomerTypeName,
				CustomerDbCr = profile.CustomerDbCr,
				VatTypeName = profile.VatTypeName,
				VatAccountNumber = profile.VatAccountNumber,
				ExpenseTypeName = profile.ExpenseTypeName,
				ExpenseJournalTypeCode = profile.ExpenseJournalTypeCode,
				VendorTypeName = profile.VendorTypeName,
				VendorDbCr = profile.VendorDbCr,
				ExpenseVatTypeName = profile.ExpenseVatTypeName,
				ExpenseVatAccountNumber = profile.ExpenseVatAccountNumber
			};

			HydrateLegacyAiFieldsFromNested(model);

			// Auto-generate MongoDB connection string if missing but parameters exist
			if (string.IsNullOrWhiteSpace(model.DatabaseConnectionString) &&
				(model.DatabaseType?.IndexOf("Mongo", StringComparison.OrdinalIgnoreCase) >= 0))
			{
				model.DatabaseConnectionString = BuildMongoConnectionString(model);
			}

			return model;
		}

		private static void HydrateLegacyAiFieldsFromNested(ConnectionModel model)
		{
			if (model == null)
				return;

			if (model.AiSettings == null)
				model.AiSettings = new AiSettingsModel();

			var provider = model.AiSettings.Provider;
			if (string.IsNullOrWhiteSpace(provider))
				provider = "OpenAI";

			model.AiSettings.Provider = provider;

			if (string.Equals(provider, "DeepSeek", StringComparison.OrdinalIgnoreCase))
			{
				if (string.IsNullOrWhiteSpace(model.AiSettings.DeepSeekModel) && !string.IsNullOrWhiteSpace(model.AiModel))
					model.AiSettings.DeepSeekModel = model.AiModel;

				if (string.IsNullOrWhiteSpace(model.AiSettings.EncryptedDeepSeekApiKey) && !string.IsNullOrWhiteSpace(model.EncryptedAiApikey))
					model.AiSettings.EncryptedDeepSeekApiKey = model.EncryptedAiApikey;
			}
			else
			{
				if (string.IsNullOrWhiteSpace(model.AiSettings.OpenAiModel) && !string.IsNullOrWhiteSpace(model.AiModel))
					model.AiSettings.OpenAiModel = model.AiModel;

				if (string.IsNullOrWhiteSpace(model.AiSettings.EncryptedOpenAiApiKey) && !string.IsNullOrWhiteSpace(model.EncryptedAiApikey))
					model.AiSettings.EncryptedOpenAiApiKey = model.EncryptedAiApikey;
			}

			model.AiModel = string.Equals(provider, "DeepSeek", StringComparison.OrdinalIgnoreCase)
				? model.AiSettings.DeepSeekModel
				: model.AiSettings.OpenAiModel;

			model.EncryptedAiApikey = string.Equals(provider, "DeepSeek", StringComparison.OrdinalIgnoreCase)
				? model.AiSettings.EncryptedDeepSeekApiKey
				: model.AiSettings.EncryptedOpenAiApiKey;
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
				var connectionString = NormalizeMongoConnectionString(conn.DatabaseConnectionString);

				var url = new MongoUrl(connectionString);
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
			var host = string.IsNullOrWhiteSpace(model.DatabaseHost) ? "localhost" : NormalizeMongoHost(model.DatabaseHost);
			var port = model.DatabasePort > 0 ? model.DatabasePort : 27017;

			string userInfo = string.Empty;
			if (!string.IsNullOrWhiteSpace(model.DatabaseUser) && !string.IsNullOrWhiteSpace(model.DatabasePassword))
			{
				userInfo = $"{Uri.EscapeDataString(model.DatabaseUser)}:{Uri.EscapeDataString(model.DatabasePassword)}@";
			}

			if (IsAtlasSrvHost(host))
			{
				var databaseName = string.IsNullOrWhiteSpace(model.DatabaseName)
					? string.Empty
					: Uri.EscapeDataString(model.DatabaseName);

				return $"mongodb+srv://{userInfo}{host}/{databaseName}";
			}

			return $"mongodb://{userInfo}{host}:{port}/{model.DatabaseName}?authSource=admin";
		}

		private static string NormalizeMongoHost(string host)
		{
			if (string.IsNullOrWhiteSpace(host))
				return string.Empty;

			host = host.Trim();

			if (host.StartsWith("mongodb+srv://", StringComparison.OrdinalIgnoreCase))
				host = host.Substring("mongodb+srv://".Length);
			else if (host.StartsWith("mongodb://", StringComparison.OrdinalIgnoreCase))
				host = host.Substring("mongodb://".Length);

			var atIndex = host.LastIndexOf('@');
			if (atIndex >= 0 && atIndex < host.Length - 1)
				host = host.Substring(atIndex + 1);

			var slashIndex = host.IndexOf('/');
			if (slashIndex >= 0)
				host = host.Substring(0, slashIndex);

			var queryIndex = host.IndexOf('?');
			if (queryIndex >= 0)
				host = host.Substring(0, queryIndex);

			host = StripPortFromHost(host);

			return host.Trim();
		}

		private static string NormalizeMongoConnectionString(string connectionString)
		{
			if (string.IsNullOrWhiteSpace(connectionString))
				return connectionString;

			connectionString = connectionString.Trim();

			const string srvScheme = "mongodb+srv://";
			if (!connectionString.StartsWith(srvScheme, StringComparison.OrdinalIgnoreCase))
				return connectionString;

			var authorityStart = srvScheme.Length;
			var suffixStart = connectionString.IndexOfAny(new[] { '/', '?' }, authorityStart);
			var authority = suffixStart >= 0
				? connectionString.Substring(authorityStart, suffixStart - authorityStart)
				: connectionString.Substring(authorityStart);
			var suffix = suffixStart >= 0
				? connectionString.Substring(suffixStart)
				: string.Empty;

			var atIndex = authority.LastIndexOf('@');
			var userInfo = atIndex >= 0
				? authority.Substring(0, atIndex + 1)
				: string.Empty;
			var host = atIndex >= 0
				? authority.Substring(atIndex + 1)
				: authority;

			suffix = NormalizeSrvConnectionSuffix(suffix, userInfo);

			return $"{srvScheme}{userInfo}{NormalizeMongoHost(host)}{suffix}";
		}

		private static string NormalizeSrvConnectionSuffix(string suffix, string userInfo)
		{
			if (string.IsNullOrWhiteSpace(suffix))
				return suffix;

			var queryIndex = suffix.IndexOf('?');
			if (queryIndex < 0)
				return suffix;

			var username = GetMongoUsernameFromUserInfo(userInfo);
			if (string.IsNullOrWhiteSpace(username))
				return suffix;

			var path = suffix.Substring(0, queryIndex);
			var query = suffix.Substring(queryIndex + 1);
			var queryParts = query.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
			if (queryParts.Length == 0)
				return suffix;

			var keptParts = new System.Collections.Generic.List<string>(queryParts.Length);
			foreach (var part in queryParts)
			{
				var separatorIndex = part.IndexOf('=');
				var key = separatorIndex >= 0 ? part.Substring(0, separatorIndex) : part;
				var value = separatorIndex >= 0 ? part.Substring(separatorIndex + 1) : string.Empty;

				if (key.Equals("authSource", StringComparison.OrdinalIgnoreCase) &&
					value.Equals(username, StringComparison.OrdinalIgnoreCase))
				{
					continue;
				}

				keptParts.Add(part);
			}

			if (keptParts.Count == queryParts.Length)
				return suffix;

			return keptParts.Count == 0
				? path
				: $"{path}?{string.Join("&", keptParts)}";
		}

		private static string GetMongoUsernameFromUserInfo(string userInfo)
		{
			if (string.IsNullOrWhiteSpace(userInfo))
				return string.Empty;

			var trimmedUserInfo = userInfo.TrimEnd('@');
			if (string.IsNullOrWhiteSpace(trimmedUserInfo))
				return string.Empty;

			var separatorIndex = trimmedUserInfo.IndexOf(':');
			var username = separatorIndex >= 0
				? trimmedUserInfo.Substring(0, separatorIndex)
				: trimmedUserInfo;

			return Uri.UnescapeDataString(username);
		}

		private static string StripPortFromHost(string host)
		{
			if (string.IsNullOrWhiteSpace(host))
				return string.Empty;

			host = host.Trim();

			if (host.IndexOf(',') >= 0)
				return host;

			if (host.StartsWith("[", StringComparison.Ordinal))
			{
				var bracketEnd = host.IndexOf(']');
				if (bracketEnd >= 0 && bracketEnd < host.Length - 1 && host[bracketEnd + 1] == ':')
				{
					var portPart = host.Substring(bracketEnd + 2);
					if (int.TryParse(portPart, out _))
						return host.Substring(0, bracketEnd + 1);
				}

				return host;
			}

			var colonIndex = host.LastIndexOf(':');
			if (colonIndex > 0 && colonIndex < host.Length - 1)
			{
				var portPart = host.Substring(colonIndex + 1);
				if (int.TryParse(portPart, out _))
					return host.Substring(0, colonIndex);
			}

			return host;
		}

		private static bool IsAtlasSrvHost(string host)
		{
			return !string.IsNullOrWhiteSpace(host) &&
				host.IndexOf(".mongodb.net", StringComparison.OrdinalIgnoreCase) >= 0;
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
			else
				connString = NormalizeMongoConnectionString(connString);

			try
			{
				ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
				var client = new MongoClient(connString);
				var url = new MongoUrl(connString);
				var dbName = !string.IsNullOrWhiteSpace(connToCheck.DatabaseName)
					? connToCheck.DatabaseName
					: (url.DatabaseName ?? "admin");
				// Ping command is lightweight
				client.GetDatabase(dbName).RunCommand((Command<BsonDocument>)"{ping:1}");
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
