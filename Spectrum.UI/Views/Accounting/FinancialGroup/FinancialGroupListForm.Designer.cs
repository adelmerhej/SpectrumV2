namespace Spectrum.Views.Accounting.FinancialGroup
{
    partial class FinancialGroupListForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FinancialGroupListForm));
            this.rcFinancialGroups = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.btnNew = new DevExpress.XtraBars.BarButtonItem();
            this.btnDelete = new DevExpress.XtraBars.BarButtonItem();
            this.btnEdit = new DevExpress.XtraBars.BarButtonItem();
            this.btnPrint = new DevExpress.XtraBars.BarButtonItem();
            this.btnRefresh = new DevExpress.XtraBars.BarButtonItem();
            this.btnClose = new DevExpress.XtraBars.BarButtonItem();
            this.btnResetGridStyle = new DevExpress.XtraBars.BarButtonItem();
            this.rpFinancialGroups = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup6 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup8 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup11 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.rpViewSettings = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonStatusBar1 = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.mainLayout = new DevExpress.XtraLayout.LayoutControl();
            this.gcFinancialGroups = new DevExpress.XtraGrid.GridControl();
            this.bsFinancialGroups = new System.Windows.Forms.BindingSource(this.components);
            this.gvFinancialGroups = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.col_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFinancialGroupName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNotes = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colActive = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repCheckEdit = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.colIsDefault = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.rcFinancialGroups)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainLayout)).BeginInit();
            this.mainLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcFinancialGroups)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsFinancialGroups)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvFinancialGroups)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repCheckEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // rcFinancialGroups
            // 
            this.rcFinancialGroups.EmptyAreaImageOptions.ImagePadding = new System.Windows.Forms.Padding(35, 39, 35, 39);
            this.rcFinancialGroups.ExpandCollapseItem.Id = 0;
            this.rcFinancialGroups.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.rcFinancialGroups.ExpandCollapseItem,
            this.btnNew,
            this.btnDelete,
            this.btnEdit,
            this.btnPrint,
            this.btnRefresh,
            this.btnClose,
            this.btnResetGridStyle});
            this.rcFinancialGroups.Location = new System.Drawing.Point(0, 0);
            this.rcFinancialGroups.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rcFinancialGroups.MaxItemId = 42;
            this.rcFinancialGroups.MdiMergeStyle = DevExpress.XtraBars.Ribbon.RibbonMdiMergeStyle.Always;
            this.rcFinancialGroups.Name = "rcFinancialGroups";
            this.rcFinancialGroups.OptionsMenuMinWidth = 385;
            this.rcFinancialGroups.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.rpFinancialGroups,
            this.rpViewSettings});
            this.rcFinancialGroups.Size = new System.Drawing.Size(1395, 193);
            this.rcFinancialGroups.StatusBar = this.ribbonStatusBar1;
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
            this.btnNew.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnNew_ItemClick);
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
            this.btnDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnDelete_ItemClick);
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
            this.btnEdit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnEdit_ItemClick);
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
            this.btnPrint.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnPrint_ItemClick);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Caption = "Refresh";
            this.btnRefresh.Id = 37;
            this.btnRefresh.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnRefresh.ImageOptions.SvgImage")));
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnRefresh_ItemClick);
            // 
            // btnClose
            // 
            this.btnClose.Caption = "Close";
            this.btnClose.Id = 38;
            this.btnClose.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnClose.ImageOptions.SvgImage")));
            this.btnClose.Name = "btnClose";
            this.btnClose.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnClose_ItemClick);
            // 
            // btnResetGridStyle
            // 
            this.btnResetGridStyle.Caption = "Reset Grid Style";
            this.btnResetGridStyle.Id = 41;
            this.btnResetGridStyle.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnResetGridStyle.ImageOptions.SvgImage")));
            this.btnResetGridStyle.Name = "btnResetGridStyle";
            this.btnResetGridStyle.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnResetGridStyle_ItemClick);
            // 
            // rpFinancialGroups
            // 
            this.rpFinancialGroups.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1,
            this.ribbonPageGroup6,
            this.ribbonPageGroup8,
            this.ribbonPageGroup11});
            this.rpFinancialGroups.Name = "rpFinancialGroups";
            this.rpFinancialGroups.Text = "Financial Group";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.AllowTextClipping = false;
            this.ribbonPageGroup1.CaptionButtonVisible = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonPageGroup1.ItemLinks.Add(this.btnNew);
            this.ribbonPageGroup1.MergeOrder = 0;
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
            this.ribbonPageGroup6.MergeOrder = 1;
            this.ribbonPageGroup6.Name = "ribbonPageGroup6";
            this.ribbonPageGroup6.Text = "Actions";
            // 
            // ribbonPageGroup8
            // 
            this.ribbonPageGroup8.AllowTextClipping = false;
            this.ribbonPageGroup8.CaptionButtonVisible = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonPageGroup8.ItemLinks.Add(this.btnDelete);
            this.ribbonPageGroup8.MergeOrder = 2;
            this.ribbonPageGroup8.Name = "ribbonPageGroup8";
            this.ribbonPageGroup8.Text = "Delete";
            // 
            // ribbonPageGroup11
            // 
            this.ribbonPageGroup11.Alignment = DevExpress.XtraBars.Ribbon.RibbonPageGroupAlignment.Far;
            this.ribbonPageGroup11.ItemLinks.Add(this.btnClose);
            this.ribbonPageGroup11.Name = "ribbonPageGroup11";
            this.ribbonPageGroup11.Text = "Close View";
            // 
            // rpViewSettings
            // 
            this.rpViewSettings.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup2});
            this.rpViewSettings.Name = "rpViewSettings";
            this.rpViewSettings.Text = "VIEW SETTINGS";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.btnResetGridStyle);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "Grid Settings";
            // 
            // ribbonStatusBar1
            // 
            this.ribbonStatusBar1.Location = new System.Drawing.Point(0, 744);
            this.ribbonStatusBar1.Name = "ribbonStatusBar1";
            this.ribbonStatusBar1.Ribbon = this.rcFinancialGroups;
            this.ribbonStatusBar1.Size = new System.Drawing.Size(1395, 30);
            // 
            // mainLayout
            // 
            this.mainLayout.Controls.Add(this.gcFinancialGroups);
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.Location = new System.Drawing.Point(0, 193);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.Root = this.Root;
            this.mainLayout.Size = new System.Drawing.Size(1395, 551);
            this.mainLayout.TabIndex = 8;
            this.mainLayout.Text = "layoutControl1";
            // 
            // gcFinancialGroups
            // 
            this.gcFinancialGroups.DataSource = this.bsFinancialGroups;
            this.gcFinancialGroups.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcFinancialGroups.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcFinancialGroups.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcFinancialGroups.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcFinancialGroups.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcFinancialGroups.Location = new System.Drawing.Point(14, 14);
            this.gcFinancialGroups.MainView = this.gvFinancialGroups;
            this.gcFinancialGroups.Name = "gcFinancialGroups";
            this.gcFinancialGroups.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repCheckEdit});
            this.gcFinancialGroups.Size = new System.Drawing.Size(1367, 523);
            this.gcFinancialGroups.TabIndex = 8;
            this.gcFinancialGroups.UseEmbeddedNavigator = true;
            this.gcFinancialGroups.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvFinancialGroups});
            // 
            // bsFinancialGroups
            // 
            this.bsFinancialGroups.DataSource = typeof(Spectrum.Models.Accounting.FinancialGroup.FinancialGroupModel);
            // 
            // gvFinancialGroups
            // 
            this.gvFinancialGroups.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.col_id,
            this.colFinancialGroupName,
            this.colNotes,
            this.colActive,
            this.colIsDefault});
            this.gvFinancialGroups.GridControl = this.gcFinancialGroups;
            this.gvFinancialGroups.Name = "gvFinancialGroups";
            this.gvFinancialGroups.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvFinancialGroups.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvFinancialGroups.OptionsBehavior.Editable = false;
            this.gvFinancialGroups.OptionsBehavior.ReadOnly = true;
            this.gvFinancialGroups.OptionsFind.AlwaysVisible = true;
            this.gvFinancialGroups.OptionsView.ColumnAutoWidth = false;
            this.gvFinancialGroups.OptionsView.ShowGroupedColumns = true;
            this.gvFinancialGroups.OptionsView.ShowGroupPanel = false;
            this.gvFinancialGroups.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gvFlowTypes_RowCellStyle);
            this.gvFinancialGroups.DoubleClick += new System.EventHandler(this.gvFinancialGroups_DoubleClick);
            // 
            // col_id
            // 
            this.col_id.FieldName = "_id";
            this.col_id.MinWidth = 25;
            this.col_id.Name = "col_id";
            this.col_id.Width = 94;
            // 
            // colFinancialGroupName
            // 
            this.colFinancialGroupName.Caption = "Name";
            this.colFinancialGroupName.FieldName = "Name";
            this.colFinancialGroupName.MinWidth = 25;
            this.colFinancialGroupName.Name = "colFinancialGroupName";
            this.colFinancialGroupName.Visible = true;
            this.colFinancialGroupName.VisibleIndex = 0;
            this.colFinancialGroupName.Width = 201;
            // 
            // colNotes
            // 
            this.colNotes.FieldName = "Notes";
            this.colNotes.MinWidth = 25;
            this.colNotes.Name = "colNotes";
            this.colNotes.Visible = true;
            this.colNotes.VisibleIndex = 1;
            this.colNotes.Width = 367;
            // 
            // colActive
            // 
            this.colActive.ColumnEdit = this.repCheckEdit;
            this.colActive.FieldName = "Active";
            this.colActive.MinWidth = 25;
            this.colActive.Name = "colActive";
            this.colActive.Visible = true;
            this.colActive.VisibleIndex = 2;
            this.colActive.Width = 94;
            // 
            // repCheckEdit
            // 
            this.repCheckEdit.AutoHeight = false;
            this.repCheckEdit.Name = "repCheckEdit";
            // 
            // colIsDefault
            // 
            this.colIsDefault.ColumnEdit = this.repCheckEdit;
            this.colIsDefault.FieldName = "IsDefault";
            this.colIsDefault.MinWidth = 25;
            this.colIsDefault.Name = "colIsDefault";
            this.colIsDefault.Visible = true;
            this.colIsDefault.VisibleIndex = 3;
            this.colIsDefault.Width = 94;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(1395, 551);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.gcFinancialGroups;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1371, 527);
            this.layoutControlItem1.TextVisible = false;
            // 
            // FinancialGroupListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1395, 774);
            this.Controls.Add(this.mainLayout);
            this.Controls.Add(this.rcFinancialGroups);
            this.Controls.Add(this.ribbonStatusBar1);
            this.Name = "FinancialGroupListForm";
            this.Ribbon = this.rcFinancialGroups;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.StatusBar = this.ribbonStatusBar1;
            this.Text = "Financial Group List";
            ((System.ComponentModel.ISupportInitialize)(this.rcFinancialGroups)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainLayout)).EndInit();
            this.mainLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcFinancialGroups)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsFinancialGroups)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvFinancialGroups)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repCheckEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public DevExpress.XtraBars.Ribbon.RibbonControl rcFinancialGroups;
        private DevExpress.XtraBars.BarButtonItem btnNew;
        private DevExpress.XtraBars.BarButtonItem btnDelete;
        private DevExpress.XtraBars.BarButtonItem btnEdit;
        private DevExpress.XtraBars.BarButtonItem btnPrint;
        private DevExpress.XtraBars.BarButtonItem btnRefresh;
        private DevExpress.XtraBars.BarButtonItem btnClose;
        private DevExpress.XtraBars.BarButtonItem btnResetGridStyle;
        private DevExpress.XtraBars.Ribbon.RibbonPage rpFinancialGroups;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup6;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup8;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup11;
        private DevExpress.XtraBars.Ribbon.RibbonPage rpViewSettings;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar1;
        private DevExpress.XtraLayout.LayoutControl mainLayout;
        private DevExpress.XtraGrid.GridControl gcFinancialGroups;
        private DevExpress.XtraGrid.Views.Grid.GridView gvFinancialGroups;
        private DevExpress.XtraGrid.Columns.GridColumn col_id;
        private DevExpress.XtraGrid.Columns.GridColumn colFinancialGroupName;
        private DevExpress.XtraGrid.Columns.GridColumn colNotes;
        private DevExpress.XtraGrid.Columns.GridColumn colActive;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repCheckEdit;
        private DevExpress.XtraGrid.Columns.GridColumn colIsDefault;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private System.Windows.Forms.BindingSource bsFinancialGroups;
    }
}