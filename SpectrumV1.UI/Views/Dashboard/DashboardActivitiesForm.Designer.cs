namespace SpectrumV1.Views.Dashboard
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
			this.components = new System.ComponentModel.Container();
			this.rcDashboard = new DevExpress.XtraBars.Ribbon.RibbonControl();
			this.btnRefresh = new DevExpress.XtraBars.BarButtonItem();
			this.btnClose = new DevExpress.XtraBars.BarButtonItem();
			this.rpDashboard = new DevExpress.XtraBars.Ribbon.RibbonPage();
			this.rpgDashboard = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			((System.ComponentModel.ISupportInitialize)(this.rcDashboard)).BeginInit();
			this.SuspendLayout();
			// 
			// rcDashboard
			// 
			this.rcDashboard.ExpandCollapseItem.Id = 0;
			this.rcDashboard.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.rcDashboard.ExpandCollapseItem,
            this.rcDashboard.SearchEditItem,
            this.btnRefresh,
            this.btnClose});
			this.rcDashboard.Location = new System.Drawing.Point(0, 0);
			this.rcDashboard.MaxItemId = 3;
			this.rcDashboard.MdiMergeStyle = DevExpress.XtraBars.Ribbon.RibbonMdiMergeStyle.Always;
			this.rcDashboard.Name = "rcDashboard";
			this.rcDashboard.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.rpDashboard});
			this.rcDashboard.Size = new System.Drawing.Size(1000, 158);
			// 
			// btnRefresh
			// 
			this.btnRefresh.Caption = "Refresh";
			this.btnRefresh.Id = 1;
			this.btnRefresh.ImageOptions.Image = global::SpectrumV1.Properties.Resources.refresh_button_10_gray;
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnRefresh_ItemClick);
			// 
			// btnClose
			// 
			this.btnClose.Caption = "Close";
			this.btnClose.Id = 2;
			this.btnClose.ImageOptions.Image = global::SpectrumV1.Properties.Resources.close_button_12_light;
			this.btnClose.Name = "btnClose";
			this.btnClose.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnClose_ItemClick);
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
			// DashboardActivitiesForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1000, 700);
			this.Controls.Add(this.rcDashboard);
			this.Name = "DashboardActivitiesForm";
			this.Ribbon = this.rcDashboard;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Activities Dashboard";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			((System.ComponentModel.ISupportInitialize)(this.rcDashboard)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private DevExpress.XtraBars.Ribbon.RibbonControl rcDashboard;
		private DevExpress.XtraBars.BarButtonItem btnRefresh;
		private DevExpress.XtraBars.BarButtonItem btnClose;
		private DevExpress.XtraBars.Ribbon.RibbonPage rpDashboard;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup rpgDashboard;
	}
}
