using DevExpress.XtraEditors;
using Spectrum.DataLayers.DataAccess;
using Spectrum.DataLayers.Projects;
using Spectrum.Models.Projects;
using Spectrum.Reports.Adapters.Projects;
using Spectrum.Reports.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Spectrum.Views.Projects.Reports
{
    /// <summary>
    /// Thin wrapper that hosts a <see cref="ReportDesignerControl"/>
    /// bound to a <see cref="ProjectModel"/> via <see cref="ProjectReportAdapter"/>.
    /// </summary>
    public partial class ProjectReportDesignerForm : XtraForm
    {
        private readonly ProjectModel _project;
        private readonly IList<ProjectModel> _availableProjects;
        private readonly IList<ProjectModel> _selectedProjects;
        private readonly ProjectRepository _projectRepository;
        private readonly ReportDesignerControl _designer;

        public ProjectReportDesignerForm(ProjectModel project)
            : this(project, new List<ProjectModel> { project }, new List<ProjectModel> { project })
        {
        }

        public ProjectReportDesignerForm(ProjectModel activeProject, IList<ProjectModel> availableProjects, IList<ProjectModel> selectedProjects)
        {
            if (activeProject == null) throw new ArgumentNullException(nameof(activeProject));

            _project = activeProject;
            _availableProjects = (availableProjects ?? new List<ProjectModel> { activeProject })
                .Where(x => x != null)
                .ToList();
            _selectedProjects = (selectedProjects ?? new List<ProjectModel> { activeProject })
                .Where(x => x != null)
                .ToList();

            if (_availableProjects.Count == 0)
                _availableProjects.Add(activeProject);
            if (_selectedProjects.Count == 0)
                _selectedProjects.Add(activeProject);

            _projectRepository = new ProjectRepository(DatabaseFactory.ProfilePrimary);

            var adapter = new ProjectReportAdapter(_availableProjects, _selectedProjects, _project);

            Text = adapter.Title;
            Width = 1400;
            Height = 900;
            StartPosition = FormStartPosition.CenterParent;

            _designer = new ReportDesignerControl();
            _designer.Dock = DockStyle.Fill;
            _designer.ModelChanged += Designer_ModelChanged;
            Controls.Add(_designer);

            _designer.LoadAdapter(adapter);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!e.Cancel && _designer != null && _designer.HasPendingDataChanges)
            {
                var result = XtraMessageBox.Show(
                    "Invoice or addendum changes have not been saved. Save them to MongoDB before closing?",
                    "Project Report Designer",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }

                if (result == DialogResult.Yes)
                    _designer.SaveDataChanges();
            }

            base.OnFormClosing(e);
        }

        private async void Designer_ModelChanged(object sender, EventArgs e)
        {
            try
            {
                await _projectRepository.UpdateProjectAsync(_project);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Project Report Designer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_designer != null)
                    _designer.ModelChanged -= Designer_ModelChanged;
                _designer?.Dispose();
                _projectRepository?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}