using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using Spectrum.DataLayers.Accounting.FinancialGroup;
using Spectrum.DataLayers.Accounting.FlowType;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Accounting.FinancialGroup;
using Spectrum.Models.Accounting.FlowType;
using Spectrum.Models.Users;
using Spectrum.Utilities;
using Spectrum.Utilities.Interfaces;
using Spectrum.Utilities.Layout;
using Spectrum.Views.Accounting.FlowType;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.Accounting.FinancialGroup
{
    public partial class FinancialGroupListForm : RibbonForm, IFormWithRibbon
    {
        private bool _resetMenu;
        private FinancialGroupEditForm _financialGroupEditForm;

        private FinancialGroupModel _financialGroupModel = new FinancialGroupModel();
        private IList<FinancialGroupModel> _financialGroups = new List<FinancialGroupModel>();

        private readonly FinancialGroupRepository _financialGroupRepository = new FinancialGroupRepository(DatabaseFactory.ProfilePrimary);

        //Init permissionvariables
        private bool _canAdd = true;
        private bool _canEdit = true;
        private bool _canDelete = true;
        private bool _canPrint = true;
        private bool _isAdmin = true;
        private bool _isProtected = true;

        #region Implementation of IFormWithRibbon

        public RibbonControl MainRibbon => rcFinancialGroups;
        public RibbonPage DefaultPage => rpFinancialGroups;


        #endregion

        public FinancialGroupListForm()
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

                _financialGroups = await _financialGroupRepository.GetFinancialGroupsAsync();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WireUpBindings()
        {
            gcFinancialGroups.DataSource = null;
            gcFinancialGroups.DataSource = _financialGroups;
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
            ShowFinancialGroupEditor(new FinancialGroupModel());
        }

        private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!_financialGroups.Any()) return;

            try
            {
                string currentRowId = gvFinancialGroups.GetFocusedRowCellValue("_id").ToString();
                if (string.IsNullOrEmpty(currentRowId)) return;

                _financialGroupModel = _financialGroups.SingleOrDefault(x => x._id == currentRowId);
                if (_financialGroupModel == null) return;

                ShowFinancialGroupEditor(_financialGroupModel);
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
            gcFinancialGroups.ShowRibbonPrintPreview();
        }

        private async void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!CanDelete()) return;

            try
            {
                string id = gvFinancialGroups.GetFocusedRowCellValue("_id").ToString();
                string name = gvFinancialGroups.GetFocusedRowCellValue("FinancialGroupName").ToString();

                if (!string.IsNullOrEmpty(id))
                {
                    if (XtraMessageBox.Show($"Are you sure you want to delete Record: `{name}`?",
                            "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        _financialGroupModel = gvFinancialGroups.GetFocusedRow() as FinancialGroupModel;
                        if (_financialGroupModel == null)
                        {
                            return;
                        }
                        _financialGroupModel.Deleted = true;

                        //delete the record
                        await _financialGroupRepository.DeleteFinancialGroupAsync(_financialGroupModel._id);
                        RcvUpdatedFinancialGroupAsync(_financialGroupModel, EventArgs.Empty);
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
                LayoutsStyle.ResetLayoutGrid(gvFinancialGroups, CurrentUser.UserName, CurrentUser.Company);
            }
        }


        #endregion

        private async void RcvUpdatedFinancialGroupAsync(object sender, EventArgs e)
        {
            if (sender == null) return;
            _financialGroupModel = sender as FinancialGroupModel;
            if (_financialGroupModel == null) return;

            if (_financialGroupModel.Deleted || _financialGroupModel.LastModifiedDate == null)
            {
                await InitializeBindings();
                WireUpBindings();
            }
            else
            {
                gcFinancialGroups.RefreshDataSource();
                gvFinancialGroups.RefreshRow(gvFinancialGroups.FocusedRowHandle);
                gvFinancialGroups.UpdateCurrentRow();
            }
        }

        private bool CanDelete()
        {
            FinancialGroupModel dataBoundItem = gvFinancialGroups.GetFocusedRow() as FinancialGroupModel;

            if (gvFinancialGroups == null || gvFinancialGroups.SelectedRowsCount == 0) return false;
            if (gvFinancialGroups.SelectedRowsCount > 1)
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

        private void ShowFinancialGroupEditor(FinancialGroupModel model)
        {
            if (_financialGroupEditForm == null || _financialGroupEditForm.IsDisposed)
            {
                _financialGroupEditForm = new FinancialGroupEditForm(model);
                _financialGroupEditForm.SendUpdatedFinancialGroup += RcvUpdatedFinancialGroupAsync;
                _financialGroupEditForm.FormClosed += FinancialGroupEditForm_FormClosed;
                _financialGroupEditForm.Show(this);
                return;
            }

            if (_financialGroupEditForm.WindowState == FormWindowState.Minimized)
                _financialGroupEditForm.WindowState = FormWindowState.Normal;

            _financialGroupEditForm.Activate();
            _financialGroupEditForm.BringToFront();
        }

        private void FinancialGroupEditForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            var form = sender as FinancialGroupEditForm;
            if (form != null)
            {
                form.SendUpdatedFinancialGroup -= RcvUpdatedFinancialGroupAsync;
                form.FormClosed -= FinancialGroupEditForm_FormClosed;
            }
            if (ReferenceEquals(_financialGroupEditForm, sender))
                _financialGroupEditForm = null;
        }

        private void gvFinancialGroups_DoubleClick(object sender, EventArgs e)
        {
            if (!_financialGroups.Any()) return;

            try
            {
                string currentRowId = gvFinancialGroups.GetFocusedRowCellValue("_id").ToString();
                if (string.IsNullOrEmpty(currentRowId)) return;

                _financialGroupModel = _financialGroups.SingleOrDefault(x => x._id == currentRowId);
                if (_financialGroupModel == null) return;

                ShowFinancialGroupEditor(_financialGroupModel);
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gvFlowTypes_RowCellStyle(object sender, RowCellStyleEventArgs e)
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