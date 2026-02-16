using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Users;
using Spectrum.Utilities;
using Spectrum.Utilities.Interfaces;
using Spectrum.Utilities.Layout;
using SpectrumV1.DataLayers.HumanResources.Candidates;
using SpectrumV1.Models.HumanResources.Candidates;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.HumanResources.HRCVs
{
    public partial class HrCvsListForm : RibbonForm, IFormWithRibbon
    {
        private bool _resetMenu;
        private HrCvEditForm _hrCvEditForm;

        private CandidateModel _candidateModel = new CandidateModel();
        private IList<CandidateModel> _candidates = new List<CandidateModel>();

        private readonly CandidateRepository _candidateRepository = new CandidateRepository(DatabaseFactory.ProfilePrimary);

        //Init permissionvariables
        private bool _canAdd = true;
        private bool _canEdit = true;
        private bool _canDelete = true;
        private bool _canPrint = true;
        private bool _isAdmin = true;
        private bool _isProtected = true;

        #region Implementation of IFormWithRibbon

        public RibbonControl MainRibbon => rcHrCvsList;
        public RibbonPage DefaultPage => rpHrCvsList;


        #endregion

        public HrCvsListForm()
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

                _candidates = await _candidateRepository.GetCandidatesAsync();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WireUpBindings()
        {
            gcCandidates.DataSource = null;
            gcCandidates.DataSource = _candidates;
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

        private void btnNew_ItemClick(object sender, ItemClickEventArgs e)
        {
            ShowCandidateEditor(new CandidateModel());
        }

        private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!_candidates.Any()) return;

            try
            {
                string currentRowId = gvCandidates.GetFocusedRowCellValue("_id").ToString();
                if (string.IsNullOrEmpty(currentRowId)) return;

                _candidateModel = _candidates.SingleOrDefault(x => x._id == currentRowId);
                if (_candidateModel == null) return;

                ShowCandidateEditor(_candidateModel);
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
            gcCandidates.ShowRibbonPrintPreview();
        }

        private async void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!CanDelete()) return;

            try
            {
                string id = gvCandidates.GetFocusedRowCellValue("_id").ToString();
                string name = gvCandidates.GetFocusedRowCellValue("CandidateName").ToString();

                if (!string.IsNullOrEmpty(id))
                {
                    if (XtraMessageBox.Show($"Are you sure you want to delete Record: `{name}`?",
                            "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        _candidateModel = gvCandidates.GetFocusedRow() as CandidateModel;
                        if (_candidateModel == null)
                        {
                            return;
                        }
                        _candidateModel.Deleted = true;

                        //delete the record
                        await _candidateRepository.DeleteCandidateAsync(_candidateModel._id);
                        RcvUpdatedCandidateAsync(_candidateModel, EventArgs.Empty);
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
                LayoutsStyle.ResetLayoutGrid(gvCandidates, CurrentUser.UserName, CurrentUser.Company);
            }
        }

        private async void RcvUpdatedCandidateAsync(object sender, EventArgs e)
        {
            if (sender == null) return;
            _candidateModel = sender as CandidateModel;

            if (_candidateModel != null && (_candidateModel.LastModifiedDate == null || _candidateModel.Deleted))
            {
                await InitializeBindings();
                WireUpBindings();
            }
            else
            {
                gvCandidates.UpdateCurrentRow();
            }
        }

        private void ShowCandidateEditor(CandidateModel model)
        {
            if (_hrCvEditForm == null || _hrCvEditForm.IsDisposed)
            {
                _hrCvEditForm = new HrCvEditForm(model);
                _hrCvEditForm.SendUpdatedCandidate += RcvUpdatedCandidateAsync;
                _hrCvEditForm.FormClosed += HrCvEditForm_FormClosed;
                _hrCvEditForm.Show(this);
                return;
            }

            if (_hrCvEditForm.WindowState == FormWindowState.Minimized)
                _hrCvEditForm.WindowState = FormWindowState.Normal;

            _hrCvEditForm.Activate();
            _hrCvEditForm.BringToFront();
        }

        private void HrCvEditForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            var form = sender as HrCvEditForm;
            if (form != null)
            {
                form.SendUpdatedCandidate -= RcvUpdatedCandidateAsync;
                form.FormClosed -= HrCvEditForm_FormClosed;
            }
            if (ReferenceEquals(_hrCvEditForm, sender))
                _hrCvEditForm = null;
        }

        private void gvCandidates_DoubleClick(object sender, EventArgs e)
        {
            if (!_candidates.Any()) return;

            try
            {
                string currentRowId = gvCandidates.GetFocusedRowCellValue("_id").ToString();
                if (string.IsNullOrEmpty(currentRowId)) return;

                _candidateModel = _candidates.SingleOrDefault(x => x._id == currentRowId);
                if (_candidateModel == null) return;

                ShowCandidateEditor(_candidateModel);
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gvCandidates_RowCellStyle(object sender, RowCellStyleEventArgs e)
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

        private void HrCvsListForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            var form = sender as HrCvEditForm;
            if (form != null)
            {
                form.SendUpdatedCandidate -= RcvUpdatedCandidateAsync;
                form.FormClosed -= HrCvEditForm_FormClosed;
            }
            if (ReferenceEquals(_hrCvEditForm, sender))
                _hrCvEditForm = null;
        }

        private bool CanDelete()
        {
            CandidateModel dataBoundItem = gvCandidates.GetFocusedRow() as CandidateModel;

            if (gvCandidates == null || gvCandidates.SelectedRowsCount == 0) return false;
            if (gvCandidates.SelectedRowsCount > 1)
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
    }
}