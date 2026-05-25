namespace Spectrum.Views.Upgrade
{
    partial class ChartMigrateForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChartMigrateForm));
            this.mainLayout = new DevExpress.XtraLayout.LayoutControl();
            this.lblCounter = new DevExpress.XtraEditors.LabelControl();
            this.progressBarControl1 = new DevExpress.XtraEditors.ProgressBarControl();
            this.btnMigrate = new DevExpress.XtraEditors.SimpleButton();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.btnReadFile = new DevExpress.XtraEditors.SimpleButton();
            this.gcPreview = new DevExpress.XtraGrid.GridControl();
            this.gvPreview = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.txtFilePath = new DevExpress.XtraEditors.ButtonEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lblSource = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup4 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.progressBarLayout = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem9 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.mainLayout)).BeginInit();
            this.mainLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.progressBarControl1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFilePath.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBarLayout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // mainLayout
            // 
            this.mainLayout.Controls.Add(this.lblCounter);
            this.mainLayout.Controls.Add(this.progressBarControl1);
            this.mainLayout.Controls.Add(this.btnMigrate);
            this.mainLayout.Controls.Add(this.btnClose);
            this.mainLayout.Controls.Add(this.btnReadFile);
            this.mainLayout.Controls.Add(this.gcPreview);
            this.mainLayout.Controls.Add(this.txtFilePath);
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.Location = new System.Drawing.Point(0, 0);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.Root = this.Root;
            this.mainLayout.Size = new System.Drawing.Size(1230, 642);
            this.mainLayout.TabIndex = 5;
            this.mainLayout.Text = "mainLayout";
            // 
            // lblCounter
            // 
            this.lblCounter.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblCounter.Location = new System.Drawing.Point(832, 567);
            this.lblCounter.Name = "lblCounter";
            this.lblCounter.Size = new System.Drawing.Size(370, 16);
            this.lblCounter.StyleController = this.mainLayout;
            this.lblCounter.TabIndex = 25;
            this.lblCounter.Text = " Records Found: 0      Valid Records: 0      Invalid Records: 0";
            // 
            // progressBarControl1
            // 
            this.progressBarControl1.Location = new System.Drawing.Point(28, 567);
            this.progressBarControl1.Name = "progressBarControl1";
            this.progressBarControl1.Size = new System.Drawing.Size(790, 16);
            this.progressBarControl1.StyleController = this.mainLayout;
            this.progressBarControl1.TabIndex = 24;
            // 
            // btnMigrate
            // 
            this.btnMigrate.Location = new System.Drawing.Point(842, 601);
            this.btnMigrate.Name = "btnMigrate";
            this.btnMigrate.Size = new System.Drawing.Size(168, 27);
            this.btnMigrate.StyleController = this.mainLayout;
            this.btnMigrate.TabIndex = 23;
            this.btnMigrate.Text = "Migrate";
            this.btnMigrate.Click += new System.EventHandler(this.btnMigrate_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(1014, 601);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(202, 27);
            this.btnClose.StyleController = this.mainLayout;
            this.btnClose.TabIndex = 22;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnReadFile
            // 
            this.btnReadFile.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnReadFile.ImageOptions.SvgImage")));
            this.btnReadFile.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.btnReadFile.Location = new System.Drawing.Point(1107, 14);
            this.btnReadFile.Name = "btnReadFile";
            this.btnReadFile.Size = new System.Drawing.Size(109, 27);
            this.btnReadFile.StyleController = this.mainLayout;
            this.btnReadFile.TabIndex = 21;
            this.btnReadFile.TabStop = false;
            this.btnReadFile.Text = "Read file";
            this.btnReadFile.Click += new System.EventHandler(this.btnReadFile_Click);
            // 
            // gcPreview
            // 
            this.gcPreview.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcPreview.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcPreview.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcPreview.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcPreview.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcPreview.Location = new System.Drawing.Point(28, 85);
            this.gcPreview.MainView = this.gvPreview;
            this.gcPreview.Name = "gcPreview";
            this.gcPreview.Size = new System.Drawing.Size(1174, 456);
            this.gcPreview.TabIndex = 20;
            this.gcPreview.UseEmbeddedNavigator = true;
            this.gcPreview.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvPreview});
            // 
            // gvPreview
            // 
            this.gvPreview.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn3,
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn2,
            this.gridColumn4,
            this.gridColumn5});
            this.gvPreview.GridControl = this.gcPreview;
            this.gvPreview.Name = "gvPreview";
            this.gvPreview.OptionsView.ColumnAutoWidth = false;
            this.gvPreview.OptionsView.ShowGroupPanel = false;
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(69, 14);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Plus)});
            this.txtFilePath.Size = new System.Drawing.Size(1024, 22);
            this.txtFilePath.StyleController = this.mainLayout;
            this.txtFilePath.TabIndex = 13;
            this.txtFilePath.TabStop = false;
            this.txtFilePath.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtUploadCv_ButtonClick);
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lblSource,
            this.layoutControlGroup4,
            this.layoutControlItem5,
            this.emptySpaceItem9,
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.emptySpaceItem1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(1230, 642);
            this.Root.TextVisible = false;
            // 
            // lblSource
            // 
            this.lblSource.Control = this.txtFilePath;
            this.lblSource.Location = new System.Drawing.Point(0, 0);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(1083, 31);
            this.lblSource.Text = "Source";
            this.lblSource.TextSize = new System.Drawing.Size(40, 16);
            // 
            // layoutControlGroup4
            // 
            this.layoutControlGroup4.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.layoutControlGroup4.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem4,
            this.progressBarLayout,
            this.emptySpaceItem3,
            this.emptySpaceItem2,
            this.layoutControlItem6});
            this.layoutControlGroup4.Location = new System.Drawing.Point(0, 31);
            this.layoutControlGroup4.Name = "layoutControlGroup4";
            this.layoutControlGroup4.Size = new System.Drawing.Size(1206, 556);
            this.layoutControlGroup4.Text = "Preview list";
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.gcPreview;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(1178, 460);
            this.layoutControlItem4.TextVisible = false;
            // 
            // progressBarLayout
            // 
            this.progressBarLayout.Control = this.progressBarControl1;
            this.progressBarLayout.Location = new System.Drawing.Point(0, 482);
            this.progressBarLayout.Name = "progressBarLayout";
            this.progressBarLayout.Size = new System.Drawing.Size(794, 20);
            this.progressBarLayout.TextVisible = false;
            this.progressBarLayout.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // emptySpaceItem3
            // 
            this.emptySpaceItem3.Location = new System.Drawing.Point(0, 460);
            this.emptySpaceItem3.Name = "emptySpaceItem3";
            this.emptySpaceItem3.Size = new System.Drawing.Size(1178, 22);
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.Location = new System.Drawing.Point(794, 482);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(10, 20);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.lblCounter;
            this.layoutControlItem6.Location = new System.Drawing.Point(804, 482);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(374, 20);
            this.layoutControlItem6.TextVisible = false;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.btnReadFile;
            this.layoutControlItem5.Location = new System.Drawing.Point(1093, 0);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(113, 31);
            this.layoutControlItem5.TextVisible = false;
            // 
            // emptySpaceItem9
            // 
            this.emptySpaceItem9.Location = new System.Drawing.Point(1083, 0);
            this.emptySpaceItem9.Name = "emptySpaceItem9";
            this.emptySpaceItem9.Size = new System.Drawing.Size(10, 31);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.btnClose;
            this.layoutControlItem1.Location = new System.Drawing.Point(1000, 587);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(206, 31);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.btnMigrate;
            this.layoutControlItem2.Location = new System.Drawing.Point(828, 587);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(172, 31);
            this.layoutControlItem2.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 587);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(828, 31);
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "acno";
            this.gridColumn1.FieldName = "acno";
            this.gridColumn1.MinWidth = 25;
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 70;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "acname1";
            this.gridColumn2.FieldName = "acname1";
            this.gridColumn2.MinWidth = 25;
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 4;
            this.gridColumn2.Width = 220;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "glacno";
            this.gridColumn3.FieldName = "glacno";
            this.gridColumn3.MinWidth = 25;
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 1;
            this.gridColumn3.Width = 70;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "detail";
            this.gridColumn4.FieldName = "detail";
            this.gridColumn4.MinWidth = 25;
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 5;
            this.gridColumn4.Width = 70;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "crid";
            this.gridColumn5.FieldName = "crid";
            this.gridColumn5.MinWidth = 25;
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 6;
            this.gridColumn5.Width = 70;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "Number";
            this.gridColumn6.FieldName = "Number";
            this.gridColumn6.MinWidth = 25;
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 2;
            this.gridColumn6.Width = 70;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "Serial";
            this.gridColumn7.FieldName = "Serial";
            this.gridColumn7.MinWidth = 25;
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 3;
            this.gridColumn7.Width = 70;
            // 
            // ChartMigrateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1230, 642);
            this.Controls.Add(this.mainLayout);
            this.MaximizeBox = false;
            this.Name = "ChartMigrateForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Upgrade Chart of Account";
            ((System.ComponentModel.ISupportInitialize)(this.mainLayout)).EndInit();
            this.mainLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.progressBarControl1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFilePath.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBarLayout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl mainLayout;
        private DevExpress.XtraEditors.SimpleButton btnReadFile;
        private DevExpress.XtraGrid.GridControl gcPreview;
        private DevExpress.XtraGrid.Views.Grid.GridView gvPreview;
        private DevExpress.XtraEditors.ButtonEdit txtFilePath;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlItem lblSource;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem3;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem9;
        private DevExpress.XtraEditors.SimpleButton btnMigrate;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraEditors.ProgressBarControl progressBarControl1;
        private DevExpress.XtraLayout.LayoutControlItem progressBarLayout;
        private DevExpress.XtraEditors.LabelControl lblCounter;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
    }
}