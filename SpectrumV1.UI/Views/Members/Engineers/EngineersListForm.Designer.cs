namespace SpectrumV1.Views.Members.Engineers
{
	partial class EngineersListForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EngineersListForm));
			this.rcEngineers = new DevExpress.XtraBars.Ribbon.RibbonControl();
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
			this.rpEngineers = new DevExpress.XtraBars.Ribbon.RibbonPage();
			this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.ribbonPageGroup6 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.ribbonPageGroup8 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.ribbonPageGroup11 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.rpViewSettings = new DevExpress.XtraBars.Ribbon.RibbonPage();
			this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.ribbonStatusBar1 = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
			this.mainLayout = new DevExpress.XtraLayout.LayoutControl();
			this.gcEngineers = new DevExpress.XtraGrid.GridControl();
			this.gvEngineers = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.col_id = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colClientName = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colContactPerson = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colAddress = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colEmail = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colPhoneNumber1 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colPhoneNumber2 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colPhoneNumber3 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colFaxNumber = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colWebsite = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colPoBox = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colMofNo = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colActivity = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colCountry = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colCity = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colNotes = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colIsDefault = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colActive = new DevExpress.XtraGrid.Columns.GridColumn();
			this.repDateFormat = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
			this.repCountries = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
			this.repCities = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
			this.repCheckBox = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
			this.repUsers = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
			this.repMemberOf = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
			this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
			this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
			((System.ComponentModel.ISupportInitialize)(this.rcEngineers)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.mainLayout)).BeginInit();
			this.mainLayout.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gcEngineers)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gvEngineers)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repDateFormat)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repDateFormat.CalendarTimeProperties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repCountries)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repCities)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repCheckBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repUsers)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repMemberOf)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
			this.SuspendLayout();
			// 
			// rcEngineers
			// 
			this.rcEngineers.EmptyAreaImageOptions.ImagePadding = new System.Windows.Forms.Padding(35, 39, 35, 39);
			this.rcEngineers.ExpandCollapseItem.Id = 0;
			this.rcEngineers.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.rcEngineers.ExpandCollapseItem,
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
			this.rcEngineers.Location = new System.Drawing.Point(0, 0);
			this.rcEngineers.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.rcEngineers.MaxItemId = 44;
			this.rcEngineers.MdiMergeStyle = DevExpress.XtraBars.Ribbon.RibbonMdiMergeStyle.Always;
			this.rcEngineers.Name = "rcEngineers";
			this.rcEngineers.OptionsMenuMinWidth = 385;
			this.rcEngineers.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.rpEngineers,
            this.rpViewSettings});
			this.rcEngineers.Size = new System.Drawing.Size(1393, 193);
			this.rcEngineers.StatusBar = this.ribbonStatusBar1;
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
			this.btnResetGridStyle.Id = 43;
			this.btnResetGridStyle.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnResetGridStyle.ImageOptions.SvgImage")));
			this.btnResetGridStyle.Name = "btnResetGridStyle";
			// 
			// rpEngineers
			// 
			this.rpEngineers.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1,
            this.ribbonPageGroup6,
            this.ribbonPageGroup8,
            this.ribbonPageGroup11});
			this.rpEngineers.Name = "rpEngineers";
			this.rpEngineers.Text = "ENGINEERS LIST";
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
			this.ribbonStatusBar1.Location = new System.Drawing.Point(0, 726);
			this.ribbonStatusBar1.Name = "ribbonStatusBar1";
			this.ribbonStatusBar1.Ribbon = this.rcEngineers;
			this.ribbonStatusBar1.Size = new System.Drawing.Size(1393, 30);
			// 
			// mainLayout
			// 
			this.mainLayout.Controls.Add(this.gcEngineers);
			this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainLayout.Location = new System.Drawing.Point(0, 193);
			this.mainLayout.Name = "mainLayout";
			this.mainLayout.Root = this.Root;
			this.mainLayout.Size = new System.Drawing.Size(1393, 533);
			this.mainLayout.TabIndex = 18;
			this.mainLayout.Text = "layoutControl1";
			// 
			// gcEngineers
			// 
			this.gcEngineers.EmbeddedNavigator.Buttons.Append.Visible = false;
			this.gcEngineers.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
			this.gcEngineers.EmbeddedNavigator.Buttons.Edit.Visible = false;
			this.gcEngineers.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
			this.gcEngineers.EmbeddedNavigator.Buttons.Remove.Visible = false;
			this.gcEngineers.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
			this.gcEngineers.Location = new System.Drawing.Point(14, 14);
			this.gcEngineers.MainView = this.gvEngineers;
			this.gcEngineers.Margin = new System.Windows.Forms.Padding(14, 16, 14, 16);
			this.gcEngineers.Name = "gcEngineers";
			this.gcEngineers.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repDateFormat,
            this.repCountries,
            this.repCities,
            this.repCheckBox,
            this.repUsers,
            this.repMemberOf});
			this.gcEngineers.ShowOnlyPredefinedDetails = true;
			this.gcEngineers.Size = new System.Drawing.Size(1365, 505);
			this.gcEngineers.TabIndex = 4;
			this.gcEngineers.UseEmbeddedNavigator = true;
			this.gcEngineers.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvEngineers});
			// 
			// gvEngineers
			// 
			this.gvEngineers.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.col_id,
            this.colClientName,
            this.colContactPerson,
            this.colAddress,
            this.colEmail,
            this.colPhoneNumber1,
            this.colPhoneNumber2,
            this.colPhoneNumber3,
            this.colFaxNumber,
            this.colWebsite,
            this.colPoBox,
            this.colMofNo,
            this.colActivity,
            this.colCountry,
            this.colCity,
            this.colNotes,
            this.colIsDefault,
            this.colActive});
			this.gvEngineers.DetailHeight = 458;
			this.gvEngineers.DetailVerticalIndent = 13;
			this.gvEngineers.GridControl = this.gcEngineers;
			this.gvEngineers.Name = "gvEngineers";
			this.gvEngineers.OptionsBehavior.AutoExpandAllGroups = true;
			this.gvEngineers.OptionsBehavior.Editable = false;
			this.gvEngineers.OptionsBehavior.ReadOnly = true;
			this.gvEngineers.OptionsFind.AlwaysVisible = true;
			this.gvEngineers.OptionsSelection.MultiSelect = true;
			this.gvEngineers.OptionsView.ColumnAutoWidth = false;
			this.gvEngineers.OptionsView.ShowGroupPanel = false;
			this.gvEngineers.OptionsView.ShowIndicator = false;
			this.gvEngineers.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
			// 
			// col_id
			// 
			this.col_id.FieldName = "_id";
			this.col_id.MinWidth = 25;
			this.col_id.Name = "col_id";
			this.col_id.Width = 94;
			// 
			// colClientName
			// 
			this.colClientName.FieldName = "ClientName";
			this.colClientName.MinWidth = 25;
			this.colClientName.Name = "colClientName";
			this.colClientName.Visible = true;
			this.colClientName.VisibleIndex = 0;
			this.colClientName.Width = 300;
			// 
			// colContactPerson
			// 
			this.colContactPerson.FieldName = "ContactPerson";
			this.colContactPerson.MinWidth = 25;
			this.colContactPerson.Name = "colContactPerson";
			this.colContactPerson.Visible = true;
			this.colContactPerson.VisibleIndex = 1;
			this.colContactPerson.Width = 300;
			// 
			// colAddress
			// 
			this.colAddress.FieldName = "Address";
			this.colAddress.MinWidth = 25;
			this.colAddress.Name = "colAddress";
			this.colAddress.Visible = true;
			this.colAddress.VisibleIndex = 2;
			this.colAddress.Width = 400;
			// 
			// colEmail
			// 
			this.colEmail.FieldName = "Email";
			this.colEmail.MinWidth = 25;
			this.colEmail.Name = "colEmail";
			this.colEmail.Visible = true;
			this.colEmail.VisibleIndex = 3;
			this.colEmail.Width = 300;
			// 
			// colPhoneNumber1
			// 
			this.colPhoneNumber1.FieldName = "PhoneNumber1";
			this.colPhoneNumber1.MinWidth = 25;
			this.colPhoneNumber1.Name = "colPhoneNumber1";
			this.colPhoneNumber1.Visible = true;
			this.colPhoneNumber1.VisibleIndex = 4;
			this.colPhoneNumber1.Width = 200;
			// 
			// colPhoneNumber2
			// 
			this.colPhoneNumber2.FieldName = "PhoneNumber2";
			this.colPhoneNumber2.MinWidth = 25;
			this.colPhoneNumber2.Name = "colPhoneNumber2";
			this.colPhoneNumber2.Visible = true;
			this.colPhoneNumber2.VisibleIndex = 5;
			this.colPhoneNumber2.Width = 200;
			// 
			// colPhoneNumber3
			// 
			this.colPhoneNumber3.FieldName = "PhoneNumber3";
			this.colPhoneNumber3.MinWidth = 25;
			this.colPhoneNumber3.Name = "colPhoneNumber3";
			this.colPhoneNumber3.Visible = true;
			this.colPhoneNumber3.VisibleIndex = 6;
			this.colPhoneNumber3.Width = 200;
			// 
			// colFaxNumber
			// 
			this.colFaxNumber.FieldName = "FaxNumber";
			this.colFaxNumber.MinWidth = 25;
			this.colFaxNumber.Name = "colFaxNumber";
			this.colFaxNumber.Visible = true;
			this.colFaxNumber.VisibleIndex = 7;
			this.colFaxNumber.Width = 200;
			// 
			// colWebsite
			// 
			this.colWebsite.FieldName = "Website";
			this.colWebsite.MinWidth = 25;
			this.colWebsite.Name = "colWebsite";
			this.colWebsite.Visible = true;
			this.colWebsite.VisibleIndex = 8;
			this.colWebsite.Width = 200;
			// 
			// colPoBox
			// 
			this.colPoBox.FieldName = "PoBox";
			this.colPoBox.MinWidth = 25;
			this.colPoBox.Name = "colPoBox";
			this.colPoBox.Visible = true;
			this.colPoBox.VisibleIndex = 9;
			this.colPoBox.Width = 200;
			// 
			// colMofNo
			// 
			this.colMofNo.FieldName = "MofNo";
			this.colMofNo.MinWidth = 25;
			this.colMofNo.Name = "colMofNo";
			this.colMofNo.Visible = true;
			this.colMofNo.VisibleIndex = 10;
			this.colMofNo.Width = 200;
			// 
			// colActivity
			// 
			this.colActivity.FieldName = "Activity";
			this.colActivity.MinWidth = 25;
			this.colActivity.Name = "colActivity";
			this.colActivity.Visible = true;
			this.colActivity.VisibleIndex = 11;
			this.colActivity.Width = 300;
			// 
			// colCountry
			// 
			this.colCountry.Caption = "Country";
			this.colCountry.FieldName = "Country";
			this.colCountry.MinWidth = 25;
			this.colCountry.Name = "colCountry";
			this.colCountry.Visible = true;
			this.colCountry.VisibleIndex = 12;
			this.colCountry.Width = 200;
			// 
			// colCity
			// 
			this.colCity.Caption = "City";
			this.colCity.FieldName = "City";
			this.colCity.MinWidth = 25;
			this.colCity.Name = "colCity";
			this.colCity.Visible = true;
			this.colCity.VisibleIndex = 13;
			this.colCity.Width = 200;
			// 
			// colNotes
			// 
			this.colNotes.FieldName = "Notes";
			this.colNotes.MinWidth = 25;
			this.colNotes.Name = "colNotes";
			this.colNotes.Visible = true;
			this.colNotes.VisibleIndex = 14;
			this.colNotes.Width = 300;
			// 
			// colIsDefault
			// 
			this.colIsDefault.FieldName = "IsDefault";
			this.colIsDefault.MinWidth = 25;
			this.colIsDefault.Name = "colIsDefault";
			this.colIsDefault.Visible = true;
			this.colIsDefault.VisibleIndex = 15;
			this.colIsDefault.Width = 94;
			// 
			// colActive
			// 
			this.colActive.FieldName = "Active";
			this.colActive.MinWidth = 25;
			this.colActive.Name = "colActive";
			this.colActive.Visible = true;
			this.colActive.VisibleIndex = 16;
			this.colActive.Width = 94;
			// 
			// repDateFormat
			// 
			this.repDateFormat.AutoHeight = false;
			this.repDateFormat.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.repDateFormat.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.repDateFormat.CalendarTimeProperties.MaskSettings.Set("mask", "dd/MM/yyyy");
			this.repDateFormat.CalendarTimeProperties.UseMaskAsDisplayFormat = true;
			this.repDateFormat.MaskSettings.Set("mask", "dd/MM/yyyy");
			this.repDateFormat.Name = "repDateFormat";
			this.repDateFormat.UseMaskAsDisplayFormat = true;
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
			// repCheckBox
			// 
			this.repCheckBox.AutoHeight = false;
			this.repCheckBox.Name = "repCheckBox";
			// 
			// repUsers
			// 
			this.repUsers.AutoHeight = false;
			this.repUsers.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.repUsers.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Id", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("UserName", "Name", 60, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default)});
			this.repUsers.DisplayMember = "UserName";
			this.repUsers.Name = "repUsers";
			this.repUsers.NullText = "";
			this.repUsers.ValueMember = "Id";
			// 
			// repMemberOf
			// 
			this.repMemberOf.AutoHeight = false;
			this.repMemberOf.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.repMemberOf.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Id", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CustomerName", "Name", 60, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default)});
			this.repMemberOf.DisplayMember = "CustomerName";
			this.repMemberOf.Name = "repMemberOf";
			this.repMemberOf.NullText = "";
			this.repMemberOf.ValueMember = "Id";
			// 
			// Root
			// 
			this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
			this.Root.GroupBordersVisible = false;
			this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
			this.Root.Name = "Root";
			this.Root.Size = new System.Drawing.Size(1393, 533);
			this.Root.TextVisible = false;
			// 
			// layoutControlItem1
			// 
			this.layoutControlItem1.Control = this.gcEngineers;
			this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
			this.layoutControlItem1.Name = "layoutControlItem1";
			this.layoutControlItem1.Size = new System.Drawing.Size(1369, 509);
			this.layoutControlItem1.TextVisible = false;
			// 
			// EngineersListForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1393, 756);
			this.Controls.Add(this.mainLayout);
			this.Controls.Add(this.ribbonStatusBar1);
			this.Controls.Add(this.rcEngineers);
			this.Name = "EngineersListForm";
			this.Ribbon = this.rcEngineers;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.StatusBar = this.ribbonStatusBar1;
			this.Text = "Engineers List";
			((System.ComponentModel.ISupportInitialize)(this.rcEngineers)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.mainLayout)).EndInit();
			this.mainLayout.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.gcEngineers)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gvEngineers)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repDateFormat.CalendarTimeProperties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repDateFormat)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repCountries)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repCities)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repCheckBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repUsers)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repMemberOf)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public DevExpress.XtraBars.Ribbon.RibbonControl rcEngineers;
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
		private DevExpress.XtraBars.Ribbon.RibbonPage rpEngineers;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup6;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup8;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup11;
		private DevExpress.XtraBars.Ribbon.RibbonPage rpViewSettings;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
		private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar1;
		private DevExpress.XtraLayout.LayoutControl mainLayout;
		private DevExpress.XtraGrid.GridControl gcEngineers;
		private DevExpress.XtraGrid.Views.Grid.GridView gvEngineers;
		private DevExpress.XtraGrid.Columns.GridColumn col_id;
		private DevExpress.XtraGrid.Columns.GridColumn colClientName;
		private DevExpress.XtraGrid.Columns.GridColumn colContactPerson;
		private DevExpress.XtraGrid.Columns.GridColumn colAddress;
		private DevExpress.XtraGrid.Columns.GridColumn colEmail;
		private DevExpress.XtraGrid.Columns.GridColumn colPhoneNumber1;
		private DevExpress.XtraGrid.Columns.GridColumn colPhoneNumber2;
		private DevExpress.XtraGrid.Columns.GridColumn colPhoneNumber3;
		private DevExpress.XtraGrid.Columns.GridColumn colFaxNumber;
		private DevExpress.XtraGrid.Columns.GridColumn colWebsite;
		private DevExpress.XtraGrid.Columns.GridColumn colPoBox;
		private DevExpress.XtraGrid.Columns.GridColumn colMofNo;
		private DevExpress.XtraGrid.Columns.GridColumn colActivity;
		private DevExpress.XtraGrid.Columns.GridColumn colCountry;
		private DevExpress.XtraGrid.Columns.GridColumn colCity;
		private DevExpress.XtraGrid.Columns.GridColumn colNotes;
		private DevExpress.XtraGrid.Columns.GridColumn colIsDefault;
		private DevExpress.XtraGrid.Columns.GridColumn colActive;
		private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repDateFormat;
		private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repCountries;
		private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repCities;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repCheckBox;
		private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repUsers;
		private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repMemberOf;
		private DevExpress.XtraLayout.LayoutControlGroup Root;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
	}
}