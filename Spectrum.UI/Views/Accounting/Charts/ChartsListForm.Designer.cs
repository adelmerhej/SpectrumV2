namespace Spectrum.Views.Accounting.Charts
{
	partial class ChartsListForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChartsListForm));
            this.rcCharts = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.btnNew = new DevExpress.XtraBars.BarButtonItem();
            this.btnDelete = new DevExpress.XtraBars.BarButtonItem();
            this.btnEdit = new DevExpress.XtraBars.BarButtonItem();
            this.btnPrint = new DevExpress.XtraBars.BarButtonItem();
            this.btnRefresh = new DevExpress.XtraBars.BarButtonItem();
            this.btnClose = new DevExpress.XtraBars.BarButtonItem();
            this.btnResetGridStyle = new DevExpress.XtraBars.BarButtonItem();
            this.btnMigrateCharts = new DevExpress.XtraBars.BarButtonItem();
            this.cboWorkingYear = new DevExpress.XtraBars.BarEditItem();
            this.repWorkingYear = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.rpCharts = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup6 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup8 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup11 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup4 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.rpViewSettings = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonStatusBar1 = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.mainLayout = new DevExpress.XtraLayout.LayoutControl();
            this.chartTreeList = new DevExpress.XtraTreeList.TreeList();
            this.colParentId = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colNumber = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colSerial = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colAccountName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colAccountTypeId = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colLAmountOpening = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colLAmountDebit = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colLAmountCredit = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colFAmountOpening = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colFAmountDebit = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colFAmountCredit = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colNotes = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colChartId = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.bsCharts = new System.Windows.Forms.BindingSource(this.components);
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.colArabicAccountName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListBand5 = new DevExpress.XtraTreeList.Columns.TreeListBand();
            this.treeListBand6 = new DevExpress.XtraTreeList.Columns.TreeListBand();
            this.treeListBand7 = new DevExpress.XtraTreeList.Columns.TreeListBand();
            this.treeListBand2 = new DevExpress.XtraTreeList.Columns.TreeListBand();
            this.treeListBand3 = new DevExpress.XtraTreeList.Columns.TreeListBand();
            this.treeListBand8 = new DevExpress.XtraTreeList.Columns.TreeListBand();
            ((System.ComponentModel.ISupportInitialize)(this.rcCharts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repWorkingYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainLayout)).BeginInit();
            this.mainLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartTreeList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsCharts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // rcCharts
            // 
            this.rcCharts.EmptyAreaImageOptions.ImagePadding = new System.Windows.Forms.Padding(35, 39, 35, 39);
            this.rcCharts.ExpandCollapseItem.Id = 0;
            this.rcCharts.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.rcCharts.ExpandCollapseItem,
            this.btnNew,
            this.btnDelete,
            this.btnEdit,
            this.btnPrint,
            this.btnRefresh,
            this.btnClose,
            this.btnResetGridStyle,
            this.btnMigrateCharts,
            this.cboWorkingYear});
            this.rcCharts.Location = new System.Drawing.Point(0, 0);
            this.rcCharts.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rcCharts.MaxItemId = 45;
            this.rcCharts.MdiMergeStyle = DevExpress.XtraBars.Ribbon.RibbonMdiMergeStyle.Always;
            this.rcCharts.Name = "rcCharts";
            this.rcCharts.OptionsMenuMinWidth = 385;
            this.rcCharts.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.rpCharts,
            this.rpViewSettings});
            this.rcCharts.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repWorkingYear});
            this.rcCharts.Size = new System.Drawing.Size(1412, 193);
            this.rcCharts.StatusBar = this.ribbonStatusBar1;
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
            // btnMigrateCharts
            // 
            this.btnMigrateCharts.Caption = "Migrate Charts";
            this.btnMigrateCharts.Id = 43;
            this.btnMigrateCharts.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnMigrateCharts.ImageOptions.SvgImage")));
            this.btnMigrateCharts.Name = "btnMigrateCharts";
            this.btnMigrateCharts.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnMigrateCharts_ItemClick);
            // 
            // cboWorkingYear
            // 
            this.cboWorkingYear.Caption = "Working Year       ";
            this.cboWorkingYear.CaptionAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.cboWorkingYear.Edit = this.repWorkingYear;
            this.cboWorkingYear.EditWidth = 100;
            this.cboWorkingYear.Id = 44;
            this.cboWorkingYear.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("cboWorkingYear.ImageOptions.SvgImage")));
            this.cboWorkingYear.Name = "cboWorkingYear";
            this.cboWorkingYear.EditValueChanged += new System.EventHandler(this.cboWorkingYear_EditValueChanged);
            // 
            // repWorkingYear
            // 
            this.repWorkingYear.AutoHeight = false;
            this.repWorkingYear.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repWorkingYear.Name = "repWorkingYear";
            this.repWorkingYear.NullText = "";
            // 
            // rpCharts
            // 
            this.rpCharts.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1,
            this.ribbonPageGroup6,
            this.ribbonPageGroup8,
            this.ribbonPageGroup11,
            this.ribbonPageGroup4});
            this.rpCharts.Name = "rpCharts";
            this.rpCharts.Text = "CHARTS";
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
            // ribbonPageGroup4
            // 
            this.ribbonPageGroup4.ItemLinks.Add(this.cboWorkingYear);
            this.ribbonPageGroup4.Name = "ribbonPageGroup4";
            this.ribbonPageGroup4.Text = "Current Period";
            // 
            // rpViewSettings
            // 
            this.rpViewSettings.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup2,
            this.ribbonPageGroup3});
            this.rpViewSettings.Name = "rpViewSettings";
            this.rpViewSettings.Text = "VIEW SETTINGS";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.btnResetGridStyle);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "Grid Settings";
            // 
            // ribbonPageGroup3
            // 
            this.ribbonPageGroup3.ItemLinks.Add(this.btnMigrateCharts);
            this.ribbonPageGroup3.Name = "ribbonPageGroup3";
            this.ribbonPageGroup3.Text = "Migration";
            // 
            // ribbonStatusBar1
            // 
            this.ribbonStatusBar1.Location = new System.Drawing.Point(0, 758);
            this.ribbonStatusBar1.Name = "ribbonStatusBar1";
            this.ribbonStatusBar1.Ribbon = this.rcCharts;
            this.ribbonStatusBar1.Size = new System.Drawing.Size(1412, 30);
            // 
            // mainLayout
            // 
            this.mainLayout.Controls.Add(this.chartTreeList);
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.Location = new System.Drawing.Point(0, 193);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.Root = this.Root;
            this.mainLayout.Size = new System.Drawing.Size(1412, 565);
            this.mainLayout.TabIndex = 8;
            this.mainLayout.Text = "layoutControl1";
            // 
            // chartTreeList
            // 
            this.chartTreeList.Bands.AddRange(new DevExpress.XtraTreeList.Columns.TreeListBand[] {
            this.treeListBand5,
            this.treeListBand6,
            this.treeListBand7,
            this.treeListBand8});
            this.chartTreeList.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colChartId,
            this.colParentId,
            this.colNumber,
            this.colSerial,
            this.colAccountName,
            this.colArabicAccountName,
            this.colAccountTypeId,
            this.colLAmountOpening,
            this.colLAmountDebit,
            this.colLAmountCredit,
            this.colFAmountOpening,
            this.colFAmountDebit,
            this.colFAmountCredit,
            this.colNotes});
            this.chartTreeList.DataSource = this.bsCharts;
            this.chartTreeList.KeyFieldName = "Id";
            this.chartTreeList.Location = new System.Drawing.Point(14, 14);
            this.chartTreeList.MenuManager = this.rcCharts;
            this.chartTreeList.MinWidth = 23;
            this.chartTreeList.Name = "chartTreeList";
            this.chartTreeList.OptionsBehavior.Editable = false;
            this.chartTreeList.OptionsBehavior.PopulateServiceColumns = true;
            this.chartTreeList.OptionsFind.AlwaysVisible = true;
            this.chartTreeList.OptionsNavigation.EnterMovesNextColumn = true;
            this.chartTreeList.OptionsView.AutoWidth = false;
            this.chartTreeList.OptionsView.ShowHierarchyIndentationLines = DevExpress.Utils.DefaultBoolean.True;
            this.chartTreeList.OptionsView.ShowTreeLines = DevExpress.Utils.DefaultBoolean.True;
            this.chartTreeList.ParentFieldName = "ParentId";
            this.chartTreeList.Size = new System.Drawing.Size(1384, 537);
            this.chartTreeList.TabIndex = 4;
            this.chartTreeList.TreeLevelWidth = 21;
            this.chartTreeList.TreeViewColumn = this.colParentId;
            this.chartTreeList.NodeCellStyle += new DevExpress.XtraTreeList.GetCustomNodeCellStyleEventHandler(this.chartTreeList_NodeCellStyle);
            this.chartTreeList.DoubleClick += new System.EventHandler(this.chartTreeList_DoubleClick);
            // 
            // colParentId
            // 
            this.colParentId.FieldName = "ParentId";
            this.colParentId.Name = "colParentId";
            this.colParentId.OptionsColumn.AllowSort = true;
            this.colParentId.Visible = true;
            this.colParentId.VisibleIndex = 0;
            // 
            // colNumber
            // 
            this.colNumber.FieldName = "Number";
            this.colNumber.MinWidth = 23;
            this.colNumber.Name = "colNumber";
            this.colNumber.OptionsColumn.AllowEdit = false;
            this.colNumber.OptionsColumn.AllowSort = true;
            this.colNumber.SortOrder = System.Windows.Forms.SortOrder.Ascending;
            this.colNumber.Visible = true;
            this.colNumber.VisibleIndex = 1;
            this.colNumber.Width = 87;
            // 
            // colSerial
            // 
            this.colSerial.FieldName = "Serial";
            this.colSerial.MinWidth = 23;
            this.colSerial.Name = "colSerial";
            this.colSerial.OptionsColumn.AllowEdit = false;
            this.colSerial.OptionsColumn.AllowSort = true;
            this.colSerial.Visible = true;
            this.colSerial.VisibleIndex = 2;
            this.colSerial.Width = 105;
            // 
            // colAccountName
            // 
            this.colAccountName.FieldName = "AccountName";
            this.colAccountName.MinWidth = 23;
            this.colAccountName.Name = "colAccountName";
            this.colAccountName.OptionsColumn.AllowEdit = false;
            this.colAccountName.OptionsColumn.AllowSort = true;
            this.colAccountName.Visible = true;
            this.colAccountName.VisibleIndex = 3;
            this.colAccountName.Width = 412;
            // 
            // colAccountTypeId
            // 
            this.colAccountTypeId.Caption = "Type";
            this.colAccountTypeId.FieldName = "AccountType";
            this.colAccountTypeId.Name = "colAccountTypeId";
            this.colAccountTypeId.OptionsColumn.AllowSort = true;
            this.colAccountTypeId.Visible = true;
            this.colAccountTypeId.VisibleIndex = 5;
            this.colAccountTypeId.Width = 81;
            // 
            // colLAmountOpening
            // 
            this.colLAmountOpening.Caption = "Opening LL";
            this.colLAmountOpening.FieldName = "LAmountOpening";
            this.colLAmountOpening.MinWidth = 23;
            this.colLAmountOpening.Name = "colLAmountOpening";
            this.colLAmountOpening.OptionsColumn.AllowSort = true;
            this.colLAmountOpening.Visible = true;
            this.colLAmountOpening.VisibleIndex = 6;
            this.colLAmountOpening.Width = 143;
            // 
            // colLAmountDebit
            // 
            this.colLAmountDebit.Caption = "Debit LL";
            this.colLAmountDebit.FieldName = "LAmountDebit";
            this.colLAmountDebit.MinWidth = 23;
            this.colLAmountDebit.Name = "colLAmountDebit";
            this.colLAmountDebit.OptionsColumn.AllowEdit = false;
            this.colLAmountDebit.OptionsColumn.AllowSort = true;
            this.colLAmountDebit.Visible = true;
            this.colLAmountDebit.VisibleIndex = 7;
            this.colLAmountDebit.Width = 118;
            // 
            // colLAmountCredit
            // 
            this.colLAmountCredit.Caption = "Credit LL";
            this.colLAmountCredit.FieldName = "LAmountCredit";
            this.colLAmountCredit.MinWidth = 23;
            this.colLAmountCredit.Name = "colLAmountCredit";
            this.colLAmountCredit.OptionsColumn.AllowEdit = false;
            this.colLAmountCredit.OptionsColumn.AllowSort = true;
            this.colLAmountCredit.Visible = true;
            this.colLAmountCredit.VisibleIndex = 8;
            this.colLAmountCredit.Width = 99;
            // 
            // colFAmountOpening
            // 
            this.colFAmountOpening.Caption = "Opening USD";
            this.colFAmountOpening.FieldName = "FAmountOpening";
            this.colFAmountOpening.MinWidth = 23;
            this.colFAmountOpening.Name = "colFAmountOpening";
            this.colFAmountOpening.OptionsColumn.AllowEdit = false;
            this.colFAmountOpening.OptionsColumn.AllowSort = true;
            this.colFAmountOpening.Visible = true;
            this.colFAmountOpening.VisibleIndex = 9;
            this.colFAmountOpening.Width = 143;
            // 
            // colFAmountDebit
            // 
            this.colFAmountDebit.Caption = "Debit USD";
            this.colFAmountDebit.FieldName = "FAmountDebit";
            this.colFAmountDebit.MinWidth = 23;
            this.colFAmountDebit.Name = "colFAmountDebit";
            this.colFAmountDebit.OptionsColumn.AllowEdit = false;
            this.colFAmountDebit.OptionsColumn.AllowSort = true;
            this.colFAmountDebit.Visible = true;
            this.colFAmountDebit.VisibleIndex = 10;
            this.colFAmountDebit.Width = 98;
            // 
            // colFAmountCredit
            // 
            this.colFAmountCredit.Caption = "Credit USD";
            this.colFAmountCredit.FieldName = "FAmountCredit";
            this.colFAmountCredit.Name = "colFAmountCredit";
            this.colFAmountCredit.OptionsColumn.AllowSort = true;
            this.colFAmountCredit.Visible = true;
            this.colFAmountCredit.VisibleIndex = 11;
            this.colFAmountCredit.Width = 81;
            // 
            // colNotes
            // 
            this.colNotes.FieldName = "Notes";
            this.colNotes.MinWidth = 23;
            this.colNotes.Name = "colNotes";
            this.colNotes.OptionsColumn.AllowEdit = false;
            this.colNotes.OptionsColumn.AllowSort = true;
            this.colNotes.Visible = true;
            this.colNotes.VisibleIndex = 12;
            this.colNotes.Width = 271;
            // 
            // colChartId
            // 
            this.colChartId.Caption = "Id";
            this.colChartId.FieldName = "_id";
            this.colChartId.MinWidth = 23;
            this.colChartId.Name = "colChartId";
            this.colChartId.OptionsColumn.AllowEdit = false;
            this.colChartId.OptionsColumn.AllowSort = true;
            this.colChartId.Width = 54;
            // 
            // bsCharts
            // 
            this.bsCharts.DataSource = typeof(Spectrum.Models.Accounting.Charts.ChartModel);
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(1412, 565);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.chartTreeList;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1388, 541);
            this.layoutControlItem1.TextVisible = false;
            // 
            // colArabicAccountName
            // 
            this.colArabicAccountName.Caption = "Arabic Name";
            this.colArabicAccountName.FieldName = "ArabicAccountName";
            this.colArabicAccountName.Name = "colArabicAccountName";
            this.colArabicAccountName.OptionsColumn.AllowSort = true;
            this.colArabicAccountName.Visible = true;
            this.colArabicAccountName.VisibleIndex = 4;
            this.colArabicAccountName.Width = 438;
            // 
            // treeListBand5
            // 
            this.treeListBand5.Caption = "Account Number";
            this.treeListBand5.Columns.Add(this.colParentId);
            this.treeListBand5.Columns.Add(this.colNumber);
            this.treeListBand5.Columns.Add(this.colSerial);
            this.treeListBand5.MinWidth = 23;
            this.treeListBand5.Name = "treeListBand5";
            this.treeListBand5.Width = 267;
            // 
            // treeListBand6
            // 
            this.treeListBand6.Caption = "Account Information";
            this.treeListBand6.Columns.Add(this.colAccountName);
            this.treeListBand6.Columns.Add(this.colArabicAccountName);
            this.treeListBand6.Columns.Add(this.colAccountTypeId);
            this.treeListBand6.MinWidth = 23;
            this.treeListBand6.Name = "treeListBand6";
            this.treeListBand6.Width = 858;
            // 
            // treeListBand7
            // 
            this.treeListBand7.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Bold);
            this.treeListBand7.AppearanceHeader.Options.UseFont = true;
            this.treeListBand7.AppearanceHeader.Options.UseTextOptions = true;
            this.treeListBand7.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.treeListBand7.Bands.AddRange(new DevExpress.XtraTreeList.Columns.TreeListBand[] {
            this.treeListBand2,
            this.treeListBand3});
            this.treeListBand7.Caption = "Account Figures";
            this.treeListBand7.MinWidth = 23;
            this.treeListBand7.Name = "treeListBand7";
            this.treeListBand7.Width = 682;
            // 
            // treeListBand2
            // 
            this.treeListBand2.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Bold);
            this.treeListBand2.AppearanceHeader.Options.UseFont = true;
            this.treeListBand2.AppearanceHeader.Options.UseTextOptions = true;
            this.treeListBand2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.treeListBand2.Caption = "Amount L.L.";
            this.treeListBand2.Columns.Add(this.colLAmountOpening);
            this.treeListBand2.Columns.Add(this.colLAmountDebit);
            this.treeListBand2.Columns.Add(this.colLAmountCredit);
            this.treeListBand2.Name = "treeListBand2";
            this.treeListBand2.Width = 360;
            // 
            // treeListBand3
            // 
            this.treeListBand3.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Bold);
            this.treeListBand3.AppearanceHeader.Options.UseFont = true;
            this.treeListBand3.AppearanceHeader.Options.UseTextOptions = true;
            this.treeListBand3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.treeListBand3.Caption = "Amount USD";
            this.treeListBand3.Columns.Add(this.colFAmountOpening);
            this.treeListBand3.Columns.Add(this.colFAmountDebit);
            this.treeListBand3.Columns.Add(this.colFAmountCredit);
            this.treeListBand3.Name = "treeListBand3";
            this.treeListBand3.Width = 322;
            // 
            // treeListBand8
            // 
            this.treeListBand8.Caption = "Misc";
            this.treeListBand8.Columns.Add(this.colNotes);
            this.treeListBand8.Columns.Add(this.colChartId);
            this.treeListBand8.MinWidth = 23;
            this.treeListBand8.Name = "treeListBand8";
            this.treeListBand8.Width = 271;
            // 
            // ChartsListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1412, 788);
            this.Controls.Add(this.mainLayout);
            this.Controls.Add(this.rcCharts);
            this.Controls.Add(this.ribbonStatusBar1);
            this.Name = "ChartsListForm";
            this.Ribbon = this.rcCharts;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.StatusBar = this.ribbonStatusBar1;
            this.Text = "Charts List";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChartsListForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.rcCharts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repWorkingYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainLayout)).EndInit();
            this.mainLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartTreeList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsCharts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		public DevExpress.XtraBars.Ribbon.RibbonControl rcCharts;
		private DevExpress.XtraBars.BarButtonItem btnNew;
		private DevExpress.XtraBars.BarButtonItem btnDelete;
		private DevExpress.XtraBars.BarButtonItem btnEdit;
		private DevExpress.XtraBars.BarButtonItem btnPrint;
		private DevExpress.XtraBars.BarButtonItem btnRefresh;
		private DevExpress.XtraBars.BarButtonItem btnClose;
		private DevExpress.XtraBars.BarButtonItem btnResetGridStyle;
		private DevExpress.XtraBars.Ribbon.RibbonPage rpCharts;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup6;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup8;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup11;
		private DevExpress.XtraBars.Ribbon.RibbonPage rpViewSettings;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
		private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar1;
        private DevExpress.XtraLayout.LayoutControl mainLayout;
        private DevExpress.XtraTreeList.TreeList chartTreeList;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colParentId;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colNumber;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colSerial;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colAccountName;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colAccountTypeId;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colLAmountOpening;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colLAmountDebit;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colLAmountCredit;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colFAmountOpening;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colFAmountDebit;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colFAmountCredit;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colNotes;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colChartId;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private System.Windows.Forms.BindingSource bsCharts;
        private DevExpress.XtraBars.BarButtonItem btnMigrateCharts;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup4;
        private DevExpress.XtraBars.BarEditItem cboWorkingYear;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repWorkingYear;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colArabicAccountName;
        private DevExpress.XtraTreeList.Columns.TreeListBand treeListBand5;
        private DevExpress.XtraTreeList.Columns.TreeListBand treeListBand6;
        private DevExpress.XtraTreeList.Columns.TreeListBand treeListBand7;
        private DevExpress.XtraTreeList.Columns.TreeListBand treeListBand2;
        private DevExpress.XtraTreeList.Columns.TreeListBand treeListBand3;
        private DevExpress.XtraTreeList.Columns.TreeListBand treeListBand8;
    }
}