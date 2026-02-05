using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using Spectrum.DataLayers.Common.Companies;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Common.Companies;
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

namespace Spectrum.Views.Common.Companies
{
	public partial class CompaniesListForm : RibbonForm, IFormWithRibbon
	{
		private bool _resetMenu;
		private CompanyEditForm _companyEditForm;

		private CompanyModel _companyModel = new CompanyModel();
		private IList<CompanyModel> _companies = new List<CompanyModel>();

		private readonly CompanyRepository _companyRepository = new CompanyRepository(DatabaseFactory.ProfilePrimary);

		//Init permissionvariables
		private bool _canAdd = true;
		private bool _canEdit = true;
		private bool _canDelete = true;
		private bool _canPrint = true;
		private bool _isAdmin = true;
		private bool _isProtected = true;

		#region Implementation of IFormWithRibbon

		public RibbonControl MainRibbon => rcCompaniesList;
		public RibbonPage DefaultPage => rpCompaniesList;


		#endregion

		public CompaniesListForm()
		{
			InitializeComponent();

			// wire events
			btnNew.ItemClick += btnNew_ItemClick;
			btnEdit.ItemClick += btnEdit_ItemClick;
			btnDelete.ItemClick += btnDelete_ItemClick;
			btnPrint.ItemClick += btnPrint_ItemClick;
			btnRefresh.ItemClick += btnRefresh_ItemClick;
			btnClose.ItemClick += btnClose_ItemClick;
			btnResetGridStyle.ItemClick += btnResetGridStyle_ItemClick;
			gvCompanies.DoubleClick += gvCompanies_DoubleClick;
			gvCompanies.RowCellStyle += gvCompanies_RowCellStyle;

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

				_companies = await _companyRepository.GetCompaniesAsync();
			}
			catch (Exception ex)
			{
				XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void WireUpBindings()
		{
			gcCompanies.DataSource = null;
			gcCompanies.DataSource = _companies;
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

		#region Buttons Events

		private void btnNew_ItemClick(object sender, ItemClickEventArgs e)
		{
			ShowCompanyEditor(new CompanyModel());
		}

		private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!_companies.Any()) return;

			try
			{
				string currentRowId = gvCompanies.GetFocusedRowCellValue("_id").ToString();
				if (string.IsNullOrEmpty(currentRowId)) return;

				_companyModel = _companies.SingleOrDefault(x => x._id == currentRowId);
				if (_companyModel == null) return;

				ShowCompanyEditor(_companyModel);
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
			gcCompanies.ShowRibbonPrintPreview();
		}

		private async void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!CanDelete()) return;

			try
			{
				string id = gvCompanies.GetFocusedRowCellValue("_id").ToString();
				string name = gvCompanies.GetFocusedRowCellValue("CompanyName").ToString();

				if (!string.IsNullOrEmpty(id))
				{
					if (XtraMessageBox.Show($"Are you sure you want to delete Record: `{name}`?",
							"Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
							MessageBoxDefaultButton.Button2) == DialogResult.Yes)
					{
						_companyModel = gvCompanies.GetFocusedRow() as CompanyModel;
						if (_companyModel == null)
						{
							return;
						}
						_companyModel.Deleted = true;

						//delete the record
						await _companyRepository.DeleteCompanyAsync(_companyModel._id);
						RcvUpdatedCompanyAsync(_companyModel, EventArgs.Empty);
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
				LayoutsStyle.ResetLayoutGrid(gvCompanies, CurrentUser.UserName, CurrentUser.Company);
			}
		}

		#endregion

		private void gvCompanies_DoubleClick(object sender, EventArgs e)
		{
			if (!_companies.Any()) return;

			try
			{
				string currentRowId = gvCompanies.GetFocusedRowCellValue("_id").ToString();
				if (string.IsNullOrEmpty(currentRowId)) return;

				_companyModel = _companies.SingleOrDefault(x => x._id == currentRowId);
				if (_companyModel == null) return;

				ShowCompanyEditor(_companyModel);
			}
			catch (Exception exception)
			{
				XtraMessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private async void RcvUpdatedCompanyAsync(object sender, EventArgs e)
		{
			if (sender == null) return;
			_companyModel = sender as CompanyModel;

			if (_companyModel != null && (_companyModel.LastModifiedDate == null || _companyModel.Deleted))
			{
				await InitializeBindings();
				WireUpBindings();
			}
			else
			{
				gvCompanies.UpdateCurrentRow();
			}
		}

		private bool CanDelete()
		{
			CompanyModel dataBoundItem = gvCompanies.GetFocusedRow() as CompanyModel;

			if (gvCompanies == null || gvCompanies.SelectedRowsCount == 0) return false;
			if (gvCompanies.SelectedRowsCount > 1)
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

		private void ShowCompanyEditor(CompanyModel model)
		{
			if (_companyEditForm == null || _companyEditForm.IsDisposed)
			{
				_companyEditForm = new CompanyEditForm(model);
				_companyEditForm.SendUpdatedCompany += RcvUpdatedCompanyAsync;
				_companyEditForm.FormClosed += CompanyEditForm_FormClosed;
				_companyEditForm.Show(this);
				return;
			}

			if (_companyEditForm.WindowState == FormWindowState.Minimized)
				_companyEditForm.WindowState = FormWindowState.Normal;

			_companyEditForm.Activate();
			_companyEditForm.BringToFront();
		}

		private void CompanyEditForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			var form = sender as CompanyEditForm;
			if (form != null)
			{
				form.SendUpdatedCompany -= RcvUpdatedCompanyAsync;
				form.FormClosed -= CompanyEditForm_FormClosed;
			}
			if (ReferenceEquals(_companyEditForm, sender))
				_companyEditForm = null;
		}

		private void gvCompanies_RowCellStyle(object sender, RowCellStyleEventArgs e)
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