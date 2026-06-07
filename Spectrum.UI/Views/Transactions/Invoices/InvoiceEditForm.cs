using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.WinExplorer;
using Spectrum.DataLayers.Accounting.Invoices;
using Spectrum.DataLayers.Common.Currencies;
using Spectrum.DataLayers.Common.Items;
using Spectrum.DataLayers.DataAccess;
using Spectrum.DataLayers.Members.Clients;
using Spectrum.DataLayers.Projects;
using Spectrum.Models.Accounting.Invoices;
using Spectrum.Models.Accounting.Journals;
using Spectrum.Models.Common.Currencies;
using Spectrum.Models.Common.Items;
using Spectrum.Models.Members.Clients;
using Spectrum.Models.Projects;
using Spectrum.Models.Users;
using Spectrum.Utilities;
using Spectrum.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.Transactions.Invoices
{
    public partial class InvoiceEditForm : RibbonForm
    {
        private InvoiceModel _invoiceModel = new InvoiceModel();
        private IList<ClientModel> _clients = new List<ClientModel>();
        private IList<CurrencyModel> _currencies = new List<CurrencyModel>();
        private IList<ProjectModel> _projects = new List<ProjectModel>();
        private IList<ItemModel> _items = new List<ItemModel>();

        private BindingList<InvoiceDetailModel> _invoiceDetails = new BindingList<InvoiceDetailModel>();

        private BindingList<InvoiceDocumentModel> _invoiceDocuments = new BindingList<InvoiceDocumentModel>();
        private readonly List<ActivityTimelineEntry> _lineActivityEntries = new List<ActivityTimelineEntry>();
        private readonly Dictionary<InvoiceDetailModel, InvoiceDetailSnapshot> _detailSnapshots = new Dictionary<InvoiceDetailModel, InvoiceDetailSnapshot>();

        private readonly ClientRepository _clientRepository = new ClientRepository(DatabaseFactory.ProfilePrimary);
        private readonly CurrencyRepository _currencyRepository = new CurrencyRepository(DatabaseFactory.ProfilePrimary);
        private readonly CurrencyExchangeRepository _currencyExchangeRepository = new CurrencyExchangeRepository(DatabaseFactory.ProfilePrimary);
        private readonly InvoiceRepository _invoiceRepository = new InvoiceRepository(DatabaseFactory.ProfilePrimary);
        private readonly ProjectRepository _projectRepository = new ProjectRepository(DatabaseFactory.ProfilePrimary);
        private readonly ItemRepository _itemRepository = new ItemRepository(DatabaseFactory.ProfilePrimary);

        private readonly LogInfoRepository _logInfoRepository = new LogInfoRepository();

        //Init permissionvariables
        private bool _canAdd = true;
        private bool _canEdit = true;
        private bool _canDelete = true;
        private bool _canPrint = true;
        private bool _isAdmin = true;
        private bool _isProtected = true;
        private bool _suspendActivityTracking;
        private int _activitySequence;

        private DXMenuItem[] _menuItems;

        public EventHandler SendUpdatedInvoice;

        public InvoiceEditForm(InvoiceModel model)
        {
            InitializeComponent();

            _invoiceModel = model ?? new InvoiceModel();

            StartLoading();
        }

        private async void StartLoading()
        {
            try
            {
                await InitializeBindings();
                WireUpBindings();
                await ApplyDefaultsAsync();
                ApplyPermissions();
                InitializeMenuItems();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region InitializeBindings Methods
        private async Task InitializeBindings()
        {
            try
            {
                var loadTasks = new[]
                {
                    LoadClientsAsync(),
                    LoadDocumentsAsync(),
                    LoadCurrenciesAsync(),
                    LoadProjectsAsync(),
                    LoadItemsAsync()
            };

                await Task.WhenAll(loadTasks);
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading form data", ex);
            }
        }



        #region Data Loading Methods

        //
        private async Task LoadClientsAsync()
        {
            _clients = await _clientRepository.GetClientsAsync();
        }


        private async Task LoadDocumentsAsync()
        {
            _invoiceDocuments = LoadDocumentsForInvoice();
            bsDocuments.DataSource = _invoiceDocuments;
        }

        private async Task LoadCurrenciesAsync()
        {
            _currencies = await _currencyRepository.GetCurrenciesAsync();
        }

        private async Task LoadProjectsAsync()
        {
            _projects = await _projectRepository.GetProjectsAsync();
        }

        private async Task LoadItemsAsync()
        {
            _items = await _itemRepository.GetItemsAsync();
        }

        #endregion
        private void WireUpBindings()
        {
            _invoiceDetails = new BindingList<InvoiceDetailModel>((_invoiceModel.InvoiceDetails ?? new List<InvoiceDetailModel>())
                .OrderBy(x => x.LineNo)
                .ToList());

            bsInvoice.DataSource = _invoiceModel;
            bsInvoiceDetails.DataSource = _invoiceDetails;

            cboCurrencies.Properties.DataSource = null;
            cboCurrencies.Properties.DataSource = _currencies;

            repCurrencies.DataSource = null;
            repCurrencies.DataSource = _currencies;

            ResetDetailSnapshots();
            RenderActivityTimeline();

            cboMemebrs.Properties.DataSource = null;
            cboMemebrs.Properties.DataSource = _clients;

            cboProjectReferences.Properties.DataSource = null;
            cboProjectReferences.Properties.DataSource = _projects;

            repItems.DataSource = null;
            repItems.DataSource = _items;
        }

        private async Task ApplyDefaultsAsync()
        {
            dtInvoiceDate.EditValue = DateTime.Now;

            if (string.IsNullOrWhiteSpace(_invoiceModel.Currency))
            {
                var defaultCurrency = _currencies.FirstOrDefault(x => x != null && x.IsDefault)
                    ?? _currencies.FirstOrDefault(x => x != null);

                if (defaultCurrency != null)
                    cboCurrencies.EditValue = defaultCurrency.CurrencyCode;
            }

            await LoadSelectedCurrencyRateAsync();

            gvDocuments.GetThumbnailImage += TileView1_GetThumbnailImage;
            gvDocuments.OptionsImageLoad.RandomShow = true;
            gvDocuments.OptionsImageLoad.LoadThumbnailImagesFromDataSource = false;
            gvDocuments.OptionsImageLoad.AsyncLoad = true;
            activityTimelineScroll.BackColor = Color.FromArgb(245, 247, 250);

            mnuInvoice.Click += LeftSideInvoiceMenu_Click;
            mnuDetails.Click += LeftSideInvoiceMenu_Click;
            mnuAttachment.Click += LeftSideInvoiceMenu_Click;

            gvInvoiceDetails.InitNewRow -= gvInvoiceDetails_InitNewRow;
            gvInvoiceDetails.InitNewRow += gvInvoiceDetails_InitNewRow;
            gvInvoiceDetails.CellValueChanged -= gvInvoiceDetails_CellValueChanged;
            gvInvoiceDetails.CellValueChanged += gvInvoiceDetails_CellValueChanged;

            _invoiceDetails.ListChanged -= InvoiceDetails_ListChanged;
            _invoiceDetails.ListChanged += InvoiceDetails_ListChanged;

            cboCurrencies.EditValueChanged -= cboCurrencies_EditValueChanged;
            cboCurrencies.EditValueChanged += cboCurrencies_EditValueChanged;

            dtInvoiceDate.EditValueChanged -= dtInvoiceDate_EditValueChanged;
            dtInvoiceDate.EditValueChanged += dtInvoiceDate_EditValueChanged;

            activityTimelineScroll.Resize -= activityTimelineScroll_Resize;
            activityTimelineScroll.Resize += activityTimelineScroll_Resize;

            tabInvoiceGroup.SelectedTabPage = tabInvoice;

            RenderActivityTimeline();
        }

        private void ApplyPermissions()
        {
            btnNew.Enabled = _isAdmin || _canAdd;
            btnDuplicate.Enabled = _isAdmin || _canAdd;
            btnSave.Enabled = _isAdmin || _canEdit;
            btnSaveAndNew.Enabled = _isAdmin || _canEdit;
            btnPrint.Enabled = _isAdmin || _canPrint;
            btnPrintOriginal.Enabled = _isAdmin || _canPrint;
            btnDelete.Enabled = _isAdmin || _canDelete;
        }

        private void InitializeMenuItems()
        {
            _menuItems = new[]
            {
                new DXMenuItem("New", ItemNew_Click),
                new DXMenuItem("Edit", ItemEdit_Click),
                new DXMenuItem("Delete", ItemDelete_Click)
            };

            //gvInvoiceDetails.PopupMenuShowing -= gvAddendum_PopupMenuShowing;
            //gvInvoiceDetails.PopupMenuShowing += gvAddendum_PopupMenuShowing;
        }

        private BindingList<InvoiceDocumentModel> LoadDocumentsForInvoice()
        {
            var documents = new BindingList<InvoiceDocumentModel>();
            var invoiceId = _invoiceModel?._id;
            if (string.IsNullOrWhiteSpace(invoiceId))
                return documents;

            var connection = DatabaseFactory.GetConnection(DatabaseFactory.ProfilePrimary);
            var rootFolder = string.IsNullOrWhiteSpace(connection?.InvoicesDocumentsFolder)
                ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SpectrumApp", "Invoices")
                : connection.InvoicesDocumentsFolder;

            if (!Directory.Exists(rootFolder))
                return documents;

            var prefix = invoiceId + "_";
            var files = Directory.GetFiles(rootFolder)
                .Where(path => Path.GetFileName(path).StartsWith(prefix, StringComparison.OrdinalIgnoreCase));

            foreach (var filePath in files)
            {
                try
                {
                    documents.Add(new InvoiceDocumentModel
                    {
                        DocumentName = GetDisplayDocumentName(invoiceId, filePath),
                        OriginPath = filePath,
                        DocumentDate = File.GetCreationTime(filePath),
                        StreamedDate = DateTime.Now,
                        DocumentContent = File.ReadAllBytes(filePath)
                    });
                }
                catch
                {
                    //
                }
            }

            return documents;
        }

        private static string GetDisplayDocumentName(string recordId, string filePath)
        {
            var fileName = Path.GetFileName(filePath);
            var prefix = (recordId ?? string.Empty) + "_";
            return fileName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
                ? fileName.Substring(prefix.Length)
                : fileName;
        }

        private void TileView1_GetThumbnailImage(object sender, ThumbnailImageEventArgs e)
        {
            var fileName = (string)gvDocuments.GetRowCellValue(e.DataSourceIndex, colName);
            var ext = Path.GetExtension(fileName);
            e.ThumbnailImage = HelperApplication.GetFileExtensionImage(ext, IconSizeType.Large, new Size(64, 64));
        }

        private void activityTimelineScroll_Resize(object sender, EventArgs e)
        {
            RenderActivityTimeline();
        }

        private async void dtInvoiceDate_EditValueChanged(object sender, EventArgs e)
        {
            if (_suspendActivityTracking)
                return;

            await LoadSelectedCurrencyRateAsync();
            RenderActivityTimeline();
        }

        private async void cboCurrencies_EditValueChanged(object sender, EventArgs e)
        {
            if (_suspendActivityTracking)
                return;

            await LoadSelectedCurrencyRateAsync();
            RenderActivityTimeline();
        }

        private async Task LoadSelectedCurrencyRateAsync()
        {
            var selectedCurrency = Convert.ToString(cboCurrencies.EditValue);
            if (string.IsNullOrWhiteSpace(selectedCurrency))
            {
                txtRate.EditValue = 0m;
                return;
            }

            var exchangeDate = dtInvoiceDate.EditValue as DateTime? ?? DateTime.Now;
            var exchange = await _currencyExchangeRepository.GetLatestExchangeByCurrencyAsync(selectedCurrency, exchangeDate);

            txtRate.EditValue = exchange != null ? exchange.Rate : 0m;
        }

        private void gvInvoiceDetails_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            if (_suspendActivityTracking)
                return;

            var detail = gvInvoiceDetails.GetRow(e.RowHandle) as InvoiceDetailModel;
            if (detail == null)
                return;

            if (detail.LineNo <= 0)
            {
                detail.LineNo = GetNextLineNumber(detail);
                gvInvoiceDetails.SetRowCellValue(e.RowHandle, colLineNo, detail.LineNo);
            }

            AppendLineActivity(DateTime.UtcNow, "Line Added", BuildAddedLineDescription(detail), CurrentUser.UserName, Color.FromArgb(37, 99, 235));
            _detailSnapshots[detail] = CreateSnapshot(detail);
            SyncInvoiceDetailsFromBinding();
            RenderActivityTimeline();
        }

        private void gvInvoiceDetails_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (_suspendActivityTracking || e.RowHandle < 0 || e.Column == null || string.Equals(e.Column.FieldName, nameof(InvoiceDetailModel.LineNo), StringComparison.Ordinal))
                return;

            var detail = gvInvoiceDetails.GetRow(e.RowHandle) as InvoiceDetailModel;
            if (detail == null)
                return;

            InvoiceDetailSnapshot snapshot;
            if (!_detailSnapshots.TryGetValue(detail, out snapshot))
            {
                _detailSnapshots[detail] = CreateSnapshot(detail);
                SyncInvoiceDetailsFromBinding();
                return;
            }

            var changeDescription = BuildLineChangeDescription(snapshot, detail, e.Column.FieldName);
            _detailSnapshots[detail] = CreateSnapshot(detail);
            SyncInvoiceDetailsFromBinding();

            if (string.IsNullOrWhiteSpace(changeDescription))
                return;

            AppendLineActivity(DateTime.UtcNow, "Line Updated", changeDescription, CurrentUser.UserName, Color.FromArgb(37, 99, 235));
            RenderActivityTimeline();
        }

        private void InvoiceDetails_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (_suspendActivityTracking)
                return;

            SyncInvoiceDetailsFromBinding();

            if (e.ListChangedType == ListChangedType.ItemDeleted || e.ListChangedType == ListChangedType.Reset)
            {
                TrackDeletedLines();
                RenderActivityTimeline();
                return;
            }

            if (e.ListChangedType == ListChangedType.ItemAdded && e.NewIndex >= 0 && e.NewIndex < _invoiceDetails.Count)
            {
                var detail = _invoiceDetails[e.NewIndex];
                if (detail != null && !_detailSnapshots.ContainsKey(detail))
                {
                    _detailSnapshots[detail] = CreateSnapshot(detail);
                }
            }
        }

        private void ResetDetailSnapshots()
        {
            _detailSnapshots.Clear();
            foreach (var detail in _invoiceDetails.Where(x => x != null))
            {
                _detailSnapshots[detail] = CreateSnapshot(detail);
            }
        }

        private void TrackDeletedLines()
        {
            var currentDetails = new HashSet<InvoiceDetailModel>(_invoiceDetails.Where(x => x != null));
            var removedDetails = _detailSnapshots.Keys.Where(detail => !currentDetails.Contains(detail)).ToList();

            foreach (var removedDetail in removedDetails)
            {
                InvoiceDetailSnapshot snapshot;
                if (!_detailSnapshots.TryGetValue(removedDetail, out snapshot))
                    continue;

                AppendLineActivity(DateTime.UtcNow, "Line Deleted", BuildDeletedLineDescription(snapshot), CurrentUser.UserName, Color.FromArgb(156, 163, 175));
                _detailSnapshots.Remove(removedDetail);
            }
        }

        private void SyncInvoiceDetailsFromBinding()
        {
            _invoiceModel.InvoiceDetails = _invoiceDetails.Where(x => x != null)
                .OrderBy(x => x.LineNo)
                .ToList();
        }

        private int GetNextLineNumber(InvoiceDetailModel currentDetail)
        {
            return _invoiceDetails
                .Where(x => x != null && !ReferenceEquals(x, currentDetail))
                .Select(x => x.LineNo)
                .DefaultIfEmpty(0)
                .Max() + 1;
        }

        private void AppendLineActivity(DateTime timestamp, string title, string description, string performedBy, Color accentColor)
        {
            _lineActivityEntries.Add(new ActivityTimelineEntry
            {
                Title = title,
                Description = description,
                Timestamp = timestamp,
                PerformedBy = string.IsNullOrWhiteSpace(performedBy) ? "System" : performedBy,
                AccentColor = accentColor,
                Sequence = ++_activitySequence
            });
        }

        private void RenderActivityTimeline()
        {
            if (activityTimelineScroll == null)
                return;

            var activities = BuildTimelineEntries()
                .OrderByDescending(x => x.Timestamp)
                .ThenByDescending(x => x.Sequence)
                .ToList();

            activityTimelineScroll.SuspendLayout();
            activityTimelineScroll.Controls.Clear();

            var contentWidth = Math.Max(300, activityTimelineScroll.ClientSize.Width - 32);
            var y = 16;

            var header = new Label
            {
                AutoSize = false,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(17, 24, 39),
                Location = new Point(16, y),
                Size = new Size(contentWidth, 26),
                Text = "Activity"
            };
            activityTimelineScroll.Controls.Add(header);
            y += 34;

            if (!activities.Any())
            {
                var emptyLabel = new Label
                {
                    AutoSize = false,
                    Font = new Font("Segoe UI", 9F),
                    ForeColor = Color.FromArgb(107, 114, 128),
                    Location = new Point(16, y),
                    Size = new Size(contentWidth, 22),
                    Text = "No invoice activity available."
                };
                activityTimelineScroll.Controls.Add(emptyLabel);
                y += emptyLabel.Height;
            }
            else
            {
                for (var index = 0; index < activities.Count; index++)
                {
                    var activityPanel = CreateActivityPanel(activities[index], contentWidth, index == activities.Count - 1);
                    activityPanel.Location = new Point(16, y);
                    activityTimelineScroll.Controls.Add(activityPanel);
                    y += activityPanel.Height + 10;
                }
            }

            activityTimelineScroll.AutoScrollMinSize = new Size(0, y + 16);
            activityTimelineScroll.ResumeLayout();
        }

        private List<ActivityTimelineEntry> BuildTimelineEntries()
        {
            var entries = new List<ActivityTimelineEntry>();
            var createdBy = string.IsNullOrWhiteSpace(_invoiceModel.CreatedBy) ? CurrentUser.UserName : _invoiceModel.CreatedBy;
            var detailCount = _invoiceModel.InvoiceDetails != null ? _invoiceModel.InvoiceDetails.Count : 0;

            if (_invoiceModel.CreatedAt != default(DateTime))
            {
                entries.Add(new ActivityTimelineEntry
                {
                    Title = "Invoice Created",
                    Description = string.Format("New invoice with {0} detail line{1}. Currency: {2}.", detailCount, detailCount == 1 ? string.Empty : "s", string.IsNullOrWhiteSpace(_invoiceModel.Currency) ? "N/A" : _invoiceModel.Currency),
                    Timestamp = _invoiceModel.CreatedAt,
                    PerformedBy = string.IsNullOrWhiteSpace(createdBy) ? "System" : createdBy,
                    AccentColor = Color.FromArgb(156, 163, 175),
                    Sequence = 1
                });
            }

            if (_invoiceModel.InvoiceDate != default(DateTime))
            {
                entries.Add(new ActivityTimelineEntry
                {
                    Title = "Invoice Date",
                    Description = string.Format("Invoice dated {0:dd MMM yyyy}.", _invoiceModel.InvoiceDate),
                    Timestamp = _invoiceModel.InvoiceDate,
                    PerformedBy = string.IsNullOrWhiteSpace(_invoiceModel.LastModifiedBy) ? (string.IsNullOrWhiteSpace(createdBy) ? "System" : createdBy) : _invoiceModel.LastModifiedBy,
                    AccentColor = Color.FromArgb(37, 99, 235),
                    Sequence = 2,
                    DateOnly = true
                });
            }

            entries.AddRange(_lineActivityEntries);
            return entries;
        }

        private Panel CreateActivityPanel(ActivityTimelineEntry entry, int width, bool isLast)
        {
            const int leftPadding = 34;
            const int rightPadding = 12;
            const int panelTop = 4;
            var metaFont = new Font("Segoe UI", 8.5F);
            var titleFont = new Font("Segoe UI", 10F, FontStyle.Bold);
            var descriptionFont = new Font("Segoe UI", 9F);
            var textWidth = Math.Max(220, width - leftPadding - rightPadding);

            var titleHeight = TextRenderer.MeasureText(entry.Title ?? string.Empty, titleFont, new Size(textWidth, 0), TextFormatFlags.WordBreak).Height;
            var metaText = FormatActivityMeta(entry);
            var metaHeight = TextRenderer.MeasureText(metaText, metaFont, new Size(textWidth, 0), TextFormatFlags.WordBreak).Height;
            var descriptionHeight = TextRenderer.MeasureText(entry.Description ?? string.Empty, descriptionFont, new Size(textWidth, 0), TextFormatFlags.WordBreak).Height;
            var panelHeight = Math.Max(72, panelTop + titleHeight + metaHeight + descriptionHeight + 16);

            var panel = new Panel
            {
                BackColor = Color.Transparent,
                Size = new Size(width, panelHeight),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            var line = new Panel
            {
                BackColor = isLast ? activityTimelineScroll.BackColor : Color.FromArgb(217, 222, 228),
                Location = new Point(9, 16),
                Size = new Size(2, panelHeight - 12)
            };
            panel.Controls.Add(line);

            var marker = new Label
            {
                AutoSize = false,
                Font = new Font("Segoe UI Symbol", 10F, FontStyle.Bold),
                ForeColor = entry.AccentColor,
                Location = new Point(0, 0),
                Size = new Size(22, 22),
                Text = "●",
                TextAlign = ContentAlignment.TopCenter
            };
            panel.Controls.Add(marker);

            var titleLabel = new Label
            {
                AutoSize = false,
                Font = titleFont,
                ForeColor = Color.FromArgb(17, 24, 39),
                Location = new Point(leftPadding, panelTop),
                Size = new Size(textWidth, titleHeight),
                Text = entry.Title
            };
            panel.Controls.Add(titleLabel);

            var metaLabel = new Label
            {
                AutoSize = false,
                Font = metaFont,
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(leftPadding, titleLabel.Bottom + 2),
                Size = new Size(textWidth, metaHeight),
                Text = metaText
            };
            panel.Controls.Add(metaLabel);

            var descriptionLabel = new Label
            {
                AutoSize = false,
                Font = descriptionFont,
                ForeColor = Color.FromArgb(55, 65, 81),
                Location = new Point(leftPadding, metaLabel.Bottom + 4),
                Size = new Size(textWidth, descriptionHeight),
                Text = entry.Description
            };
            panel.Controls.Add(descriptionLabel);

            return panel;
        }

        private string FormatActivityMeta(ActivityTimelineEntry entry)
        {
            var performedBy = string.IsNullOrWhiteSpace(entry.PerformedBy) ? "System" : entry.PerformedBy;
            return entry.DateOnly
                ? string.Format("{0} — {1:dd MMM yyyy}", performedBy, entry.Timestamp)
                : string.Format("{0} — {1:MMM dd, yyyy HH:mm} UTC", performedBy, entry.Timestamp);
        }

        private static InvoiceDetailSnapshot CreateSnapshot(InvoiceDetailModel detail)
        {
            return new InvoiceDetailSnapshot
            {
                LineNo = detail.LineNo,
                ItemCode = detail.ItemCode,
                Description = detail.Description,
                Currency = detail.Currency,
                Rate = detail.Rate,
                Amount = detail.Amount,
                LAmount = detail.LAmount,
                FAmount = detail.FAmount,
                VAT = detail.VAT,
                VATRate = detail.VATRate,
                VATValue = detail.VATValue,
                Notes = detail.Notes
            };
        }

        private static string BuildAddedLineDescription(InvoiceDetailModel detail)
        {
            return string.Format("Added line {0}{1}.", detail.LineNo > 0 ? detail.LineNo.ToString() : "#", BuildLineIdentity(detail.ItemCode, detail.Description));
        }

        private static string BuildDeletedLineDescription(InvoiceDetailSnapshot snapshot)
        {
            return string.Format("Deleted line {0}{1}.", snapshot.LineNo > 0 ? snapshot.LineNo.ToString() : "#", BuildLineIdentity(snapshot.ItemCode, snapshot.Description));
        }

        private static string BuildLineChangeDescription(InvoiceDetailSnapshot snapshot, InvoiceDetailModel detail, string fieldName)
        {
            switch (fieldName)
            {
                case nameof(InvoiceDetailModel.ItemCode):
                    return BuildFieldChangeDescription(detail.LineNo, "item code", snapshot.ItemCode, detail.ItemCode);
                case nameof(InvoiceDetailModel.Description):
                    return BuildFieldChangeDescription(detail.LineNo, "description", snapshot.Description, detail.Description);
                case nameof(InvoiceDetailModel.Currency):
                    return BuildFieldChangeDescription(detail.LineNo, "currency", snapshot.Currency, detail.Currency);
                case nameof(InvoiceDetailModel.Rate):
                    return BuildFieldChangeDescription(detail.LineNo, "rate", snapshot.Rate, detail.Rate);
                case nameof(InvoiceDetailModel.Amount):
                    return BuildFieldChangeDescription(detail.LineNo, "amount", snapshot.Amount, detail.Amount);
                case nameof(InvoiceDetailModel.LAmount):
                    return BuildFieldChangeDescription(detail.LineNo, "local amount", snapshot.LAmount, detail.LAmount);
                case nameof(InvoiceDetailModel.FAmount):
                    return BuildFieldChangeDescription(detail.LineNo, "foreign amount", snapshot.FAmount, detail.FAmount);
                case nameof(InvoiceDetailModel.VAT):
                    return BuildFieldChangeDescription(detail.LineNo, "VAT", snapshot.VAT, detail.VAT);
                case nameof(InvoiceDetailModel.VATRate):
                    return BuildFieldChangeDescription(detail.LineNo, "VAT rate", snapshot.VATRate, detail.VATRate);
                case nameof(InvoiceDetailModel.VATValue):
                    return BuildFieldChangeDescription(detail.LineNo, "VAT value", snapshot.VATValue, detail.VATValue);
                case nameof(InvoiceDetailModel.Notes):
                    return BuildFieldChangeDescription(detail.LineNo, "notes", snapshot.Notes, detail.Notes);
                default:
                    return null;
            }
        }

        private static string BuildFieldChangeDescription<T>(int lineNo, string fieldCaption, T oldValue, T newValue)
        {
            if (Equals(oldValue, newValue))
                return null;

            return string.Format("Updated line {0} {1} from '{2}' to '{3}'.", lineNo > 0 ? lineNo.ToString() : "#", fieldCaption, FormatValue(oldValue), FormatValue(newValue));
        }

        private static string FormatValue(object value)
        {
            if (value == null)
                return string.Empty;

            var dateValue = value as DateTime?;
            if (dateValue.HasValue)
                return dateValue.Value.ToString("dd MMM yyyy HH:mm");

            return Convert.ToString(value);
        }

        private static string BuildLineIdentity(string itemCode, string description)
        {
            var values = new[] { itemCode, description }
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToArray();

            if (!values.Any())
                return string.Empty;

            return string.Format(" ({0})", string.Join(" - ", values));
        }

        private sealed class ActivityTimelineEntry
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime Timestamp { get; set; }
            public string PerformedBy { get; set; }
            public Color AccentColor { get; set; }
            public int Sequence { get; set; }
            public bool DateOnly { get; set; }
        }

        private sealed class InvoiceDetailSnapshot
        {
            public int LineNo { get; set; }
            public string ItemId { get; set; }
            public string ItemCode { get; set; }
            public string Description { get; set; }
            public string Currency { get; set; }
            public decimal Rate { get; set; }
            public decimal Amount { get; set; }
            public decimal LAmount { get; set; }
            public decimal FAmount { get; set; }
            public bool VAT { get; set; }
            public decimal VATRate { get; set; }
            public decimal VATValue { get; set; }
            public string Notes { get; set; }
        }

        #endregion

        #region Menu Item Events

        private void ItemNew_Click(object sender, EventArgs e)
        {
            // Handle new item logic here
        }

        private void ItemEdit_Click(object sender, EventArgs e)
        {
            gvInvoiceDetails.ShowEditor();
        }

        private void ItemDelete_Click(object sender, EventArgs e)
        {
            if (!ConfirmAction("Delete row?", "Confirmation")) return;

            var invoiceDetail = gvInvoiceDetails.GetFocusedRow() as InvoiceDetailModel;
            if (invoiceDetail == null)
            {
                return;
            }

            invoiceDetail.Deleted = true;

            _invoiceDetails.Remove(invoiceDetail);
            _invoiceModel.InvoiceDetails = _invoiceDetails.ToList();
            bsInvoiceDetails.ResetBindings(false);
            EnumerateLines();
        }

        private void EnumerateLines()
        {
            for (int i = 0; i < gvInvoiceDetails.DataRowCount; i++)
            {
                gvInvoiceDetails.SetRowCellValue(i, gvInvoiceDetails.Columns["LineNo"], i + 1);
            }
        }

        #endregion


        #region Buttons Events Handlers

        private void btnNew_ItemClick(object sender, ItemClickEventArgs e)
        {
            _invoiceModel = new InvoiceModel();
            StartLoading();
        }
        private void btnDuplicate_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private async void btnSave_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!ValidateData()) return;
            await SaveInvoiceAsync();
        }

        private async void btnSaveAndNew_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!ValidateData()) return;
            await SaveInvoiceAsync();
            _invoiceModel = new InvoiceModel();
            StartLoading();
        }

        private void btnRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void btnPrint_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
        private void btnPrintOriginal_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void btnClose_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }

        private void btnResetGridStyle_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        #endregion

        private bool ValidateData()
        {
            var validateReturnValue = true;
            var messageNumber = 0;
            var validateMessage = new StringBuilder();

            if (string.IsNullOrWhiteSpace(txtSubject.Text))
            {
                messageNumber += 1; validateMessage.Append("\n- Subject cannot be empty.");
                validateReturnValue = false; txtSubject.Focus();
            }

            if (string.IsNullOrWhiteSpace(cboMemebrs.Text))
            {
                messageNumber += 1; validateMessage.Append("\n- Member cannot be empty.");
                validateReturnValue = false; cboMemebrs.Focus();
            }

            if (string.IsNullOrWhiteSpace(cboCurrencies.Text))
            {
                messageNumber += 1; validateMessage.Append("\n- Currency cannot be empty.");
                validateReturnValue = false; cboCurrencies.Focus();
            }

            if (!validateReturnValue)
            {
                validateMessage.Insert(0, "The following need your attention:");
                if (messageNumber > 1) validateMessage.Replace("following", "followings");
                XtraMessageBox.Show(validateMessage + " \nPlease try again.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return validateReturnValue;
        }

        private void LeftSideInvoiceMenu_Click(object sender, EventArgs e)
        {
            var selectedElement = sender as DevExpress.XtraBars.Navigation.AccordionControlElement;
            if (selectedElement == null)
                return;

            if (selectedElement == mnuInvoice)
            {
                tabInvoiceGroup.SelectedTabPage = tabInvoice;
            }
            else if (selectedElement == mnuDetails)
            {
                tabInvoiceGroup.SelectedTabPage = tabDetails;
            }
            else if (selectedElement == mnuAttachment)
            {
                tabInvoiceGroup.SelectedTabPage = tabAttachment;
            }
        }

        private void openDocuments_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            try
            {
                var invoiceId = _invoiceModel?._id;
                if (string.IsNullOrWhiteSpace(invoiceId))
                {
                    XtraMessageBox.Show("Please save the invoice record first to generate an ID for archived attachments.", "Save Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var ofd = new OpenFileDialog
                {
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    Multiselect = true,
                    Filter = @"All Files (*.*)|*.*" +
                             @"|PDF Portable Document Format (*.pdf)|*.pdf" +
                             @"|PNG Portable Network Graphics (*.png)|*.png" +
                             @"|JPEG File Interchange Format (*.jpg *.jpeg *jfif)|*.jpg;*.jpeg;*.jfif" +
                             @"|BMP Windows Bitmap (*.bmp)|*.bmp" +
                             @"|TIF Tagged Imaged File Format (*.tif *.tiff)|*.tif;*.tiff" +
                             @"|GIF Graphics Interchange Format (*.gif)|*.gif"
                };

                if (ofd.ShowDialog() != DialogResult.OK) return;

                var connection = DatabaseFactory.GetConnection(DatabaseFactory.ProfilePrimary);
                var rootFolder = string.IsNullOrWhiteSpace(connection?.EmployeesDocumentsFolder)
                    ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SpectrumApp", "Invoices")
                    : connection.EmployeesDocumentsFolder;
                Directory.CreateDirectory(rootFolder);

                foreach (var file in ofd.FileNames)
                {
                    try
                    {
                        string fileName = Path.GetFileName(file);
                        string archivedFileName = invoiceId + "_" + fileName;
                        string destinationPath = Path.Combine(rootFolder, archivedFileName);

                        if (File.Exists(destinationPath))
                        {
                            var result = XtraMessageBox.Show($"File '{archivedFileName}' already exists. Overwrite?", "Confirm Overwrite", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                            if (result == DialogResult.Cancel)
                            {
                                break;
                            }
                            if (result == DialogResult.No)
                            {
                                continue;
                            }
                        }

                        File.Copy(file, destinationPath, true);

                        var newDocument = new InvoiceDocumentModel
                        {
                            DocumentName = fileName,
                            OriginPath = destinationPath,
                            DocumentDate = File.GetCreationTime(destinationPath),
                            StreamedDate = DateTime.Now,
                            DocumentContent = File.ReadAllBytes(destinationPath)
                        };

                        bsDocuments.Add(newDocument);
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, @"Copy Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                PersistDocumentLinks();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gvDocuments_ContextButtonCustomize(object sender, WinExplorerViewContextButtonCustomizeEventArgs e)
        {
            if (gvDocuments.FocusedRowHandle == e.RowHandle)
            {
                e.Item.AppearanceNormal.ForeColor = Color.White;
                e.Item.AppearanceHover.ForeColor = Color.Gray;
            }
        }

        private void gvDocuments_ContextButtonClick(object sender, DevExpress.Utils.ContextItemClickEventArgs e)
        {
            try
            {
                var focusedRowHandle = gvDocuments.FocusedRowHandle;
                if (focusedRowHandle < 0)
                {
                    return;
                }

                var document = gvDocuments.GetRow(focusedRowHandle) as InvoiceDocumentModel;
                if (document == null || string.IsNullOrWhiteSpace(document.OriginPath))
                {
                    return;
                }

                var result = XtraMessageBox.Show(
                    $"Are you sure you want to delete '{document.DocumentName}'?\n\nThis will permanently delete the file from disk.",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);

                if (result != DialogResult.Yes)
                {
                    return;
                }

                // Delete the physical file
                if (File.Exists(document.OriginPath))
                {
                    File.Delete(document.OriginPath);
                }

                // Remove from the binding list
                _invoiceDocuments.RemoveAt(focusedRowHandle);

                XtraMessageBox.Show("Document deleted successfully.", "Delete Document", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (UnauthorizedAccessException ex)
            {
                XtraMessageBox.Show($"Access denied: {ex.Message}\n\nPlease check file permissions.", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException ex)
            {
                XtraMessageBox.Show($"File error: {ex.Message}\n\nThe file may be in use by another application.", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Error deleting document: {ex.Message}", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gvDocuments_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            gvDocuments.RefreshContextButtons();
        }

        private void gvDocuments_DoubleClick(object sender, EventArgs e)
        {
            OpenSelectedDocument();
        }

        private void OpenSelectedDocument()
        {
            try
            {
                var document = gvDocuments.GetFocusedRow() as InvoiceDocumentModel;
                if (document == null || string.IsNullOrWhiteSpace(document.OriginPath) || !File.Exists(document.OriginPath))
                {
                    return;
                }

                Process.Start(document.OriginPath);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Open Document", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task SaveInvoiceAsync()
        {
            try
            {
                gvInvoiceDetails.PostEditor();
                gvInvoiceDetails.UpdateCurrentRow();

                BindingContext[bsInvoice].EndCurrentEdit();
                BindingContext[bsInvoiceDetails].EndCurrentEdit();

                _invoiceModel = bsInvoice.Current as InvoiceModel;
                if (_invoiceModel == null)
                    throw new InvalidOperationException("Invoice data is not available for saving.");

                SyncInvoiceDetailsFromBinding();
                PersistDocumentLinks();

                var isNewInvoice = string.IsNullOrWhiteSpace(_invoiceModel._id);

                if (isNewInvoice)
                {
                    _logInfoRepository.CreateLogInfo(_invoiceModel);
                    await _invoiceRepository.AddNewInvoiceAsync(_invoiceModel);
                }
                else
                {
                    _logInfoRepository.UpdateLogInfo(_invoiceModel);
                    await _invoiceRepository.UpdateInvoiceAsync(_invoiceModel);
                }

                SendUpdatedInvoice?.Invoke(_invoiceModel, EventArgs.Empty);
                XtraMessageBox.Show("Invoice saved successfully.", "Save Invoice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Save Invoice", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PersistDocumentLinks()
        {
            if (_invoiceModel == null) return;

            _invoiceModel.DocumentLink = string.Join(";",
                _invoiceDocuments.Where(x => x != null && !string.IsNullOrWhiteSpace(x.OriginPath))
                    .Select(x => x.OriginPath)
                    .Distinct(StringComparer.OrdinalIgnoreCase));
        }


        #region UI Helper Methods

        private void ShowError(string message, string title = "Error")
        {
            XtraMessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private bool ConfirmAction(string message, string title)
        {
            return XtraMessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) == DialogResult.Yes;
        }

        #endregion

        private void gvInvoiceDetails_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (!CanManageInvoice())
            {
                e.Menu = null;
                return;
            }

            if (e.MenuType != GridMenuType.Row && e.MenuType != GridMenuType.User) return;

            var view = sender as GridView;
            if (view == null) return;

            if (e.HitInfo.InRow || e.HitInfo.InRowCell)
            {
                view.FocusedRowHandle = e.HitInfo.RowHandle;
            }

            if (e.Menu == null)
            {
                e.Menu = new GridViewMenu(view);
            }

            e.Menu.Items.Clear();
            foreach (var menuItem in _menuItems)
            {
                e.Menu.Items.Add(menuItem);
            }
        }

        private bool CanManageInvoice()
        {
            if (gvInvoiceDetails.OptionsBehavior.ReadOnly || !gvInvoiceDetails.OptionsBehavior.Editable)
            {
                return false;
            }

            if (!ValidateData() || string.IsNullOrWhiteSpace(_invoiceModel?._id))
            {
                XtraMessageBox.Show("Finish invoice first, then add details.", "Invoice",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return true;
        }
    }
}