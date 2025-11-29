namespace SpectrumV1.Views.Projects
{
	partial class ProjectEditForm
	{
		private System.ComponentModel.IContainer components = null;
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null)) components.Dispose();
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectEditForm));
			this.bsProject = new System.Windows.Forms.BindingSource(this.components);
			this.ribbonControl = new DevExpress.XtraBars.Ribbon.RibbonControl();
			this.btnSave = new DevExpress.XtraBars.BarButtonItem();
			this.btnClose = new DevExpress.XtraBars.BarButtonItem();
			this.btnSaveAndClose = new DevExpress.XtraBars.BarButtonItem();
			this.btnDelete = new DevExpress.XtraBars.BarButtonItem();
			this.btnPrint = new DevExpress.XtraBars.BarButtonItem();
			this.btnRefresh = new DevExpress.XtraBars.BarButtonItem();
			this.btnNew = new DevExpress.XtraBars.BarButtonItem();
			this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
			this.ribbonPageGroup8 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.ribbonPageGroup7 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.ribbonPageGroup4 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.ribbonStatusBar1 = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
			this.mainLayout = new DevExpress.XtraLayout.LayoutControl();
			this.cboStatus = new DevExpress.XtraEditors.ComboBoxEdit();
			this.txtProjectName = new DevExpress.XtraEditors.TextEdit();
			this.txtReference = new DevExpress.XtraEditors.TextEdit();
			this.cboClients = new DevExpress.XtraEditors.LookUpEdit();
			this.cboEngineers = new DevExpress.XtraEditors.LookUpEdit();
			this.dtIssuanceDate = new DevExpress.XtraEditors.DateEdit();
			this.dtExpiryDate = new DevExpress.XtraEditors.DateEdit();
			this.memoDescription = new DevExpress.XtraEditors.MemoEdit();
			this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
			this.lciDescription = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
			this.lciProjectName = new DevExpress.XtraLayout.LayoutControlItem();
			this.lciReference = new DevExpress.XtraLayout.LayoutControlItem();
			this.lciStatus = new DevExpress.XtraLayout.LayoutControlItem();
			this.lciIssuanceDate = new DevExpress.XtraLayout.LayoutControlItem();
			this.lciExpiryDate = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
			this.lciClient = new DevExpress.XtraLayout.LayoutControlItem();
			this.lciEngineer = new DevExpress.XtraLayout.LayoutControlItem();
			((System.ComponentModel.ISupportInitialize)(this.bsProject)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.mainLayout)).BeginInit();
			this.mainLayout.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.cboStatus.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtProjectName.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtReference.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cboClients.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cboEngineers.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dtIssuanceDate.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dtIssuanceDate.Properties.CalendarTimeProperties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dtExpiryDate.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dtExpiryDate.Properties.CalendarTimeProperties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.memoDescription.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.lciDescription)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.lciProjectName)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.lciReference)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.lciStatus)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.lciIssuanceDate)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.lciExpiryDate)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.lciClient)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.lciEngineer)).BeginInit();
			this.SuspendLayout();
			// 
			// bsProject
			// 
			this.bsProject.DataSource = typeof(SpectrumV1.Models.Projects.ProjectModel);
			// 
			// ribbonControl
			// 
			this.ribbonControl.EmptyAreaImageOptions.ImagePadding = new System.Windows.Forms.Padding(35, 39, 35, 39);
			this.ribbonControl.ExpandCollapseItem.Id = 0;
			this.ribbonControl.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl.ExpandCollapseItem,
            this.btnSave,
            this.btnClose,
            this.btnSaveAndClose,
            this.btnDelete,
            this.btnPrint,
            this.btnRefresh,
            this.btnNew});
			this.ribbonControl.Location = new System.Drawing.Point(0, 0);
			this.ribbonControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.ribbonControl.MaxItemId = 21;
			this.ribbonControl.Name = "ribbonControl";
			this.ribbonControl.OptionsMenuMinWidth = 385;
			this.ribbonControl.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
			this.ribbonControl.Size = new System.Drawing.Size(1362, 193);
			this.ribbonControl.StatusBar = this.ribbonStatusBar1;
			// 
			// btnSave
			// 
			this.btnSave.Caption = "Save";
			this.btnSave.Enabled = false;
			this.btnSave.Id = 1;
			this.btnSave.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.False;
			this.btnSave.ImageOptions.ImageUri.Uri = "resource://DevExpress.DevAV.Resources.Save.svg";
			this.btnSave.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnSave.ImageOptions.SvgImage")));
			this.btnSave.Name = "btnSave";
			this.btnSave.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnSave_ItemClick);
			// 
			// btnClose
			// 
			this.btnClose.Caption = "Close";
			this.btnClose.Id = 2;
			this.btnClose.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.False;
			this.btnClose.ImageOptions.ImageUri.Uri = "resource://DevExpress.DevAV.Resources.Close.svg";
			this.btnClose.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnClose.ImageOptions.SvgImage")));
			this.btnClose.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.Escape);
			this.btnClose.Name = "btnClose";
			this.btnClose.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnClose_ItemClick);
			// 
			// btnSaveAndClose
			// 
			this.btnSaveAndClose.Caption = "Save && Close";
			this.btnSaveAndClose.Enabled = false;
			this.btnSaveAndClose.Id = 3;
			this.btnSaveAndClose.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.False;
			this.btnSaveAndClose.ImageOptions.ImageUri.Uri = "resource://DevExpress.DevAV.Resources.SaveAndClose.svg";
			this.btnSaveAndClose.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnSaveAndClose.ImageOptions.SvgImage")));
			this.btnSaveAndClose.Name = "btnSaveAndClose";
			this.btnSaveAndClose.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnSaveAndClose_ItemClick);
			// 
			// btnDelete
			// 
			this.btnDelete.Caption = "Delete";
			this.btnDelete.Enabled = false;
			this.btnDelete.Id = 4;
			this.btnDelete.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.False;
			this.btnDelete.ImageOptions.ImageUri.Uri = "resource://DevExpress.DevAV.Resources.Delete.svg";
			this.btnDelete.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnDelete.ImageOptions.SvgImage")));
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnDelete_ItemClick);
			// 
			// btnPrint
			// 
			this.btnPrint.Caption = "Print";
			this.btnPrint.Enabled = false;
			this.btnPrint.Id = 11;
			this.btnPrint.ImageOptions.ImageUri.Uri = "resource://DevExpress.DevAV.Resources.Task.svg";
			this.btnPrint.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnPrint.ImageOptions.SvgImage")));
			this.btnPrint.Name = "btnPrint";
			this.btnPrint.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnPrint_ItemClick);
			// 
			// btnRefresh
			// 
			this.btnRefresh.Caption = "Reset Changes";
			this.btnRefresh.Id = 17;
			this.btnRefresh.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnRefresh.ImageOptions.SvgImage")));
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnRefresh_ItemClick);
			// 
			// btnNew
			// 
			this.btnNew.Caption = "Add New";
			this.btnNew.Enabled = false;
			this.btnNew.Id = 18;
			this.btnNew.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnNew.ImageOptions.SvgImage")));
			this.btnNew.Name = "btnNew";
			this.btnNew.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnNew_ItemClick);
			// 
			// ribbonPage1
			// 
			this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup8,
            this.ribbonPageGroup1,
            this.ribbonPageGroup7,
            this.ribbonPageGroup2,
            this.ribbonPageGroup4,
            this.ribbonPageGroup3});
			this.ribbonPage1.Name = "ribbonPage1";
			this.ribbonPage1.Text = "PROJECT EDIT";
			// 
			// ribbonPageGroup8
			// 
			this.ribbonPageGroup8.ItemLinks.Add(this.btnNew);
			this.ribbonPageGroup8.Name = "ribbonPageGroup8";
			this.ribbonPageGroup8.Text = "New";
			// 
			// ribbonPageGroup1
			// 
			this.ribbonPageGroup1.CaptionButtonVisible = DevExpress.Utils.DefaultBoolean.False;
			this.ribbonPageGroup1.ItemLinks.Add(this.btnSave);
			this.ribbonPageGroup1.ItemLinks.Add(this.btnSaveAndClose);
			this.ribbonPageGroup1.Name = "ribbonPageGroup1";
			this.ribbonPageGroup1.Text = "Save";
			// 
			// ribbonPageGroup7
			// 
			this.ribbonPageGroup7.AllowTextClipping = false;
			this.ribbonPageGroup7.CaptionButtonVisible = DevExpress.Utils.DefaultBoolean.False;
			this.ribbonPageGroup7.ItemLinks.Add(this.btnRefresh);
			this.ribbonPageGroup7.Name = "ribbonPageGroup7";
			this.ribbonPageGroup7.Text = "Edit";
			// 
			// ribbonPageGroup2
			// 
			this.ribbonPageGroup2.AllowTextClipping = false;
			this.ribbonPageGroup2.CaptionButtonVisible = DevExpress.Utils.DefaultBoolean.False;
			this.ribbonPageGroup2.ItemLinks.Add(this.btnDelete);
			this.ribbonPageGroup2.Name = "ribbonPageGroup2";
			this.ribbonPageGroup2.Text = "Delete";
			// 
			// ribbonPageGroup4
			// 
			this.ribbonPageGroup4.CaptionButtonVisible = DevExpress.Utils.DefaultBoolean.False;
			this.ribbonPageGroup4.ItemLinks.Add(this.btnPrint);
			this.ribbonPageGroup4.Name = "ribbonPageGroup4";
			this.ribbonPageGroup4.Text = "Quick Reports";
			// 
			// ribbonPageGroup3
			// 
			this.ribbonPageGroup3.Alignment = DevExpress.XtraBars.Ribbon.RibbonPageGroupAlignment.Far;
			this.ribbonPageGroup3.AllowTextClipping = false;
			this.ribbonPageGroup3.CaptionButtonVisible = DevExpress.Utils.DefaultBoolean.False;
			this.ribbonPageGroup3.ItemLinks.Add(this.btnClose);
			this.ribbonPageGroup3.Name = "ribbonPageGroup3";
			this.ribbonPageGroup3.Text = "Close";
			// 
			// ribbonStatusBar1
			// 
			this.ribbonStatusBar1.Location = new System.Drawing.Point(0, 668);
			this.ribbonStatusBar1.Name = "ribbonStatusBar1";
			this.ribbonStatusBar1.Ribbon = this.ribbonControl;
			this.ribbonStatusBar1.Size = new System.Drawing.Size(1362, 30);
			// 
			// mainLayout
			// 
			this.mainLayout.Controls.Add(this.cboStatus);
			this.mainLayout.Controls.Add(this.txtProjectName);
			this.mainLayout.Controls.Add(this.txtReference);
			this.mainLayout.Controls.Add(this.cboClients);
			this.mainLayout.Controls.Add(this.cboEngineers);
			this.mainLayout.Controls.Add(this.dtIssuanceDate);
			this.mainLayout.Controls.Add(this.dtExpiryDate);
			this.mainLayout.Controls.Add(this.memoDescription);
			this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainLayout.Location = new System.Drawing.Point(0, 193);
			this.mainLayout.Name = "mainLayout";
			this.mainLayout.Root = this.Root;
			this.mainLayout.Size = new System.Drawing.Size(1362, 475);
			this.mainLayout.TabIndex = 8;
			// 
			// cboStatus
			// 
			this.cboStatus.Location = new System.Drawing.Point(123, 106);
			this.cboStatus.Name = "cboStatus";
			this.cboStatus.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.cboStatus.Size = new System.Drawing.Size(517, 22);
			this.cboStatus.StyleController = this.mainLayout;
			this.cboStatus.TabIndex = 7;
			// 
			// txtProjectName
			// 
			this.txtProjectName.Location = new System.Drawing.Point(123, 54);
			this.txtProjectName.Name = "txtProjectName";
			this.txtProjectName.Size = new System.Drawing.Size(517, 22);
			this.txtProjectName.StyleController = this.mainLayout;
			this.txtProjectName.TabIndex = 4;
			// 
			// txtReference
			// 
			this.txtReference.Location = new System.Drawing.Point(123, 80);
			this.txtReference.Name = "txtReference";
			this.txtReference.Size = new System.Drawing.Size(517, 22);
			this.txtReference.StyleController = this.mainLayout;
			this.txtReference.TabIndex = 5;
			// 
			// cboClients
			// 
			this.cboClients.Location = new System.Drawing.Point(767, 54);
			this.cboClients.Name = "cboClients";
			this.cboClients.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.cboClients.Properties.NullText = "";
			this.cboClients.Size = new System.Drawing.Size(567, 22);
			this.cboClients.StyleController = this.mainLayout;
			this.cboClients.TabIndex = 8;
			// 
			// cboEngineers
			// 
			this.cboEngineers.Location = new System.Drawing.Point(767, 80);
			this.cboEngineers.Name = "cboEngineers";
			this.cboEngineers.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.cboEngineers.Properties.NullText = "";
			this.cboEngineers.Size = new System.Drawing.Size(567, 22);
			this.cboEngineers.StyleController = this.mainLayout;
			this.cboEngineers.TabIndex = 9;
			// 
			// dtIssuanceDate
			// 
			this.dtIssuanceDate.EditValue = new System.DateTime(2025, 11, 27, 0, 0, 0, 0);
			this.dtIssuanceDate.Location = new System.Drawing.Point(123, 132);
			this.dtIssuanceDate.Name = "dtIssuanceDate";
			this.dtIssuanceDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.dtIssuanceDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.dtIssuanceDate.Size = new System.Drawing.Size(517, 22);
			this.dtIssuanceDate.StyleController = this.mainLayout;
			this.dtIssuanceDate.TabIndex = 10;
			// 
			// dtExpiryDate
			// 
			this.dtExpiryDate.EditValue = new System.DateTime(2025, 11, 27, 0, 0, 0, 0);
			this.dtExpiryDate.Location = new System.Drawing.Point(123, 158);
			this.dtExpiryDate.Name = "dtExpiryDate";
			this.dtExpiryDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.dtExpiryDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.dtExpiryDate.Size = new System.Drawing.Size(517, 22);
			this.dtExpiryDate.StyleController = this.mainLayout;
			this.dtExpiryDate.TabIndex = 11;
			// 
			// memoDescription
			// 
			this.memoDescription.Location = new System.Drawing.Point(14, 218);
			this.memoDescription.Name = "memoDescription";
			this.memoDescription.Size = new System.Drawing.Size(1334, 243);
			this.memoDescription.StyleController = this.mainLayout;
			this.memoDescription.TabIndex = 12;
			// 
			// Root
			// 
			this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
			this.Root.GroupBordersVisible = false;
			this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciDescription,
            this.layoutControlGroup1,
            this.layoutControlGroup2});
			this.Root.Name = "Root";
			this.Root.Size = new System.Drawing.Size(1362, 475);
			this.Root.TextVisible = false;
			// 
			// lciDescription
			// 
			this.lciDescription.Control = this.memoDescription;
			this.lciDescription.Location = new System.Drawing.Point(0, 184);
			this.lciDescription.Name = "lciDescription";
			this.lciDescription.Size = new System.Drawing.Size(1338, 267);
			this.lciDescription.Text = "Description";
			this.lciDescription.TextLocation = DevExpress.Utils.Locations.Top;
			this.lciDescription.TextSize = new System.Drawing.Size(80, 16);
			// 
			// layoutControlGroup1
			// 
			this.layoutControlGroup1.GroupStyle = DevExpress.Utils.GroupStyle.Light;
			this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciProjectName,
            this.lciReference,
            this.lciStatus,
            this.lciIssuanceDate,
            this.lciExpiryDate});
			this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
			this.layoutControlGroup1.Name = "layoutControlGroup1";
			this.layoutControlGroup1.Size = new System.Drawing.Size(644, 184);
			this.layoutControlGroup1.Text = "Project Info";
			// 
			// lciProjectName
			// 
			this.lciProjectName.Control = this.txtProjectName;
			this.lciProjectName.Location = new System.Drawing.Point(0, 0);
			this.lciProjectName.Name = "lciProjectName";
			this.lciProjectName.Size = new System.Drawing.Size(616, 26);
			this.lciProjectName.Text = "Project Name";
			this.lciProjectName.TextSize = new System.Drawing.Size(80, 16);
			// 
			// lciReference
			// 
			this.lciReference.Control = this.txtReference;
			this.lciReference.Location = new System.Drawing.Point(0, 26);
			this.lciReference.Name = "lciReference";
			this.lciReference.Size = new System.Drawing.Size(616, 26);
			this.lciReference.Text = "Reference";
			this.lciReference.TextSize = new System.Drawing.Size(80, 16);
			// 
			// lciStatus
			// 
			this.lciStatus.Control = this.cboStatus;
			this.lciStatus.Location = new System.Drawing.Point(0, 52);
			this.lciStatus.Name = "lciStatus";
			this.lciStatus.Size = new System.Drawing.Size(616, 26);
			this.lciStatus.Text = "Status";
			this.lciStatus.TextSize = new System.Drawing.Size(80, 16);
			// 
			// lciIssuanceDate
			// 
			this.lciIssuanceDate.Control = this.dtIssuanceDate;
			this.lciIssuanceDate.Location = new System.Drawing.Point(0, 78);
			this.lciIssuanceDate.Name = "lciIssuanceDate";
			this.lciIssuanceDate.Size = new System.Drawing.Size(616, 26);
			this.lciIssuanceDate.Text = "Issuance Date";
			this.lciIssuanceDate.TextSize = new System.Drawing.Size(80, 16);
			// 
			// lciExpiryDate
			// 
			this.lciExpiryDate.Control = this.dtExpiryDate;
			this.lciExpiryDate.Location = new System.Drawing.Point(0, 104);
			this.lciExpiryDate.Name = "lciExpiryDate";
			this.lciExpiryDate.Size = new System.Drawing.Size(616, 26);
			this.lciExpiryDate.Text = "Expiry Date";
			this.lciExpiryDate.TextSize = new System.Drawing.Size(80, 16);
			// 
			// layoutControlGroup2
			// 
			this.layoutControlGroup2.GroupStyle = DevExpress.Utils.GroupStyle.Light;
			this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciClient,
            this.lciEngineer});
			this.layoutControlGroup2.Location = new System.Drawing.Point(644, 0);
			this.layoutControlGroup2.Name = "layoutControlGroup2";
			this.layoutControlGroup2.Size = new System.Drawing.Size(694, 184);
			this.layoutControlGroup2.Text = "Resources Details";
			// 
			// lciClient
			// 
			this.lciClient.Control = this.cboClients;
			this.lciClient.Location = new System.Drawing.Point(0, 0);
			this.lciClient.Name = "lciClient";
			this.lciClient.Size = new System.Drawing.Size(666, 26);
			this.lciClient.Text = "Client";
			this.lciClient.TextSize = new System.Drawing.Size(80, 16);
			// 
			// lciEngineer
			// 
			this.lciEngineer.Control = this.cboEngineers;
			this.lciEngineer.Location = new System.Drawing.Point(0, 26);
			this.lciEngineer.Name = "lciEngineer";
			this.lciEngineer.Size = new System.Drawing.Size(666, 104);
			this.lciEngineer.Text = "Engineer";
			this.lciEngineer.TextSize = new System.Drawing.Size(80, 16);
			// 
			// ProjectEditForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1362, 698);
			this.Controls.Add(this.mainLayout);
			this.Controls.Add(this.ribbonStatusBar1);
			this.Controls.Add(this.ribbonControl);
			this.Name = "ProjectEditForm";
			this.Ribbon = this.ribbonControl;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.StatusBar = this.ribbonStatusBar1;
			this.Text = "Project";
			((System.ComponentModel.ISupportInitialize)(this.bsProject)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.mainLayout)).EndInit();
			this.mainLayout.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.cboStatus.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtProjectName.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtReference.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cboClients.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cboEngineers.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dtIssuanceDate.Properties.CalendarTimeProperties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dtIssuanceDate.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dtExpiryDate.Properties.CalendarTimeProperties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dtExpiryDate.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.memoDescription.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.lciDescription)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.lciProjectName)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.lciReference)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.lciStatus)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.lciIssuanceDate)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.lciExpiryDate)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.lciClient)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.lciEngineer)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		private System.Windows.Forms.BindingSource bsProject;
		private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl;
		private DevExpress.XtraBars.BarButtonItem btnSave;
		private DevExpress.XtraBars.BarButtonItem btnClose;
		private DevExpress.XtraBars.BarButtonItem btnSaveAndClose;
		private DevExpress.XtraBars.BarButtonItem btnDelete;
		private DevExpress.XtraBars.BarButtonItem btnPrint;
		private DevExpress.XtraBars.BarButtonItem btnRefresh;
		private DevExpress.XtraBars.BarButtonItem btnNew;
		private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup8;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup7;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup4;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
		private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar1;
		private DevExpress.XtraLayout.LayoutControl mainLayout;
		private DevExpress.XtraEditors.ComboBoxEdit cboStatus;
		private DevExpress.XtraEditors.TextEdit txtProjectName;
		private DevExpress.XtraEditors.TextEdit txtReference;
		private DevExpress.XtraEditors.LookUpEdit cboClients;
		private DevExpress.XtraEditors.LookUpEdit cboEngineers;
		private DevExpress.XtraEditors.DateEdit dtIssuanceDate;
		private DevExpress.XtraEditors.DateEdit dtExpiryDate;
		private DevExpress.XtraEditors.MemoEdit memoDescription;
		private DevExpress.XtraLayout.LayoutControlGroup Root;
		private DevExpress.XtraLayout.LayoutControlItem lciProjectName;
		private DevExpress.XtraLayout.LayoutControlItem lciReference;
		private DevExpress.XtraLayout.LayoutControlItem lciClient;
		private DevExpress.XtraLayout.LayoutControlItem lciEngineer;
		private DevExpress.XtraLayout.LayoutControlItem lciStatus;
		private DevExpress.XtraLayout.LayoutControlItem lciIssuanceDate;
		private DevExpress.XtraLayout.LayoutControlItem lciExpiryDate;
		private DevExpress.XtraLayout.LayoutControlItem lciDescription;
		private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
		private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
	}
}
