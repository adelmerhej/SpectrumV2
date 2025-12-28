using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using SpectrumV1.DataLayers.DataAccess;
using SpectrumV1.DataLayers.Projects;
using SpectrumV1.Models.Projects;
using SpectrumV1.Models.Users;
using SpectrumV1.Utilities.Interfaces;
using SpectrumV1.Utilities.Layout;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpectrumV1.Views.Projects
{
	public partial class ProjectsListForm : RibbonForm, IFormWithRibbon
	{
		private bool _resetMenu;
		private ProjectModel _projectModel = new ProjectModel();
		private IList<ProjectModel> _projects = new List<ProjectModel>();

		private readonly ProjectRepository _projectRepository = new ProjectRepository(DatabaseFactory.ProfilePrimary);

		private bool _canAdd = true;
		private bool _canEdit = true;
		private bool _canDelete = true;
		private bool _canPrint = true;
		private bool _isAdmin = true;
		private bool _isProtected = true;

		public RibbonControl MainRibbon => rcProjectsList;
		public RibbonPage DefaultPage => rpProjectsList;

		public ProjectsListForm()
		{
			InitializeComponent();

			// wire events
			btnNew.ItemClick += btnNew_ItemClick;
			btnEdit.ItemClick += btnEdit_ItemClick;
			btnDelete.ItemClick += btnDelete_ItemClick;
			btnPrint.ItemClick += btnPrint_ItemClick;
			btnRefresh.ItemClick += btnRefresh_ItemClick;
			btnClose.ItemClick += btnClose_ItemClick;
			btnResetGridStyle.ItemClick += btnResetGridStyle_ItemClick;
			gvProjects.DoubleClick += gvProjects_DoubleClick;
			gvProjects.RowCellStyle += gvProjects_RowCellStyle;

			StartLoading();
		}

		private async void StartLoading()
		{
			await InitializeBindings();
			WireUpBindings();
			ApplyDefaults();
			ApplyPermissions();
		}

		private async Task InitializeBindings()
		{
			try
			{
				_projects = await _projectRepository.GetProjectsAsync();
			}
			catch (Exception ex)
			{
				XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void WireUpBindings()
		{
			gcProjects.DataSource = null;
			gcProjects.DataSource = _projects;
		}

		private void ApplyDefaults() { }

		private void ApplyPermissions()
		{
			btnNew.Enabled = _isAdmin || _canAdd;
			btnEdit.Enabled = _isAdmin || _canEdit;
			btnPrint.Enabled = _isAdmin || _canPrint;
			btnDelete.Enabled = _isAdmin || _canDelete;
		}

		private void btnNew_ItemClick(object sender, ItemClickEventArgs e)
		{

		}

		private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
		{

		}

		private void btnRefresh_ItemClick(object sender, ItemClickEventArgs e)
		{
			StartLoading();
		}

		private void btnPrint_ItemClick(object sender, ItemClickEventArgs e)
		{
			gcProjects.ShowRibbonPrintPreview();
		}

		private async void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
		{

		}

		private void btnClose_ItemClick(object sender, ItemClickEventArgs e)
		{
			Close();
		}

		private void btnResetGridStyle_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (XtraMessageBox.Show("This will reset Grid layout next login, to its default settings.\nAre you sure you want to continue?", "Reset Menu...",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) ==
				DialogResult.Yes)
			{
				_resetMenu = true;
				LayoutsStyle.ResetLayoutGrid(gvProjects, CurrentUser.UserName, CurrentUser.Company);
			}
		}

		private async void RcvUpdatedProjectAsync(object sender, EventArgs e)
		{
			if (sender == null) return;
			_projectModel = sender as ProjectModel;
			if (_projectModel != null && (_projectModel.LastModifiedDate == null || _projectModel.Deleted))
			{
				await InitializeBindings();
				WireUpBindings();
			}
			else
			{
				gvProjects.UpdateCurrentRow();
			}
		}

		private void gvProjects_DoubleClick(object sender, EventArgs e)
		{

		}

		private bool CanDelete()
		{
			ProjectModel dataBoundItem = gvProjects.GetFocusedRow() as ProjectModel;
			if (gvProjects == null || gvProjects.SelectedRowsCount == 0) return false;
			if (gvProjects.SelectedRowsCount > 1)
			{
				XtraMessageBox.Show("Only one record can be selected at a time, please try again",
					"Delete error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			if (dataBoundItem != null && dataBoundItem.IsDefault)
			{
				XtraMessageBox.Show("Cannot delete system record!",
					"Delete error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			return true;
		}

		private void gvProjects_RowCellStyle(object sender, RowCellStyleEventArgs e)
		{

		}
	}
}
