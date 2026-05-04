using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using Spectrum.DataLayers.Accounting.Charts;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Accounting.Charts;
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

namespace Spectrum.Views.Accounting.Charts
{
    public partial class ChartsListForm : RibbonForm, IFormWithRibbon
    {
        private bool _resetMenu;
        private ChartEditForm _chartEditForm;

        private ChartModel _chartModel = new ChartModel();
        private IList<ChartModel> _charts = new List<ChartModel>();

        private readonly ChartRepository _chartRepository = new ChartRepository(DatabaseFactory.ProfilePrimary);

        //Init permissionvariables
        private bool _canAdd = true;
        private bool _canEdit = true;
        private bool _canDelete = true;
        private bool _canPrint = true;
        private bool _isAdmin = true;
        private bool _isProtected = true;

        #region Implementation of IFormWithRibbon

        public RibbonControl MainRibbon => rcCharts;
        public RibbonPage DefaultPage => rpCharts;


        #endregion

        public ChartsListForm()
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

                _charts = await _chartRepository.GetChartsAsync();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WireUpBindings()
        {
            chartTreeList.DataSource = null;
            chartTreeList.DataSource = _charts;
        }

        private void ApplyDefaults()
        {
            LayoutsStyle.LoadLayoutTreeList(chartTreeList, CurrentUser.UserName, CurrentUser.Company);
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

        #region button events


        private void btnNew_ItemClick(object sender, ItemClickEventArgs e)
        {
            ShowChartEditor(new ChartModel());
        }

        private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!_charts.Any()) return;

            try
            {
                string currentRowId = chartTreeList.GetFocusedRowCellValue("_id").ToString();
                if (string.IsNullOrEmpty(currentRowId)) return;

                _chartModel = _charts.SingleOrDefault(x => x._id == currentRowId);
                if (_chartModel == null) return;

                ShowChartEditor(_chartModel);
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
            chartTreeList.ShowRibbonPrintPreview();
        }

        private async void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!CanDelete()) return;

            try
            {
                string id = chartTreeList.GetFocusedRowCellValue("_id").ToString();
                string name = chartTreeList.GetFocusedRowCellValue("AccountName").ToString();

                if (!string.IsNullOrEmpty(id))
                {
                    if (XtraMessageBox.Show($"Are you sure you want to delete Record: `{name}`?",
                            "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        _chartModel = chartTreeList.GetFocusedRow() as ChartModel;
                        if (_chartModel == null)
                        {
                            return;
                        }
                        _chartModel.Deleted = true;

                        //delete the record
                        await _chartRepository.DeleteChartAsync(_chartModel._id);
                        RcvUpdatedChartAsync(_chartModel, EventArgs.Empty);
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
                LayoutsStyle.ResetLayoutTreeList(chartTreeList, CurrentUser.UserName, CurrentUser.Company);
            }
        }

        #endregion


        private void chartTreeList_DoubleClick(object sender, EventArgs e)
        {
            if (!_charts.Any()) return;

            try
            {
                string currentRowId = chartTreeList.GetFocusedRowCellValue("_id").ToString();
                if (string.IsNullOrEmpty(currentRowId)) return;

                _chartModel = _charts.SingleOrDefault(x => x._id == currentRowId);
                if (_chartModel == null) return;

                ShowChartEditor(_chartModel);
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void RcvUpdatedChartAsync(object sender, EventArgs e)
        {
            if (sender == null) return;
            _chartModel = sender as ChartModel;
            if (_chartModel == null) return;

            if (_chartModel.Deleted || _chartModel.LastModifiedDate == null || _chartModel.Deleted)
            {
                await InitializeBindings();
                WireUpBindings();
            }
            else
            {
                chartTreeList.Update();
            }
        }

        private bool CanDelete()
        {
            ChartModel dataBoundItem = chartTreeList.GetFocusedRow() as ChartModel;
            if (!_charts.Any()) return false;

            if (chartTreeList == null) return false;
            string currentRowId = chartTreeList.GetFocusedRowCellValue("_id")?.ToString();
            string accountNumber = chartTreeList.GetFocusedRowCellValue("AccountNumber")?.ToString();
            if (string.IsNullOrEmpty(currentRowId)) return false;

            if (_chartRepository.ValidateAccountInJournal(accountNumber))
            {
                XtraMessageBox.Show($"There is one or more transaction related to `{accountNumber}`, cannot be deleted.",
                    "Delete error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (dataBoundItem != null && dataBoundItem.IsDefault)
            {
                XtraMessageBox.Show("Cannot delete system record!",
                    "Delete error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (HasTransactions()) return false;

            return true;
        }

        private void ShowChartEditor(ChartModel model)
        {
            if (_chartEditForm == null || _chartEditForm.IsDisposed)
            {
                _chartEditForm = new ChartEditForm(model);
                _chartEditForm.SendUpdatedChartAccount += RcvUpdatedChartAsync;
                _chartEditForm.FormClosed += ChartEditForm_FormClosed;
                _chartEditForm.Show(this);
                return;
            }

            if (_chartEditForm.WindowState == FormWindowState.Minimized)
                _chartEditForm.WindowState = FormWindowState.Normal;

            _chartEditForm.Activate();
            _chartEditForm.BringToFront();
        }

        private void ChartEditForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            var form = sender as ChartEditForm;
            if (form != null)
            {
                form.SendUpdatedChartAccount -= RcvUpdatedChartAsync;
                form.FormClosed -= ChartEditForm_FormClosed;
            }
            if (ReferenceEquals(_chartEditForm, sender))
                _chartEditForm = null;
        }

        private void chartTreeList_RowCellStyle(object sender, RowCellStyleEventArgs e)
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

        private void ChartsListForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_resetMenu)
            {
                LayoutsStyle.SaveLayoutTreeList(chartTreeList, CurrentUser.UserName, CurrentUser.Company);
            }
        }

        private bool HasTransactions()
        {
            try
            {
                //if (!_charts.Any()) return;

                //int currentRowId = (int)chartTreeList.GetFocusedRowCellValue("Id");
                //if (currentRowId == 0) return;

                //_chartModel = _charts.SingleOrDefault(x => x.Id == currentRowId);
                //if (_chartModel == null) return;

                //var frm = new ChartEditForm(_chartModel);
                //frm.SendUpdatedChartAccount += RcvUpdatedChartAccount;
                //frm.ShowDialog();

                if (!_charts.Any()) return false;

                //_chartRepository.SelectChartsByNumber();

                return false;
            }
            catch (Exception e)
            {
                XtraMessageBox.Show(e.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }
    }
}