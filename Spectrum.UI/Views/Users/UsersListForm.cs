using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using Spectrum.DataLayers.DataAccess;
using Spectrum.DataLayers.Users;
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

namespace Spectrum.Views.Users
{
	public partial class UsersListForm : RibbonForm, IFormWithRibbon
	{
		private bool _resetMenu;
		private UserEditForm _userEditForm;

		private UserModel _userModel = new UserModel();
		private IList<UserModel> _users = new List<UserModel>();

		private readonly UserRepository _userRepository = new UserRepository(DatabaseFactory.ProfilePrimary);

		//Init permissionvariables
		private bool _canAdd = true;
		private bool _canEdit = true;
		private bool _canDelete = true;
		private bool _canPrint = true;
		private bool _isAdmin = true;
		private bool _isProtected = true;

		#region Implementation of IFormWithRibbon

		public RibbonControl MainRibbon => rcUsersList;
		public RibbonPage DefaultPage => rpUsersList;


		#endregion

		public UsersListForm()
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

				_users = await _userRepository.GetUsersAsync();
			}
			catch (Exception ex)
			{
				XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void WireUpBindings()
		{
			gcUsers.DataSource = null;
			gcUsers.DataSource = _users;
		}

		private void ApplyDefaults()
		{
			//lnkAbout.Text = $@"About {Settings.Default.ApplicationName.Trim()}";
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

		private void UsersListForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!_resetMenu)
			{
				LayoutsStyle.SaveLayoutGrid(gvUsers, CurrentUser.UserName, CurrentUser.Company);
			}
		}

		#region Buttons Events

		private void btnNew_ItemClick(object sender, ItemClickEventArgs e)
		{
			ShowUserEditor(new UserModel());
		}

		private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!_users.Any()) return;

			try
			{
				string currentRowId = gvUsers.GetFocusedRowCellValue("_id").ToString();
				if (string.IsNullOrEmpty(currentRowId)) return;

				_userModel = _users.SingleOrDefault(x => x._id == currentRowId);
				if (_userModel == null) return;

				ShowUserEditor(_userModel);
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
			gcUsers.ShowRibbonPrintPreview();
		}

		private async void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!CanDelete()) return;

			try
			{
				string id = gvUsers.GetFocusedRowCellValue("_id").ToString();
				string name = gvUsers.GetFocusedRowCellValue("UserName").ToString();

				if (!string.IsNullOrEmpty(id))
				{
					if (XtraMessageBox.Show($"Are you sure you want to delete Record: `{name}`?",
							"Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
							MessageBoxDefaultButton.Button2) == DialogResult.Yes)
					{
						_userModel = gvUsers.GetFocusedRow() as UserModel;
						if (_userModel == null)
						{
							return;
						}
						_userModel.Deleted = true;

						//delete the record
						await _userRepository.DeleteUserAsync(_userModel._id);
						RcvUpdatedUserAsync(_userModel, EventArgs.Empty);
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
			if (XtraMessageBox.Show("This will reset Grid layout next login, to its default settings.\nAre you sure you want to continue?", "Reset Menu...",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) ==
				DialogResult.Yes)
			{
				_resetMenu = true;
				LayoutsStyle.ResetLayoutGrid(gvUsers, CurrentUser.UserName, CurrentUser.Company);
			}
		}

		#endregion

		private void gvUsers_DoubleClick(object sender, EventArgs e)
		{
			if (!_users.Any()) return;

			try
			{
				string currentRowId = gvUsers.GetFocusedRowCellValue("_id").ToString();
				if (string.IsNullOrEmpty(currentRowId)) return;

				_userModel = _users.SingleOrDefault(x => x._id == currentRowId);
				if (_userModel == null) return;

				ShowUserEditor(_userModel);
			}
			catch (Exception exception)
			{
				XtraMessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private async void RcvUpdatedUserAsync(object sender, EventArgs e)
		{
			if (sender == null) return;
			_userModel = sender as UserModel;

			if (_userModel != null && (_userModel.LastModifiedDate == null || _userModel.Deleted))
			{
				await InitializeBindings();
				WireUpBindings();
			}
			else
			{
				gvUsers.UpdateCurrentRow();
			}
		}

		private bool CanDelete()
		{
			UserModel dataBoundItem = gvUsers.GetFocusedRow() as UserModel;

			if (gvUsers == null || gvUsers.SelectedRowsCount == 0) return false;
			if (gvUsers.SelectedRowsCount > 1)
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


		private void ShowUserEditor(UserModel model)
		{
			if (_userEditForm == null || _userEditForm.IsDisposed)
			{
				_userEditForm = new UserEditForm(model);
				_userEditForm.SendUpdatedUser += RcvUpdatedUserAsync;
				_userEditForm.FormClosed += UserEditForm_FormClosed;
				_userEditForm.Show(this);
				return;
			}

			if (_userEditForm.WindowState == FormWindowState.Minimized)
				_userEditForm.WindowState = FormWindowState.Normal;

			_userEditForm.Activate();
			_userEditForm.BringToFront();
		}

		private void UserEditForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			var form = sender as UserEditForm;
			if (form != null)
			{
				form.SendUpdatedUser -= RcvUpdatedUserAsync;
				form.FormClosed -= UserEditForm_FormClosed;
			}
			if (ReferenceEquals(_userEditForm, sender))
				_userEditForm = null;
		}

		private void gvUsers_RowCellStyle(object sender, RowCellStyleEventArgs e)
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