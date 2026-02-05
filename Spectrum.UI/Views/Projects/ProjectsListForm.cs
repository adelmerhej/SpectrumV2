using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using Spectrum.DataLayers.DataAccess;
using Spectrum.DataLayers.Projects;
using Spectrum.Models.Projects;
using Spectrum.Models.Users;
using Spectrum.Utilities;
using Spectrum.Utilities.Interfaces;
using Spectrum.Utilities.Layout;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.Projects
{
	public partial class ProjectsListForm : RibbonForm, IFormWithRibbon
	{
		private bool _resetMenu;
		private ProjectEditForm _projectEditForm;

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
			ShowProjectEditor(new ProjectModel());
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

				ShowProjectEditor(_projectModel);
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
						if (_projectModel == null)
						{
							return;
						}
						_projectModel.Deleted = true;

						//delete the record
						await _projectRepository.DeleteProjectAsync(_projectModel._id);
						RcvUpdatedProjectAsync(_projectModel, EventArgs.Empty);
					}
				}
			}
			catch (Exception exception)
			{
				switch (exception.Message)
				{
					case "-2146233088":
						XtraMessageBox.Show("This record is linked to one or more transactions, delete all links first.",
							"Delete error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						break;

					default:
						XtraMessageBox.Show(exception.Message,
							"Delete error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						break;
				}
			}
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
			if (!_projects.Any()) return;

			try
			{
				string currentRowId = gvProjects.GetFocusedRowCellValue("_id").ToString();
				if (string.IsNullOrEmpty(currentRowId)) return;

				_projectModel = _projects.SingleOrDefault(x => x._id == currentRowId);
				if (_projectModel == null) return;

				ShowProjectEditor(_projectModel);
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

		private void ShowProjectEditor(ProjectModel model)
		{
			if (_projectEditForm == null || _projectEditForm.IsDisposed)
			{
				_projectEditForm = new ProjectEditForm(model);
				_projectEditForm.SendUpdatedProject += RcvUpdatedProjectAsync;
				_projectEditForm.FormClosed += ProjectEditForm_FormClosed;
				_projectEditForm.Show(this);
				return;
			}

			if (_projectEditForm.WindowState == FormWindowState.Minimized)
				_projectEditForm.WindowState = FormWindowState.Normal;

			_projectEditForm.Activate();
			_projectEditForm.BringToFront();
		}

		private void ProjectEditForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			var form = sender as ProjectEditForm;
			if (form != null)
			{
				form.SendUpdatedProject -= RcvUpdatedProjectAsync;
				form.FormClosed -= ProjectEditForm_FormClosed;
			}
			if (ReferenceEquals(_projectEditForm, sender))
				_projectEditForm = null;
		}

		private void gvProjects_RowCellStyle(object sender, RowCellStyleEventArgs e)
		{
			var view = sender as GridView;
			if (view == null || e.RowHandle < 0) return;
			bool isActive = HelperApplication.ConvertToBool(view.GetRowCellValue(e.RowHandle, "Active")) ?? false;
			bool isDefault = HelperApplication.ConvertToBool(view.GetRowCellValue(e.RowHandle, "IsDefault")) ?? false;
			if (!isActive)
			{
				e.Appearance.ForeColor = Color.Gray;
				e.Appearance.Font = new Font("Tahoma", 8, FontStyle.Italic);
			}
			if (isDefault)
			{
				e.Appearance.Font = new Font("Tahoma", 8, FontStyle.Bold);
			}
		}
	}
}
