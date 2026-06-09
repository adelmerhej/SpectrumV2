using Spectrum.Models.Accounting.Invoices;

namespace Spectrum.Views.Transactions.Invoices
{
	partial class InvoicesListForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InvoicesListForm));
            this.gvInvoiceDetails = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repTextEdit = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repCheckEdit = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn12 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn13 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcInvoices = new DevExpress.XtraGrid.GridControl();
            this.bsInvoices = new System.Windows.Forms.BindingSource(this.components);
            this.gvInvoices = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.col_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colProjectName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colInvoiceNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colInvoiceDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repDateEdit = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.colAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colVAT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPaid = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPaidAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colBank = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNotes = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colActive = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rcInvoices = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.btnNew = new DevExpress.XtraBars.BarButtonItem();
            this.btnDelete = new DevExpress.XtraBars.BarButtonItem();
            this.btnEdit = new DevExpress.XtraBars.BarButtonItem();
            this.btnPrint = new DevExpress.XtraBars.BarButtonItem();
            this.btnRefresh = new DevExpress.XtraBars.BarButtonItem();
            this.btnClose = new DevExpress.XtraBars.BarButtonItem();
            this.btnResetGridStyle = new DevExpress.XtraBars.BarButtonItem();
            this.rpInvoices = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup6 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup8 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup11 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.rpViewSettings = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonStatusBar1 = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.mainLayout = new DevExpress.XtraLayout.LayoutControl();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.colMemberId = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gvInvoiceDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repTextEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repCheckEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcInvoices)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsInvoices)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvInvoices)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repDateEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repDateEdit.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rcInvoices)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainLayout)).BeginInit();
            this.mainLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // gvInvoiceDetails
            // 
            this.gvInvoiceDetails.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn8,
            this.gridColumn9,
            this.gridColumn10,
            this.gridColumn11,
            this.gridColumn12,
            this.gridColumn13});
            this.gvInvoiceDetails.GridControl = this.gcInvoices;
            this.gvInvoiceDetails.Name = "gvInvoiceDetails";
            this.gvInvoiceDetails.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gvInvoiceDetails.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gvInvoiceDetails.OptionsBehavior.Editable = false;
            this.gvInvoiceDetails.OptionsBehavior.ReadOnly = true;
            this.gvInvoiceDetails.OptionsView.ColumnAutoWidth = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "Id";
            this.gridColumn1.FieldName = "_id";
            this.gridColumn1.MinWidth = 25;
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Width = 87;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "Line#";
            this.gridColumn2.FieldName = "LineNo";
            this.gridColumn2.MinWidth = 25;
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 0;
            this.gridColumn2.Width = 60;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "Item Code";
            this.gridColumn3.FieldName = "ItemCode";
            this.gridColumn3.MinWidth = 25;
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 1;
            this.gridColumn3.Width = 89;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "Description";
            this.gridColumn4.FieldName = "Description";
            this.gridColumn4.MinWidth = 25;
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 2;
            this.gridColumn4.Width = 280;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "Currency";
            this.gridColumn5.FieldName = "Currency";
            this.gridColumn5.MinWidth = 25;
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 3;
            this.gridColumn5.Width = 73;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "Rate";
            this.gridColumn6.ColumnEdit = this.repTextEdit;
            this.gridColumn6.FieldName = "Rate";
            this.gridColumn6.MinWidth = 25;
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 4;
            this.gridColumn6.Width = 101;
            // 
            // repTextEdit
            // 
            this.repTextEdit.AutoHeight = false;
            this.repTextEdit.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.NumericMaskManager));
            this.repTextEdit.MaskSettings.Set("mask", "n");
            this.repTextEdit.Name = "repTextEdit";
            this.repTextEdit.UseMaskAsDisplayFormat = true;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "Amount";
            this.gridColumn7.ColumnEdit = this.repTextEdit;
            this.gridColumn7.FieldName = "Amount";
            this.gridColumn7.MinWidth = 25;
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 5;
            this.gridColumn7.Width = 113;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "L.L.";
            this.gridColumn8.ColumnEdit = this.repTextEdit;
            this.gridColumn8.FieldName = "LAmount";
            this.gridColumn8.MinWidth = 25;
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 6;
            this.gridColumn8.Width = 104;
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption = "USD";
            this.gridColumn9.ColumnEdit = this.repTextEdit;
            this.gridColumn9.FieldName = "FAmount";
            this.gridColumn9.MinWidth = 25;
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 7;
            this.gridColumn9.Width = 120;
            // 
            // gridColumn10
            // 
            this.gridColumn10.Caption = "VAT";
            this.gridColumn10.ColumnEdit = this.repCheckEdit;
            this.gridColumn10.FieldName = "VAT";
            this.gridColumn10.MinWidth = 25;
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 8;
            this.gridColumn10.Width = 45;
            // 
            // repCheckEdit
            // 
            this.repCheckEdit.AutoHeight = false;
            this.repCheckEdit.Name = "repCheckEdit";
            // 
            // gridColumn11
            // 
            this.gridColumn11.Caption = "VAT Rate";
            this.gridColumn11.ColumnEdit = this.repTextEdit;
            this.gridColumn11.FieldName = "VATRate";
            this.gridColumn11.MinWidth = 25;
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.Visible = true;
            this.gridColumn11.VisibleIndex = 9;
            this.gridColumn11.Width = 99;
            // 
            // gridColumn12
            // 
            this.gridColumn12.Caption = "VAT Value";
            this.gridColumn12.ColumnEdit = this.repTextEdit;
            this.gridColumn12.FieldName = "VATValue";
            this.gridColumn12.MinWidth = 25;
            this.gridColumn12.Name = "gridColumn12";
            this.gridColumn12.Visible = true;
            this.gridColumn12.VisibleIndex = 10;
            this.gridColumn12.Width = 95;
            // 
            // gridColumn13
            // 
            this.gridColumn13.Caption = "Notes";
            this.gridColumn13.FieldName = "Notes";
            this.gridColumn13.MinWidth = 25;
            this.gridColumn13.Name = "gridColumn13";
            this.gridColumn13.Visible = true;
            this.gridColumn13.VisibleIndex = 11;
            this.gridColumn13.Width = 228;
            // 
            // gcInvoices
            // 
            this.gcInvoices.DataSource = this.bsInvoices;
            this.gcInvoices.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcInvoices.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcInvoices.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcInvoices.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcInvoices.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcInvoices.Location = new System.Drawing.Point(14, 14);
            this.gcInvoices.MainView = this.gvInvoices;
            this.gcInvoices.Name = "gcInvoices";
            this.gcInvoices.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repCheckEdit,
            this.repDateEdit,
            this.repTextEdit});
            this.gcInvoices.Size = new System.Drawing.Size(1353, 544);
            this.gcInvoices.TabIndex = 8;
            this.gcInvoices.UseEmbeddedNavigator = true;
            this.gcInvoices.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvInvoices,
            this.gvInvoiceDetails});
            // 
            // bsInvoices
            // 
            this.bsInvoices.DataSource = typeof(Spectrum.Models.Accounting.Invoices.InvoiceModel);
            // 
            // gvInvoices
            // 
            this.gvInvoices.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.col_id,
            this.colProjectName,
            this.colInvoiceNumber,
            this.colInvoiceDate,
            this.colMemberId,
            this.colAmount,
            this.colVAT,
            this.colPaid,
            this.colPaidAmount,
            this.colBank,
            this.colNotes,
            this.colActive});
            this.gvInvoices.GridControl = this.gcInvoices;
            this.gvInvoices.Name = "gvInvoices";
            this.gvInvoices.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvInvoices.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvInvoices.OptionsBehavior.Editable = false;
            this.gvInvoices.OptionsBehavior.ReadOnly = true;
            this.gvInvoices.OptionsFind.AlwaysVisible = true;
            this.gvInvoices.OptionsView.ColumnAutoWidth = false;
            this.gvInvoices.OptionsView.ShowGroupedColumns = true;
            this.gvInvoices.OptionsView.ShowGroupPanel = false;
            this.gvInvoices.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gvInvoices_RowCellStyle);
            this.gvInvoices.DoubleClick += new System.EventHandler(this.gvInvoices_DoubleClick);
            // 
            // col_id
            // 
            this.col_id.FieldName = "_id";
            this.col_id.MinWidth = 25;
            this.col_id.Name = "col_id";
            this.col_id.Width = 94;
            // 
            // colProjectName
            // 
            this.colProjectName.Caption = "Project";
            this.colProjectName.FieldName = "Reference";
            this.colProjectName.MinWidth = 25;
            this.colProjectName.Name = "colProjectName";
            this.colProjectName.Visible = true;
            this.colProjectName.VisibleIndex = 0;
            this.colProjectName.Width = 143;
            // 
            // colInvoiceNumber
            // 
            this.colInvoiceNumber.FieldName = "InvoiceNo";
            this.colInvoiceNumber.MinWidth = 25;
            this.colInvoiceNumber.Name = "colInvoiceNumber";
            this.colInvoiceNumber.Visible = true;
            this.colInvoiceNumber.VisibleIndex = 1;
            this.colInvoiceNumber.Width = 125;
            // 
            // colInvoiceDate
            // 
            this.colInvoiceDate.ColumnEdit = this.repDateEdit;
            this.colInvoiceDate.FieldName = "InvoiceDate";
            this.colInvoiceDate.MinWidth = 25;
            this.colInvoiceDate.Name = "colInvoiceDate";
            this.colInvoiceDate.Visible = true;
            this.colInvoiceDate.VisibleIndex = 2;
            this.colInvoiceDate.Width = 135;
            // 
            // repDateEdit
            // 
            this.repDateEdit.AutoHeight = false;
            this.repDateEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repDateEdit.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repDateEdit.CalendarTimeProperties.MaskSettings.Set("mask", "dd/MM/yyyy");
            this.repDateEdit.CalendarTimeProperties.UseMaskAsDisplayFormat = true;
            this.repDateEdit.MaskSettings.Set("mask", "dd/MM/yyyy");
            this.repDateEdit.Name = "repDateEdit";
            this.repDateEdit.UseMaskAsDisplayFormat = true;
            // 
            // colAmount
            // 
            this.colAmount.ColumnEdit = this.repTextEdit;
            this.colAmount.FieldName = "TotalAmount";
            this.colAmount.MinWidth = 25;
            this.colAmount.Name = "colAmount";
            this.colAmount.Visible = true;
            this.colAmount.VisibleIndex = 3;
            this.colAmount.Width = 159;
            // 
            // colVAT
            // 
            this.colVAT.ColumnEdit = this.repTextEdit;
            this.colVAT.FieldName = "VAT";
            this.colVAT.MinWidth = 25;
            this.colVAT.Name = "colVAT";
            this.colVAT.Visible = true;
            this.colVAT.VisibleIndex = 5;
            this.colVAT.Width = 105;
            // 
            // colPaid
            // 
            this.colPaid.ColumnEdit = this.repCheckEdit;
            this.colPaid.FieldName = "Paid";
            this.colPaid.MinWidth = 25;
            this.colPaid.Name = "colPaid";
            this.colPaid.Visible = true;
            this.colPaid.VisibleIndex = 6;
            this.colPaid.Width = 81;
            // 
            // colPaidAmount
            // 
            this.colPaidAmount.ColumnEdit = this.repTextEdit;
            this.colPaidAmount.FieldName = "PaidAmount";
            this.colPaidAmount.MinWidth = 25;
            this.colPaidAmount.Name = "colPaidAmount";
            this.colPaidAmount.Visible = true;
            this.colPaidAmount.VisibleIndex = 7;
            this.colPaidAmount.Width = 183;
            // 
            // colBank
            // 
            this.colBank.FieldName = "Bank";
            this.colBank.MinWidth = 25;
            this.colBank.Name = "colBank";
            this.colBank.Visible = true;
            this.colBank.VisibleIndex = 8;
            this.colBank.Width = 235;
            // 
            // colNotes
            // 
            this.colNotes.FieldName = "Notes";
            this.colNotes.MinWidth = 25;
            this.colNotes.Name = "colNotes";
            this.colNotes.Visible = true;
            this.colNotes.VisibleIndex = 9;
            this.colNotes.Width = 276;
            // 
            // colActive
            // 
            this.colActive.ColumnEdit = this.repCheckEdit;
            this.colActive.FieldName = "Active";
            this.colActive.MinWidth = 25;
            this.colActive.Name = "colActive";
            this.colActive.Visible = true;
            this.colActive.VisibleIndex = 10;
            this.colActive.Width = 94;
            // 
            // rcInvoices
            // 
            this.rcInvoices.EmptyAreaImageOptions.ImagePadding = new System.Windows.Forms.Padding(35, 39, 35, 39);
            this.rcInvoices.ExpandCollapseItem.Id = 0;
            this.rcInvoices.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.rcInvoices.ExpandCollapseItem,
            this.btnNew,
            this.btnDelete,
            this.btnEdit,
            this.btnPrint,
            this.btnRefresh,
            this.btnClose,
            this.btnResetGridStyle});
            this.rcInvoices.Location = new System.Drawing.Point(0, 0);
            this.rcInvoices.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rcInvoices.MaxItemId = 42;
            this.rcInvoices.MdiMergeStyle = DevExpress.XtraBars.Ribbon.RibbonMdiMergeStyle.Always;
            this.rcInvoices.Name = "rcInvoices";
            this.rcInvoices.OptionsMenuMinWidth = 385;
            this.rcInvoices.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.rpInvoices,
            this.rpViewSettings});
            this.rcInvoices.Size = new System.Drawing.Size(1381, 193);
            this.rcInvoices.StatusBar = this.ribbonStatusBar1;
            // 
            // btnNew
            // 
            this.btnNew.Caption = "Add New";
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
            // rpInvoices
            // 
            this.rpInvoices.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1,
            this.ribbonPageGroup6,
            this.ribbonPageGroup8,
            this.ribbonPageGroup11});
            this.rpInvoices.Name = "rpInvoices";
            this.rpInvoices.Text = "INVOICES";
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
            this.ribbonStatusBar1.Location = new System.Drawing.Point(0, 765);
            this.ribbonStatusBar1.Name = "ribbonStatusBar1";
            this.ribbonStatusBar1.Ribbon = this.rcInvoices;
            this.ribbonStatusBar1.Size = new System.Drawing.Size(1381, 30);
            // 
            // mainLayout
            // 
            this.mainLayout.Controls.Add(this.gcInvoices);
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.Location = new System.Drawing.Point(0, 193);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.Root = this.Root;
            this.mainLayout.Size = new System.Drawing.Size(1381, 572);
            this.mainLayout.TabIndex = 6;
            this.mainLayout.Text = "layoutControl1";
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(1381, 572);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.gcInvoices;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1357, 548);
            this.layoutControlItem1.TextVisible = false;
            // 
            // colMemberId
            // 
            this.colMemberId.Caption = "Member Name";
            this.colMemberId.FieldName = "MemberId";
            this.colMemberId.MinWidth = 25;
            this.colMemberId.Name = "colMemberId";
            this.colMemberId.Visible = true;
            this.colMemberId.VisibleIndex = 4;
            this.colMemberId.Width = 94;
            // 
            // InvoicesListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1381, 795);
            this.Controls.Add(this.mainLayout);
            this.Controls.Add(this.rcInvoices);
            this.Controls.Add(this.ribbonStatusBar1);
            this.Name = "InvoicesListForm";
            this.Ribbon = this.rcInvoices;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.StatusBar = this.ribbonStatusBar1;
            this.Text = "Invoices List";
            ((System.ComponentModel.ISupportInitialize)(this.gvInvoiceDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repTextEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repCheckEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcInvoices)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsInvoices)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvInvoices)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repDateEdit.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repDateEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rcInvoices)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainLayout)).EndInit();
            this.mainLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		public DevExpress.XtraBars.Ribbon.RibbonControl rcInvoices;
		private DevExpress.XtraBars.BarButtonItem btnNew;
		private DevExpress.XtraBars.BarButtonItem btnDelete;
		private DevExpress.XtraBars.BarButtonItem btnEdit;
		private DevExpress.XtraBars.BarButtonItem btnPrint;
		private DevExpress.XtraBars.BarButtonItem btnRefresh;
		private DevExpress.XtraBars.BarButtonItem btnClose;
		private DevExpress.XtraBars.BarButtonItem btnResetGridStyle;
		private DevExpress.XtraBars.Ribbon.RibbonPage rpInvoices;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup6;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup8;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup11;
		private DevExpress.XtraBars.Ribbon.RibbonPage rpViewSettings;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
		private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar1;
		private DevExpress.XtraLayout.LayoutControl mainLayout;
		private DevExpress.XtraGrid.GridControl gcInvoices;
		private DevExpress.XtraGrid.Views.Grid.GridView gvInvoices;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repCheckEdit;
		private DevExpress.XtraLayout.LayoutControlGroup Root;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
		private System.Windows.Forms.BindingSource bsInvoices;
		private DevExpress.XtraGrid.Columns.GridColumn col_id;
		private DevExpress.XtraGrid.Columns.GridColumn colInvoiceNumber;
		private DevExpress.XtraGrid.Columns.GridColumn colInvoiceDate;
		private DevExpress.XtraGrid.Columns.GridColumn colAmount;
		private DevExpress.XtraGrid.Columns.GridColumn colVAT;
		private DevExpress.XtraGrid.Columns.GridColumn colPaid;
		private DevExpress.XtraGrid.Columns.GridColumn colPaidAmount;
		private DevExpress.XtraGrid.Columns.GridColumn colBank;
		private DevExpress.XtraGrid.Columns.GridColumn colNotes;
		private DevExpress.XtraGrid.Columns.GridColumn colActive;
		private DevExpress.XtraGrid.Columns.GridColumn colProjectName;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repDateEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repTextEdit;
        private DevExpress.XtraGrid.Views.Grid.GridView gvInvoiceDetails;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn12;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn13;
        private DevExpress.XtraGrid.Columns.GridColumn colMemberId;
    }
}