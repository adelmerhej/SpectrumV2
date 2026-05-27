using DevExpress.XtraReports.UI;
using Spectrum.Reports.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace Spectrum.Reports.Adapters
{
    /// <summary>
    /// Abstract base implementing <see cref="IReportAdapter"/> with sensible defaults.
    /// Override only what each domain adapter needs.
    /// </summary>
    public abstract class ReportAdapterBase : IReportAdapter
    {
        public abstract string Title { get; }
        public abstract object Model { get; }

        public abstract IList<GridColumnDescriptor> GetGridColumns();
        public abstract IBindingList GetGridDataSource();
        public abstract string ExportToCsvString();

        /// <summary>
        /// Default: converts <see cref="ExportToCsvString"/> into a UTF-8 MemoryStream.
        /// Override to provide XLSX stream instead.
        /// </summary>
        public virtual Stream GetSpreadsheetStream(out string format)
        {
            format = "csv";
            string csv = ExportToCsvString();
            if (string.IsNullOrEmpty(csv))
                return null;

            var bytes = Encoding.UTF8.GetBytes(csv);
            return new MemoryStream(bytes);
        }

        /// <summary>Default: no quick actions.</summary>
        public virtual IList<QuickActionDescriptor> GetQuickActions()
        {
            return new List<QuickActionDescriptor>();
        }

        /// <summary>Default: no-op recalculation.</summary>
        public virtual void Recalculate() { }

        /// <summary>Default: spreadsheet edits do not map back to the model.</summary>
        public virtual void ApplySpreadsheetChanges(DevExpress.Spreadsheet.IWorkbook workbook) { }

        /// <summary>Default: empty summary.</summary>
        public virtual IList<KeyValuePair<string, string>> GetSummaryFields()
        {
            return new List<KeyValuePair<string, string>>();
        }

        /// <summary>Default: XLSX export not supported.</summary>
        public virtual byte[] ExportToXlsxBytes()
        {
            return null;
        }

        /// <summary>Default: no XtraReport.</summary>
        public virtual XtraReport BuildReport()
        {
            return null;
        }

        /// <summary>Default: no field descriptors.</summary>
        public virtual IList<FieldDescriptor> GetFieldDescriptors()
        {
            return new List<FieldDescriptor>();
        }

        /// <summary>Override to release UnitOfWork, streams, etc.</summary>
        public virtual void Dispose() { }

        #region CSV Helpers

        /// <summary>
        /// Escapes a value for CSV output (RFC 4180).
        /// </summary>
        public static string CsvEscape(string value)
        {
            if (value == null) return string.Empty;
            if (value.IndexOfAny(new[] { ',', '"', '\n', '\r' }) >= 0)
                return "\"" + value.Replace("\"", "\"\"") + "\"";
            return value;
        }

        /// <summary>
        /// Builds a CSV row from an array of values.
        /// </summary>
        protected static string CsvRow(params string[] values)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < values.Length; i++)
            {
                if (i > 0) sb.Append(',');
                sb.Append(CsvEscape(values[i]));
            }
            return sb.ToString();
        }

        #endregion
    }
}
