using DevExpress.XtraEditors;
using Microsoft.AspNet.Identity;
using SpectrumV1.DataLayers.Administration.Update;
using SpectrumV1.DataLayers.DataAccess;
using SpectrumV1.Models.Administration.Connections;
using SpectrumV1.Models.Users;
using SpectrumV1.Update.Utilities;
using SpectrumV1.Utilities.Common;
using SpectrumV1.Views.Main.Connections;
using SpectrumV1.Views.Main.Update;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace SpectrumV1.Utilities
{
	public static class HelperApplication
	{
		#region Database Connection Result Types

		/// <summary>
		/// Represents the result of a database connection attempt
		/// </summary>
		private class DatabaseConnectionResult
		{
			public DatabaseConnectionStatus Status { get; set; }
			public ConnectionModel ConnectionModel { get; set; }
			public IDatabase Database { get; set; }
			public string ErrorMessage { get; set; }
		}

		/// <summary>
		/// Represents the status of a database connection attempt
		/// </summary>
		private enum DatabaseConnectionStatus
		{
			Success,
			DatabaseNotFound,
			ConnectionFailed,
			ConfigurationRequired
		}

		#endregion

		#region Check for Application Update

		public static void CheckForUpdate()
		{
			// Called on MainForm_Load after successful login.
			// If newer version available, display LiveUpdateForm so user can decide.
			try
			{
				// Load settings (from DB or defaults)
				var settings = LiveUpdateSettingsProvider.LoadOrDefault(Application.StartupPath);

				// Determine current version (from settings override or executable version)
				string processExe = settings.ProcessToEnd + ".exe";
				string currentVersion = string.IsNullOrWhiteSpace(settings.CurrentVersionNo)
					? GetExternalFileVersion(processExe)
					: settings.CurrentVersionNo;

				// Fetch update info (version.txt |pipe delimited| first field expected = version)
				List<string> info = LiveUpdateHelper.GetUpdateInfo(
					settings.UrlLink,
					settings.VersionFilename,
					Application.StartupPath + @"\",
					0);

				if (info == null || info.Count == 0) return; // Nothing retrieved

				string availableVersion = info[0];

				if (!IsNewerVersion(currentVersion, availableVersion)) return; // Up to date

				// Show update dialog (user can choose to update). Using ShowDialog to block until decision.
				using (var frm = new CheckForUpdateForm())
				{
					frm.ShowDialog();
				}
			}
			catch (Exception ex)
			{
				// Swallow non-critical errors; optionally notify user silently.
				System.Diagnostics.Debug.WriteLine("CheckForUpdate error: " + ex.Message);
			}
		}

		public static void CheckForLiveUpdate()
		{
			// Silently update LiveUpdate.exe (the updater engine) if a newer version exists.
			// Uses remote file 'live.version.txt' (pipe-delimited, first segment = version)
			// Expected archive naming convention: LiveUpdate.<version>.zip containing new engine (optionally prefixed files).
			try
			{
				var settings = LiveUpdateSettingsProvider.LoadOrDefault(Application.StartupPath);
				string engineVersionFile = "live.version.txt"; // remote version descriptor for updater engine

				// Current engine version
				string currentEngineVersion = GetExternalFileVersion("LiveUpdate.exe");

				// Fetch remote engine version info
				List<string> info = LiveUpdateHelper.GetUpdateInfo(
					settings.UrlLink,
					engineVersionFile,
					Application.StartupPath + @"\",
					0);

				if (info == null || info.Count == 0) return; // No info retrieved
				string availableEngineVersion = info[0];

				if (!IsNewerVersion(currentEngineVersion, availableEngineVersion)) return; // Already latest

				// Build expected zip filename
				string zipName = $"LiveUpdate.{availableEngineVersion}.zip";

				// Download and unzip silently
				LiveUpdateHelper.InstallUpdateNow(settings.UrlLink, zipName, Application.StartupPath + @"\", true);

				// Rename any prefixed files to original names (e.g. V1234_LiveUpdate.exe -> LiveUpdate.exe)
				LiveUpdateHelper.UpdateMe(settings.UpdaterPrefix, Application.StartupPath + @"\");

				// Optional: verify and log
				string newVersion = GetExternalFileVersion("LiveUpdate.exe");
				System.Diagnostics.Debug.WriteLine($"LiveUpdate engine updated silently from {currentEngineVersion} to {newVersion}");
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("Silent LiveUpdate engine update failed: " + ex.Message);
			}
		}

		// Helper copied from LiveUpdateForm (public here for reuse)
		private static bool IsNewerVersion(string current, string available)
		{
			if (Version.TryParse(current, out var cur) && Version.TryParse(available, out var avail))
			{
				return avail > cur;
			}

			// Fallback manual comparison
			var curParts = current.Split('.');
			var availParts = available.Split('.');
			int max = Math.Max(curParts.Length, availParts.Length);
			for (int i = 0; i < max; i++)
			{
				int curVal = i < curParts.Length && int.TryParse(curParts[i], out var c) ? c : 0;
				int availVal = i < availParts.Length && int.TryParse(availParts[i], out var a) ? a : 0;
				if (availVal > curVal) return true;
				if (availVal < curVal) return false;
			}
			return false;
		}

		private static string GetExternalFileVersion(string fileName)
		{
			try
			{
				string fullPath = Path.Combine(Application.StartupPath, fileName);
				if (!File.Exists(fullPath)) return "0.0.0.0";
				var fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(fullPath);
				string ver = !string.IsNullOrWhiteSpace(fvi.ProductVersion) ? fvi.ProductVersion : fvi.FileVersion;
				return ver ?? "0.0.0.0";
			}
			catch
			{
				return "0.0.0.0";
			}
		}

		#endregion

		#region Database Connection
		public static bool CheckDatabaseConnection()
		{
			// Keep attempting connection; if configuration or failure occurs, loop showing configuration form until user cancels.
			while (true)
			{
				var connectionResult = AttemptDatabaseConnection();
				switch (connectionResult.Status)
				{
					case DatabaseConnectionStatus.Success:
						return true;

					case DatabaseConnectionStatus.DatabaseNotFound:
						if (HandleDatabaseNotFound(connectionResult.ConnectionModel))
						{
							continue; // try again after handling
						}
						return false;

					case DatabaseConnectionStatus.ConfigurationRequired:
					case DatabaseConnectionStatus.ConnectionFailed:
						if (!ShowConfigurationLoop())
							return false; // user cancelled
						continue; // configuration updated; re-attempt

					default:
						return false;
				}
			}
		}

		/// <summary>
		/// Loop that keeps showing configuration dialog until user cancels or connection succeeds.
		/// Returns true if configuration changed and connection now succeeds; false if user cancelled.
		/// </summary>
		private static bool ShowConfigurationLoop()
		{
			bool firstIteration = true;
			while (true)
			{
				if (!firstIteration)
				{
					SafeShowMessageBox("Unable to connect with the supplied settings. Please adjust and try again.",
						"Connection Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

					bool configured = ShowServerConfigurationDialog();
					if (!configured) return false; // user cancelled
				}
				firstIteration = false;

				var retryResult = AttemptDatabaseConnection();
				if (retryResult.Status == DatabaseConnectionStatus.Success)
					return true;
				if (retryResult.Status == DatabaseConnectionStatus.DatabaseNotFound)
				{
					if (HandleDatabaseNotFound(retryResult.ConnectionModel))
						continue; // database created; re-attempt
					return false;
				}
				// Anything else loops again automatically.
			}
		}

		/// <summary>
		/// Attempts to establish database connection and returns detailed result
		/// </summary>
		private static DatabaseConnectionResult AttemptDatabaseConnection()
		{
			try
			{
				var connectionModel = DatabaseFactory.GetConnection(DatabaseFactory.ProfilePrimary);

				if (connectionModel == null || string.IsNullOrWhiteSpace(connectionModel.DatabaseHost))
				{
					return new DatabaseConnectionResult
					{
						Status = DatabaseConnectionStatus.ConfigurationRequired,
						ConnectionModel = connectionModel,
						ErrorMessage = "Database configuration is missing or invalid"
					};
				}


				var connectionState = DatabaseFactory.TestConnection(connectionModel, DatabaseFactory.ProfilePrimary);

				if (connectionState)
				{
					// Update current user context on successful connection
					UpdateUserContext(connectionModel);

					return new DatabaseConnectionResult
					{
						Status = DatabaseConnectionStatus.Success,
						ConnectionModel = connectionModel
					};
				}

				return new DatabaseConnectionResult
				{
					Status = DatabaseConnectionStatus.ConnectionFailed,
					ConnectionModel = connectionModel,
					ErrorMessage = $"Connection failed. State: {connectionState}"
				};
			}
			catch (Exception ex)
			{
				return new DatabaseConnectionResult
				{
					Status = DatabaseConnectionStatus.ConnectionFailed,
					ConnectionModel = null,
					ErrorMessage = $"Exception occurred: {ex.Message}"
				};
			}
		}

		/// <summary>
		/// Handles the case when database is not found and prompts user for action
		/// </summary>
		private static bool HandleDatabaseNotFound(ConnectionModel connectionModel)
		{
			var userChoice = SafeShowMessageBox($"The database '{connectionModel.DatabaseName}' could not be found.\n\nPlease create it or adjust settings.",
				"Database Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			return false; // Placeholder: creation logic could be added here.
		}

		/// <summary>
		/// Shows server configuration dialog and returns success status
		/// </summary>
		private static bool ShowServerConfigurationDialog()
		{
			try
			{
				// Fix for CS8370: Use traditional using statement instead of using declaration
				using (var configForm = new ServerConfigurationForm())
				{
					return configForm.ShowDialog() == DialogResult.OK;
				}
			}
			catch (Exception ex)
			{
				SafeShowMessageBox(
					$"Failed to open configuration dialog: {ex.Message}",
					"Configuration Error",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				return false;
			}
		}

		/// <summary>
		/// Updates current user context with connection information
		/// </summary>
		private static void UpdateUserContext(ConnectionModel connectionModel)
		{
			if (connectionModel != null)
			{
				CurrentUser.HostName = connectionModel.DatabaseHost;
				CurrentUser.DatabaseName = connectionModel.DatabaseName;
			}
		}

		#endregion

		#region Default Settings

		public static void ApplyDefaultSettings()
		{
			// Add default records or settings as needed
			try
			{
				//1- Check collection Companies and add default if empty
				var companyRepo = new DataLayers.Common.Companies.CompanyRepository(DatabaseFactory.ProfilePrimary);
				var companyCount = companyRepo.GetCountAsync().Result;
				if (companyCount == 0)
				{
					var defaultCompany = new Models.Common.Companies.CompanyModel
					{
						CompanyName = "Spectrum Lebanon",
						Company = "Spectrum Lebanon",
						Branch = "Default Branch",
						ShortName = "Spectrum lb",
						DirectoryPath = @"z:\Companies\Spectrum-lb",
						Address = "",
						PhoneNumber1 = "48 3683 838",
						Email = "",
						Website = "www.spectrumlb.com",
						CreatedBy = "admin",
						CreatedAt = DateTime.Now,
						IsDefault = true,
						Active = true,
						WorkingYear = DateTime.Now.Year,
					};
					companyRepo.AddNewCompanyAsync(defaultCompany).Wait();
				}

				//2- Check collection Companies and add default if empty
				var branchRepo = new DataLayers.Common.Branches.BranchRepository(DatabaseFactory.ProfilePrimary);
				var branchCount = branchRepo.GetCountAsync().Result;
				if (branchCount == 0)
				{
					var defaultBranch = new Models.Common.Companies.BranchModel
					{
						BranchName = "Default Branch",
						Company = "Spectrum Lebanon",
						Branch = "Default Branch",
						CreatedBy = "admin",
						CreatedAt = DateTime.Now,
						IsDefault = true,
						Active = true,
						WorkingYear = DateTime.Now.Year,
					};
					branchRepo.AddNewBranchAsync(defaultBranch).Wait();
				}

				//3- Check collection Continents and add default if empty
				var continentRepo = new DataLayers.Common.Countries.ContinentRepository(DatabaseFactory.ProfilePrimary);
				var continentCount = continentRepo.GetCountAsync().Result;
				if (continentCount == 0)
				{
					var defaultContinent = new Models.Common.Countries.ContinentModel
					{
						ContinentName = "Asia",
						Company = "Spectrum Lebanon",
						Branch = "Default Branch",
						CreatedBy = "admin",
						CreatedAt = DateTime.Now,
						IsDefault = true,
						Active = true,
						WorkingYear = DateTime.Now.Year,
					};
					continentRepo.AddNewContinentAsync(defaultContinent).Wait();
				}

				//4- Check collection Regions and add default if empty
				var regionRepo = new DataLayers.Common.Countries.RegionRepository(DatabaseFactory.ProfilePrimary);
				var regionCount = regionRepo.GetCountAsync().Result;
				if (regionCount == 0)
				{
					var defaultRegion = new Models.Common.Countries.RegionModel
					{
						RegionName = "Middle East",
						Continent = "Asia",
						Company = "Spectrum Lebanon",
						Branch = "Default Branch",
						CreatedBy = "admin",
						CreatedAt = DateTime.Now,
						IsDefault = true,
						Active = true,
						WorkingYear = DateTime.Now.Year,
					};
					regionRepo.AddNewRegionAsync(defaultRegion).Wait();
				}

				//5- Check collection Countries and add default if empty
				var countryRepo = new DataLayers.Common.Countries.CountryRepository(DatabaseFactory.ProfilePrimary);
				var countryCount = countryRepo.GetCountAsync().Result;
				if (countryCount == 0)
				{
					var defaultCountry = new Models.Common.Countries.CountryModel
					{
						CountryName = "Lebanon",
						Region = "Middle East",
						Continent = "Asia",
						Company = "Spectrum Lebanon",
						Branch = "Default Branch",
						CreatedBy = "admin",
						CreatedAt = DateTime.Now,
						IsDefault = true,
						Active = true,
						WorkingYear = DateTime.Now.Year,
					};
					countryRepo.AddNewCountryAsync(defaultCountry).Wait();
				}

				//6- Check collection Provinces and add default if empty
				var provinceRepo = new DataLayers.Common.Countries.ProvinceRepository(DatabaseFactory.ProfilePrimary);
				var provinceCount = provinceRepo.GetCountAsync().Result;
				if (provinceCount == 0)
				{
					var defaultProvince = new Models.Common.Countries.ProvinceModel
					{
						ProvinceName = "Beirut",
						Country = "Lebanon",
						Region = "Middle East",
						Continent = "Asia",
						Company = "Spectrum Lebanon",
						Branch = "Default Branch",
						CreatedBy = "admin",
						CreatedAt = DateTime.Now,
						IsDefault = true,
						Active = true,
						WorkingYear = DateTime.Now.Year,
					};
					provinceRepo.AddNewProvinceAsync(defaultProvince).Wait();
				}

				//7- Check collection Districts and add default if empty
				var districtRepo = new DataLayers.Common.Countries.DistrictRepository(DatabaseFactory.ProfilePrimary);
				var districtCount = districtRepo.GetCountAsync().Result;
				if (districtCount == 0)
				{
					var defaultDistrict = new Models.Common.Countries.DistrictModel
					{
						DistrictName = "Achrafieh",
						Province = "Beirut",
						Country = "Lebanon",
						Region = "Middle East",
						Continent = "Asia",
						Company = "Spectrum Lebanon",
						Branch = "Default Branch",
						CreatedBy = "admin",
						CreatedAt = DateTime.Now,
						IsDefault = true,
						Active = true,
						WorkingYear = DateTime.Now.Year,
					};
					districtRepo.AddNewDistrictAsync(defaultDistrict).Wait();
				}

				//8- Check collection Cities and add default if empty
				var cityRepo = new DataLayers.Common.Countries.CityRepository(DatabaseFactory.ProfilePrimary);
				var cityCount = cityRepo.GetCountAsync().Result;
				if (cityCount == 0)
				{
					var defaultCity = new Models.Common.Countries.CityModel
					{
						CityName = "Beirut",
						District = "Achrafieh",
						Province = "Beirut",
						Country = "Lebanon",
						Region = "Middle East",
						Continent = "Asia",
						Company = "Spectrum Lebanon",
						Branch = "Default Branch",
						CreatedBy = "admin",
						CreatedAt = DateTime.Now,
						IsDefault = true,
						Active = true,
						WorkingYear = DateTime.Now.Year,
					};
					cityRepo.AddNewCityAsync(defaultCity).Wait();
				}

				//8- Check collection Currencies and add default if empty
				var currencyRepo = new DataLayers.Common.Currencies.CurrencyRepository(DatabaseFactory.ProfilePrimary);
				var currencyCount = currencyRepo.GetCountAsync().Result;
				if (currencyCount == 0)
				{
					var defaultCurrency = new Models.Common.Currencies.CurrencyModel
					{
						CurrencyName = "Lebanese Pound",
						CurrencyCode = "LBP",
						CurrencySymbol = "ل.ل",
						Company = "Spectrum Lebanon",
						Branch = "Default Branch",
						CreatedBy = "admin",
						CreatedAt = DateTime.Now,
						IsDefault = true,
						Active = true,
						WorkingYear = DateTime.Now.Year,
					};
					currencyRepo.AddNewCurrencyAsync(defaultCurrency).Wait();

					defaultCurrency = new Models.Common.Currencies.CurrencyModel
					{
						CurrencyName = "US Dollar",
						CurrencyCode = "USD",
						CurrencySymbol = "$",
						Company = "Spectrum Lebanon",
						Branch = "Default Branch",
						CreatedBy = "admin",
						CreatedAt = DateTime.Now,
						IsDefault = false,
						Active = true,
						WorkingYear = DateTime.Now.Year,
					};
					currencyRepo.AddNewCurrencyAsync(defaultCurrency).Wait();
				}

				//9- Check collection Users and add default if empty
				var userRepo = new DataLayers.Users.UserRepository(DatabaseFactory.ProfilePrimary);
				var userCount = userRepo.GetCountAsync().Result;
				if (userCount == 0)
				{
					// Create the default admin user document
					SystemUtilities.PasswordHasher = new PasswordHasher();
					var temporaryAdminPassword = SystemUtilities.PasswordHasher.HashPassword("admin");

					var defaultUser = new Models.Users.UserModel
					{
						Username = "admin",
						Email = "admin@spectrum-lb.com",
						PasswordHash = temporaryAdminPassword,
						SecurityStamp = Guid.NewGuid().ToString(),
						Roles = new List<string> { "admin" },
						IsLockedOut = false,
						CreatedBy = "admin",
						CreatedAt = DateTime.UtcNow
					};
					userRepo.AddNewUserAsync(defaultUser).Wait();
				}
			}
			catch (Exception ex)
			{
				SafeShowMessageBox(ex.Message, $@"Error {ex.HResult.ToString()}", MessageBoxButtons.OK,
					MessageBoxIcon.Error);
			}
		}

		#endregion

		#region Thread-Safe MessageBox

		/// <summary>
		/// Thread-safe method to show message boxes that can be called from any thread
		/// </summary>
		public static DialogResult SafeShowMessageBox(string message, string caption = "Error", MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.Error)
		{
			DialogResult result = DialogResult.OK;
			try
			{
				if (Application.OpenForms.Count > 0 && Application.OpenForms[0] != null)
				{
					var mainForm = Application.OpenForms[0];
					if (mainForm.InvokeRequired)
					{
						result = (DialogResult)mainForm.Invoke(new Func<DialogResult>(() =>
						{
							return XtraMessageBox.Show(message, caption, buttons, icon);
						}));
					}
					else
					{
						result = XtraMessageBox.Show(message, caption, buttons, icon);
					}
				}
				else
				{
					// Fallback to standard MessageBox if no forms are available
					result = MessageBox.Show(message, caption, buttons, icon);
				}
			}
			catch (Exception ex)
			{
				// Last resort: use standard MessageBox
				try
				{
					result = MessageBox.Show($@"Error displaying message: {ex.Message}\nOriginal message: {message}",
						@"Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				catch
				{
					// If even this fails, write to debug output
					System.Diagnostics.Debug.WriteLine($"Critical error: {ex.Message}, Original: {message}");
				}
			}
			return result;
		}

		#endregion

		#region Assembly Attribute Accessors

		public static string AssemblyTitle
		{
			get
			{
				// Get all Title attributes on this assembly
				object[] attributes = Assembly.GetExecutingAssembly()
					.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
				// If there is at least one Title attribute
				if (attributes.Length > 0)
				{
					// Select the first one
					AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
					// If it is not an empty string, return it
					if (titleAttribute.Title != "")
						return titleAttribute.Title;
				}

				// If there was no Title attribute, or if the Title attribute was the empty string, return the .exe name
				return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
			}
		}

		public static string AssemblyVersion
		{
			get
			{
				//return Assembly.GetExecutingAssembly().GetName().Version.ToString();   //get assembly version

				System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
				System.Diagnostics.FileVersionInfo fvi =
					System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
				string version = fvi.FileVersion; // get file version

				return version;
			}
		}

		public static string AssemblyDescription
		{
			get
			{
				// Get all Description attributes on this assembly
				object[] attributes = Assembly.GetExecutingAssembly()
					.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
				// If there aren't any Description attributes, return an empty string
				if (attributes.Length == 0)
					return "";
				// If there is a Description attribute, return its value
				return ((AssemblyDescriptionAttribute)attributes[0]).Description;
			}
		}

		public static string AssemblyProduct
		{
			get
			{
				// Get all Product attributes on this assembly
				object[] attributes = Assembly.GetExecutingAssembly()
					.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
				// If there aren't any Product attributes, return an empty string
				if (attributes.Length == 0)
					return "";
				// If there is a Product attribute, return its value
				return ((AssemblyProductAttribute)attributes[0]).Product;
			}
		}

		public static string AssemblyCopyright
		{
			get
			{
				// Get all Copyright attributes on this assembly
				object[] attributes = Assembly.GetExecutingAssembly()
					.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
				// If there aren't any Copyright attributes, return an empty string
				if (attributes.Length == 0)
					return "";
				// If there is a Copyright attribute, return its value
				return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
			}
		}

		public static string AssemblyCompany
		{
			get
			{
				// Get all Company attributes on this assembly
				object[] attributes = Assembly.GetExecutingAssembly()
					.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
				// If there aren't any Company attributes, return an empty string
				if (attributes.Length == 0)
					return "";
				// If there is a Company attribute, return its value
				return ((AssemblyCompanyAttribute)attributes[0]).Company;
			}
		}

		#endregion

	}
}

