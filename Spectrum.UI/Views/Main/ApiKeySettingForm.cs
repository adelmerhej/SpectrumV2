using DevExpress.XtraEditors;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Administration.Connections;
using System;
using System.Windows.Forms;

namespace Spectrum.Views.Main
{
    public partial class ApiKeySettingForm : XtraForm
    {
        private ConnectionModel _connectionModel = new ConnectionModel();
        private const string _AiModel = "gpt-4.1-mini";

        public ApiKeySettingForm()
        {
            InitializeComponent();

            ConnectionParams();
            InitializeBindings();
            WireUpBindings();
            ApplyPermissions();
        }

        private void ConnectionParams()
        {
            _connectionModel = DatabaseFactory.GetConnection(DatabaseFactory.ProfilePrimary);
        }

        private void InitializeBindings()
        {
        }

        private void WireUpBindings()
        {
            txtAIModel.Text = _connectionModel.AiModel ?? _AiModel;
            txtAPIKey.Text = _connectionModel.EncryptedAiApikey ?? string.Empty;
            txtProjectsFolder.Text = _connectionModel.ProjectsDocumentsFolder ?? string.Empty;
            txtEmployeesFolder.Text = _connectionModel.EmployeesDocumentsFolder ?? string.Empty;
            txtEngineersFolder.Text = _connectionModel.EngineersDocumentsFolder ?? string.Empty;
        }

        private void ApplyPermissions()
        {
        }

        private void btnBrowseProjects_Click(object sender, EventArgs e)
        {
            BrowseFolder("Select Projects Documents Folder", txtProjectsFolder);
        }

        private void btnBrowseEmployees_Click(object sender, EventArgs e)
        {
            BrowseFolder("Select Employees Documents Folder", txtEmployeesFolder);
        }

        private void btnBrowseEngineers_Click(object sender, EventArgs e)
        {
            BrowseFolder("Select Engineers Documents Folder", txtEngineersFolder);
        }

        private void BrowseFolder(string description, ButtonEdit target)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = description;
                fbd.ShowNewFolderButton = true;
                if (!string.IsNullOrWhiteSpace(target.Text))
                    fbd.SelectedPath = target.Text;

                if (fbd.ShowDialog(this) == DialogResult.OK)
                    target.Text = fbd.SelectedPath;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                _connectionModel.AiModel = txtAIModel.Text.Trim();
                _connectionModel.EncryptedAiApikey = txtAPIKey.Text;
                _connectionModel.ProjectsDocumentsFolder = txtProjectsFolder.Text.Trim();
                _connectionModel.EmployeesDocumentsFolder = txtEmployeesFolder.Text.Trim();
                _connectionModel.EngineersDocumentsFolder = txtEngineersFolder.Text.Trim();

                DatabaseFactory.SaveConnection(_connectionModel, DatabaseFactory.ProfilePrimary);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception)
            {
                DialogResult = DialogResult.Cancel;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}