using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Members.Suppliers;
using Spectrum.Models.Users;
using Spectrum.Utilities;
using Spectrum.Utilities.Interfaces;
using Spectrum.Utilities.Layout;
using Spectrum.DataLayers.Members.Suppliers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.Members.Suppliers
{
    public partial class SuppliersListForm : RibbonForm, IFormWithRibbon
    {
        private bool _resetMenu;
        private SupplierEditForm _supplierEditForm;

        private SupplierModel _supplierModel = new SupplierModel();
        private IList<SupplierModel> _suppliers = new List<SupplierModel>();

        private readonly SupplierRepository _supplierRepository = new SupplierRepository(DatabaseFactory.ProfilePrimary);

        //Init permissionvariables
        private bool _canAdd = true;
        private bool _canEdit = true;
        private bool _canDelete = true;
        private bool _canPrint = true;
        private bool _isAdmin = true;
        private bool _isProtected = true;

        #region Implementation of IFormWithRibbon

        public RibbonControl MainRibbon => rcSuppliers;
        public RibbonPage DefaultPage => rpSuppliers;


        #endregion


        public SuppliersListForm()
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

                _suppliers = await _supplierRepository.GetSuppliersAsync();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WireUpBindings()
        {
            gcSuppliers.DataSource = null;
            gcSuppliers.DataSource = _suppliers;
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


        #region Butons Events

        private void btnNew_ItemClick(object sender, ItemClickEventArgs e)
        {
            ShowSupplierEditor(new SupplierModel());
        }

        private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!_suppliers.Any()) return;

            try
            {
                string currentRowId = gvSuppliers.GetFocusedRowCellValue("_id").ToString();
                if (string.IsNullOrEmpty(currentRowId)) return;

                _supplierModel = _suppliers.SingleOrDefault(x => x._id == currentRowId);
                if (_supplierModel == null) return;

                ShowSupplierEditor(_supplierModel);
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
            gcSuppliers.ShowRibbonPrintPreview();
        }

        private async void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!CanDelete()) return;

            try
            {
                string id = gvSuppliers.GetFocusedRowCellValue("_id").ToString();
                string name = gvSuppliers.GetFocusedRowCellValue("SupplierName").ToString();

                if (!string.IsNullOrEmpty(id))
                {
                    if (XtraMessageBox.Show($"Are you sure you want to delete Record: `{name}`?",
                            "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        _supplierModel = gvSuppliers.GetFocusedRow() as SupplierModel;
                        if (_supplierModel == null)
                        {
                            return;
                        }
                        _supplierModel.Deleted = true;

                        //delete the record
                        await _supplierRepository.DeleteSupplierAsync(_supplierModel._id);
                        RcvUpdatedSupplierAsync(_supplierModel, EventArgs.Empty);
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
                LayoutsStyle.ResetLayoutGrid(gvSuppliers, CurrentUser.UserName, CurrentUser.Company);
            }

        }

        #endregion

        private void gvSuppliers_DoubleClick(object sender, EventArgs e)
        {
            if (!_suppliers.Any()) return;

            try
            {
                string currentRowId = gvSuppliers.GetFocusedRowCellValue("_id").ToString();
                if (string.IsNullOrEmpty(currentRowId)) return;

                _supplierModel = _suppliers.SingleOrDefault(x => x._id == currentRowId);
                if (_supplierModel == null) return;

                ShowSupplierEditor(_supplierModel);
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void RcvUpdatedSupplierAsync(object sender, EventArgs e)
        {
            if (sender == null) return;
            _supplierModel = sender as SupplierModel;
            if (_supplierModel == null) return;

            if (_supplierModel.Deleted || _supplierModel.LastModifiedDate == null)
            {
                await InitializeBindings();
                WireUpBindings();
            }
            else
            {
                gcSuppliers.RefreshDataSource();
                gvSuppliers.RefreshRow(gvSuppliers.FocusedRowHandle);
                gvSuppliers.UpdateCurrentRow();
            }
        }

        private bool CanDelete()
        {
            SupplierModel dataBoundItem = gvSuppliers.GetFocusedRow() as SupplierModel;

            if (gvSuppliers == null || gvSuppliers.SelectedRowsCount == 0) return false;
            if (gvSuppliers.SelectedRowsCount > 1)
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

        private void ShowSupplierEditor(SupplierModel model)
        {
            if (_supplierEditForm == null || _supplierEditForm.IsDisposed)
            {
                _supplierEditForm = new SupplierEditForm(model);
                _supplierEditForm.SendUpdatedSupplier += RcvUpdatedSupplierAsync;
                _supplierEditForm.FormClosed += SupplierEditForm_FormClosed;
                _supplierEditForm.Show(this);
                return;
            }

            if (_supplierEditForm.WindowState == FormWindowState.Minimized)
                _supplierEditForm.WindowState = FormWindowState.Normal;

            _supplierEditForm.Activate();
            _supplierEditForm.BringToFront();
        }

        private void SupplierEditForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            var form = sender as SupplierEditForm;
            if (form != null)
            {
                form.SendUpdatedSupplier -= RcvUpdatedSupplierAsync;
                form.FormClosed -= SupplierEditForm_FormClosed;
            }
            if (ReferenceEquals(_supplierEditForm, sender))
                _supplierEditForm = null;
        }

        private void gvSuppliers_RowCellStyle(object sender, RowCellStyleEventArgs e)
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