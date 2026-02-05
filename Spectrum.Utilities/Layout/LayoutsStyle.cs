using DevExpress.XtraBars.Navigation;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTreeList;
using System;
using System.IO;
using static System.Environment;

namespace Spectrum.Utilities.Layout
{
	public static class LayoutsStyle
	{
		private static readonly string _layoutsFolder = "Layouts";

		private static string BuildDirectory(string companyName, string userName)
		{
			if (string.IsNullOrWhiteSpace(companyName) || string.IsNullOrWhiteSpace(userName)) return null;
			string directoryPath = Path.Combine(
				GetFolderPath(SpecialFolder.ApplicationData),
				Sanitize(companyName),
				_layoutsFolder,
				Sanitize(userName));
			try
			{
				if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
			}
			catch
			{
				return null; // Directory creation failed
			}
			return directoryPath;
		}

		private static string GetLayoutFilePath(string controlName, string companyName, string userName)
		{
			if (string.IsNullOrWhiteSpace(controlName)) return null;
			var directory = BuildDirectory(companyName, userName);
			if (directory == null) return null;
			return Path.Combine(directory, controlName + ".xml");
		}

		private static bool SaveLayoutCore(string controlName, Action<string> saveAction, string companyName, string userName)
		{
			var path = GetLayoutFilePath(controlName, companyName, userName);
			if (path == null) return false;
			try
			{
				saveAction(path);
				return true;
			}
			catch
			{
				return false;
			}
		}

		private static bool LoadLayoutCore(string controlName, Action<string> loadAction, string companyName, string userName)
		{
			var path = GetLayoutFilePath(controlName, companyName, userName);
			if (path == null || !File.Exists(path)) return false;
			try
			{
				loadAction(path);
				return true;
			}
			catch
			{
				return false;
			}
		}

		private static bool DeleteLayoutCore(string controlName, string companyName, string userName)
		{
			var path = GetLayoutFilePath(controlName, companyName, userName);
			if (path == null || !File.Exists(path)) return false;
			try
			{
				File.Delete(path);
				return true;
			}
			catch
			{
				return false;
			}
		}

		private static string Sanitize(string value)
		{
			return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Replace(" ", string.Empty);
		}

		// Menu (AccordionControl) overload with string name
		public static bool ResetLayoutMenu(string accordion, string userName, string companyName)
		{
			return DeleteLayoutCore(accordion, companyName, userName);
		}

		public static bool ResetLayoutMenu(AccordionControl accordion, string userName, string companyName)
		{
			return accordion != null && DeleteLayoutCore(accordion.Name, companyName, userName);
		}

		public static bool SaveLayoutMenu(AccordionControl accordion, string userName, string companyName)
		{
			return accordion != null && SaveLayoutCore(accordion.Name, accordion.SaveLayoutToXml, companyName, userName);
		}

		public static bool LoadLayoutMenu(AccordionControl accordion, string userName, string companyName)
		{
			return accordion != null && LoadLayoutCore(accordion.Name, accordion.RestoreLayoutFromXml, companyName, userName);
		}

		public static bool SaveLayoutGrid(GridView grid, string userName, string companyName)
		{
			return grid != null && SaveLayoutCore(grid.Name, grid.SaveLayoutToXml, companyName, userName);
		}

		public static bool LoadLayoutGrid(GridView grid, string userName, string companyName)
		{
			return grid != null && LoadLayoutCore(grid.Name, grid.RestoreLayoutFromXml, companyName, userName);
		}

		public static bool ResetLayoutGrid(GridView grid, string userName, string companyName)
		{
			return grid != null && DeleteLayoutCore(grid.Name, companyName, userName);
		}

		public static bool SaveLayoutTreeList(TreeList grid, string userName, string companyName)
		{
			return grid != null && SaveLayoutCore(grid.Name, grid.SaveLayoutToXml, companyName, userName);
		}

		public static bool LoadLayoutTreeList(TreeList grid, string userName, string companyName)
		{
			return grid != null && LoadLayoutCore(grid.Name, grid.RestoreLayoutFromXml, companyName, userName);
		}

		public static bool ResetLayoutTreeList(TreeList grid, string userName, string companyName)
		{
			return grid != null && DeleteLayoutCore(grid.Name, companyName, userName);
		}
	}
}
