using DevExpress.Spreadsheet;
using DevExpress.XtraSpreadsheet;
using System;
using System.Data;
using System.Globalization;

namespace Spectrum.Reports.Common
{
    public static class ReportSpreadsheetHelper
    {
        public static DataTable LoadWorksheetPreview(string filePath)
        {
            using (var spreadsheet = new SpreadsheetControl())
            {
                spreadsheet.LoadDocument(filePath, DocumentFormat.Xlsx);
                var workbook = spreadsheet.Document;

                if (workbook.Worksheets.Count <= 0)
                    throw new Exception("The selected Excel file does not contain any worksheets.");

                var worksheet = workbook.Worksheets[0];
                var usedRange = worksheet.GetUsedRange();
                if (usedRange == null || usedRange.RowCount <= 0 || usedRange.ColumnCount <= 0)
                    throw new Exception("The selected Excel file does not contain any data.");

                var table = new DataTable();
                var headerRowIndex = usedRange.TopRowIndex;
                var firstDataRowIndex = headerRowIndex + 1;

                for (var columnOffset = 0; columnOffset < usedRange.ColumnCount; columnOffset++)
                {
                    var columnIndex = usedRange.LeftColumnIndex + columnOffset;
                    var headerText = worksheet.Cells[headerRowIndex, columnIndex].DisplayText;
                    table.Columns.Add(string.IsNullOrWhiteSpace(headerText) ? $"Column{columnOffset + 1}" : headerText.Trim());
                }

                for (var rowIndex = firstDataRowIndex; rowIndex <= usedRange.BottomRowIndex; rowIndex++)
                {
                    var row = table.NewRow();
                    for (var columnOffset = 0; columnOffset < usedRange.ColumnCount; columnOffset++)
                    {
                        var columnIndex = usedRange.LeftColumnIndex + columnOffset;
                        row[columnOffset] = worksheet.Cells[rowIndex, columnIndex].DisplayText;
                    }

                    table.Rows.Add(row);
                }

                RemoveEmptyRows(table);
                EnsureColumnNames(table);

                if (table.Rows.Count <= 0)
                    throw new Exception("The selected Excel file does not contain any data.");

                return table;
            }
        }

        public static int WriteDetailRow(Worksheet worksheet, int row, string label, object value, string numberFormat = null)
        {
            worksheet.Cells[row, 0].Value = label;
            worksheet.Cells[row, 0].Font.Bold = true;

            if (value == null)
            {
                worksheet.Cells[row, 1].Value = string.Empty;
            }
            else if (value is DateTime)
            {
                worksheet.Cells[row, 1].Value = (DateTime)value;
                worksheet.Cells[row, 1].NumberFormat = "yyyy-mm-dd";
            }
            else if (value is decimal)
            {
                worksheet.Cells[row, 1].Value = (double)(decimal)value;
                if (numberFormat != null)
                    worksheet.Cells[row, 1].NumberFormat = numberFormat;
            }
            else
            {
                worksheet.Cells[row, 1].Value = value.ToString();
            }

            return row + 1;
        }

        public static void ApplyDefaultPrintSettings(IWorkbook workbook, Worksheet activeWorksheet, int portraitSheetIndex = 0)
        {
            if (workbook == null)
                return;

            for (int i = 0; i < workbook.Worksheets.Count; i++)
            {
                var worksheet = workbook.Worksheets[i];
                worksheet.ActiveView.ShowGridlines = true;
                worksheet.ActiveView.ShowHeadings = true;
                worksheet.ActiveView.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.A4;
                worksheet.ActiveView.Orientation = i == portraitSheetIndex ? PageOrientation.Portrait : PageOrientation.Landscape;
                worksheet.PrintOptions.PrintGridlines = true;
                worksheet.PrintOptions.FitToPage = true;
                worksheet.PrintOptions.FitToWidth = 1;
            }

            if (activeWorksheet != null)
                workbook.Worksheets.ActiveWorksheet = activeWorksheet;
        }

        public static string GetCellText(Worksheet worksheet, int row, int column)
        {
            return worksheet.Cells[row, column].DisplayText?.Trim();
        }

        public static DateTime? GetCellDate(Worksheet worksheet, int row, int column)
        {
            var text = GetCellText(worksheet, row, column);
            if (string.IsNullOrWhiteSpace(text))
                return null;

            DateTime value;
            return DateTime.TryParse(text, CultureInfo.CurrentCulture, DateTimeStyles.None, out value)
                || DateTime.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.None, out value)
                ? value
                : (DateTime?)null;
        }

        public static decimal? GetCellDecimal(Worksheet worksheet, int row, int column)
        {
            var text = GetCellText(worksheet, row, column);
            if (string.IsNullOrWhiteSpace(text))
                return null;

            decimal value;
            return decimal.TryParse(text, NumberStyles.Any, CultureInfo.CurrentCulture, out value)
                || decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out value)
                ? value
                : (decimal?)null;
        }

        public static int GetCellInt(Worksheet worksheet, int row, int column)
        {
            var text = GetCellText(worksheet, row, column);
            if (string.IsNullOrWhiteSpace(text))
                return 0;

            int value;
            return int.TryParse(text, NumberStyles.Integer, CultureInfo.CurrentCulture, out value)
                || int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out value)
                ? value
                : 0;
        }

        public static bool GetCellBoolean(Worksheet worksheet, int row, int column)
        {
            var text = GetCellText(worksheet, row, column);
            if (string.IsNullOrWhiteSpace(text))
                return false;

            return string.Equals(text, "yes", StringComparison.OrdinalIgnoreCase)
                || string.Equals(text, "true", StringComparison.OrdinalIgnoreCase)
                || string.Equals(text, "1", StringComparison.OrdinalIgnoreCase);
        }

        private static void RemoveEmptyRows(DataTable table)
        {
            for (var rowIndex = table.Rows.Count - 1; rowIndex >= 0; rowIndex--)
            {
                var hasValue = false;
                for (var columnIndex = 0; columnIndex < table.Columns.Count; columnIndex++)
                {
                    var value = Convert.ToString(table.Rows[rowIndex][columnIndex]);
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        hasValue = true;
                        break;
                    }
                }

                if (!hasValue)
                    table.Rows.RemoveAt(rowIndex);
            }
        }

        private static void EnsureColumnNames(DataTable table)
        {
            for (var columnIndex = 0; columnIndex < table.Columns.Count; columnIndex++)
            {
                table.Columns[columnIndex].ColumnName = GetUniqueColumnName(table, table.Columns[columnIndex].ColumnName, columnIndex + 1, columnIndex);
            }
        }

        private static string GetUniqueColumnName(DataTable table, string headerText, int columnNumber, int currentColumnIndex = -1)
        {
            var baseName = string.IsNullOrWhiteSpace(headerText)
                ? $"Column{columnNumber}"
                : headerText.Trim();

            var uniqueName = baseName;
            var suffix = 1;

            while (ColumnNameExists(table, uniqueName, currentColumnIndex))
            {
                uniqueName = $"{baseName}_{suffix}";
                suffix++;
            }

            return uniqueName;
        }

        private static bool ColumnNameExists(DataTable table, string columnName, int currentColumnIndex)
        {
            for (var columnIndex = 0; columnIndex < table.Columns.Count; columnIndex++)
            {
                if (columnIndex == currentColumnIndex)
                    continue;

                if (string.Equals(table.Columns[columnIndex].ColumnName, columnName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }
    }
}
