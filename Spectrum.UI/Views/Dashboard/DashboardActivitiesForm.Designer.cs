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
			((System.ComponentModel.ISupportInitialize)(this.rcDashboard)).BeginInit();
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
			this.rcDashboard.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.rcDashboard.MaxItemId = 3;
			this.rcDashboard.MdiMergeStyle = DevExpress.XtraBars.Ribbon.RibbonMdiMergeStyle.Always;
			this.rcDashboard.Name = "rcDashboard";
			this.rcDashboard.OptionsMenuMinWidth = 385;
			this.rcDashboard.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.rpDashboard});
			this.rcDashboard.Size = new System.Drawing.Size(1425, 193);
			// 
			// btnRefresh
			// 
			this.btnRefresh.Caption = "Refresh";
			this.btnRefresh.Id = 1;
			this.btnRefresh.ImageOptions.Image = global::Spectrum.Properties.Resources.refresh_button_10_gray;
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnRefresh_ItemClick);
			// 
			// btnClose
			// 
			this.btnClose.Caption = "Close";
			this.btnClose.Id = 2;
			this.btnClose.ImageOptions.Image = global::Spectrum.Properties.Resources.close_button_12_light;
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
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1425, 760);
			this.Controls.Add(this.rcDashboard);
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
