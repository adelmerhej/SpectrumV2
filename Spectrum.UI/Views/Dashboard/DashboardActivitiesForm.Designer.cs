namespace Spectrum.Views.Dashboard
{
    partial class DashboardActivitiesForm
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
            this.rcDashboard = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.btnRefresh = new DevExpress.XtraBars.BarButtonItem();
            this.btnClose = new DevExpress.XtraBars.BarButtonItem();
            this.rpDashboard = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.rpgDashboard = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonStatusBar1 = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            ((System.ComponentModel.ISupportInitialize)(this.rcDashboard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            this.SuspendLayout();
            // 
            // rcDashboard
            // 
            this.rcDashboard.EmptyAreaImageOptions.ImagePadding = new System.Windows.Forms.Padding(35, 37, 35, 37);
            this.rcDashboard.ExpandCollapseItem.Id = 0;
            this.rcDashboard.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.rcDashboard.ExpandCollapseItem,
            this.btnRefresh,
            this.btnClose});
            this.rcDashboard.Location = new System.Drawing.Point(0, 0);
            this.rcDashboard.Margin = new System.Windows.Forms.Padding(4);
            this.rcDashboard.MaxItemId = 3;
            this.rcDashboard.MdiMergeStyle = DevExpress.XtraBars.Ribbon.RibbonMdiMergeStyle.Always;
            this.rcDashboard.Name = "rcDashboard";
            this.rcDashboard.OptionsMenuMinWidth = 385;
            this.rcDashboard.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.rpDashboard});
            this.rcDashboard.Size = new System.Drawing.Size(1425, 193);
            this.rcDashboard.StatusBar = this.ribbonStatusBar1;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Caption = "Refresh";
            this.btnRefresh.Id = 1;
            this.btnRefresh.Name = "btnRefresh";
            // 
            // btnClose
            // 
            this.btnClose.Caption = "Close";
            this.btnClose.Id = 2;
            this.btnClose.Name = "btnClose";
            // 
            // rpDashboard
            // 
            this.rpDashboard.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.rpgDashboard});
            this.rpDashboard.Name = "rpDashboard";
            this.rpDashboard.Text = "Dashboard";
            // 
            // rpgDashboard
            // 
            this.rpgDashboard.ItemLinks.Add(this.btnRefresh);
            this.rpgDashboard.ItemLinks.Add(this.btnClose);
            this.rpgDashboard.Name = "rpgDashboard";
            this.rpgDashboard.Text = "Actions";
            // 
            // ribbonStatusBar1
            // 
            this.ribbonStatusBar1.Location = new System.Drawing.Point(0, 759);
            this.ribbonStatusBar1.Name = "ribbonStatusBar1";
            this.ribbonStatusBar1.Ribbon = this.rcDashboard;
            this.ribbonStatusBar1.Size = new System.Drawing.Size(1425, 30);
            // 
            // layoutControl
            // 
            this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl.Location = new System.Drawing.Point(0, 193);
            this.layoutControl.Name = "layoutControl";
            this.layoutControl.Root = this.Root;
            this.layoutControl.Size = new System.Drawing.Size(1425, 566);
            this.layoutControl.TabIndex = 2;
            this.layoutControl.Text = "layoutControl1";
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(1425, 566);
            this.Root.TextVisible = false;
            // 
            // DashboardActivitiesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1425, 789);
            this.Controls.Add(this.layoutControl);
            this.Controls.Add(this.rcDashboard);
            this.Controls.Add(this.ribbonStatusBar1);
            this.Name = "DashboardActivitiesForm";
            this.Ribbon = this.rcDashboard;
            this.StatusBar = this.ribbonStatusBar1;
            this.Text = "DashboardActivitiesForm";
            ((System.ComponentModel.ISupportInitialize)(this.rcDashboard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraBars.Ribbon.RibbonControl rcDashboard;
        private DevExpress.XtraBars.BarButtonItem btnRefresh;
        private DevExpress.XtraBars.BarButtonItem btnClose;
        private DevExpress.XtraBars.Ribbon.RibbonPage rpDashboard;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rpgDashboard;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar1;
        private DevExpress.XtraLayout.LayoutControl layoutControl;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
    }
}