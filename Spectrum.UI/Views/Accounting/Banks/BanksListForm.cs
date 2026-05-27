using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using Spectrum.Models.Users;
using Spectrum.Utilities;
using Spectrum.Utilities.Interfaces;
using Spectrum.Utilities.Layout;
using Spectrum.Models.Accounting.Banks;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Spectrum.DataLayers.Accounting.Banks;
using Spectrum.DataLayers.DataAccess;

namespace Spectrum.Views.Accounting.Banks
{
    public partial class BanksListForm : RibbonForm, IFormWithRibbon
    {
        private bool _resetMenu;
        private BankEditForm _bankEditForm;

        private BankModel _bankModel = new BankModel();
        private IList<BankModel> _banks = new List<BankModel>();

        private readonly BankRepository _bankRepository = new BankRepository(DatabaseFactory.ProfilePrimary);

        //Init permissionvariables
        private bool _canAdd = true;
        private bool _canEdit = true;
        private bool _canDelete = true;
        private bool _canPrint = true;
        private bool _isAdmin = true;
        private bool _isProtected = true;

        #region Implementation of IFormWithRibbon

        public RibbonControl MainRibbon => rcBanks;
        public RibbonPage DefaultPage => rpBanks;


        #endregion

        public BanksListForm()
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

                _banks = await _bankRepository.GetBanksAsync();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WireUpBindings()
        {
            gcBanks.DataSource = null;
            gcBanks.DataSource = _banks;
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
            ShowBankEditor(new BankModel());
        }

        private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!_banks.Any()) return;

            try
            {
                string currentRowId = gvBanks.GetFocusedRowCellValue("_id").ToString();
                if (string.IsNullOrEmpty(currentRowId)) return;

                _bankModel = _banks.SingleOrDefault(x => x._id == currentRowId);
                if (_bankModel == null) return;

                ShowBankEditor(_bankModel);
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
            gcBanks.ShowRibbonPrintPreview();
        }

        private async void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!CanDelete()) return;

            try
            {
                string id = gvBanks.GetFocusedRowCellValue("_id").ToString();
                string name = gvBanks.GetFocusedRowCellValue("BankName").ToString();

                if (!string.IsNullOrEmpty(id))
                {
                    if (XtraMessageBox.Show($"Are you sure you want to delete Record: `{name}`?",
                            "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        _bankModel = gvBanks.GetFocusedRow() as BankModel;
                        if (_bankModel == null)
                        {
                            return;
                        }
                        _bankModel.Deleted = true;

                        //delete the record
                        await _bankRepository.DeleteBankAsync(_bankModel._id);
                        RcvUpdatedBankAsync(_bankModel, EventArgs.Empty);
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
                LayoutsStyle.ResetLayoutGrid(gvBanks, CurrentUser.UserName, CurrentUser.Company);
            }

        }

        #endregion

        private void gvBanks_DoubleClick(object sender, EventArgs e)
        {
            if (!_banks.Any()) return;

            try
            {
                string currentRowId = gvBanks.GetFocusedRowCellValue("_id").ToString();
                if (string.IsNullOrEmpty(currentRowId)) return;

                _bankModel = _banks.SingleOrDefault(x => x._id == currentRowId);
                if (_bankModel == null) return;

                ShowBankEditor(_bankModel);
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void RcvUpdatedBankAsync(object sender, EventArgs e)
        {
            if (sender == null) return;
            _bankModel = sender as BankModel;
            if (_bankModel == null) return;

            if (_bankModel.Deleted || _bankModel.LastModifiedDate == null)
            {
                await InitializeBindings();
                WireUpBindings();
            }
            else
            {
                gcBanks.RefreshDataSource();
                gvBanks.RefreshRow(gvBanks.FocusedRowHandle);
                gvBanks.UpdateCurrentRow();
            }
        }

        private bool CanDelete()
        {
            BankModel dataBoundItem = gvBanks.GetFocusedRow() as BankModel;

            if (gvBanks == null || gvBanks.SelectedRowsCount == 0) return false;
            if (gvBanks.SelectedRowsCount > 1)
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

        private void ShowBankEditor(BankModel model)
        {
            if (_bankEditForm == null || _bankEditForm.IsDisposed)
            {
                _bankEditForm = new BankEditForm(model);
                _bankEditForm.SendUpdatedBank += RcvUpdatedBankAsync;
                _bankEditForm.FormClosed += BankEditForm_FormClosed;
                _bankEditForm.Show(this);
                return;
            }

            if (_bankEditForm.WindowState == FormWindowState.Minimized)
                _bankEditForm.WindowState = FormWindowState.Normal;

            _bankEditForm.Activate();
            _bankEditForm.BringToFront();
        }

        private void BankEditForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            var form = sender as BankEditForm;
            if (form != null)
            {
                form.SendUpdatedBank -= RcvUpdatedBankAsync;
                form.FormClosed -= BankEditForm_FormClosed;
            }
            if (ReferenceEquals(_bankEditForm, sender))
                _bankEditForm = null;
        }

        private void gvBanks_RowCellStyle(object sender, RowCellStyleEventArgs e)
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