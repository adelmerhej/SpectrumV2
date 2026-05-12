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
            this._fieldListPanel = new System.Windows.Forms.Panel();
            this._fieldListTree = new System.Windows.Forms.TreeView();
            this._fieldListLabel = new DevExpress.XtraEditors.LabelControl();
            this._summaryPanel = new System.Windows.Forms.FlowLayoutPanel();

            ((System.ComponentModel.ISupportInitialize)(this._splitContainer)).BeginInit();
            this._splitContainer.Panel1.SuspendLayout();
            this._splitContainer.Panel2.SuspendLayout();
            this._splitContainer.SuspendLayout();
            this._fieldListPanel.SuspendLayout();
            this.SuspendLayout();

            // ظ¤ظ¤ SpreadsheetControl ظ¤ظ¤
            this._spreadsheetControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._spreadsheetControl.Name = "_spreadsheetControl";
            this._spreadsheetControl.ActiveSheetChanged += new DevExpress.Spreadsheet.ActiveSheetChangedEventHandler(this.OnSpreadsheetActiveSheetChanged);
            this._spreadsheetControl.CellValueChanged += new DevExpress.XtraSpreadsheet.CellValueChangedEventHandler(this.OnSpreadsheetCellValueChanged);
            this._spreadsheetControl.RowsInserted += new DevExpress.Spreadsheet.RowsInsertedEventHandler(this.OnSpreadsheetRowsInserted);
            this._spreadsheetControl.SelectionChanged += new System.EventHandler(this.OnSpreadsheetSelectionChanged);

            // ظ¤ظ¤ Built-in Spreadsheet Ribbon ظ¤ظ¤
            // Creates all standard tabs: Home, Insert, Page Layout, Formulas, Data, Review, View
            // Provides: cell formatting, borders, colors, merging, text alignment, number formats,
            //           insert/delete rows & columns, page setup & margins, zoom, undo/redo,
            //           print, conditional formatting, styles, charts, pictures, and more.
            this._ribbonControl = this._spreadsheetControl.CreateRibbon();
            this._ribbonControl.ShowToolbarCustomizeItem = false;
            this._ribbonControl.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;

            // ظ¤ظ¤ Formula Bar (cell address + formula editor) ظ¤ظ¤
            this._formulaBar = new DevExpress.XtraSpreadsheet.SpreadsheetFormulaBar();
            this._formulaBar.SpreadsheetControl = this._spreadsheetControl;
            this._formulaBar.Dock = System.Windows.Forms.DockStyle.Top;
            this._formulaBar.Name = "_formulaBar";

            // ظ¤ظ¤ Status Bar (zoom slider, sheet info, cell stats) ظ¤ظ¤
            this._ribbonStatusBar = this._spreadsheetControl.CreateRibbonStatusBar(this._ribbonControl);

            // ظ¤ظ¤ Custom "Report Builder" ribbon items ظ¤ظ¤
            // Fields group
            this._biInsertValue = CreateReportBarButton("Insert Value", "EditDataSource", OnInsertValueClick);
            this._biInsertHorizontal = CreateReportBarButton("Label \u2192 Value", "AlignHorizontalCenter", OnInsertHorizontalClick);
            this._biInsertVertical = CreateReportBarButton("Label \u2193 Value", "AlignVerticalCenter", OnInsertVerticalClick);
            this._biToggleFieldList = CreateReportBarButton("Field List", "ListBullets", OnToggleFieldListClick);
            this._biSectionMode = CreateReportBarButton("Add Section Mode", "Select", OnToggleSectionModeClick);
            // Format Painter button
            this._biFormatPainter = CreateReportBarButton("Format Painter", "FormatPainter", OnFormatPainterClick);
            this._biFormatPainter.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.Check;

            // Layout group
            this._biBestFit = CreateReportBarButton("Best Fit", "BestFit", OnBestFitClick);
            this._biApplyA4 = CreateReportBarButton("Apply A4 Layout", "PageSetup", OnApplyA4LayoutClick);

            // Templates group
            this._biSaveLayout = CreateReportBarButton("Save Layout", "SaveAs", OnSaveLayoutClick);
            this._biLoadLayout = CreateReportBarButton("Load Layout", "Open", OnLoadLayoutClick);
            this._biSetDefaultLayout = CreateReportBarButton("Set Default", "Apply", OnSetDefaultLayoutClick);
            this._biSaveData = CreateReportBarButton("Save Data", "Save", OnSaveDataClick);

            // Register custom items with the ribbon
            this._ribbonControl.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
                this._biInsertValue, this._biInsertHorizontal, this._biInsertVertical,
                this._biToggleFieldList, this._biSectionMode, this._biFormatPainter,
                this._biBestFit, this._biApplyA4
                , this._biSaveLayout, this._biLoadLayout, this._biSetDefaultLayout, this._biSaveData
            });

            // ظ¤ظ¤ Report Builder page ظ¤ظ¤
            var pageReportBuilder = new DevExpress.XtraBars.Ribbon.RibbonPage("Report Builder");

            var groupFields = new DevExpress.XtraBars.Ribbon.RibbonPageGroup("Fields");
            groupFields.ItemLinks.Add(this._biInsertValue);
            groupFields.ItemLinks.Add(this._biInsertHorizontal);
            groupFields.ItemLinks.Add(this._biInsertVertical);
            groupFields.ItemLinks.Add(this._biToggleFieldList);
            groupFields.ItemLinks.Add(this._biSectionMode);
            groupFields.ItemLinks.Add(this._biFormatPainter);

            var groupLayout = new DevExpress.XtraBars.Ribbon.RibbonPageGroup("Layout");
            groupLayout.ItemLinks.Add(this._biBestFit);
            groupLayout.ItemLinks.Add(this._biApplyA4);

            var groupTemplates = new DevExpress.XtraBars.Ribbon.RibbonPageGroup("Templates");
            groupTemplates.ItemLinks.Add(this._biSaveLayout);
            groupTemplates.ItemLinks.Add(this._biLoadLayout);
            groupTemplates.ItemLinks.Add(this._biSetDefaultLayout);
            groupTemplates.ItemLinks.Add(this._biSaveData);

            pageReportBuilder.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
                groupFields, groupLayout, groupTemplates
            });

            this._ribbonControl.Pages.Add(pageReportBuilder);

            // ظ¤ظ¤ Field List Label ظ¤ظ¤
            this._fieldListLabel.Text = "Available Fields";
            this._fieldListLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this._fieldListLabel.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Default;
            this._fieldListLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this._fieldListLabel.Padding = new System.Windows.Forms.Padding(6, 6, 6, 4);
            this._fieldListLabel.Name = "_fieldListLabel";

            // ظ¤ظ¤ Field List TreeView ظ¤ظ¤
            this._fieldListTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this._fieldListTree.ShowNodeToolTips = true;
            this._fieldListTree.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._fieldListTree.CheckBoxes = false;
            this._fieldListTree.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this._fieldListTree.Name = "_fieldListTree";
            this._fieldListTree.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.OnFieldListAfterCheck);
            this._fieldListTree.DoubleClick += new System.EventHandler(this.OnFieldListDoubleClick);

            // ظ¤ظ¤ Field List Panel ظ¤ظ¤
            this._fieldListPanel.Controls.Add(this._fieldListTree);
            this._fieldListPanel.Controls.Add(this._fieldListLabel);
            this._fieldListPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._fieldListPanel.Name = "_fieldListPanel";

            // ظ¤ظ¤ SplitContainer ظ¤ظ¤
            this._splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this._splitContainer.Name = "_splitContainer";
            this._splitContainer.SplitterDistance = 250;
            this._splitContainer.Panel1.Controls.Add(this._fieldListPanel);
            this._splitContainer.Panel1MinSize = 180;
            this._splitContainer.Panel2.Controls.Add(this._spreadsheetControl);

            // ظ¤ظ¤ Summary Panel ظ¤ظ¤
            this._summaryPanel.AutoSize = true;
            this._summaryPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._summaryPanel.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this._summaryPanel.MinimumSize = new System.Drawing.Size(0, 30);
            this._summaryPanel.Name = "_summaryPanel";
            this._summaryPanel.Padding = new System.Windows.Forms.Padding(4);

            // ظ¤ظ¤ ReportDesignerControl ظ¤ظ¤
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
            this._fieldListPanel.ResumeLayout(false);
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
        private System.Windows.Forms.Panel _fieldListPanel;
        private DevExpress.XtraEditors.LabelControl _fieldListLabel;
        private System.Windows.Forms.TreeView _fieldListTree;
        private DevExpress.XtraSpreadsheet.SpreadsheetControl _spreadsheetControl;
        private System.Windows.Forms.FlowLayoutPanel _summaryPanel;

        // Custom Report Builder bar items
        private DevExpress.XtraBars.BarButtonItem _biInsertValue;
        private DevExpress.XtraBars.BarButtonItem _biInsertHorizontal;
        private DevExpress.XtraBars.BarButtonItem _biInsertVertical;
        private DevExpress.XtraBars.BarButtonItem _biToggleFieldList;
        private DevExpress.XtraBars.BarButtonItem _biSectionMode;
        private DevExpress.XtraBars.BarButtonItem _biFormatPainter;
        private DevExpress.XtraBars.BarButtonItem _biBestFit;
        private DevExpress.XtraBars.BarButtonItem _biApplyA4;
        private DevExpress.XtraBars.BarButtonItem _biSaveLayout;
        private DevExpress.XtraBars.BarButtonItem _biLoadLayout;
        private DevExpress.XtraBars.BarButtonItem _biSetDefaultLayout;
        private DevExpress.XtraBars.BarButtonItem _biSaveData;
    }
}
