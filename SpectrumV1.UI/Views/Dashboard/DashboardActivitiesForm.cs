using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using SpectrumV1.Utilities.Interfaces;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SpectrumV1.Views.Dashboard
{
	public partial class DashboardActivitiesForm : RibbonForm, IFormWithRibbon
	{
		private LayoutControl layoutControl;

		// Grid for activities
		private GridControl gcActivities;
		private GridView gvActivities;

		// Summary grid for projects by status
		private GridControl gcProjectsByStatus;
		private GridView gvProjectsByStatus;

		public RibbonControl MainRibbon => rcDashboard;
		public RibbonPage DefaultPage => rpDashboard;

		public DashboardActivitiesForm()
		{
			InitializeComponent();
			InitializeDashboardLayout();
			LoadSampleData();
		}

		private void InitializeDashboardLayout()
		{
			// Create main layout control
			layoutControl = new LayoutControl();
			layoutControl.Dock = DockStyle.Fill;
			this.Controls.Add(layoutControl);
			layoutControl.BringToFront();

			layoutControl.Root.GroupBordersVisible = false;
			layoutControl.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(10);

			// Create Activities Grid
			CreateActivitiesGrid();

			// Create Projects By Status Summary
			CreateProjectsByStatusGrid();

			// Arrange layout in bento style
			ArrangeBentoLayout();
		}

		private void CreateActivitiesGrid()
		{
			gcActivities = new GridControl();
			gvActivities = new GridView(gcActivities);
			gcActivities.MainView = gvActivities;
			gcActivities.ViewCollection.Add(gvActivities);

			gvActivities.OptionsView.ShowGroupPanel = false;
			gvActivities.OptionsView.ColumnAutoWidth = true;
			gvActivities.OptionsBehavior.Editable = false;
			gvActivities.OptionsView.ShowIndicator = false;
		}

		private void CreateProjectsByStatusGrid()
		{
			gcProjectsByStatus = new GridControl();
			gvProjectsByStatus = new GridView(gcProjectsByStatus);
			gcProjectsByStatus.MainView = gvProjectsByStatus;
			gcProjectsByStatus.ViewCollection.Add(gvProjectsByStatus);

			gvProjectsByStatus.OptionsView.ShowGroupPanel = false;
			gvProjectsByStatus.OptionsView.ColumnAutoWidth = true;
			gvProjectsByStatus.OptionsBehavior.Editable = false;
			gvProjectsByStatus.OptionsView.ShowIndicator = false;
		}

		private void ArrangeBentoLayout()
		{
			// Create panels for KPI cards with accent colors
			var pnlRevenue = CreateKpiPanel("$283,000", "Total Revenue", Color.FromArgb(76, 175, 80));
			var pnlProjects = CreateKpiPanel("104", "Active Projects", Color.FromArgb(33, 150, 243));
			var pnlEngineers = CreateKpiPanel("168", "Engineers", Color.FromArgb(255, 152, 0));
			var pnlClients = CreateKpiPanel("12", "Clients", Color.FromArgb(156, 39, 176));

			// Row 1: KPI Cards
			var liRevenue = layoutControl.AddItem("Total Revenue", pnlRevenue);
			liRevenue.TextVisible = false;
			liRevenue.SizeConstraintsType = SizeConstraintsType.Custom;
			liRevenue.MinSize = new Size(200, 100);

			var liProjects = layoutControl.AddItem("Active Projects", pnlProjects);
			liProjects.TextVisible = false;
			liProjects.SizeConstraintsType = SizeConstraintsType.Custom;
			liProjects.MinSize = new Size(200, 100);

			var liEngineers = layoutControl.AddItem("Engineers", pnlEngineers);
			liEngineers.TextVisible = false;
			liEngineers.SizeConstraintsType = SizeConstraintsType.Custom;
			liEngineers.MinSize = new Size(200, 100);

			var liClients = layoutControl.AddItem("Clients", pnlClients);
			liClients.TextVisible = false;
			liClients.SizeConstraintsType = SizeConstraintsType.Custom;
			liClients.MinSize = new Size(200, 100);

			// Row 2: Projects by Status Summary
			var liStatusSummary = layoutControl.AddItem("Projects by Status", gcProjectsByStatus);
			liStatusSummary.TextLocation = DevExpress.Utils.Locations.Top;
			liStatusSummary.AppearanceItemCaption.Font = new Font("Segoe UI", 12, FontStyle.Bold);
			liStatusSummary.SizeConstraintsType = SizeConstraintsType.Custom;
			liStatusSummary.MinSize = new Size(300, 180);

			// Row 3: Recent Activities Grid
			var liActivities = layoutControl.AddItem("Recent Activities", gcActivities);
			liActivities.TextLocation = DevExpress.Utils.Locations.Top;
			liActivities.AppearanceItemCaption.Font = new Font("Segoe UI", 12, FontStyle.Bold);
			liActivities.SizeConstraintsType = SizeConstraintsType.Custom;
			liActivities.MinSize = new Size(400, 200);

			// Arrange items using table layout mode for bento style
			layoutControl.Root.BeginUpdate();
			layoutControl.Root.LayoutMode = DevExpress.XtraLayout.Utils.LayoutMode.Table;

			// Define 4 columns
			layoutControl.Root.OptionsTableLayoutGroup.ColumnDefinitions.Clear();
			layoutControl.Root.OptionsTableLayoutGroup.ColumnDefinitions.Add(new ColumnDefinition { SizeType = SizeType.Percent, Width = 25 });
			layoutControl.Root.OptionsTableLayoutGroup.ColumnDefinitions.Add(new ColumnDefinition { SizeType = SizeType.Percent, Width = 25 });
			layoutControl.Root.OptionsTableLayoutGroup.ColumnDefinitions.Add(new ColumnDefinition { SizeType = SizeType.Percent, Width = 25 });
			layoutControl.Root.OptionsTableLayoutGroup.ColumnDefinitions.Add(new ColumnDefinition { SizeType = SizeType.Percent, Width = 25 });

			// Define 3 rows
			layoutControl.Root.OptionsTableLayoutGroup.RowDefinitions.Clear();
			layoutControl.Root.OptionsTableLayoutGroup.RowDefinitions.Add(new RowDefinition { SizeType = SizeType.Absolute, Height = 110 });
			layoutControl.Root.OptionsTableLayoutGroup.RowDefinitions.Add(new RowDefinition { SizeType = SizeType.Percent, Height = 35 });
			layoutControl.Root.OptionsTableLayoutGroup.RowDefinitions.Add(new RowDefinition { SizeType = SizeType.Percent, Height = 65 });

			// Position KPI cards in row 0
			liRevenue.OptionsTableLayoutItem.RowIndex = 0;
			liRevenue.OptionsTableLayoutItem.ColumnIndex = 0;

			liProjects.OptionsTableLayoutItem.RowIndex = 0;
			liProjects.OptionsTableLayoutItem.ColumnIndex = 1;

			liEngineers.OptionsTableLayoutItem.RowIndex = 0;
			liEngineers.OptionsTableLayoutItem.ColumnIndex = 2;

			liClients.OptionsTableLayoutItem.RowIndex = 0;
			liClients.OptionsTableLayoutItem.ColumnIndex = 3;

			// Position status summary in row 1, spanning 4 columns
			liStatusSummary.OptionsTableLayoutItem.RowIndex = 1;
			liStatusSummary.OptionsTableLayoutItem.ColumnIndex = 0;
			liStatusSummary.OptionsTableLayoutItem.ColumnSpan = 4;

			// Position activities grid in row 2, spanning 4 columns
			liActivities.OptionsTableLayoutItem.RowIndex = 2;
			liActivities.OptionsTableLayoutItem.ColumnIndex = 0;
			liActivities.OptionsTableLayoutItem.ColumnSpan = 4;

			layoutControl.Root.EndUpdate();
		}

		private PanelControl CreateKpiPanel(string value, string title, Color accentColor)
		{
			var panel = new PanelControl();
			panel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
			panel.Size = new Size(200, 100);
			panel.Padding = new Padding(15);

			// Create accent bar on left
			var accentBar = new PanelControl();
			accentBar.Width = 5;
			accentBar.Dock = DockStyle.Left;
			accentBar.BackColor = accentColor;
			accentBar.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
			panel.Controls.Add(accentBar);

			var lblValue = new LabelControl();
			lblValue.Text = value;
			lblValue.Font = new Font("Segoe UI", 28, FontStyle.Bold);
			lblValue.ForeColor = accentColor;
			lblValue.AutoSizeMode = LabelAutoSizeMode.None;
			lblValue.Dock = DockStyle.Top;
			lblValue.Height = 45;
			lblValue.Padding = new Padding(15, 5, 0, 0);
			panel.Controls.Add(lblValue);

			var lblTitle = new LabelControl();
			lblTitle.Text = title;
			lblTitle.Font = new Font("Segoe UI", 11, FontStyle.Regular);
			lblTitle.AutoSizeMode = LabelAutoSizeMode.None;
			lblTitle.Dock = DockStyle.Bottom;
			lblTitle.Height = 25;
			lblTitle.Padding = new Padding(15, 0, 0, 5);
			lblTitle.Appearance.ForeColor = Color.Gray;
			panel.Controls.Add(lblTitle);

			return panel;
		}

		private void LoadSampleData()
		{
			var activities = GetSampleActivities();

			// Bind activities grid
			gcActivities.DataSource = activities;

			// Create summary by status
			var statusSummary = activities
				.GroupBy(a => a.Status)
				.Select(g => new
				{
					Status = g.Key,
					ProjectCount = g.Sum(x => x.ProjectCount),
					TotalRevenue = g.Sum(x => x.Revenue),
					EngineersCount = g.Sum(x => x.EngineersCount)
				})
				.ToList();

			gcProjectsByStatus.DataSource = statusSummary;
		}

		private List<ActivityData> GetSampleActivities()
		{
			return new List<ActivityData>
			{
				new ActivityData { ActivityName = "Project Alpha", Status = "Active", Month = "January", Revenue = 15000, ProjectCount = 5, EngineersCount = 12, ActivityCount = 45 },
				new ActivityData { ActivityName = "Project Beta", Status = "Active", Month = "February", Revenue = 22000, ProjectCount = 8, EngineersCount = 15, ActivityCount = 62 },
				new ActivityData { ActivityName = "Project Gamma", Status = "Completed", Month = "March", Revenue = 18500, ProjectCount = 6, EngineersCount = 10, ActivityCount = 38 },
				new ActivityData { ActivityName = "Project Delta", Status = "Active", Month = "April", Revenue = 31000, ProjectCount = 12, EngineersCount = 18, ActivityCount = 75 },
				new ActivityData { ActivityName = "Project Epsilon", Status = "On Hold", Month = "May", Revenue = 9500, ProjectCount = 3, EngineersCount = 8, ActivityCount = 22 },
				new ActivityData { ActivityName = "Project Zeta", Status = "Active", Month = "June", Revenue = 27500, ProjectCount = 10, EngineersCount = 14, ActivityCount = 58 },
				new ActivityData { ActivityName = "Project Eta", Status = "Completed", Month = "July", Revenue = 21000, ProjectCount = 7, EngineersCount = 11, ActivityCount = 41 },
				new ActivityData { ActivityName = "Project Theta", Status = "Active", Month = "August", Revenue = 35000, ProjectCount = 15, EngineersCount = 20, ActivityCount = 82 },
				new ActivityData { ActivityName = "Project Iota", Status = "On Hold", Month = "September", Revenue = 12000, ProjectCount = 4, EngineersCount = 9, ActivityCount = 28 },
				new ActivityData { ActivityName = "Project Kappa", Status = "Active", Month = "October", Revenue = 29000, ProjectCount = 11, EngineersCount = 16, ActivityCount = 67 },
				new ActivityData { ActivityName = "Project Lambda", Status = "Completed", Month = "November", Revenue = 24500, ProjectCount = 9, EngineersCount = 13, ActivityCount = 52 },
				new ActivityData { ActivityName = "Project Mu", Status = "Active", Month = "December", Revenue = 38000, ProjectCount = 14, EngineersCount = 22, ActivityCount = 91 }
			};
		}

		private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			Close();
		}

		private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			LoadSampleData();
		}
	}

	public class ActivityData
	{
		public string ActivityName { get; set; }
		public string Status { get; set; }
		public string Month { get; set; }
		public decimal Revenue { get; set; }
		public int ProjectCount { get; set; }
		public int EngineersCount { get; set; }
		public int ActivityCount { get; set; }
	}
}
