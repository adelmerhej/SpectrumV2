using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Accounting.Charts;
using Spectrum.Models.Common.Currencies;
using Spectrum.Models.Users;
using Spectrum.Utilities;
using Spectrum.Utilities.Interfaces;
using Spectrum.Utilities.Layout;
using Spectrum.DataLayers.Accounting.Journals;
using Spectrum.Models.Accounting.Journals;
using Spectrum.Models.Accounting.JournalType;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.Accounting.Journals
{
    public partial class JournalsListForm : RibbonForm, IFormWithRibbon
    {
        private bool _resetMenu;
        private JournalEditForm _journalEditForm;

        private JournalModel _journalModel = new JournalModel();
        private IList<JournalModel> _journals = new List<JournalModel>();
        private IList<JournalDetailModel> _journalDetails = new List<JournalDetailModel>();
        private IList<JournalTypeModel> _journalTypes = new List<JournalTypeModel>();
        private IList<CurrencyModel> _currencies = new List<CurrencyModel>();
        private IList<ChartModel> _charts = new List<ChartModel>();

        private readonly JournalRepository _journalRepository = new JournalRepository(DatabaseFactory.ProfilePrimary);

        //Init permissionvariables
        private bool _canAdd = true;
        private bool _canEdit = true;
        private bool _canDelete = true;
        private bool _canPrint = true;
        private bool _isAdmin = true;
        private bool _isProtected = true;

        #region Implementation of IFormWithRibbon

        public RibbonControl MainRibbon => rcJournals;
        public RibbonPage DefaultPage => rpJournals;


        #endregion

        public JournalsListForm()
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

                _journals = await _journalRepository.GetJournalsAsync();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WireUpBindings()
        {
            gcJournals.DataSource = null;
            gcJournals.DataSource = _journals;
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


        #region Buttons Event Handlers

        private void btnNew_ItemClick(object sender, ItemClickEventArgs e)
        {
            ShowJournalEditor(new JournalModel());
        }

        private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!_journals.Any()) return;

            try
            {
                string currentRowId = gvJournals.GetFocusedRowCellValue("_id").ToString();
                if (string.IsNullOrEmpty(currentRowId)) return;

                _journalModel = _journals.SingleOrDefault(x => x._id == currentRowId);
                if (_journalModel == null) return;

                ShowJournalEditor(_journalModel);
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
            gcJournals.ShowRibbonPrintPreview();
        }

        private void btnPrintFilter_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Print single JV
        }

        private async void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!CanDelete()) return;

            try
            {
                string id = gvJournals.GetFocusedRowCellValue("_id").ToString();
                string name = gvJournals.GetFocusedRowCellValue("JournalName").ToString();

                if (!string.IsNullOrEmpty(id))
                {
                    if (XtraMessageBox.Show($"Are you sure you want to delete Record: `{name}`?",
                            "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        _journalModel = gvJournals.GetFocusedRow() as JournalModel;
                        if (_journalModel == null)
                        {
                            return;
                        }
                        _journalModel.Deleted = true;

                        //delete the record
                        await _journalRepository.DeleteJournalAsync(_journalModel._id);
                        RcvUpdatedJournalAsync(_journalModel, EventArgs.Empty);
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

        private void btnPrintStatement_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Print statement of selected Account JV
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
                LayoutsStyle.ResetLayoutGrid(gvJournals, CurrentUser.UserName, CurrentUser.Company);
            }
        }

        #endregion

        private void gvJournals_DoubleClick(object sender, EventArgs e)
        {
            if (!_journals.Any()) return;

            try
            {
                string currentRowId = gvJournals.GetFocusedRowCellValue("_id").ToString();
                if (string.IsNullOrEmpty(currentRowId)) return;

                _journalModel = _journals.SingleOrDefault(x => x._id == currentRowId);
                if (_journalModel == null) return;

                ShowJournalEditor(_journalModel);
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void RcvUpdatedJournalAsync(object sender, EventArgs e)
        {
            if (sender == null) return;
            _journalModel = sender as JournalModel;
            if (_journalModel == null) return;

            if (_journalModel.Deleted || _journalModel.LastModifiedDate == null)
            {
                await InitializeBindings();
                WireUpBindings();
            }
            else
            {
                gcJournals.RefreshDataSource();
                gvJournals.RefreshRow(gvJournals.FocusedRowHandle);
                gvJournals.UpdateCurrentRow();
            }
        }

        private bool CanDelete()
        {
            JournalModel dataBoundItem = gvJournals.GetFocusedRow() as JournalModel;

            if (gvJournals == null || gvJournals.SelectedRowsCount == 0) return false;
            if (gvJournals.SelectedRowsCount > 1)
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

        private void ShowJournalEditor(JournalModel model)
        {
            if (_journalEditForm == null || _journalEditForm.IsDisposed)
            {
                _journalEditForm = new JournalEditForm(model);
                _journalEditForm.SendUpdatedJournal += RcvUpdatedJournalAsync;
                _journalEditForm.FormClosed += JournalEditForm_FormClosed;
                _journalEditForm.Show(this);
                return;
            }

            if (_journalEditForm.WindowState == FormWindowState.Minimized)
                _journalEditForm.WindowState = FormWindowState.Normal;

            _journalEditForm.Activate();
            _journalEditForm.BringToFront();
        }

        private void JournalEditForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            var form = sender as JournalEditForm;
            if (form != null)
            {
                form.SendUpdatedJournal -= RcvUpdatedJournalAsync;
                form.FormClosed -= JournalEditForm_FormClosed;
            }
            if (ReferenceEquals(_journalEditForm, sender))
                _journalEditForm = null;
        }

        private void gvJournals_RowCellStyle(object sender, RowCellStyleEventArgs e)
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