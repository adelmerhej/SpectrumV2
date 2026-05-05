using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Users;
using Spectrum.Utilities;
using Spectrum.Utilities.Interfaces;
using Spectrum.Utilities.Layout;
using SpectrumV1.DataLayers.Accounting.CostCenter;
using SpectrumV1.Models.Accounting.CostCenter;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.Accounting.CostCenter
{
    public partial class CostCentersListForm : RibbonForm, IFormWithRibbon
    {
        private bool _resetMenu;
        private CostCenterEditForm _costCenterEditForm;

        private CostCenterModel _costCenterModel = new CostCenterModel();
        private IList<CostCenterModel> _costCenters = new List<CostCenterModel>();

        private readonly CostCenterRepository _costCenterRepository = new CostCenterRepository(DatabaseFactory.ProfilePrimary);

        //Init permissionvariables
        private bool _canAdd = true;
        private bool _canEdit = true;
        private bool _canDelete = true;
        private bool _canPrint = true;
        private bool _isAdmin = true;
        private bool _isProtected = true;

        #region Implementation of IFormWithRibbon

        public RibbonControl MainRibbon => rcCostCenters;
        public RibbonPage DefaultPage => rpCostCenters;


        #endregion

        public CostCentersListForm()
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

                _costCenters = await _costCenterRepository.GetCostCentersAsync();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WireUpBindings()
        {
            gcCostCenters.DataSource = null;
            gcCostCenters.DataSource = _costCenters;
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
            ShowBankEditor(new CostCenterModel());
        }

        private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!_costCenters.Any()) return;

            try
            {
                string currentRowId = gvCostCenters.GetFocusedRowCellValue("_id").ToString();
                if (string.IsNullOrEmpty(currentRowId)) return;

                _costCenterModel = _costCenters.SingleOrDefault(x => x._id == currentRowId);
                if (_costCenterModel == null) return;

                ShowBankEditor(_costCenterModel);
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
            gcCostCenters.ShowRibbonPrintPreview();
        }

        private async void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!CanDelete()) return;

            try
            {
                string id = gvCostCenters.GetFocusedRowCellValue("_id").ToString();
                string name = gvCostCenters.GetFocusedRowCellValue("CostCenterName").ToString();

                if (!string.IsNullOrEmpty(id))
                {
                    if (XtraMessageBox.Show($"Are you sure you want to delete Record: `{name}`?",
                            "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        _costCenterModel = gvCostCenters.GetFocusedRow() as CostCenterModel;
                        if (_costCenterModel == null)
                        {
                            return;
                        }
                        _costCenterModel.Deleted = true;

                        //delete the record
                        await _costCenterRepository.DeleteCostCenterAsync(_costCenterModel._id);
                        RcvUpdatedBankAsync(_costCenterModel, EventArgs.Empty);
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
                LayoutsStyle.ResetLayoutGrid(gvCostCenters, CurrentUser.UserName, CurrentUser.Company);
            }
        }


        #endregion

        private async void RcvUpdatedBankAsync(object sender, EventArgs e)
        {
            if (sender == null) return;
            _costCenterModel = sender as CostCenterModel;
            if (_costCenterModel == null) return;

            if (_costCenterModel.Deleted || _costCenterModel.LastModifiedDate == null)
            {
                await InitializeBindings();
                WireUpBindings();
            }
            else
            {
                gcCostCenters.RefreshDataSource();
                gvCostCenters.RefreshRow(gvCostCenters.FocusedRowHandle);
                gvCostCenters.UpdateCurrentRow();
            }
        }

        private bool CanDelete()
        {
            CostCenterModel dataBoundItem = gvCostCenters.GetFocusedRow() as CostCenterModel;

            if (gvCostCenters == null || gvCostCenters.SelectedRowsCount == 0) return false;
            if (gvCostCenters.SelectedRowsCount > 1)
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

        private void ShowBankEditor(CostCenterModel model)
        {
            if (_costCenterEditForm == null || _costCenterEditForm.IsDisposed)
            {
                _costCenterEditForm = new CostCenterEditForm(model);
                _costCenterEditForm.SendUpdatedCostCenter += RcvUpdatedBankAsync;
                _costCenterEditForm.FormClosed += CostCenterEditForm_FormClosed;
                _costCenterEditForm.Show(this);
                return;
            }

            if (_costCenterEditForm.WindowState == FormWindowState.Minimized)
                _costCenterEditForm.WindowState = FormWindowState.Normal;

            _costCenterEditForm.Activate();
            _costCenterEditForm.BringToFront();
        }

        private void CostCenterEditForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            var form = sender as CostCenterEditForm;
            if (form != null)
            {
                form.SendUpdatedCostCenter -= RcvUpdatedBankAsync;
                form.FormClosed -= CostCenterEditForm_FormClosed;
            }
            if (ReferenceEquals(_costCenterEditForm, sender))
                _costCenterEditForm = null;
        }

        private void gvJournalTypes_DoubleClick(object sender, EventArgs e)
        {
            if (!_costCenters.Any()) return;

            try
            {
                string currentRowId = gvCostCenters.GetFocusedRowCellValue("_id").ToString();
                if (string.IsNullOrEmpty(currentRowId)) return;

                _costCenterModel = _costCenters.SingleOrDefault(x => x._id == currentRowId);
                if (_costCenterModel == null) return;

                ShowBankEditor(_costCenterModel);
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gvJournalTypes_RowCellStyle(object sender, RowCellStyleEventArgs e)
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