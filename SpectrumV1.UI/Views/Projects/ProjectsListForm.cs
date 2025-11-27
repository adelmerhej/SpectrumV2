using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using SpectrumV1.DataLayers.Projects;
using SpectrumV1.Models.Projects;
using SpectrumV1.Utilities.Interfaces;
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
		private ProjectModel _projectModel = new ProjectModel();
		private IList<ProjectModel> _projects = new List<ProjectModel>();

		private readonly ProjectRepository _projectRepository = new ProjectRepository();

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
			try
			{
				ProjectEditForm frm = new ProjectEditForm(new ProjectModel());
				frm.SendUpdatedProject += RcvUpdatedProjectAsync;
				frm.ShowDialog();
			}
			catch (Exception ex)
			{

				XtraMessageBox.Show(ex.Message);
			}

		}

		private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!_projects.Any()) return;
			try
			{
				string currentRowId = gvProjects.GetFocusedRowCellValue("_id").ToString();
				if (string.IsNullOrEmpty(currentRowId)) return;
				_projectModel = _projects.SingleOrDefault(x => x._id == currentRowId);
				if (_projectModel == null) return;
				var projectForm = new ProjectEditForm(_projectModel);
				projectForm.SendUpdatedProject += RcvUpdatedProjectAsync;
				projectForm.ShowDialog();
			}
			catch (Exception exception)
			{
				XtraMessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
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
			if (!CanDelete()) return;
			try
			{
				string id = gvProjects.GetFocusedRowCellValue("_id").ToString();
				string name = gvProjects.GetFocusedRowCellValue("ProjectName").ToString();
				if (!string.IsNullOrEmpty(id))
				{
					if (XtraMessageBox.Show($"Are you sure you want to delete Record: `{name}`?",
							"Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
							MessageBoxDefaultButton.Button2) == DialogResult.Yes)
					{
						_projectModel = gvProjects.GetFocusedRow() as ProjectModel;
						if (_projectModel == null) return;
						_projectModel.Deleted = true;
						await _projectRepository.DeleteProjectAsync(_projectModel._id);
						RcvUpdatedProjectAsync(_projectModel, EventArgs.Empty);
					}
				}
			}
			catch (Exception exception)
			{
				XtraMessageBox.Show(exception.Message, "Delete error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btnClose_ItemClick(object sender, ItemClickEventArgs e)
		{
			Close();
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
			if (!_projects.Any()) return;
			try
			{
				string currentRowId = gvProjects.GetFocusedRowCellValue("_id").ToString();
				if (string.IsNullOrEmpty(currentRowId)) return;
				_projectModel = _projects.SingleOrDefault(x => x._id == currentRowId);
				if (_projectModel == null) return;
				var projectForm = new ProjectEditForm(_projectModel);
				projectForm.SendUpdatedProject += RcvUpdatedProjectAsync;
				projectForm.ShowDialog();
			}
			catch (Exception exception)
			{
				XtraMessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
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
			GridView view = sender as GridView;
			if (e.RowHandle >= 0)
			{
				bool isActive = (bool)view.GetRowCellValue(e.RowHandle, "Active");
				bool isDefault = (bool)view.GetRowCellValue(e.RowHandle, "IsDefault");
				if (isDefault)
				{
					e.Appearance.Font = new Font("Tahoma", 8, FontStyle.Bold);
				}
				if (!isActive)
				{
					e.Appearance.ForeColor = Color.Gray;
					e.Appearance.Font = new Font("Tahoma", 8, FontStyle.Italic);
				}
			}
		}

		private void btnResetGridStyle_ItemClick(object sender, ItemClickEventArgs e)
		{

		}
	}
}
