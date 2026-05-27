using DevExpress.XtraEditors;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Administration.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Spectrum.Views.Main
{
    public partial class ApiKeySettingForm : XtraForm
    {
        private ConnectionModel _connectionModel = new ConnectionModel();

        private const string ProviderOpenAi = "OpenAI";
        private const string ProviderDeepSeek = "DeepSeek";

        private static readonly IReadOnlyDictionary<string, string[]> ProviderModels =
            new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
            {
                {
                    ProviderOpenAi,
                    new[]
                    {
                        "gpt-5.4-nano",
                        "gpt-5.4-mini",
                        "gpt-5.4",
                        "gpt-5-nano",
                        "gpt-5-mini",
                        "gpt-5",
                        "gpt-4.1-nano",
                        "gpt-4.1-mini",
                        "gpt-4.1",
                        "gpt-4o-mini",
                        "gpt-4o",
                        "o4-mini",
                        "o3",
                        "o3-mini"
                    }
                },
                { ProviderDeepSeek, new[] { "deepseek-chat", "deepseek-reasoner" } }
            };

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
            _connectionModel = DatabaseFactory.GetConnection(DatabaseFactory.ProfilePrimary) ?? new ConnectionModel();
            if (_connectionModel.AiSettings == null)
                _connectionModel.AiSettings = new AiSettingsModel();
        }

        private void InitializeBindings()
        {
            cboProvider.Properties.Items.Clear();
            cboProvider.Properties.Items.Add(ProviderOpenAi);
            cboProvider.Properties.Items.Add(ProviderDeepSeek);
        }

        private void WireUpBindings()
        {
            var provider = ResolveProvider();
            cboProvider.EditValue = provider;
            LoadModelOptions(provider);
            txtAPIKey.Text = GetProviderApiKey(provider) ?? string.Empty;

            txtProjectsFolder.Text = _connectionModel.ProjectsDocumentsFolder ?? string.Empty;
            txtEmployeesFolder.Text = _connectionModel.EmployeesDocumentsFolder ?? string.Empty;
            txtEngineersFolder.Text = _connectionModel.EngineersDocumentsFolder ?? string.Empty;
        }

        private string ResolveProvider()
        {
            var provider = _connectionModel.AiSettings?.Provider;
            if (string.IsNullOrWhiteSpace(provider))
                provider = ProviderOpenAi;
            return provider;
        }

        private void LoadModelOptions(string provider)
        {
            var models = ProviderModels.ContainsKey(provider)
                ? ProviderModels[provider]
                : ProviderModels[ProviderOpenAi];

            cboModel.Properties.Items.Clear();
            cboModel.Properties.Items.AddRange(models.Cast<object>().ToArray());

            var selectedModel = GetProviderModel(provider);
            if (string.IsNullOrWhiteSpace(selectedModel) || !models.Contains(selectedModel))
                selectedModel = models[0];

            cboModel.EditValue = selectedModel;
        }

        private string GetProviderModel(string provider)
        {
            if (string.Equals(provider, ProviderDeepSeek, StringComparison.OrdinalIgnoreCase))
                return _connectionModel.AiSettings?.DeepSeekModel;
            return _connectionModel.AiSettings?.OpenAiModel;
        }

        private string GetProviderApiKey(string provider)
        {
            if (string.Equals(provider, ProviderDeepSeek, StringComparison.OrdinalIgnoreCase))
                return _connectionModel.AiSettings?.EncryptedDeepSeekApiKey;
            return _connectionModel.AiSettings?.EncryptedOpenAiApiKey;
        }

        private void SetProviderValues(string provider, string model, string apiKey)
        {
            if (_connectionModel.AiSettings == null)
                _connectionModel.AiSettings = new AiSettingsModel();

            if (string.Equals(provider, ProviderDeepSeek, StringComparison.OrdinalIgnoreCase))
            {
                _connectionModel.AiSettings.DeepSeekModel = model;
                _connectionModel.AiSettings.EncryptedDeepSeekApiKey = apiKey;
            }
            else
            {
                _connectionModel.AiSettings.OpenAiModel = model;
                _connectionModel.AiSettings.EncryptedOpenAiApiKey = apiKey;
            }
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

        private void cboProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            var provider = (cboProvider.EditValue ?? ProviderOpenAi).ToString();
            LoadModelOptions(provider);
            txtAPIKey.Text = GetProviderApiKey(provider) ?? string.Empty;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var provider = (cboProvider.EditValue ?? ProviderOpenAi).ToString();
                var selectedModel = (cboModel.EditValue ?? string.Empty).ToString().Trim();
                var apiKey = txtAPIKey.Text;

                _connectionModel.AiSettings.Provider = provider;
                SetProviderValues(provider, selectedModel, apiKey);

                // keep legacy fields in sync for existing callers
                _connectionModel.AiModel = selectedModel;
                _connectionModel.EncryptedAiApikey = apiKey;

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