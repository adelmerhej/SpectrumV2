using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.WinExplorer;
using Spectrum.DataLayers.DataAccess;
using Spectrum.DataLayers.Members.Clients;
using Spectrum.Models.Accounting.Invoices;
using Spectrum.Models.Members.Clients;
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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.Transactions.Invoices
{
    public partial class InvoiceEditForm : RibbonForm
    {
        private InvoiceModel _invoiceModel = new InvoiceModel();
        private IList<ClientModel> _clients = new List<ClientModel>();
        private BindingList<InvoiceDocumentModel> _invoiceDocuments = new BindingList<InvoiceDocumentModel>();

        private readonly ClientRepository _clientRepository = new ClientRepository(DatabaseFactory.ProfilePrimary);

        private readonly LogInfoRepository _logInfoRepository = new LogInfoRepository();

        //Init permissionvariables
        private bool _canAdd = true;
        private bool _canEdit = true;
        private bool _canDelete = true;
        private bool _canPrint = true;
        private bool _isAdmin = true;
        private bool _isProtected = true;

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
                ApplyDefaults();
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
                    LoadDocumentsAsync()
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

        #endregion
        private void WireUpBindings()
        {
            //cboMemebrs.Properties.DataSource = null;
            //cboMemebrs.Properties.DisplayMember = "ClientName";
            //cboMemebrs.Properties.ValueMember = "_id";
            //cboMemebrs.Properties.DataSource = _clients;
        }

        private void ApplyDefaults()
        {
            gvDocuments.GetThumbnailImage += TileView1_GetThumbnailImage;
            gvDocuments.OptionsImageLoad.RandomShow = true;
            gvDocuments.OptionsImageLoad.LoadThumbnailImagesFromDataSource = false;
            gvDocuments.OptionsImageLoad.AsyncLoad = true;

            tabInvoiceGroup.SelectedTabPage = tabInvoice;
        }

        private void ApplyPermissions()
        {
            btnNew.Enabled = _isAdmin || _canAdd;
            btnSave.Enabled = _isAdmin || _canEdit;
            btnSaveAndNew.Enabled = _isAdmin || _canEdit;
            btnPrint.Enabled = _isAdmin || _canPrint;
            btnDelete.Enabled = _isAdmin || _canDelete;
        }

        private void InitializeMenuItems()
        {
            //var itemNew = new DXMenuItem("New", ItemNew_Click);
            //var itemEdit = new DXMenuItem("Edit", ItemEdit_Click);
            //var itemDelete = new DXMenuItem("Delete", ItemDelete_Click);
            //_menuItems = new[] { itemNew, itemEdit, itemDelete };
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

        #endregion

        #region Buttons Events Handlers

        private void btnNew_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
        private void btnDuplicate_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void btnSave_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void btnSaveAndNew_ItemClick(object sender, ItemClickEventArgs e)
        {

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

        private void PersistDocumentLinks()
        {
            if (_invoiceModel == null) return;

            _invoiceModel.DocumentLink = string.Join(";",
                _invoiceDocuments.Where(x => x != null && !string.IsNullOrWhiteSpace(x.OriginPath))
                    .Select(x => x.OriginPath)
                    .Distinct(StringComparer.OrdinalIgnoreCase));
        }
    }
}