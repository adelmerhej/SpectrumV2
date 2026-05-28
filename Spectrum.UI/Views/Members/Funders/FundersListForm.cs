using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using Spectrum.DataLayers.DataAccess;
using Spectrum.DataLayers.Members.Funders;
using Spectrum.Models.Members.Funders;
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

namespace Spectrum.Views.Members.Funders
{
    public partial class FundersListForm : RibbonForm, IFormWithRibbon
    {
        private bool _resetMenu;
        private FunderEditForm _funderEditForm;

        private FunderModel _funderModel = new FunderModel();
        private IList<FunderModel> _funders = new List<FunderModel>();

        private readonly FunderRepository _funderRepository = new FunderRepository(DatabaseFactory.ProfilePrimary);

        //Init permissionvariables
        private bool _canAdd = true;
        private bool _canEdit = true;
        private bool _canDelete = true;
        private bool _canPrint = true;
        private bool _isAdmin = true;
        private bool _isProtected = true;

        #region Implementation of IFormWithRibbon

        public RibbonControl MainRibbon => rcFunders;
        public RibbonPage DefaultPage => rpFunders;


        #endregion

        public FundersListForm()
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

                _funders = await _funderRepository.GetFundersAsync();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WireUpBindings()
        {
            gcFunders.DataSource = null;
            gcFunders.DataSource = _funders;
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
            ShowFunderEditor(new FunderModel());
        }

        private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!_funders.Any()) return;

            try
            {
                string currentRowId = gvFunders.GetFocusedRowCellValue("_id").ToString();
                if (string.IsNullOrEmpty(currentRowId)) return;

                _funderModel = _funders.SingleOrDefault(x => x._id == currentRowId);
                if (_funderModel == null) return;

                ShowFunderEditor(_funderModel);
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
            gcFunders.ShowRibbonPrintPreview();
        }

        private async void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!CanDelete()) return;

            try
            {
                string id = gvFunders.GetFocusedRowCellValue("_id").ToString();
                string name = gvFunders.GetFocusedRowCellValue("FunderName").ToString();

                if (!string.IsNullOrEmpty(id))
                {
                    if (XtraMessageBox.Show($"Are you sure you want to delete Record: `{name}`?",
                            "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        _funderModel = gvFunders.GetFocusedRow() as FunderModel;
                        if (_funderModel == null)
                        {
                            return;
                        }
                        _funderModel.Deleted = true;

                        //delete the record
                        await _funderRepository.DeleteFunderAsync(_funderModel._id);
                        RcvUpdatedFunderAsync(_funderModel, EventArgs.Empty);
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
                LayoutsStyle.ResetLayoutGrid(gvFunders, CurrentUser.UserName, CurrentUser.Company);
            }

        }

        #endregion

        private void gvFunders_DoubleClick(object sender, EventArgs e)
        {
            if (!_funders.Any()) return;

            try
            {
                string currentRowId = gvFunders.GetFocusedRowCellValue("_id").ToString();
                if (string.IsNullOrEmpty(currentRowId)) return;

                _funderModel = _funders.SingleOrDefault(x => x._id == currentRowId);
                if (_funderModel == null) return;

                ShowFunderEditor(_funderModel);
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void RcvUpdatedFunderAsync(object sender, EventArgs e)
        {
            if (sender == null) return;
            _funderModel = sender as FunderModel;
            if (_funderModel == null) return;

            if (_funderModel.Deleted || _funderModel.LastModifiedDate == null)
            {
                await InitializeBindings();
                WireUpBindings();
            }
            else
            {
                gcFunders.RefreshDataSource();
                gvFunders.RefreshRow(gvFunders.FocusedRowHandle);
                gvFunders.UpdateCurrentRow();
            }
        }

        private bool CanDelete()
        {
            FunderModel dataBoundItem = gvFunders.GetFocusedRow() as FunderModel;

            if (gvFunders == null || gvFunders.SelectedRowsCount == 0) return false;
            if (gvFunders.SelectedRowsCount > 1)
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

        private void ShowFunderEditor(FunderModel model)
        {
            if (_funderEditForm == null || _funderEditForm.IsDisposed)
            {
                _funderEditForm = new FunderEditForm(model);
                _funderEditForm.SendUpdatedFunder += RcvUpdatedFunderAsync;
                _funderEditForm.FormClosed += FunderEditForm_FormClosed;
                _funderEditForm.Show(this);
                return;
            }

            if (_funderEditForm.WindowState == FormWindowState.Minimized)
                _funderEditForm.WindowState = FormWindowState.Normal;

            _funderEditForm.Activate();
            _funderEditForm.BringToFront();
        }

        private void FunderEditForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            var form = sender as FunderEditForm;
            if (form != null)
            {
                form.SendUpdatedFunder -= RcvUpdatedFunderAsync;
                form.FormClosed -= FunderEditForm_FormClosed;
            }
            if (ReferenceEquals(_funderEditForm, sender))
                _funderEditForm = null;
        }

        private void gvFunders_RowCellStyle(object sender, RowCellStyleEventArgs e)
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