using SpectrumV1.Models.Administration.Connections;
using System;
using System.IO;
using static System.Environment;
using Formatting = Newtonsoft.Json.Formatting;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace SpectrumV1.DataLayers.DataUtilities
{
	/// <summary>
	/// Handles loading and saving the configuration to a JSON file in AppData.
	/// </summary>
	public static class ConfigManager
	{
		private static readonly string _layoutsFolder = "Connections";

		public static AppConfigModel Load(string profileName)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(profileName) || string.IsNullOrWhiteSpace(profileName)) return null;
				string directoryPath = Path.Combine(
					GetFolderPath(SpecialFolder.ApplicationData),
					Sanitize(profileName),
					_layoutsFolder);

				string filePath = Path.Combine(directoryPath, profileName, "connections.json");

				if (!File.Exists(filePath)) return new AppConfigModel();

				string json = File.ReadAllText(filePath);
				return JsonConvert.DeserializeObject<AppConfigModel>(json) ?? new AppConfigModel();
			}
			catch (Exception)
			{
				// If file is corrupted, return new config
				return new AppConfigModel();
			}
		}

		public static void Save(AppConfigModel config, string profileName)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(profileName) || string.IsNullOrWhiteSpace(profileName)) return;

				string directoryPath = Path.Combine(
					GetFolderPath(SpecialFolder.ApplicationData),
					Sanitize(profileName),
					_layoutsFolder);

				string filePath = Path.Combine(directoryPath, profileName, "connections.json");

				// Pretty print JSON for easier debugging (values are still encrypted)
				string json = JsonConvert.SerializeObject(config, Formatting.Indented);
				File.WriteAllText(filePath, json);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		private static string Sanitize(string value)
		{
			return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Replace(" ", string.Empty);
		}
	}
}
