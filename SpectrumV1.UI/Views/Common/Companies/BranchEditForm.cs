using DevExpress.XtraEditors;
using SpectrumV1.DataLayers.Common.Branches;
using SpectrumV1.DataLayers.Common.Companies;
using SpectrumV1.DataLayers.DataAccess;
using SpectrumV1.Models.Common.Companies;
using SpectrumV1.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpectrumV1.Views.Common.Companies
{
	public partial class BranchEditForm : XtraForm
	{
		private BranchModel _branchModel = new BranchModel();
		private IList<BranchModel> _branches = new List<BranchModel>();
		private CompanyModel _companyModel = new CompanyModel();
		private IList<CompanyModel> _companies = new List<CompanyModel>();

		private readonly BranchRepository _branchRepository = new BranchRepository(DatabaseFactory.ProfilePrimary);
		private readonly CompanyRepository _companyRepository = new CompanyRepository(DatabaseFactory.ProfilePrimary);

		private readonly LogInfoRepository _logInfoRepository = new LogInfoRepository();

		//init permission variables
		private bool _canEdit = true;
		private bool _isAdmin = true;
		private bool _isProtected = true;

		public EventHandler SendUpdatedBranch;

		public BranchEditForm(BranchModel model)
		{
			InitializeComponent();

			_branchModel = model;

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
			bsBranch.DataSource = _branchModel;

			cboCompanies.Properties.DataSource = null;
			cboCompanies.Properties.DataSource = _companies;
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

			btnSave.Enabled = _isAdmin || _canEdit;
		}

		private async void btnSave_Click(object sender, EventArgs e)
		{
			if (!ValidateData()) return;

			try
			{
				BindingContext[bsBranch].EndCurrentEdit();
				_branchModel = (BranchModel)bsBranch.Current;

				// Map selected company
				var selectedCompanyId = cboCompanies.EditValue?.ToString();

				if (string.IsNullOrEmpty(_branchModel._id))
				{
					_logInfoRepository.CreateLogInfo(_branchModel);
					_branchModel.Company = selectedCompanyId;

					var newId = await _branchRepository.AddNewBranchAsync(_branchModel);
				}
				else
				{
					_logInfoRepository.UpdateLogInfo(_branchModel);
					_branchModel.Company = selectedCompanyId;

					await _branchRepository.UpdateBranchAsync(_branchModel);
				}

				SendUpdatedBranch(_branchModel, EventArgs.Empty);
				Close();
			}
			catch (Exception ex)
			{
				XtraMessageBox.Show(ex.Message, @"Error Saving user", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		private bool ValidateData()
		{
			var validateReturnValue = true;
			var messageNumber = 0;
			var validateMessage = new StringBuilder();

			if (txtName.Text == "")
			{
				messageNumber += 1;
				validateMessage.Append("\n- Branch Name cannot be empty.");
				validateReturnValue = false;
				txtName.Focus();
			}

			if (cboCompanies.Text == "")
			{
				messageNumber += 1;
				validateMessage.Append("\n- Company cannot be empty.");
				validateReturnValue = false;
				cboCompanies.Focus();
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
	}
}