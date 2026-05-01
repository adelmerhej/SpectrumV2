using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace Spectrum.Reports.Interfaces
{
	/// <summary>
	/// Identifies a grid-row entity that exposes a numeric Id.
	/// </summary>
	public interface IHasId
	{
		long Id { get; }
	}

	/// <summary>
	/// Optional interface implemented by adapters that require specific worksheet names
	/// to be present in the editable workbook (for example: "Invoices", "Addendums").
	/// </summary>
	public interface IWorksheetRequirements
	{
		/// <summary>
		/// Returns the set of worksheet names that the adapter expects to exist in the workbook.
		/// </summary>
		IEnumerable<string> RequiredWorksheetNames();
	}

	/// <summary>
	/// Describes a single column for the DevExpress GridControl.
	/// </summary>
	public class GridColumnDescriptor
	{
		public string FieldName { get; set; }
		public string Caption { get; set; }
		public bool ReadOnly { get; set; }
		public int Width { get; set; }
		public string FormatString { get; set; }

		public GridColumnDescriptor() { }

		public GridColumnDescriptor(string fieldName, string caption, bool readOnly = false, int width = 100, string formatString = null)
		{
			FieldName = fieldName;
			Caption = caption;
			ReadOnly = readOnly;
			Width = width;
			FormatString = formatString;
		}
	}

	/// <summary>
	/// Describes a quick-action toolbar button.
	/// </summary>
	public class QuickActionDescriptor
	{
		public string Name { get; set; }
		public string Caption { get; set; }
		public string IconName { get; set; }
		public Func<bool> CanExecute { get; set; }
		public Action Execute { get; set; }

		public QuickActionDescriptor() { }

		public QuickActionDescriptor(string name, string caption, Action execute, Func<bool> canExecute = null, string iconName = null)
		{
			Name = name;
			Caption = caption;
			Execute = execute;
			CanExecute = canExecute;
			IconName = iconName;
		}
	}

	/// <summary>
	/// Describes a single field available for report building.
	/// Scalar fields have <see cref="GetValue"/>; collection/row-level fields have <see cref="GetRowValue"/>.
	/// </summary>
	public class FieldDescriptor
	{
		public string Key { get; set; }
		public string Caption { get; set; }
		public string Category { get; set; }
		public Type ValueType { get; set; }
		public string FormatString { get; set; }
		public Func<object> GetValue { get; set; }
		public Func<int, object> GetRowValue { get; set; }
		public int RowCount { get; set; }

		public bool IsRowLevel { get { return GetRowValue != null; } }

		public FieldDescriptor() { }

		public FieldDescriptor(string key, string caption, string category,
			Type valueType = null, string formatString = null,
			Func<object> getValue = null,
			Func<int, object> getRowValue = null, int rowCount = 0)
		{
			Key = key;
			Caption = caption;
			Category = category;
			ValueType = valueType;
			FormatString = formatString;
			GetValue = getValue;
			GetRowValue = getRowValue;
			RowCount = rowCount;
		}
	}

	/// <summary>
	/// Adapter contract that bridges a domain entity to the ReportPreviewControl.
	/// Implement per model to feed grid data, spreadsheet streams, exports, and XtraReports.
	/// </summary>
	public interface IReportAdapter : IDisposable
	{
		/// <summary>Display title shown in the toolbar / form caption.</summary>
		string Title { get; }

		/// <summary>The bound domain entity.</summary>
		object Model { get; }

		/// <summary>Column metadata for GridControl auto-configuration.</summary>
		IList<GridColumnDescriptor> GetGridColumns();

		/// <summary>Data source for GridControl (must implement IBindingList for change tracking).</summary>
		IBindingList GetGridDataSource();

		/// <summary>
		/// Returns a stream containing CSV or XLSX data for SpreadsheetControl.
		/// <paramref name="format"/> should be "csv" or "xlsx".
		/// </summary>
		Stream GetSpreadsheetStream(out string format);

		/// <summary>Toolbar quick-action descriptors.</summary>
		IList<QuickActionDescriptor> GetQuickActions();

		/// <summary>Recalculate derived / summary fields after a grid edit.</summary>
		void Recalculate();

		/// <summary>Key-value pairs for the summary panel (label ظْ formatted value).</summary>
		IList<KeyValuePair<string, string>> GetSummaryFields();

		/// <summary>Generate full CSV string for file export.</summary>
		string ExportToCsvString();

		/// <summary>Generate XLSX byte array for file export. Return null if unsupported.</summary>
		byte[] ExportToXlsxBytes();

		/// <summary>Build/load an XtraReport for print preview. Return null if unsupported.</summary>
		XtraReport BuildReport();

		/// <summary>Apply editable spreadsheet worksheet changes back to the underlying model.</summary>
		void ApplySpreadsheetChanges(DevExpress.Spreadsheet.IWorkbook workbook);

		/// <summary>Available field descriptors for dynamic report building.</summary>
		IList<FieldDescriptor> GetFieldDescriptors();
	}
}
