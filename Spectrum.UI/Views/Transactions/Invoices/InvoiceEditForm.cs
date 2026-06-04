using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using Spectrum.DataLayers.DataAccess;
using Spectrum.DataLayers.Members.Clients;
using Spectrum.Models.Members.Clients;
using Spectrum.Models.Projects;
using Spectrum.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
                    LoadClientsAsync()
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


    }
}