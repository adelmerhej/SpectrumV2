using DevExpress.Spreadsheet;
using DevExpress.XtraEditors;
using DevExpress.XtraSpreadsheet;
using Spectrum.Reports.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Spectrum.Reports.Editors
{
    public sealed class SpreadsheetReportEditorController : IDisposable
    {
        private readonly SpreadsheetControl _spreadsheetControl;
        private IReportAdapter _reportAdapter;
        private ISpreadsheetReportEditorAdapter _activeAdapter;
        private byte[] _checkpoint;
        private bool _applyChangesOnRowsRemoved;

        public SpreadsheetReportEditorController(SpreadsheetControl spreadsheetControl)
        {
            _spreadsheetControl = spreadsheetControl ?? throw new ArgumentNullException(nameof(spreadsheetControl));

            _spreadsheetControl.RowsRemoving += SpreadsheetControl_RowsRemoving;
            _spreadsheetControl.RowsRemoved += SpreadsheetControl_RowsRemoved;
        }

        public void Initialize(IReportAdapter reportAdapter)
        {
            _reportAdapter = reportAdapter;
            _activeAdapter = null;

            if (_reportAdapter == null)
                return;

            _activeAdapter = SpreadsheetReportEditorAdapterRegistry.Resolve(_reportAdapter);
            if (_activeAdapter != null)
                _activeAdapter.Initialize(_spreadsheetControl, _reportAdapter);

            CaptureCheckpoint();
            ApplyA4LayoutToAllSheets();
        }

        public bool TryAddRecord()
        {
            CloseInplaceEditor();
            return _activeAdapter != null && _activeAdapter.TryAddRecord();
        }

        public bool TryRemoveSelectedRecord()
        {
            CloseInplaceEditor();
            return _activeAdapter != null && _activeAdapter.TryRemoveSelectedRecord();
        }

        public void ApplyChanges()
        {
            CloseInplaceEditor();
            // If the adapter declares required worksheets, ensure they exist or prompt the user
            if (_reportAdapter is Spectrum.Reports.Interfaces.IWorksheetRequirements req)
            {
                var required = req.RequiredWorksheetNames() ?? new List<string>();
                var workbook = _spreadsheetControl.Document;
                var missing = required.Where(r => !workbook.Worksheets.Cast<DevExpress.Spreadsheet.Worksheet>().Any(ws => string.Equals(ws.Name, r, StringComparison.OrdinalIgnoreCase))).ToList();
                if (missing.Count > 0)
                {
                    var msg = "The current layout is missing required sheets: " + string.Join(", ", missing) + ".\n\n" +
                        "Choose 'Create' to add empty sheets with the required names, 'Load Default' to replace the workbook with the adapter's default layout, or 'Cancel' to abort saving.";
                    var result = XtraMessageBox.Show(msg, "Missing Sheets", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                    // Map: Yes = Create, No = Load Default, Cancel = abort
                    if (result == DialogResult.Cancel)
                        return;
                    if (result == DialogResult.No)
                    {
                        // Load adapter's default workbook stream (replace current document)
                        try
                        {
                            string format;
                            using (var stream = _reportAdapter.GetSpreadsheetStream(out format))
                            {
                                if (stream != null)
                                {
                                    var docFormat = string.Equals(format, "xlsx", StringComparison.OrdinalIgnoreCase) ? DevExpress.Spreadsheet.DocumentFormat.Xlsx : DevExpress.Spreadsheet.DocumentFormat.Csv;
                                    _spreadsheetControl.LoadDocument(stream, docFormat);
                                }
                            }
                        }
                        catch
                        {
                            XtraMessageBox.Show("Failed to load default layout.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else if (result == DialogResult.Yes)
                    {
                        // Create empty sheets for missing names
                        try
                        {
                            foreach (var name in missing)
                            {
                                // only add if still missing (race)
                                if (!workbook.Worksheets.Cast<DevExpress.Spreadsheet.Worksheet>().Any(ws => string.Equals(ws.Name, name, StringComparison.OrdinalIgnoreCase)))
                                    workbook.Worksheets.Add(name);
                            }
                        }
                        catch
                        {
                            XtraMessageBox.Show("Failed to create sheets.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
            }
            _reportAdapter?.ApplySpreadsheetChanges(_spreadsheetControl.Document);
            _reportAdapter?.Recalculate();
            CaptureCheckpoint();
        }

        public void CancelChanges()
        {
            CloseInplaceEditor();
            if (_checkpoint == null || _checkpoint.Length == 0)
                return;

            using (var stream = new MemoryStream(_checkpoint))
            {
                _spreadsheetControl.LoadDocument(stream, DocumentFormat.Xlsx);
            }

            ApplyA4LayoutToAllSheets();
        }

        public void ApplyA4LayoutToAllSheets()
        {
            var workbook = _spreadsheetControl.Document;
            if (workbook == null) return;

            for (int i = 0; i < workbook.Worksheets.Count; i++)
            {
                var ws = workbook.Worksheets[i];
                if (_activeAdapter != null)
                    _activeAdapter.ApplyA4Layout(ws);
                else
                    ApplyDefaultA4Layout(ws);
            }
        }

        private static void ApplyDefaultA4Layout(Worksheet ws)
        {
            if (ws == null) return;
            ws.ActiveView.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.A4;
            ws.PrintOptions.PrintGridlines = true;
            ws.PrintOptions.FitToPage = true;
            ws.PrintOptions.FitToWidth = 1;
            ws.PrintOptions.FitToHeight = 0;
        }

        private void CaptureCheckpoint()
        {
            using (var stream = new MemoryStream())
            {
                _spreadsheetControl.SaveDocument(stream, DocumentFormat.Xlsx);
                _checkpoint = stream.ToArray();
            }
        }

        private void SpreadsheetControl_RowsRemoving(object sender, RowsChangingEventArgs e)
        {
            if (_activeAdapter == null)
                return;

            var ws = _spreadsheetControl.ActiveWorksheet;
            if (ws == null)
                return;

            if (!string.Equals(ws.Name, "Invoices", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(ws.Name, "Addendums", StringComparison.OrdinalIgnoreCase))
                return;

            var result = XtraMessageBox.Show(
                "Delete selected row(s)?",
                "Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            _applyChangesOnRowsRemoved = result == DialogResult.Yes;
            e.Cancel = result == DialogResult.No;
        }

        private void SpreadsheetControl_RowsRemoved(object sender, RowsChangedEventArgs e)
        {
            if (!_applyChangesOnRowsRemoved)
                return;

            _applyChangesOnRowsRemoved = false;
            ApplyChanges();
        }

        private void CloseInplaceEditor()
        {
            if (_spreadsheetControl.IsCellEditorActive)
                _spreadsheetControl.CloseCellEditor(CellEditorEnterValueMode.Default);
        }

        public void Dispose()
        {
            _spreadsheetControl.RowsRemoving -= SpreadsheetControl_RowsRemoving;
            _spreadsheetControl.RowsRemoved -= SpreadsheetControl_RowsRemoved;
        }
    }
}
