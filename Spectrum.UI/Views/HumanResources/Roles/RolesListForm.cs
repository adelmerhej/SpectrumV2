using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Users;
using Spectrum.Utilities;
using Spectrum.Utilities.Interfaces;
using Spectrum.Utilities.Layout;
using Spectrum.DataLayers.Common.Roles;
using Spectrum.Models.Common.Roles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.HumanResources.Roles
{
    public partial class RolesListForm : RibbonForm, IFormWithRibbon
    {
        private bool _resetMenu;
        private RoleEditForm _roleEditForm;

        private RoleModel _roleModel = new RoleModel();
        private IList<RoleModel> _roles = new List<RoleModel>();

        private readonly RoleRepository _roleRepository = new RoleRepository(DatabaseFactory.ProfilePrimary);

        //Init permissionvariables
        private bool _canAdd = true;
        private bool _canEdit = true;
        private bool _canDelete = true;
        private bool _canPrint = true;
        private bool _isAdmin = true;
        private bool _isProtected = true;


        #region Implementation of IFormWithRibbon

        public RibbonControl MainRibbon => rcRolesList;
        public RibbonPage DefaultPage => rpRolesList;


        #endregion

        public RolesListForm()
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

                _roles = await _roleRepository.GetRolesAsync();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WireUpBindings()
        {
            gcRoles.DataSource = null;
            gcRoles.DataSource = _roles;
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

        #region Button Events

        private void btnNew_ItemClick(object sender, ItemClickEventArgs e)
        {
            ShowRoleEditor(new RoleModel());
        }

        private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!_roles.Any()) return;

            try
            {
                string currentRowId = gvRoles.GetFocusedRowCellValue("_id").ToString();
                if (string.IsNullOrEmpty(currentRowId)) return;

                _roleModel = _roles.SingleOrDefault(x => x._id == currentRowId);
                if (_roleModel == null) return;

                ShowRoleEditor(_roleModel);
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
            gcRoles.ShowRibbonPrintPreview();
        }

        private async void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!CanDelete()) return;

            try
            {
                string id = gvRoles.GetFocusedRowCellValue("_id").ToString();
                string name = gvRoles.GetFocusedRowCellValue("RoleName").ToString();

                if (!string.IsNullOrEmpty(id))
                {
                    if (XtraMessageBox.Show($"Are you sure you want to delete Record: `{name}`?",
                            "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        _roleModel = gvRoles.GetFocusedRow() as RoleModel;
                        if (_roleModel == null)
                        {
                            return;
                        }
                        _roleModel.Deleted = true;

                        //delete the record
                        await _roleRepository.DeleteRoleAsync(_roleModel._id);
                        RcvUpdatedRoleAsync(_roleModel, EventArgs.Empty);
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
                LayoutsStyle.ResetLayoutGrid(gvRoles, CurrentUser.UserName, CurrentUser.Company);
            }
        }

        #endregion

        private void gvRoles_DoubleClick(object sender, EventArgs e)
        {
            if (!_roles.Any()) return;

            try
            {
                string currentRowId = gvRoles.GetFocusedRowCellValue("_id").ToString();
                if (string.IsNullOrEmpty(currentRowId)) return;

                _roleModel = _roles.SingleOrDefault(x => x._id == currentRowId);
                if (_roleModel == null) return;

                ShowRoleEditor(_roleModel);
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void RcvUpdatedRoleAsync(object sender, EventArgs e)
        {
            if (sender == null) return;
            _roleModel = sender as RoleModel;
            if (_roleModel == null) return;

            if (_roleModel.Deleted || _roleModel.LastModifiedDate == null)
            {
                await InitializeBindings();
                WireUpBindings();
            }
            else
            {
                gcRoles.RefreshDataSource();
                gvRoles.RefreshRow(gvRoles.FocusedRowHandle);
                gvRoles.UpdateCurrentRow();
            }
        }

        private bool CanDelete()
        {
            RoleModel dataBoundItem = gvRoles.GetFocusedRow() as RoleModel;

            if (gvRoles == null || gvRoles.SelectedRowsCount == 0) return false;
            if (gvRoles.SelectedRowsCount > 1)
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

        private void ShowRoleEditor(RoleModel model)
        {
            if (_roleEditForm == null || _roleEditForm.IsDisposed)
            {
                _roleEditForm = new RoleEditForm(model);
                _roleEditForm.SendUpdatedRole += RcvUpdatedRoleAsync;
                _roleEditForm.FormClosed += RoleEditForm_FormClosed;
                _roleEditForm.Show(this);
                return;
            }

            if (_roleEditForm.WindowState == FormWindowState.Minimized)
                _roleEditForm.WindowState = FormWindowState.Normal;

            _roleEditForm.Activate();
            _roleEditForm.BringToFront();
        }

        private void RoleEditForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            var form = sender as RoleEditForm;
            if (form != null)
            {
                form.SendUpdatedRole -= RcvUpdatedRoleAsync;
                form.FormClosed -= RoleEditForm_FormClosed;
            }
            if (ReferenceEquals(_roleEditForm, sender))
                _roleEditForm = null;
        }

        private void gvRoles_RowCellStyle(object sender, RowCellStyleEventArgs e)
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