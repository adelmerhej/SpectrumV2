using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Users;
using Spectrum.Utilities;
using Spectrum.Utilities.Interfaces;
using Spectrum.Utilities.Layout;
using Spectrum.Views.Accounting.FlowType;
using Spectrum.DataLayers.Accounting.FlowType;
using Spectrum.Models.Accounting.FlowType;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.Accounting.FlowType
{
    public partial class FlowTypeListForm : RibbonForm, IFormWithRibbon
    {
        private bool _resetMenu;
        private FlowTypeEditForm _flowTypeEditForm;

        private FlowTypeModel _flowTypeModel = new FlowTypeModel();
        private IList<FlowTypeModel> _flowTypes = new List<FlowTypeModel>();

        private readonly FlowTypeRepository _flowTypeRepository = new FlowTypeRepository(DatabaseFactory.ProfilePrimary);

        //Init permissionvariables
        private bool _canAdd = true;
        private bool _canEdit = true;
        private bool _canDelete = true;
        private bool _canPrint = true;
        private bool _isAdmin = true;
        private bool _isProtected = true;

        #region Implementation of IFormWithRibbon

        public RibbonControl MainRibbon => rcFlowTypes;
        public RibbonPage DefaultPage => rpFlowTypes;


        #endregion

        public FlowTypeListForm()
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

                _flowTypes = await _flowTypeRepository.GetFlowTypesAsync();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WireUpBindings()
        {
            gcFlowTypes.DataSource = null;
            gcFlowTypes.DataSource = _flowTypes;
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
            ShowBankEditor(new FlowTypeModel());
        }

        private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!_flowTypes.Any()) return;

            try
            {
                string currentRowId = gvFlowTypes.GetFocusedRowCellValue("_id").ToString();
                if (string.IsNullOrEmpty(currentRowId)) return;

                _flowTypeModel = _flowTypes.SingleOrDefault(x => x._id == currentRowId);
                if (_flowTypeModel == null) return;

                ShowBankEditor(_flowTypeModel);
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
            gcFlowTypes.ShowRibbonPrintPreview();
        }

        private async void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!CanDelete()) return;

            try
            {
                string id = gvFlowTypes.GetFocusedRowCellValue("_id").ToString();
                string name = gvFlowTypes.GetFocusedRowCellValue("FlowTypeName").ToString();

                if (!string.IsNullOrEmpty(id))
                {
                    if (XtraMessageBox.Show($"Are you sure you want to delete Record: `{name}`?",
                            "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        _flowTypeModel = gvFlowTypes.GetFocusedRow() as FlowTypeModel;
                        if (_flowTypeModel == null)
                        {
                            return;
                        }
                        _flowTypeModel.Deleted = true;

                        //delete the record
                        await _flowTypeRepository.DeleteFlowTypeAsync(_flowTypeModel._id);
                        RcvUpdatedBankAsync(_flowTypeModel, EventArgs.Empty);
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
                LayoutsStyle.ResetLayoutGrid(gvFlowTypes, CurrentUser.UserName, CurrentUser.Company);
            }
        }


        #endregion

        private async void RcvUpdatedBankAsync(object sender, EventArgs e)
        {
            if (sender == null) return;
            _flowTypeModel = sender as FlowTypeModel;
            if (_flowTypeModel == null) return;

            if (_flowTypeModel.Deleted || _flowTypeModel.LastModifiedDate == null)
            {
                await InitializeBindings();
                WireUpBindings();
            }
            else
            {
                gcFlowTypes.RefreshDataSource();
                gvFlowTypes.RefreshRow(gvFlowTypes.FocusedRowHandle);
                gvFlowTypes.UpdateCurrentRow();
            }
        }

        private bool CanDelete()
        {
            FlowTypeModel dataBoundItem = gvFlowTypes.GetFocusedRow() as FlowTypeModel;

            if (gvFlowTypes == null || gvFlowTypes.SelectedRowsCount == 0) return false;
            if (gvFlowTypes.SelectedRowsCount > 1)
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

        private void ShowBankEditor(FlowTypeModel model)
        {
            if (_flowTypeEditForm == null || _flowTypeEditForm.IsDisposed)
            {
                _flowTypeEditForm = new FlowTypeEditForm(model);
                _flowTypeEditForm.SendUpdatedFlowType += RcvUpdatedBankAsync;
                _flowTypeEditForm.FormClosed += FlowTypeEditForm_FormClosed;
                _flowTypeEditForm.Show(this);
                return;
            }

            if (_flowTypeEditForm.WindowState == FormWindowState.Minimized)
                _flowTypeEditForm.WindowState = FormWindowState.Normal;

            _flowTypeEditForm.Activate();
            _flowTypeEditForm.BringToFront();
        }

        private void FlowTypeEditForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            var form = sender as FlowTypeEditForm;
            if (form != null)
            {
                form.SendUpdatedFlowType -= RcvUpdatedBankAsync;
                form.FormClosed -= FlowTypeEditForm_FormClosed;
            }
            if (ReferenceEquals(_flowTypeEditForm, sender))
                _flowTypeEditForm = null;
        }

        private void gvFlowTypes_DoubleClick(object sender, EventArgs e)
        {
            if (!_flowTypes.Any()) return;

            try
            {
                string currentRowId = gvFlowTypes.GetFocusedRowCellValue("_id").ToString();
                if (string.IsNullOrEmpty(currentRowId)) return;

                _flowTypeModel = _flowTypes.SingleOrDefault(x => x._id == currentRowId);
                if (_flowTypeModel == null) return;

                ShowBankEditor(_flowTypeModel);
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