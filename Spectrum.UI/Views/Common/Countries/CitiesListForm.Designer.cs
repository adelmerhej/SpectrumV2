namespace Spectrum.Views.Common.Countries
{
	partial class CitiesListForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CitiesListForm));
			this.rcCitiesList = new DevExpress.XtraBars.Ribbon.RibbonControl();
			this.btnNew = new DevExpress.XtraBars.BarButtonItem();
			this.btnDelete = new DevExpress.XtraBars.BarButtonItem();
			this.btnEdit = new DevExpress.XtraBars.BarButtonItem();
			this.btnPrint = new DevExpress.XtraBars.BarButtonItem();
			this.btnRefresh = new DevExpress.XtraBars.BarButtonItem();
			this.btnClose = new DevExpress.XtraBars.BarButtonItem();
			this.btnResetGridStyle = new DevExpress.XtraBars.BarButtonItem();
			this.rpCitiesList = new DevExpress.XtraBars.Ribbon.RibbonPage();
			this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.ribbonPageGroup6 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.ribbonPageGroup8 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.ribbonPageGroup11 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.rpViewSettings = new DevExpress.XtraBars.Ribbon.RibbonPage();
			this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.ribbonStatusBar1 = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
			this.mainLayout = new DevExpress.XtraLayout.LayoutControl();
			this.gcCities = new DevExpress.XtraGrid.GridControl();
			this.bsCities = new System.Windows.Forms.BindingSource(this.components);
			this.gvCities = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.col_id = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colCityName = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colCityCode = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colCountry = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colProvince = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colDistrict = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colRegion = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colContinent = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colPopulation = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colIsCapital = new DevExpress.XtraGrid.Columns.GridColumn();
			this.repCheckEdit = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
			this.colNotes = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colIsDefault = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colActive = new DevExpress.XtraGrid.Columns.GridColumn();
			this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
			this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
			((System.ComponentModel.ISupportInitialize)(this.rcCitiesList)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.mainLayout)).BeginInit();
			this.mainLayout.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gcCities)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.bsCities)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gvCities)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repCheckEdit)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
			this.SuspendLayout();
			// 
			// rcCitiesList
			// 
			this.rcCitiesList.EmptyAreaImageOptions.ImagePadding = new System.Windows.Forms.Padding(35, 39, 35, 39);
			this.rcCitiesList.ExpandCollapseItem.Id = 0;
			this.rcCitiesList.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.rcCitiesList.ExpandCollapseItem,
            this.btnNew,
            this.btnDelete,
            this.btnEdit,
            this.btnPrint,
            this.btnRefresh,
            this.btnClose,
            this.btnResetGridStyle});
			this.rcCitiesList.Location = new System.Drawing.Point(0, 0);
			this.rcCitiesList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.rcCitiesList.MaxItemId = 43;
			this.rcCitiesList.MdiMergeStyle = DevExpress.XtraBars.Ribbon.RibbonMdiMergeStyle.Always;
			this.rcCitiesList.Name = "rcCitiesList";
			this.rcCitiesList.OptionsMenuMinWidth = 385;
			this.rcCitiesList.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.rpCitiesList,
            this.rpViewSettings});
			this.rcCitiesList.Size = new System.Drawing.Size(1425, 193);
			this.rcCitiesList.StatusBar = this.ribbonStatusBar1;
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
			this.btnResetGridStyle.Id = 42;
			this.btnResetGridStyle.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnResetGridStyle.ImageOptions.SvgImage")));
			this.btnResetGridStyle.Name = "btnResetGridStyle";
			// 
			// rpCitiesList
			// 
			this.rpCitiesList.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1,
            this.ribbonPageGroup6,
            this.ribbonPageGroup8,
            this.ribbonPageGroup11});
			this.rpCitiesList.Name = "rpCitiesList";
			this.rpCitiesList.Text = "CITIES";
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
			this.ribbonStatusBar1.Location = new System.Drawing.Point(0, 696);
			this.ribbonStatusBar1.Name = "ribbonStatusBar1";
			this.ribbonStatusBar1.Ribbon = this.rcCitiesList;
			this.ribbonStatusBar1.Size = new System.Drawing.Size(1425, 30);
			// 
			// mainLayout
			// 
			this.mainLayout.Controls.Add(this.gcCities);
			this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainLayout.Location = new System.Drawing.Point(0, 193);
			this.mainLayout.Name = "mainLayout";
			this.mainLayout.Root = this.Root;
			this.mainLayout.Size = new System.Drawing.Size(1425, 503);
			this.mainLayout.TabIndex = 5;
			this.mainLayout.Text = "layoutControl1";
			// 
			// gcCities
			// 
			this.gcCities.DataSource = this.bsCities;
			this.gcCities.EmbeddedNavigator.Buttons.Append.Visible = false;
			this.gcCities.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
			this.gcCities.EmbeddedNavigator.Buttons.Edit.Visible = false;
			this.gcCities.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
			this.gcCities.EmbeddedNavigator.Buttons.Remove.Visible = false;
			this.gcCities.Location = new System.Drawing.Point(14, 14);
			this.gcCities.MainView = this.gvCities;
			this.gcCities.Name = "gcCities";
			this.gcCities.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repCheckEdit});
			this.gcCities.Size = new System.Drawing.Size(1397, 475);
			this.gcCities.TabIndex = 8;
			this.gcCities.UseEmbeddedNavigator = true;
			this.gcCities.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvCities});
			// 
			// bsCities
			// 
			this.bsCities.DataSource = typeof(Spectrum.Models.Common.Countries.CityModel);
			// 
			// gvCities
			// 
			this.gvCities.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.col_id,
            this.colCityName,
            this.colCityCode,
            this.colCountry,
            this.colProvince,
            this.colDistrict,
            this.colRegion,
            this.colContinent,
            this.colPopulation,
            this.colIsCapital,
            this.colNotes,
            this.colIsDefault,
            this.colActive});
			this.gvCities.GridControl = this.gcCities;
			this.gvCities.Name = "gvCities";
			this.gvCities.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
			this.gvCities.OptionsBehavior.AutoExpandAllGroups = true;
			this.gvCities.OptionsBehavior.Editable = false;
			this.gvCities.OptionsBehavior.ReadOnly = true;
			this.gvCities.OptionsFind.AlwaysVisible = true;
			this.gvCities.OptionsView.ColumnAutoWidth = false;
			this.gvCities.OptionsView.ShowGroupedColumns = true;
			this.gvCities.OptionsView.ShowGroupPanel = false;
			// 
			// col_id
			// 
			this.col_id.FieldName = "_id";
			this.col_id.MinWidth = 25;
			this.col_id.Name = "col_id";
			this.col_id.Width = 94;
			// 
			// colCityName
			// 
			this.colCityName.FieldName = "CityName";
			this.colCityName.MinWidth = 25;
			this.colCityName.Name = "colCityName";
			this.colCityName.Visible = true;
			this.colCityName.VisibleIndex = 0;
			this.colCityName.Width = 300;
			// 
			// colCityCode
			// 
			this.colCityCode.FieldName = "CityCode";
			this.colCityCode.MinWidth = 25;
			this.colCityCode.Name = "colCityCode";
			this.colCityCode.Visible = true;
			this.colCityCode.VisibleIndex = 1;
			this.colCityCode.Width = 150;
			// 
			// colCountry
			// 
			this.colCountry.FieldName = "Country";
			this.colCountry.MinWidth = 25;
			this.colCountry.Name = "colCountry";
			this.colCountry.Visible = true;
			this.colCountry.VisibleIndex = 2;
			this.colCountry.Width = 200;
			// 
			// colProvince
			// 
			this.colProvince.FieldName = "Province";
			this.colProvince.MinWidth = 25;
			this.colProvince.Name = "colProvince";
			this.colProvince.Visible = true;
			this.colProvince.VisibleIndex = 3;
			this.colProvince.Width = 200;
			// 
			// colDistrict
			// 
			this.colDistrict.FieldName = "District";
			this.colDistrict.MinWidth = 25;
			this.colDistrict.Name = "colDistrict";
			this.colDistrict.Visible = true;
			this.colDistrict.VisibleIndex = 4;
			this.colDistrict.Width = 200;
			// 
			// colRegion
			// 
			this.colRegion.FieldName = "Region";
			this.colRegion.MinWidth = 25;
			this.colRegion.Name = "colRegion";
			this.colRegion.Visible = true;
			this.colRegion.VisibleIndex = 5;
			this.colRegion.Width = 200;
			// 
			// colContinent
			// 
			this.colContinent.FieldName = "Continent";
			this.colContinent.MinWidth = 25;
			this.colContinent.Name = "colContinent";
			this.colContinent.Visible = true;
			this.colContinent.VisibleIndex = 6;
			this.colContinent.Width = 200;
			// 
			// colPopulation
			// 
			this.colPopulation.FieldName = "Population";
			this.colPopulation.MinWidth = 25;
			this.colPopulation.Name = "colPopulation";
			this.colPopulation.Visible = true;
			this.colPopulation.VisibleIndex = 7;
			this.colPopulation.Width = 100;
			// 
			// colIsCapital
			// 
			this.colIsCapital.ColumnEdit = this.repCheckEdit;
			this.colIsCapital.FieldName = "IsCapital";
			this.colIsCapital.MinWidth = 25;
			this.colIsCapital.Name = "colIsCapital";
			this.colIsCapital.Visible = true;
			this.colIsCapital.VisibleIndex = 8;
			this.colIsCapital.Width = 94;
			// 
			// repCheckEdit
			// 
			this.repCheckEdit.AutoHeight = false;
			this.repCheckEdit.Name = "repCheckEdit";
			// 
			// colNotes
			// 
			this.colNotes.FieldName = "Notes";
			this.colNotes.MinWidth = 25;
			this.colNotes.Name = "colNotes";
			this.colNotes.Visible = true;
			this.colNotes.VisibleIndex = 9;
			this.colNotes.Width = 400;
			// 
			// colIsDefault
			// 
			this.colIsDefault.ColumnEdit = this.repCheckEdit;
			this.colIsDefault.FieldName = "IsDefault";
			this.colIsDefault.MinWidth = 25;
			this.colIsDefault.Name = "colIsDefault";
			this.colIsDefault.Visible = true;
			this.colIsDefault.VisibleIndex = 10;
			this.colIsDefault.Width = 94;
			// 
			// colActive
			// 
			this.colActive.ColumnEdit = this.repCheckEdit;
			this.colActive.FieldName = "Active";
			this.colActive.MinWidth = 25;
			this.colActive.Name = "colActive";
			this.colActive.Visible = true;
			this.colActive.VisibleIndex = 11;
			this.colActive.Width = 94;
			// 
			// Root
			// 
			this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
			this.Root.GroupBordersVisible = false;
			this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
			this.Root.Name = "Root";
			this.Root.Size = new System.Drawing.Size(1425, 503);
			this.Root.TextVisible = false;
			// 
			// layoutControlItem1
			// 
			this.layoutControlItem1.Control = this.gcCities;
			this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
			this.layoutControlItem1.Name = "layoutControlItem1";
			this.layoutControlItem1.Size = new System.Drawing.Size(1401, 479);
			this.layoutControlItem1.TextVisible = false;
			// 
			// CitiesListForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1425, 726);
			this.Controls.Add(this.mainLayout);
			this.Controls.Add(this.rcCitiesList);
			this.Controls.Add(this.ribbonStatusBar1);
			this.Name = "CitiesListForm";
			this.Ribbon = this.rcCitiesList;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.StatusBar = this.ribbonStatusBar1;
			this.Text = "Cities List";
			((System.ComponentModel.ISupportInitialize)(this.rcCitiesList)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.mainLayout)).EndInit();
			this.mainLayout.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.gcCities)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.bsCities)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gvCities)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repCheckEdit)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public DevExpress.XtraBars.Ribbon.RibbonControl rcCitiesList;
		private DevExpress.XtraBars.BarButtonItem btnNew;
		private DevExpress.XtraBars.BarButtonItem btnDelete;
		private DevExpress.XtraBars.BarButtonItem btnEdit;
		private DevExpress.XtraBars.BarButtonItem btnPrint;
		private DevExpress.XtraBars.BarButtonItem btnRefresh;
		private DevExpress.XtraBars.BarButtonItem btnClose;
		private DevExpress.XtraBars.BarButtonItem btnResetGridStyle;
		private DevExpress.XtraBars.Ribbon.RibbonPage rpCitiesList;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup6;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup8;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup11;
		private DevExpress.XtraBars.Ribbon.RibbonPage rpViewSettings;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
		private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar1;
		private DevExpress.XtraLayout.LayoutControl mainLayout;
		private DevExpress.XtraGrid.GridControl gcCities;
		private DevExpress.XtraGrid.Views.Grid.GridView gvCities;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repCheckEdit;
		private DevExpress.XtraLayout.LayoutControlGroup Root;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
		private System.Windows.Forms.BindingSource bsCities;
		private DevExpress.XtraGrid.Columns.GridColumn col_id;
		private DevExpress.XtraGrid.Columns.GridColumn colCityName;
		private DevExpress.XtraGrid.Columns.GridColumn colCityCode;
		private DevExpress.XtraGrid.Columns.GridColumn colCountry;
		private DevExpress.XtraGrid.Columns.GridColumn colProvince;
		private DevExpress.XtraGrid.Columns.GridColumn colDistrict;
		private DevExpress.XtraGrid.Columns.GridColumn colRegion;
		private DevExpress.XtraGrid.Columns.GridColumn colContinent;
		private DevExpress.XtraGrid.Columns.GridColumn colPopulation;
		private DevExpress.XtraGrid.Columns.GridColumn colIsCapital;
		private DevExpress.XtraGrid.Columns.GridColumn colNotes;
		private DevExpress.XtraGrid.Columns.GridColumn colIsDefault;
		private DevExpress.XtraGrid.Columns.GridColumn colActive;
	}
}