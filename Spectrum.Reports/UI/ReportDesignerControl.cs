using DevExpress.Spreadsheet;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraSpreadsheet;
using DevExpress.XtraTab;
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
        private CellRange _formatSourceRange;
        private DevExpress.Spreadsheet.Worksheet _formatSourceWorksheet;

        private bool _isSectionMode;
        private bool _handlingFieldTreeCheck;
        private bool _hasPendingDataChanges;
        private string _currentLayoutName;
        // _recordSelectorVisible is no longer the gating flag; the tab control handles visibility.

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
            BuildRecordSelector();
            RefreshSummary();
            UpdateButtonStates();
            try { if (this._biFormatPainter != null) this._biFormatPainter.Enabled = true; } catch { }
        }

        /// <summary>Returns the currently loaded adapter.</summary>
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

        #region Record Selector

        /// <summary>
        /// Populates the record selector panel when the adapter supports IMultiRecordReportAdapter.
        /// When the adapter is single-record only the panel stays hidden.
        /// </summary>
        private void BuildRecordSelector()
        {
            _recordSelectorList.ItemCheck -= OnRecordSelectorItemCheck;
            _recordSelectorList.Items.Clear();
            _activeRecordCombo.Properties.Items.Clear();
            _activeRecordCombo.SelectedIndexChanged -= OnActiveRecordChanged;

            var multi = _currentAdapter as IMultiRecordReportAdapter;
            if (multi == null)
            {
                _biToggleRecordSelector.Enabled = false;
                _biApplyRecordSelection.Enabled = false;
                _tabRecords.PageVisible = false;
                return;
            }
            _tabRecords.PageVisible = true;

            var available = multi.GetAvailableRecords();
            var selected = multi.GetSelectedRecords();
            var active = multi.GetActiveRecord();
            var selectedKeys = new HashSet<string>(
                selected.Select(x => x.Key),
                StringComparer.OrdinalIgnoreCase);

            foreach (var rec in available)
            {
                bool isChecked = selectedKeys.Contains(rec.Key);
                _recordSelectorList.Items.Add(rec, isChecked ? CheckState.Checked : CheckState.Unchecked);
                _activeRecordCombo.Properties.Items.Add(rec);
            }

            // Set active selection in combo
            if (active != null)
            {
                for (int i = 0; i < _activeRecordCombo.Properties.Items.Count; i++)
                {
                    var item = _activeRecordCombo.Properties.Items[i] as ReportRecordDescriptor;
                    if (item != null && string.Equals(item.Key, active.Key, StringComparison.OrdinalIgnoreCase))
                    {
                        _activeRecordCombo.SelectedIndex = i;
                        break;
                    }
                }
            }

            _recordSelectorList.ItemCheck += OnRecordSelectorItemCheck;
            _activeRecordCombo.SelectedIndexChanged += OnActiveRecordChanged;
            _biToggleRecordSelector.Enabled = true;
            _biApplyRecordSelection.Enabled = true;
        }

        private void OnRecordSelectorItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            // Keep the active-record combo in sync: if the newly checked item is
            // the only checked one, promote it to active automatically.
            // Actual adapter update happens when Apply is clicked.
            var checkedItems = GetCheckedRecordDescriptors();
            if (checkedItems.Count == 1)
            {
                var only = checkedItems[0];
                for (int i = 0; i < _activeRecordCombo.Properties.Items.Count; i++)
                {
                    var item = _activeRecordCombo.Properties.Items[i] as ReportRecordDescriptor;
                    if (item != null && string.Equals(item.Key, only.Key, StringComparison.OrdinalIgnoreCase))
                    {
                        _activeRecordCombo.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        private void OnActiveRecordChanged(object sender, EventArgs e)
        {
            // Auto-ensure active record is checked
            var activeDescriptor = _activeRecordCombo.SelectedItem as ReportRecordDescriptor;
            if (activeDescriptor == null) return;

            for (int i = 0; i < _recordSelectorList.Items.Count; i++)
            {
                var item = _recordSelectorList.Items[i].Value as ReportRecordDescriptor;
                if (item != null && string.Equals(item.Key, activeDescriptor.Key, StringComparison.OrdinalIgnoreCase))
                {
                    if (_recordSelectorList.Items[i].CheckState != CheckState.Checked)
                        _recordSelectorList.Items[i].CheckState = CheckState.Checked;
                    break;
                }
            }
        }

        private IList<ReportRecordDescriptor> GetCheckedRecordDescriptors()
        {
            var list = new List<ReportRecordDescriptor>();
            for (int i = 0; i < _recordSelectorList.Items.Count; i++)
            {
                if (_recordSelectorList.Items[i].CheckState == CheckState.Checked)
                {
                    var desc = _recordSelectorList.Items[i].Value as ReportRecordDescriptor;
                    if (desc != null)
                        list.Add(desc);
                }
            }
            return list;
        }

        private void ApplyRecordSelection()
        {
            var multi = _currentAdapter as IMultiRecordReportAdapter;
            if (multi == null) return;

            var checkedItems = GetCheckedRecordDescriptors();
            if (checkedItems.Count == 0)
            {
                XtraMessageBox.Show("At least one record must be selected.",
                    "Select Records", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var activeDescriptor = _activeRecordCombo.SelectedItem as ReportRecordDescriptor;
            if (activeDescriptor == null)
                activeDescriptor = checkedItems[0];

            var selectedKeys = checkedItems.Select(x => x.Key).ToList();
            multi.UpdateRecordSelection(selectedKeys, activeDescriptor.Key);

            BuildFieldList();
            RefreshSummary();

            XtraMessageBox.Show(
                checkedItems.Count == 1
                    ? "Active record: " + activeDescriptor.Caption
                    : checkedItems.Count + " records selected. Active: " + activeDescriptor.Caption,
                "Record Selection Applied", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void OnToggleRecordSelectorClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the Records tab (or back to Fields if already there)
            bool goToRecords = _leftTabControl.SelectedTabPage != _tabRecords;
            _leftTabControl.SelectedTabPage = goToRecords ? _tabRecords : _tabFields;
            _biToggleRecordSelector.Down = goToRecords;
            if (!_splitContainer.Panel1Collapsed)
                return;
            // Ensure the side panel is visible when navigating
            _splitContainer.Panel1Collapsed = false;
            _biToggleFieldList.Down = true;
        }

        private void OnApplyRecordSelectionClick(object sender, ItemClickEventArgs e)
        {
            ApplyRecordSelection();
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

        /// <summary>
        /// Returns the fields to act on for an insert-value operation:
        /// checked fields when any are checked (section / multi-field mode),
        /// otherwise the single selected node.
        /// </summary>
        private IList<FieldDescriptor> ResolveTargetFields()
        {
            var checked_ = GetCheckedFieldDescriptors();
            if (checked_.Count > 0) return checked_;

            var selected = GetSelectedFieldDescriptor();
            if (selected != null)
                return new List<FieldDescriptor> { selected };

            return new List<FieldDescriptor>();
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

        #region Multi-Record Helpers

        /// <summary>
        /// Returns the count of currently-selected records when the adapter supports
        /// IMultiRecordReportAdapter; returns 1 for single-record adapters.
        /// </summary>
        private int GetSelectedRecordCount()
        {
            var multi = _currentAdapter as IMultiRecordReportAdapter;
            if (multi == null) return 1;
            return multi.GetSelectedRecords().Count;
        }

        #endregion

        #region Field Insertion

        /// <summary>
        /// Inserts only the field value(s) into the currently selected cell.
        /// • Single record: existing behavior preserved.
        /// • Multiple records: shows a choice dialog (horizontal / vertical).
        /// </summary>
        private void InsertFieldValueOnly(FieldDescriptor fd)
        {
            if (GetSelectedRecordCount() > 1 && fd.HasSelectedRecordValues)
            {
                // Ask the user for direction when multiple records are selected
                var choice = XtraMessageBox.Show(
                    "Multiple records are selected.\n\nInsert values Horizontally (columns) or Vertically (rows)?",
                    "Insert Value Direction",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (choice == DialogResult.Yes)
                    InsertMultiValueHorizontal(fd);
                else if (choice == DialogResult.No)
                    InsertMultiValueVertical(fd);
                return;
            }
            // Fall through to single-record path when no multi-record values available

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
        /// Multi-record: insert one value per selected record, going right (?).
        /// Grouped: project-first then fields per project.
        /// </summary>
        private void InsertMultiValueHorizontal(FieldDescriptor fd)
        {
            var ws = _spreadsheetControl.ActiveWorksheet;
            if (ws == null) return;
            var cell = _spreadsheetControl.SelectedCell;
            if (cell == null) return;

            int startRow = cell.TopRowIndex;
            int startCol = cell.LeftColumnIndex;

            if (!fd.HasSelectedRecordValues)
            {
                // Single-record fallback: insert value(s) at selected cell going right
                if (fd.IsRowLevel && fd.RowCount > 0)
                {
                    for (int i = 0; i < fd.RowCount; i++)
                        SetCellValue(ws, startRow, startCol + i, fd.GetRowValue(i), fd.FormatString);
                }
                else
                {
                    object val = fd.GetValue != null ? fd.GetValue() : null;
                    SetCellValue(ws, startRow, startCol, val, fd.FormatString);
                }
                return;
            }

            var multi = _currentAdapter as IMultiRecordReportAdapter;
            int recordCount = multi != null ? multi.GetSelectedRecords().Count : 1;

            if (fd.IsRowLevel)
            {
                // Each record occupies a block of rows; records go column-by-column
                for (int si = 0; si < recordCount; si++)
                {
                    int rowCount = fd.GetSelectedRowCount != null ? fd.GetSelectedRowCount(si) : 0;
                    for (int ri = 0; ri < rowCount; ri++)
                    {
                        object val = fd.GetSelectedRowValue != null ? fd.GetSelectedRowValue(si, ri) : null;
                        SetCellValue(ws, startRow + ri, startCol + si, val, fd.FormatString);
                    }
                }
            }
            else
            {
                for (int si = 0; si < recordCount; si++)
                {
                    object val = fd.GetSelectedValue != null ? fd.GetSelectedValue(si) : null;
                    SetCellValue(ws, startRow, startCol + si, val, fd.FormatString);
                }
            }
        }

        /// <summary>
        /// Multi-record: insert one value per selected record, going down (?).
        /// Grouped: project-first then fields per project.
        /// </summary>
        private void InsertMultiValueVertical(FieldDescriptor fd)
        {
            var ws = _spreadsheetControl.ActiveWorksheet;
            if (ws == null) return;
            var cell = _spreadsheetControl.SelectedCell;
            if (cell == null) return;

            int startRow = cell.TopRowIndex;
            int startCol = cell.LeftColumnIndex;

            if (!fd.HasSelectedRecordValues)
            {
                // Single-record fallback: insert value(s) at selected cell going down
                if (fd.IsRowLevel && fd.RowCount > 0)
                {
                    for (int i = 0; i < fd.RowCount; i++)
                        SetCellValue(ws, startRow + i, startCol, fd.GetRowValue(i), fd.FormatString);
                }
                else
                {
                    object val = fd.GetValue != null ? fd.GetValue() : null;
                    SetCellValue(ws, startRow, startCol, val, fd.FormatString);
                }
                return;
            }

            var multi = _currentAdapter as IMultiRecordReportAdapter;
            int recordCount = multi != null ? multi.GetSelectedRecords().Count : 1;

            if (fd.IsRowLevel)
            {
                int colOffset = 0;
                for (int si = 0; si < recordCount; si++)
                {
                    int rowCount = fd.GetSelectedRowCount != null ? fd.GetSelectedRowCount(si) : 0;
                    for (int ri = 0; ri < rowCount; ri++)
                    {
                        object val = fd.GetSelectedRowValue != null ? fd.GetSelectedRowValue(si, ri) : null;
                        SetCellValue(ws, startRow + ri, startCol + colOffset, val, fd.FormatString);
                    }
                    colOffset++;
                }
            }
            else
            {
                for (int si = 0; si < recordCount; si++)
                {
                    object val = fd.GetSelectedValue != null ? fd.GetSelectedValue(si) : null;
                    SetCellValue(ws, startRow + si, startCol, val, fd.FormatString);
                }
            }
        }

        // ?? Value-only positional helpers ????????????????????????????????????????
        // These mirror InsertFieldHorizontalAt / InsertFieldVerticalAt but write
        // only the value (no label column).  They accept an explicit (row, col)
        // anchor so the caller can advance the cursor across multiple fields.

        /// <summary>
        /// Writes only the value(s) for <paramref name="fd"/> at the given anchor,
        /// going right across columns for multi-record scalar fields.
        /// Returns the number of rows consumed so the caller can advance vertically.
        /// </summary>
        private int InsertValueOnlyHorizontalAt(FieldDescriptor fd, int row, int col, int recordCount)
        {
            var ws = _spreadsheetControl.ActiveWorksheet;
            if (ws == null) return 1;

            if (recordCount > 1 && fd.HasSelectedRecordValues)
            {
                if (fd.IsRowLevel)
                {
                    int maxRows = 0;
                    for (int si = 0; si < recordCount; si++)
                    {
                        int rc = fd.GetSelectedRowCount != null ? fd.GetSelectedRowCount(si) : 0;
                        if (rc > maxRows) maxRows = rc;
                    }
                    for (int ri = 0; ri < maxRows; ri++)
                        for (int si = 0; si < recordCount; si++)
                        {
                            object val = fd.GetSelectedRowValue != null ? fd.GetSelectedRowValue(si, ri) : null;
                            SetCellValue(ws, row + ri, col + si, val, fd.FormatString);
                        }
                    return maxRows > 0 ? maxRows : 1;
                }
                else
                {
                    for (int si = 0; si < recordCount; si++)
                    {
                        object val = fd.GetSelectedValue != null ? fd.GetSelectedValue(si) : null;
                        SetCellValue(ws, row, col + si, val, fd.FormatString);
                    }
                    return 1;
                }
            }

            // Single-record / no multi-record values
            if (fd.IsRowLevel && fd.RowCount > 0)
            {
                for (int i = 0; i < fd.RowCount; i++)
                    SetCellValue(ws, row + i, col, fd.GetRowValue(i), fd.FormatString);
                return fd.RowCount;
            }

            object sval = fd.GetValue != null ? fd.GetValue() : null;
            SetCellValue(ws, row, col, sval, fd.FormatString);
            return 1;
        }

        /// <summary>
        /// Writes only the value(s) for <paramref name="fd"/> at the given anchor,
        /// going down rows for multi-record scalar fields.
        /// Returns the number of columns consumed so the caller can advance horizontally.
        /// </summary>
        private int InsertValueOnlyVerticalAt(FieldDescriptor fd, int row, int col, int recordCount)
        {
            var ws = _spreadsheetControl.ActiveWorksheet;
            if (ws == null) return 1;

            if (recordCount > 1 && fd.HasSelectedRecordValues)
            {
                if (fd.IsRowLevel)
                {
                    int maxRows = 0;
                    for (int si = 0; si < recordCount; si++)
                    {
                        int rc = fd.GetSelectedRowCount != null ? fd.GetSelectedRowCount(si) : 0;
                        if (rc > maxRows) maxRows = rc;
                    }
                    for (int ri = 0; ri < maxRows; ri++)
                        for (int si = 0; si < recordCount; si++)
                        {
                            object val = fd.GetSelectedRowValue != null ? fd.GetSelectedRowValue(si, ri) : null;
                            SetCellValue(ws, row + ri, col + si, val, fd.FormatString);
                        }
                    // occupies recordCount columns
                    return recordCount;
                }
                else
                {
                    for (int si = 0; si < recordCount; si++)
                    {
                        object val = fd.GetSelectedValue != null ? fd.GetSelectedValue(si) : null;
                        SetCellValue(ws, row + si, col, val, fd.FormatString);
                    }
                    return 1;
                }
            }

            // Single-record / no multi-record values
            if (fd.IsRowLevel && fd.RowCount > 0)
            {
                for (int i = 0; i < fd.RowCount; i++)
                    SetCellValue(ws, row + i, col, fd.GetRowValue(i), fd.FormatString);
                return 1;
            }

            object sval = fd.GetValue != null ? fd.GetValue() : null;
            SetCellValue(ws, row, col, sval, fd.FormatString);
            return 1;
        }

        // ?????????????????????????????????????????????????????????????????????????

        /// <summary>
        /// Inserts the field label (bold) + value(s) in horizontal layout.
        /// • Single record: label | value
        /// • Multiple records: label | value? | value? | …
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
            int recordCount = GetSelectedRecordCount();

            if (recordCount > 1 && fd.HasSelectedRecordValues)
            {
                // Label once, then one value per record going right
                if (fd.IsRowLevel)
                {
                    int maxRows = 0;
                    for (int si = 0; si < recordCount; si++)
                    {
                        int rc = fd.GetSelectedRowCount != null ? fd.GetSelectedRowCount(si) : 0;
                        if (rc > maxRows) maxRows = rc;
                    }
                    for (int ri = 0; ri < maxRows; ri++)
                    {
                        SetLabelCell(ws, row + ri, col, ri == 0 ? label : string.Empty);
                        for (int si = 0; si < recordCount; si++)
                        {
                            object val = fd.GetSelectedRowValue != null ? fd.GetSelectedRowValue(si, ri) : null;
                            SetCellValue(ws, row + ri, col + 1 + si, val, fd.FormatString);
                        }
                    }
                }
                else
                {
                    SetLabelCell(ws, row, col, label);
                    for (int si = 0; si < recordCount; si++)
                    {
                        object val = fd.GetSelectedValue != null ? fd.GetSelectedValue(si) : null;
                        SetCellValue(ws, row, col + 1 + si, val, fd.FormatString);
                    }
                }
                return;
            }

            // Single-record path (original behavior)
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
        /// Inserts the field label (bold) + value(s) in vertical layout.
        /// • Single record: label / value
        /// • Multiple records: label / value? / value? / …
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
            int recordCount = GetSelectedRecordCount();

            if (recordCount > 1 && fd.HasSelectedRecordValues)
            {
                // Label once, then one value per record going down
                SetLabelCell(ws, row, col, label);
                if (fd.IsRowLevel)
                {
                    int offset = 1;
                    for (int si = 0; si < recordCount; si++)
                    {
                        int rc = fd.GetSelectedRowCount != null ? fd.GetSelectedRowCount(si) : 0;
                        for (int ri = 0; ri < rc; ri++)
                        {
                            object val = fd.GetSelectedRowValue != null ? fd.GetSelectedRowValue(si, ri) : null;
                            SetCellValue(ws, row + offset, col, val, fd.FormatString);
                            offset++;
                        }
                    }
                }
                else
                {
                    for (int si = 0; si < recordCount; si++)
                    {
                        object val = fd.GetSelectedValue != null ? fd.GetSelectedValue(si) : null;
                        SetCellValue(ws, row + 1 + si, col, val, fd.FormatString);
                    }
                }
                return;
            }

            // Single-record path (original behavior)
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

        // Format Painter
        private void OnFormatPainterClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                var doc = _spreadsheetControl?.Document;
                if (doc == null) return;

                if (!_formatPainterActive)
                {
                    var sel = _spreadsheetControl.Selection;
                    if (sel == null || sel.RowCount == 0 || sel.ColumnCount == 0) return;
                    _formatSourceRange = sel;
                    _formatSourceWorksheet = sel.Worksheet;
                    _formatPainterActive = true;
                    _spreadsheetControl.Cursor = Cursors.Cross;
                }
                else
                {
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

                if (target.Worksheet == _formatSourceWorksheet
                    && target.TopRowIndex == _formatSourceRange.TopRowIndex
                    && target.LeftColumnIndex == _formatSourceRange.LeftColumnIndex
                    && target.RowCount == _formatSourceRange.RowCount
                    && target.ColumnCount == _formatSourceRange.ColumnCount)
                {
                    return;
                }

                ApplyFormatting(_formatSourceRange, target);
            }
            finally
            {
                _formatPainterActive = false;
                _formatSourceRange = null;
                _formatSourceWorksheet = null;
                _spreadsheetControl.Cursor = Cursors.Default;
                try { if (this._biFormatPainter != null) this._biFormatPainter.Down = false; } catch { }
            }
        }

        private void ApplyFormatting(CellRange source, CellRange target)
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
                        try
                        {
                            dstCell.Style = srcCell.Style;
                            dstCell.Alignment.Horizontal = srcCell.Alignment.Horizontal;
                            dstCell.Alignment.Vertical = srcCell.Alignment.Vertical;
                            dstCell.NumberFormat = srcCell.NumberFormat;
                        }
                        catch { }
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
            // Unwrap Nullable<T>
            if (value == null)
            {
                ws.Cells[row, col].Value = string.Empty;
                return;
            }

            // Note: a boxed DateTime? is either null (caught above) or a plain boxed DateTime.
            // No additional unwrapping is needed.

            if (value is DateTime dt)
            {
                ws.Cells[row, col].Value = dt;
                // Resolve a proper Excel number-format string for date cells.
                // "d"  is a .NET format specifier, not a valid Excel format code.
                // Fall back to "dd/mm/yyyy" when format is null/empty/"d".
                string dateFmt = formatString;
                if (string.IsNullOrEmpty(dateFmt)
                    || string.Equals(dateFmt, "d", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(dateFmt, "D", StringComparison.OrdinalIgnoreCase))
                {
                    dateFmt = "dd/mm/yyyy";
                }
                ws.Cells[row, col].NumberFormat = dateFmt;
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
            var fields = ResolveTargetFields();
            if (fields.Count == 0)
            {
                XtraMessageBox.Show("Select or check a field in the Field List panel first.",
                    "Insert Value", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Single field: use the original path (may show direction dialog for multi-record)
            if (fields.Count == 1)
            {
                InsertFieldValueOnly(fields[0]);
                return;
            }

            // Multiple fields: write each field's value on its own row (cursor advances downward).
            // This is the value-only counterpart of Label?Value Horizontal layout.
            var ws = _spreadsheetControl.ActiveWorksheet;
            var cell = _spreadsheetControl.SelectedCell;
            if (ws == null || cell == null) return;

            int row = cell.TopRowIndex;
            int col = cell.LeftColumnIndex;
            int recordCount = GetSelectedRecordCount();

            foreach (var fd in fields)
            {
                int usedRows = InsertValueOnlyHorizontalAt(fd, row, col, recordCount);
                row += usedRows;
            }
        }

        private void OnInsertValueHorizontalClick(object sender, ItemClickEventArgs e)
        {
            var fields = ResolveTargetFields();
            if (fields.Count == 0)
            {
                XtraMessageBox.Show("Select or check a field in the Field List panel first.",
                    "Insert Value Horizontal", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var ws = _spreadsheetControl.ActiveWorksheet;
            var cell = _spreadsheetControl.SelectedCell;
            if (ws == null || cell == null) return;

            int row = cell.TopRowIndex;
            int col = cell.LeftColumnIndex;
            int recordCount = GetSelectedRecordCount();

            // Each field occupies one or more rows; advance the row cursor after each field.
            // Multi-record scalar: values go right across columns on the same row.
            // Row-level field:     values stack downward; cursor advances by row count.
            foreach (var fd in fields)
            {
                int usedRows = InsertValueOnlyHorizontalAt(fd, row, col, recordCount);
                row += usedRows;
            }
        }

        private void OnInsertValueVerticalClick(object sender, ItemClickEventArgs e)
        {
            var fields = ResolveTargetFields();
            if (fields.Count == 0)
            {
                XtraMessageBox.Show("Select or check a field in the Field List panel first.",
                    "Insert Value Vertical", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var ws = _spreadsheetControl.ActiveWorksheet;
            var cell = _spreadsheetControl.SelectedCell;
            if (ws == null || cell == null) return;

            int row = cell.TopRowIndex;
            int col = cell.LeftColumnIndex;
            int recordCount = GetSelectedRecordCount();

            // Each field occupies one column; advance the column cursor after each field.
            // Multi-record scalar: values go down the column.
            // Row-level field:     values stack downward in the same column.
            foreach (var fd in fields)
            {
                int usedCols = InsertValueOnlyVerticalAt(fd, row, col, recordCount);
                col += usedCols;
            }
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
            _biSectionMode.Down = _isSectionMode;
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
            int recordCount = GetSelectedRecordCount();

            foreach (var fd in checkedFields)
            {
                if (horizontal)
                {
                    InsertFieldHorizontalAt(fd, row, col, recordCount);
                    // Advance row by the max rows this field+records occupy
                    int usedRows = 1;
                    if (fd.IsRowLevel)
                    {
                        if (recordCount > 1 && fd.HasSelectedRecordValues)
                        {
                            var multi = _currentAdapter as IMultiRecordReportAdapter;
                            int max = 0;
                            for (int si = 0; si < recordCount; si++)
                            {
                                int rc = fd.GetSelectedRowCount != null ? fd.GetSelectedRowCount(si) : 0;
                                if (rc > max) max = rc;
                            }
                            usedRows = max > 0 ? max : 1;
                        }
                        else
                        {
                            usedRows = fd.RowCount > 1 ? fd.RowCount : 1;
                        }
                    }
                    row += usedRows;
                }
                else
                {
                    InsertFieldVerticalAt(fd, row, col, recordCount);
                    col += 1;
                }
            }
        }

        private void InsertFieldHorizontalAt(FieldDescriptor fd, int row, int col, int recordCount)
        {
            var ws = _spreadsheetControl.ActiveWorksheet;
            if (ws == null) return;

            string label = fd.Caption ?? fd.Key;

            if (recordCount > 1 && fd.HasSelectedRecordValues)
            {
                if (fd.IsRowLevel)
                {
                    int maxRows = 0;
                    for (int si = 0; si < recordCount; si++)
                    {
                        int rc = fd.GetSelectedRowCount != null ? fd.GetSelectedRowCount(si) : 0;
                        if (rc > maxRows) maxRows = rc;
                    }
                    for (int ri = 0; ri < maxRows; ri++)
                    {
                        SetLabelCell(ws, row + ri, col, ri == 0 ? label : string.Empty);
                        for (int si = 0; si < recordCount; si++)
                        {
                            object val = fd.GetSelectedRowValue != null ? fd.GetSelectedRowValue(si, ri) : null;
                            SetCellValue(ws, row + ri, col + 1 + si, val, fd.FormatString);
                        }
                    }
                }
                else
                {
                    SetLabelCell(ws, row, col, label);
                    for (int si = 0; si < recordCount; si++)
                    {
                        object val = fd.GetSelectedValue != null ? fd.GetSelectedValue(si) : null;
                        SetCellValue(ws, row, col + 1 + si, val, fd.FormatString);
                    }
                }
                return;
            }

            // Single-record
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

        private void InsertFieldVerticalAt(FieldDescriptor fd, int row, int col, int recordCount)
        {
            var ws = _spreadsheetControl.ActiveWorksheet;
            if (ws == null) return;

            string label = fd.Caption ?? fd.Key;

            if (recordCount > 1 && fd.HasSelectedRecordValues)
            {
                SetLabelCell(ws, row, col, label);
                if (fd.IsRowLevel)
                {
                    int offset = 1;
                    for (int si = 0; si < recordCount; si++)
                    {
                        int rc = fd.GetSelectedRowCount != null ? fd.GetSelectedRowCount(si) : 0;
                        for (int ri = 0; ri < rc; ri++)
                        {
                            object val = fd.GetSelectedRowValue != null ? fd.GetSelectedRowValue(si, ri) : null;
                            SetCellValue(ws, row + offset, col, val, fd.FormatString);
                            offset++;
                        }
                    }
                }
                else
                {
                    for (int si = 0; si < recordCount; si++)
                    {
                        object val = fd.GetSelectedValue != null ? fd.GetSelectedValue(si) : null;
                        SetCellValue(ws, row + 1 + si, col, val, fd.FormatString);
                    }
                }
                return;
            }

            // Single-record
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
            _biToggleFieldList.Down = !_splitContainer.Panel1Collapsed;
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
            if (worksheet == null || _currentAdapter == null)
                return;

            var editableProvider = _currentAdapter as Interfaces.IEditableSheetProvider;
            if (editableProvider != null)
            {
                var names = editableProvider.GetEditableSheetNames();
                if (names != null)
                {
                    foreach (var name in names)
                    {
                        if (string.Equals(worksheet.Name, name, StringComparison.OrdinalIgnoreCase))
                        {
                            _hasPendingDataChanges = true;
                            return;
                        }
                    }
                }
                return;
            }

            // Fallback: any cell edit is a pending change if no editable-sheet contract
            _hasPendingDataChanges = true;
        }

        #endregion

        #region Button State Management

        private void UpdateButtonStates()
        {
            bool hasAdapter = _currentAdapter != null;
            bool isMulti = hasAdapter && (_currentAdapter is IMultiRecordReportAdapter);

            _biInsertValue.Enabled = hasAdapter;
            _biInsertValueH.Enabled = hasAdapter;
            _biInsertValueV.Enabled = hasAdapter;
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
            _biToggleRecordSelector.Enabled = isMulti;
            _biApplyRecordSelection.Enabled = isMulti;

            // Sync visual Down states
            _biToggleFieldList.Down = hasAdapter && !_splitContainer.Panel1Collapsed;
            _biSectionMode.Down = _isSectionMode;
            _biToggleRecordSelector.Down = isMulti && _leftTabControl.SelectedTabPage == _tabRecords;
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
            _recordSelectorList.Items.Clear();
            _activeRecordCombo.Properties.Items.Clear();

            // Reset tab to Fields and hide Records tab until an adapter is loaded
            _leftTabControl.SelectedTabPage = _tabFields;
            _tabRecords.PageVisible = false;

            // Reset button Down states
            _biSectionMode.Down = false;
            _biToggleRecordSelector.Down = false;
        }

        #endregion
    }
}
