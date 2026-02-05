namespace Spectrum.Views.Common.Countries
{
	partial class CountriesListForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CountriesListForm));
			this.rcCountriesList = new DevExpress.XtraBars.Ribbon.RibbonControl();
			this.btnNew = new DevExpress.XtraBars.BarButtonItem();
			this.btnDelete = new DevExpress.XtraBars.BarButtonItem();
			this.btnEdit = new DevExpress.XtraBars.BarButtonItem();
			this.btnPrint = new DevExpress.XtraBars.BarButtonItem();
			this.btnRefresh = new DevExpress.XtraBars.BarButtonItem();
			this.btnClose = new DevExpress.XtraBars.BarButtonItem();
			this.btnResetGridStyle = new DevExpress.XtraBars.BarButtonItem();
			this.rpCountriesList = new DevExpress.XtraBars.Ribbon.RibbonPage();
			this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.ribbonPageGroup6 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.ribbonPageGroup8 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.ribbonPageGroup11 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.rpViewSettings = new DevExpress.XtraBars.Ribbon.RibbonPage();
			this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.ribbonStatusBar1 = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
			this.mainLayout = new DevExpress.XtraLayout.LayoutControl();
			this.gcCountries = new DevExpress.XtraGrid.GridControl();
			this.bsCountries = new System.Windows.Forms.BindingSource(this.components);
			this.gvCountries = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.col_id = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colCountryName = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colCountryCode = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colRegion = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colContinent = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colNotes = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colIsDefault = new DevExpress.XtraGrid.Columns.GridColumn();
			this.repCheckEdit = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
			this.colActive = new DevExpress.XtraGrid.Columns.GridColumn();
			this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
			this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
			((System.ComponentModel.ISupportInitialize)(this.rcCountriesList)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.mainLayout)).BeginInit();
			this.mainLayout.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gcCountries)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.bsCountries)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gvCountries)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repCheckEdit)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
			this.SuspendLayout();
			// 
			// rcCountriesList
			// 
			this.rcCountriesList.EmptyAreaImageOptions.ImagePadding = new System.Windows.Forms.Padding(35, 39, 35, 39);
			this.rcCountriesList.ExpandCollapseItem.Id = 0;
			this.rcCountriesList.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.rcCountriesList.ExpandCollapseItem,
            this.btnNew,
            this.btnDelete,
            this.btnEdit,
            this.btnPrint,
            this.btnRefresh,
            this.btnClose,
            this.btnResetGridStyle});
			this.rcCountriesList.Location = new System.Drawing.Point(0, 0);
			this.rcCountriesList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.rcCountriesList.MaxItemId = 42;
			this.rcCountriesList.MdiMergeStyle = DevExpress.XtraBars.Ribbon.RibbonMdiMergeStyle.Always;
			this.rcCountriesList.Name = "rcCountriesList";
			this.rcCountriesList.OptionsMenuMinWidth = 385;
			this.rcCountriesList.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.rpCountriesList,
            this.rpViewSettings});
			this.rcCountriesList.Size = new System.Drawing.Size(1420, 193);
			this.rcCountriesList.StatusBar = this.ribbonStatusBar1;
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
			// rpCountriesList
			// 
			this.rpCountriesList.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1,
            this.ribbonPageGroup6,
            this.ribbonPageGroup8,
            this.ribbonPageGroup11});
			this.rpCountriesList.Name = "rpCountriesList";
			this.rpCountriesList.Text = "COUNTRIES";
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
			this.ribbonStatusBar1.Location = new System.Drawing.Point(0, 685);
			this.ribbonStatusBar1.Name = "ribbonStatusBar1";
			this.ribbonStatusBar1.Ribbon = this.rcCountriesList;
			this.ribbonStatusBar1.Size = new System.Drawing.Size(1420, 30);
			// 
			// mainLayout
			// 
			this.mainLayout.Controls.Add(this.gcCountries);
			this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainLayout.Location = new System.Drawing.Point(0, 193);
			this.mainLayout.Name = "mainLayout";
			this.mainLayout.Root = this.Root;
			this.mainLayout.Size = new System.Drawing.Size(1420, 492);
			this.mainLayout.TabIndex = 6;
			this.mainLayout.Text = "layoutControl1";
			// 
			// gcCountries
			// 
			this.gcCountries.DataSource = this.bsCountries;
			this.gcCountries.EmbeddedNavigator.Buttons.Append.Visible = false;
			this.gcCountries.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
			this.gcCountries.EmbeddedNavigator.Buttons.Edit.Visible = false;
			this.gcCountries.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
			this.gcCountries.EmbeddedNavigator.Buttons.Remove.Visible = false;
			this.gcCountries.Location = new System.Drawing.Point(14, 14);
			this.gcCountries.MainView = this.gvCountries;
			this.gcCountries.Name = "gcCountries";
			this.gcCountries.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repCheckEdit});
			this.gcCountries.Size = new System.Drawing.Size(1392, 464);
			this.gcCountries.TabIndex = 8;
			this.gcCountries.UseEmbeddedNavigator = true;
			this.gcCountries.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvCountries});
			// 
			// bsCountries
			// 
			this.bsCountries.DataSource = typeof(Spectrum.Models.Common.Countries.CountryModel);
			// 
			// gvCountries
			// 
			this.gvCountries.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.col_id,
            this.colCountryName,
            this.colCountryCode,
            this.colRegion,
            this.colContinent,
            this.colNotes,
            this.colIsDefault,
            this.colActive});
			this.gvCountries.GridControl = this.gcCountries;
			this.gvCountries.Name = "gvCountries";
			this.gvCountries.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
			this.gvCountries.OptionsBehavior.AutoExpandAllGroups = true;
			this.gvCountries.OptionsBehavior.Editable = false;
			this.gvCountries.OptionsBehavior.ReadOnly = true;
			this.gvCountries.OptionsFind.AlwaysVisible = true;
			this.gvCountries.OptionsView.ColumnAutoWidth = false;
			this.gvCountries.OptionsView.ShowGroupedColumns = true;
			this.gvCountries.OptionsView.ShowGroupPanel = false;
			// 
			// col_id
			// 
			this.col_id.FieldName = "_id";
			this.col_id.MinWidth = 25;
			this.col_id.Name = "col_id";
			this.col_id.Width = 94;
			// 
			// colCountryName
			// 
			this.colCountryName.FieldName = "CountryName";
			this.colCountryName.MinWidth = 25;
			this.colCountryName.Name = "colCountryName";
			this.colCountryName.Visible = true;
			this.colCountryName.VisibleIndex = 0;
			this.colCountryName.Width = 300;
			// 
			// colCountryCode
			// 
			this.colCountryCode.Caption = "Country Code";
			this.colCountryCode.FieldName = "CountryCode";
			this.colCountryCode.MinWidth = 25;
			this.colCountryCode.Name = "colCountryCode";
			this.colCountryCode.Visible = true;
			this.colCountryCode.VisibleIndex = 1;
			this.colCountryCode.Width = 150;
			// 
			// colRegion
			// 
			this.colRegion.FieldName = "Region";
			this.colRegion.MinWidth = 25;
			this.colRegion.Name = "colRegion";
			this.colRegion.Visible = true;
			this.colRegion.VisibleIndex = 2;
			this.colRegion.Width = 200;
			// 
			// colContinent
			// 
			this.colContinent.FieldName = "Continent";
			this.colContinent.MinWidth = 25;
			this.colContinent.Name = "colContinent";
			this.colContinent.Visible = true;
			this.colContinent.VisibleIndex = 3;
			this.colContinent.Width = 200;
			// 
			// colNotes
			// 
			this.colNotes.FieldName = "Notes";
			this.colNotes.MinWidth = 25;
			this.colNotes.Name = "colNotes";
			this.colNotes.Visible = true;
			this.colNotes.VisibleIndex = 4;
			this.colNotes.Width = 400;
			// 
			// colIsDefault
			// 
			this.colIsDefault.ColumnEdit = this.repCheckEdit;
			this.colIsDefault.FieldName = "IsDefault";
			this.colIsDefault.MinWidth = 25;
			this.colIsDefault.Name = "colIsDefault";
			this.colIsDefault.Visible = true;
			this.colIsDefault.VisibleIndex = 5;
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
			this.colActive.VisibleIndex = 6;
			this.colActive.Width = 94;
			// 
			// Root
			// 
			this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
			this.Root.GroupBordersVisible = false;
			this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
			this.Root.Name = "Root";
			this.Root.Size = new System.Drawing.Size(1420, 492);
			this.Root.TextVisible = false;
			// 
			// layoutControlItem1
			// 
			this.layoutControlItem1.Control = this.gcCountries;
			this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
			this.layoutControlItem1.Name = "layoutControlItem1";
			this.layoutControlItem1.Size = new System.Drawing.Size(1396, 468);
			this.layoutControlItem1.TextVisible = false;
			// 
			// CountriesListForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1420, 715);
			this.Controls.Add(this.mainLayout);
			this.Controls.Add(this.ribbonStatusBar1);
			this.Controls.Add(this.rcCountriesList);
			this.Name = "CountriesListForm";
			this.Ribbon = this.rcCountriesList;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.StatusBar = this.ribbonStatusBar1;
			this.Text = "Countries List";
			((System.ComponentModel.ISupportInitialize)(this.rcCountriesList)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.mainLayout)).EndInit();
			this.mainLayout.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.gcCountries)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.bsCountries)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gvCountries)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repCheckEdit)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public DevExpress.XtraBars.Ribbon.RibbonControl rcCountriesList;
		private DevExpress.XtraBars.BarButtonItem btnNew;
		private DevExpress.XtraBars.BarButtonItem btnDelete;
		private DevExpress.XtraBars.BarButtonItem btnEdit;
		private DevExpress.XtraBars.BarButtonItem btnPrint;
		private DevExpress.XtraBars.BarButtonItem btnRefresh;
		private DevExpress.XtraBars.BarButtonItem btnClose;
		private DevExpress.XtraBars.BarButtonItem btnResetGridStyle;
		private DevExpress.XtraBars.Ribbon.RibbonPage rpCountriesList;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup6;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup8;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup11;
		private DevExpress.XtraBars.Ribbon.RibbonPage rpViewSettings;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
		private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar1;
		private DevExpress.XtraLayout.LayoutControl mainLayout;
		private DevExpress.XtraGrid.GridControl gcCountries;
		private DevExpress.XtraGrid.Views.Grid.GridView gvCountries;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repCheckEdit;
		private DevExpress.XtraLayout.LayoutControlGroup Root;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
		private System.Windows.Forms.BindingSource bsCountries;
		private DevExpress.XtraGrid.Columns.GridColumn col_id;
		private DevExpress.XtraGrid.Columns.GridColumn colCountryName;
		private DevExpress.XtraGrid.Columns.GridColumn colCountryCode;
		private DevExpress.XtraGrid.Columns.GridColumn colRegion;
		private DevExpress.XtraGrid.Columns.GridColumn colContinent;
		private DevExpress.XtraGrid.Columns.GridColumn colNotes;
		private DevExpress.XtraGrid.Columns.GridColumn colIsDefault;
		private DevExpress.XtraGrid.Columns.GridColumn colActive;
	}
}