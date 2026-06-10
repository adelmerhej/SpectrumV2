using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using Spectrum.DataLayers.Accounting.CashRegister;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Accounting.CashRegister;
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

namespace Spectrum.Views.Accounting.CashRegister
{
    public partial class CashRegistersListForm : RibbonForm, IFormWithRibbon
    {
        private bool _resetMenu;
        private CashRegisterEditForm _cashRegisterEditForm;

        private CashRegisterModel _cashRegisterModel = new CashRegisterModel();
        private IList<CashRegisterModel> _cashRegisters = new List<CashRegisterModel>();

        private readonly CashRegisterRepository _cashRegisterRepository = new CashRegisterRepository(DatabaseFactory.ProfilePrimary);

        //Init permissionvariables
        private bool _canAdd = true;
        private bool _canEdit = true;
        private bool _canDelete = true;
        private bool _canPrint = true;
        private bool _isAdmin = true;
        private bool _isProtected = true;

        #region Implementation of IFormWithRibbon

        public RibbonControl MainRibbon => rcCashRegister;
        public RibbonPage DefaultPage => rpCashRegister;



        #endregion
        public CashRegistersListForm()
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

                _cashRegisters = await _cashRegisterRepository.GetCashRegistersAsync();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WireUpBindings()
        {
            gcCashRegisters.DataSource = null;
            gcCashRegisters.DataSource = _cashRegisters;
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
            ShowCashRegisterEditor(new CashRegisterModel());
        }

        private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!_cashRegisters.Any()) return;

            try
            {
                string currentRowId = gvCashRegisters.GetFocusedRowCellValue("_id").ToString();
                if (string.IsNullOrEmpty(currentRowId)) return;

                _cashRegisterModel = _cashRegisters.SingleOrDefault(x => x._id == currentRowId);
                if (_cashRegisterModel == null) return;

                ShowCashRegisterEditor(_cashRegisterModel);
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
            gcCashRegisters.ShowRibbonPrintPreview();
        }

        private async void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!CanDelete()) return;

            try
            {
                string id = gvCashRegisters.GetFocusedRowCellValue("_id").ToString();
                string name = gvCashRegisters.GetFocusedRowCellValue("Name").ToString();

                if (!string.IsNullOrEmpty(id))
                {
                    if (XtraMessageBox.Show($"Are you sure you want to delete Record: `{name}`?",
                            "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        _cashRegisterModel = gvCashRegisters.GetFocusedRow() as CashRegisterModel;
                        if (_cashRegisterModel == null)
                        {
                            return;
                        }
                        _cashRegisterModel.Deleted = true;

                        //delete the record
                        await _cashRegisterRepository.DeleteCashRegisterAsync(_cashRegisterModel._id);
                        RcvUpdatedCashRegisterAsync(_cashRegisterModel, EventArgs.Empty);
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
                LayoutsStyle.ResetLayoutGrid(gvCashRegisters, CurrentUser.UserName, CurrentUser.Company);
            }
        }


        #endregion

        private async void RcvUpdatedCashRegisterAsync(object sender, EventArgs e)
        {
            if (sender == null) return;
            _cashRegisterModel = sender as CashRegisterModel;
            if (_cashRegisterModel == null) return;

            if (_cashRegisterModel.Deleted || _cashRegisterModel.LastModifiedDate == null)
            {
                await InitializeBindings();
                WireUpBindings();
            }
            else
            {
                gcCashRegisters.RefreshDataSource();
                gvCashRegisters.RefreshRow(gvCashRegisters.FocusedRowHandle);
                gvCashRegisters.UpdateCurrentRow();
            }
        }

        private bool CanDelete()
        {
            CashRegisterModel dataBoundItem = gvCashRegisters.GetFocusedRow() as CashRegisterModel;

            if (gvCashRegisters == null || gvCashRegisters.SelectedRowsCount == 0) return false;
            if (gvCashRegisters.SelectedRowsCount > 1)
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

        private void ShowCashRegisterEditor(CashRegisterModel model)
        {
            if (_cashRegisterEditForm == null || _cashRegisterEditForm.IsDisposed)
            {
                _cashRegisterEditForm = new CashRegisterEditForm(model);
                _cashRegisterEditForm.SendUpdatedCashRegister += RcvUpdatedCashRegisterAsync;
                _cashRegisterEditForm.FormClosed += CashRegisterEditForm_FormClosed;
                _cashRegisterEditForm.Show(this);
                return;
            }

            if (_cashRegisterEditForm.WindowState == FormWindowState.Minimized)
                _cashRegisterEditForm.WindowState = FormWindowState.Normal;

            _cashRegisterEditForm.Activate();
            _cashRegisterEditForm.BringToFront();
        }

        private void CashRegisterEditForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            var form = sender as CashRegisterEditForm;
            if (form != null)
            {
                form.SendUpdatedCashRegister -= RcvUpdatedCashRegisterAsync;
                form.FormClosed -= CashRegisterEditForm_FormClosed;
            }
            if (ReferenceEquals(_cashRegisterEditForm, sender))
                _cashRegisterEditForm = null;
        }

        private void gvCashRegisters_DoubleClick(object sender, EventArgs e)
        {
            if (!_cashRegisters.Any()) return;

            try
            {
                string currentRowId = gvCashRegisters.GetFocusedRowCellValue("_id").ToString();
                if (string.IsNullOrEmpty(currentRowId)) return;

                _cashRegisterModel = _cashRegisters.SingleOrDefault(x => x._id == currentRowId);
                if (_cashRegisterModel == null) return;

                ShowCashRegisterEditor(_cashRegisterModel);
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gvCashRegisters_RowCellStyle(object sender, RowCellStyleEventArgs e)
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