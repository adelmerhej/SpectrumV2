namespace Spectrum.Views.Transactions.Invoices
{
	partial class MyInvoiceForm
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
			this.rcMyInvoice = new DevExpress.XtraBars.Ribbon.RibbonControl();
			this.btnRefresh = new DevExpress.XtraBars.BarButtonItem();
			this.btnClose = new DevExpress.XtraBars.BarButtonItem();
			this.rpMyInvoice = new DevExpress.XtraBars.Ribbon.RibbonPage();
			this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.ribbonStatusBar1 = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
			this.mainLayout = new DevExpress.XtraLayout.LayoutControl();
			this.gcMyInvoice = new DevExpress.XtraGrid.GridControl();
			this.gvMyInvoice = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.colId = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colInvoiceNumber = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colCustomerName = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colDescription = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colQuantity = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colUnitPrice = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colTotalAmount = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colInvoiceDate = new DevExpress.XtraGrid.Columns.GridColumn();
			this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
			this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
			((System.ComponentModel.ISupportInitialize)(this.rcMyInvoice)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.mainLayout)).BeginInit();
			this.mainLayout.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gcMyInvoice)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gvMyInvoice)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
			this.SuspendLayout();
			// 
			// rcMyInvoice
			// 
			this.rcMyInvoice.ExpandCollapseItem.Id = 0;
			this.rcMyInvoice.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
				this.rcMyInvoice.ExpandCollapseItem,
				this.btnRefresh,
				this.btnClose});
			this.rcMyInvoice.Location = new System.Drawing.Point(0, 0);
			this.rcMyInvoice.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.rcMyInvoice.MaxItemId = 3;
			this.rcMyInvoice.MdiMergeStyle = DevExpress.XtraBars.Ribbon.RibbonMdiMergeStyle.Always;
			this.rcMyInvoice.Name = "rcMyInvoice";
			this.rcMyInvoice.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
				this.rpMyInvoice});
			this.rcMyInvoice.Size = new System.Drawing.Size(1024, 158);
			this.rcMyInvoice.StatusBar = this.ribbonStatusBar1;
			// 
			// btnRefresh
			// 
			this.btnRefresh.Caption = "Refresh";
			this.btnRefresh.Id = 1;
			this.btnRefresh.ImageOptions.ImageUri.Uri = "Refresh";
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnRefresh_ItemClick);
			// 
			// btnClose
			// 
			this.btnClose.Caption = "Close";
			this.btnClose.Id = 2;
			this.btnClose.ImageOptions.ImageUri.Uri = "Close";
			this.btnClose.Name = "btnClose";
			this.btnClose.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnClose_ItemClick);
			// 
			// rpMyInvoice
			// 
			this.rpMyInvoice.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
				this.ribbonPageGroup1,
				this.ribbonPageGroup2});
			this.rpMyInvoice.Name = "rpMyInvoice";
			this.rpMyInvoice.Text = "MY INVOICE";
			// 
			// ribbonPageGroup1
			// 
			this.ribbonPageGroup1.AllowTextClipping = false;
			this.ribbonPageGroup1.CaptionButtonVisible = DevExpress.Utils.DefaultBoolean.False;
			this.ribbonPageGroup1.ItemLinks.Add(this.btnRefresh);
			this.ribbonPageGroup1.MergeOrder = 0;
			this.ribbonPageGroup1.Name = "ribbonPageGroup1";
			this.ribbonPageGroup1.Text = "Actions";
			// 
			// ribbonPageGroup2
			// 
			this.ribbonPageGroup2.Alignment = DevExpress.XtraBars.Ribbon.RibbonPageGroupAlignment.Far;
			this.ribbonPageGroup2.ItemLinks.Add(this.btnClose);
			this.ribbonPageGroup2.Name = "ribbonPageGroup2";
			this.ribbonPageGroup2.Text = "Close View";
			// 
			// ribbonStatusBar1
			// 
			this.ribbonStatusBar1.Location = new System.Drawing.Point(0, 568);
			this.ribbonStatusBar1.Name = "ribbonStatusBar1";
			this.ribbonStatusBar1.Ribbon = this.rcMyInvoice;
			this.ribbonStatusBar1.Size = new System.Drawing.Size(1024, 30);
			// 
			// mainLayout
			// 
			this.mainLayout.Controls.Add(this.gcMyInvoice);
			this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainLayout.Location = new System.Drawing.Point(0, 158);
			this.mainLayout.Name = "mainLayout";
			this.mainLayout.Root = this.Root;
			this.mainLayout.Size = new System.Drawing.Size(1024, 410);
			this.mainLayout.TabIndex = 2;
			this.mainLayout.Text = "layoutControl1";
			// 
			// gcMyInvoice
			// 
			this.gcMyInvoice.Location = new System.Drawing.Point(12, 12);
			this.gcMyInvoice.MainView = this.gvMyInvoice;
			this.gcMyInvoice.Name = "gcMyInvoice";
			this.gcMyInvoice.Size = new System.Drawing.Size(1000, 386);
			this.gcMyInvoice.TabIndex = 4;
			this.gcMyInvoice.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
				this.gvMyInvoice});
			// 
			// gvMyInvoice
			// 
			this.gvMyInvoice.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
				this.colId,
				this.colInvoiceNumber,
				this.colCustomerName,
				this.colDescription,
				this.colQuantity,
				this.colUnitPrice,
				this.colTotalAmount,
				this.colInvoiceDate});
			this.gvMyInvoice.GridControl = this.gcMyInvoice;
			this.gvMyInvoice.Name = "gvMyInvoice";
			this.gvMyInvoice.OptionsBehavior.Editable = false;
			this.gvMyInvoice.OptionsBehavior.ReadOnly = true;
			this.gvMyInvoice.OptionsFind.AlwaysVisible = true;
			this.gvMyInvoice.OptionsView.ShowGroupPanel = false;
			// 
			// colId
			// 
			this.colId.Caption = "ID";
			this.colId.FieldName = "Id";
			this.colId.Name = "colId";
			this.colId.Visible = true;
			this.colId.VisibleIndex = 0;
			this.colId.Width = 50;
			// 
			// colInvoiceNumber
			// 
			this.colInvoiceNumber.Caption = "Invoice #";
			this.colInvoiceNumber.FieldName = "InvoiceNumber";
			this.colInvoiceNumber.Name = "colInvoiceNumber";
			this.colInvoiceNumber.Visible = true;
			this.colInvoiceNumber.VisibleIndex = 1;
			this.colInvoiceNumber.Width = 100;
			// 
			// colCustomerName
			// 
			this.colCustomerName.Caption = "Customer";
			this.colCustomerName.FieldName = "CustomerName";
			this.colCustomerName.Name = "colCustomerName";
			this.colCustomerName.Visible = true;
			this.colCustomerName.VisibleIndex = 2;
			this.colCustomerName.Width = 150;
			// 
			// colDescription
			// 
			this.colDescription.Caption = "Description";
			this.colDescription.FieldName = "Description";
			this.colDescription.Name = "colDescription";
			this.colDescription.Visible = true;
			this.colDescription.VisibleIndex = 3;
			this.colDescription.Width = 200;
			// 
			// colQuantity
			// 
			this.colQuantity.Caption = "Qty";
			this.colQuantity.FieldName = "Quantity";
			this.colQuantity.Name = "colQuantity";
			this.colQuantity.Visible = true;
			this.colQuantity.VisibleIndex = 4;
			this.colQuantity.Width = 60;
			// 
			// colUnitPrice
			// 
			this.colUnitPrice.Caption = "Unit Price";
			this.colUnitPrice.DisplayFormat.FormatString = "c2";
			this.colUnitPrice.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
			this.colUnitPrice.FieldName = "UnitPrice";
			this.colUnitPrice.Name = "colUnitPrice";
			this.colUnitPrice.Visible = true;
			this.colUnitPrice.VisibleIndex = 5;
			this.colUnitPrice.Width = 100;
			// 
			// colTotalAmount
			// 
			this.colTotalAmount.Caption = "Total";
			this.colTotalAmount.DisplayFormat.FormatString = "c2";
			this.colTotalAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
			this.colTotalAmount.FieldName = "TotalAmount";
			this.colTotalAmount.Name = "colTotalAmount";
			this.colTotalAmount.Visible = true;
			this.colTotalAmount.VisibleIndex = 6;
			this.colTotalAmount.Width = 100;
			// 
			// colInvoiceDate
			// 
			this.colInvoiceDate.Caption = "Date";
			this.colInvoiceDate.DisplayFormat.FormatString = "d";
			this.colInvoiceDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
			this.colInvoiceDate.FieldName = "InvoiceDate";
			this.colInvoiceDate.Name = "colInvoiceDate";
			this.colInvoiceDate.Visible = true;
			this.colInvoiceDate.VisibleIndex = 7;
			this.colInvoiceDate.Width = 100;
			// 
			// Root
			// 
			this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
			this.Root.GroupBordersVisible = false;
			this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
				this.layoutControlItem1});
			this.Root.Name = "Root";
			this.Root.Size = new System.Drawing.Size(1024, 410);
			this.Root.TextVisible = false;
			// 
			// layoutControlItem1
			// 
			this.layoutControlItem1.Control = this.gcMyInvoice;
			this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
			this.layoutControlItem1.Name = "layoutControlItem1";
			this.layoutControlItem1.Size = new System.Drawing.Size(1004, 390);
			this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem1.TextVisible = false;
			// 
			// MyInvoiceForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1024, 598);
			this.Controls.Add(this.mainLayout);
			this.Controls.Add(this.ribbonStatusBar1);
			this.Controls.Add(this.rcMyInvoice);
			this.Name = "MyInvoiceForm";
			this.Ribbon = this.rcMyInvoice;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.StatusBar = this.ribbonStatusBar1;
			this.Text = "My Invoice";
			((System.ComponentModel.ISupportInitialize)(this.rcMyInvoice)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.mainLayout)).EndInit();
			this.mainLayout.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.gcMyInvoice)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gvMyInvoice)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		#endregion

		private DevExpress.XtraBars.Ribbon.RibbonControl rcMyInvoice;
		private DevExpress.XtraBars.BarButtonItem btnRefresh;
		private DevExpress.XtraBars.BarButtonItem btnClose;
		private DevExpress.XtraBars.Ribbon.RibbonPage rpMyInvoice;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
		private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar1;
		private DevExpress.XtraLayout.LayoutControl mainLayout;
		private DevExpress.XtraGrid.GridControl gcMyInvoice;
		private DevExpress.XtraGrid.Views.Grid.GridView gvMyInvoice;
		private DevExpress.XtraGrid.Columns.GridColumn colId;
		private DevExpress.XtraGrid.Columns.GridColumn colInvoiceNumber;
		private DevExpress.XtraGrid.Columns.GridColumn colCustomerName;
		private DevExpress.XtraGrid.Columns.GridColumn colDescription;
		private DevExpress.XtraGrid.Columns.GridColumn colQuantity;
		private DevExpress.XtraGrid.Columns.GridColumn colUnitPrice;
		private DevExpress.XtraGrid.Columns.GridColumn colTotalAmount;
		private DevExpress.XtraGrid.Columns.GridColumn colInvoiceDate;
		private DevExpress.XtraLayout.LayoutControlGroup Root;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
	}
}
