namespace Spectrum.Views.Common.Departments
{
	partial class DepartmentsListForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DepartmentsListForm));
			this.rcDepartments = new DevExpress.XtraBars.Ribbon.RibbonControl();
			this.btnNew = new DevExpress.XtraBars.BarButtonItem();
			this.btnDelete = new DevExpress.XtraBars.BarButtonItem();
			this.btnEdit = new DevExpress.XtraBars.BarButtonItem();
			this.btnPrint = new DevExpress.XtraBars.BarButtonItem();
			this.btnRefresh = new DevExpress.XtraBars.BarButtonItem();
			this.btnClose = new DevExpress.XtraBars.BarButtonItem();
			this.btnResetGridStyle = new DevExpress.XtraBars.BarButtonItem();
			this.rpDepartments = new DevExpress.XtraBars.Ribbon.RibbonPage();
			this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.ribbonPageGroup6 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.ribbonPageGroup8 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.ribbonPageGroup11 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.rpViewSettings = new DevExpress.XtraBars.Ribbon.RibbonPage();
			this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.ribbonStatusBar1 = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
			this.mainLayout = new DevExpress.XtraLayout.LayoutControl();
			this.gcDepartments = new DevExpress.XtraGrid.GridControl();
			this.bsDepartments = new System.Windows.Forms.BindingSource(this.components);
			this.gvDepartments = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.col_id = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colDepartmentName = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colNotes = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colIsDefault = new DevExpress.XtraGrid.Columns.GridColumn();
			this.repCheckEdit = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
			this.colActive = new DevExpress.XtraGrid.Columns.GridColumn();
			this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
			this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
			((System.ComponentModel.ISupportInitialize)(this.rcDepartments)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.mainLayout)).BeginInit();
			this.mainLayout.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gcDepartments)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.bsDepartments)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gvDepartments)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repCheckEdit)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
			this.SuspendLayout();
			// 
			// rcDepartments
			// 
			this.rcDepartments.EmptyAreaImageOptions.ImagePadding = new System.Windows.Forms.Padding(35, 39, 35, 39);
			this.rcDepartments.ExpandCollapseItem.Id = 0;
			this.rcDepartments.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.rcDepartments.ExpandCollapseItem,
            this.btnNew,
            this.btnDelete,
            this.btnEdit,
            this.btnPrint,
            this.btnRefresh,
            this.btnClose,
            this.btnResetGridStyle});
			this.rcDepartments.Location = new System.Drawing.Point(0, 0);
			this.rcDepartments.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.rcDepartments.MaxItemId = 42;
			this.rcDepartments.MdiMergeStyle = DevExpress.XtraBars.Ribbon.RibbonMdiMergeStyle.Always;
			this.rcDepartments.Name = "rcDepartments";
			this.rcDepartments.OptionsMenuMinWidth = 385;
			this.rcDepartments.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.rpDepartments,
            this.rpViewSettings});
			this.rcDepartments.Size = new System.Drawing.Size(1382, 193);
			this.rcDepartments.StatusBar = this.ribbonStatusBar1;
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
			// btnResetGridStyle
			// 
			this.btnResetGridStyle.Caption = "Reset Grid Style";
			this.btnResetGridStyle.Id = 41;
			this.btnResetGridStyle.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnResetGridStyle.ImageOptions.SvgImage")));
			this.btnResetGridStyle.Name = "btnResetGridStyle";
			// 
			// rpDepartments
			// 
			this.rpDepartments.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1,
            this.ribbonPageGroup6,
            this.ribbonPageGroup8,
            this.ribbonPageGroup11});
			this.rpDepartments.Name = "rpDepartments";
			this.rpDepartments.Text = "DEPARTMENTS";
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
			this.ribbonStatusBar1.Location = new System.Drawing.Point(0, 752);
			this.ribbonStatusBar1.Name = "ribbonStatusBar1";
			this.ribbonStatusBar1.Ribbon = this.rcDepartments;
			this.ribbonStatusBar1.Size = new System.Drawing.Size(1382, 30);
			// 
			// mainLayout
			// 
			this.mainLayout.Controls.Add(this.gcDepartments);
			this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainLayout.Location = new System.Drawing.Point(0, 193);
			this.mainLayout.Name = "mainLayout";
			this.mainLayout.Root = this.Root;
			this.mainLayout.Size = new System.Drawing.Size(1382, 559);
			this.mainLayout.TabIndex = 5;
			this.mainLayout.Text = "layoutControl1";
			// 
			// gcDepartments
			// 
			this.gcDepartments.DataSource = this.bsDepartments;
			this.gcDepartments.EmbeddedNavigator.Buttons.Append.Visible = false;
			this.gcDepartments.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
			this.gcDepartments.EmbeddedNavigator.Buttons.Edit.Visible = false;
			this.gcDepartments.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
			this.gcDepartments.EmbeddedNavigator.Buttons.Remove.Visible = false;
			this.gcDepartments.Location = new System.Drawing.Point(14, 14);
			this.gcDepartments.MainView = this.gvDepartments;
			this.gcDepartments.Name = "gcDepartments";
			this.gcDepartments.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repCheckEdit});
			this.gcDepartments.Size = new System.Drawing.Size(1354, 531);
			this.gcDepartments.TabIndex = 8;
			this.gcDepartments.UseEmbeddedNavigator = true;
			this.gcDepartments.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDepartments});
			// 
			// bsDepartments
			// 
			this.bsDepartments.DataSource = typeof(Spectrum.Models.Common.Departments.DepartmentModel);
			// 
			// gvDepartments
			// 
			this.gvDepartments.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.col_id,
            this.colDepartmentName,
            this.colNotes,
            this.colIsDefault,
            this.colActive});
			this.gvDepartments.GridControl = this.gcDepartments;
			this.gvDepartments.Name = "gvDepartments";
			this.gvDepartments.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
			this.gvDepartments.OptionsBehavior.AutoExpandAllGroups = true;
			this.gvDepartments.OptionsBehavior.Editable = false;
			this.gvDepartments.OptionsBehavior.ReadOnly = true;
			this.gvDepartments.OptionsFind.AlwaysVisible = true;
			this.gvDepartments.OptionsView.ColumnAutoWidth = false;
			this.gvDepartments.OptionsView.ShowGroupedColumns = true;
			this.gvDepartments.OptionsView.ShowGroupPanel = false;
			// 
			// col_id
			// 
			this.col_id.FieldName = "_id";
			this.col_id.MinWidth = 25;
			this.col_id.Name = "col_id";
			this.col_id.Width = 94;
			// 
			// colDepartmentName
			// 
			this.colDepartmentName.FieldName = "DepartmentName";
			this.colDepartmentName.MinWidth = 25;
			this.colDepartmentName.Name = "colDepartmentName";
			this.colDepartmentName.Visible = true;
			this.colDepartmentName.VisibleIndex = 0;
			this.colDepartmentName.Width = 300;
			// 
			// colNotes
			// 
			this.colNotes.FieldName = "Notes";
			this.colNotes.MinWidth = 25;
			this.colNotes.Name = "colNotes";
			this.colNotes.Visible = true;
			this.colNotes.VisibleIndex = 1;
			this.colNotes.Width = 400;
			// 
			// colIsDefault
			// 
			this.colIsDefault.ColumnEdit = this.repCheckEdit;
			this.colIsDefault.FieldName = "IsDefault";
			this.colIsDefault.MinWidth = 25;
			this.colIsDefault.Name = "colIsDefault";
			this.colIsDefault.Visible = true;
			this.colIsDefault.VisibleIndex = 2;
			this.colIsDefault.Width = 94;
			// 
			// repCheckEdit
			// 
			this.repCheckEdit.AutoHeight = false;
			this.repCheckEdit.Name = "repCheckEdit";
			// 
			// colActive
			// 
			this.colActive.ColumnEdit = this.repCheckEdit;
			this.colActive.FieldName = "Active";
			this.colActive.MinWidth = 25;
			this.colActive.Name = "colActive";
			this.colActive.Visible = true;
			this.colActive.VisibleIndex = 3;
			this.colActive.Width = 94;
			// 
			// Root
			// 
			this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
			this.Root.GroupBordersVisible = false;
			this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
			this.Root.Name = "Root";
			this.Root.Size = new System.Drawing.Size(1382, 559);
			this.Root.TextVisible = false;
			// 
			// layoutControlItem1
			// 
			this.layoutControlItem1.Control = this.gcDepartments;
			this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
			this.layoutControlItem1.Name = "layoutControlItem1";
			this.layoutControlItem1.Size = new System.Drawing.Size(1358, 535);
			this.layoutControlItem1.TextVisible = false;
			// 
			// DepartmentsListForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1382, 782);
			this.Controls.Add(this.mainLayout);
			this.Controls.Add(this.ribbonStatusBar1);
			this.Controls.Add(this.rcDepartments);
			this.Name = "DepartmentsListForm";
			this.Ribbon = this.rcDepartments;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.StatusBar = this.ribbonStatusBar1;
			this.Text = "Departments List";
			((System.ComponentModel.ISupportInitialize)(this.rcDepartments)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.mainLayout)).EndInit();
			this.mainLayout.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.gcDepartments)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.bsDepartments)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gvDepartments)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repCheckEdit)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public DevExpress.XtraBars.Ribbon.RibbonControl rcDepartments;
		private DevExpress.XtraBars.BarButtonItem btnNew;
		private DevExpress.XtraBars.BarButtonItem btnDelete;
		private DevExpress.XtraBars.BarButtonItem btnEdit;
		private DevExpress.XtraBars.BarButtonItem btnPrint;
		private DevExpress.XtraBars.BarButtonItem btnRefresh;
		private DevExpress.XtraBars.BarButtonItem btnClose;
		private DevExpress.XtraBars.BarButtonItem btnResetGridStyle;
		private DevExpress.XtraBars.Ribbon.RibbonPage rpDepartments;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup6;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup8;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup11;
		private DevExpress.XtraBars.Ribbon.RibbonPage rpViewSettings;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
		private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar1;
		private DevExpress.XtraLayout.LayoutControl mainLayout;
		private DevExpress.XtraGrid.GridControl gcDepartments;
		private DevExpress.XtraGrid.Views.Grid.GridView gvDepartments;
		private DevExpress.XtraGrid.Columns.GridColumn col_id;
		private DevExpress.XtraGrid.Columns.GridColumn colDepartmentName;
		private DevExpress.XtraGrid.Columns.GridColumn colNotes;
		private DevExpress.XtraGrid.Columns.GridColumn colIsDefault;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repCheckEdit;
		private DevExpress.XtraGrid.Columns.GridColumn colActive;
		private DevExpress.XtraLayout.LayoutControlGroup Root;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
		private System.Windows.Forms.BindingSource bsDepartments;
	}
}