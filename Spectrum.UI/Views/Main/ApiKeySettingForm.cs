using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using Spectrum.DataLayers.Accounting.Charts;
using Spectrum.DataLayers.Accounting.JournalType;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Accounting.Charts;
using Spectrum.Models.Accounting.JournalType;
using Spectrum.Models.Administration.Connections;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.Main
{
    public partial class ApiKeySettingForm : XtraForm
    {
        private ConnectionModel _connectionModel = new ConnectionModel();
        private readonly ChartRepository _chartRepository = new ChartRepository(DatabaseFactory.ProfilePrimary);
        private readonly JournalTypeRepository _journalTypeRepository = new JournalTypeRepository(DatabaseFactory.ProfilePrimary);
        private IList<ChartModel> _charts = new List<ChartModel>();
        private IList<JournalTypeModel> _journalTypes = new List<JournalTypeModel>();

        private TextEdit txtInvoiceType;
        private SearchLookUpEdit cboInvoiceJournalType;
        private GridView gvInvoiceJournalTypes;
        private TextEdit txtCustomerType;
        private TextEdit txtCustomerDbCr;
        private TextEdit txtVatType;
        private SearchLookUpEdit cboVatAccount;
        private GridView gvVatAccounts;
        private LayoutControlGroup grpInvoicePosting;

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
            BuildInvoicePostingControls();
            _ = StartLoadingAsync();
        }

        private async Task StartLoadingAsync()
        {
            try
            {
                ConnectionParams();
                await InitializeBindingsAsync();
                WireUpBindings();
                ApplyPermissions();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConnectionParams()
        {
            _connectionModel = DatabaseFactory.GetConnection(DatabaseFactory.ProfilePrimary) ?? new ConnectionModel();
            if (_connectionModel.AiSettings == null)
                _connectionModel.AiSettings = new AiSettingsModel();
        }

        private async Task InitializeBindingsAsync()
        {
            _journalTypes = await _journalTypeRepository.GetJournalTypesAsync();
            _charts = await _chartRepository.GetChartsAsync();

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

            ConfigureLookupViews();

            cboInvoiceJournalType.Properties.DataSource = null;
            cboInvoiceJournalType.Properties.DataSource = _journalTypes;
            cboInvoiceJournalType.Properties.DisplayMember = nameof(JournalTypeModel.Code);
            cboInvoiceJournalType.Properties.ValueMember = nameof(JournalTypeModel.Code);

            cboVatAccount.Properties.DataSource = null;
            cboVatAccount.Properties.DataSource = _charts;
            cboVatAccount.Properties.DisplayMember = nameof(ChartModel.AccountNumber);
            cboVatAccount.Properties.ValueMember = nameof(ChartModel.AccountNumber);

            txtInvoiceType.Text = string.IsNullOrWhiteSpace(_connectionModel.InvoicePostingInvoiceType)
                ? "Invoice"
                : _connectionModel.InvoicePostingInvoiceType;
            cboInvoiceJournalType.EditValue = _connectionModel.InvoicePostingJournalType ?? string.Empty;

            txtCustomerType.Text = string.IsNullOrWhiteSpace(_connectionModel.InvoicePostingCustomerType)
                ? "Customer"
                : _connectionModel.InvoicePostingCustomerType;
            txtCustomerDbCr.Text = string.IsNullOrWhiteSpace(_connectionModel.InvoicePostingCustomerDbCr)
                ? "D"
                : _connectionModel.InvoicePostingCustomerDbCr;

            txtVatType.Text = string.IsNullOrWhiteSpace(_connectionModel.InvoicePostingVatType)
                ? "VAT"
                : _connectionModel.InvoicePostingVatType;
            cboVatAccount.EditValue = _connectionModel.InvoicePostingVatAccountNumber ?? string.Empty;
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

        private void BuildInvoicePostingControls()
        {
            const int invoicePostingRows = 6;
            const int rowHeight = 26;
            var sectionHeight = invoicePostingRows * rowHeight;

            txtInvoiceType = new TextEdit { Name = "txtInvoiceType", StyleController = mainLayout };
            cboInvoiceJournalType = new SearchLookUpEdit { Name = "cboInvoiceJournalType", StyleController = mainLayout };
            gvInvoiceJournalTypes = new GridView();
            ConfigureSearchLookupEditor(cboInvoiceJournalType, gvInvoiceJournalTypes);

            txtCustomerType = new TextEdit { Name = "txtCustomerType", StyleController = mainLayout };
            txtCustomerDbCr = new TextEdit { Name = "txtCustomerDbCr", StyleController = mainLayout };
            txtVatType = new TextEdit { Name = "txtVatType", StyleController = mainLayout };

            cboVatAccount = new SearchLookUpEdit { Name = "cboVatAccount", StyleController = mainLayout };
            gvVatAccounts = new GridView();
            ConfigureSearchLookupEditor(cboVatAccount, gvVatAccounts);

            mainLayout.Controls.Add(txtInvoiceType);
            mainLayout.Controls.Add(cboInvoiceJournalType);
            mainLayout.Controls.Add(txtCustomerType);
            mainLayout.Controls.Add(txtCustomerDbCr);
            mainLayout.Controls.Add(txtVatType);
            mainLayout.Controls.Add(cboVatAccount);

            grpInvoicePosting = new LayoutControlGroup
            {
                Name = "grpInvoicePosting",
                GroupStyle = DevExpress.Utils.GroupStyle.Light,
                Text = "Invoice Posting Settings",
                Location = new Point(0, grpInfo.Location.Y + grpInfo.Size.Height),
                Size = new Size(710, sectionHeight)
            };

            grpInvoicePosting.Items.AddRange(new BaseLayoutItem[]
            {
                new LayoutControlItem { Control = txtInvoiceType, Text = "Invoice Type", TextSize = new Size(131, 16) },
                new LayoutControlItem { Control = cboInvoiceJournalType, Text = "Journal Type", TextSize = new Size(131, 16) },
                new LayoutControlItem { Control = txtCustomerType, Text = "Customer Type", TextSize = new Size(131, 16) },
                new LayoutControlItem { Control = txtCustomerDbCr, Text = "Customer DbCr", TextSize = new Size(131, 16) },
                new LayoutControlItem { Control = txtVatType, Text = "VAT Type", TextSize = new Size(131, 16) },
                new LayoutControlItem { Control = cboVatAccount, Text = "VAT Account", TextSize = new Size(131, 16) }
            });

            layoutControlGroup1.Items.Add(grpInvoicePosting);

            MoveLayoutItem(layoutControlGroup4, sectionHeight);
            MoveLayoutItem(emptySpaceItem1, sectionHeight);
            MoveLayoutItem(emptySpaceItem2, sectionHeight);
            MoveLayoutItem(layoutControlItem4, sectionHeight);
            MoveLayoutItem(layoutControlItem5, sectionHeight);

            var newHeight = ClientSize.Height + sectionHeight;
            ClientSize = new Size(ClientSize.Width, newHeight);
            mainLayout.Size = new Size(mainLayout.Size.Width, newHeight);
            layoutControlGroup1.Size = new Size(layoutControlGroup1.Size.Width, layoutControlGroup1.Size.Height + sectionHeight);
            Text = "Application Settings";
        }

        private static void MoveLayoutItem(BaseLayoutItem item, int offsetY)
        {
            item.Location = new Point(item.Location.X, item.Location.Y + offsetY);
        }

        private static void ConfigureSearchLookupEditor(SearchLookUpEdit editor, GridView view)
        {
            editor.Properties.NullText = string.Empty;
            editor.Properties.PopupView = view;
            editor.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
        }

        private void ConfigureLookupViews()
        {
            gvInvoiceJournalTypes.Columns.Clear();
            gvInvoiceJournalTypes.OptionsSelection.EnableAppearanceFocusedCell = false;
            gvInvoiceJournalTypes.FocusRectStyle = DrawFocusRectStyle.RowFocus;
            gvInvoiceJournalTypes.OptionsView.ShowGroupPanel = false;
            gvInvoiceJournalTypes.Columns.AddVisible(nameof(JournalTypeModel.Code), "Code");
            gvInvoiceJournalTypes.Columns.AddVisible(nameof(JournalTypeModel.Name), "Name");

            gvVatAccounts.Columns.Clear();
            gvVatAccounts.OptionsSelection.EnableAppearanceFocusedCell = false;
            gvVatAccounts.FocusRectStyle = DrawFocusRectStyle.RowFocus;
            gvVatAccounts.OptionsView.ShowGroupPanel = false;
            gvVatAccounts.Columns.AddVisible(nameof(ChartModel.AccountNumber), "Account");
            gvVatAccounts.Columns.AddVisible(nameof(ChartModel.AccountName), "Account Name");
            gvVatAccounts.Columns.AddVisible(nameof(ChartModel.AccountType), "Type");
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
                var customerDbCr = (txtCustomerDbCr.Text ?? string.Empty).Trim().ToUpperInvariant();
                if (!string.IsNullOrWhiteSpace(customerDbCr) &&
                    !string.Equals(customerDbCr, "D", StringComparison.Ordinal) &&
                    !string.Equals(customerDbCr, "C", StringComparison.Ordinal))
                {
                    XtraMessageBox.Show("Customer DbCr must be either D or C.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCustomerDbCr.Focus();
                    return;
                }

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

                _connectionModel.InvoicePostingInvoiceType = txtInvoiceType.Text.Trim();
                _connectionModel.InvoicePostingJournalType = (cboInvoiceJournalType.EditValue ?? string.Empty).ToString().Trim();
                _connectionModel.InvoicePostingCustomerType = txtCustomerType.Text.Trim();
                _connectionModel.InvoicePostingCustomerDbCr = customerDbCr;
                _connectionModel.InvoicePostingVatType = txtVatType.Text.Trim();
                _connectionModel.InvoicePostingVatAccountNumber = (cboVatAccount.EditValue ?? string.Empty).ToString().Trim();

                var selectedVatAccount = _charts.FirstOrDefault(x =>
                    string.Equals(x.AccountNumber, _connectionModel.InvoicePostingVatAccountNumber, StringComparison.OrdinalIgnoreCase));
                _connectionModel.InvoicePostingVatAccountType = selectedVatAccount?.AccountType ?? string.Empty;

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