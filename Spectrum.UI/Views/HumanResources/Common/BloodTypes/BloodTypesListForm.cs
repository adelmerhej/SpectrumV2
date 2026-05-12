using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Users;
using Spectrum.Utilities;
using Spectrum.Utilities.Interfaces;
using Spectrum.Utilities.Layout;
using Spectrum.DataLayers.HumanResources.BloodTypes;
using Spectrum.Models.HumanResources.BloodTypes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.HumanResources.Common.BloodTypes
{
    public partial class BloodTypesListForm : RibbonForm, IFormWithRibbon
    {
        private bool _resetMenu;
        private BloodTypeEditForm _bloodTypeEditForm;

        private BloodTypeModel _bloodTypeModel = new BloodTypeModel();
        private IList<BloodTypeModel> _bloodTypes = new List<BloodTypeModel>();

        private readonly BloodTypeRepository _bloodTypeRepository = new BloodTypeRepository(DatabaseFactory.ProfilePrimary);

        //Init permissionvariables
        private bool _canAdd = true;
        private bool _canEdit = true;
        private bool _canDelete = true;
        private bool _canPrint = true;
        private bool _isAdmin = true;
        private bool _isProtected = true;

        #region Implementation of IFormWithRibbon

        public RibbonControl MainRibbon => rcBloodTypeList;
        public RibbonPage DefaultPage => rpBloodTypeList;


        #endregion

        public BloodTypesListForm()
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

                _bloodTypes = await _bloodTypeRepository.GetBloodTypesAsync();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WireUpBindings()
        {
            gcBloodTypes.DataSource = null;
            gcBloodTypes.DataSource = _bloodTypes;
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
            ShowBloodTypeEditor(new BloodTypeModel());
        }

        private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!_bloodTypes.Any()) return;

            try
            {
                string currentRowId = gvBloodTypes.GetFocusedRowCellValue("_id").ToString();
                if (string.IsNullOrEmpty(currentRowId)) return;

                _bloodTypeModel = _bloodTypes.SingleOrDefault(x => x._id == currentRowId);
                if (_bloodTypeModel == null) return;

                ShowBloodTypeEditor(_bloodTypeModel);
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
            gcBloodTypes.ShowRibbonPrintPreview();
        }

        private async void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!CanDelete()) return;

            try
            {
                string id = gvBloodTypes.GetFocusedRowCellValue("_id").ToString();
                string name = gvBloodTypes.GetFocusedRowCellValue("BloodTypeName").ToString();

                if (!string.IsNullOrEmpty(id))
                {
                    if (XtraMessageBox.Show($"Are you sure you want to delete Record: `{name}`?",
                            "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        _bloodTypeModel = gvBloodTypes.GetFocusedRow() as BloodTypeModel;
                        if (_bloodTypeModel == null)
                        {
                            return;
                        }
                        _bloodTypeModel.Deleted = true;

                        //delete the record
                        await _bloodTypeRepository.DeleteBloodTypeAsync(_bloodTypeModel._id);
                        RcvUpdatedBloodTypeAsync(_bloodTypeModel, EventArgs.Empty);
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
                LayoutsStyle.ResetLayoutGrid(gvBloodTypes, CurrentUser.UserName, CurrentUser.Company);
            }

        }


        #endregion

        private void gvBloodTypes_DoubleClick(object sender, EventArgs e)
        {
            if (!_bloodTypes.Any()) return;

            try
            {
                string currentRowId = gvBloodTypes.GetFocusedRowCellValue("_id").ToString();
                if (string.IsNullOrEmpty(currentRowId)) return;

                _bloodTypeModel = _bloodTypes.SingleOrDefault(x => x._id == currentRowId);
                if (_bloodTypeModel == null) return;

                ShowBloodTypeEditor(_bloodTypeModel);
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void RcvUpdatedBloodTypeAsync(object sender, EventArgs e)
        {
            if (sender == null) return;
            _bloodTypeModel = sender as BloodTypeModel;
            if (_bloodTypeModel == null) return;

            if (_bloodTypeModel.Deleted || _bloodTypeModel.LastModifiedDate == null)
            {
                await InitializeBindings();
                WireUpBindings();
            }
            else
            {
                gcBloodTypes.RefreshDataSource();
                gvBloodTypes.RefreshRow(gvBloodTypes.FocusedRowHandle);
                gvBloodTypes.UpdateCurrentRow();
            }
        }

        private bool CanDelete()
        {
            BloodTypeModel dataBoundItem = gvBloodTypes.GetFocusedRow() as BloodTypeModel;

            if (gvBloodTypes == null || gvBloodTypes.SelectedRowsCount == 0) return false;
            if (gvBloodTypes.SelectedRowsCount > 1)
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

        private void ShowBloodTypeEditor(BloodTypeModel model)
        {
            if (_bloodTypeEditForm == null || _bloodTypeEditForm.IsDisposed)
            {
                _bloodTypeEditForm = new BloodTypeEditForm(model);
                _bloodTypeEditForm.SendUpdatedBloodType += RcvUpdatedBloodTypeAsync;
                _bloodTypeEditForm.FormClosed += BloodTypeEditForm_FormClosed;
                _bloodTypeEditForm.Show(this);
                return;
            }

            if (_bloodTypeEditForm.WindowState == FormWindowState.Minimized)
                _bloodTypeEditForm.WindowState = FormWindowState.Normal;

            _bloodTypeEditForm.Activate();
            _bloodTypeEditForm.BringToFront();
        }

        private void BloodTypeEditForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            var form = sender as BloodTypeEditForm;
            if (form != null)
            {
                form.SendUpdatedBloodType -= RcvUpdatedBloodTypeAsync;
                form.FormClosed -= BloodTypeEditForm_FormClosed;
            }
            if (ReferenceEquals(_bloodTypeEditForm, sender))
                _bloodTypeEditForm = null;
        }

        private void gvBloodTypes_RowCellStyle(object sender, RowCellStyleEventArgs e)
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