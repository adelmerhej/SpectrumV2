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
            this._splitContainer = new System.Windows.Forms.SplitContainer();
            this._leftPanel = new System.Windows.Forms.Panel();
            this._fieldListPanel = new System.Windows.Forms.Panel();
            this._fieldListTree = new System.Windows.Forms.TreeView();
            this._fieldListLabel = new DevExpress.XtraEditors.LabelControl();
            this._recordSelectorPanel = new System.Windows.Forms.Panel();
            this._recordSelectorList = new DevExpress.XtraEditors.CheckedListBoxControl();
            this._recordSelectorLabel = new DevExpress.XtraEditors.LabelControl();
            this._activeRecordCombo = new DevExpress.XtraEditors.ComboBoxEdit();
            this._activeRecordLabel = new DevExpress.XtraEditors.LabelControl();
            this._summaryPanel = new System.Windows.Forms.FlowLayoutPanel();

            ((System.ComponentModel.ISupportInitialize)(this._splitContainer)).BeginInit();
            this._splitContainer.Panel1.SuspendLayout();
            this._splitContainer.Panel2.SuspendLayout();
            this._splitContainer.SuspendLayout();
            this._leftPanel.SuspendLayout();
            this._fieldListPanel.SuspendLayout();
            this._recordSelectorPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._recordSelectorList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._activeRecordCombo.Properties)).BeginInit();
            this.SuspendLayout();

            // SpreadsheetControl
            this._spreadsheetControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._spreadsheetControl.Name = "_spreadsheetControl";
            this._spreadsheetControl.ActiveSheetChanged += new DevExpress.Spreadsheet.ActiveSheetChangedEventHandler(this.OnSpreadsheetActiveSheetChanged);
            this._spreadsheetControl.CellValueChanged += new DevExpress.XtraSpreadsheet.CellValueChangedEventHandler(this.OnSpreadsheetCellValueChanged);
            this._spreadsheetControl.RowsInserted += new DevExpress.Spreadsheet.RowsInsertedEventHandler(this.OnSpreadsheetRowsInserted);
            this._spreadsheetControl.SelectionChanged += new System.EventHandler(this.OnSpreadsheetSelectionChanged);

            // Built-in Spreadsheet Ribbon
            this._ribbonControl = this._spreadsheetControl.CreateRibbon();
            this._ribbonControl.ShowToolbarCustomizeItem = false;
            this._ribbonControl.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;

            // Formula Bar
            this._formulaBar = new DevExpress.XtraSpreadsheet.SpreadsheetFormulaBar();
            this._formulaBar.SpreadsheetControl = this._spreadsheetControl;
            this._formulaBar.Dock = System.Windows.Forms.DockStyle.Top;
            this._formulaBar.Name = "_formulaBar";

            // Status Bar
            this._ribbonStatusBar = this._spreadsheetControl.CreateRibbonStatusBar(this._ribbonControl);

            // Custom "Report Builder" ribbon items — Fields group
            this._biInsertValue = CreateReportBarButton("Insert Value", "EditDataSource", OnInsertValueClick);
            this._biInsertValueH = CreateReportBarButton("Insert Value \u2192", "AlignHorizontalCenter", OnInsertValueHorizontalClick);
            this._biInsertValueV = CreateReportBarButton("Insert Value \u2193", "AlignVerticalCenter", OnInsertValueVerticalClick);
            this._biInsertHorizontal = CreateReportBarButton("Label \u2192 Value", "AlignHorizontalCenter", OnInsertHorizontalClick);
            this._biInsertVertical = CreateReportBarButton("Label \u2193 Value", "AlignVerticalCenter", OnInsertVerticalClick);
            this._biToggleFieldList = CreateReportBarButton("Field List", "ListBullets", OnToggleFieldListClick);
            this._biSectionMode = CreateReportBarButton("Add Section Mode", "Select", OnToggleSectionModeClick);
            this._biFormatPainter = CreateReportBarButton("Format Painter", "FormatPainter", OnFormatPainterClick);
            this._biFormatPainter.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.Check;
            this._biToggleRecordSelector = CreateReportBarButton("Select Records", "CheckedListBox", OnToggleRecordSelectorClick);
            this._biApplyRecordSelection = CreateReportBarButton("Apply Selection", "Apply", OnApplyRecordSelectionClick);

            // Layout group
            this._biBestFit = CreateReportBarButton("Best Fit", "BestFit", OnBestFitClick);
            this._biApplyA4 = CreateReportBarButton("Apply A4 Layout", "PageSetup", OnApplyA4LayoutClick);

            // Templates group
            this._biSaveLayout = CreateReportBarButton("Save Layout", "SaveAs", OnSaveLayoutClick);
            this._biLoadLayout = CreateReportBarButton("Load Layout", "Open", OnLoadLayoutClick);
            this._biSetDefaultLayout = CreateReportBarButton("Set Default", "Apply", OnSetDefaultLayoutClick);
            this._biSaveData = CreateReportBarButton("Save Data", "Save", OnSaveDataClick);

            // Register all items with the ribbon
            this._ribbonControl.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
                this._biInsertValue, this._biInsertValueH, this._biInsertValueV,
                this._biInsertHorizontal, this._biInsertVertical,
                this._biToggleFieldList, this._biSectionMode, this._biFormatPainter,
                this._biToggleRecordSelector, this._biApplyRecordSelection,
                this._biBestFit, this._biApplyA4,
                this._biSaveLayout, this._biLoadLayout, this._biSetDefaultLayout, this._biSaveData
            });

            // Report Builder ribbon page
            var pageReportBuilder = new DevExpress.XtraBars.Ribbon.RibbonPage("Report Builder");

            var groupFields = new DevExpress.XtraBars.Ribbon.RibbonPageGroup("Fields");
            groupFields.ItemLinks.Add(this._biInsertValue);
            groupFields.ItemLinks.Add(this._biInsertValueH);
            groupFields.ItemLinks.Add(this._biInsertValueV);
            groupFields.ItemLinks.Add(this._biInsertHorizontal);
            groupFields.ItemLinks.Add(this._biInsertVertical);
            groupFields.ItemLinks.Add(this._biToggleFieldList);
            groupFields.ItemLinks.Add(this._biSectionMode);
            groupFields.ItemLinks.Add(this._biFormatPainter);

            var groupRecords = new DevExpress.XtraBars.Ribbon.RibbonPageGroup("Records");
            groupRecords.ItemLinks.Add(this._biToggleRecordSelector);
            groupRecords.ItemLinks.Add(this._biApplyRecordSelection);

            var groupLayout = new DevExpress.XtraBars.Ribbon.RibbonPageGroup("Layout");
            groupLayout.ItemLinks.Add(this._biBestFit);
            groupLayout.ItemLinks.Add(this._biApplyA4);

            var groupTemplates = new DevExpress.XtraBars.Ribbon.RibbonPageGroup("Templates");
            groupTemplates.ItemLinks.Add(this._biSaveLayout);
            groupTemplates.ItemLinks.Add(this._biLoadLayout);
            groupTemplates.ItemLinks.Add(this._biSetDefaultLayout);
            groupTemplates.ItemLinks.Add(this._biSaveData);

            pageReportBuilder.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
                groupFields, groupRecords, groupLayout, groupTemplates
            });

            this._ribbonControl.Pages.Add(pageReportBuilder);

            // Field List Label
            this._fieldListLabel.Text = "Available Fields";
            this._fieldListLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this._fieldListLabel.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Default;
            this._fieldListLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this._fieldListLabel.Padding = new System.Windows.Forms.Padding(6, 6, 6, 4);
            this._fieldListLabel.Name = "_fieldListLabel";

            // Field List TreeView
            this._fieldListTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this._fieldListTree.ShowNodeToolTips = true;
            this._fieldListTree.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._fieldListTree.CheckBoxes = false;
            this._fieldListTree.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this._fieldListTree.Name = "_fieldListTree";
            this._fieldListTree.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.OnFieldListAfterCheck);
            this._fieldListTree.DoubleClick += new System.EventHandler(this.OnFieldListDoubleClick);

            // Field List Panel
            this._fieldListPanel.Controls.Add(this._fieldListTree);
            this._fieldListPanel.Controls.Add(this._fieldListLabel);
            this._fieldListPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._fieldListPanel.Name = "_fieldListPanel";

            // Record Selector Label
            this._recordSelectorLabel.Text = "Select Projects";
            this._recordSelectorLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this._recordSelectorLabel.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Default;
            this._recordSelectorLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this._recordSelectorLabel.Padding = new System.Windows.Forms.Padding(6, 6, 6, 2);
            this._recordSelectorLabel.Name = "_recordSelectorLabel";

            // Record Selector CheckedListBox
            this._recordSelectorList.Dock = System.Windows.Forms.DockStyle.Fill;
            this._recordSelectorList.Name = "_recordSelectorList";
            this._recordSelectorList.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

            // Active Record Label
            this._activeRecordLabel.Text = "Active:";
            this._activeRecordLabel.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Default;
            this._activeRecordLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this._activeRecordLabel.Padding = new System.Windows.Forms.Padding(6, 4, 6, 2);
            this._activeRecordLabel.Name = "_activeRecordLabel";

            // Active Record ComboBox
            this._activeRecordCombo.Dock = System.Windows.Forms.DockStyle.Top;
            this._activeRecordCombo.Name = "_activeRecordCombo";
            this._activeRecordCombo.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;

            // Record Selector Panel
            this._recordSelectorPanel.Controls.Add(this._recordSelectorList);
            this._recordSelectorPanel.Controls.Add(this._activeRecordCombo);
            this._recordSelectorPanel.Controls.Add(this._activeRecordLabel);
            this._recordSelectorPanel.Controls.Add(this._recordSelectorLabel);
            this._recordSelectorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._recordSelectorPanel.Name = "_recordSelectorPanel";
            this._recordSelectorPanel.Visible = false;

            // Left Panel — stacks Field List + Record Selector
            this._leftPanel.Controls.Add(this._fieldListPanel);
            this._leftPanel.Controls.Add(this._recordSelectorPanel);
            this._leftPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._leftPanel.Name = "_leftPanel";

            // SplitContainer
            this._splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this._splitContainer.Name = "_splitContainer";
            this._splitContainer.SplitterDistance = 250;
            this._splitContainer.Panel1.Controls.Add(this._leftPanel);
            this._splitContainer.Panel1MinSize = 180;
            this._splitContainer.Panel2.Controls.Add(this._spreadsheetControl);

            // Summary Panel
            this._summaryPanel.AutoSize = true;
            this._summaryPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._summaryPanel.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this._summaryPanel.MinimumSize = new System.Drawing.Size(0, 30);
            this._summaryPanel.Name = "_summaryPanel";
            this._summaryPanel.Padding = new System.Windows.Forms.Padding(4);

            // ReportDesignerControl
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
            this._leftPanel.ResumeLayout(false);
            this._fieldListPanel.ResumeLayout(false);
            this._recordSelectorPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._recordSelectorList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._activeRecordCombo.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private int _nextReportBarItemId = 50000;

        private DevExpress.XtraBars.BarButtonItem CreateReportBarButton(
            string caption, string imageUri,
            DevExpress.XtraBars.ItemClickEventHandler handler)
        {
            var item = new DevExpress.XtraBars.BarButtonItem();
            item.Caption = caption;
            item.Name = "biReport_" + _nextReportBarItemId;
            item.Id = _nextReportBarItemId++;
            item.ImageOptions.ImageUri.Uri = imageUri;
            item.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            item.Enabled = false;
            if (handler != null)
                item.ItemClick += handler;
            return item;
        }

        #endregion

        // Core controls
        private DevExpress.XtraBars.Ribbon.RibbonControl _ribbonControl;
        private DevExpress.XtraSpreadsheet.SpreadsheetFormulaBar _formulaBar;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar _ribbonStatusBar;
        private System.Windows.Forms.SplitContainer _splitContainer;
        private System.Windows.Forms.Panel _leftPanel;
        private System.Windows.Forms.Panel _fieldListPanel;
        private DevExpress.XtraEditors.LabelControl _fieldListLabel;
        private System.Windows.Forms.TreeView _fieldListTree;
        private System.Windows.Forms.Panel _recordSelectorPanel;
        private DevExpress.XtraEditors.CheckedListBoxControl _recordSelectorList;
        private DevExpress.XtraEditors.LabelControl _recordSelectorLabel;
        private DevExpress.XtraEditors.ComboBoxEdit _activeRecordCombo;
        private DevExpress.XtraEditors.LabelControl _activeRecordLabel;
        private DevExpress.XtraSpreadsheet.SpreadsheetControl _spreadsheetControl;
        private System.Windows.Forms.FlowLayoutPanel _summaryPanel;

        // Custom Report Builder bar items
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
