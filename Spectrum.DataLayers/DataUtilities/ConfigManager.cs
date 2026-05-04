using Spectrum.Models.Administration.Connections;
using System;
using System.IO;
using static System.Environment;
using Formatting = Newtonsoft.Json.Formatting;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace Spectrum.DataLayers.DataUtilities
{
	/// <summary>
	/// Handles loading and saving the configuration to a JSON file in AppData.
	/// </summary>
	public static class ConfigManager
	{
		private static readonly string _companyName = "Spectrum";
		private static readonly string _layoutsFolder = "Connections";
		private static readonly string _fileName = "AppConnections.json";

		/// <summary>
		/// Loads the application configuration for the specified profile.
		/// Returns a new AppConfigModel if the file doesn't exist or is corrupted.
		/// </summary>
		/// <param name="profileName">The profile name to load configuration for.</param>
		/// <returns>The loaded configuration or a new instance if not found.</returns>
		public static AppConfigModel Load(string profileName)
		{
			if (!IsValidProfileName(profileName))
			{
				return new AppConfigModel();
			}

			string filePath = GetConfigFilePath(profileName);
			if (filePath == null)
			{
				return new AppConfigModel();
			}

			if (!File.Exists(filePath))
			{
				return new AppConfigModel();
			}

			try
			{
				string json = File.ReadAllText(filePath);
				return JsonConvert.DeserializeObject<AppConfigModel>(json) ?? new AppConfigModel();
			}
			catch (Exception ex)
			{
				// Log the error for debugging purposes
				System.Diagnostics.Debug.WriteLine($"Error loading config for profile '{profileName}': {ex.Message}");
				// If file is corrupted or unreadable, return new config
				return new AppConfigModel();
			}
		}

		/// <summary>
		/// Saves the application configuration for the specified profile.
		/// Creates the directory structure if it doesn't exist.
		/// </summary>
		/// <param name="config">The configuration to save.</param>
		/// <param name="profileName">The profile name to save configuration for.</param>
		public static void Save(AppConfigModel config, string profileName)
		{
			if (config == null)
			{
				throw new ArgumentNullException(nameof(config), "Configuration cannot be null");
			}

			if (!IsValidProfileName(profileName))
			{
				throw new ArgumentException("Profile name cannot be null or whitespace", nameof(profileName));
			}

			string directoryPath = GetConfigDirectoryPath(profileName);
			if (directoryPath == null)
			{
				throw new InvalidOperationException($"Failed to determine configuration path for profile '{profileName}'");
			}

			try
			{
				// Ensure directory exists before writing
				EnsureDirectoryExists(directoryPath);

				string filePath = Path.Combine(directoryPath, _fileName);
				string json = JsonConvert.SerializeObject(config, Formatting.Indented);
				File.WriteAllText(filePath, json);
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Failed to save configuration for profile '{profileName}': {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Gets the full file path for the configuration file.
		/// </summary>
		/// <param name="profileName">The profile name.</param>
		/// <returns>The full file path or null if the profile name is invalid.</returns>
		private static string GetConfigFilePath(string profileName)
		{
			string directoryPath = GetConfigDirectoryPath(profileName);
			return directoryPath != null ? Path.Combine(directoryPath, _fileName) : null;
		}

		/// <summary>
		/// Gets the directory path where configuration files are stored.
		/// </summary>
		/// <param name="profileName">The profile name.</param>
		/// <returns>The directory path or null if the profile name is invalid.</returns>
		private static string GetConfigDirectoryPath(string profileName)
		{
			if (!IsValidProfileName(profileName))
			{
				return null;
			}

			try
			{
				return Path.Combine(
					GetFolderPath(SpecialFolder.ApplicationData),
					Sanitize(_companyName),
					Sanitize(profileName),
					_layoutsFolder);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Error building config path for profile '{profileName}': {ex.Message}");
				return null;
			}
		}

		/// <summary>
		/// Ensures the specified directory exists, creating it if necessary.
		/// </summary>
		/// <param name="directoryPath">The directory path to ensure exists.</param>
		private static void EnsureDirectoryExists(string directoryPath)
		{
			if (string.IsNullOrWhiteSpace(directoryPath))
			{
				throw new ArgumentException("Directory path cannot be null or whitespace", nameof(directoryPath));
			}

			try
			{
				if (!Directory.Exists(directoryPath))
				{
					Directory.CreateDirectory(directoryPath);
				}
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Failed to create directory '{directoryPath}': {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Validates that the profile name is not null or whitespace.
		/// </summary>
		/// <param name="profileName">The profile name to validate.</param>
		/// <returns>True if valid, false otherwise.</returns>
		private static bool IsValidProfileName(string profileName)
		{
			return !string.IsNullOrWhiteSpace(profileName);
		}

		/// <summary>
		/// Sanitizes a string value by removing spaces.
		/// </summary>
		/// <param name="value">The value to sanitize.</param>
		/// <returns>The sanitized value or empty string if input is null or whitespace.</returns>
		private static string Sanitize(string value)
		{
			return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Replace(" ", string.Empty);
		}
	}
}
