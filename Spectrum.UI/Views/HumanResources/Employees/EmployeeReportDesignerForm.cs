using DevExpress.XtraEditors;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.HumanResources.Employees;
using Spectrum.Reports.Helpers;
using Spectrum.Reports.UI;
using SpectrumV1.DataLayers.HumanResources.Employees;
using System;
using System.Windows.Forms;

namespace Spectrum.Views.HumanResources.Employees
{
    public class EmployeeReportDesignerForm : XtraForm
    {
        private readonly EmployeeModel _employee;
        private readonly EmployeeRepository _employeeRepository;
        private readonly ReportDesignerControl _designer;

        public EmployeeReportDesignerForm(EmployeeModel employee)
        {
            if (employee == null) throw new ArgumentNullException(nameof(employee));
            _employee = employee;
            _employeeRepository = new EmployeeRepository(DatabaseFactory.ProfilePrimary);

            var adapter = AdapterFactory.Create(_employee);

            Text = adapter.Title;
            Width = 1200;
            Height = 850;
            StartPosition = FormStartPosition.CenterParent;

            _designer = new ReportDesignerControl();
            _designer.Dock = DockStyle.Fill;
            _designer.ModelChanged += Designer_ModelChanged;
            Controls.Add(_designer);

            _designer.LoadAdapter(adapter);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!e.Cancel && _designer != null && _designer.HasPendingDataChanges)
            {
                var result = XtraMessageBox.Show(
                    "Employee report changes have not been saved. Save them before closing?",
                    "Employee Report Designer",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }

                if (result == DialogResult.Yes)
                    _designer.SaveDataChanges();
            }

            base.OnFormClosing(e);
        }

        private async void Designer_ModelChanged(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(_employee._id))
                    await _employeeRepository.UpdateEmployeeAsync(_employee);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Employee Report Designer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_designer != null)
                    _designer.ModelChanged -= Designer_ModelChanged;
                _designer?.Dispose();
                _employeeRepository?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
