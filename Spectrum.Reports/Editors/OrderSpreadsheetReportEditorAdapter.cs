using DevExpress.Spreadsheet;
using DevExpress.XtraEditors;
using DevExpress.XtraSpreadsheet;
using Spectrum.Reports.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Spectrum.Reports.Editors
{
    /// <summary>
    /// Spreadsheet editor adapter for project-order adapters.
    /// Editable sheet names are resolved at runtime via <see cref="IEditableSheetProvider"/>
    /// rather than being hard-coded, so this adapter works for any adapter that
    /// declares its editable sheets through that interface.
    /// </summary>
    public sealed class OrderSpreadsheetReportEditorAdapter : ISpreadsheetReportEditorAdapter
    {
        private SpreadsheetControl _spreadsheetControl;
        private IReportAdapter _reportAdapter;
        private HashSet<string> _editableSheets;

        public string Name
        {
            get { return "Order Spreadsheet Adapter"; }
        }

        public bool CanHandle(IReportAdapter reportAdapter)
        {
            return reportAdapter is IEditableSheetProvider;
        }

        public void Initialize(SpreadsheetControl spreadsheetControl, IReportAdapter reportAdapter)
        {
            _spreadsheetControl = spreadsheetControl;
            _reportAdapter = reportAdapter;

            _editableSheets = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var provider = reportAdapter as IEditableSheetProvider;
            if (provider != null)
            {
                foreach (var name in provider.GetEditableSheetNames())
                    _editableSheets.Add(name);
            }
        }

        public bool TryAddRecord()
        {
            if (_spreadsheetControl == null) return false;
            var sheet = _spreadsheetControl.ActiveWorksheet;
            if (sheet == null) return false;

            if (!_editableSheets.Contains(sheet.Name)) return false;

            var used = sheet.GetUsedRange();
            int insertRow = used == null ? 2 : used.BottomRowIndex + 1;
            if (insertRow < 2) insertRow = 2;

            if (ContainsTotalsRow(sheet, used))
                insertRow = Math.Max(2, used.BottomRowIndex);

            sheet.Rows[insertRow].Insert();
            _spreadsheetControl.SelectedCell = sheet.Cells[insertRow, 0];
            return true;
        }

        public bool TryRemoveSelectedRecord()
        {
            if (_spreadsheetControl == null) return false;
            var sheet = _spreadsheetControl.ActiveWorksheet;
            if (sheet == null) return false;
            if (!_editableSheets.Contains(sheet.Name)) return false;

            var selected = _spreadsheetControl.Selection;
            if (selected == null) return false;

            int rowIndex = selected.TopRowIndex;
            if (rowIndex < 2)
            {
                XtraMessageBox.Show("Select a data row first.", "Remove Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }

            if (IsTotalsRow(sheet, rowIndex))
            {
                XtraMessageBox.Show("Totals row cannot be removed.", "Remove Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }

            sheet.Rows.Remove(rowIndex);
            return true;
        }

        public void ApplyA4Layout(Worksheet worksheet)
        {
            if (worksheet == null) return;

            worksheet.ActiveView.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.A4;
            worksheet.PrintOptions.PrintGridlines = true;
            worksheet.PrintOptions.FitToPage = true;
            worksheet.PrintOptions.FitToWidth = 1;
            worksheet.PrintOptions.FitToHeight = 0;

            var margins = worksheet.ActiveView.Margins;
            margins.Left = 0.4f;
            margins.Right = 0.4f;
            margins.Top = 0.5f;
            margins.Bottom = 0.5f;
            margins.Header = 0.3f;
            margins.Footer = 0.3f;

            try
            {
                worksheet.ActiveView.ViewType = WorksheetViewType.PageLayout;
            }
            catch
            {
            }
        }

        private static bool ContainsTotalsRow(Worksheet sheet, CellRange used)
        {
            if (sheet == null || used == null || used.BottomRowIndex < 2) return false;
            return IsTotalsRow(sheet, used.BottomRowIndex);
        }

        private static bool IsTotalsRow(Worksheet sheet, int rowIndex)
        {
            for (int c = 0; c <= 3; c++)
            {
                var text = sheet.Cells[rowIndex, c].DisplayText;
                if (string.Equals(text, "Totals", StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}
