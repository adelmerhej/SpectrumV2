using DevExpress.Spreadsheet;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraSpreadsheet;
using Spectrum.Reports.Editors;
using Spectrum.Reports.Interfaces;
using Spectrum.Reports.Templates;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Spectrum.Reports.UI
{
    public partial class ReportDesignerControl : XtraUserControl
    {
        private IReportAdapter _currentAdapter;
        private SpreadsheetReportEditorController _editorController;
        // Format painter state
        private bool _formatPainterActive = false;
        private dynamic _formatSourceRange;
        private DevExpress.Spreadsheet.Worksheet _formatSourceWorksheet;

        private bool _isSectionMode;
        private bool _handlingFieldTreeCheck;
        private bool _hasPendingDataChanges;
        private string _currentLayoutName;

        /// <summary>Fires after changes are applied to the adapter model.</summary>
        public event EventHandler ModelChanged;

        public ReportDesignerControl()
        {
            InitializeComponent();
        }

        #region Public API

        /// <summary>
        /// Binds a new adapter: loads the workbook, populates the field list,
        /// initialises the editor controller, and refreshes the summary panel.
        /// </summary>
        public void LoadAdapter(IReportAdapter adapter)
        {
            if (adapter == null) throw new ArgumentNullException(nameof(adapter));

            ClearPreviousAdapter();
            _currentAdapter = adapter;

            _editorController = new SpreadsheetReportEditorController(_spreadsheetControl);

            LoadWorkbook();

            _editorController.Initialize(_currentAdapter);

            BuildFieldList();
            RefreshSummary();
            UpdateButtonStates();
            try { if (this._biFormatPainter != null) this._biFormatPainter.Enabled = true; } catch { }
        }

        /// <summary>
        /// Returns the currently loaded adapter.
        /// </summary>
        public IReportAdapter CurrentAdapter
        {
            get { return _currentAdapter; }
        }

        public bool HasPendingDataChanges
        {
            get { return _hasPendingDataChanges; }
        }

        public void SaveDataChanges()
        {
            if (_editorController == null || _currentAdapter == null)
                return;

            _editorController.ApplyChanges();
            RefreshSummary();
            _hasPendingDataChanges = false;
            ModelChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Workbook Loading

        private void LoadWorkbook()
        {
            if (TryLoadDefaultStoredLayout())
            {
                EnsureSingleWorksheet(false);
                return;
            }

            Stream templateStream;
            if (ReportTemplateRegistry.TryOpenTemplateStream(_currentAdapter.Model, out templateStream))
            {
                using (templateStream)
                {
                    _spreadsheetControl.LoadDocument(templateStream, DocumentFormat.Xlsx);
                }
                EnsureSingleWorksheet(true);
                return;
            }

            string format;
            var stream = _currentAdapter.GetSpreadsheetStream(out format);
            if (stream != null)
            {
                using (stream)
                {
                    var docFormat = string.Equals(format, "xlsx", StringComparison.OrdinalIgnoreCase)
                        ? DocumentFormat.Xlsx
                        : DocumentFormat.Csv;
                    _spreadsheetControl.LoadDocument(stream, docFormat);
                }
                EnsureSingleWorksheet(true);
                return;
            }

            _spreadsheetControl.CreateNewDocument();
            EnsureSingleWorksheet(true);
        }

        private void EnsureSingleWorksheet(bool applyDefaultTitleMerge)
        {
            var doc = _spreadsheetControl.Document;
            if (doc == null || doc.Worksheets.Count == 0) return;

            for (int i = doc.Worksheets.Count - 1; i >= 1; i--)
                doc.Worksheets.RemoveAt(i);

            doc.Worksheets.ActiveWorksheet = doc.Worksheets[0];
            if (string.IsNullOrWhiteSpace(doc.Worksheets[0].Name))
            {
                var sheetName = _currentAdapter?.Title ?? "Report";
                if (sheetName.Length > 31)
                    sheetName = sheetName.Substring(0, 31);
                doc.Worksheets[0].Name = sheetName;
            }

            if (applyDefaultTitleMerge)
            {
                var ws = doc.Worksheets[0];
                ws.Range["A1:F1"].Merge();
            }
        }

        private static string SanitizeFileName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "ReportLayout";
            foreach (var c in Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');
            return name;
        }

        private string GetLayoutRootDirectory()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ReportLayouts");
        }

        private string GetLayoutModelKey()
        {
            var model = _currentAdapter != null ? _currentAdapter.Model : null;
            if (model != null)
                return SanitizeFileName(model.GetType().Name);
            return SanitizeFileName(_currentAdapter?.Title ?? "General");
        }

        private string GetLayoutDirectory()
        {
            return Path.Combine(GetLayoutRootDirectory(), GetLayoutModelKey());
        }

        private string GetDefaultLayoutFile()
        {
            return Path.Combine(GetLayoutDirectory(), "default-layout.txt");
        }

        private bool TryLoadDefaultStoredLayout()
        {
            try
            {
                var marker = GetDefaultLayoutFile();
                if (!File.Exists(marker))
                    return false;

                var defaultName = File.ReadAllText(marker).Trim();
                if (string.IsNullOrWhiteSpace(defaultName))
                    return false;

                var layoutPath = Path.Combine(GetLayoutDirectory(), defaultName + ".xlsx");
                if (!File.Exists(layoutPath))
                    return false;

                _spreadsheetControl.LoadDocument(layoutPath, DocumentFormat.Xlsx);
                _currentLayoutName = defaultName;
                return true;
            }
            catch
            {
                return false;
            }
        }

        private IList<string> GetStoredLayoutNames()
        {
            var folder = GetLayoutDirectory();
            if (!Directory.Exists(folder))
                return new List<string>();

            return Directory.GetFiles(folder, "*.xlsx")
                .Select(Path.GetFileNameWithoutExtension)
                .OrderBy(x => x)
                .ToList();
        }

        private void SaveLayoutTemplate()
        {
            var folder = GetLayoutDirectory();
            Directory.CreateDirectory(folder);

            string layoutName = null;
            if (!string.IsNullOrWhiteSpace(_currentLayoutName))
            {
                var saveActive = XtraMessageBox.Show(
                    "Save changes to active layout '" + _currentLayoutName + "'?",
                    "Save Layout",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (saveActive == DialogResult.Cancel)
                    return;

                if (saveActive == DialogResult.Yes)
                {
                    layoutName = _currentLayoutName;
                }
            }

            if (string.IsNullOrWhiteSpace(layoutName))
            {
                var suggested = string.IsNullOrWhiteSpace(_currentLayoutName)
                        ? SanitizeFileName(_currentAdapter?.Title ?? "Report")
                        : _currentLayoutName;
                var input = XtraInputBox.Show("Layout name:", "Save Layout", suggested) as string;
                if (string.IsNullOrWhiteSpace(input))
                    return;

                layoutName = SanitizeFileName(input.Trim());
                if (string.IsNullOrWhiteSpace(layoutName))
                    return;
            }

            var filePath = Path.Combine(folder, layoutName + ".xlsx");
            _spreadsheetControl.SaveDocument(filePath, DocumentFormat.Xlsx);
            _currentLayoutName = layoutName;
            XtraMessageBox.Show("Layout saved inside the application.", "Save Layout", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LoadLayoutTemplate()
        {
            var names = GetStoredLayoutNames();
            if (names.Count == 0)
            {
                XtraMessageBox.Show("No stored layouts found for this report type.", "Load Layout", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            while (true)
            {
                string selected;
                bool deleteRequested;
                if (!ShowLayoutPicker(names, out selected, out deleteRequested))
                    return;

                var filePath = Path.Combine(GetLayoutDirectory(), selected + ".xlsx");

                if (deleteRequested)
                {
                    var confirm = XtraMessageBox.Show(
                        "Delete layout '" + selected + "'?",
                        "Delete Layout",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);
                    if (confirm == DialogResult.Yes && File.Exists(filePath))
                    {
                        File.Delete(filePath);
                        if (string.Equals(_currentLayoutName, selected, StringComparison.OrdinalIgnoreCase))
                            _currentLayoutName = null;
                        if (File.Exists(GetDefaultLayoutFile()))
                        {
                            var def = File.ReadAllText(GetDefaultLayoutFile()).Trim();
                            if (string.Equals(def, selected, StringComparison.OrdinalIgnoreCase))
                                File.Delete(GetDefaultLayoutFile());
                        }
                    }

                    names = GetStoredLayoutNames();
                    if (names.Count == 0)
                    {
                        XtraMessageBox.Show("No layouts left.", "Load Layout", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    continue;
                }

                _spreadsheetControl.LoadDocument(filePath, DocumentFormat.Xlsx);
                EnsureSingleWorksheet(false);
                _currentLayoutName = selected;
                RefreshSummary();
                return;
            }
        }

        private bool ShowLayoutPicker(IList<string> names, out string selected, out bool deleteRequested)
        {
            selected = null;
            deleteRequested = false;
            string picked = null;
            bool delete = false;

            using (var picker = new XtraForm())
            {
                picker.Text = "Layout Picker";
                picker.StartPosition = FormStartPosition.CenterParent;
                picker.FormBorderStyle = FormBorderStyle.FixedDialog;
                picker.MinimizeBox = false;
                picker.MaximizeBox = false;
                picker.ClientSize = new Size(420, 310);

                var list = new ListBoxControl();
                list.Dock = DockStyle.Fill;
                list.Items.AddRange(names.Cast<object>().ToArray());
                if (list.ItemCount > 0)
                    list.SelectedIndex = 0;

                var btnPanel = new PanelControl();
                btnPanel.Dock = DockStyle.Bottom;
                btnPanel.Height = 50;

                var btnUse = new SimpleButton { Text = "Use", Width = 80, Left = 140, Top = 12 };
                var btnDelete = new SimpleButton { Text = "Delete", Width = 80, Left = 230, Top = 12 };
                var btnCancel = new SimpleButton { Text = "Cancel", Width = 80, Left = 320, Top = 12 };

                btnUse.Click += delegate
                {
                    if (list.SelectedItem == null) return;
                    picked = list.SelectedItem.ToString();
                    delete = false;
                    picker.DialogResult = DialogResult.OK;
                    picker.Close();
                };

                btnDelete.Click += delegate
                {
                    if (list.SelectedItem == null) return;
                    picked = list.SelectedItem.ToString();
                    delete = true;
                    picker.DialogResult = DialogResult.OK;
                    picker.Close();
                };

                btnCancel.Click += delegate
                {
                    picker.DialogResult = DialogResult.Cancel;
                    picker.Close();
                };

                btnPanel.Controls.Add(btnUse);
                btnPanel.Controls.Add(btnDelete);
                btnPanel.Controls.Add(btnCancel);

                picker.Controls.Add(list);
                picker.Controls.Add(btnPanel);

                var result = picker.ShowDialog(FindForm()) == DialogResult.OK;
                if (result)
                {
                    selected = picked;
                    deleteRequested = delete;
                }
                return result;
            }
        }

        private void SetCurrentLayoutAsDefault()
        {
            if (string.IsNullOrWhiteSpace(_currentLayoutName))
            {
                XtraMessageBox.Show("Load or save a layout first.", "Set Default", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var folder = GetLayoutDirectory();
            Directory.CreateDirectory(folder);
            File.WriteAllText(GetDefaultLayoutFile(), _currentLayoutName);
            XtraMessageBox.Show("Default layout updated.", "Set Default", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        #region Field List

        private void BuildFieldList()
        {
            _fieldListTree.BeginUpdate();
            _fieldListTree.Nodes.Clear();

            var descriptors = _currentAdapter?.GetFieldDescriptors();
            if (descriptors == null || descriptors.Count == 0)
            {
                _fieldListTree.EndUpdate();
                return;
            }

            var grouped = new Dictionary<string, TreeNode>();
            foreach (var fd in descriptors)
            {
                string category = fd.Category ?? "General";
                TreeNode catNode;
                if (!grouped.TryGetValue(category, out catNode))
                {
                    catNode = new TreeNode(category)
                    {
                        ImageIndex = 0,
                        SelectedImageIndex = 0
                    };
                    _fieldListTree.Nodes.Add(catNode);
                    grouped[category] = catNode;
                }

                string display = fd.Caption ?? fd.Key;
                if (fd.IsRowLevel)
                    display += string.Format(" [{0} rows]", fd.RowCount);

                var node = new TreeNode(display)
                {
                    Tag = fd,
                    ToolTipText = BuildFieldTooltip(fd)
                };
                catNode.Nodes.Add(node);
            }

            _fieldListTree.EndUpdate();
            _fieldListTree.ExpandAll();
        }

        private static string BuildFieldTooltip(FieldDescriptor fd)
        {
            string tip = fd.Key;
            if (fd.ValueType != null)
                tip += " (" + fd.ValueType.Name + ")";
            if (!string.IsNullOrEmpty(fd.FormatString))
                tip += "  Format: " + fd.FormatString;
            if (fd.IsRowLevel)
                tip += string.Format("  [Row-level: {0} rows]", fd.RowCount);
            return tip;
        }

        private FieldDescriptor GetSelectedFieldDescriptor()
        {
            var node = _fieldListTree.SelectedNode;
            if (node == null) return null;
            return node.Tag as FieldDescriptor;
        }

        private IList<FieldDescriptor> GetCheckedFieldDescriptors()
        {
            var list = new List<FieldDescriptor>();
            foreach (TreeNode category in _fieldListTree.Nodes)
            {
                foreach (TreeNode node in category.Nodes)
                {
                    if (!node.Checked) continue;
                    var fd = node.Tag as FieldDescriptor;
                    if (fd != null)
                        list.Add(fd);
                }
            }
            return list;
        }

        private void OnFieldListAfterCheck(object sender, TreeViewEventArgs e)
        {
            if (_handlingFieldTreeCheck || !_isSectionMode || e.Node == null)
                return;

            try
            {
                _handlingFieldTreeCheck = true;

                if (e.Node.Parent == null)
                {
                    foreach (TreeNode child in e.Node.Nodes)
                        child.Checked = e.Node.Checked;
                }
                else
                {
                    var parent = e.Node.Parent;
                    bool allChecked = true;
                    foreach (TreeNode child in parent.Nodes)
                    {
                        if (!child.Checked)
                        {
                            allChecked = false;
                            break;
                        }
                    }
                    parent.Checked = allChecked;
                }
            }
            finally
            {
                _handlingFieldTreeCheck = false;
            }
        }

        private void OnFieldListDoubleClick(object sender, EventArgs e)
        {
            var fd = GetSelectedFieldDescriptor();
            if (fd == null) return;

            InsertFieldValueOnly(fd);
        }

        #endregion

        #region Field Insertion

        /// <summary>
        /// Inserts only the field value into the currently selected cell.
        /// Row-level fields fill downward from the selected cell.
        /// </summary>
        private void InsertFieldValueOnly(FieldDescriptor fd)
        {
            var ws = _spreadsheetControl.ActiveWorksheet;
            if (ws == null) return;

            var cell = _spreadsheetControl.SelectedCell;
            if (cell == null) return;

            if (fd.IsRowLevel && fd.RowCount > 0)
            {
                for (int i = 0; i < fd.RowCount; i++)
                {
                    object val = fd.GetRowValue(i);
                    SetCellValue(ws, cell.TopRowIndex + i, cell.LeftColumnIndex, val, fd.FormatString);
                }
            }
            else if (fd.IsRowLevel)
            {
                SetCellValue(ws, cell.TopRowIndex, cell.LeftColumnIndex, "(no data)", null);
            }
            else
            {
                object val = fd.GetValue != null ? fd.GetValue() : null;
                SetCellValue(ws, cell.TopRowIndex, cell.LeftColumnIndex, val, fd.FormatString);
            }
        }

        /// <summary>
        /// Inserts the field name (bold label) in the selected cell and the value
        /// in the cell immediately to the right (horizontal layout).
        /// Row-level fields repeat for each row with the label in the first row.
        /// When a row-level field has 0 rows, the label is placed with an empty value.
        /// </summary>
        private void InsertFieldHorizontal(FieldDescriptor fd)
        {
            var ws = _spreadsheetControl.ActiveWorksheet;
            if (ws == null) return;

            var cell = _spreadsheetControl.SelectedCell;
            if (cell == null) return;

            string label = fd.Caption ?? fd.Key;
            int row = cell.TopRowIndex;
            int col = cell.LeftColumnIndex;

            if (fd.IsRowLevel && fd.RowCount > 0)
            {
                for (int i = 0; i < fd.RowCount; i++)
                {
                    SetLabelCell(ws, row + i, col, i == 0 ? label : string.Empty);
                    object val = fd.GetRowValue(i);
                    SetCellValue(ws, row + i, col + 1, val, fd.FormatString);
                }
            }
            else
            {
                SetLabelCell(ws, row, col, label);
                object val = (fd.GetValue != null) ? fd.GetValue() : null;
                SetCellValue(ws, row, col + 1, val, fd.FormatString);
            }
        }

        /// <summary>
        /// Inserts the field name (bold label) in the selected cell and the value
        /// in the cell directly below (vertical layout).
        /// Row-level fields place the label above and values flowing downward.
        /// </summary>
        private void InsertFieldVertical(FieldDescriptor fd)
        {
            var ws = _spreadsheetControl.ActiveWorksheet;
            if (ws == null) return;

            var cell = _spreadsheetControl.SelectedCell;
            if (cell == null) return;

            string label = fd.Caption ?? fd.Key;
            int row = cell.TopRowIndex;
            int col = cell.LeftColumnIndex;

            SetLabelCell(ws, row, col, label);
            if (fd.IsRowLevel && fd.RowCount > 0)
            {
                for (int i = 0; i < fd.RowCount; i++)
                {
                    object val = fd.GetRowValue(i);
                    SetCellValue(ws, row + 1 + i, col, val, fd.FormatString);
                }
            }
            else
            {
                object val = (fd.GetValue != null) ? fd.GetValue() : null;
                SetCellValue(ws, row + 1, col, val, fd.FormatString);
            }
        }

        // Format Painter: copy formatting from a source selection to target selection.
        private void OnFormatPainterClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                var doc = _spreadsheetControl?.Document;
                if (doc == null) return;

                if (!_formatPainterActive)
                {
                    // start painter: store current selection as source
                    var sel = _spreadsheetControl.Selection;
                    if (sel == null || sel.RowCount == 0 || sel.ColumnCount == 0) return;
                    _formatSourceRange = sel;
                    _formatSourceWorksheet = sel.Worksheet;
                    _formatPainterActive = true;
                    // change cursor to indicate painter
                    _spreadsheetControl.Cursor = Cursors.Cross;
                }
                else
                {
                    // cancel
                    _formatPainterActive = false;
                    _formatSourceRange = null;
                    _formatSourceWorksheet = null;
                    _spreadsheetControl.Cursor = Cursors.Default;
                }
            }
            catch
            {
                // ignore
            }
        }

        private void OnSpreadsheetSelectionChanged(object sender, EventArgs e)
        {
            if (!_formatPainterActive || _formatSourceRange == null) return;
            try
            {
                var target = _spreadsheetControl.Selection;
                if (target == null || target.RowCount == 0 || target.ColumnCount == 0) return;

                // if target is the same as source, ignore
                if (target.Worksheet == _formatSourceWorksheet && target.TopRowIndex == _formatSourceRange.TopRowIndex && target.LeftColumnIndex == _formatSourceRange.LeftColumnIndex && target.RowCount == _formatSourceRange.RowCount && target.ColumnCount == _formatSourceRange.ColumnCount)
                {
                    return;
                }

                ApplyFormatting(_formatSourceRange, target);
            }
            finally
            {
                // one-shot painter behavior: reset
                _formatPainterActive = false;
                _formatSourceRange = null;
                _formatSourceWorksheet = null;
                _spreadsheetControl.Cursor = Cursors.Default;
                // uncheck ribbon button if available
                try { if (this._biFormatPainter != null) this._biFormatPainter.Down = false; } catch { }
            }
        }

        private void ApplyFormatting(dynamic source, dynamic target)
        {
            if (source == null || target == null) return;
            var doc = _spreadsheetControl.Document;
            doc.BeginUpdate();
            try
            {
                int srcRows = source.RowCount;
                int srcCols = source.ColumnCount;
                for (int r = 0; r < target.RowCount; r++)
                {
                    for (int c = 0; c < target.ColumnCount; c++)
                    {
                        var srcCell = source.Worksheet.Cells[source.TopRowIndex + (r % srcRows), source.LeftColumnIndex + (c % srcCols)];
                        var dstCell = target.Worksheet.Cells[target.TopRowIndex + r, target.LeftColumnIndex + c];
                        // copy style
                        try
                        {
                            dstCell.Style = srcCell.Style;
                        }
                        catch
                        {
                            // fallback: copy specific properties
                            dstCell.Font.Assign(srcCell.Font);
                            dstCell.Fill.Assign(srcCell.Fill);
                            dstCell.Borders.Assign(srcCell.Borders);
                            dstCell.Alignment.Horizontal = srcCell.Alignment.Horizontal;
                            dstCell.Alignment.Vertical = srcCell.Alignment.Vertical;
                            dstCell.NumberFormat = srcCell.NumberFormat;
                        }
                    }
                }
            }
            finally
            {
                doc.EndUpdate();
            }
        }

        private static void SetLabelCell(Worksheet ws, int row, int col, string label)
        {
            ws.Cells[row, col].Value = label;
            ws.Cells[row, col].Font.Bold = true;
            ws.Cells[row, col].Font.Color = Color.FromArgb(0, 51, 102);
        }

        private static void SetCellValue(Worksheet ws, int row, int col, object value, string formatString)
        {
            if (value == null)
            {
                ws.Cells[row, col].Value = string.Empty;
                return;
            }

            if (value is DateTime dt)
            {
                ws.Cells[row, col].Value = dt;
                ws.Cells[row, col].NumberFormat = !string.IsNullOrEmpty(formatString) ? formatString : "yyyy-mm-dd";
            }
            else if (value is decimal dec)
            {
                ws.Cells[row, col].Value = (double)dec;
                if (!string.IsNullOrEmpty(formatString))
                    ws.Cells[row, col].NumberFormat = formatString == "N2" ? "#,##0.00" : formatString;
            }
            else if (value is double dbl)
            {
                ws.Cells[row, col].Value = dbl;
                if (!string.IsNullOrEmpty(formatString))
                    ws.Cells[row, col].NumberFormat = formatString;
            }
            else if (value is int intVal)
            {
                ws.Cells[row, col].Value = intVal;
            }
            else if (value is bool boolVal)
            {
                ws.Cells[row, col].Value = boolVal ? "Yes" : "No";
            }
            else
            {
                ws.Cells[row, col].Value = value.ToString();
            }
        }

        #endregion

        #region Summary Panel

        private void RefreshSummary()
        {
            _summaryPanel.Controls.Clear();

            var fields = _currentAdapter?.GetSummaryFields();
            if (fields == null) return;

            foreach (var kvp in fields)
            {
                var lbl = new LabelControl
                {
                    Text = kvp.Key + ": " + kvp.Value,
                    AutoSizeMode = LabelAutoSizeMode.Default,
                    Padding = new Padding(8, 2, 8, 2)
                };
                _summaryPanel.Controls.Add(lbl);
            }
        }

        #endregion

        #region Report Builder Ribbon Handlers

        private void OnInsertValueClick(object sender, ItemClickEventArgs e)
        {
            var fd = GetSelectedFieldDescriptor();
            if (fd == null)
            {
                XtraMessageBox.Show("Select a field from the Field List panel first.",
                    "Insert Value", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            InsertFieldValueOnly(fd);
        }

        private void OnInsertHorizontalClick(object sender, ItemClickEventArgs e)
        {
            if (_isSectionMode)
            {
                InsertCheckedFieldsAsSection(true);
                return;
            }

            var fd = GetSelectedFieldDescriptor();
            if (fd == null)
            {
                XtraMessageBox.Show("Select a field from the Field List panel first.",
                    "Insert Field", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            InsertFieldHorizontal(fd);
        }

        private void OnInsertVerticalClick(object sender, ItemClickEventArgs e)
        {
            if (_isSectionMode)
            {
                InsertCheckedFieldsAsSection(false);
                return;
            }

            var fd = GetSelectedFieldDescriptor();
            if (fd == null)
            {
                XtraMessageBox.Show("Select a field from the Field List panel first.",
                    "Insert Field", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            InsertFieldVertical(fd);
        }

        private void OnBestFitClick(object sender, ItemClickEventArgs e)
        {
            var ws = _spreadsheetControl.ActiveWorksheet;
            if (ws == null) return;

            var used = ws.GetUsedRange();
            if (used != null)
                ws.Columns.AutoFit(used.LeftColumnIndex, used.RightColumnIndex);
        }

        private void OnApplyA4LayoutClick(object sender, ItemClickEventArgs e)
        {
            if (_editorController != null)
                _editorController.ApplyA4LayoutToAllSheets();
        }

        private void OnToggleSectionModeClick(object sender, ItemClickEventArgs e)
        {
            _isSectionMode = !_isSectionMode;
            _biSectionMode.Caption = _isSectionMode ? "Section Mode: ON" : "Add Section Mode";
            _fieldListTree.CheckBoxes = _isSectionMode;

            if (!_isSectionMode)
            {
                foreach (TreeNode category in _fieldListTree.Nodes)
                {
                    category.Checked = false;
                    foreach (TreeNode node in category.Nodes)
                        node.Checked = false;
                }
            }
        }

        private void InsertCheckedFieldsAsSection(bool horizontal)
        {
            var checkedFields = GetCheckedFieldDescriptors();
            if (checkedFields.Count == 0)
            {
                XtraMessageBox.Show("Section mode is ON. Check one or more fields in the field list.",
                    "Add Section", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var ws = _spreadsheetControl.ActiveWorksheet;
            var cell = _spreadsheetControl.SelectedCell;
            if (ws == null || cell == null) return;

            int row = cell.TopRowIndex;
            int col = cell.LeftColumnIndex;

            foreach (var fd in checkedFields)
            {
                if (horizontal)
                {
                    InsertFieldHorizontalAt(fd, row, col);
                    int usedRows = (fd.IsRowLevel && fd.RowCount > 1) ? fd.RowCount : 1;
                    row += usedRows;
                }
                else
                {
                    InsertFieldVerticalAt(fd, row, col);
                    col += 1;
                }
            }
        }

        private void InsertFieldHorizontalAt(FieldDescriptor fd, int row, int col)
        {
            var ws = _spreadsheetControl.ActiveWorksheet;
            if (ws == null) return;

            string label = fd.Caption ?? fd.Key;
            if (fd.IsRowLevel && fd.RowCount > 0)
            {
                for (int i = 0; i < fd.RowCount; i++)
                {
                    SetLabelCell(ws, row + i, col, i == 0 ? label : string.Empty);
                    SetCellValue(ws, row + i, col + 1, fd.GetRowValue(i), fd.FormatString);
                }
            }
            else
            {
                SetLabelCell(ws, row, col, label);
                var val = (fd.GetValue != null) ? fd.GetValue() : null;
                SetCellValue(ws, row, col + 1, val, fd.FormatString);
            }
        }

        private void InsertFieldVerticalAt(FieldDescriptor fd, int row, int col)
        {
            var ws = _spreadsheetControl.ActiveWorksheet;
            if (ws == null) return;

            string label = fd.Caption ?? fd.Key;
            SetLabelCell(ws, row, col, label);
            if (fd.IsRowLevel && fd.RowCount > 0)
            {
                for (int i = 0; i < fd.RowCount; i++)
                    SetCellValue(ws, row + 1 + i, col, fd.GetRowValue(i), fd.FormatString);
            }
            else
            {
                var val = (fd.GetValue != null) ? fd.GetValue() : null;
                SetCellValue(ws, row + 1, col, val, fd.FormatString);
            }
        }

        private void OnToggleFieldListClick(object sender, ItemClickEventArgs e)
        {
            _splitContainer.Panel1Collapsed = !_splitContainer.Panel1Collapsed;
        }

        private void OnSaveLayoutClick(object sender, ItemClickEventArgs e)
        {
            SaveLayoutTemplate();
        }

        private void OnLoadLayoutClick(object sender, ItemClickEventArgs e)
        {
            LoadLayoutTemplate();
        }

        private void OnSetDefaultLayoutClick(object sender, ItemClickEventArgs e)
        {
            SetCurrentLayoutAsDefault();
        }

        private void OnSaveDataClick(object sender, ItemClickEventArgs e)
        {
            SaveDataChanges();
        }

        private void OnSpreadsheetActiveSheetChanged(object sender, DevExpress.Spreadsheet.ActiveSheetChangedEventArgs e)
        {
            RefreshSummary();
        }

        private void OnSpreadsheetCellValueChanged(object sender, SpreadsheetCellEventArgs e)
        {
            MarkEditableSheetAsDirty(e.Worksheet);
        }

        private void OnSpreadsheetRowsInserted(object sender, RowsChangedEventArgs e)
        {
            MarkEditableSheetAsDirty(_spreadsheetControl.ActiveWorksheet);
        }

        private void MarkEditableSheetAsDirty(Worksheet worksheet)
        {
            if (worksheet == null)
                return;

            if (!string.Equals(worksheet.Name, "Invoices", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(worksheet.Name, "Addendums", StringComparison.OrdinalIgnoreCase))
                return;

            _hasPendingDataChanges = true;
        }

        #endregion

        #region Button State Management

        private void UpdateButtonStates()
        {
            bool hasAdapter = _currentAdapter != null;
            _biInsertValue.Enabled = hasAdapter;
            _biInsertHorizontal.Enabled = hasAdapter;
            _biInsertVertical.Enabled = hasAdapter;
            _biSectionMode.Enabled = hasAdapter;
            _biBestFit.Enabled = hasAdapter;
            _biApplyA4.Enabled = hasAdapter;
            _biSaveLayout.Enabled = hasAdapter;
            _biLoadLayout.Enabled = hasAdapter;
            _biSetDefaultLayout.Enabled = hasAdapter;
            _biSaveData.Enabled = hasAdapter;
            _biToggleFieldList.Enabled = hasAdapter;
        }

        #endregion

        #region Cleanup

        private void ClearPreviousAdapter()
        {
            if (_editorController != null)
            {
                _editorController.Dispose();
                _editorController = null;
            }

            _currentAdapter?.Dispose();
            _currentAdapter = null;
            _currentLayoutName = null;
            _isSectionMode = false;
            _hasPendingDataChanges = false;

            _fieldListTree.Nodes.Clear();
            _summaryPanel.Controls.Clear();
        }

        #endregion
    }
}
