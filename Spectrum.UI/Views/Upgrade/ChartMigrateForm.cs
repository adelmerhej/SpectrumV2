using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using Spectrum.DataLayers.Accounting.Charts;
using Spectrum.DataLayers.Common.Currencies;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Accounting.Charts;
using Spectrum.Models.Administration.Connections;
using Spectrum.Models.Common.Currencies;
using Spectrum.Reports.Common;
using Spectrum.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.Upgrade
{
    public partial class ChartMigrateForm : XtraForm
    {
        private ConnectionModel _connectionModel = new ConnectionModel();
        private ChartModel _chartModel = new ChartModel();
        private IList<ChartDetailModel> _chartDetails = new List<ChartDetailModel>();
        private IList<CurrencyModel> _currencies = new List<CurrencyModel>();

        private readonly ChartRepository _chartRepository = new ChartRepository(DatabaseFactory.ProfilePrimary);
        private readonly ChartDetailRepository _chartDetailRepository = new ChartDetailRepository(DatabaseFactory.ProfilePrimary);
        private readonly CurrencyRepository _currencyRepository = new CurrencyRepository(DatabaseFactory.ProfilePrimary);

        private readonly LogInfoRepository _logInfoRepository = new LogInfoRepository();

        //init permission variables
        private bool _canEdit = true;
        private bool _isAdmin = true;
        private bool _isProtected = true;

        public ChartMigrateForm()
        {
            InitializeComponent();

            StartLoading();
        }


        #region Initialization

        private async void StartLoading()
        {
            try
            {
                await InitializeBindings();
                WireUpBindings();
                ApplyDefaults();
                ApplyPermissions();
            }
            catch (Exception ex)
            {
                ShowError("Error during form initialization", ex);
            }
        }

        private async Task InitializeBindings()
        {
            try
            {
                var loadTasks = new[]
                {
                    LoadApiKeys(),
                };

                await Task.WhenAll(loadTasks);
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading form data", ex);
            }
        }

        private void WireUpBindings()
        {
            // Example: Wire up a ComboBox with countries
        }

        private void ApplyDefaults()
        {
            // Example: Set default values for controls
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

            btnMigrate.Enabled = _isAdmin || _canEdit;
        }

        private async Task LoadApiKeys()
        {
            _connectionModel = DatabaseFactory.GetConnection(DatabaseFactory.ProfilePrimary);
            await Task.CompletedTask;
        }

        #endregion


        #region Button Events

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }


        #endregion

        private void btnMigrate_Click(object sender, EventArgs e)
        {
            try
            {
                progressBarLayout.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                progressBarLayout.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
        }

        private void txtUploadCv_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select a CV Document";
                openFileDialog.Filter = "Excel Files (*.xlsx;*.xls)|*.xlsx;*.xls|All Supported Files (*.xlsx;*.xls)|*.xlsx;*.xls";
                openFileDialog.FilterIndex = 3;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtFilePath.Text = openFileDialog.FileName;
                }
            }
        }

        private void ShowError(string message, Exception ex)
        {
            XtraMessageBox.Show(
                $"{message}\n\nDetails: {ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        private void btnReadFile_Click(object sender, EventArgs e)
        {
            try
            {
                var filePath = txtFilePath.Text?.Trim();
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    XtraMessageBox.Show("Please select an Excel file.", "Read file", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!File.Exists(filePath))
                {
                    XtraMessageBox.Show("The selected file does not exist.", "Read file", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                progressBarLayout.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                progressBarControl1.Position = 0;

                var previewTable = LoadPreviewTable(filePath);
                gcPreview.DataSource = previewTable;
                gvPreview.PopulateColumns();
                gvPreview.BestFitColumns();

                progressBarControl1.Position = 100;
                UpdatePreviewCounter(previewTable.Rows.Count, previewTable.Rows.Count, 0);
            }
            catch (Exception ex)
            {
                gcPreview.DataSource = null;
                UpdatePreviewCounter(0, 0, 0);
                ShowError("Error reading Excel file", ex);
            }
            finally
            {
                progressBarLayout.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
        }

        private DataTable LoadPreviewTable(string filePath)
        {
            return ReportSpreadsheetHelper.LoadWorksheetPreview(filePath);
        }

        private void UpdatePreviewCounter(int totalRecords, int validRecords, int invalidRecords)
        {
            lblCounter.Text = $" Records Found: {totalRecords}      Valid Records: {validRecords}      Invalid Records: {invalidRecords}";
        }
    }
}