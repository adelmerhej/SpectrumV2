using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using Spectrum.DataLayers.Common.Branches;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Common.Companies;
using Spectrum.Models.Users;
using Spectrum.Utilities;
using Spectrum.Utilities.Interfaces;
using Spectrum.Utilities.Layout;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.Common.Companies
{
	public partial class BranchesListForm : RibbonForm, IFormWithRibbon
	{
		private bool _resetMenu;
		private BranchEditForm _branchEditForm;

		private BranchModel _branchModel = new BranchModel();
		private IList<BranchModel> _branches = new List<BranchModel>();

		private readonly BranchRepository _branchRepository = new BranchRepository(DatabaseFactory.ProfilePrimary);

		//Init permissionvariables
		private bool _canAdd = true;
		private bool _canEdit = true;
		private bool _canDelete = true;
		private bool _canPrint = true;
		private bool _isAdmin = true;
		private bool _isProtected = true;

		#region Implementation of IFormWithRibbon

		public RibbonControl MainRibbon => rcBranchesList;
		public RibbonPage DefaultPage => rpBranchesList;


		#endregion

		public BranchesListForm()
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
			gvBranches.DoubleClick += gvBranches_DoubleClick;
			gvBranches.RowCellStyle += gvBranches_RowCellStyle;

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

				_branches = await _branchRepository.GetBranchesAsync();
			}
			catch (Exception ex)
			{
				XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void WireUpBindings()
		{
			gcBranches.DataSource = null;
			gcBranches.DataSource = _branches;
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
			ShowBranchEditor(new BranchModel());
		}

		private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!_branches.Any()) return;

			try
			{
				string currentRowId = gvBranches.GetFocusedRowCellValue("_id").ToString();
				if (string.IsNullOrEmpty(currentRowId)) return;

				_branchModel = _branches.SingleOrDefault(x => x._id == currentRowId);
				if (_branchModel == null) return;

				ShowBranchEditor(_branchModel);
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
			gcBranches.ShowRibbonPrintPreview();
		}

		private async void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!CanDelete()) return;

			try
			{
				string id = gvBranches.GetFocusedRowCellValue("_id").ToString();
				string name = gvBranches.GetFocusedRowCellValue("BranchName").ToString();

				if (!string.IsNullOrEmpty(id))
				{
					if (XtraMessageBox.Show($"Are you sure you want to delete Record: `{name}`?",
							"Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
							MessageBoxDefaultButton.Button2) == DialogResult.Yes)
					{
						_branchModel = gvBranches.GetFocusedRow() as BranchModel;
						if (_branchModel == null)
						{
							return;
						}
						_branchModel.Deleted = true;

						//delete the record
						await _branchRepository.DeleteBranchAsync(_branchModel._id);
						RcvUpdatedBranchAsync(_branchModel, EventArgs.Empty);
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
			if (XtraMessageBox.Show("This will reset Grid layout next login, to its default settings.\nAre you sure you want to continue?", "Reset Menu...",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) ==
				DialogResult.Yes)
			{
				_resetMenu = true;
				LayoutsStyle.ResetLayoutGrid(gvBranches, CurrentUser.UserName, CurrentUser.Company);
			}

		}

		#endregion


		private async void RcvUpdatedBranchAsync(object sender, EventArgs e)
		{
			if (sender == null) return;
			_branchModel = sender as BranchModel;

			if (_branchModel != null && (_branchModel.LastModifiedDate == null || _branchModel.Deleted))
			{
				await InitializeBindings();
				WireUpBindings();
			}
			else
			{
				gvBranches.UpdateCurrentRow();
			}
		}

		private void gvBranches_DoubleClick(object sender, EventArgs e)
		{
			if (!_branches.Any()) return;

			try
			{
				string currentRowId = gvBranches.GetFocusedRowCellValue("_id").ToString();
				if (string.IsNullOrEmpty(currentRowId)) return;

				_branchModel = _branches.SingleOrDefault(x => x._id == currentRowId);
				if (_branchModel == null) return;

				ShowBranchEditor(_branchModel);
			}
			catch (Exception exception)
			{
				XtraMessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private bool CanDelete()
		{
			BranchModel dataBoundItem = gvBranches.GetFocusedRow() as BranchModel;

			if (gvBranches == null || gvBranches.SelectedRowsCount == 0) return false;
			if (gvBranches.SelectedRowsCount > 1)
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

		private void ShowBranchEditor(BranchModel model)
		{
			if (_branchEditForm == null || _branchEditForm.IsDisposed)
			{
				_branchEditForm = new BranchEditForm(model);
				_branchEditForm.SendUpdatedBranch += RcvUpdatedBranchAsync;
				_branchEditForm.FormClosed += BranchEditForm_FormClosed;
				_branchEditForm.Show(this);
				return;
			}

			if (_branchEditForm.WindowState == FormWindowState.Minimized)
				_branchEditForm.WindowState = FormWindowState.Normal;

			_branchEditForm.Activate();
			_branchEditForm.BringToFront();
		}

		private void BranchEditForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			var form = sender as BranchEditForm;
			if (form != null)
			{
				form.SendUpdatedBranch -= RcvUpdatedBranchAsync;
				form.FormClosed -= BranchEditForm_FormClosed;
			}
			if (ReferenceEquals(_branchEditForm, sender))
				_branchEditForm = null;
		}

		private void gvBranches_RowCellStyle(object sender, RowCellStyleEventArgs e)
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