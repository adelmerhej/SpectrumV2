using DevExpress.XtraEditors;
using Spectrum.DataLayers.Accounting.Charts;
using Spectrum.DataLayers.Accounting.JournalType;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Accounting.Charts;
using Spectrum.Models.Accounting.JournalType;
using Spectrum.Models.Administration.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.Main.MainSettings
{
    public partial class ApplicationSettingForm : XtraForm
    {
        private ConnectionModel _connectionModel = new ConnectionModel();
        private IList<JournalTypeModel> _journalTypes = new List<JournalTypeModel>();
        private IList<ChartModel> _charts = new List<ChartModel>();

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

        private readonly JournalTypeRepository _journalTypeRepository = new JournalTypeRepository(DatabaseFactory.ProfilePrimary);
        private readonly ChartRepository _chartRepository = new ChartRepository(DatabaseFactory.ProfilePrimary);

        public ApplicationSettingForm()
        {
            InitializeComponent();
            cboProvider.SelectedIndexChanged += cboProvider_SelectedIndexChanged;

            StartLoading();
        }

        private async void StartLoading()
        {
            try
            {
                _connectionModel = DatabaseFactory.GetConnection(DatabaseFactory.ProfilePrimary) ?? new ConnectionModel();
                await InitializeBindings();
                WireUpBindings();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, @"Error Loading settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task InitializeBindings()
        {
            _journalTypes = await _journalTypeRepository.GetJournalTypesAsync();
            _charts = await _chartRepository.GetChartsAsync();

            cboProvider.Properties.Items.Clear();
            cboProvider.Properties.Items.Add(ProviderOpenAi);
            cboProvider.Properties.Items.Add(ProviderDeepSeek);
        }

        private void WireUpBindings()
        {
            cboInvoiceJournalType.Properties.DataSource = null;
            cboInvoiceJournalType.Properties.DataSource = _journalTypes;

            cboExpenseJournalType.Properties.DataSource = null;
            cboExpenseJournalType.Properties.DataSource = _journalTypes;

            cboVatDbCr.Properties.DataSource = null;
            cboVatDbCr.Properties.DataSource = _charts;

            cboVatDbCr1.Properties.DataSource = null;
            cboVatDbCr1.Properties.DataSource = _charts;

            var dbCrItems = new[] { "D", "C" };
            cboCustomerDbCr.Properties.DataSource = dbCrItems;
            cboVendorDbCr.Properties.DataSource = dbCrItems;

            txtInvoiceType.Text = _connectionModel.InvoiceTypeName ?? string.Empty;
            cboInvoiceJournalType.EditValue = _connectionModel.InvoiceJournalTypeCode ?? string.Empty;
            txtCustomerAccount.Text = _connectionModel.CustomerTypeName ?? string.Empty;
            cboCustomerDbCr.Text = _connectionModel.CustomerDbCr ?? string.Empty;
            txtVatAccount.Text = _connectionModel.VatTypeName ?? string.Empty;
            cboVatDbCr.EditValue = _connectionModel.VatAccountNumber ?? string.Empty;

            txtExpenseType.Text = _connectionModel.ExpenseTypeName ?? string.Empty;
            cboExpenseJournalType.EditValue = _connectionModel.ExpenseJournalTypeCode ?? string.Empty;
            txtVendorAccount.Text = _connectionModel.VendorTypeName ?? string.Empty;
            cboVendorDbCr.Text = _connectionModel.VendorDbCr ?? string.Empty;
            txtVatAccount1.Text = _connectionModel.ExpenseVatTypeName ?? string.Empty;
            cboVatDbCr1.EditValue = _connectionModel.ExpenseVatAccountNumber ?? string.Empty;

            var provider = ResolveProvider();
            cboProvider.EditValue = provider;
            LoadModelOptions(provider);
            txtAPIKey.Text = GetProviderApiKey(provider) ?? string.Empty;
            txtProjectsFolder.Text = _connectionModel.ProjectsDocumentsFolder ?? string.Empty;
            txtEmployeesFolder.Text = _connectionModel.EmployeesDocumentsFolder ?? string.Empty;
            txtEngineersFolder.Text = _connectionModel.EngineersDocumentsFolder ?? string.Empty;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var customerDbCr = (cboCustomerDbCr.Text ?? string.Empty).Trim().ToUpperInvariant();
                if (!string.IsNullOrWhiteSpace(customerDbCr) && customerDbCr != "D" && customerDbCr != "C")
                {
                    XtraMessageBox.Show("Customer DbCr must be either 'D' or 'C'.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    cboCustomerDbCr.Focus();
                    return;
                }

                var vendorDbCr = (cboVendorDbCr.Text ?? string.Empty).Trim().ToUpperInvariant();
                if (!string.IsNullOrWhiteSpace(vendorDbCr) && vendorDbCr != "D" && vendorDbCr != "C")
                {
                    XtraMessageBox.Show("Vendor DbCr must be either 'D' or 'C'.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    cboVendorDbCr.Focus();
                    return;
                }

                _connectionModel.InvoiceTypeName = (txtInvoiceType.Text ?? string.Empty).Trim();
                _connectionModel.InvoiceJournalTypeCode = (cboInvoiceJournalType.EditValue ?? string.Empty).ToString().Trim();
                _connectionModel.CustomerTypeName = (txtCustomerAccount.Text ?? string.Empty).Trim();
                _connectionModel.CustomerDbCr = customerDbCr;
                _connectionModel.VatTypeName = (txtVatAccount.Text ?? string.Empty).Trim();
                _connectionModel.VatAccountNumber = (cboVatDbCr.EditValue ?? string.Empty).ToString().Trim();

                _connectionModel.ExpenseTypeName = (txtExpenseType.Text ?? string.Empty).Trim();
                _connectionModel.ExpenseJournalTypeCode = (cboExpenseJournalType.EditValue ?? string.Empty).ToString().Trim();
                _connectionModel.VendorTypeName = (txtVendorAccount.Text ?? string.Empty).Trim();
                _connectionModel.VendorDbCr = vendorDbCr;
                _connectionModel.ExpenseVatTypeName = (txtVatAccount1.Text ?? string.Empty).Trim();
                _connectionModel.ExpenseVatAccountNumber = (cboVatDbCr1.EditValue ?? string.Empty).ToString().Trim();

                var provider = (cboProvider.EditValue ?? ProviderOpenAi).ToString();
                var selectedModel = (cboModel.EditValue ?? string.Empty).ToString().Trim();
                var apiKey = txtAPIKey.Text;

                if (_connectionModel.AiSettings == null)
                    _connectionModel.AiSettings = new AiSettingsModel();

                _connectionModel.AiSettings.Provider = provider;
                SetProviderValues(provider, selectedModel, apiKey);

                _connectionModel.AiModel = selectedModel;
                _connectionModel.EncryptedAiApikey = apiKey;

                _connectionModel.ProjectsDocumentsFolder = txtProjectsFolder.Text.Trim();
                _connectionModel.EmployeesDocumentsFolder = txtEmployeesFolder.Text.Trim();
                _connectionModel.EngineersDocumentsFolder = txtEngineersFolder.Text.Trim();

                DatabaseFactory.SaveConnection(_connectionModel, DatabaseFactory.ProfilePrimary);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, @"Error Saving settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnBrowseProjects_Click(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            BrowseFolder("Select Projects Documents Folder", txtProjectsFolder);
        }

        private void btnBrowseEmployees_Click(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            BrowseFolder("Select Employees Documents Folder", txtEmployeesFolder);
        }

        private void btnBrowseEngineers_Click(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
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

        private void cboProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            var provider = (cboProvider.EditValue ?? ProviderOpenAi).ToString();
            LoadModelOptions(provider);
            txtAPIKey.Text = GetProviderApiKey(provider) ?? string.Empty;
        }
    }
}