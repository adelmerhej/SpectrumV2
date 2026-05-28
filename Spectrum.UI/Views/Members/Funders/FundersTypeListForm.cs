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
    public partial class FundersTypeListForm : RibbonForm, IFormWithRibbon
    {
        private bool _resetMenu;
        private FunderTypeEditForm _funderTypeEditForm;

        private FunderTypeModel _funderTypeModel = new FunderTypeModel();
        private IList<FunderTypeModel> _funderTypes = new List<FunderTypeModel>();

        private readonly FunderTypeRepository _funderTypeRepository = new FunderTypeRepository(DatabaseFactory.ProfilePrimary);

        //Init permissionvariables
        private bool _canAdd = true;
        private bool _canEdit = true;
        private bool _canDelete = true;
        private bool _canPrint = true;
        private bool _isAdmin = true;
        private bool _isProtected = true;

        #region Implementation of IFormWithRibbon

        public RibbonControl MainRibbon => rcFunderType;
        public RibbonPage DefaultPage => rpFunderType;


        #endregion

        public FundersTypeListForm()
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

                _funderTypes = await _funderTypeRepository.GetFunderTypesAsync();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WireUpBindings()
        {
            gcFunderTypes.DataSource = null;
            gcFunderTypes.DataSource = _funderTypes;
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
            ShowFunderTypeEditor(new FunderTypeModel());
        }

        private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!_funderTypes.Any()) return;

            try
            {
                string currentRowId = gvFunderTypes.GetFocusedRowCellValue("_id").ToString();
                if (string.IsNullOrEmpty(currentRowId)) return;

                _funderTypeModel = _funderTypes.SingleOrDefault(x => x._id == currentRowId);
                if (_funderTypeModel == null) return;

                ShowFunderTypeEditor(_funderTypeModel);
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
            gcFunderTypes.ShowRibbonPrintPreview();
        }

        private async void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!CanDelete()) return;

            try
            {
                string id = gvFunderTypes.GetFocusedRowCellValue("_id").ToString();
                string name = gvFunderTypes.GetFocusedRowCellValue("FunderTypeName").ToString();

                if (!string.IsNullOrEmpty(id))
                {
                    if (XtraMessageBox.Show($"Are you sure you want to delete Record: `{name}`?",
                            "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        _funderTypeModel = gvFunderTypes.GetFocusedRow() as FunderTypeModel;
                        if (_funderTypeModel == null)
                        {
                            return;
                        }
                        _funderTypeModel.Deleted = true;

                        //delete the record
                        await _funderTypeRepository.DeleteFunderTypeAsync(_funderTypeModel._id);
                        RcvUpdatedFunderTypeAsync(_funderTypeModel, EventArgs.Empty);
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
                LayoutsStyle.ResetLayoutGrid(gvFunderTypes, CurrentUser.UserName, CurrentUser.Company);
            }

        }

        #endregion

        private void gvFunderTypes_DoubleClick(object sender, EventArgs e)
        {
            if (!_funderTypes.Any()) return;

            try
            {
                string currentRowId = gvFunderTypes.GetFocusedRowCellValue("_id").ToString();
                if (string.IsNullOrEmpty(currentRowId)) return;

                _funderTypeModel = _funderTypes.SingleOrDefault(x => x._id == currentRowId);
                if (_funderTypeModel == null) return;

                ShowFunderTypeEditor(_funderTypeModel);
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void RcvUpdatedFunderTypeAsync(object sender, EventArgs e)
        {
            if (sender == null) return;
            _funderTypeModel = sender as FunderTypeModel;
            if (_funderTypeModel == null) return;

            if (_funderTypeModel.Deleted || _funderTypeModel.LastModifiedDate == null)
            {
                await InitializeBindings();
                WireUpBindings();
            }
            else
            {
                gcFunderTypes.RefreshDataSource();
                gvFunderTypes.RefreshRow(gvFunderTypes.FocusedRowHandle);
                gvFunderTypes.UpdateCurrentRow();
            }
        }

        private bool CanDelete()
        {
            FunderTypeModel dataBoundItem = gvFunderTypes.GetFocusedRow() as FunderTypeModel;

            if (gvFunderTypes == null || gvFunderTypes.SelectedRowsCount == 0) return false;
            if (gvFunderTypes.SelectedRowsCount > 1)
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

        private void ShowFunderTypeEditor(FunderTypeModel model)
        {
            if (_funderTypeEditForm == null || _funderTypeEditForm.IsDisposed)
            {
                _funderTypeEditForm = new FunderTypeEditForm(model);
                _funderTypeEditForm.SendUpdatedFunderType += RcvUpdatedFunderTypeAsync;
                _funderTypeEditForm.FormClosed += FunderTypeEditForm_FormClosed;
                _funderTypeEditForm.Show(this);
                return;
            }

            if (_funderTypeEditForm.WindowState == FormWindowState.Minimized)
                _funderTypeEditForm.WindowState = FormWindowState.Normal;

            _funderTypeEditForm.Activate();
            _funderTypeEditForm.BringToFront();
        }

        private void FunderTypeEditForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            var form = sender as FunderTypeEditForm;
            if (form != null)
            {
                form.SendUpdatedFunderType -= RcvUpdatedFunderTypeAsync;
                form.FormClosed -= FunderTypeEditForm_FormClosed;
            }
            if (ReferenceEquals(_funderTypeEditForm, sender))
                _funderTypeEditForm = null;
        }

        private void gvFunderTypes_RowCellStyle(object sender, RowCellStyleEventArgs e)
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