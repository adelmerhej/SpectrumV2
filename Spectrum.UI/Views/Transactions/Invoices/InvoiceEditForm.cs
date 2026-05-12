using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using Spectrum.DataLayers.DataAccess;
using Spectrum.DataLayers.Members.Clients;
using Spectrum.Models.Members.Clients;
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
		private readonly ClientRepository _clientRepository = new ClientRepository(DatabaseFactory.ProfilePrimary);
		private IList<ClientModel> _clients = new List<ClientModel>();

		public InvoiceEditForm()
		{
			InitializeComponent();
			StartLoading();
		}

		private async void StartLoading()
		{
			try
			{
				await LoadClientsAsync();
				WireUpBindings();
			}
			catch (Exception ex)
			{
				XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private async Task LoadClientsAsync()
		{
			_clients = await _clientRepository.GetClientsAsync();
		}

		private void WireUpBindings()
		{
			cboMemebrs.Properties.DataSource = null;
			cboMemebrs.Properties.DisplayMember = "ClientName";
			cboMemebrs.Properties.ValueMember = "_id";
			cboMemebrs.Properties.DataSource = _clients;
		}



		#region Buttons Events Handlers

		private void btnNew_ItemClick(object sender, ItemClickEventArgs e)
		{

		}

		private void btnSave_ItemClick(object sender, ItemClickEventArgs e)
		{

		}

		private void btnSaveAndClose_ItemClick(object sender, ItemClickEventArgs e)
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