using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using SpectrumV1.DataLayers.Members.Clients;
using SpectrumV1.DataLayers.Members.Engineers;
using SpectrumV1.DataLayers.Projects;
using SpectrumV1.Models.Members.Clients;
using SpectrumV1.Models.Members.Engineers;
using SpectrumV1.Models.Projects;
using SpectrumV1.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpectrumV1.Views.Projects
{
	public partial class ProjectEditForm : RibbonForm
	{
		private ProjectModel _projectModel = new ProjectModel();
		private IList<ClientModel> _clients = new List<ClientModel>();
		private IList<EngineerModel> _engineers = new List<EngineerModel>();

		private readonly ProjectRepository _projectRepository = new ProjectRepository();
		private readonly ClientRepository _clientRepository = new ClientRepository();
		private readonly EngineerRepository _engineerRepository = new EngineerRepository();
		private readonly LogInfoRepository _logInfoRepository = new LogInfoRepository();

		private bool _canAdd = true;
		private bool _canEdit = true;
		private bool _canDelete = true;
		private bool _canPrint = true;
		private bool _isAdmin = true;
		private bool _isProtected = true;

		public EventHandler SendUpdatedProject;

		public ProjectEditForm(ProjectModel model)
		{
			InitializeComponent();

			_projectModel = model;
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
				_clients = await _clientRepository.GetClientsAsync();
				_engineers = await _engineerRepository.GetEngineersAsync();
			}
			catch (Exception ex)
			{
				XtraMessageBox.Show(ex.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void WireUpBindings()
		{
			bsProject.DataSource = _projectModel;
			cboClients.Properties.DataSource = null;
			cboClients.Properties.DataSource = _clients;
			cboClients.Properties.DisplayMember = "ClientName";
			cboClients.Properties.ValueMember = "_id";

			cboEngineers.Properties.DataSource = null;
			cboEngineers.Properties.DataSource = _engineers;
			cboEngineers.Properties.DisplayMember = "EngineerName";
			cboEngineers.Properties.ValueMember = "_id";

			cboStatus.Properties.Items.Clear();
			cboStatus.Properties.Items.AddRange(Enum.GetNames(typeof(ProjectStatus)));
		}

		private void ApplyDefaults()
		{
			if (string.IsNullOrEmpty(_projectModel._id))
			{
				_projectModel.Status = ProjectStatus.Active;
				_projectModel.IssuanceDate = DateTime.Today;
			}
		}

		private void ApplyPermissions()
		{
			btnNew.Enabled = _isAdmin || _canAdd;
			btnSave.Enabled = _isAdmin || _canEdit;
			btnSaveAndClose.Enabled = _isAdmin || _canEdit;
			btnPrint.Enabled = _isAdmin || _canPrint;
			btnDelete.Enabled = _isAdmin || _canDelete;
		}

		private void btnNew_ItemClick(object sender, ItemClickEventArgs e)
		{
			_projectModel = new ProjectModel();
			StartLoading();
		}

		private void btnSave_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!ValidateData()) return;
			SaveData();
		}

		private void btnSaveAndClose_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!ValidateData()) return;
			SaveData();
			Close();
		}

		private void btnRefresh_ItemClick(object sender, ItemClickEventArgs e)
		{
			StartLoading();
		}

		private async void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (string.IsNullOrEmpty(_projectModel._id)) return;
			if (XtraMessageBox.Show($"Are you sure you want to delete Record: `{_projectModel.ProjectName}`?",
				"Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2) == DialogResult.Yes)
			{
				await _projectRepository.DeleteProjectAsync(_projectModel._id);
				_projectModel.Deleted = true;
				SendUpdatedProject(_projectModel, EventArgs.Empty);
				Close();
			}
		}

		private void btnPrint_ItemClick(object sender, ItemClickEventArgs e) { }
		private void btnClose_ItemClick(object sender, ItemClickEventArgs e) { Close(); }

		private async void SaveData()
		{
			try
			{
				BindingContext[bsProject].EndCurrentEdit();
				_projectModel = (ProjectModel)bsProject.Current;

				// derive client / engineer names from selected values
				if (cboClients.EditValue != null)
				{
					var selectedClient = _clients.FirstOrDefault(c => c._id == cboClients.EditValue.ToString());
					if (selectedClient != null) _projectModel.ClientName = selectedClient.ClientName;
				}
				if (cboEngineers.EditValue != null)
				{
					var selectedEngineer = _engineers.FirstOrDefault(e => e._id == cboEngineers.EditValue.ToString());
					if (selectedEngineer != null) _projectModel.EngineerInCharge = selectedEngineer.EngineerName;
				}
				if (!string.IsNullOrEmpty(cboStatus.Text))
				{
					if (Enum.TryParse<ProjectStatus>(cboStatus.Text, out var st)) _projectModel.Status = st;
				}

				if (string.IsNullOrEmpty(_projectModel._id))
				{
					_logInfoRepository.CreateLogInfo(_projectModel);
					var newId = await _projectRepository.AddNewProjectAsync(_projectModel);
					if (string.IsNullOrEmpty(newId)) throw new Exception($"Error while saving : {_projectModel.ProjectName}");
				}
				else
				{
					_logInfoRepository.UpdateLogInfo(_projectModel);
					await _projectRepository.UpdateProjectAsync(_projectModel);
				}
				SendUpdatedProject(_projectModel, EventArgs.Empty);
			}
			catch (Exception exception)
			{
				XtraMessageBox.Show(exception.Message, "Project save error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private bool ValidateData()
		{
			var validateReturnValue = true;
			var messageNumber = 0;
			var validateMessage = new StringBuilder();

			if (string.IsNullOrWhiteSpace(txtProjectName.Text))
			{
				messageNumber += 1; validateMessage.Append("\n- Project Name cannot be empty.");
				validateReturnValue = false; txtProjectName.Focus();
			}
			if (cboStatus.EditValue == null)
			{
				messageNumber += 1; validateMessage.Append("\n- Status cannot be empty.");
				validateReturnValue = false; cboStatus.Focus();
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
