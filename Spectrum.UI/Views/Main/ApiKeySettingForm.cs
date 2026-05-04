using DevExpress.XtraEditors;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Administration.Connections;
using System;
using System.Windows.Forms;

namespace Spectrum.Views.Main
{
    public partial class ApiKeySettingForm : XtraForm
    {
        private ConnectionModel _connectionModel = new ConnectionModel();
        private const string _databaseType = "MongoDb";
        private const string _authDb = "admin";

        public ApiKeySettingForm()
        {
            InitializeComponent();

            ConnectionParams();
            InitializeBindings();
            WireUpBindings();
            ApplyPermissions();
        }

        #region Init preloading Methods
        private void ConnectionParams()
        {
            _connectionModel = DatabaseFactory.GetConnection(DatabaseFactory.ProfilePrimary);
        }

        private void InitializeBindings()
        {

        }

        private void WireUpBindings()
        {
            txtOrganizationId.Text = _connectionModel.OrganizationId ?? string.Empty;
            txtAPIKey.Text = _connectionModel.EncryptedAiApikey ?? string.Empty;
        }

        private void ApplyPermissions()
        {

        }

        #endregion


        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                _connectionModel.OrganizationId = txtOrganizationId.Text.Trim();
                _connectionModel.EncryptedAiApikey = txtAPIKey.Text;

                DatabaseFactory.SaveConnection(_connectionModel, DatabaseFactory.ProfilePrimary);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception)
            {
                DialogResult = DialogResult.Cancel;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}