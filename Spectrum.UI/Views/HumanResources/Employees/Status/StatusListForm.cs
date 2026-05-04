using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Users;
using Spectrum.Utilities;
using Spectrum.Utilities.Interfaces;
using Spectrum.Utilities.Layout;
using SpectrumV1.DataLayers.HumanResources.Employees.EmployeeStatus;
using SpectrumV1.Models.HumanResources.Employees.EmployeeStatus;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.HumanResources.Employees.Status
{
    public partial class StatusListForm : RibbonForm, IFormWithRibbon
    {
        private bool _resetMenu;
        private StatusEditForm _statusEditForm;

        private StatusModel _statusModel = new StatusModel();
        private IList<StatusModel> _status = new List<StatusModel>();

        private readonly StatusRepository _statusRepository = new StatusRepository(DatabaseFactory.ProfilePrimary);

        //Init permissionvariables
        private bool _canAdd = true;
        private bool _canEdit = true;
        private bool _canDelete = true;
        private bool _canPrint = true;
        private bool _isAdmin = true;
        private bool _isProtected = true;

        #region Implementation of IFormWithRibbon

        public RibbonControl MainRibbon => rcStatus;
        public RibbonPage DefaultPage => rpStatus;


        #endregion


        public StatusListForm()
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
            gvStatus.DoubleClick += gvStatus_DoubleClick;
            gvStatus.RowCellStyle += gvStatus_RowCellStyle;

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

                _status = await _statusRepository.GetStatusAsync();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WireUpBindings()
        {
            gcStatus.DataSource = null;
            gcStatus.DataSource = _status;
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

        #region Buttons Event

        private void btnNew_ItemClick(object sender, ItemClickEventArgs e)
        {
            ShowStatusEditor(new StatusModel());
        }

        private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!_status.Any()) return;

            try
            {
                string currentRowId = gvStatus.GetFocusedRowCellValue("_id").ToString();
                if (string.IsNullOrEmpty(currentRowId)) return;

                _statusModel = _status.SingleOrDefault(x => x._id == currentRowId);
                if (_statusModel == null) return;

                ShowStatusEditor(_statusModel);
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
            gcStatus.ShowRibbonPrintPreview();
        }

        private async void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!CanDelete()) return;

            try
            {
                string id = gvStatus.GetFocusedRowCellValue("_id").ToString();
                string name = gvStatus.GetFocusedRowCellValue("Status").ToString();

                if (!string.IsNullOrEmpty(id))
                {
                    if (XtraMessageBox.Show($"Are you sure you want to delete: `{name}`?",
                            "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        _statusModel = gvStatus.GetFocusedRow() as StatusModel;
                        if (_statusModel == null)
                        {
                            return;
                        }
                        _statusModel.Deleted = true;

                        //delete the record
                        bool deleted = await _statusRepository.DeleteStatusAsync(id);
                        if (!deleted) return;
                        RcvUpdatedStatusAsync(_statusModel, EventArgs.Empty);
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
                LayoutsStyle.ResetLayoutGrid(gvStatus, CurrentUser.UserName, CurrentUser.Company);
            }
        }

        #endregion

        private void gvStatus_DoubleClick(object sender, EventArgs e)
        {
            if (!_status.Any()) return;

            try
            {
                string currentRowId = gvStatus.GetFocusedRowCellValue("_id").ToString();
                if (string.IsNullOrEmpty(currentRowId)) return;

                _statusModel = _status.SingleOrDefault(x => x._id == currentRowId);
                if (_statusModel == null) return;

                ShowStatusEditor(_statusModel);
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void RcvUpdatedStatusAsync(object sender, EventArgs e)
        {
            if (sender == null) return;
            _statusModel = sender as StatusModel;
            if (_statusModel == null) return;

            if (_statusModel.Deleted || _statusModel.LastModifiedDate == null)
            {
                await InitializeBindings();
                WireUpBindings();
            }
            else
            {
                gcStatus.RefreshDataSource();
                gvStatus.RefreshRow(gvStatus.FocusedRowHandle);
                gvStatus.UpdateCurrentRow();
            }
        }

        private bool CanDelete()
        {
            StatusModel dataBoundItem = gvStatus.GetFocusedRow() as StatusModel;

            if (gvStatus == null || gvStatus.SelectedRowsCount == 0) return false;
            if (gvStatus.SelectedRowsCount > 1)
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

        private void ShowStatusEditor(StatusModel model)
        {
            if (_statusEditForm == null || _statusEditForm.IsDisposed)
            {
                _statusEditForm = new StatusEditForm(model);
                _statusEditForm.SendUpdatedStatus += RcvUpdatedStatusAsync;
                _statusEditForm.FormClosed += StatusEditForm_FormClosed;
                _statusEditForm.Show(this);
                return;
            }

            if (_statusEditForm.WindowState == FormWindowState.Minimized)
                _statusEditForm.WindowState = FormWindowState.Normal;

            _statusEditForm.Activate();
            _statusEditForm.BringToFront();
        }

        private void StatusEditForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            var form = sender as StatusEditForm;
            if (form != null)
            {
                form.SendUpdatedStatus -= RcvUpdatedStatusAsync;
                form.FormClosed -= StatusEditForm_FormClosed;
            }
            if (ReferenceEquals(_statusEditForm, sender))
                _statusEditForm = null;
        }

        private void gvStatus_RowCellStyle(object sender, RowCellStyleEventArgs e)
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