namespace SpectrumV1.Views.Common.Companies
{
	partial class CompaniesListForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CompaniesListForm));
			this.rcCompaniesList = new DevExpress.XtraBars.Ribbon.RibbonControl();
			this.btnNew = new DevExpress.XtraBars.BarButtonItem();
			this.btnDelete = new DevExpress.XtraBars.BarButtonItem();
			this.btnEdit = new DevExpress.XtraBars.BarButtonItem();
			this.btnPrint = new DevExpress.XtraBars.BarButtonItem();
			this.barButtonItem13 = new DevExpress.XtraBars.BarButtonItem();
			this.barCheckItem7 = new DevExpress.XtraBars.BarCheckItem();
			this.barButtonItem14 = new DevExpress.XtraBars.BarButtonItem();
			this.barHeaderItem1 = new DevExpress.XtraBars.BarHeaderItem();
			this.btnRefresh = new DevExpress.XtraBars.BarButtonItem();
			this.btnClose = new DevExpress.XtraBars.BarButtonItem();
			this.btnResetGridStyle = new DevExpress.XtraBars.BarButtonItem();
			this.rpCompaniesList = new DevExpress.XtraBars.Ribbon.RibbonPage();
			this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.ribbonPageGroup6 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.ribbonPageGroup8 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.ribbonPageGroup11 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.rpViewSettings = new DevExpress.XtraBars.Ribbon.RibbonPage();
			this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.ribbonStatusBar1 = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
			this.mainLayout = new DevExpress.XtraLayout.LayoutControl();
			this.gcCompanies = new DevExpress.XtraGrid.GridControl();
			this.bsCompanies = new System.Windows.Forms.BindingSource(this.components);
			this.gvCompanies = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.col_id = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colCompanyName = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colShortName = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colDirectoryPath = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colAddress = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colCity = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colCountry = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colPhoneNumber1 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colPhoneNumber2 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colPhoneNumber3 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colFaxNumber = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colEmail = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colWebsite = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colPoBox = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colCompanyDisclaimer = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colLocalCurrency = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colForeignCurrency = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colVatInfo = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colBankAccount = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colCompanyLogo = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colNotes = new DevExpress.XtraGrid.Columns.GridColumn();
			this.repCheckBox = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
			this.repCountries = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
			this.repCities = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
			this.repLocalCurrencies = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
			this.repForeignCurrencies = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
			this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
			this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
			((System.ComponentModel.ISupportInitialize)(this.rcCompaniesList)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.mainLayout)).BeginInit();
			this.mainLayout.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gcCompanies)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.bsCompanies)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gvCompanies)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repCheckBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repCountries)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repCities)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repLocalCurrencies)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repForeignCurrencies)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
			this.SuspendLayout();
			// 
			// rcCompaniesList
			// 
			this.rcCompaniesList.EmptyAreaImageOptions.ImagePadding = new System.Windows.Forms.Padding(35, 39, 35, 39);
			this.rcCompaniesList.ExpandCollapseItem.Id = 0;
			this.rcCompaniesList.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.rcCompaniesList.ExpandCollapseItem,
            this.btnNew,
            this.btnDelete,
            this.btnEdit,
            this.btnPrint,
            this.barButtonItem13,
            this.barCheckItem7,
            this.barButtonItem14,
            this.barHeaderItem1,
            this.btnRefresh,
            this.btnClose,
            this.btnResetGridStyle});
			this.rcCompaniesList.Location = new System.Drawing.Point(0, 0);
			this.rcCompaniesList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.rcCompaniesList.MaxItemId = 41;
			this.rcCompaniesList.MdiMergeStyle = DevExpress.XtraBars.Ribbon.RibbonMdiMergeStyle.Always;
			this.rcCompaniesList.Name = "rcCompaniesList";
			this.rcCompaniesList.OptionsMenuMinWidth = 385;
			this.rcCompaniesList.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.rpCompaniesList,
            this.rpViewSettings});
			this.rcCompaniesList.Size = new System.Drawing.Size(1429, 193);
			this.rcCompaniesList.StatusBar = this.ribbonStatusBar1;
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
			// barButtonItem13
			// 
			this.barButtonItem13.Caption = "Reverse Sort";
			this.barButtonItem13.Id = 28;
			this.barButtonItem13.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.False;
			this.barButtonItem13.ImageOptions.ImageUri.Uri = "outlook%20inspired/reverssort;Size16x16";
			this.barButtonItem13.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
			this.barButtonItem13.Name = "barButtonItem13";
			// 
			// barCheckItem7
			// 
			this.barCheckItem7.Caption = "Add Columns";
			this.barCheckItem7.Id = 30;
			this.barCheckItem7.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.False;
			this.barCheckItem7.ImageOptions.ImageUri.Uri = "outlook%20inspired/addcolumn;Size16x16";
			this.barCheckItem7.Name = "barCheckItem7";
			// 
			// barButtonItem14
			// 
			this.barButtonItem14.Caption = "Expand/Collapse";
			this.barButtonItem14.Id = 31;
			this.barButtonItem14.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.False;
			this.barButtonItem14.ImageOptions.ImageUri.Uri = "outlook%20inspired/expandcollapse;Size16x16";
			this.barButtonItem14.Name = "barButtonItem14";
			// 
			// barHeaderItem1
			// 
			this.barHeaderItem1.Caption = "RECORDS: 0";
			this.barHeaderItem1.Id = 35;
			this.barHeaderItem1.Name = "barHeaderItem1";
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
			this.btnResetGridStyle.Id = 40;
			this.btnResetGridStyle.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnResetGridStyle.ImageOptions.SvgImage")));
			this.btnResetGridStyle.Name = "btnResetGridStyle";
			// 
			// rpCompaniesList
			// 
			this.rpCompaniesList.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1,
            this.ribbonPageGroup6,
            this.ribbonPageGroup8,
            this.ribbonPageGroup11});
			this.rpCompaniesList.Name = "rpCompaniesList";
			this.rpCompaniesList.Text = "Companies";
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
			this.ribbonStatusBar1.Location = new System.Drawing.Point(0, 736);
			this.ribbonStatusBar1.Name = "ribbonStatusBar1";
			this.ribbonStatusBar1.Ribbon = this.rcCompaniesList;
			this.ribbonStatusBar1.Size = new System.Drawing.Size(1429, 30);
			// 
			// mainLayout
			// 
			this.mainLayout.AllowCustomization = false;
			this.mainLayout.Controls.Add(this.gcCompanies);
			this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainLayout.Location = new System.Drawing.Point(0, 193);
			this.mainLayout.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.mainLayout.Name = "mainLayout";
			this.mainLayout.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(717, 447, 450, 350);
			this.mainLayout.Root = this.layoutControlGroup1;
			this.mainLayout.Size = new System.Drawing.Size(1429, 543);
			this.mainLayout.TabIndex = 12;
			this.mainLayout.Text = "layoutControl1";
			// 
			// gcCompanies
			// 
			this.gcCompanies.DataSource = this.bsCompanies;
			this.gcCompanies.EmbeddedNavigator.Buttons.Append.Visible = false;
			this.gcCompanies.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
			this.gcCompanies.EmbeddedNavigator.Buttons.Edit.Visible = false;
			this.gcCompanies.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
			this.gcCompanies.EmbeddedNavigator.Buttons.Remove.Visible = false;
			this.gcCompanies.Location = new System.Drawing.Point(16, 17);
			this.gcCompanies.MainView = this.gvCompanies;
			this.gcCompanies.Name = "gcCompanies";
			this.gcCompanies.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repCheckBox,
            this.repCountries,
            this.repCities,
            this.repLocalCurrencies,
            this.repForeignCurrencies});
			this.gcCompanies.Size = new System.Drawing.Size(1397, 509);
			this.gcCompanies.TabIndex = 9;
			this.gcCompanies.UseEmbeddedNavigator = true;
			this.gcCompanies.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvCompanies});
			// 
			// bsCompanies
			// 
			this.bsCompanies.DataSource = typeof(SpectrumV1.Models.Common.Companies.CompanyModel);
			// 
			// gvCompanies
			// 
			this.gvCompanies.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.col_id,
            this.colCompanyName,
            this.colShortName,
            this.colDirectoryPath,
            this.colAddress,
            this.colCity,
            this.colCountry,
            this.colPhoneNumber1,
            this.colPhoneNumber2,
            this.colPhoneNumber3,
            this.colFaxNumber,
            this.colEmail,
            this.colWebsite,
            this.colPoBox,
            this.colCompanyDisclaimer,
            this.colLocalCurrency,
            this.colForeignCurrency,
            this.colVatInfo,
            this.colBankAccount,
            this.colCompanyLogo,
            this.colNotes});
			this.gvCompanies.GridControl = this.gcCompanies;
			this.gvCompanies.Name = "gvCompanies";
			this.gvCompanies.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
			this.gvCompanies.OptionsBehavior.AutoExpandAllGroups = true;
			this.gvCompanies.OptionsBehavior.Editable = false;
			this.gvCompanies.OptionsBehavior.ReadOnly = true;
			this.gvCompanies.OptionsDetail.EnableMasterViewMode = false;
			this.gvCompanies.OptionsFind.AlwaysVisible = true;
			this.gvCompanies.OptionsView.ColumnAutoWidth = false;
			this.gvCompanies.OptionsView.ShowGroupedColumns = true;
			this.gvCompanies.OptionsView.ShowGroupPanel = false;
			this.gvCompanies.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gvCompanies_RowCellStyle);
			this.gvCompanies.DoubleClick += new System.EventHandler(this.gvCompanies_DoubleClick);
			// 
			// col_id
			// 
			this.col_id.FieldName = "_id";
			this.col_id.MinWidth = 25;
			this.col_id.Name = "col_id";
			this.col_id.Width = 94;
			// 
			// colCompanyName
			// 
			this.colCompanyName.FieldName = "CompanyName";
			this.colCompanyName.MinWidth = 25;
			this.colCompanyName.Name = "colCompanyName";
			this.colCompanyName.Visible = true;
			this.colCompanyName.VisibleIndex = 0;
			this.colCompanyName.Width = 242;
			// 
			// colShortName
			// 
			this.colShortName.FieldName = "ShortName";
			this.colShortName.MinWidth = 25;
			this.colShortName.Name = "colShortName";
			this.colShortName.Visible = true;
			this.colShortName.VisibleIndex = 1;
			this.colShortName.Width = 161;
			// 
			// colDirectoryPath
			// 
			this.colDirectoryPath.FieldName = "DirectoryPath";
			this.colDirectoryPath.MinWidth = 25;
			this.colDirectoryPath.Name = "colDirectoryPath";
			this.colDirectoryPath.Visible = true;
			this.colDirectoryPath.VisibleIndex = 2;
			this.colDirectoryPath.Width = 198;
			// 
			// colAddress
			// 
			this.colAddress.FieldName = "Address";
			this.colAddress.MinWidth = 25;
			this.colAddress.Name = "colAddress";
			this.colAddress.Visible = true;
			this.colAddress.VisibleIndex = 3;
			this.colAddress.Width = 292;
			// 
			// colCity
			// 
			this.colCity.FieldName = "City";
			this.colCity.MinWidth = 25;
			this.colCity.Name = "colCity";
			this.colCity.Visible = true;
			this.colCity.VisibleIndex = 5;
			this.colCity.Width = 131;
			// 
			// colCountry
			// 
			this.colCountry.FieldName = "Country";
			this.colCountry.MinWidth = 25;
			this.colCountry.Name = "colCountry";
			this.colCountry.Visible = true;
			this.colCountry.VisibleIndex = 4;
			this.colCountry.Width = 99;
			// 
			// colPhoneNumber1
			// 
			this.colPhoneNumber1.FieldName = "PhoneNumber1";
			this.colPhoneNumber1.MinWidth = 25;
			this.colPhoneNumber1.Name = "colPhoneNumber1";
			this.colPhoneNumber1.Visible = true;
			this.colPhoneNumber1.VisibleIndex = 6;
			this.colPhoneNumber1.Width = 152;
			// 
			// colPhoneNumber2
			// 
			this.colPhoneNumber2.FieldName = "PhoneNumber2";
			this.colPhoneNumber2.MinWidth = 25;
			this.colPhoneNumber2.Name = "colPhoneNumber2";
			this.colPhoneNumber2.Visible = true;
			this.colPhoneNumber2.VisibleIndex = 7;
			this.colPhoneNumber2.Width = 141;
			// 
			// colPhoneNumber3
			// 
			this.colPhoneNumber3.FieldName = "PhoneNumber3";
			this.colPhoneNumber3.MinWidth = 25;
			this.colPhoneNumber3.Name = "colPhoneNumber3";
			this.colPhoneNumber3.Visible = true;
			this.colPhoneNumber3.VisibleIndex = 8;
			this.colPhoneNumber3.Width = 168;
			// 
			// colFaxNumber
			// 
			this.colFaxNumber.FieldName = "FaxNumber";
			this.colFaxNumber.MinWidth = 25;
			this.colFaxNumber.Name = "colFaxNumber";
			this.colFaxNumber.Visible = true;
			this.colFaxNumber.VisibleIndex = 9;
			this.colFaxNumber.Width = 108;
			// 
			// colEmail
			// 
			this.colEmail.FieldName = "Email";
			this.colEmail.MinWidth = 25;
			this.colEmail.Name = "colEmail";
			this.colEmail.Visible = true;
			this.colEmail.VisibleIndex = 10;
			this.colEmail.Width = 185;
			// 
			// colWebsite
			// 
			this.colWebsite.FieldName = "Website";
			this.colWebsite.MinWidth = 25;
			this.colWebsite.Name = "colWebsite";
			this.colWebsite.Visible = true;
			this.colWebsite.VisibleIndex = 11;
			this.colWebsite.Width = 267;
			// 
			// colPoBox
			// 
			this.colPoBox.FieldName = "PoBox";
			this.colPoBox.MinWidth = 25;
			this.colPoBox.Name = "colPoBox";
			this.colPoBox.Visible = true;
			this.colPoBox.VisibleIndex = 12;
			this.colPoBox.Width = 176;
			// 
			// colCompanyDisclaimer
			// 
			this.colCompanyDisclaimer.FieldName = "CompanyDisclaimer";
			this.colCompanyDisclaimer.MinWidth = 25;
			this.colCompanyDisclaimer.Name = "colCompanyDisclaimer";
			this.colCompanyDisclaimer.Visible = true;
			this.colCompanyDisclaimer.VisibleIndex = 13;
			this.colCompanyDisclaimer.Width = 190;
			// 
			// colLocalCurrency
			// 
			this.colLocalCurrency.FieldName = "LocalCurrency";
			this.colLocalCurrency.MinWidth = 25;
			this.colLocalCurrency.Name = "colLocalCurrency";
			this.colLocalCurrency.Visible = true;
			this.colLocalCurrency.VisibleIndex = 14;
			this.colLocalCurrency.Width = 121;
			// 
			// colForeignCurrency
			// 
			this.colForeignCurrency.FieldName = "ForeignCurrency";
			this.colForeignCurrency.MinWidth = 25;
			this.colForeignCurrency.Name = "colForeignCurrency";
			this.colForeignCurrency.Visible = true;
			this.colForeignCurrency.VisibleIndex = 15;
			this.colForeignCurrency.Width = 120;
			// 
			// colVatInfo
			// 
			this.colVatInfo.FieldName = "VatInfo";
			this.colVatInfo.MinWidth = 25;
			this.colVatInfo.Name = "colVatInfo";
			this.colVatInfo.Visible = true;
			this.colVatInfo.VisibleIndex = 16;
			this.colVatInfo.Width = 107;
			// 
			// colBankAccount
			// 
			this.colBankAccount.FieldName = "BankAccount";
			this.colBankAccount.MinWidth = 25;
			this.colBankAccount.Name = "colBankAccount";
			this.colBankAccount.Visible = true;
			this.colBankAccount.VisibleIndex = 17;
			this.colBankAccount.Width = 181;
			// 
			// colCompanyLogo
			// 
			this.colCompanyLogo.FieldName = "CompanyLogo";
			this.colCompanyLogo.MinWidth = 25;
			this.colCompanyLogo.Name = "colCompanyLogo";
			this.colCompanyLogo.Visible = true;
			this.colCompanyLogo.VisibleIndex = 18;
			this.colCompanyLogo.Width = 165;
			// 
			// colNotes
			// 
			this.colNotes.FieldName = "Notes";
			this.colNotes.MinWidth = 25;
			this.colNotes.Name = "colNotes";
			this.colNotes.Visible = true;
			this.colNotes.VisibleIndex = 19;
			this.colNotes.Width = 364;
			// 
			// repCheckBox
			// 
			this.repCheckBox.AutoHeight = false;
			this.repCheckBox.Name = "repCheckBox";
			// 
			// repCountries
			// 
			this.repCountries.AutoHeight = false;
			this.repCountries.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.repCountries.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Id", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CountryName", "Name", 60, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default)});
			this.repCountries.DisplayMember = "CountryName";
			this.repCountries.Name = "repCountries";
			this.repCountries.NullText = "";
			this.repCountries.ValueMember = "Id";
			// 
			// repCities
			// 
			this.repCities.AutoHeight = false;
			this.repCities.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.repCities.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Id", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CityName", "Name", 60, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default)});
			this.repCities.DisplayMember = "CityName";
			this.repCities.Name = "repCities";
			this.repCities.NullText = "";
			this.repCities.ValueMember = "Id";
			// 
			// repLocalCurrencies
			// 
			this.repLocalCurrencies.AutoHeight = false;
			this.repLocalCurrencies.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.repLocalCurrencies.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Id", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CurrencyCode", "LL", 60, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default)});
			this.repLocalCurrencies.DisplayMember = "CurrencyCode";
			this.repLocalCurrencies.Name = "repLocalCurrencies";
			this.repLocalCurrencies.NullText = "";
			this.repLocalCurrencies.ValueMember = "Id";
			// 
			// repForeignCurrencies
			// 
			this.repForeignCurrencies.AutoHeight = false;
			this.repForeignCurrencies.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.repForeignCurrencies.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Id", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CurrencyCode", "USD", 60, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default)});
			this.repForeignCurrencies.DisplayMember = "CurrencyCode";
			this.repForeignCurrencies.Name = "repForeignCurrencies";
			this.repForeignCurrencies.NullText = "";
			this.repForeignCurrencies.ValueMember = "Id";
			// 
			// layoutControlGroup1
			// 
			this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
			this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
			this.layoutControlGroup1.GroupBordersVisible = false;
			this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
			this.layoutControlGroup1.Name = "Root";
			this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(14, 14, 15, 15);
			this.layoutControlGroup1.Size = new System.Drawing.Size(1429, 543);
			this.layoutControlGroup1.TextVisible = false;
			// 
			// layoutControlItem1
			// 
			this.layoutControlItem1.Control = this.gcCompanies;
			this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
			this.layoutControlItem1.Name = "layoutControlItem1";
			this.layoutControlItem1.Size = new System.Drawing.Size(1401, 513);
			this.layoutControlItem1.TextVisible = false;
			// 
			// CompaniesListForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1429, 766);
			this.Controls.Add(this.mainLayout);
			this.Controls.Add(this.ribbonStatusBar1);
			this.Controls.Add(this.rcCompaniesList);
			this.Name = "CompaniesListForm";
			this.Ribbon = this.rcCompaniesList;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.StatusBar = this.ribbonStatusBar1;
			this.Text = "Companies List";
			((System.ComponentModel.ISupportInitialize)(this.rcCompaniesList)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.mainLayout)).EndInit();
			this.mainLayout.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.gcCompanies)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.bsCompanies)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gvCompanies)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repCheckBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repCountries)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repCities)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repLocalCurrencies)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repForeignCurrencies)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public DevExpress.XtraBars.Ribbon.RibbonControl rcCompaniesList;
		private DevExpress.XtraBars.BarButtonItem btnNew;
		private DevExpress.XtraBars.BarButtonItem btnDelete;
		private DevExpress.XtraBars.BarButtonItem btnEdit;
		private DevExpress.XtraBars.BarButtonItem btnPrint;
		private DevExpress.XtraBars.BarButtonItem barButtonItem13;
		private DevExpress.XtraBars.BarCheckItem barCheckItem7;
		private DevExpress.XtraBars.BarButtonItem barButtonItem14;
		private DevExpress.XtraBars.BarHeaderItem barHeaderItem1;
		private DevExpress.XtraBars.BarButtonItem btnRefresh;
		private DevExpress.XtraBars.BarButtonItem btnClose;
		private DevExpress.XtraBars.BarButtonItem btnResetGridStyle;
		private DevExpress.XtraBars.Ribbon.RibbonPage rpCompaniesList;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup6;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup8;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup11;
		private DevExpress.XtraBars.Ribbon.RibbonPage rpViewSettings;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
		private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar1;
		private DevExpress.XtraLayout.LayoutControl mainLayout;
		private DevExpress.XtraGrid.GridControl gcCompanies;
		private DevExpress.XtraGrid.Views.Grid.GridView gvCompanies;
		private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repCountries;
		private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repCities;
		private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repLocalCurrencies;
		private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repForeignCurrencies;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repCheckBox;
		private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
		private System.Windows.Forms.BindingSource bsCompanies;
		private DevExpress.XtraGrid.Columns.GridColumn col_id;
		private DevExpress.XtraGrid.Columns.GridColumn colCompanyName;
		private DevExpress.XtraGrid.Columns.GridColumn colShortName;
		private DevExpress.XtraGrid.Columns.GridColumn colDirectoryPath;
		private DevExpress.XtraGrid.Columns.GridColumn colAddress;
		private DevExpress.XtraGrid.Columns.GridColumn colCity;
		private DevExpress.XtraGrid.Columns.GridColumn colCountry;
		private DevExpress.XtraGrid.Columns.GridColumn colPhoneNumber1;
		private DevExpress.XtraGrid.Columns.GridColumn colPhoneNumber2;
		private DevExpress.XtraGrid.Columns.GridColumn colPhoneNumber3;
		private DevExpress.XtraGrid.Columns.GridColumn colFaxNumber;
		private DevExpress.XtraGrid.Columns.GridColumn colEmail;
		private DevExpress.XtraGrid.Columns.GridColumn colWebsite;
		private DevExpress.XtraGrid.Columns.GridColumn colPoBox;
		private DevExpress.XtraGrid.Columns.GridColumn colCompanyDisclaimer;
		private DevExpress.XtraGrid.Columns.GridColumn colLocalCurrency;
		private DevExpress.XtraGrid.Columns.GridColumn colForeignCurrency;
		private DevExpress.XtraGrid.Columns.GridColumn colVatInfo;
		private DevExpress.XtraGrid.Columns.GridColumn colBankAccount;
		private DevExpress.XtraGrid.Columns.GridColumn colCompanyLogo;
		private DevExpress.XtraGrid.Columns.GridColumn colNotes;
	}
}