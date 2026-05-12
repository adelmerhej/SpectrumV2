using DevExpress.Spreadsheet;
using System;
using System.Globalization;

namespace Spectrum.Reports.Common
{
    public static class ReportSpreadsheetHelper
    {
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
    }
}
