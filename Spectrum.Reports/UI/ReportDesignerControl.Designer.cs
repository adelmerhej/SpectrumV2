namespace Spectrum.Reports.UI
{
    partial class ReportDesignerControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ClearPreviousAdapter();
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this._spreadsheetControl = new DevExpress.XtraSpreadsheet.SpreadsheetControl();
            this._splitContainer    = new System.Windows.Forms.SplitContainer();
            this._leftTabControl    = new DevExpress.XtraTab.XtraTabControl();
            this._tabFields         = new DevExpress.XtraTab.XtraTabPage();
            this._fieldListTree     = new System.Windows.Forms.TreeView();
            this._tabRecords        = new DevExpress.XtraTab.XtraTabPage();
            this._recordSelectorList  = new DevExpress.XtraEditors.CheckedListBoxControl();
            this._activeRecordCombo   = new DevExpress.XtraEditors.ComboBoxEdit();
            this._activeRecordLabel   = new DevExpress.XtraEditors.LabelControl();
            this._summaryPanel        = new System.Windows.Forms.FlowLayoutPanel();

            // --- kept for code-behind compat; hidden behind tab control ---
            this._fieldListLabel      = new DevExpress.XtraEditors.LabelControl();
            this._recordSelectorLabel = new DevExpress.XtraEditors.LabelControl();
            this._fieldListPanel      = new System.Windows.Forms.Panel();
            this._recordSelectorPanel = new System.Windows.Forms.Panel();
            this._leftPanel           = new System.Windows.Forms.Panel();

            ((System.ComponentModel.ISupportInitialize)(this._splitContainer)).BeginInit();
            this._splitContainer.Panel1.SuspendLayout();
            this._splitContainer.Panel2.SuspendLayout();
            this._splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._leftTabControl)).BeginInit();
            this._leftTabControl.SuspendLayout();
            this._tabFields.SuspendLayout();
            this._tabRecords.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._recordSelectorList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._activeRecordCombo.Properties)).BeginInit();
            this.SuspendLayout();

            // ── SpreadsheetControl ─────────────────────────────────────────────
            this._spreadsheetControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._spreadsheetControl.Name = "_spreadsheetControl";
            this._spreadsheetControl.ActiveSheetChanged += new DevExpress.Spreadsheet.ActiveSheetChangedEventHandler(this.OnSpreadsheetActiveSheetChanged);
            this._spreadsheetControl.CellValueChanged   += new DevExpress.XtraSpreadsheet.CellValueChangedEventHandler(this.OnSpreadsheetCellValueChanged);
            this._spreadsheetControl.RowsInserted       += new DevExpress.Spreadsheet.RowsInsertedEventHandler(this.OnSpreadsheetRowsInserted);
            this._spreadsheetControl.SelectionChanged   += new System.EventHandler(this.OnSpreadsheetSelectionChanged);

            // ── Ribbon & bars ──────────────────────────────────────────────────
            this._ribbonControl = this._spreadsheetControl.CreateRibbon();
            this._ribbonControl.ShowToolbarCustomizeItem = false;
            this._ribbonControl.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;

            this._formulaBar = new DevExpress.XtraSpreadsheet.SpreadsheetFormulaBar();
            this._formulaBar.SpreadsheetControl = this._spreadsheetControl;
            this._formulaBar.Dock = System.Windows.Forms.DockStyle.Top;
            this._formulaBar.Name = "_formulaBar";

            this._ribbonStatusBar = this._spreadsheetControl.CreateRibbonStatusBar(this._ribbonControl);

            // ── Insert group buttons ───────────────────────────────────────────
            this._biInsertValue  = CreateReportBarButton("Insert\nValue",     "EditDataSource",        OnInsertValueClick,
                "Insert Value (cell)",
                "Inserts the selected field's value into the active spreadsheet cell.\nSelect a field in the Field List first, then click a target cell.");

            this._biInsertValueH = CreateReportBarButton("Insert Value\n\u2192",  "AlignHorizontalCenter", OnInsertValueHorizontalClick,
                "Insert Value \u2192 (horizontal)",
                "Inserts the selected field value and advances one column to the right.\nWith multiple fields checked, each value fills the next column.");

            this._biInsertValueV = CreateReportBarButton("Insert Value\n\u2193",  "AlignVerticalCenter",   OnInsertValueVerticalClick,
                "Insert Value \u2193 (vertical)",
                "Inserts the selected field value and advances one row down.\nWith multiple fields checked, each value fills the next row.");

            // ── Label+Value group buttons ──────────────────────────────────────
            this._biInsertHorizontal = CreateReportBarButton("Label \u2192 Value", "AlignHorizontalCenter", OnInsertHorizontalClick,
                "Label \u2192 Value (horizontal)",
                "Inserts the field name in the current cell and its value one column to the right.\nIn Section Mode, repeats for all checked fields going down.");

            this._biInsertVertical   = CreateReportBarButton("Label \u2193 Value", "AlignVerticalCenter",   OnInsertVerticalClick,
                "Label \u2193 Value (vertical)",
                "Inserts the field name in the current cell and its value one row below.\nIn Section Mode, repeats for all checked fields going right.");

            // ── Toggle group buttons (Check style) ────────────────────────────
            this._biToggleFieldList = CreateReportCheckButton("Field List",     "ListBullets",   OnToggleFieldListClick,
                "Show / Hide the side panel",
                "Collapses or expands the left panel that contains the field list and record selector.");
            this._biToggleFieldList.Down = true;

            this._biSectionMode = CreateReportCheckButton("Section Mode",  "Select",        OnToggleSectionModeClick,
                "Toggle Section Mode",
                "When ON, check-boxes appear on each field.\nSelect multiple fields, then use a Label\u2192Value button to insert them as a block.");

            this._biFormatPainter = CreateReportCheckButton("Format\nPainter", "FormatPainter", OnFormatPainterClick,
                "Format Painter",
                "Click to capture the formatting of the selected cell(s).\nThen click a target range to apply the same formatting.");
            this._biFormatPainter.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.Check;

            // ── Record selector buttons ────────────────────────────────────────
            this._biToggleRecordSelector = CreateReportCheckButton("Records\nPanel", "CheckedListBox", OnToggleRecordSelectorClick,
                "Show / Hide Records Panel",
                "Switches the side panel to the record selector tab.\nOnly enabled when the adapter supports multiple records.");

            this._biApplyRecordSelection = CreateReportBarButton("Apply\nSelection", "Apply", OnApplyRecordSelectionClick,
                "Apply Record Selection",
                "Commits the checked record list and updates the active record.\nThe field list is rebuilt to reflect the new selection.");

            // ── Layout group buttons ───────────────────────────────────────────
            this._biBestFit = CreateReportBarButton("Best Fit",         "BestFit",    OnBestFitClick,
                "Auto-fit columns",
                "Resizes all used columns on every sheet to fit their content.");

            this._biApplyA4 = CreateReportBarButton("Apply A4\nLayout", "PageSetup",  OnApplyA4LayoutClick,
                "Apply A4 page layout",
                "Sets portrait A4 margins, print area, and zoom on all sheets.");

            // ── Template group buttons ─────────────────────────────────────────
            this._biSaveLayout      = CreateReportBarButton("Save\nLayout",    "SaveAs", OnSaveLayoutClick,
                "Save layout template",
                "Saves the current spreadsheet as a named layout template for this report type.");

            this._biLoadLayout      = CreateReportBarButton("Load\nLayout",    "Open",   OnLoadLayoutClick,
                "Load a saved layout",
                "Opens a previously saved layout template and applies it to this report.");

            this._biSetDefaultLayout = CreateReportBarButton("Set\nDefault",   "Apply",  OnSetDefaultLayoutClick,
                "Set as default layout",
                "Marks the current layout as the default that loads automatically when this report is opened.");

            this._biSaveData        = CreateReportBarButton("Save\nData",      "Save",   OnSaveDataClick,
                "Save data changes",
                "Commits inline data edits (rows added / removed in editable sheets) back to the underlying model.");

            // Register all items
            this._ribbonControl.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
                this._biInsertValue, this._biInsertValueH, this._biInsertValueV,
                this._biInsertHorizontal, this._biInsertVertical,
                this._biToggleFieldList, this._biSectionMode, this._biFormatPainter,
                this._biToggleRecordSelector, this._biApplyRecordSelection,
                this._biBestFit, this._biApplyA4,
                this._biSaveLayout, this._biLoadLayout, this._biSetDefaultLayout, this._biSaveData
            });

            // ── Report Builder ribbon page ─────────────────────────────────────
            var pageReportBuilder = new DevExpress.XtraBars.Ribbon.RibbonPage("Report Builder");

            // Group 1 — Insert Values (value-only insertion)
            var groupInsert = new DevExpress.XtraBars.Ribbon.RibbonPageGroup("Insert Values");
            groupInsert.ItemLinks.Add(this._biInsertValue);
            groupInsert.ItemLinks.Add(this._biInsertValueH);
            groupInsert.ItemLinks.Add(this._biInsertValueV);

            // Group 2 — Fields (label+value blocks & field list control)
            var groupFields = new DevExpress.XtraBars.Ribbon.RibbonPageGroup("Fields");
            groupFields.ItemLinks.Add(this._biInsertHorizontal);
            groupFields.ItemLinks.Add(this._biInsertVertical);
            groupFields.ItemLinks.Add(this._biToggleFieldList);
            groupFields.ItemLinks.Add(this._biSectionMode);
            groupFields.ItemLinks.Add(this._biFormatPainter);

            // Group 3 — Records (multi-record switching)
            var groupRecords = new DevExpress.XtraBars.Ribbon.RibbonPageGroup("Records");
            groupRecords.ItemLinks.Add(this._biToggleRecordSelector);
            groupRecords.ItemLinks.Add(this._biApplyRecordSelection);

            // Group 4 — Layout (sheet formatting)
            var groupLayout = new DevExpress.XtraBars.Ribbon.RibbonPageGroup("Layout");
            groupLayout.ItemLinks.Add(this._biBestFit);
            groupLayout.ItemLinks.Add(this._biApplyA4);

            // Group 5 — Templates (layout persistence & data save)
            var groupTemplates = new DevExpress.XtraBars.Ribbon.RibbonPageGroup("Templates");
            groupTemplates.ItemLinks.Add(this._biSaveLayout);
            groupTemplates.ItemLinks.Add(this._biLoadLayout);
            groupTemplates.ItemLinks.Add(this._biSetDefaultLayout);
            groupTemplates.ItemLinks.Add(this._biSaveData);

            pageReportBuilder.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
                groupInsert, groupFields, groupRecords, groupLayout, groupTemplates
            });

            this._ribbonControl.Pages.Add(pageReportBuilder);

            // ── Field List tree (tab page) ─────────────────────────────────────
            this._fieldListTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this._fieldListTree.ShowNodeToolTips = true;
            this._fieldListTree.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._fieldListTree.CheckBoxes = false;
            this._fieldListTree.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this._fieldListTree.Name = "_fieldListTree";
            this._fieldListTree.AfterCheck  += new System.Windows.Forms.TreeViewEventHandler(this.OnFieldListAfterCheck);
            this._fieldListTree.DoubleClick += new System.EventHandler(this.OnFieldListDoubleClick);

            this._tabFields.Text     = "  Fields  ";
            this._tabFields.Name     = "_tabFields";
            this._tabFields.Padding  = new System.Windows.Forms.Padding(2);
            this._tabFields.Controls.Add(this._fieldListTree);

            // ── Record Selector controls (tab page) ───────────────────────────
            this._activeRecordLabel.Text = "Active record:";
            this._activeRecordLabel.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Default;
            this._activeRecordLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this._activeRecordLabel.Padding = new System.Windows.Forms.Padding(6, 6, 6, 2);
            this._activeRecordLabel.Name = "_activeRecordLabel";

            this._activeRecordCombo.Dock = System.Windows.Forms.DockStyle.Top;
            this._activeRecordCombo.Name = "_activeRecordCombo";
            this._activeRecordCombo.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;

            this._recordSelectorList.Dock = System.Windows.Forms.DockStyle.Fill;
            this._recordSelectorList.Name = "_recordSelectorList";
            this._recordSelectorList.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

            this._tabRecords.Text    = "  Records  ";
            this._tabRecords.Name    = "_tabRecords";
            this._tabRecords.Padding = new System.Windows.Forms.Padding(2);
            this._tabRecords.Controls.Add(this._recordSelectorList);
            this._tabRecords.Controls.Add(this._activeRecordCombo);
            this._tabRecords.Controls.Add(this._activeRecordLabel);

            // ── Tab control ────────────────────────────────────────────────────
            this._leftTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._leftTabControl.Name = "_leftTabControl";
            this._leftTabControl.TabPages.Add(this._tabFields);
            this._leftTabControl.TabPages.Add(this._tabRecords);

            // ── Hidden legacy panels (still referenced in cs file) ─────────────
            this._fieldListLabel.Visible      = false;
            this._recordSelectorLabel.Visible = false;
            this._fieldListPanel.Visible      = false;
            this._recordSelectorPanel.Visible = false;
            this._leftPanel.Visible           = false;

            // ── SplitContainer ────────────────────────────────────────────────
            this._splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this._splitContainer.Name = "_splitContainer";
            this._splitContainer.SplitterDistance = 270;
            this._splitContainer.Panel1MinSize = 200;
            this._splitContainer.Panel1.Controls.Add(this._leftTabControl);
            this._splitContainer.Panel2.Controls.Add(this._spreadsheetControl);

            // ── Summary Panel ─────────────────────────────────────────────────
            this._summaryPanel.AutoSize = true;
            this._summaryPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._summaryPanel.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this._summaryPanel.MinimumSize = new System.Drawing.Size(0, 30);
            this._summaryPanel.Name = "_summaryPanel";
            this._summaryPanel.Padding = new System.Windows.Forms.Padding(4);

            // ── ReportDesignerControl ─────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._splitContainer);
            this.Controls.Add(this._summaryPanel);
            this.Controls.Add(this._ribbonStatusBar);
            this.Controls.Add(this._formulaBar);
            this.Controls.Add(this._ribbonControl);
            this.Name = "ReportDesignerControl";
            this.Size = new System.Drawing.Size(1200, 700);

            this._splitContainer.Panel1.ResumeLayout(false);
            this._splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer)).EndInit();
            this._splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._leftTabControl)).EndInit();
            this._leftTabControl.ResumeLayout(false);
            this._tabFields.ResumeLayout(false);
            this._tabRecords.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._recordSelectorList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._activeRecordCombo.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        // ── Factory helpers ────────────────────────────────────────────────────
        private int _nextReportBarItemId = 50000;

        private DevExpress.XtraBars.BarButtonItem CreateReportBarButton(
            string caption, string imageUri,
            DevExpress.XtraBars.ItemClickEventHandler handler,
            string tipTitle = null, string tipText = null)
        {
            var item = new DevExpress.XtraBars.BarButtonItem();
            item.Caption = caption;
            item.Name    = "biReport_" + _nextReportBarItemId;
            item.Id      = _nextReportBarItemId++;
            item.ImageOptions.ImageUri.Uri = imageUri;
            item.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            item.Enabled = false;
            if (handler != null)
                item.ItemClick += handler;
            if (!string.IsNullOrEmpty(tipTitle))
                SetSuperTip(item, tipTitle, tipText);
            return item;
        }

        private DevExpress.XtraBars.BarButtonItem CreateReportCheckButton(
            string caption, string imageUri,
            DevExpress.XtraBars.ItemClickEventHandler handler,
            string tipTitle = null, string tipText = null)
        {
            var item = CreateReportBarButton(caption, imageUri, handler, tipTitle, tipText);
            item.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.Check;
            return item;
        }

        private static void SetSuperTip(DevExpress.XtraBars.BarItem item, string title, string body)
        {
            var tip = new DevExpress.Utils.SuperToolTip();
            var header = new DevExpress.Utils.ToolTipTitleItem { Text = title };
            tip.Items.Add(header);
            if (!string.IsNullOrEmpty(body))
            {
                tip.Items.Add(new DevExpress.Utils.ToolTipItem { Text = body });
            }
            item.SuperTip = tip;
        }

        #endregion

        // ── Core controls ──────────────────────────────────────────────────────
        private DevExpress.XtraBars.Ribbon.RibbonControl         _ribbonControl;
        private DevExpress.XtraSpreadsheet.SpreadsheetFormulaBar  _formulaBar;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar        _ribbonStatusBar;
        private System.Windows.Forms.SplitContainer               _splitContainer;

        // Left-panel tab control
        private DevExpress.XtraTab.XtraTabControl  _leftTabControl;
        private DevExpress.XtraTab.XtraTabPage     _tabFields;
        private DevExpress.XtraTab.XtraTabPage     _tabRecords;

        // Field list tab contents
        private System.Windows.Forms.TreeView _fieldListTree;

        // Record selector tab contents
        private DevExpress.XtraEditors.CheckedListBoxControl _recordSelectorList;
        private DevExpress.XtraEditors.LabelControl          _activeRecordLabel;
        private DevExpress.XtraEditors.ComboBoxEdit          _activeRecordCombo;

        // Summary bar
        private System.Windows.Forms.FlowLayoutPanel _summaryPanel;

        // Spreadsheet
        private DevExpress.XtraSpreadsheet.SpreadsheetControl _spreadsheetControl;

        // Legacy panels (kept so existing code compiles; kept hidden)
        private System.Windows.Forms.Panel              _leftPanel;
        private System.Windows.Forms.Panel              _fieldListPanel;
        private DevExpress.XtraEditors.LabelControl     _fieldListLabel;
        private System.Windows.Forms.Panel              _recordSelectorPanel;
        private DevExpress.XtraEditors.LabelControl     _recordSelectorLabel;

        // ── Custom Report Builder bar items ────────────────────────────────────
        private DevExpress.XtraBars.BarButtonItem _biInsertValue;
        private DevExpress.XtraBars.BarButtonItem _biInsertValueH;
        private DevExpress.XtraBars.BarButtonItem _biInsertValueV;
        private DevExpress.XtraBars.BarButtonItem _biInsertHorizontal;
        private DevExpress.XtraBars.BarButtonItem _biInsertVertical;
        private DevExpress.XtraBars.BarButtonItem _biToggleFieldList;
        private DevExpress.XtraBars.BarButtonItem _biSectionMode;
        private DevExpress.XtraBars.BarButtonItem _biFormatPainter;
        private DevExpress.XtraBars.BarButtonItem _biToggleRecordSelector;
        private DevExpress.XtraBars.BarButtonItem _biApplyRecordSelection;
        private DevExpress.XtraBars.BarButtonItem _biBestFit;
        private DevExpress.XtraBars.BarButtonItem _biApplyA4;
        private DevExpress.XtraBars.BarButtonItem _biSaveLayout;
        private DevExpress.XtraBars.BarButtonItem _biLoadLayout;
        private DevExpress.XtraBars.BarButtonItem _biSetDefaultLayout;
        private DevExpress.XtraBars.BarButtonItem _biSaveData;
    }
}
