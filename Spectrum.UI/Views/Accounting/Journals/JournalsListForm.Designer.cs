namespace Spectrum.Views.Accounting.Journals
{
    partial class JournalsListForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JournalsListForm));
            this.rcJournals = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.btnNew = new DevExpress.XtraBars.BarButtonItem();
            this.btnDelete = new DevExpress.XtraBars.BarButtonItem();
            this.btnEdit = new DevExpress.XtraBars.BarButtonItem();
            this.btnPrint = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem11 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem12 = new DevExpress.XtraBars.BarButtonItem();
            this.btnRefresh = new DevExpress.XtraBars.BarButtonItem();
            this.btnClose = new DevExpress.XtraBars.BarButtonItem();
            this.cboWorkingYear = new DevExpress.XtraBars.BarEditItem();
            this.btnPrintFilter = new DevExpress.XtraBars.BarButtonItem();
            this.btnResetGridStyle = new DevExpress.XtraBars.BarButtonItem();
            this.btnPrintStatement = new DevExpress.XtraBars.BarButtonItem();
            this.dtFrom = new DevExpress.XtraBars.BarEditItem();
            this.dtTo = new DevExpress.XtraBars.BarEditItem();
            this.rpJournals = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup6 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup8 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.rpgDepartments = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup11 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.rpViewSettings = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonStatusBar1 = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            ((System.ComponentModel.ISupportInitialize)(this.rcJournals)).BeginInit();
            this.SuspendLayout();
            // 
            // rcJournals
            // 
            this.rcJournals.EmptyAreaImageOptions.ImagePadding = new System.Windows.Forms.Padding(35, 39, 35, 39);
            this.rcJournals.ExpandCollapseItem.Id = 0;
            this.rcJournals.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.rcJournals.ExpandCollapseItem,
            this.btnNew,
            this.btnDelete,
            this.btnEdit,
            this.btnPrint,
            this.barButtonItem11,
            this.barButtonItem12,
            this.btnRefresh,
            this.btnClose,
            this.cboWorkingYear,
            this.btnPrintFilter,
            this.btnResetGridStyle,
            this.btnPrintStatement,
            this.dtFrom,
            this.dtTo});
            this.rcJournals.Location = new System.Drawing.Point(0, 0);
            this.rcJournals.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rcJournals.MaxItemId = 66;
            this.rcJournals.MdiMergeStyle = DevExpress.XtraBars.Ribbon.RibbonMdiMergeStyle.Always;
            this.rcJournals.Name = "rcJournals";
            this.rcJournals.OptionsMenuMinWidth = 385;
            this.rcJournals.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.rpJournals,
            this.rpViewSettings});
            this.rcJournals.Size = new System.Drawing.Size(1379, 193);
            this.rcJournals.StatusBar = this.ribbonStatusBar1;
            // 
            // btnNew
            // 
            this.btnNew.Caption = "Add New";
            this.btnNew.Enabled = false;
            this.btnNew.Id = 1;
            this.btnNew.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.False;
            this.btnNew.ImageOptions.ImageUri.Uri = "resource://DevExpress.DevAV.Resources.NewCustomer.svg";
            this.btnNew.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnNew.ImageOptions.SvgImage")));
            this.btnNew.Name = "btnNew";
            // 
            // btnDelete
            // 
            this.btnDelete.Caption = "Delete";
            this.btnDelete.Enabled = false;
            this.btnDelete.Id = 3;
            this.btnDelete.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.False;
            this.btnDelete.ImageOptions.ImageUri.Uri = "resource://DevExpress.DevAV.Resources.Delete.svg";
            this.btnDelete.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnDelete.ImageOptions.SvgImage")));
            this.btnDelete.Name = "btnDelete";
            // 
            // btnEdit
            // 
            this.btnEdit.Caption = "Edit";
            this.btnEdit.Enabled = false;
            this.btnEdit.Id = 13;
            this.btnEdit.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.False;
            this.btnEdit.ImageOptions.ImageUri.Uri = "resource://DevExpress.DevAV.Resources.Edit.svg";
            this.btnEdit.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnEdit.ImageOptions.SvgImage")));
            this.btnEdit.Name = "btnEdit";
            // 
            // btnPrint
            // 
            this.btnPrint.Caption = "Print View";
            this.btnPrint.Enabled = false;
            this.btnPrint.Id = 16;
            this.btnPrint.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.False;
            this.btnPrint.ImageOptions.ImageUri.Uri = "resource://DevExpress.DevAV.Resources.SalesAnalysis.svg";
            this.btnPrint.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnPrint.ImageOptions.SvgImage")));
            this.btnPrint.Name = "btnPrint";
            // 
            // barButtonItem11
            // 
            this.barButtonItem11.Caption = "View Settings";
            this.barButtonItem11.Id = 22;
            this.barButtonItem11.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.False;
            this.barButtonItem11.ImageOptions.ImageUri.Uri = "resource://DevExpress.DevAV.Resources.ViewSetting.svg";
            this.barButtonItem11.Name = "barButtonItem11";
            // 
            // barButtonItem12
            // 
            this.barButtonItem12.Caption = "Reset View";
            this.barButtonItem12.Id = 23;
            this.barButtonItem12.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.False;
            this.barButtonItem12.ImageOptions.ImageUri.Uri = "resource://DevExpress.DevAV.Resources.ResetView.svg";
            this.barButtonItem12.Name = "barButtonItem12";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Caption = "Refresh";
            this.btnRefresh.Id = 37;
            this.btnRefresh.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnRefresh.ImageOptions.SvgImage")));
            this.btnRefresh.Name = "btnRefresh";
            // 
            // btnClose
            // 
            this.btnClose.Caption = "Close";
            this.btnClose.Id = 38;
            this.btnClose.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnClose.ImageOptions.SvgImage")));
            this.btnClose.Name = "btnClose";
            // 
            // cboWorkingYear
            // 
            this.cboWorkingYear.Caption = "Working Year";
            this.cboWorkingYear.CaptionToEditorIndent = 6;
            this.cboWorkingYear.Edit = null;
            this.cboWorkingYear.EditWidth = 150;
            this.cboWorkingYear.Id = 39;
            this.cboWorkingYear.Name = "cboWorkingYear";
            // 
            // btnPrintFilter
            // 
            this.btnPrintFilter.Caption = "Print Filter";
            this.btnPrintFilter.Enabled = false;
            this.btnPrintFilter.Id = 58;
            this.btnPrintFilter.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnPrintFilter.ImageOptions.SvgImage")));
            this.btnPrintFilter.Name = "btnPrintFilter";
            // 
            // btnResetGridStyle
            // 
            this.btnResetGridStyle.Caption = "Reset Grid Style";
            this.btnResetGridStyle.Id = 61;
            this.btnResetGridStyle.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnResetGridStyle.ImageOptions.SvgImage")));
            this.btnResetGridStyle.Name = "btnResetGridStyle";
            // 
            // btnPrintStatement
            // 
            this.btnPrintStatement.Caption = "Print Statement";
            this.btnPrintStatement.Id = 62;
            this.btnPrintStatement.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnPrintStatement.ImageOptions.SvgImage")));
            this.btnPrintStatement.Name = "btnPrintStatement";
            // 
            // dtFrom
            // 
            this.dtFrom.Caption = "Date From";
            this.dtFrom.CaptionToEditorIndent = 10;
            this.dtFrom.Edit = null;
            this.dtFrom.EditWidth = 120;
            this.dtFrom.Id = 63;
            this.dtFrom.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("dtFrom.ImageOptions.SvgImage")));
            this.dtFrom.Name = "dtFrom";
            // 
            // dtTo
            // 
            this.dtTo.Caption = "Date To";
            this.dtTo.CaptionToEditorIndent = 22;
            this.dtTo.Edit = null;
            this.dtTo.EditWidth = 120;
            this.dtTo.Id = 64;
            this.dtTo.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("dtTo.ImageOptions.SvgImage")));
            this.dtTo.Name = "dtTo";
            // 
            // rpJournals
            // 
            this.rpJournals.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1,
            this.ribbonPageGroup6,
            this.ribbonPageGroup8,
            this.rpgDepartments,
            this.ribbonPageGroup11,
            this.ribbonPageGroup2});
            this.rpJournals.Name = "rpJournals";
            this.rpJournals.Text = "JOURNALS";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.AllowTextClipping = false;
            this.ribbonPageGroup1.CaptionButtonVisible = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonPageGroup1.ItemLinks.Add(this.btnNew);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "New";
            // 
            // ribbonPageGroup6
            // 
            this.ribbonPageGroup6.AllowTextClipping = false;
            this.ribbonPageGroup6.CaptionButtonVisible = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonPageGroup6.ItemLinks.Add(this.btnEdit);
            this.ribbonPageGroup6.ItemLinks.Add(this.btnRefresh);
            this.ribbonPageGroup6.ItemLinks.Add(this.btnPrint);
            this.ribbonPageGroup6.ItemLinks.Add(this.btnPrintFilter, true);
            this.ribbonPageGroup6.Name = "ribbonPageGroup6";
            this.ribbonPageGroup6.Text = "Actions";
            // 
            // ribbonPageGroup8
            // 
            this.ribbonPageGroup8.AllowTextClipping = false;
            this.ribbonPageGroup8.CaptionButtonVisible = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonPageGroup8.ItemLinks.Add(this.btnDelete);
            this.ribbonPageGroup8.Name = "ribbonPageGroup8";
            this.ribbonPageGroup8.Text = "Delete";
            // 
            // rpgDepartments
            // 
            this.rpgDepartments.AllowTextClipping = false;
            this.rpgDepartments.CaptionButtonVisible = DevExpress.Utils.DefaultBoolean.False;
            this.rpgDepartments.ItemLinks.Add(this.cboWorkingYear);
            this.rpgDepartments.Name = "rpgDepartments";
            this.rpgDepartments.Text = "Filter View";
            // 
            // ribbonPageGroup11
            // 
            this.ribbonPageGroup11.Alignment = DevExpress.XtraBars.Ribbon.RibbonPageGroupAlignment.Far;
            this.ribbonPageGroup11.ItemLinks.Add(this.btnClose);
            this.ribbonPageGroup11.Name = "ribbonPageGroup11";
            this.ribbonPageGroup11.Text = "Close View";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.dtFrom);
            this.ribbonPageGroup2.ItemLinks.Add(this.dtTo);
            this.ribbonPageGroup2.ItemLinks.Add(this.btnPrintStatement, true);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "ribbonPageGroup2";
            // 
            // rpViewSettings
            // 
            this.rpViewSettings.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup3});
            this.rpViewSettings.Name = "rpViewSettings";
            this.rpViewSettings.Text = "VIEW SETTINGS";
            // 
            // ribbonPageGroup3
            // 
            this.ribbonPageGroup3.ItemLinks.Add(this.btnResetGridStyle);
            this.ribbonPageGroup3.Name = "ribbonPageGroup3";
            this.ribbonPageGroup3.Text = "Grid Settings";
            // 
            // ribbonStatusBar1
            // 
            this.ribbonStatusBar1.Location = new System.Drawing.Point(0, 732);
            this.ribbonStatusBar1.Name = "ribbonStatusBar1";
            this.ribbonStatusBar1.Ribbon = this.rcJournals;
            this.ribbonStatusBar1.Size = new System.Drawing.Size(1379, 30);
            // 
            // JournalsListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1379, 762);
            this.Controls.Add(this.rcJournals);
            this.Controls.Add(this.ribbonStatusBar1);
            this.Name = "JournalsListForm";
            this.Ribbon = this.rcJournals;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.StatusBar = this.ribbonStatusBar1;
            this.Text = "Journals List";
            ((System.ComponentModel.ISupportInitialize)(this.rcJournals)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public DevExpress.XtraBars.Ribbon.RibbonControl rcJournals;
        private DevExpress.XtraBars.BarButtonItem btnNew;
        private DevExpress.XtraBars.BarButtonItem btnDelete;
        private DevExpress.XtraBars.BarButtonItem btnEdit;
        private DevExpress.XtraBars.BarButtonItem btnPrint;
        private DevExpress.XtraBars.BarButtonItem barButtonItem11;
        private DevExpress.XtraBars.BarButtonItem barButtonItem12;
        private DevExpress.XtraBars.BarButtonItem btnRefresh;
        private DevExpress.XtraBars.BarButtonItem btnClose;
        private DevExpress.XtraBars.BarEditItem cboWorkingYear;
        private DevExpress.XtraBars.BarButtonItem btnPrintFilter;
        private DevExpress.XtraBars.BarButtonItem btnResetGridStyle;
        private DevExpress.XtraBars.BarButtonItem btnPrintStatement;
        private DevExpress.XtraBars.BarEditItem dtFrom;
        private DevExpress.XtraBars.BarEditItem dtTo;
        private DevExpress.XtraBars.Ribbon.RibbonPage rpJournals;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup6;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup8;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rpgDepartments;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup11;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.Ribbon.RibbonPage rpViewSettings;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar1;
    }
}