using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using SpectrumV1.DataLayers.DataAccess;
using SpectrumV1.DataLayers.Members.Clients;
using SpectrumV1.Models.Members.Clients;
using SpectrumV1.Utilities;
using SpectrumV1.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpectrumV1.Views.Members.Clients
{
	public partial class ClientsListForm : RibbonForm, IFormWithRibbon
	{
		private bool _resetMenu;
		private ClientModel _clientModel = new ClientModel();
		private IList<ClientModel> _clients = new List<ClientModel>();
		private ClientEditForm _clientEditForm;

		private readonly ClientRepository _clientRepository = new ClientRepository(DatabaseFactory.ProfilePrimary);

		//Init permission variables
		private bool _canAdd = true;
		private bool _canEdit = true;
		private bool _canDelete = true;
		private bool _canPrint = true;
		private bool _isAdmin = true;
		private bool _isProtected = true;
		private bool _isLimitedView = true;

		#region Implementation of IFormWithRibbon

		public RibbonControl MainRibbon => rcCustomersList;
		public RibbonPage DefaultPage => rpCustomersList;


		#endregion

		public ClientsListForm()
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
			gvClients.DoubleClick += gvClients_DoubleClick;
			gvClients.RowCellStyle += gvClients_RowCellStyle;

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
				//	//
				//	_formId = _formRepository.SelectFormByName(_formName);
				//	_userPermission = _userPermissionRepository.SelectUserPermissionById(CurrentUser.UserId, _formId);
				//	if (_userPermission is { Count: > 0 })
				//	{
				//		var isProtected = _userPermission.SingleOrDefault(x => x.ControlName == "IsProtected")?.Value;
				//		if (isProtected != null) _isProtected = (bool)isProtected;
				//	}
				//	//

				_clients = await _clientRepository.GetClientsAsync();
			}
			catch (Exception ex)
			{
				XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void WireUpBindings()
		{
			gcClients.DataSource = null;
			gcClients.DataSource = _clients;
		}

		private void ApplyDefaults()
		{

		}

		private void ApplyPermissions()
		{
			//if (_userPermission == null) return;
			//if (_userPermission.Count <= 0) return;

			//var canAdd = _userPermission.SingleOrDefault(x => x.ControlName == "CanAdd")?.Value;
			//if (canAdd != null) _canAdd = (bool)canAdd;

			//var canEdit = _userPermission.SingleOrDefault(x => x.ControlName == "CanEdit")?.Value;
			//if (canEdit != null) _canEdit = (bool)canEdit;

			//var canDelete = _userPermission.SingleOrDefault(x => x.ControlName == "CanDelete")?.Value;
			//if (canDelete != null) _canDelete = (bool)canDelete;

			//var canPrint = _userPermission.SingleOrDefault(x => x.ControlName == "CanPrint")?.Value;
			//if (canPrint != null) _canPrint = (bool)canPrint;

			//var isAdmin = _userPermission.SingleOrDefault(x => x.ControlName == "IsAdmin")?.Value;
			//if (isAdmin != null) _isAdmin = (bool)isAdmin;

			btnNew.Enabled = _isAdmin || _canAdd;
			btnEdit.Enabled = _isAdmin || _canEdit;
			btnPrint.Enabled = _isAdmin || _canPrint;
			btnDelete.Enabled = _isAdmin || _canDelete;
		}

		#region Buttons Events

		private void btnNew_ItemClick(object sender, ItemClickEventArgs e)
		{
			ShowClientEditor(new ClientModel());
		}

		private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!_clients.Any()) return;

			try
			{
				string currentRowId = gvClients.GetFocusedRowCellValue("_id").ToString();
				if (string.IsNullOrEmpty(currentRowId)) return;

				_clientModel = _clients.SingleOrDefault(x => x._id == currentRowId);
				if (_clientModel == null) return;

				ShowClientEditor(_clientModel);
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
			gcClients.ShowRibbonPrintPreview();
		}

		private async void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!CanDelete()) return;

			try
			{
				string id = gvClients.GetFocusedRowCellValue("_id").ToString();
				string name = gvClients.GetFocusedRowCellValue("ClientName").ToString();

				if (!string.IsNullOrEmpty(id))
				{
					if (XtraMessageBox.Show($"Are you sure you want to delete Record: `{name}`?",
							"Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
							MessageBoxDefaultButton.Button2) == DialogResult.Yes)
					{
						_clientModel = gvClients.GetFocusedRow() as ClientModel;
						if (_clientModel == null)
						{
							return;
						}
						_clientModel.Deleted = true;

						//delete the record
						await _clientRepository.DeleteClientAsync(_clientModel._id);
						RcvUpdatedClientAsync(_clientModel, EventArgs.Empty);
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

		}

		#endregion


		private async void RcvUpdatedClientAsync(object sender, EventArgs e)
		{
			if (sender == null) return;
			_clientModel = sender as ClientModel;

			if (_clientModel != null && (_clientModel.LastModifiedDate == null || _clientModel.Deleted))
			{
				await InitializeBindings();
				WireUpBindings();
			}
			else
			{
				gvClients.UpdateCurrentRow();
			}
		}

		private void gvClients_DoubleClick(object sender, EventArgs e)
		{
			if (!_clients.Any()) return;

			try
			{
				string currentRowId = gvClients.GetFocusedRowCellValue("_id").ToString();
				if (string.IsNullOrEmpty(currentRowId)) return;

				_clientModel = _clients.SingleOrDefault(x => x._id == currentRowId);
				if (_clientModel == null) return;

				ShowClientEditor(_clientModel);
			}
			catch (Exception exception)
			{
				XtraMessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void ShowClientEditor(ClientModel model)
		{
			if (_clientEditForm == null || _clientEditForm.IsDisposed)
			{
				_clientEditForm = new ClientEditForm(model);
				_clientEditForm.SendUpdatedClient += RcvUpdatedClientAsync;
				_clientEditForm.FormClosed += ClientEditForm_FormClosed;
				_clientEditForm.Show(this);
				return;
			}

			if (_clientEditForm.WindowState == FormWindowState.Minimized)
				_clientEditForm.WindowState = FormWindowState.Normal;

			_clientEditForm.Activate();
			_clientEditForm.BringToFront();
		}

		private void ClientEditForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			var form = sender as ClientEditForm;
			if (form != null)
			{
				form.SendUpdatedClient -= RcvUpdatedClientAsync;
				form.FormClosed -= ClientEditForm_FormClosed;
			}
			if (ReferenceEquals(_clientEditForm, sender))
				_clientEditForm = null;
		}

		private bool CanDelete()
		{
			ClientModel dataBoundItem = gvClients.GetFocusedRow() as ClientModel;

			if (gvClients == null || gvClients.SelectedRowsCount == 0) return false;
			if (gvClients.SelectedRowsCount > 1)
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

		private void gvClients_RowCellStyle(object sender, RowCellStyleEventArgs e)
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