using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Users;
using Spectrum.Utilities;
using Spectrum.Utilities.Interfaces;
using Spectrum.Utilities.Layout;
using Spectrum.DataLayers.Accounting.JournalType;
using Spectrum.Models.Accounting.JournalType;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.Accounting.JournalType
{
    public partial class JournalTypesListForm : RibbonForm, IFormWithRibbon
    {
        private bool _resetMenu;
        private JournalTypeEditForm _journalTypeEditForm;

        private JournalTypeModel _journalTypeModel = new JournalTypeModel();
        private IList<JournalTypeModel> _journalTypes = new List<JournalTypeModel>();

        private readonly JournalTypeRepository _journalTypeRepository = new JournalTypeRepository(DatabaseFactory.ProfilePrimary);

        //Init permissionvariables
        private bool _canAdd = true;
        private bool _canEdit = true;
        private bool _canDelete = true;
        private bool _canPrint = true;
        private bool _isAdmin = true;
        private bool _isProtected = true;

        #region Implementation of IFormWithRibbon

        public RibbonControl MainRibbon => rcJournalTypes;
        public RibbonPage DefaultPage => rpJournalTypes;


        #endregion

        public JournalTypesListForm()
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

                _journalTypes = await _journalTypeRepository.GetJournalTypesAsync();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WireUpBindings()
        {
            gcJournalTypes.DataSource = null;
            gcJournalTypes.DataSource = _journalTypes;
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
            ShowBankEditor(new JournalTypeModel());
        }

        private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!_journalTypes.Any()) return;

            try
            {
                string currentRowId = gvJournalTypes.GetFocusedRowCellValue("_id").ToString();
                if (string.IsNullOrEmpty(currentRowId)) return;

                _journalTypeModel = _journalTypes.SingleOrDefault(x => x._id == currentRowId);
                if (_journalTypeModel == null) return;

                ShowBankEditor(_journalTypeModel);
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
            gcJournalTypes.ShowRibbonPrintPreview();
        }

        private async void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!CanDelete()) return;

            try
            {
                string id = gvJournalTypes.GetFocusedRowCellValue("_id").ToString();
                string name = gvJournalTypes.GetFocusedRowCellValue("JournalTypeName").ToString();

                if (!string.IsNullOrEmpty(id))
                {
                    if (XtraMessageBox.Show($"Are you sure you want to delete Record: `{name}`?",
                            "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        _journalTypeModel = gvJournalTypes.GetFocusedRow() as JournalTypeModel;
                        if (_journalTypeModel == null)
                        {
                            return;
                        }
                        _journalTypeModel.Deleted = true;

                        //delete the record
                        await _journalTypeRepository.DeleteJournalTypeAsync(_journalTypeModel._id);
                        RcvUpdatedBankAsync(_journalTypeModel, EventArgs.Empty);
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
                LayoutsStyle.ResetLayoutGrid(gvJournalTypes, CurrentUser.UserName, CurrentUser.Company);
            }
        }


        #endregion

        private async void RcvUpdatedBankAsync(object sender, EventArgs e)
        {
            if (sender == null) return;
            _journalTypeModel = sender as JournalTypeModel;
            if (_journalTypeModel == null) return;

            if (_journalTypeModel.Deleted || _journalTypeModel.LastModifiedDate == null)
            {
                await InitializeBindings();
                WireUpBindings();
            }
            else
            {
                gcJournalTypes.RefreshDataSource();
                gvJournalTypes.RefreshRow(gvJournalTypes.FocusedRowHandle);
                gvJournalTypes.UpdateCurrentRow();
            }
        }

        private bool CanDelete()
        {
            JournalTypeModel dataBoundItem = gvJournalTypes.GetFocusedRow() as JournalTypeModel;

            if (gvJournalTypes == null || gvJournalTypes.SelectedRowsCount == 0) return false;
            if (gvJournalTypes.SelectedRowsCount > 1)
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

        private void ShowBankEditor(JournalTypeModel model)
        {
            if (_journalTypeEditForm == null || _journalTypeEditForm.IsDisposed)
            {
                _journalTypeEditForm = new JournalTypeEditForm(model);
                _journalTypeEditForm.SendUpdatedJournalType += RcvUpdatedBankAsync;
                _journalTypeEditForm.FormClosed += JournalTypeEditForm_FormClosed;
                _journalTypeEditForm.Show(this);
                return;
            }

            if (_journalTypeEditForm.WindowState == FormWindowState.Minimized)
                _journalTypeEditForm.WindowState = FormWindowState.Normal;

            _journalTypeEditForm.Activate();
            _journalTypeEditForm.BringToFront();
        }

        private void JournalTypeEditForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            var form = sender as JournalTypeEditForm;
            if (form != null)
            {
                form.SendUpdatedJournalType -= RcvUpdatedBankAsync;
                form.FormClosed -= JournalTypeEditForm_FormClosed;
            }
            if (ReferenceEquals(_journalTypeEditForm, sender))
                _journalTypeEditForm = null;
        }

        private void gvJournalTypes_DoubleClick(object sender, EventArgs e)
        {
            if (!_journalTypes.Any()) return;

            try
            {
                string currentRowId = gvJournalTypes.GetFocusedRowCellValue("_id").ToString();
                if (string.IsNullOrEmpty(currentRowId)) return;

                _journalTypeModel = _journalTypes.SingleOrDefault(x => x._id == currentRowId);
                if (_journalTypeModel == null) return;

                ShowBankEditor(_journalTypeModel);
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