using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using SpectrumV1.DataLayers.DataAccess;
using SpectrumV1.DataLayers.Members.Engineers;
using SpectrumV1.Models.Members.Engineers;
using SpectrumV1.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpectrumV1.Views.Members.Engineers
{
	public partial class EngineersListForm : RibbonForm, IFormWithRibbon
	{
		private EngineerModel _engineerModel = new EngineerModel();
		private IList<EngineerModel> _engineers = new List<EngineerModel>();

		private readonly EngineerRepository _engineerRepository = new EngineerRepository(DatabaseFactory.ProfilePrimary);

		// Init permission variables
		private bool _canAdd = true;
		private bool _canEdit = true;
		private bool _canDelete = true;
		private bool _canPrint = true;
		private bool _isAdmin = true;
		private bool _isProtected = true;

		#region Implementation of IFormWithRibbon

		public RibbonControl MainRibbon => rcEngineers;
		public RibbonPage DefaultPage => rpEngineers;

		#endregion

		public EngineersListForm()
		{
			InitializeComponent();

			// wire events
			btnNew.ItemClick += btnNew_ItemClick;
			btnEdit.ItemClick += btnEdit_ItemClick;
			btnDelete.ItemClick += btnDelete_ItemClick;
			btnPrint.ItemClick += btnPrint_ItemClick;
			btnRefresh.ItemClick += btnRefresh_ItemClick;
			btnClose.ItemClick += btnClose_ItemClick;
			gvEngineers.DoubleClick += gvEngineers_DoubleClick;
			gvEngineers.RowCellStyle += gvEngineers_RowCellStyle;

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
				// Load permissions here if needed, following app conventions
				_engineers = await _engineerRepository.GetEngineersAsync();
			}
			catch (Exception ex)
			{
				XtraMessageBox.Show(ex.Message, @"Error Loading", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void WireUpBindings()
		{
			gcEngineers.DataSource = null;
			gcEngineers.DataSource = _engineers;
			barHeaderItem1.Caption = $"RECORDS: {_engineers.Count}";
		}

		private void ApplyDefaults()
		{
		}

		private void ApplyPermissions()
		{
			btnNew.Enabled = _isAdmin || _canAdd;
			btnEdit.Enabled = _isAdmin || _canEdit;
			btnPrint.Enabled = _isAdmin || _canPrint;
			btnDelete.Enabled = _isAdmin || _canDelete;
		}

		#region Buttons Events

		private void btnNew_ItemClick(object sender, ItemClickEventArgs e)
		{
			var frm = new EngineerEditForm(new EngineerModel());
			frm.SendUpdatedEngineer += RcvUpdatedEngineerAsync;
			frm.Show();
		}

		private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!_engineers.Any()) return;

			try
			{
				var currentRowIdObj = gvEngineers.GetFocusedRowCellValue("_id");
				if (currentRowIdObj == null) return;
				string currentRowId = currentRowIdObj.ToString();
				if (string.IsNullOrEmpty(currentRowId)) return;

				_engineerModel = _engineers.SingleOrDefault(x => x._id == currentRowId);
				if (_engineerModel == null) return;

				var frm = new EngineerEditForm(_engineerModel);
				frm.SendUpdatedEngineer += RcvUpdatedEngineerAsync;
				frm.Show();
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
			gcEngineers.ShowRibbonPrintPreview();
		}

		private async void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!CanDelete()) return;

			try
			{
				string id = gvEngineers.GetFocusedRowCellValue("_id").ToString();
				string name = gvEngineers.GetFocusedRowCellValue("EngineerName").ToString();

				if (!string.IsNullOrEmpty(id))
				{
					if (XtraMessageBox.Show($"Are you sure you want to delete Record: `{name}`?",
							"Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
							MessageBoxDefaultButton.Button2) == DialogResult.Yes)
					{
						_engineerModel = gvEngineers.GetFocusedRow() as EngineerModel;
						if (_engineerModel == null)
						{
							return;
						}
						_engineerModel.Deleted = true;

						await _engineerRepository.DeleteEngineerAsync(_engineerModel._id);
						RcvUpdatedEngineerAsync(_engineerModel, EventArgs.Empty);
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
			// reset settings if needed
		}

		#endregion

		private async void RcvUpdatedEngineerAsync(object sender, EventArgs e)
		{
			if (sender == null) return;
			_engineerModel = sender as EngineerModel;

			if (_engineerModel != null && (_engineerModel.LastModifiedDate == null || _engineerModel.Deleted))
			{
				await InitializeBindings();
				WireUpBindings();
			}
			else
			{
				gvEngineers.UpdateCurrentRow();
			}
		}

		private void gvEngineers_DoubleClick(object sender, EventArgs e)
		{
			if (!_engineers.Any()) return;

			try
			{
				var currentRowIdObj = gvEngineers.GetFocusedRowCellValue("_id");
				if (currentRowIdObj == null) return;
				string currentRowId = currentRowIdObj.ToString();
				if (string.IsNullOrEmpty(currentRowId)) return;

				_engineerModel = _engineers.SingleOrDefault(x => x._id == currentRowId);
				if (_engineerModel == null) return;

				var frm = new EngineerEditForm(_engineerModel);
				frm.SendUpdatedEngineer += RcvUpdatedEngineerAsync;
				frm.Show();
			}
			catch (Exception exception)
			{
				XtraMessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private bool CanDelete()
		{
			EngineerModel dataBoundItem = gvEngineers.GetFocusedRow() as EngineerModel;

			if (gvEngineers == null || gvEngineers.SelectedRowsCount == 0) return false;
			if (gvEngineers.SelectedRowsCount > 1)
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

		private void gvEngineers_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
		{
			GridView view = sender as GridView;
			if (e.RowHandle >= 0)
			{
				bool isActive = false;
				bool isDefault = false;
				var activeObj = view.GetRowCellValue(e.RowHandle, "Active");
				var defaultObj = view.GetRowCellValue(e.RowHandle, "IsDefault");
				if (activeObj is bool) isActive = (bool)activeObj;
				if (defaultObj is bool) isDefault = (bool)defaultObj;

				if (isDefault)
				{
					e.Appearance.Font = new Font("Tahoma", 8, FontStyle.Bold);
				}
				if (!isActive)
				{
					e.Appearance.ForeColor = Color.Gray;
					e.Appearance.Font = new Font("Tahoma", 8, FontStyle.Italic);
				}
			}
		}
	}
}