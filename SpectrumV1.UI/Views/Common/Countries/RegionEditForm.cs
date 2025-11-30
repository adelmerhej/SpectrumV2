using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using SpectrumV1.DataLayers.Common.Countries;
using SpectrumV1.DataLayers.DataAccess;
using SpectrumV1.Models.Common.Countries;
using SpectrumV1.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpectrumV1.Views.Common.Countries
{
	public partial class RegionEditForm : XtraForm
	{
		private RegionModel _regionModel = new RegionModel();

		private ContinentModel _continentModel = new ContinentModel();
		private IList<ContinentModel> _continents = new List<ContinentModel>();

		private readonly RegionRepository _regionRepository = new RegionRepository(DatabaseFactory.ProfilePrimary);
		private readonly ContinentRepository _continentRepository = new ContinentRepository(DatabaseFactory.ProfilePrimary);

		private readonly LogInfoRepository _logInfoRepository = new LogInfoRepository();

		//init permission variables
		private bool _canEdit = true;
		private bool _isAdmin = true;
		private bool _isProtected = true;

		public EventHandler SendUpdatedRegion;

		public RegionEditForm(RegionModel model)
		{
			InitializeComponent();

			_regionModel = model;

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

				_continents = await _continentRepository.GetContinentsAsync();
			}
			catch (Exception ex)
			{
				XtraMessageBox.Show(ex.Message, @"Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void WireUpBindings()
		{
			bsRegion.DataSource = _regionModel;

			cboContinents.Properties.DataSource = null;
			cboContinents.Properties.DataSource = _continents;
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
				BindingContext[bsRegion].EndCurrentEdit();
				_regionModel = (RegionModel)bsRegion.Current;


				if (string.IsNullOrEmpty(_regionModel._id))
				{
					_logInfoRepository.CreateLogInfo(_regionModel);

					var newId = await _regionRepository.AddNewRegionAsync(_regionModel);
				}
				else
				{
					_logInfoRepository.UpdateLogInfo(_regionModel);

					await _regionRepository.UpdateRegionAsync(_regionModel);
				}

				SendUpdatedRegion(_regionModel, EventArgs.Empty);
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
				validateMessage.Append("\n- Name cannot be empty.");
				validateReturnValue = false;
				txtName.Focus();
			}

			if (cboContinents.Text == "")
			{
				messageNumber += 1;
				validateMessage.Append("\n- Continent cannot be empty.");
				validateReturnValue = false;
				cboContinents.Focus();
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

		private void cboContinents_AddNewValue(object sender, AddNewValueEventArgs e)
		{
			ContinentEditForm frm = new ContinentEditForm(new ContinentModel());
			frm.SendUpdatedContinent += RcvUpdatedContinent;
			frm.ShowDialog();
		}

		private void RcvUpdatedContinent(object sender, EventArgs e)
		{
			if (sender == null) return;
			_continentModel = sender as ContinentModel;

			_continents.Add(_continentModel);

			cboContinents.Properties.DataSource = null;
			cboContinents.Properties.DataSource = _continents;
			if (_continentModel != null) cboContinents.EditValue = _continentModel.ContinentName;
		}
	}
}