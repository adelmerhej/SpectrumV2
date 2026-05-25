using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using Spectrum.DataLayers.DataAccess;
using Spectrum.DataLayers.HumanResources.JobPositions;
using Spectrum.Models.HumanResources.JobPositions;
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

namespace Spectrum.Views.HumanResources.JobPositions
{
	public partial class JobPositionsListForm : RibbonForm, IFormWithRibbon
	{
		private bool _resetMenu;
		private JobPositionEditForm _jobPositionEditForm;

		private JobPositionModel _jobPositionModel = new JobPositionModel();
		private IList<JobPositionModel> _jobPositions = new List<JobPositionModel>();

		private readonly JobPositionRepository _jobPositionRepository = new JobPositionRepository(DatabaseFactory.ProfilePrimary);

		//Init permissionvariables
		private bool _canAdd = true;
		private bool _canEdit = true;
		private bool _canDelete = true;
		private bool _canPrint = true;
		private bool _isAdmin = true;
		private bool _isProtected = true;

		#region Implementation of IFormWithRibbon

		public RibbonControl MainRibbon => rcJobPositions;
		public RibbonPage DefaultPage => rpJobPositions;


		#endregion

		public JobPositionsListForm()
		{
			InitializeComponent();

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

				_jobPositions = await _jobPositionRepository.GetJobPositionsAsync();
			}
			catch (Exception ex)
			{
				XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void WireUpBindings()
		{
			gcJobPositions.DataSource = null;
			gcJobPositions.DataSource = _jobPositions;
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

		#region Buttons EventHandlers

		private void btnNew_ItemClick(object sender, ItemClickEventArgs e)
		{
			ShowJobPositionEditor(new JobPositionModel());
		}

		private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!_jobPositions.Any()) return;

			try
			{
				string currentRowId = gvJobPositions.GetFocusedRowCellValue("_id").ToString();
				if (string.IsNullOrEmpty(currentRowId)) return;

				_jobPositionModel = _jobPositions.SingleOrDefault(x => x._id == currentRowId);
				if (_jobPositionModel == null) return;

				ShowJobPositionEditor(_jobPositionModel);
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
			gcJobPositions.ShowRibbonPrintPreview();
		}

		private async void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!CanDelete()) return;

			try
			{
				string id = gvJobPositions.GetFocusedRowCellValue("_id").ToString();
				string name = gvJobPositions.GetFocusedRowCellValue("JobPositionName").ToString();

				if (!string.IsNullOrEmpty(id))
				{
					if (XtraMessageBox.Show($"Are you sure you want to delete Record: `{name}`?",
							"Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
							MessageBoxDefaultButton.Button2) == DialogResult.Yes)
					{
						_jobPositionModel = gvJobPositions.GetFocusedRow() as JobPositionModel;
						if (_jobPositionModel == null)
						{
							return;
						}
						_jobPositionModel.Deleted = true;

						//delete the record
						await _jobPositionRepository.DeleteJobPositionAsync(_jobPositionModel._id);
						RcvUpdatedJobPositionAsync(_jobPositionModel, EventArgs.Empty);
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
				LayoutsStyle.ResetLayoutGrid(gvJobPositions, CurrentUser.UserName, CurrentUser.Company);
			}
		}

		#endregion

		private void gvJobPositions_DoubleClick(object sender, EventArgs e)
		{
			if (!_jobPositions.Any()) return;

			try
			{
				string currentRowId = gvJobPositions.GetFocusedRowCellValue("_id").ToString();
				if (string.IsNullOrEmpty(currentRowId)) return;

				_jobPositionModel = _jobPositions.SingleOrDefault(x => x._id == currentRowId);
				if (_jobPositionModel == null) return;

				ShowJobPositionEditor(_jobPositionModel);
			}
			catch (Exception exception)
			{
				XtraMessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private async void RcvUpdatedJobPositionAsync(object sender, EventArgs e)
		{
			if (sender == null) return;
			_jobPositionModel = sender as JobPositionModel;
			if (_jobPositionModel == null) return;

			if (_jobPositionModel.Deleted || _jobPositionModel.LastModifiedDate == null)
			{
				await InitializeBindings();
				WireUpBindings();
			}
			else
			{
				gcJobPositions.RefreshDataSource();
				gvJobPositions.RefreshRow(gvJobPositions.FocusedRowHandle);
				gvJobPositions.UpdateCurrentRow();
			}
		}

		private bool CanDelete()
		{
			JobPositionModel dataBoundItem = gvJobPositions.GetFocusedRow() as JobPositionModel;

			if (gvJobPositions == null || gvJobPositions.SelectedRowsCount == 0) return false;
			if (gvJobPositions.SelectedRowsCount > 1)
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

		private void ShowJobPositionEditor(JobPositionModel model)
		{
			if (_jobPositionEditForm == null || _jobPositionEditForm.IsDisposed)
			{
				_jobPositionEditForm = new JobPositionEditForm(model);
				_jobPositionEditForm.SendUpdatedJobPosition += RcvUpdatedJobPositionAsync;
				_jobPositionEditForm.FormClosed += JobPositionEditForm_FormClosed;
				_jobPositionEditForm.Show(this);
				return;
			}

			if (_jobPositionEditForm.WindowState == FormWindowState.Minimized)
				_jobPositionEditForm.WindowState = FormWindowState.Normal;

			_jobPositionEditForm.Activate();
			_jobPositionEditForm.BringToFront();
		}

		private void JobPositionEditForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			var form = sender as JobPositionEditForm;
			if (form != null)
			{
				form.SendUpdatedJobPosition -= RcvUpdatedJobPositionAsync;
				form.FormClosed -= JobPositionEditForm_FormClosed;
			}
			if (ReferenceEquals(_jobPositionEditForm, sender))
				_jobPositionEditForm = null;
		}

		private void gvJobPositions_RowCellStyle(object sender, RowCellStyleEventArgs e)
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