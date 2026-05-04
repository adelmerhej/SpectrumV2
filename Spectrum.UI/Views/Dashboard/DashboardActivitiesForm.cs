using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using Spectrum.Utilities.Interfaces;
using SpectrumV1.Models.Administration.Dashboard;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Spectrum.Views.Dashboard
{
    public partial class DashboardActivitiesForm : RibbonForm, IFormWithRibbon
    {
        public RibbonControl MainRibbon => rcDashboard;
        public RibbonPage DefaultPage => rpDashboard;

        public DashboardActivitiesForm()
        {
            InitializeComponent();

            //InitializeDashboardLayout();
            //LoadSampleData();
        }

        //private void InitializeDashboardLayout()
        //{
        //    // Create main layout control with professional styling
        //    layoutControl = new LayoutControl();
        //    layoutControl.Dock = DockStyle.Fill;
        //    layoutControl.BackColor = Color.FromArgb(240, 244, 248);
        //    this.Controls.Add(layoutControl);
        //    layoutControl.BringToFront();

        //    layoutControl.Root.GroupBordersVisible = false;
        //    layoutControl.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(20);

        //    // Create all dashboard components
        //    CreateActivitiesGrid();
        //    CreateProjectsByStatusGrid();
        //    CreateTopProjectsGrid();

        //    // Arrange in professional bento layout
        //    ArrangeBentoLayout();
        //}

        //private void CreateActivitiesGrid()
        //{
        //    gcActivities = new GridControl();
        //    gvActivities = new GridView(gcActivities);
        //    gcActivities.MainView = gvActivities;
        //    gcActivities.ViewCollection.Add(gvActivities);

        //    // Professional grid styling
        //    gvActivities.OptionsView.ShowGroupPanel = false;
        //    gvActivities.OptionsView.ColumnAutoWidth = false;
        //    gvActivities.OptionsBehavior.Editable = false;
        //    gvActivities.OptionsView.ShowIndicator = false;
        //    gvActivities.OptionsView.EnableAppearanceEvenRow = true;
        //    gvActivities.OptionsView.EnableAppearanceOddRow = true;
        //    gvActivities.Appearance.EvenRow.BackColor = Color.White;
        //    gvActivities.Appearance.OddRow.BackColor = Color.FromArgb(249, 250, 251);
        //    gvActivities.Appearance.HeaderPanel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        //    gvActivities.Appearance.HeaderPanel.Options.UseFont = true;
        //    gvActivities.Appearance.Row.Font = new Font("Segoe UI", 9F);
        //    gvActivities.Appearance.Row.Options.UseFont = true;
        //    gvActivities.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        //}

        //private void CreateProjectsByStatusGrid()
        //{
        //    gcProjectsByStatus = new GridControl();
        //    gvProjectsByStatus = new GridView(gcProjectsByStatus);
        //    gcProjectsByStatus.MainView = gvProjectsByStatus;
        //    gcProjectsByStatus.ViewCollection.Add(gvProjectsByStatus);

        //    // Professional summary grid styling
        //    gvProjectsByStatus.OptionsView.ShowGroupPanel = false;
        //    gvProjectsByStatus.OptionsView.ColumnAutoWidth = false;
        //    gvProjectsByStatus.OptionsBehavior.Editable = false;
        //    gvProjectsByStatus.OptionsView.ShowIndicator = false;
        //    gvProjectsByStatus.OptionsView.EnableAppearanceEvenRow = true;
        //    gvProjectsByStatus.OptionsView.EnableAppearanceOddRow = true;
        //    gvProjectsByStatus.Appearance.EvenRow.BackColor = Color.White;
        //    gvProjectsByStatus.Appearance.OddRow.BackColor = Color.FromArgb(249, 250, 251);
        //    gvProjectsByStatus.Appearance.HeaderPanel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        //    gvProjectsByStatus.Appearance.HeaderPanel.Options.UseFont = true;
        //    gvProjectsByStatus.Appearance.Row.Font = new Font("Segoe UI", 9F);
        //    gvProjectsByStatus.Appearance.Row.Options.UseFont = true;
        //    gvProjectsByStatus.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        //}

        //private void CreateTopProjectsGrid()
        //{
        //    gcTopProjects = new GridControl();
        //    gvTopProjects = new GridView(gcTopProjects);
        //    gcTopProjects.MainView = gvTopProjects;
        //    gcTopProjects.ViewCollection.Add(gvTopProjects);

        //    // Professional top projects grid styling
        //    gvTopProjects.OptionsView.ShowGroupPanel = false;
        //    gvTopProjects.OptionsView.ColumnAutoWidth = false;
        //    gvTopProjects.OptionsBehavior.Editable = false;
        //    gvTopProjects.OptionsView.ShowIndicator = false;
        //    gvTopProjects.OptionsView.EnableAppearanceEvenRow = true;
        //    gvTopProjects.OptionsView.EnableAppearanceOddRow = true;
        //    gvTopProjects.Appearance.EvenRow.BackColor = Color.White;
        //    gvTopProjects.Appearance.OddRow.BackColor = Color.FromArgb(249, 250, 251);
        //    gvTopProjects.Appearance.HeaderPanel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        //    gvTopProjects.Appearance.HeaderPanel.Options.UseFont = true;
        //    gvTopProjects.Appearance.Row.Font = new Font("Segoe UI", 9F);
        //    gvTopProjects.Appearance.Row.Options.UseFont = true;
        //    gvTopProjects.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        //}

        //private void ArrangeBentoLayout()
        //{
        //    // Create KPI cards with professional design
        //    pnlRevenue = CreateKpiPanel("$0", "Total Revenue", Color.FromArgb(16, 185, 129), "💰");
        //    pnlProjects = CreateKpiPanel("0", "Active Projects", Color.FromArgb(59, 130, 246), "📊");
        //    pnlEngineers = CreateKpiPanel("0", "Engineers", Color.FromArgb(245, 158, 11), "👥");
        //    pnlClients = CreateKpiPanel("0", "Clients", Color.FromArgb(139, 92, 246), "🏢");

        //    // Row 1: KPI Cards with professional styling
        //    var liRevenue = layoutControl.AddItem(string.Empty, pnlRevenue);
        //    ConfigureKpiLayoutItem(liRevenue);

        //    var liProjects = layoutControl.AddItem(string.Empty, pnlProjects);
        //    ConfigureKpiLayoutItem(liProjects);

        //    var liEngineers = layoutControl.AddItem(string.Empty, pnlEngineers);
        //    ConfigureKpiLayoutItem(liEngineers);

        //    var liClients = layoutControl.AddItem(string.Empty, pnlClients);
        //    ConfigureKpiLayoutItem(liClients);

        //    // Row 2: Left - Projects by Status Summary | Right - Top Projects
        //    var pnlStatusWrapper = CreateCardPanel("Projects by Status", gcProjectsByStatus);
        //    var liStatusSummary = layoutControl.AddItem(string.Empty, pnlStatusWrapper);
        //    ConfigureCardLayoutItem(liStatusSummary);

        //    var pnlTopProjectsWrapper = CreateCardPanel("Top Revenue Projects", gcTopProjects);
        //    var liTopProjects = layoutControl.AddItem(string.Empty, pnlTopProjectsWrapper);
        //    ConfigureCardLayoutItem(liTopProjects);

        //    // Row 3: Recent Activities Grid (full width)
        //    var pnlActivitiesWrapper = CreateCardPanel("Recent Activities", gcActivities);
        //    var liActivities = layoutControl.AddItem(string.Empty, pnlActivitiesWrapper);
        //    ConfigureCardLayoutItem(liActivities);

        //    // Configure table layout for bento style
        //    layoutControl.Root.BeginUpdate();
        //    layoutControl.Root.LayoutMode = DevExpress.XtraLayout.Utils.LayoutMode.Table;

        //    // Define 4 equal columns
        //    layoutControl.Root.OptionsTableLayoutGroup.ColumnDefinitions.Clear();
        //    for (int i = 0; i < 4; i++)
        //    {
        //        layoutControl.Root.OptionsTableLayoutGroup.ColumnDefinitions.Add(
        //            new ColumnDefinition { SizeType = SizeType.Percent, Width = 25 });
        //    }

        //    // Define 3 rows with optimized heights
        //    layoutControl.Root.OptionsTableLayoutGroup.RowDefinitions.Clear();
        //    layoutControl.Root.OptionsTableLayoutGroup.RowDefinitions.Add(
        //        new RowDefinition { SizeType = SizeType.Absolute, Height = 130 });
        //    layoutControl.Root.OptionsTableLayoutGroup.RowDefinitions.Add(
        //        new RowDefinition { SizeType = SizeType.Percent, Height = 40 });
        //    layoutControl.Root.OptionsTableLayoutGroup.RowDefinitions.Add(
        //        new RowDefinition { SizeType = SizeType.Percent, Height = 60 });

        //    // Position KPI cards in row 0 (4 columns)
        //    liRevenue.OptionsTableLayoutItem.RowIndex = 0;
        //    liRevenue.OptionsTableLayoutItem.ColumnIndex = 0;

        //    liProjects.OptionsTableLayoutItem.RowIndex = 0;
        //    liProjects.OptionsTableLayoutItem.ColumnIndex = 1;

        //    liEngineers.OptionsTableLayoutItem.RowIndex = 0;
        //    liEngineers.OptionsTableLayoutItem.ColumnIndex = 2;

        //    liClients.OptionsTableLayoutItem.RowIndex = 0;
        //    liClients.OptionsTableLayoutItem.ColumnIndex = 3;

        //    // Position status summary in row 1, left 2 columns
        //    liStatusSummary.OptionsTableLayoutItem.RowIndex = 1;
        //    liStatusSummary.OptionsTableLayoutItem.ColumnIndex = 0;
        //    liStatusSummary.OptionsTableLayoutItem.ColumnSpan = 2;

        //    // Position top projects in row 1, right 2 columns
        //    liTopProjects.OptionsTableLayoutItem.RowIndex = 1;
        //    liTopProjects.OptionsTableLayoutItem.ColumnIndex = 2;
        //    liTopProjects.OptionsTableLayoutItem.ColumnSpan = 2;

        //    // Position activities grid in row 2, full width
        //    liActivities.OptionsTableLayoutItem.RowIndex = 2;
        //    liActivities.OptionsTableLayoutItem.ColumnIndex = 0;
        //    liActivities.OptionsTableLayoutItem.ColumnSpan = 4;

        //    layoutControl.Root.EndUpdate();
        //}

        //private void ConfigureKpiLayoutItem(LayoutControlItem item)
        //{
        //    item.TextVisible = false;
        //    item.SizeConstraintsType = SizeConstraintsType.Custom;
        //    item.MinSize = new Size(180, 120);
        //    item.Padding = new DevExpress.XtraLayout.Utils.Padding(5);
        //}

        //private void ConfigureCardLayoutItem(LayoutControlItem item)
        //{
        //    item.TextVisible = false;
        //    item.SizeConstraintsType = SizeConstraintsType.Custom;
        //    item.MinSize = new Size(300, 150);
        //    item.Padding = new DevExpress.XtraLayout.Utils.Padding(5);
        //}

        //private PanelControl CreateKpiPanel(string value, string title, Color accentColor, string icon)
        //{
        //    var panel = new PanelControl();
        //    panel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        //    panel.BackColor = Color.White;
        //    panel.Size = new Size(220, 120);
        //    panel.Padding = new Padding(0);

        //    // Create gradient background panel
        //    var gradientPanel = new PanelControl();
        //    gradientPanel.Dock = DockStyle.Fill;
        //    gradientPanel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        //    gradientPanel.BackColor = Color.White;
        //    gradientPanel.Padding = new Padding(20, 15, 20, 15);
        //    panel.Controls.Add(gradientPanel);

        //    // Icon label with background
        //    var lblIcon = new LabelControl();
        //    lblIcon.Text = icon;
        //    lblIcon.Font = new Font("Segoe UI", 24, FontStyle.Regular);
        //    lblIcon.AutoSizeMode = LabelAutoSizeMode.None;
        //    lblIcon.Size = new Size(50, 50);
        //    lblIcon.Location = new Point(20, 15);
        //    lblIcon.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        //    lblIcon.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
        //    gradientPanel.Controls.Add(lblIcon);

        //    // Value label
        //    var lblValue = new LabelControl();
        //    lblValue.Name = "lblValue";
        //    lblValue.Text = value;
        //    lblValue.Font = new Font("Segoe UI", 20, FontStyle.Bold);
        //    lblValue.ForeColor = Color.FromArgb(31, 41, 55);
        //    lblValue.AutoSizeMode = LabelAutoSizeMode.None;
        //    lblValue.Location = new Point(80, 15);
        //    lblValue.Size = new Size(120, 35);
        //    gradientPanel.Controls.Add(lblValue);

        //    // Title label
        //    var lblTitle = new LabelControl();
        //    lblTitle.Text = title;
        //    lblTitle.Font = new Font("Segoe UI", 9, FontStyle.Regular);
        //    lblTitle.AutoSizeMode = LabelAutoSizeMode.None;
        //    lblTitle.Location = new Point(80, 50);
        //    lblTitle.Size = new Size(120, 20);
        //    lblTitle.Appearance.ForeColor = Color.FromArgb(107, 114, 128);
        //    gradientPanel.Controls.Add(lblTitle);

        //    // Trend indicator (small colored bar at bottom)
        //    var trendBar = new PanelControl();
        //    trendBar.Dock = DockStyle.Bottom;
        //    trendBar.Height = 4;
        //    trendBar.BackColor = accentColor;
        //    trendBar.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        //    panel.Controls.Add(trendBar);

        //    return panel;
        //}

        //private PanelControl CreateCardPanel(string title, Control content)
        //{
        //    var panel = new PanelControl();
        //    panel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        //    panel.BackColor = Color.White;
        //    panel.Padding = new Padding(0);

        //    // Header panel
        //    var headerPanel = new PanelControl();
        //    headerPanel.Dock = DockStyle.Top;
        //    headerPanel.Height = 50;
        //    headerPanel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        //    headerPanel.BackColor = Color.White;
        //    headerPanel.Padding = new Padding(20, 15, 20, 10);
        //    panel.Controls.Add(headerPanel);

        //    // Title label
        //    var lblTitle = new LabelControl();
        //    lblTitle.Text = title;
        //    lblTitle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
        //    lblTitle.ForeColor = Color.FromArgb(31, 41, 55);
        //    lblTitle.Dock = DockStyle.Fill;
        //    headerPanel.Controls.Add(lblTitle);

        //    // Content panel
        //    var contentPanel = new PanelControl();
        //    contentPanel.Dock = DockStyle.Fill;
        //    contentPanel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        //    contentPanel.BackColor = Color.White;
        //    contentPanel.Padding = new Padding(15, 5, 15, 15);
        //    panel.Controls.Add(contentPanel);

        //    content.Dock = DockStyle.Fill;
        //    contentPanel.Controls.Add(content);

        //    return panel;
        //}

        //private void LoadSampleData()
        //{
        //    var activities = GetSampleActivities();

        //    // Bind activities grid with custom columns
        //    gcActivities.DataSource = activities;
        //    ConfigureActivitiesGridColumns();

        //    // Create summary by status
        //    var statusSummary = activities
        //        .GroupBy(a => a.Status)
        //        .Select(g => new
        //        {
        //            Status = g.Key,
        //            Projects = g.Sum(x => x.ProjectCount),
        //            Revenue = g.Sum(x => x.Revenue),
        //            Engineers = g.Sum(x => x.EngineersCount),
        //            Activities = g.Sum(x => x.ActivityCount)
        //        })
        //        .OrderByDescending(x => x.Revenue)
        //        .ToList();

        //    gcProjectsByStatus.DataSource = statusSummary;
        //    ConfigureStatusGridColumns();

        //    // Top 5 projects by revenue
        //    var topProjects = activities
        //        .OrderByDescending(a => a.Revenue)
        //        .Take(5)
        //        .Select(a => new
        //        {
        //            Project = a.ActivityName,
        //            Status = a.Status,
        //            Revenue = a.Revenue,
        //            Engineers = a.EngineersCount
        //        })
        //        .ToList();

        //    gcTopProjects.DataSource = topProjects;
        //    ConfigureTopProjectsGridColumns();

        //    // Update KPI values
        //    UpdateKpiValues(activities);
        //}

        //private void ConfigureActivitiesGridColumns()
        //{
        //    gvActivities.Columns.Clear();

        //    var colName = gvActivities.Columns.AddField("ActivityName");
        //    colName.Caption = "Project Name";
        //    colName.Visible = true;
        //    colName.Width = 180;

        //    var colStatus = gvActivities.Columns.AddField("Status");
        //    colStatus.Caption = "Status";
        //    colStatus.Visible = true;
        //    colStatus.Width = 100;

        //    var colMonth = gvActivities.Columns.AddField("Month");
        //    colMonth.Caption = "Month";
        //    colMonth.Visible = true;
        //    colMonth.Width = 100;

        //    var colRevenue = gvActivities.Columns.AddField("Revenue");
        //    colRevenue.Caption = "Revenue";
        //    colRevenue.Visible = true;
        //    colRevenue.Width = 120;
        //    colRevenue.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        //    colRevenue.DisplayFormat.FormatString = "c0";

        //    var colProjects = gvActivities.Columns.AddField("ProjectCount");
        //    colProjects.Caption = "Projects";
        //    colProjects.Visible = true;
        //    colProjects.Width = 80;

        //    var colEngineers = gvActivities.Columns.AddField("EngineersCount");
        //    colEngineers.Caption = "Engineers";
        //    colEngineers.Visible = true;
        //    colEngineers.Width = 90;

        //    var colActivities = gvActivities.Columns.AddField("ActivityCount");
        //    colActivities.Caption = "Activities";
        //    colActivities.Visible = true;
        //    colActivities.Width = 90;
        //}

        //private void ConfigureStatusGridColumns()
        //{
        //    gvProjectsByStatus.Columns.Clear();

        //    var colStatus = gvProjectsByStatus.Columns.AddField("Status");
        //    colStatus.Caption = "Status";
        //    colStatus.Visible = true;
        //    colStatus.Width = 120;

        //    var colProjects = gvProjectsByStatus.Columns.AddField("Projects");
        //    colProjects.Caption = "Projects";
        //    colProjects.Visible = true;
        //    colProjects.Width = 100;

        //    var colRevenue = gvProjectsByStatus.Columns.AddField("Revenue");
        //    colRevenue.Caption = "Revenue";
        //    colRevenue.Visible = true;
        //    colRevenue.Width = 120;
        //    colRevenue.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        //    colRevenue.DisplayFormat.FormatString = "c0";

        //    var colEngineers = gvProjectsByStatus.Columns.AddField("Engineers");
        //    colEngineers.Caption = "Engineers";
        //    colEngineers.Visible = true;
        //    colEngineers.Width = 100;

        //    var colActivities = gvProjectsByStatus.Columns.AddField("Activities");
        //    colActivities.Caption = "Activities";
        //    colActivities.Visible = true;
        //    colActivities.Width = 100;
        //}

        //private void ConfigureTopProjectsGridColumns()
        //{
        //    gvTopProjects.Columns.Clear();

        //    var colProject = gvTopProjects.Columns.AddField("Project");
        //    colProject.Caption = "Project";
        //    colProject.Visible = true;
        //    colProject.Width = 200;

        //    var colStatus = gvTopProjects.Columns.AddField("Status");
        //    colStatus.Caption = "Status";
        //    colStatus.Visible = true;
        //    colStatus.Width = 100;

        //    var colRevenue = gvTopProjects.Columns.AddField("Revenue");
        //    colRevenue.Caption = "Revenue";
        //    colRevenue.Visible = true;
        //    colRevenue.Width = 120;
        //    colRevenue.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        //    colRevenue.DisplayFormat.FormatString = "c0";

        //    var colEngineers = gvTopProjects.Columns.AddField("Engineers");
        //    colEngineers.Caption = "Engineers";
        //    colEngineers.Visible = true;
        //    colEngineers.Width = 100;
        //}

        //private void UpdateKpiValues(List<ActivityDataModel> activities)
        //{
        //    // Calculate totals
        //    var totalRevenue = activities.Sum(a => a.Revenue);
        //    var totalProjects = activities.Sum(a => a.ProjectCount);
        //    var totalEngineers = activities.Sum(a => a.EngineersCount);
        //    var uniqueClients = activities.Select(a => a.Status).Distinct().Count() * 4;

        //    // Update KPI panels
        //    UpdateKpiPanel(pnlRevenue, totalRevenue.ToString("C0"));
        //    UpdateKpiPanel(pnlProjects, totalProjects.ToString());
        //    UpdateKpiPanel(pnlEngineers, totalEngineers.ToString());
        //    UpdateKpiPanel(pnlClients, uniqueClients.ToString());
        //}

        //private void UpdateKpiPanel(PanelControl panel, string value)
        //{
        //    var gradientPanel = panel.Controls[0] as PanelControl;
        //    if (gradientPanel != null)
        //    {
        //        foreach (Control ctrl in gradientPanel.Controls)
        //        {
        //            if (ctrl is LabelControl lbl && lbl.Name == "lblValue")
        //            {
        //                lbl.Text = value;
        //                break;
        //            }
        //        }
        //    }
        //}

        //private List<ActivityDataModel> GetSampleActivities()
        //{
        //    return new List<ActivityDataModel>
        //    {
        //        new ActivityDataModel { ActivityName = "Project Alpha", Status = "Active", Month = "January", Revenue = 15000, ProjectCount = 5, EngineersCount = 12, ActivityCount = 45 },
        //        new ActivityDataModel { ActivityName = "Project Beta", Status = "Active", Month = "February", Revenue = 22000, ProjectCount = 8, EngineersCount = 15, ActivityCount = 62 },
        //        new ActivityDataModel { ActivityName = "Project Gamma", Status = "Completed", Month = "March", Revenue = 18500, ProjectCount = 6, EngineersCount = 10, ActivityCount = 38 },
        //        new ActivityDataModel { ActivityName = "Project Delta", Status = "Active", Month = "April", Revenue = 31000, ProjectCount = 12, EngineersCount = 18, ActivityCount = 75 },
        //        new ActivityDataModel { ActivityName = "Project Epsilon", Status = "On Hold", Month = "May", Revenue = 9500, ProjectCount = 3, EngineersCount = 8, ActivityCount = 22 },
        //        new ActivityDataModel { ActivityName = "Project Zeta", Status = "Active", Month = "June", Revenue = 27500, ProjectCount = 10, EngineersCount = 14, ActivityCount = 58 },
        //        new ActivityDataModel { ActivityName = "Project Eta", Status = "Completed", Month = "July", Revenue = 21000, ProjectCount = 7, EngineersCount = 11, ActivityCount = 41 },
        //        new ActivityDataModel { ActivityName = "Project Theta", Status = "Active", Month = "August", Revenue = 35000, ProjectCount = 15, EngineersCount = 20, ActivityCount = 82 },
        //        new ActivityDataModel { ActivityName = "Project Iota", Status = "On Hold", Month = "September", Revenue = 12000, ProjectCount = 4, EngineersCount = 9, ActivityCount = 28 },
        //        new ActivityDataModel { ActivityName = "Project Kappa", Status = "Active", Month = "October", Revenue = 29000, ProjectCount = 11, EngineersCount = 16, ActivityCount = 67 },
        //        new ActivityDataModel { ActivityName = "Project Lambda", Status = "Completed", Month = "November", Revenue = 24500, ProjectCount = 9, EngineersCount = 13, ActivityCount = 52 },
        //        new ActivityDataModel { ActivityName = "Project Mu", Status = "Active", Month = "December", Revenue = 38000, ProjectCount = 14, EngineersCount = 22, ActivityCount = 91 }
        //    };
        //}

        //private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    Close();
        //}

        //private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    LoadSampleData();
        //}






    }
}