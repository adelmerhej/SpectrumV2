using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using Spectrum.DataLayers.Common.Items;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Common.Items;
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

namespace Spectrum.Views.Common.Items
{
    public partial class ItemsListForm : RibbonForm, IFormWithRibbon
    {
        private bool _resetMenu;
        private ItemEditForm _itemEditForm;

        private ItemModel _itemModel = new ItemModel();
        private IList<ItemModel> _items = new List<ItemModel>();

        private readonly ItemRepository _itemRepository = new ItemRepository(DatabaseFactory.ProfilePrimary);

        //Init permissionvariables
        private bool _canAdd = true;
        private bool _canEdit = true;
        private bool _canDelete = true;
        private bool _canPrint = true;
        private bool _isAdmin = true;
        private bool _isProtected = true;

        #region Implementation of IFormWithRibbon

        public RibbonControl MainRibbon => rcItemsList;
        public RibbonPage DefaultPage => rpItemsList;


        #endregion


        public ItemsListForm()
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

                _items = await _itemRepository.GetItemsAsync();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WireUpBindings()
        {
            gcItems.DataSource = null;
            gcItems.DataSource = _items;
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

        #region Buttons Event

        private void btnNew_ItemClick(object sender, ItemClickEventArgs e)
        {
            ShowItemEditor(new ItemModel());
        }

        private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!_items.Any()) return;

            try
            {
                string currentRowId = gvItems.GetFocusedRowCellValue("_id").ToString();
                if (string.IsNullOrEmpty(currentRowId)) return;

                _itemModel = _items.SingleOrDefault(x => x._id == currentRowId);
                if (_itemModel == null) return;

                ShowItemEditor(_itemModel);
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
            gcItems.ShowRibbonPrintPreview();
        }

        private async void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!CanDelete()) return;

            try
            {
                string id = gvItems.GetFocusedRowCellValue("_id").ToString();
                string name = gvItems.GetFocusedRowCellValue("Name").ToString();

                if (!string.IsNullOrEmpty(id))
                {
                    if (XtraMessageBox.Show($"Are you sure you want to delete Record: `{name}`?",
                            "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        _itemModel = gvItems.GetFocusedRow() as ItemModel;
                        if (_itemModel == null)
                        {
                            return;
                        }
                        _itemModel.Deleted = true;

                        //delete the record
                        await _itemRepository.DeleteItemAsync(_itemModel._id);
                        RcvUpdatedItemAsync(_itemModel, EventArgs.Empty);
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
                LayoutsStyle.ResetLayoutGrid(gvItems, CurrentUser.UserName, CurrentUser.Company);
            }
        }

        #endregion

        private void gvItems_DoubleClick(object sender, EventArgs e)
        {
            if (!_items.Any()) return;

            try
            {
                string currentRowId = gvItems.GetFocusedRowCellValue("_id").ToString();
                if (string.IsNullOrEmpty(currentRowId)) return;

                _itemModel = _items.SingleOrDefault(x => x._id == currentRowId);
                if (_itemModel == null) return;

                ShowItemEditor(_itemModel);
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void RcvUpdatedItemAsync(object sender, EventArgs e)
        {
            if (sender == null) return;
            _itemModel = sender as ItemModel;
            if (_itemModel == null) return;

            if (_itemModel.Deleted || _itemModel.LastModifiedDate == null)
            {
                await InitializeBindings();
                WireUpBindings();
            }
            else
            {
                gcItems.RefreshDataSource();
                gvItems.RefreshRow(gvItems.FocusedRowHandle);
                gvItems.UpdateCurrentRow();
            }
        }

        private bool CanDelete()
        {
            ItemModel dataBoundItem = gvItems.GetFocusedRow() as ItemModel;

            if (gvItems == null || gvItems.SelectedRowsCount == 0) return false;
            if (gvItems.SelectedRowsCount > 1)
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

        private void ShowItemEditor(ItemModel model)
        {
            if (_itemEditForm == null || _itemEditForm.IsDisposed)
            {
                _itemEditForm = new ItemEditForm(model);
                _itemEditForm.SendUpdatedItem += RcvUpdatedItemAsync;
                _itemEditForm.FormClosed += ItemEditForm_FormClosed;
                _itemEditForm.Show(this);
                return;
            }

            if (_itemEditForm.WindowState == FormWindowState.Minimized)
                _itemEditForm.WindowState = FormWindowState.Normal;

            _itemEditForm.Activate();
            _itemEditForm.BringToFront();
        }

        private void ItemEditForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            var form = sender as ItemEditForm;
            if (form != null)
            {
                form.SendUpdatedItem -= RcvUpdatedItemAsync;
                form.FormClosed -= ItemEditForm_FormClosed;
            }
            if (ReferenceEquals(_itemEditForm, sender))
                _itemEditForm = null;
        }

        private void gvItems_RowCellStyle(object sender, RowCellStyleEventArgs e)
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