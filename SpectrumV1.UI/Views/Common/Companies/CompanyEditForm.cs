using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using SpectrumV1.DataLayers.Common.Branches;
using SpectrumV1.DataLayers.Common.Companies;
using SpectrumV1.DataLayers.Common.Countries;
using SpectrumV1.DataLayers.DataAccess;
using SpectrumV1.Models.Common.Companies;
using SpectrumV1.Models.Common.Countries;
using SpectrumV1.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpectrumV1.Views.Common.Companies
{
	public partial class CompanyEditForm : RibbonForm
	{
		private CompanyModel _companyModel = new CompanyModel();
		private BranchModel _branchModel = new BranchModel();

		private IList<CompanyModel> _companies = new List<CompanyModel>();
		private IList<BranchModel> _branches = new List<BranchModel>();
		private IList<CountryModel> _countries = new List<CountryModel>();
		private IList<CityModel> _cities = new List<CityModel>();

		private readonly CompanyRepository _companyRepository = new CompanyRepository(DatabaseFactory.ProfilePrimary);
		private readonly BranchRepository _branchRepository = new BranchRepository(DatabaseFactory.ProfilePrimary);
		private readonly CountryRepository _countryRepository = new CountryRepository(DatabaseFactory.ProfilePrimary);
		private readonly CityRepository _cityRepository = new CityRepository(DatabaseFactory.ProfilePrimary);

		private readonly LogInfoRepository _logInfoRepository = new LogInfoRepository();

		//Init permissionvariables
		private bool _canAdd = true;
		private bool _canEdit = true;
		private bool _canDelete = true;
		private bool _canPrint = true;
		private bool _isAdmin = true;
		private bool _isProtected = true;

		public EventHandler SendUpdatedCompany;

		public CompanyEditForm(CompanyModel model)
		{
			InitializeComponent();

			_companyModel = model;

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

				_branches = await _branchRepository.GetBranchesAsync();
				_countries = await _countryRepository.GetCountriesAsync();
				_cities = await _cityRepository.GetCitiesAsync();
			}
			catch (Exception ex)
			{
				XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void WireUpBindings()
		{
			bsCompany.DataSource = _companyModel;

			gcBranches.DataSource = null;
			gcBranches.DataSource = _branches;

			repCompanies.DataSource = null;
			repCompanies.DataSource = _companies;

			cboCountries.Properties.DataSource = null;
			cboCountries.Properties.DataSource = _countries;

			cboCities.Properties.DataSource = null;
			cboCities.Properties.DataSource = _cities;
		}

		private void ApplyDefaults()
		{
			if (string.IsNullOrEmpty(_branchModel._id))
			{
				_companyModel.Active = true;
			}
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
			btnSave.Enabled = _isAdmin || _canEdit;
			btnSaveAndClose.Enabled = _isAdmin || _canEdit;
			btnRefresh.Enabled = _isAdmin || _canEdit;
			btnPrint.Enabled = _isAdmin || _canPrint;
			btnDelete.Enabled = _isAdmin || _canDelete;

			btnAddNewBranch.Enabled = _isAdmin || _canAdd;
			btnEditBranch.Enabled = _isAdmin || _canEdit;
			btnDeleteBranch.Enabled = _isAdmin || _canDelete;
		}

		#region Buttons EventHandlers

		private void btnNew_ItemClick(object sender, ItemClickEventArgs e)
		{
			_companyModel = new CompanyModel();
			bsCompany.DataSource = _companyModel;
			bsCompany.ResetBindings(true);
			ApplyDefaults();
		}

		private void btnSave_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!ValidateData()) return;
			SaveCompany();
		}

		private void btnSaveAndClose_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!ValidateData()) return;
			SaveCompany();
			Close();
		}

		private void btnRefresh_ItemClick(object sender, ItemClickEventArgs e)
		{
			StartLoading();
		}

		private void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
		{
			// delete company
		}

		private void btnPrint_ItemClick(object sender, ItemClickEventArgs e)
		{
			// print company details
		}

		private void btnAddNewBranch_ItemClick(object sender, ItemClickEventArgs e)
		{
			BranchEditForm frm = new BranchEditForm(new BranchModel());
			frm.SendUpdatedBranch += RcvUpdatedBranchAsync;
			frm.ShowDialog();
		}

		private void btnEditBranch_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!_branches.Any()) return;

			var currentRowId = gvBranches.GetFocusedRowCellValue("_id").ToString();
			if (string.IsNullOrEmpty(currentRowId)) return;

			_branchModel = _branches.SingleOrDefault(x => x._id == currentRowId);
			if (_branchModel == null) return;

			var frm = new BranchEditForm(_branchModel);
			frm.SendUpdatedBranch += RcvUpdatedBranchAsync;
			frm.ShowDialog();
		}

		private void btnDeleteBranch_ItemClick(object sender, ItemClickEventArgs e)
		{
			throw new NotImplementedException();
		}

		private void btnClose_ItemClick(object sender, ItemClickEventArgs e)
		{
			Close();
		}

		#endregion

		private async void SaveCompany()
		{
			try
			{
				BindingContext[bsCompany].EndCurrentEdit();

				if (string.IsNullOrEmpty(_companyModel._id))
				{
					string newRecord = await _companyRepository.AddNewCompanyAsync(_companyModel);
					if (string.IsNullOrEmpty(newRecord))
					{
						throw new Exception($"Error while saving : {txtCompanyName.Text}");
					}
					//txtCompanyId.Text = newRecord.ToString();
					//SeedDefaultForNewCompany(false);
				}
				else
				{
					bool updateSucceeded = await _companyRepository.UpdateCompanyAsync(_companyModel);
				}

				SendUpdatedCompany(_companyModel, EventArgs.Empty);
			}
			catch (Exception exception)
			{
				XtraMessageBox.Show(exception.Message,
					"Error While saving", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private bool ValidateData()
		{
			var validateReturnValue = true;
			var messageNumber = 0;
			var validateMessage = new StringBuilder();

			if (txtCompanyName.Text == "")
			{
				messageNumber += 1;
				validateMessage.Append("\n- Company Name cannot be empty.");
				validateReturnValue = false;
				txtCompanyName.Focus();
			}

			if (txtShortName.Text == "")
			{
				messageNumber += 1;
				validateMessage.Append("\n- Short Name cannot be empty.");
				validateReturnValue = false;
				txtShortName.Focus();
			}

			if (!validateReturnValue)
			{
				validateMessage.Insert(0, "The following need your attention:");
				if (messageNumber > 1) validateMessage.Replace("following", "followings");
				XtraMessageBox.Show(validateMessage + " \nPlease try again.",
					"Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}

			return validateReturnValue;
		}

		private void RcvUpdatedBranchAsync(object sender, EventArgs e)
		{
			var updatedBranch = (BranchModel)sender;
			var existingBranch = _branches.SingleOrDefault(x => x._id == updatedBranch._id);
			if (existingBranch != null)
			{
				// Update existing branch
				var index = _branches.IndexOf(existingBranch);
				_branches[index] = updatedBranch;
			}
			else
			{
				// Add new branch
				_branches.Add(updatedBranch);
			}
			gvBranches.RefreshData();
		}

		private void gvBranches_DoubleClick(object sender, EventArgs e)
		{
			if (!_branches.Any()) return;

			var currentRowId = gvBranches.GetFocusedRowCellValue("_id").ToString();
			if (string.IsNullOrEmpty(currentRowId)) return;

			_branchModel = _branches.SingleOrDefault(x => x._id == currentRowId);
			if (_branchModel == null) return;

			var frm = new BranchEditForm(_branchModel);
			frm.SendUpdatedBranch += RcvUpdatedBranchAsync;
			frm.ShowDialog();
		}
	}
}