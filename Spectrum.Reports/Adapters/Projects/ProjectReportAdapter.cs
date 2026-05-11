using Spectrum.Models.Projects;
using Spectrum.Reports.Adapters;
using Spectrum.Reports.Common;
using Spectrum.Reports.Exporters;
using Spectrum.Reports.Interfaces;
using DevExpress.Spreadsheet;
using DevExpress.XtraSpreadsheet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.IO;

namespace Spectrum.Reports.Adapters.Projects
{
	/// <summary>
	/// Report adapter for <see cref="ProjectModel"/>.
	/// Grid: project invoices (editable).
	/// Summary: contract totals, invoiced totals, balance.
	/// CSV: full invoice listing.
	/// </summary>
	public class ProjectReportAdapter : ReportAdapterBase
		, Spectrum.Reports.Interfaces.IWorksheetRequirements
	{
		private readonly ProjectModel _project;
		private BindingList<InvoiceModel> _invoiceBindingList;

		public ProjectReportAdapter(ProjectModel project)
		{
			_project = project ?? throw new ArgumentNullException(nameof(project));
			_invoiceBindingList = new BindingList<InvoiceModel>(_project.Invoices ?? new List<InvoiceModel>());
			_invoiceBindingList.AllowNew = true;
			_invoiceBindingList.AllowEdit = true;
			_invoiceBindingList.AllowRemove = true;
		}

		// IWorksheetRequirements
		public IEnumerable<string> RequiredWorksheetNames()
		{
			return new[] { "Invoices", "Addendums" };
		}

		public override string Title
		{
			get
			{
				return string.IsNullOrEmpty(_project.ProjectName)
					? "Project Report"
					: _project.ProjectName + " - Report";
			}
		}

		public override object Model
		{
			get { return _project; }
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
			// Sync the binding list back to the model's Invoices collection
			_project.Invoices = _invoiceBindingList.ToList();
		}

		public override void ApplySpreadsheetChanges(IWorkbook workbook)
		{
			if (workbook == null)
				return;

			// Use name-safe lookup: some user-saved layouts may have different sheet names
			// so avoid using the indexer which throws when the name is missing.
			var invoicesSheet = workbook.Worksheets.FirstOrDefault(ws => string.Equals(ws.Name, "Invoices", StringComparison.OrdinalIgnoreCase));
			if (invoicesSheet != null)
			{
				var invoices = ReadInvoices(invoicesSheet);
				_project.Invoices = invoices;
				_invoiceBindingList = new BindingList<InvoiceModel>(invoices);
				_invoiceBindingList.AllowNew = true;
				_invoiceBindingList.AllowEdit = true;
				_invoiceBindingList.AllowRemove = true;
			}

			var addendumsSheet = workbook.Worksheets.FirstOrDefault(ws => string.Equals(ws.Name, "Addendums", StringComparison.OrdinalIgnoreCase));
			if (addendumsSheet != null)
				_project.Addendums = ReadAddendums(addendumsSheet);
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
				new KeyValuePair<string, string>("Reference", _project.Reference ?? "ظ¤"),
				new KeyValuePair<string, string>("Status", _project.Status.ToString()),
				new KeyValuePair<string, string>("Client", _project.ClientName ?? "ظ¤"),
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
			var contractDetails = _project.ContractDetails;
			yield return new FieldDescriptor("Project.Reference", "Project Reference", "Project Info", typeof(string), null, () => _project.Reference);
			yield return new FieldDescriptor("Project.TentativeReference", "Tentative Reference", "Project Info", typeof(string), null, () => _project.TentativeReference);
			yield return new FieldDescriptor("Project.ProjectName", "Project Name", "Project Info", typeof(string), null, () => _project.ProjectName);
			yield return new FieldDescriptor("Project.Contract", "Contract", "Project Info", typeof(string), null, () => _project.Contract);
			yield return new FieldDescriptor("Project.JointVenture", "Joint Venture", "Project Info", typeof(string), null, () => _project.JointVenture);
			yield return new FieldDescriptor("Project.ClientName", "Client", "Project Info", typeof(string), null, () => _project.ClientName);
			yield return new FieldDescriptor("Project.ClientContact", "Client Contact", "Project Info", typeof(string), null, () => _project.ClientContact);
			yield return new FieldDescriptor("Project.FundedBy", "Funded By", "Project Info", typeof(string), null, () => _project.FundedBy);
			yield return new FieldDescriptor("Project.EngineerInCharge", "Engineer in Charge", "Project Info", typeof(string), null, () => _project.EngineerInCharge);
			yield return new FieldDescriptor("Project.Username", "Username", "Project Info", typeof(string), null, () => _project.Username);
			yield return new FieldDescriptor("Project.Status", "Status", "Project Info", typeof(string), null, () => _project.Status.ToString());
			yield return new FieldDescriptor("Project.Active", "Active", "Project Info", typeof(bool), null, () => _project.Active);
			yield return new FieldDescriptor("Project.Area", "Area", "Project Info", typeof(string), null, () => _project.Location?.Area);
			yield return new FieldDescriptor("Project.Bank", "Bank", "Project Info", typeof(string), null, () => GetProjectBank());
			yield return new FieldDescriptor("Project.ContractLink", "Contract Link", "Project Info", typeof(string), null, () => contractDetails?.ContractFileLink);
			yield return new FieldDescriptor("Project.SourceFile", "Source File", "Project Info", typeof(string), null, () => _project.SourceFile);
		}

		private IEnumerable<FieldDescriptor> GetDateFields()
		{
			yield return new FieldDescriptor("Project.YearOfIssuance", "Year of Issuance", "Dates", typeof(int?), null, () => _project.YearOfIssuance);
			yield return new FieldDescriptor("Project.IssuanceDate", "Issuance Date", "Dates", typeof(DateTime?), "d", () => _project.ProjectDate);
			yield return new FieldDescriptor("Project.ProjectDate", "Project Date", "Dates", typeof(DateTime?), "d", () => _project.ProjectDate);
			yield return new FieldDescriptor("Project.SignatureDate", "Signature Date", "Dates", typeof(DateTime?), "d", () => _project.ContractDetails?.SignatureDate);
			yield return new FieldDescriptor("Project.ExpiryDate", "Expiry Date", "Dates", typeof(DateTime?), "d", () => _project.ExpiryDate);
		}

		private IEnumerable<FieldDescriptor> GetLocationFields()
		{
			yield return new FieldDescriptor("Project.Location.RawLocation", "Location", "Location", typeof(string), null, () => ResolveLocationText(_project.Location));
			yield return new FieldDescriptor("Project.Location.Country", "Country", "Location", typeof(string), null, () => _project.Location?.Country);
			yield return new FieldDescriptor("Project.Location.City", "City", "Location", typeof(string), null, () => _project.Location?.City);
		}

		private IEnumerable<FieldDescriptor> GetServiceFields()
		{
			var contractDetails = _project.ContractDetails;
			yield return new FieldDescriptor("Contract.Reference", "Contract Reference", "Services / Contract", typeof(string), null, () => contractDetails?.ContractNumber ?? _project.Contract);
			yield return new FieldDescriptor("Contract.InitialAmount", "Initial Contract Amount", "Services / Contract", typeof(decimal?), "N2", () => contractDetails?.InitialContractAmount);
			yield return new FieldDescriptor("Contract.Currency", "Currency", "Services / Contract", typeof(string), null, () => contractDetails?.CurrencyCode);
			yield return new FieldDescriptor("Contract.Retention", "Retention", "Services / Contract", typeof(decimal?), "N2", () => contractDetails?.RetentionPercentage);
			yield return new FieldDescriptor("Contract.VAT", "VAT", "Services / Contract", typeof(decimal?), "N2", () => contractDetails?.InitialVatAmount);
			yield return new FieldDescriptor("Contract.TTC", "TTC", "Services / Contract", typeof(decimal?), "N2", () => contractDetails?.InitialTtcAmount);
			yield return new FieldDescriptor("Project.ServicesProvided", "Services Provided", "Services / Contract", typeof(string), null, () => string.Join(", ", contractDetails?.ServicesProvided ?? new List<string>()));
			yield return new FieldDescriptor("Project.ServiceTypes", "Service Types", "Services / Contract", typeof(string), null, () => string.Join(", ", contractDetails?.ServiceTypes ?? new List<string>()));
		}

		private IEnumerable<FieldDescriptor> GetContractDetailFields()
		{
			var cd = _project.ContractDetails;
			yield return new FieldDescriptor("ContractDetail.ContractNumber", "Contract Number", "Contract Details", typeof(string), null, () => cd?.ContractNumber);
			yield return new FieldDescriptor("ContractDetail.ContractFileLink", "Contract File Link", "Contract Details", typeof(string), null, () => cd?.ContractFileLink);
			yield return new FieldDescriptor("ContractDetail.SignatureDate", "Contract Signature Date", "Contract Details", typeof(DateTime?), "d", () => cd?.SignatureDate);
			yield return new FieldDescriptor("ContractDetail.ContractPeriod", "Contract Period", "Contract Details", typeof(string), null, () => cd?.ContractPeriod);
			yield return new FieldDescriptor("ContractDetail.ExtensionDetails", "Extension Details", "Contract Details", typeof(string), null, () => cd?.ExtensionDetails);
			yield return new FieldDescriptor("ContractDetail.ActualCompletionDate", "Actual Completion Date", "Contract Details", typeof(DateTime?), "d", () => cd?.ActualCompletionDate);
			yield return new FieldDescriptor("ContractDetail.ClientContactEmail", "Client Contact Email", "Contract Details", typeof(string), null, () => cd?.ClientContactEmail);
			yield return new FieldDescriptor("ContractDetail.CurrencyCode", "Currency Code", "Contract Details", typeof(string), null, () => cd?.CurrencyCode);
			yield return new FieldDescriptor("ContractDetail.DesignFee", "Design Fee", "Contract Details", typeof(decimal?), "N2", () => cd?.DesignFee);
			yield return new FieldDescriptor("ContractDetail.SupervisionFee", "Supervision Fee", "Contract Details", typeof(decimal?), "N2", () => cd?.SupervisionFee);
			yield return new FieldDescriptor("ContractDetail.InitialContractAmount", "Initial Amount (Detail)", "Contract Details", typeof(decimal?), "N2", () => cd?.InitialContractAmount);
			yield return new FieldDescriptor("ContractDetail.RetentionPercentage", "Retention %", "Contract Details", typeof(decimal?), "N2", () => cd?.RetentionPercentage);
			yield return new FieldDescriptor("ContractDetail.InitialVatAmount", "Initial VAT Amount", "Contract Details", typeof(decimal?), "N2", () => cd?.InitialVatAmount);
			yield return new FieldDescriptor("ContractDetail.InitialTtcAmount", "Initial TTC Amount", "Contract Details", typeof(decimal?), "N2", () => cd?.InitialTtcAmount);
		}

		private IEnumerable<FieldDescriptor> GetWarrantyAndDocumentFields()
		{
			var cd = _project.ContractDetails;
			yield return new FieldDescriptor("Warranty.Reference", "Warranty Reference", "Warranty", typeof(string), null, () => cd?.WarrantyReference);
			yield return new FieldDescriptor("Warranty.Amount", "Warranty Amount", "Warranty", typeof(decimal?), "N2", () => cd?.WarrantyAmount);
			yield return new FieldDescriptor("Warranty.Bank", "Warranty Bank", "Warranty", typeof(string), null, () => cd?.WarrantyBank);
			yield return new FieldDescriptor("Warranty.IssuanceDate", "Warranty Issuance Date", "Warranty", typeof(DateTime?), "d", () => cd?.WarrantyIssuanceDate);
			yield return new FieldDescriptor("Warranty.ExpiryDate", "Warranty Expiry Date", "Warranty", typeof(DateTime?), "d", () => cd?.WarrantyExpiryDate);
			yield return new FieldDescriptor("Warranty.Status", "Warranty Status", "Warranty", typeof(string), null, () => cd?.WarrantyStatus);

			var docs = _project.Documents;
			yield return new FieldDescriptor("Documents.PreliminaryHandoverLink", "Preliminary Handover Link", "Documents", typeof(string), null, () => docs?.PreliminaryHandoverLink);
			yield return new FieldDescriptor("Documents.PreliminaryHandoverDate", "Preliminary Handover Date", "Documents", typeof(DateTime?), "d", () => docs?.PreliminaryHandoverDate);
			yield return new FieldDescriptor("Documents.FinalHandoverLink", "Final Handover Link", "Documents", typeof(string), null, () => docs?.FinalHandoverLink);
			yield return new FieldDescriptor("Documents.FinalHandoverDate", "Final Handover Date", "Documents", typeof(DateTime?), "d", () => docs?.FinalHandoverDate);
			yield return new FieldDescriptor("Documents.CompletionCertificateLink", "Completion Certificate Link", "Documents", typeof(string), null, () => docs?.CompletionCertificateLink);
			yield return new FieldDescriptor("Documents.CompletionCertificateDate", "Completion Certificate Date", "Documents", typeof(DateTime?), "d", () => docs?.CompletionCertificateDate);
		}

		private IEnumerable<FieldDescriptor> GetFinancialSummaryFields(List<InvoiceModel> invoices, List<AddendumModel> addendums)
		{
			yield return new FieldDescriptor("Summary.AddendumsTotal", "Addendums Total", "Financial Summary", typeof(decimal), "N2", () => addendums.Sum(x => x.Amount ?? 0m));
			yield return new FieldDescriptor("Summary.InvoicedTotal", "Invoiced Total", "Financial Summary", typeof(decimal), "N2", () => invoices.Sum(x => x.Amount ?? 0m));
			yield return new FieldDescriptor("Summary.PaidTotal", "Paid Total", "Financial Summary", typeof(decimal), "N2", () => invoices.Sum(x => x.PaidAmount ?? 0m));
			yield return new FieldDescriptor("Summary.ContractTotal", "Contract Total", "Financial Summary", typeof(decimal), "N2", () => (_project.ContractDetails?.InitialContractAmount ?? 0m) + addendums.Sum(x => x.Amount ?? 0m));
			yield return new FieldDescriptor("Summary.Balance", "Balance", "Financial Summary", typeof(decimal), "N2", () => (_project.ContractDetails?.InitialContractAmount ?? 0m) + addendums.Sum(x => x.Amount ?? 0m) - invoices.Sum(x => x.Amount ?? 0m));
			yield return new FieldDescriptor("Summary.InvoicesCount", "Invoices Count", "Financial Summary", typeof(int), null, () => invoices.Count);
			yield return new FieldDescriptor("Summary.AddendumsCount", "Addendums Count", "Financial Summary", typeof(int), null, () => addendums.Count);
		}

		private IEnumerable<FieldDescriptor> GetInvoiceFields(List<InvoiceModel> invoices, int invoiceCount)
		{
			yield return new FieldDescriptor("Invoice.InvoiceNumber", "Invoice #", "Invoice", typeof(string), null, null, i => i < invoiceCount ? (object)invoices[i].InvoiceNumber : null, invoiceCount);
			yield return new FieldDescriptor("Invoice.InvoiceDate", "Invoice Date", "Invoice", typeof(DateTime?), "d", null, i => i < invoiceCount ? (object)invoices[i].InvoiceDate : null, invoiceCount);
			yield return new FieldDescriptor("Invoice.Amount", "Amount", "Invoice", typeof(decimal?), "N2", null, i => i < invoiceCount ? (object)invoices[i].Amount : null, invoiceCount);
			yield return new FieldDescriptor("Invoice.VAT", "Invoice VAT", "Invoice", typeof(decimal?), "N2", null, i => i < invoiceCount ? (object)invoices[i].VAT : null, invoiceCount);
			yield return new FieldDescriptor("Invoice.Paid", "Paid?", "Invoice", typeof(bool), null, null, i => i < invoiceCount ? (object)invoices[i].Paid : null, invoiceCount);
			yield return new FieldDescriptor("Invoice.PaidAmount", "Paid Amount", "Invoice", typeof(decimal?), "N2", null, i => i < invoiceCount ? (object)invoices[i].PaidAmount : null, invoiceCount);
			yield return new FieldDescriptor("Invoice.Bank", "Invoice Bank", "Invoice", typeof(string), null, null, i => i < invoiceCount ? (object)invoices[i].Bank : null, invoiceCount);
			yield return new FieldDescriptor("Invoice.ProjectName", "Project (Invoice)", "Invoice", typeof(string), null, null, i => i < invoiceCount ? (object)invoices[i].ProjectName : null, invoiceCount);
		}

		private IEnumerable<FieldDescriptor> GetAddendumFields(List<AddendumModel> addendums, int addendumCount)
		{
			yield return new FieldDescriptor("Addendum.Sequence", "Addendum #", "Addendum", typeof(int), null, null, i => i < addendumCount ? (object)addendums[i].Sequence : null, addendumCount);
			yield return new FieldDescriptor("Addendum.Title", "Addendum Title", "Addendum", typeof(string), null, null, i => i < addendumCount ? (object)addendums[i].Title : null, addendumCount);
			yield return new FieldDescriptor("Addendum.Reference", "Addendum Reference", "Addendum", typeof(string), null, null, i => i < addendumCount ? (object)addendums[i].Reference : null, addendumCount);
			yield return new FieldDescriptor("Addendum.DecisionNo", "Decision No", "Addendum", typeof(string), null, null, i => i < addendumCount ? (object)addendums[i].DecisionNo : null, addendumCount);
			yield return new FieldDescriptor("Addendum.BODDate", "BOD Date", "Addendum", typeof(DateTime?), "d", null, i => i < addendumCount ? (object)addendums[i].BODDate : null, addendumCount);
			yield return new FieldDescriptor("Addendum.EffectiveDate", "Effective Date", "Addendum", typeof(DateTime?), "d", null, i => i < addendumCount ? (object)addendums[i].EffectiveDate : null, addendumCount);
			yield return new FieldDescriptor("Addendum.Amount", "Addendum Amount", "Addendum", typeof(decimal?), "N2", null, i => i < addendumCount ? (object)addendums[i].Amount : null, addendumCount);
			yield return new FieldDescriptor("Addendum.VAT", "Addendum VAT", "Addendum", typeof(decimal?), "N2", null, i => i < addendumCount ? (object)addendums[i].VAT : null, addendumCount);
			yield return new FieldDescriptor("Addendum.Retention", "Addendum Retention", "Addendum", typeof(decimal?), "N2", null, i => i < addendumCount ? (object)addendums[i].Retention : null, addendumCount);
			yield return new FieldDescriptor("Addendum.TTC", "Addendum TTC", "Addendum", typeof(decimal?), "N2", null, i => i < addendumCount ? (object)addendums[i].TTC : null, addendumCount);
		}

		private string ResolveLocationText(object loc)
		{
			if (loc == null) return null;
			var t = loc.GetType();
			var prop = t.GetProperty("RawLocation");
			if (prop != null)
			{
				var v = prop.GetValue(loc) as string;
				if (!string.IsNullOrWhiteSpace(v)) return v;
			}

			prop = t.GetProperty("LocationName");
			if (prop != null)
			{
				var v = prop.GetValue(loc) as string;
				if (!string.IsNullOrWhiteSpace(v)) return v;
			}

			prop = t.GetProperty("Name");
			if (prop != null)
			{
				var v = prop.GetValue(loc) as string;
				if (!string.IsNullOrWhiteSpace(v)) return v;
			}

			prop = t.GetProperty("City");
			string city = prop != null ? prop.GetValue(loc) as string : null;
			prop = t.GetProperty("Country");
			string country = prop != null ? prop.GetValue(loc) as string : null;
			var parts = new List<string>();
			if (!string.IsNullOrWhiteSpace(city)) parts.Add(city);
			if (!string.IsNullOrWhiteSpace(country)) parts.Add(country);
			if (parts.Count > 0) return string.Join(", ", parts);

			var toString = loc.ToString();
			return string.IsNullOrWhiteSpace(toString) ? null : toString;
		}

		private string GetProjectBank()
		{
			var invoiceBank = _invoiceBindingList
				.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.Bank))
				?.Bank;

			return !string.IsNullOrWhiteSpace(invoiceBank)
				? invoiceBank
				: _project.ContractDetails?.WarrantyBank;
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
            row = WriteKeyValue(worksheet, row, "Funded By", _project.FundedBy, labelColor);
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
            row = WriteDetailRow(worksheet, row, "Contract Number", cd?.ContractNumber);
            row = WriteDetailRow(worksheet, row, "Contract File Link", cd?.ContractFileLink);
            row = WriteDetailRow(worksheet, row, "Signature Date", cd?.SignatureDate);
            row = WriteDetailRow(worksheet, row, "Contract Period", cd?.ContractPeriod);
            row = WriteDetailRow(worksheet, row, "Extension Details", cd?.ExtensionDetails);
            row = WriteDetailRow(worksheet, row, "Actual Completion Date", cd?.ActualCompletionDate);
            row = WriteDetailRow(worksheet, row, "Client Contact Email", cd?.ClientContactEmail);
            row = WriteDetailRow(worksheet, row, "Currency Code", cd?.CurrencyCode);
            row = WriteDetailRow(worksheet, row, "Design Fee", cd?.DesignFee, "#,##0.00");
            row = WriteDetailRow(worksheet, row, "Supervision Fee", cd?.SupervisionFee, "#,##0.00");
            row = WriteDetailRow(worksheet, row, "Initial Contract Amount", cd?.InitialContractAmount, "#,##0.00");
            row = WriteDetailRow(worksheet, row, "Retention %", cd?.RetentionPercentage, "#,##0.00");
            row = WriteDetailRow(worksheet, row, "Initial VAT Amount", cd?.InitialVatAmount, "#,##0.00");
            row = WriteDetailRow(worksheet, row, "Initial TTC Amount", cd?.InitialTtcAmount, "#,##0.00");

            // Addendum details from ContractDetailModel
            row++;
            worksheet.Cells[row, 0].Value = "Addendum Details (Fixed)";
            worksheet.Cells[row, 0].Font.Bold = true;
            var addendumHeaderRange = worksheet.Range.FromLTRB(0, row, 1, row);
            addendumHeaderRange.Fill.BackgroundColor = headerColor;
            worksheet.Cells[row, 0].Font.Color = System.Drawing.Color.White;
            row++;
            row = WriteDetailRow(worksheet, row, "Addendum 1 Ref", cd?.Addendum1Ref);
            row = WriteDetailRow(worksheet, row, "Addendum 1 Amount", cd?.Addendum1Amount, "#,##0.00");
            row = WriteDetailRow(worksheet, row, "Addendum 1 VAT", cd?.Addendum1Vat, "#,##0.00");
            row = WriteDetailRow(worksheet, row, "Addendum 1 TTC", cd?.Addendum1Ttc, "#,##0.00");
            row = WriteDetailRow(worksheet, row, "Addendum 1 Board Date", cd?.Addendum1BoardDate);
            row = WriteDetailRow(worksheet, row, "Addendum 2 Amount", cd?.Addendum2Amount, "#,##0.00");
            row = WriteDetailRow(worksheet, row, "Addendum 2 VAT", cd?.Addendum2Vat, "#,##0.00");
            row = WriteDetailRow(worksheet, row, "Addendum 2 TTC", cd?.Addendum2Ttc, "#,##0.00");

            worksheet.Columns.AutoFit(0, 1);
            worksheet.Columns[0].Width = 220;
            worksheet.Columns[1].Width = 280;
        }

        private int WriteDetailRow(Worksheet ws, int row, string label, object value, string numberFormat = null)
        {
            return ReportSpreadsheetHelper.WriteDetailRow(ws, row, label, value, numberFormat);
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
                worksheet.Cells[targetRow, 1].Value = addendum.Title ?? string.Empty;
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

            row = WriteDetailRow(worksheet, row, "Warranty Reference", cd?.WarrantyReference);
            row = WriteDetailRow(worksheet, row, "Warranty Amount", cd?.WarrantyAmount, "#,##0.00");
            row = WriteDetailRow(worksheet, row, "Warranty Bank", cd?.WarrantyBank);
            row = WriteDetailRow(worksheet, row, "Issuance Date", cd?.WarrantyIssuanceDate);
            row = WriteDetailRow(worksheet, row, "Expiry Date", cd?.WarrantyExpiryDate);
            row = WriteDetailRow(worksheet, row, "Warranty Status", cd?.WarrantyStatus);
            row++;

            // Documents section
            worksheet.Cells[row, 0].Value = "Documents";
            worksheet.Cells[row, 0].Font.Bold = true;
            worksheet.Cells[row, 0].Font.Color = System.Drawing.Color.White;
            var docsRange = worksheet.Range.FromLTRB(0, row, 1, row);
            docsRange.Merge();
            docsRange.Fill.BackgroundColor = headerColor;
            row++;

            row = WriteDetailRow(worksheet, row, "Preliminary Handover Link", docs?.PreliminaryHandoverLink);
            row = WriteDetailRow(worksheet, row, "Preliminary Handover Date", docs?.PreliminaryHandoverDate);
            row = WriteDetailRow(worksheet, row, "Final Handover Link", docs?.FinalHandoverLink);
            row = WriteDetailRow(worksheet, row, "Final Handover Date", docs?.FinalHandoverDate);
            row = WriteDetailRow(worksheet, row, "Completion Certificate Link", docs?.CompletionCertificateLink);
            row = WriteDetailRow(worksheet, row, "Completion Certificate Date", docs?.CompletionCertificateDate);
            row++;

            // Contract Link
            row = WriteDetailRow(worksheet, row, "Contract Link", cd?.ContractFileLink);

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
                if (string.Equals(GetCellText(worksheet, row, 1), "Totals", StringComparison.OrdinalIgnoreCase))
                    continue;

                var invoice = new InvoiceModel
                {
                    InvoiceNumber = GetCellText(worksheet, row, 0),
                    InvoiceDate = GetCellDate(worksheet, row, 1),
                    Amount = GetCellDecimal(worksheet, row, 2),
                    VAT = GetCellDecimal(worksheet, row, 3),
                    Paid = GetCellBoolean(worksheet, row, 4),
                    PaidAmount = GetCellDecimal(worksheet, row, 5),
                    Bank = GetCellText(worksheet, row, 6),
                    ProjectName = string.IsNullOrWhiteSpace(GetCellText(worksheet, row, 7)) ? _project.ProjectName : GetCellText(worksheet, row, 7)
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
                if (string.Equals(GetCellText(worksheet, row, 5), "Totals", StringComparison.OrdinalIgnoreCase))
                    continue;

                var addendum = new AddendumModel
                {
                    Sequence = GetCellInt(worksheet, row, 0),
                    Title = GetCellText(worksheet, row, 1),
                    Reference = GetCellText(worksheet, row, 2),
                    DecisionNo = GetCellText(worksheet, row, 3),
                    BODDate = GetCellDate(worksheet, row, 4),
                    EffectiveDate = GetCellDate(worksheet, row, 5),
                    Amount = GetCellDecimal(worksheet, row, 6),
                    VAT = GetCellDecimal(worksheet, row, 7),
                    Retention = GetCellDecimal(worksheet, row, 8),
                    TTC = GetCellDecimal(worksheet, row, 9)
                };

                if (addendum.Sequence == 0
                    && string.IsNullOrWhiteSpace(addendum.Title)
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

        private static string GetCellText(Worksheet worksheet, int row, int column)
        {
            return ReportSpreadsheetHelper.GetCellText(worksheet, row, column);
        }

        private static DateTime? GetCellDate(Worksheet worksheet, int row, int column)
        {
            return ReportSpreadsheetHelper.GetCellDate(worksheet, row, column);
        }

        private static decimal? GetCellDecimal(Worksheet worksheet, int row, int column)
        {
            return ReportSpreadsheetHelper.GetCellDecimal(worksheet, row, column);
        }

        private static int GetCellInt(Worksheet worksheet, int row, int column)
        {
            return ReportSpreadsheetHelper.GetCellInt(worksheet, row, column);
        }

        private static bool GetCellBoolean(Worksheet worksheet, int row, int column)
        {
            return ReportSpreadsheetHelper.GetCellBoolean(worksheet, row, column);
        }
    }
}

