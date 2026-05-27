using DevExpress.Spreadsheet;
using DevExpress.XtraSpreadsheet;
using Spectrum.Models.Projects;
using Spectrum.Reports.Common;
using Spectrum.Reports.Exporters;
using Spectrum.Reports.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Spectrum.Reports.Adapters.Projects
{
    /// <summary>
    /// Report adapter for <see cref="ProjectModel"/>.
    /// Grid: project invoices (editable).
    /// Summary: contract totals, invoiced totals, balance.
    /// CSV: full invoice listing.
    /// </summary>
    public class ProjectReportAdapter : ReportAdapterBase
        , IMultiRecordReportAdapter
        , IEditableSheetProvider
    {
        private ProjectModel _project;
        private readonly IList<ProjectModel> _availableProjects;
        private List<ProjectModel> _selectedProjects;
        private BindingList<InvoiceModel> _invoiceBindingList;

        public ProjectReportAdapter(ProjectModel project)
            : this(new List<ProjectModel> { project }, new List<ProjectModel> { project }, project)
        {
        }

        public ProjectReportAdapter(IList<ProjectModel> availableProjects, IList<ProjectModel> selectedProjects, ProjectModel activeProject)
        {
            if (availableProjects == null)
                throw new ArgumentNullException(nameof(availableProjects));

            _availableProjects = availableProjects
                .Where(x => x != null)
                .GroupBy(GetProjectKey)
                .Select(x => x.First())
                .ToList();

            if (_availableProjects.Count == 0)
                throw new ArgumentException("At least one project is required.", nameof(availableProjects));

            _selectedProjects = BuildProjectSelection(selectedProjects, _availableProjects.First());
            _project = ResolveProject(activeProject) ?? _selectedProjects.First();
            EnsureActiveProjectIsSelected();
            _invoiceBindingList = CreateInvoiceBindingList(_project.Invoices);
        }

        public override string Title
        {
            get
            {
                if (_selectedProjects.Count > 1)
                    return string.Format("Project Report ({0} Projects)", _selectedProjects.Count);

                return string.IsNullOrEmpty(_project.ProjectName)
                    ? "Project Report"
                    : _project.ProjectName + " - Report";
            }
        }

        public override object Model
        {
            get { return _project; }
        }

        public IList<ReportRecordDescriptor> GetAvailableRecords()
        {
            return _availableProjects.Select(CreateRecordDescriptor).ToList();
        }

        public IList<ReportRecordDescriptor> GetSelectedRecords()
        {
            return _selectedProjects.Select(CreateRecordDescriptor).ToList();
        }

        public ReportRecordDescriptor GetActiveRecord()
        {
            return _project == null ? null : CreateRecordDescriptor(_project);
        }

        public void UpdateRecordSelection(IList<string> selectedKeys, string activeKey)
        {
            Recalculate();

            var selectedProjects = _availableProjects
                .Where(x => selectedKeys != null && selectedKeys.Contains(GetProjectKey(x)))
                .ToList();

            _selectedProjects = BuildProjectSelection(selectedProjects, _project ?? _availableProjects.First());

            var activeProject = _availableProjects.FirstOrDefault(x => string.Equals(GetProjectKey(x), activeKey, StringComparison.OrdinalIgnoreCase));
            if (activeProject == null || !_selectedProjects.Any(x => string.Equals(GetProjectKey(x), GetProjectKey(activeProject), StringComparison.OrdinalIgnoreCase)))
                activeProject = _selectedProjects.First();

            SetActiveProject(activeProject);
        }

        #region Grid Configuration

        public override IList<GridColumnDescriptor> GetGridColumns()
        {
            return new List<GridColumnDescriptor>
            {
                new GridColumnDescriptor("InvoiceNumber", "Invoice #", false, 120),
                new GridColumnDescriptor("InvoiceDate", "Date", false, 100, "d"),
                new GridColumnDescriptor("Amount", "Amount", false, 120, "N2"),
                new GridColumnDescriptor("VAT", "VAT", false, 100, "N2"),
                new GridColumnDescriptor("Paid", "Paid?", false, 60),
                new GridColumnDescriptor("PaidAmount", "Paid Amount", false, 120, "N2"),
                new GridColumnDescriptor("Bank", "Bank", false, 120),
                new GridColumnDescriptor("ProjectName", "Project", true, 150)
            };
        }

        public override IBindingList GetGridDataSource()
        {
            return _invoiceBindingList;
        }

        public override Stream GetSpreadsheetStream(out string format)
        {
            var bytes = ExportToXlsxBytes();
            if (bytes != null && bytes.Length > 0)
            {
                format = "xlsx";
                return new MemoryStream(bytes);
            }

            return base.GetSpreadsheetStream(out format);
        }

        #endregion

        #region Quick Actions

        public override IList<QuickActionDescriptor> GetQuickActions()
        {
            return new List<QuickActionDescriptor>
            {
                new QuickActionDescriptor(
                    "AddInvoice",
                    "Add Invoice",
                    () =>
                    {
                        var inv = new InvoiceModel
                        {
                            ProjectName = _project.ProjectName,
                            InvoiceDate = DateTime.Now
                        };
                        _invoiceBindingList.Add(inv);
                    }),
                new QuickActionDescriptor(
                    "RemoveInvoice",
                    "Remove Invoice",
                    () =>
                    {
                        if (_invoiceBindingList.Count > 0)
                            _invoiceBindingList.RemoveAt(_invoiceBindingList.Count - 1);
                    },
                    () => _invoiceBindingList.Count > 0)
            };
        }

        #endregion

        #region Recalculate & Summary

        public override void Recalculate()
        {
            _project.Invoices = _invoiceBindingList.ToList();
        }

        public override void ApplySpreadsheetChanges(IWorkbook workbook)
        {
            if (workbook == null)
                return;

            var invoicesSheet = workbook.Worksheets.FirstOrDefault(ws => string.Equals(ws.Name, "Invoices", StringComparison.OrdinalIgnoreCase));
            if (invoicesSheet != null)
            {
                var invoices = ReadInvoices(invoicesSheet);
                _project.Invoices = invoices;
                _invoiceBindingList = CreateInvoiceBindingList(invoices);
            }

            var addendumsSheet = workbook.Worksheets.FirstOrDefault(ws => string.Equals(ws.Name, "Addendums", StringComparison.OrdinalIgnoreCase));
            if (addendumsSheet != null)
                _project.Addendums = ReadAddendums(addendumsSheet);
        }

        public IEnumerable<string> GetEditableSheetNames()
        {
            yield return "Invoices";
            yield return "Addendums";
        }

        public override IList<KeyValuePair<string, string>> GetSummaryFields()
        {
            decimal initialAmount = _project.ContractDetails?.InitialContractAmount ?? 0m;
            decimal addendumTotal = 0m;
            if (_project.Addendums != null)
            {
                foreach (var a in _project.Addendums)
                    addendumTotal += a.Amount ?? 0m;
            }
            decimal contractTotal = initialAmount + addendumTotal;

            decimal invoicedTotal = 0m;
            decimal paidTotal = 0m;
            foreach (var inv in _invoiceBindingList)
            {
                invoicedTotal += inv.Amount ?? 0m;
                paidTotal += inv.PaidAmount ?? 0m;
            }

            decimal balance = contractTotal - invoicedTotal;

            var fields = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Type", _project.ProjectType ?? "-"),
                new KeyValuePair<string, string>("Reference", _project.Reference ?? "-"),
                new KeyValuePair<string, string>("Status", _project.Status.ToString()),
                new KeyValuePair<string, string>("Client", _project.ClientName ?? "-"),
                new KeyValuePair<string, string>("Initial Contract", initialAmount.ToString("N2")),
                new KeyValuePair<string, string>("Addendums", addendumTotal.ToString("N2")),
                new KeyValuePair<string, string>("Contract Total", contractTotal.ToString("N2")),
                new KeyValuePair<string, string>("Total Invoiced", invoicedTotal.ToString("N2")),
                new KeyValuePair<string, string>("Total Paid", paidTotal.ToString("N2")),
                new KeyValuePair<string, string>("Balance", balance.ToString("N2"))
            };

            return fields;
        }

        #endregion

        #region Field Descriptors

        public override IList<FieldDescriptor> GetFieldDescriptors()
        {
            var invoices = _project.Invoices ?? new List<InvoiceModel>();
            int invoiceCount = invoices.Count;
            var addendums = _project.Addendums ?? new List<AddendumModel>();
            int addendumCount = addendums.Count;
            var fields = new List<FieldDescriptor>();

            fields.AddRange(GetProjectInfoFields());
            fields.AddRange(GetDateFields());
            fields.AddRange(GetLocationFields());
            fields.AddRange(GetServiceFields());
            fields.AddRange(GetContractDetailFields());
            fields.AddRange(GetWarrantyAndDocumentFields());
            fields.AddRange(GetFinancialSummaryFields(invoices, addendums));
            fields.AddRange(GetInvoiceFields(invoices, invoiceCount));
            fields.AddRange(GetAddendumFields(addendums, addendumCount));

            return fields;
        }

        private IEnumerable<FieldDescriptor> GetProjectInfoFields()
        {
            yield return CreateProjectField("Project.ProjectType", "Project Type", "Project Info", typeof(string), null, project => project.ProjectType);
            yield return CreateProjectField("Project.Reference", "Project Reference", "Project Info", typeof(string), null, project => project.Reference);
            yield return CreateProjectField("Project.TentativeReference", "Tentative Reference", "Project Info", typeof(string), null, project => project.TentativeReference);
            yield return CreateProjectField("Project.ProjectName", "Project Name", "Project Info", typeof(string), null, project => project.ProjectName);
            yield return CreateProjectField("Project.Contract", "Contract", "Project Info", typeof(string), null, project => project.Contract);
            yield return CreateProjectField("Project.JointVenture", "Joint Venture", "Project Info", typeof(string), null, project => project.JointVenture);
            yield return CreateProjectField("Project.ClientName", "Client", "Project Info", typeof(string), null, project => project.ClientName);
            yield return CreateProjectField("Project.ClientContact", "Client Contact", "Project Info", typeof(string), null, project => project.ClientContact);
            yield return CreateProjectField("Project.FundedBy", "Funded By", "Project Info", typeof(string), null, project => project.ContractDetails?.SponsorId);
            yield return CreateProjectField("Project.EngineerInCharge", "Engineer in Charge", "Project Info", typeof(string), null, project => project.EngineerInCharge);
            yield return CreateProjectField("Project.Username", "Username", "Project Info", typeof(string), null, project => project.Username);
            yield return CreateProjectField("Project.Status", "Status", "Project Info", typeof(string), null, project => project.Status.ToString());
            yield return CreateProjectField("Project.Active", "Active", "Project Info", typeof(bool), null, project => project.Active);
            yield return CreateProjectField("Project.Area", "Area", "Project Info", typeof(string), null, project => project.Location?.Area);
            yield return CreateProjectField("Project.Bank", "Bank", "Project Info", typeof(string), null, project => GetProjectBank(project));
            yield return CreateProjectField("Project.ContractLink", "Contract Link", "Project Info", typeof(string), null, project => project.ContractDetails?.ContractFileLink);
            yield return CreateProjectField("Project.SourceFile", "Source File", "Project Info", typeof(string), null, project => project.SourceFile);
        }

        private IEnumerable<FieldDescriptor> GetDateFields()
        {
            yield return CreateProjectField("Project.YearOfIssuance", "Year of Issuance", "Dates", typeof(int?), null, project => project.YearOfIssuance);
            yield return CreateProjectField("Project.IssuanceDate", "Issuance Date", "Dates", typeof(DateTime?), "d", project => project.ProjectDate);
            yield return CreateProjectField("Project.ProjectDate", "Project Date", "Dates", typeof(DateTime?), "d", project => project.ProjectDate);
            yield return CreateProjectField("Project.SignatureDate", "Signature Date", "Dates", typeof(DateTime?), "d", project => project.ContractDetails?.SignatureDate);
            yield return CreateProjectField("Project.ExpiryDate", "Expiry Date", "Dates", typeof(DateTime?), "d", project => project.ExpiryDate);
        }

        private IEnumerable<FieldDescriptor> GetLocationFields()
        {
            yield return CreateProjectField("Project.Location.RawLocation", "Location", "Location", typeof(string), null, project => project.Location?.RawLocation ?? project.Location?.City);
            yield return CreateProjectField("Project.Location.Country", "Country", "Location", typeof(string), null, project => project.Location?.Country);
            yield return CreateProjectField("Project.Location.City", "City", "Location", typeof(string), null, project => project.Location?.City);
        }

        private IEnumerable<FieldDescriptor> GetServiceFields()
        {
            yield return CreateProjectField("Contract.Reference", "Contract Reference", "Services / Contract", typeof(string), null, project => project.ContractDetails?.ContractNumber ?? project.Contract);
            yield return CreateProjectField("Contract.InitialAmount", "Initial Contract Amount", "Services / Contract", typeof(decimal?), "N2", project => project.ContractDetails?.InitialContractAmount);
            yield return CreateProjectField("Contract.Currency", "Currency", "Services / Contract", typeof(string), null, project => project.ContractDetails?.CurrencyCode);
            yield return CreateProjectField("Contract.Retention", "Retention", "Services / Contract", typeof(decimal?), "N2", project => project.ContractDetails?.RetentionPercentage);
            yield return CreateProjectField("Contract.VAT", "VAT", "Services / Contract", typeof(decimal?), "N2", project => project.ContractDetails?.InitialVatAmount);
            yield return CreateProjectField("Contract.TTC", "TTC", "Services / Contract", typeof(decimal?), "N2", project => project.ContractDetails?.InitialTtcAmount);
            yield return CreateProjectField("Project.ServicesProvided", "Services Provided", "Services / Contract", typeof(string), null, project => string.Join(", ", project.ContractDetails?.ServicesProvided ?? new List<string>()));
            yield return CreateProjectField("Project.ServiceTypes", "Service Types", "Services / Contract", typeof(string), null, project => string.Join(", ", project.ContractDetails?.ServiceTypes ?? new List<string>()));
        }

        private IEnumerable<FieldDescriptor> GetContractDetailFields()
        {
            yield return CreateProjectField("ContractDetail.ContractNumber", "Contract Number", "Contract Details", typeof(string), null, project => project.ContractDetails?.ContractNumber);
            yield return CreateProjectField("ContractDetail.ContractFileLink", "Contract File Link", "Contract Details", typeof(string), null, project => project.ContractDetails?.ContractFileLink);
            yield return CreateProjectField("ContractDetail.SignatureDate", "Contract Signature Date", "Contract Details", typeof(DateTime?), "d", project => project.ContractDetails?.SignatureDate);
            yield return CreateProjectField("ContractDetail.ContractPeriod", "Contract Period", "Contract Details", typeof(string), null, project => project.ContractDetails?.ContractPeriod);
            yield return CreateProjectField("ContractDetail.ExtensionDetails", "Extension Details", "Contract Details", typeof(string), null, project => project.ContractDetails?.ExtensionDetails);
            yield return CreateProjectField("ContractDetail.ActualCompletionDate", "Actual Completion Date", "Contract Details", typeof(DateTime?), "d", project => project.ContractDetails?.ActualCompletionDate);
            yield return CreateProjectField("ContractDetail.ClientContactEmail", "Client Contact Email", "Contract Details", typeof(string), null, project => project.ContractDetails?.ClientContactEmail);
            yield return CreateProjectField("ContractDetail.CurrencyCode", "Currency Code", "Contract Details", typeof(string), null, project => project.ContractDetails?.CurrencyCode);
            yield return CreateProjectField("ContractDetail.DesignFee", "Design Fee", "Contract Details", typeof(decimal?), "N2", project => project.ContractDetails?.DesignFee);
            yield return CreateProjectField("ContractDetail.SupervisionFee", "Supervision Fee", "Contract Details", typeof(decimal?), "N2", project => project.ContractDetails?.SupervisionFee);
            yield return CreateProjectField("ContractDetail.InitialContractAmount", "Initial Amount (Detail)", "Contract Details", typeof(decimal?), "N2", project => project.ContractDetails?.InitialContractAmount);
            yield return CreateProjectField("ContractDetail.RetentionPercentage", "Retention %", "Contract Details", typeof(decimal?), "N2", project => project.ContractDetails?.RetentionPercentage);
            yield return CreateProjectField("ContractDetail.InitialVatAmount", "Initial VAT Amount", "Contract Details", typeof(decimal?), "N2", project => project.ContractDetails?.InitialVatAmount);
            yield return CreateProjectField("ContractDetail.InitialTtcAmount", "Initial TTC Amount", "Contract Details", typeof(decimal?), "N2", project => project.ContractDetails?.InitialTtcAmount);
        }

        private IEnumerable<FieldDescriptor> GetWarrantyAndDocumentFields()
        {
            yield return CreateProjectField("Warranty.Reference", "Warranty Reference", "Warranty", typeof(string), null, project => project.ContractDetails?.WarrantyReference);
            yield return CreateProjectField("Warranty.Amount", "Warranty Amount", "Warranty", typeof(decimal?), "N2", project => project.ContractDetails?.WarrantyAmount);
            yield return CreateProjectField("Warranty.Bank", "Warranty Bank", "Warranty", typeof(string), null, project => project.ContractDetails?.WarrantyBank);
            yield return CreateProjectField("Warranty.IssuanceDate", "Warranty Issuance Date", "Warranty", typeof(DateTime?), "d", project => project.ContractDetails?.WarrantyIssuanceDate);
            yield return CreateProjectField("Warranty.ExpiryDate", "Warranty Expiry Date", "Warranty", typeof(DateTime?), "d", project => project.ContractDetails?.WarrantyExpiryDate);
            yield return CreateProjectField("Warranty.Status", "Warranty Status", "Warranty", typeof(string), null, project => project.ContractDetails?.WarrantyStatus);
            yield return CreateProjectField("Documents.PreliminaryHandoverLink", "Preliminary Handover Link", "Documents", typeof(string), null, project => project.Documents?.PreliminaryHandoverLink);
            yield return CreateProjectField("Documents.PreliminaryHandoverDate", "Preliminary Handover Date", "Documents", typeof(DateTime?), "d", project => project.Documents?.PreliminaryHandoverDate);
            yield return CreateProjectField("Documents.FinalHandoverLink", "Final Handover Link", "Documents", typeof(string), null, project => project.Documents?.FinalHandoverLink);
            yield return CreateProjectField("Documents.FinalHandoverDate", "Final Handover Date", "Documents", typeof(DateTime?), "d", project => project.Documents?.FinalHandoverDate);
            yield return CreateProjectField("Documents.CompletionCertificateLink", "Completion Certificate Link", "Documents", typeof(string), null, project => project.Documents?.CompletionCertificateLink);
            yield return CreateProjectField("Documents.CompletionCertificateDate", "Completion Certificate Date", "Documents", typeof(DateTime?), "d", project => project.Documents?.CompletionCertificateDate);
        }

        private IEnumerable<FieldDescriptor> GetFinancialSummaryFields(List<InvoiceModel> invoices, List<AddendumModel> addendums)
        {
            yield return CreateProjectField("Summary.AddendumsTotal", "Addendums Total", "Financial Summary", typeof(decimal), "N2", project => (project.Addendums ?? new List<AddendumModel>()).Sum(x => x.Amount ?? 0m));
            yield return CreateProjectField("Summary.InvoicedTotal", "Invoiced Total", "Financial Summary", typeof(decimal), "N2", project => (project.Invoices ?? new List<InvoiceModel>()).Sum(x => x.Amount ?? 0m));
            yield return CreateProjectField("Summary.PaidTotal", "Paid Total", "Financial Summary", typeof(decimal), "N2", project => (project.Invoices ?? new List<InvoiceModel>()).Sum(x => x.PaidAmount ?? 0m));
            yield return CreateProjectField("Summary.ContractTotal", "Contract Total", "Financial Summary", typeof(decimal), "N2", project => (project.ContractDetails?.InitialContractAmount ?? 0m) + (project.Addendums ?? new List<AddendumModel>()).Sum(x => x.Amount ?? 0m));
            yield return CreateProjectField("Summary.Balance", "Balance", "Financial Summary", typeof(decimal), "N2", project => (project.ContractDetails?.InitialContractAmount ?? 0m) + (project.Addendums ?? new List<AddendumModel>()).Sum(x => x.Amount ?? 0m) - (project.Invoices ?? new List<InvoiceModel>()).Sum(x => x.Amount ?? 0m));
            yield return CreateProjectField("Summary.InvoicesCount", "Invoices Count", "Financial Summary", typeof(int), null, project => (project.Invoices ?? new List<InvoiceModel>()).Count);
            yield return CreateProjectField("Summary.AddendumsCount", "Addendums Count", "Financial Summary", typeof(int), null, project => (project.Addendums ?? new List<AddendumModel>()).Count);
        }

        private IEnumerable<FieldDescriptor> GetInvoiceFields(List<InvoiceModel> invoices, int invoiceCount)
        {
            yield return CreateProjectRowField("Invoice.InvoiceNumber", "Invoice #", "Invoice", typeof(string), null, project => project.Invoices ?? new List<InvoiceModel>(), invoice => invoice.InvoiceNumber);
            yield return CreateProjectRowField("Invoice.InvoiceDate", "Invoice Date", "Invoice", typeof(DateTime?), "d", project => project.Invoices ?? new List<InvoiceModel>(), invoice => invoice.InvoiceDate);
            yield return CreateProjectRowField("Invoice.Amount", "Amount", "Invoice", typeof(decimal?), "N2", project => project.Invoices ?? new List<InvoiceModel>(), invoice => invoice.Amount);
            yield return CreateProjectRowField("Invoice.VAT", "Invoice VAT", "Invoice", typeof(decimal?), "N2", project => project.Invoices ?? new List<InvoiceModel>(), invoice => invoice.VAT);
            yield return CreateProjectRowField("Invoice.Paid", "Paid?", "Invoice", typeof(bool), null, project => project.Invoices ?? new List<InvoiceModel>(), invoice => invoice.Paid);
            yield return CreateProjectRowField("Invoice.PaidAmount", "Paid Amount", "Invoice", typeof(decimal?), "N2", project => project.Invoices ?? new List<InvoiceModel>(), invoice => invoice.PaidAmount);
            yield return CreateProjectRowField("Invoice.Bank", "Invoice Bank", "Invoice", typeof(string), null, project => project.Invoices ?? new List<InvoiceModel>(), invoice => invoice.Bank);
            yield return CreateProjectRowField("Invoice.ProjectName", "Project (Invoice)", "Invoice", typeof(string), null, project => project.Invoices ?? new List<InvoiceModel>(), invoice => invoice.ProjectName);
        }

        private IEnumerable<FieldDescriptor> GetAddendumFields(List<AddendumModel> addendums, int addendumCount)
        {
            yield return CreateProjectRowField("Addendum.Sequence", "Addendum #", "Addendum", typeof(int), null, project => project.Addendums ?? new List<AddendumModel>(), addendum => addendum.Sequence);
            yield return CreateProjectRowField("Addendum.Subject", "Addendum Subject", "Addendum", typeof(string), null, project => project.Addendums ?? new List<AddendumModel>(), addendum => addendum.Subject);
            yield return CreateProjectRowField("Addendum.Reference", "Addendum Reference", "Addendum", typeof(string), null, project => project.Addendums ?? new List<AddendumModel>(), addendum => addendum.Reference);
            yield return CreateProjectRowField("Addendum.DecisionNo", "Decision No", "Addendum", typeof(string), null, project => project.Addendums ?? new List<AddendumModel>(), addendum => addendum.DecisionNo);
            yield return CreateProjectRowField("Addendum.BODDate", "BOD Date", "Addendum", typeof(DateTime?), "d", project => project.Addendums ?? new List<AddendumModel>(), addendum => addendum.BODDate);
            yield return CreateProjectRowField("Addendum.EffectiveDate", "Effective Date", "Addendum", typeof(DateTime?), "d", project => project.Addendums ?? new List<AddendumModel>(), addendum => addendum.EffectiveDate);
            yield return CreateProjectRowField("Addendum.Amount", "Addendum Amount", "Addendum", typeof(decimal?), "N2", project => project.Addendums ?? new List<AddendumModel>(), addendum => addendum.Amount);
            yield return CreateProjectRowField("Addendum.VAT", "Addendum VAT", "Addendum", typeof(decimal?), "N2", project => project.Addendums ?? new List<AddendumModel>(), addendum => addendum.VAT);
            yield return CreateProjectRowField("Addendum.Retention", "Addendum Retention", "Addendum", typeof(decimal?), "N2", project => project.Addendums ?? new List<AddendumModel>(), addendum => addendum.Retention);
            yield return CreateProjectRowField("Addendum.TTC", "Addendum TTC", "Addendum", typeof(decimal?), "N2", project => project.Addendums ?? new List<AddendumModel>(), addendum => addendum.TTC);
        }

        private FieldDescriptor CreateProjectField(string key, string caption, string category, Type valueType, string formatString, Func<ProjectModel, object> valueAccessor)
        {
            return new FieldDescriptor(
                key,
                caption,
                category,
                valueType,
                formatString,
                () => valueAccessor(_project),
                null,
                0,
                selectedIndex =>
                {
                    var project = GetSelectedProject(selectedIndex);
                    return project == null ? null : valueAccessor(project);
                });
        }

        private FieldDescriptor CreateProjectRowField<TItem>(string key, string caption, string category, Type valueType, string formatString, Func<ProjectModel, IList<TItem>> itemsAccessor, Func<TItem, object> valueAccessor)
        {
            return new FieldDescriptor(
                key,
                caption,
                category,
                valueType,
                formatString,
                null,
                rowIndex => GetProjectRowValue(_project, itemsAccessor, valueAccessor, rowIndex),
                GetProjectRowCount(_project, itemsAccessor),
                null,
                (selectedIndex, rowIndex) => GetProjectRowValue(GetSelectedProject(selectedIndex), itemsAccessor, valueAccessor, rowIndex),
                selectedIndex => GetProjectRowCount(GetSelectedProject(selectedIndex), itemsAccessor));
        }

        private static object GetProjectRowValue<TItem>(ProjectModel project, Func<ProjectModel, IList<TItem>> itemsAccessor, Func<TItem, object> valueAccessor, int rowIndex)
        {
            var items = itemsAccessor(project) ?? new List<TItem>();
            return rowIndex >= 0 && rowIndex < items.Count ? valueAccessor(items[rowIndex]) : null;
        }

        private static int GetProjectRowCount<TItem>(ProjectModel project, Func<ProjectModel, IList<TItem>> itemsAccessor)
        {
            var items = itemsAccessor(project) ?? new List<TItem>();
            return items.Count;
        }

        private ProjectModel GetSelectedProject(int selectedIndex)
        {
            if (selectedIndex < 0 || selectedIndex >= _selectedProjects.Count)
                return null;

            return _selectedProjects[selectedIndex];
        }

        private ReportRecordDescriptor CreateRecordDescriptor(ProjectModel project)
        {
            return new ReportRecordDescriptor
            {
                Key = GetProjectKey(project),
                Caption = string.IsNullOrWhiteSpace(project.ProjectName) ? "Unnamed Project" : project.ProjectName,
                Model = project
            };
        }

        private void SetActiveProject(ProjectModel project)
        {
            var resolvedProject = ResolveProject(project) ?? _selectedProjects.FirstOrDefault() ?? _availableProjects.First();
            _project = resolvedProject;
            EnsureActiveProjectIsSelected();
            _invoiceBindingList = CreateInvoiceBindingList(_project.Invoices);
        }

        private void EnsureActiveProjectIsSelected()
        {
            if (_project == null)
                return;

            if (_selectedProjects.Any(x => string.Equals(GetProjectKey(x), GetProjectKey(_project), StringComparison.OrdinalIgnoreCase)))
                return;

            _selectedProjects.Insert(0, _project);
        }

        private ProjectModel ResolveProject(ProjectModel project)
        {
            if (project == null)
                return null;

            return _availableProjects.FirstOrDefault(x => string.Equals(GetProjectKey(x), GetProjectKey(project), StringComparison.OrdinalIgnoreCase)) ?? project;
        }

        private List<ProjectModel> BuildProjectSelection(IList<ProjectModel> selectedProjects, ProjectModel fallbackProject)
        {
            var selectedKeys = (selectedProjects ?? new List<ProjectModel>())
                .Where(x => x != null)
                .Select(GetProjectKey)
                .ToList();

            var projects = _availableProjects
                .Where(x => selectedKeys.Contains(GetProjectKey(x)))
                .ToList();

            if (projects.Count == 0)
            {
                var fallbackKey = GetProjectKey(fallbackProject);
                var fallback = _availableProjects.FirstOrDefault(x => string.Equals(GetProjectKey(x), fallbackKey, StringComparison.OrdinalIgnoreCase))
                    ?? _availableProjects.First();
                projects.Add(fallback);
            }

            return projects;
        }

        private static BindingList<InvoiceModel> CreateInvoiceBindingList(List<InvoiceModel> invoices)
        {
            var bindingList = new BindingList<InvoiceModel>(invoices ?? new List<InvoiceModel>());
            bindingList.AllowNew = true;
            bindingList.AllowEdit = true;
            bindingList.AllowRemove = true;
            return bindingList;
        }

        private static string GetProjectKey(ProjectModel project)
        {
            if (project == null)
                return string.Empty;

            if (!string.IsNullOrWhiteSpace(project._id))
                return project._id;

            return project.ProjectName ?? string.Empty;
        }

        private string GetProjectBank(ProjectModel project)
        {
            var invoices = project?.Invoices ?? new List<InvoiceModel>();
            var invoiceBank = invoices.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.Bank))?.Bank;

            return !string.IsNullOrWhiteSpace(invoiceBank)
                ? invoiceBank
                : project?.ContractDetails?.WarrantyBank;
        }

        #endregion

        #region CSV Export

        public override string ExportToCsvString()
        {
            var headers = new[]
            {
                "Project Reference", "Project Name", "Client", "Status",
                "Invoice #", "Invoice Date", "Amount", "VAT", "Paid", "Paid Amount", "Bank"
            };

            var rows = new List<string[]>();
            foreach (var inv in _invoiceBindingList)
            {
                rows.Add(new[]
                {
                    _project.Reference ?? "",
                    _project.ProjectName ?? "",
                    _project.ClientName ?? "",
                    _project.Status.ToString(),
                    inv.InvoiceNumber ?? "",
                    inv.InvoiceDate?.ToString("yyyy-MM-dd") ?? "",
                    (inv.Amount ?? 0m).ToString("F2"),
                    (inv.VAT ?? 0m).ToString("F2"),
                    inv.Paid ? "Yes" : "No",
                    (inv.PaidAmount ?? 0m).ToString("F2"),
                    inv.Bank ?? ""
                });
            }

            return CsvHelper.BuildCsv(headers, rows);
        }

        public override byte[] ExportToXlsxBytes()
        {
            using (var spreadsheet = new SpreadsheetControl())
            {
                BuildWorkbook(spreadsheet.Document);

                using (var stream = new MemoryStream())
                {
                    spreadsheet.SaveDocument(stream, DocumentFormat.Xlsx);
                    return stream.ToArray();
                }
            }
        }

        #endregion

        private void BuildWorkbook(IWorkbook workbook)
        {
            if (workbook == null) throw new ArgumentNullException(nameof(workbook));
            workbook.Unit = DevExpress.Office.DocumentUnit.Inch;

            // Sheet 1: Project Summary
            var summary = workbook.Worksheets[0];
            summary.Name = "Project Summary";
            BuildSummaryWorksheet(summary);

            // Sheet 2: Contract Details
            var contractSheet = workbook.Worksheets.Add("Contract Details");
            BuildContractDetailsWorksheet(contractSheet);

            // Sheet 3: Invoices
            var invoicesSheet = workbook.Worksheets.Add("Invoices");
            BuildInvoicesWorksheet(invoicesSheet);

            // Sheet 4: Addendums
            var addendumsSheet = workbook.Worksheets.Add("Addendums");
            BuildAddendumsWorksheet(addendumsSheet);

            // Sheet 5: Warranty & Documents
            var warrantySheet = workbook.Worksheets.Add("Warranty & Documents");
            BuildWarrantyDocumentsWorksheet(warrantySheet);

            invoicesSheet.FreezeRows(1);
            addendumsSheet.FreezeRows(1);
            ReportSpreadsheetHelper.ApplyDefaultPrintSettings(workbook, summary);
        }

        private void BuildSummaryWorksheet(Worksheet worksheet)
        {
            var headerColor = System.Drawing.Color.FromArgb(70, 130, 180);
            var labelColor = System.Drawing.Color.FromArgb(240, 248, 255);

            // Title row
            worksheet.Range["A1:H1"].Merge();
            worksheet["A1"].Value = Title;
            worksheet["A1"].Font.Bold = true;
            worksheet["A1"].Font.Size = 16;
            worksheet["A1"].Font.Color = System.Drawing.Color.White;
            worksheet["A1"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
            worksheet["A1"].Fill.BackgroundColor = headerColor;

            // Section: Project Information
            int row = 2;
            row = WriteSectionHeader(worksheet, row, "Project Information", headerColor);
            row = WriteKeyValue(worksheet, row, "Project Reference", _project.Reference, labelColor);
            row = WriteKeyValue(worksheet, row, "Tentative Reference", _project.TentativeReference, labelColor);
            row = WriteKeyValue(worksheet, row, "Project Name", _project.ProjectName, labelColor);
            row = WriteKeyValue(worksheet, row, "Contract", _project.Contract, labelColor);
            row = WriteKeyValue(worksheet, row, "Joint Venture", _project.JointVenture, labelColor);
            row = WriteKeyValue(worksheet, row, "Status", _project.Status.ToString(), labelColor);
            row = WriteKeyValue(worksheet, row, "Area", _project.Location?.Area, labelColor);

            // Section: People & Client
            row = WriteSectionHeader(worksheet, row, "People & Client", headerColor);
            row = WriteKeyValue(worksheet, row, "Client", _project.ClientName, labelColor);
            row = WriteKeyValue(worksheet, row, "Client Contact", _project.ClientContact, labelColor);
            row = WriteKeyValue(worksheet, row, "Funded By", _project.ContractDetails?.SponsorId, labelColor);
            row = WriteKeyValue(worksheet, row, "Engineer in Charge", _project.EngineerInCharge, labelColor);
            row = WriteKeyValue(worksheet, row, "Username", _project.Username, labelColor);

            // Section: Location
            row = WriteSectionHeader(worksheet, row, "Location", headerColor);
            row = WriteKeyValue(worksheet, row, "Location", _project.Location?.RawLocation, labelColor);
            row = WriteKeyValue(worksheet, row, "Country", _project.Location?.Country, labelColor);
            row = WriteKeyValue(worksheet, row, "City", _project.Location?.City, labelColor);

            // Section: Key Dates
            row = WriteSectionHeader(worksheet, row, "Key Dates", headerColor);
            row = WriteKeyValueDate(worksheet, row, "Year of Issuance", _project.YearOfIssuance?.ToString(), labelColor);
            row = WriteKeyValueDate(worksheet, row, "Project Date", _project.ProjectDate, labelColor);
            row = WriteKeyValueDate(worksheet, row, "Signature Date", _project.ContractDetails?.SignatureDate, labelColor);
            row = WriteKeyValueDate(worksheet, row, "Expiry Date", _project.ExpiryDate, labelColor);

            // Section: Financial Overview (right side)
            int finRow = 2;
            finRow = WriteSectionHeader(worksheet, finRow, "Financial Overview", headerColor, 4);
            decimal initialAmount = _project.ContractDetails?.InitialContractAmount ?? 0m;
            decimal addendumTotal = (_project.Addendums ?? new List<AddendumModel>()).Sum(x => x.Amount ?? 0m);
            decimal contractTotal = initialAmount + addendumTotal;
            decimal invoicedTotal = _invoiceBindingList.Sum(x => x.Amount ?? 0m);
            decimal paidTotal = _invoiceBindingList.Sum(x => x.PaidAmount ?? 0m);

            worksheet.Cells[finRow, 4].Value = "Initial Contract";
            worksheet.Cells[finRow, 4].Font.Bold = true;
            worksheet.Cells[finRow, 5].Value = (double)initialAmount;
            worksheet.Cells[finRow, 5].NumberFormat = "#,##0.00";
            finRow++;
            worksheet.Cells[finRow, 4].Value = "Addendums Total";
            worksheet.Cells[finRow, 4].Font.Bold = true;
            worksheet.Cells[finRow, 5].Value = (double)addendumTotal;
            worksheet.Cells[finRow, 5].NumberFormat = "#,##0.00";
            finRow++;
            worksheet.Cells[finRow, 4].Value = "Contract Total";
            worksheet.Cells[finRow, 4].Font.Bold = true;
            worksheet.Cells[finRow, 5].Formula = string.Format("=F{0}+F{1}", finRow - 1, finRow);
            worksheet.Cells[finRow, 5].NumberFormat = "#,##0.00";
            worksheet.Cells[finRow, 5].Font.Bold = true;
            finRow++;
            worksheet.Cells[finRow, 4].Value = "Total Invoiced";
            worksheet.Cells[finRow, 4].Font.Bold = true;
            worksheet.Cells[finRow, 5].Value = (double)invoicedTotal;
            worksheet.Cells[finRow, 5].NumberFormat = "#,##0.00";
            finRow++;
            worksheet.Cells[finRow, 4].Value = "Total Paid";
            worksheet.Cells[finRow, 4].Font.Bold = true;
            worksheet.Cells[finRow, 5].Value = (double)paidTotal;
            worksheet.Cells[finRow, 5].NumberFormat = "#,##0.00";
            finRow++;
            worksheet.Cells[finRow, 4].Value = "Balance";
            worksheet.Cells[finRow, 4].Font.Bold = true;
            worksheet.Cells[finRow, 4].Font.Color = System.Drawing.Color.DarkRed;
            worksheet.Cells[finRow, 5].Value = (double)(contractTotal - invoicedTotal);
            worksheet.Cells[finRow, 5].NumberFormat = "#,##0.00";
            worksheet.Cells[finRow, 5].Font.Bold = true;
            worksheet.Cells[finRow, 5].Font.Color = System.Drawing.Color.DarkRed;

            worksheet.Columns.AutoFit(0, 5);
            worksheet.Columns[0].Width = 200;
            worksheet.Columns[1].Width = 250;
            worksheet.Columns[4].Width = 180;
            worksheet.Columns[5].Width = 150;
        }

        private int WriteSectionHeader(Worksheet ws, int row, string title, System.Drawing.Color color, int startCol = 0)
        {
            ws.Cells[row, startCol].Value = title;
            ws.Cells[row, startCol].Font.Bold = true;
            ws.Cells[row, startCol].Font.Size = 11;
            ws.Cells[row, startCol].Font.Color = System.Drawing.Color.White;
            var range = ws.Range.FromLTRB(startCol, row, startCol + 1, row);
            range.Merge();
            range.Fill.BackgroundColor = color;
            return row + 1;
        }

        private int WriteKeyValue(Worksheet ws, int row, string label, string value, System.Drawing.Color bg)
        {
            ws.Cells[row, 0].Value = label;
            ws.Cells[row, 0].Font.Bold = true;
            ws.Cells[row, 0].Fill.BackgroundColor = bg;
            ws.Cells[row, 1].Value = value ?? string.Empty;
            return row + 1;
        }

        private int WriteKeyValueDate(Worksheet ws, int row, string label, object value, System.Drawing.Color bg)
        {
            ws.Cells[row, 0].Value = label;
            ws.Cells[row, 0].Font.Bold = true;
            ws.Cells[row, 0].Fill.BackgroundColor = bg;
            try
            {
                if (value == null)
                {
                    ws.Cells[row, 1].Value = string.Empty;
                }
                else if (value is DateTime dt)
                {
                    ws.Cells[row, 1].Value = dt;
                    ws.Cells[row, 1].NumberFormat = "yyyy-mm-dd";
                }
                else if (value is double od)
                {
                    // treat as OADate
                    var dt2 = DateTime.FromOADate(od);
                    ws.Cells[row, 1].Value = dt2;
                    ws.Cells[row, 1].NumberFormat = "yyyy-mm-dd";
                }
                else if (value is long l)
                {
                    // could be ticks or yyyymmdd numeric; try to detect
                    if (l > 10000000000L)
                    {
                        ws.Cells[row, 1].Value = new DateTime(l);
                        ws.Cells[row, 1].NumberFormat = "yyyy-mm-dd";
                    }
                    else if (l >= 10000101L && l <= 99991231L)
                    {
                        // yyyymmdd
                        var s = l.ToString();
                        if (DateTime.TryParseExact(s, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt3))
                        {
                            ws.Cells[row, 1].Value = dt3;
                            ws.Cells[row, 1].NumberFormat = "yyyy-mm-dd";
                        }
                        else
                        {
                            ws.Cells[row, 1].Value = s;
                        }
                    }
                    else
                    {
                        ws.Cells[row, 1].Value = value.ToString();
                    }
                }
                else if (value is int i)
                {
                    if (i >= 10000101 && i <= 99991231)
                    {
                        if (DateTime.TryParseExact(i.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt4))
                        {
                            ws.Cells[row, 1].Value = dt4;
                            ws.Cells[row, 1].NumberFormat = "yyyy-mm-dd";
                        }
                    }
                    else
                    {
                        ws.Cells[row, 1].Value = value.ToString();
                    }
                }
                else
                {
                    // fallback to string
                    ws.Cells[row, 1].Value = value.ToString();
                }
            }
            catch
            {
                ws.Cells[row, 1].Value = value?.ToString() ?? string.Empty;
            }
            return row + 1;
        }

        private void BuildContractDetailsWorksheet(Worksheet worksheet)
        {
            var headerColor = System.Drawing.Color.FromArgb(70, 130, 180);
            var cd = _project.ContractDetails;

            worksheet.Range["A1:H1"].Merge();
            worksheet["A1"].Value = "Contract Details \u0633 " + (_project.ProjectName ?? "Project");
            worksheet["A1"].Font.Bold = true;
            worksheet["A1"].Font.Size = 14;
            worksheet["A1"].Font.Color = System.Drawing.Color.White;
            worksheet["A1"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
            worksheet["A1"].Fill.BackgroundColor = headerColor;

            var headers = new[] { "Field", "Value" };
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[2, i].Value = headers[i];
                worksheet.Cells[2, i].Font.Bold = true;
                worksheet.Cells[2, i].Fill.BackgroundColor = System.Drawing.Color.Gainsboro;
            }

            int row = 3;
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Contract Number", cd?.ContractNumber);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Contract File Link", cd?.ContractFileLink);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Signature Date", cd?.SignatureDate);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Contract Period", cd?.ContractPeriod);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Extension Details", cd?.ExtensionDetails);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Actual Completion Date", cd?.ActualCompletionDate);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Client Contact Email", cd?.ClientContactEmail);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Currency Code", cd?.CurrencyCode);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Design Fee", cd?.DesignFee, "#,##0.00");
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Supervision Fee", cd?.SupervisionFee, "#,##0.00");
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Initial Contract Amount", cd?.InitialContractAmount, "#,##0.00");
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Retention %", cd?.RetentionPercentage, "#,##0.00");
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Initial VAT Amount", cd?.InitialVatAmount, "#,##0.00");
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Initial TTC Amount", cd?.InitialTtcAmount, "#,##0.00");

            worksheet.Columns.AutoFit(0, 1);
            worksheet.Columns[0].Width = 220;
            worksheet.Columns[1].Width = 280;
        }

        private void BuildInvoicesWorksheet(Worksheet worksheet)
        {
            var headerColor = System.Drawing.Color.FromArgb(70, 130, 180);

            worksheet.Range["A1:H1"].Merge();
            worksheet["A1"].Value = "Invoices \u0633 " + (_project.ProjectName ?? "Project");
            worksheet["A1"].Font.Bold = true;
            worksheet["A1"].Font.Size = 14;
            worksheet["A1"].Font.Color = System.Drawing.Color.White;
            worksheet["A1"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
            worksheet["A1"].Fill.BackgroundColor = headerColor;

            var headers = new[] { "Invoice #", "Invoice Date", "Amount", "VAT", "Paid", "Paid Amount", "Bank", "Project" };
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[1, i].Value = headers[i];
                worksheet.Cells[1, i].Font.Bold = true;
                worksheet.Cells[1, i].Fill.BackgroundColor = System.Drawing.Color.Gainsboro;
            }

            for (int row = 0; row < _invoiceBindingList.Count; row++)
            {
                var invoice = _invoiceBindingList[row];
                var targetRow = row + 2;

                worksheet.Cells[targetRow, 0].Value = invoice.InvoiceNumber ?? string.Empty;
                worksheet.Cells[targetRow, 1].Value = invoice.InvoiceDate;
                worksheet.Cells[targetRow, 2].Value = (double)(invoice.Amount ?? 0m);
                worksheet.Cells[targetRow, 3].Value = (double)(invoice.VAT ?? 0m);
                worksheet.Cells[targetRow, 4].Value = invoice.Paid ? "Yes" : "No";
                worksheet.Cells[targetRow, 5].Value = (double)(invoice.PaidAmount ?? 0m);
                worksheet.Cells[targetRow, 6].Value = invoice.Bank ?? string.Empty;
                worksheet.Cells[targetRow, 7].Value = invoice.ProjectName ?? _project.ProjectName ?? string.Empty;
            }

            if (_invoiceBindingList.Count > 0)
            {
                var lastRow = _invoiceBindingList.Count + 2;
                worksheet.Cells[lastRow, 1].Value = "Totals";
                worksheet.Cells[lastRow, 1].Font.Bold = true;
                worksheet.Cells[lastRow, 2].Formula = string.Format("=SUM(C3:C{0})", _invoiceBindingList.Count + 2);
                worksheet.Cells[lastRow, 3].Formula = string.Format("=SUM(D3:D{0})", _invoiceBindingList.Count + 2);
                worksheet.Cells[lastRow, 5].Formula = string.Format("=SUM(F3:F{0})", _invoiceBindingList.Count + 2);
                worksheet.Range[string.Format("C{0}:F{0}", lastRow + 1)].NumberFormat = "#,##0.00";
                worksheet.Range[string.Format("C{0}:F{0}", lastRow + 1)].Font.Bold = true;
            }

            worksheet.Range["B:B"].NumberFormat = "yyyy-mm-dd";
            worksheet.Range["C:F"].NumberFormat = "#,##0.00";
            worksheet.Columns.AutoFit(0, 7);
            worksheet.FreezeRows(2);
        }

        private void BuildAddendumsWorksheet(Worksheet worksheet)
        {
            var headerColor = System.Drawing.Color.FromArgb(70, 130, 180);
            var addendums = _project.Addendums ?? new List<AddendumModel>();

            worksheet.Range["A1:J1"].Merge();
            worksheet["A1"].Value = "Addendums \u0633 " + (_project.ProjectName ?? "Project");
            worksheet["A1"].Font.Bold = true;
            worksheet["A1"].Font.Size = 14;
            worksheet["A1"].Font.Color = System.Drawing.Color.White;
            worksheet["A1"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
            worksheet["A1"].Fill.BackgroundColor = headerColor;

            var headers = new[] { "Seq #", "Title", "Reference", "Decision No", "BOD Date", "Effective Date", "Amount", "VAT", "Retention", "TTC" };
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[1, i].Value = headers[i];
                worksheet.Cells[1, i].Font.Bold = true;
                worksheet.Cells[1, i].Fill.BackgroundColor = System.Drawing.Color.Gainsboro;
            }

            for (int row = 0; row < addendums.Count; row++)
            {
                var addendum = addendums[row];
                var targetRow = row + 2;

                worksheet.Cells[targetRow, 0].Value = addendum.Sequence;
                worksheet.Cells[targetRow, 1].Value = addendum.Subject ?? string.Empty;
                worksheet.Cells[targetRow, 2].Value = addendum.Reference ?? string.Empty;
                worksheet.Cells[targetRow, 3].Value = addendum.DecisionNo ?? string.Empty;
                worksheet.Cells[targetRow, 4].Value = addendum.BODDate;
                worksheet.Cells[targetRow, 5].Value = addendum.EffectiveDate;
                worksheet.Cells[targetRow, 6].Value = (double)(addendum.Amount ?? 0m);
                worksheet.Cells[targetRow, 7].Value = (double)(addendum.VAT ?? 0m);
                worksheet.Cells[targetRow, 8].Value = (double)(addendum.Retention ?? 0m);
                worksheet.Cells[targetRow, 9].Value = (double)(addendum.TTC ?? 0m);
            }

            if (addendums.Count > 0)
            {
                var lastRow = addendums.Count + 2;
                worksheet.Cells[lastRow, 5].Value = "Totals";
                worksheet.Cells[lastRow, 5].Font.Bold = true;
                worksheet.Cells[lastRow, 6].Formula = string.Format("=SUM(G3:G{0})", addendums.Count + 2);
                worksheet.Cells[lastRow, 7].Formula = string.Format("=SUM(H3:H{0})", addendums.Count + 2);
                worksheet.Cells[lastRow, 8].Formula = string.Format("=SUM(I3:I{0})", addendums.Count + 2);
                worksheet.Cells[lastRow, 9].Formula = string.Format("=SUM(J3:J{0})", addendums.Count + 2);
                worksheet.Range[string.Format("G{0}:J{0}", lastRow + 1)].Font.Bold = true;
            }

            worksheet.Range["E:F"].NumberFormat = "yyyy-mm-dd";
            worksheet.Range["G:J"].NumberFormat = "#,##0.00";
            worksheet.Columns.AutoFit(0, 9);
            worksheet.FreezeRows(2);
        }

        private void BuildWarrantyDocumentsWorksheet(Worksheet worksheet)
        {
            var headerColor = System.Drawing.Color.FromArgb(70, 130, 180);
            var labelColor = System.Drawing.Color.FromArgb(240, 248, 255);
            var cd = _project.ContractDetails;
            var docs = _project.Documents;

            worksheet.Range["A1:D1"].Merge();
            worksheet["A1"].Value = "Warranty & Documents \u0633 " + (_project.ProjectName ?? "Project");
            worksheet["A1"].Font.Bold = true;
            worksheet["A1"].Font.Size = 14;
            worksheet["A1"].Font.Color = System.Drawing.Color.White;
            worksheet["A1"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
            worksheet["A1"].Fill.BackgroundColor = headerColor;

            // Warranty section
            int row = 2;
            worksheet.Cells[row, 0].Value = "Warranty Information";
            worksheet.Cells[row, 0].Font.Bold = true;
            worksheet.Cells[row, 0].Font.Color = System.Drawing.Color.White;
            var warrantyRange = worksheet.Range.FromLTRB(0, row, 1, row);
            warrantyRange.Merge();
            warrantyRange.Fill.BackgroundColor = headerColor;
            row++;

            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Warranty Reference", cd?.WarrantyReference);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Warranty Amount", cd?.WarrantyAmount, "#,##0.00");
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Warranty Bank", cd?.WarrantyBank);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Issuance Date", cd?.WarrantyIssuanceDate);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Expiry Date", cd?.WarrantyExpiryDate);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Warranty Status", cd?.WarrantyStatus);
            row++;

            // Documents section
            worksheet.Cells[row, 0].Value = "Documents";
            worksheet.Cells[row, 0].Font.Bold = true;
            worksheet.Cells[row, 0].Font.Color = System.Drawing.Color.White;
            var docsRange = worksheet.Range.FromLTRB(0, row, 1, row);
            docsRange.Merge();
            docsRange.Fill.BackgroundColor = headerColor;
            row++;

            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Preliminary Handover Link", docs?.PreliminaryHandoverLink);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Preliminary Handover Date", docs?.PreliminaryHandoverDate);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Final Handover Link", docs?.FinalHandoverLink);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Final Handover Date", docs?.FinalHandoverDate);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Completion Certificate Link", docs?.CompletionCertificateLink);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Completion Certificate Date", docs?.CompletionCertificateDate);
            row++;

            // Contract Link
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Contract Link", cd?.ContractFileLink);

            worksheet.Columns.AutoFit(0, 1);
            worksheet.Columns[0].Width = 220;
            worksheet.Columns[1].Width = 350;
        }

        private List<InvoiceModel> ReadInvoices(Worksheet worksheet)
        {
            var invoices = new List<InvoiceModel>();
            var usedRange = worksheet.GetUsedRange();
            if (usedRange == null)
                return invoices;

            for (int row = 2; row <= usedRange.BottomRowIndex; row++)
            {
                if (string.Equals(ReportSpreadsheetHelper.GetCellText(worksheet, row, 1), "Totals", StringComparison.OrdinalIgnoreCase))
                    continue;

                var invoice = new InvoiceModel
                {
                    InvoiceNumber = ReportSpreadsheetHelper.GetCellText(worksheet, row, 0),
                    InvoiceDate = ReportSpreadsheetHelper.GetCellDate(worksheet, row, 1),
                    Amount = ReportSpreadsheetHelper.GetCellDecimal(worksheet, row, 2),
                    VAT = ReportSpreadsheetHelper.GetCellDecimal(worksheet, row, 3),
                    Paid = ReportSpreadsheetHelper.GetCellBoolean(worksheet, row, 4),
                    PaidAmount = ReportSpreadsheetHelper.GetCellDecimal(worksheet, row, 5),
                    Bank = ReportSpreadsheetHelper.GetCellText(worksheet, row, 6),
                    ProjectName = string.IsNullOrWhiteSpace(ReportSpreadsheetHelper.GetCellText(worksheet, row, 7)) ? _project.ProjectName : ReportSpreadsheetHelper.GetCellText(worksheet, row, 7)
                };

                if (string.IsNullOrWhiteSpace(invoice.InvoiceNumber)
                    && !invoice.InvoiceDate.HasValue
                    && !invoice.Amount.HasValue
                    && !invoice.VAT.HasValue
                    && string.IsNullOrWhiteSpace(invoice.Bank)
                    && !invoice.PaidAmount.HasValue)
                {
                    continue;
                }

                invoices.Add(invoice);
            }

            return invoices;
        }

        private List<AddendumModel> ReadAddendums(Worksheet worksheet)
        {
            var addendums = new List<AddendumModel>();
            var usedRange = worksheet.GetUsedRange();
            if (usedRange == null)
                return addendums;

            for (int row = 2; row <= usedRange.BottomRowIndex; row++)
            {
                if (string.Equals(ReportSpreadsheetHelper.GetCellText(worksheet, row, 5), "Totals", StringComparison.OrdinalIgnoreCase))
                    continue;

                var addendum = new AddendumModel
                {
                    Sequence = ReportSpreadsheetHelper.GetCellInt(worksheet, row, 0),
                    Subject = ReportSpreadsheetHelper.GetCellText(worksheet, row, 1),
                    Reference = ReportSpreadsheetHelper.GetCellText(worksheet, row, 2),
                    DecisionNo = ReportSpreadsheetHelper.GetCellText(worksheet, row, 3),
                    BODDate = ReportSpreadsheetHelper.GetCellDate(worksheet, row, 4),
                    EffectiveDate = ReportSpreadsheetHelper.GetCellDate(worksheet, row, 5),
                    Amount = ReportSpreadsheetHelper.GetCellDecimal(worksheet, row, 6),
                    VAT = ReportSpreadsheetHelper.GetCellDecimal(worksheet, row, 7),
                    Retention = ReportSpreadsheetHelper.GetCellDecimal(worksheet, row, 8),
                    TTC = ReportSpreadsheetHelper.GetCellDecimal(worksheet, row, 9)
                };

                if (addendum.Sequence == 0
                    && string.IsNullOrWhiteSpace(addendum.Subject)
                    && string.IsNullOrWhiteSpace(addendum.Reference)
                    && string.IsNullOrWhiteSpace(addendum.DecisionNo)
                    && !addendum.BODDate.HasValue
                    && !addendum.EffectiveDate.HasValue
                    && !addendum.Amount.HasValue
                    && !addendum.VAT.HasValue
                    && !addendum.Retention.HasValue
                    && !addendum.TTC.HasValue)
                {
                    continue;
                }

                addendums.Add(addendum);
            }

            return addendums;
        }
    }
}
