using DevExpress.Spreadsheet;
using DevExpress.XtraSpreadsheet;
using Spectrum.Models.HumanResources.Employees;
using Spectrum.Reports.Common;
using Spectrum.Reports.Exporters;
using Spectrum.Reports.Interfaces;
using Spectrum.Models.HumanResources.EmployeeTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace Spectrum.Reports.Adapters.HumanResources.Employees
{
    public class EmployeeReportAdapter : ReportAdapterBase, IWorksheetRequirements
    {
        private readonly EmployeeModel _employee;
        private BindingList<EducationInfo> _educationBindingList;

        public EmployeeReportAdapter(EmployeeModel employee)
        {
            _employee = employee ?? throw new ArgumentNullException(nameof(employee));
            _educationBindingList = CreateEducationBindingList(_employee.Education);
        }

        public IEnumerable<string> RequiredWorksheetNames()
        {
            return new[] { "Employee Summary", "Education" };
        }

        public override string Title
        {
            get
            {
                return string.IsNullOrWhiteSpace(_employee.FullName)
                    ? "Employee Report"
                    : _employee.FullName + " - Employee Report";
            }
        }

        public override object Model
        {
            get { return _employee; }
        }

        public override IList<GridColumnDescriptor> GetGridColumns()
        {
            return new List<GridColumnDescriptor>
            {
                new GridColumnDescriptor("Degree", "Degree", false, 140),
                new GridColumnDescriptor("Specialization", "Specialization", false, 160),
                new GridColumnDescriptor("SchoolOrUniversity", "School / University", false, 180),
                new GridColumnDescriptor("Place", "Place", false, 140),
                new GridColumnDescriptor("GraduationYear", "Graduation Year", false, 110)
            };
        }

        public override IBindingList GetGridDataSource()
        {
            return _educationBindingList;
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

        public override IList<QuickActionDescriptor> GetQuickActions()
        {
            return new List<QuickActionDescriptor>
            {
                new QuickActionDescriptor(
                    "AddEducation",
                    "Add Education",
                    () => _educationBindingList.Add(new EducationInfo())),
                new QuickActionDescriptor(
                    "RemoveEducation",
                    "Remove Education",
                    () =>
                    {
                        if (_educationBindingList.Count > 0)
                            _educationBindingList.RemoveAt(_educationBindingList.Count - 1);
                    },
                    () => _educationBindingList.Count > 0)
            };
        }

        public override void Recalculate()
        {
            _employee.Education = _educationBindingList.ToList();
        }

        public override void ApplySpreadsheetChanges(IWorkbook workbook)
        {
            if (workbook == null)
                return;

            var summarySheet = workbook.Worksheets.FirstOrDefault(ws => string.Equals(ws.Name, "Employee Summary", StringComparison.OrdinalIgnoreCase));
            if (summarySheet != null)
                ReadEmployeeSummary(summarySheet);

            var educationSheet = workbook.Worksheets.FirstOrDefault(ws => string.Equals(ws.Name, "Education", StringComparison.OrdinalIgnoreCase));
            if (educationSheet != null)
            {
                var education = ReadEducation(educationSheet);
                _employee.Education = education;
                _educationBindingList = CreateEducationBindingList(education);
            }
        }

        public override IList<KeyValuePair<string, string>> GetSummaryFields()
        {
            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Employee #", _employee.EmployeeNo.ToString()),
                new KeyValuePair<string, string>("Name", _employee.FullName ?? string.Empty),
                new KeyValuePair<string, string>("Employee Type", GetEmployeeTypeName()),
                new KeyValuePair<string, string>("Position", _employee.ActualPosition ?? string.Empty),
                new KeyValuePair<string, string>("Specialization", _employee.Specialization ?? string.Empty),
                new KeyValuePair<string, string>("Hired Date", FormatDate(_employee.HiredDate)),
                new KeyValuePair<string, string>("Working Date", FormatDate(_employee.WorkingDate)),
                new KeyValuePair<string, string>("Years Experience", _employee.YearsOfExperience.ToString()),
                new KeyValuePair<string, string>("Left Work", _employee.LeftWork ? "Yes" : "No")
            };
        }

        public override IList<FieldDescriptor> GetFieldDescriptors()
        {
            var education = _employee.Education ?? new List<EducationInfo>();
            var count = education.Count;
            var fields = new List<FieldDescriptor>();

            fields.AddRange(GetIdentityFields());
            fields.AddRange(GetContactFields());
            fields.AddRange(GetAdministrativeFields());
            fields.AddRange(GetWorkFields());
            fields.AddRange(GetEducationFields(education, count));

            return fields;
        }

        public override string ExportToCsvString()
        {
            var headers = new[]
            {
                "Employee #", "First Name", "Last Name", "Employee Type", "Position",
                "Specialization", "Hired Date", "Working Date", "Left Work",
                "Email", "Mobile", "Degree", "Education Specialization",
                "School / University", "Place", "Graduation Year"
            };

            var rows = new List<string[]>();
            var education = _educationBindingList.Count == 0
                ? new List<EducationInfo> { new EducationInfo() }
                : _educationBindingList.ToList();

            foreach (var item in education)
            {
                rows.Add(new[]
                {
                    _employee.EmployeeNo.ToString(),
                    _employee.FirstName ?? string.Empty,
                    _employee.LastName ?? string.Empty,
                    GetEmployeeTypeName(),
                    _employee.ActualPosition ?? string.Empty,
                    _employee.Specialization ?? string.Empty,
                    FormatDate(_employee.HiredDate),
                    FormatDate(_employee.WorkingDate),
                    _employee.LeftWork ? "Yes" : "No",
                    _employee.ContactInfo?.Email ?? string.Empty,
                    _employee.ContactInfo?.LocalMobileNo ?? string.Empty,
                    item.Degree ?? string.Empty,
                    item.Specialization ?? string.Empty,
                    item.SchoolOrUniversity ?? string.Empty,
                    item.Place ?? string.Empty,
                    item.GraduationYear?.ToString() ?? string.Empty
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

        private void BuildWorkbook(IWorkbook workbook)
        {
            if (workbook == null)
                throw new ArgumentNullException(nameof(workbook));

            workbook.Unit = DevExpress.Office.DocumentUnit.Inch;

            var summary = workbook.Worksheets[0];
            summary.Name = "Employee Summary";
            BuildSummaryWorksheet(summary);

            var education = workbook.Worksheets.Add("Education");
            BuildEducationWorksheet(education);

            education.FreezeRows(1);
            ReportSpreadsheetHelper.ApplyDefaultPrintSettings(workbook, summary);
        }

        private void BuildSummaryWorksheet(Worksheet worksheet)
        {
            var headerColor = System.Drawing.Color.FromArgb(70, 130, 180);
            worksheet.Range["A1:D1"].Merge();
            worksheet["A1"].Value = Title;
            worksheet["A1"].Font.Bold = true;
            worksheet["A1"].Font.Size = 16;
            worksheet["A1"].Font.Color = System.Drawing.Color.White;
            worksheet["A1"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
            worksheet["A1"].Fill.BackgroundColor = headerColor;

            int row = 2;
            row = WriteSectionHeader(worksheet, row, "Identity");
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Employee #", _employee.EmployeeNo);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "First Name", _employee.FirstName);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Last Name", _employee.LastName);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Father Name", _employee.FatherName);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Mother Full Name", _employee.MotherFullName);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Date Of Birth", _employee.DateOfBirth);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Place Of Birth", _employee.PlaceOfBirth);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Nationality", _employee.Nationality);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "ID / Passport", _employee.IdCardOrPassportNo);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Family Status", _employee.FamilyStatus);
            row++;

            row = WriteSectionHeader(worksheet, row, "Employment");
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Employee Type", GetEmployeeTypeName());
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Position", _employee.ActualPosition);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Specialization", _employee.Specialization);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Hired Date", _employee.HiredDate);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Working Date", _employee.WorkingDate);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Years Of Experience", _employee.YearsOfExperience);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Left Work", _employee.LeftWork ? "Yes" : "No");
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Document Link", _employee.DocumentLink);
            row++;

            row = WriteSectionHeader(worksheet, row, "Contact");
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Email", _employee.ContactInfo?.Email);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Local Mobile", _employee.ContactInfo?.LocalMobileNo);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Abroad Mobile", _employee.ContactInfo?.AbroadMobileNo);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Local Fix Phone", _employee.ContactInfo?.LocalFixPhone);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Emergency Contact", _employee.EmergencyContact?.ContactName);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Emergency Mobile", _employee.EmergencyContact?.MobileNo);
            row++;

            row = WriteSectionHeader(worksheet, row, "Registration");
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "CNSS Registration No", _employee.Cnss?.RegistrationNo);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "CNSS Registration Date", _employee.Cnss?.RegistrationDate);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "CNSS Children", _employee.Cnss?.NoOfChildren ?? 0);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "CNSS Beneficiaries", _employee.Cnss?.NoOfBeneficiary ?? 0);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Spouse Registration", _employee.Cnss != null && _employee.Cnss.SpouseRegistration ? "Yes" : "No");
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "CNSS Leave Date", _employee.Cnss?.CnssLeaveDate);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Syndicat No", _employee.Syndicat?.SyndicatNo);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Syndicat Registration Date", _employee.Syndicat?.RegistrationDate);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Syndicat Registration Place", _employee.Syndicat?.RegistrationPlace);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Working Permit No", _employee.WorkingPermit?.RegistrationNo);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Working Permit Date", _employee.WorkingPermit?.PermitDate);
            row = ReportSpreadsheetHelper.WriteDetailRow(worksheet, row, "Financial No", _employee.Financial?.FinancialNo);

            worksheet.Range["B:B"].NumberFormat = "yyyy-mm-dd";
            worksheet.Columns.AutoFit(0, 1);
            worksheet.Columns[0].Width = 230;
            worksheet.Columns[1].Width = 320;
        }

        private void BuildEducationWorksheet(Worksheet worksheet)
        {
            var headerColor = System.Drawing.Color.FromArgb(70, 130, 180);
            worksheet.Range["A1:E1"].Merge();
            worksheet["A1"].Value = "Education - " + (_employee.FullName ?? "Employee");
            worksheet["A1"].Font.Bold = true;
            worksheet["A1"].Font.Size = 14;
            worksheet["A1"].Font.Color = System.Drawing.Color.White;
            worksheet["A1"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
            worksheet["A1"].Fill.BackgroundColor = headerColor;

            var headers = new[] { "Degree", "Specialization", "School / University", "Place", "Graduation Year" };
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[1, i].Value = headers[i];
                worksheet.Cells[1, i].Font.Bold = true;
                worksheet.Cells[1, i].Fill.BackgroundColor = System.Drawing.Color.Gainsboro;
            }

            for (int row = 0; row < _educationBindingList.Count; row++)
            {
                var item = _educationBindingList[row];
                var targetRow = row + 2;
                worksheet.Cells[targetRow, 0].Value = item.Degree ?? string.Empty;
                worksheet.Cells[targetRow, 1].Value = item.Specialization ?? string.Empty;
                worksheet.Cells[targetRow, 2].Value = item.SchoolOrUniversity ?? string.Empty;
                worksheet.Cells[targetRow, 3].Value = item.Place ?? string.Empty;
                worksheet.Cells[targetRow, 4].Value = item.GraduationYear;
            }

            worksheet.Columns.AutoFit(0, 4);
        }

        private int WriteSectionHeader(Worksheet worksheet, int row, string caption)
        {
            var headerColor = System.Drawing.Color.FromArgb(70, 130, 180);
            var range = worksheet.Range.FromLTRB(0, row, 1, row);
            range.Merge();
            range.Fill.BackgroundColor = headerColor;
            worksheet.Cells[row, 0].Value = caption;
            worksheet.Cells[row, 0].Font.Bold = true;
            worksheet.Cells[row, 0].Font.Color = System.Drawing.Color.White;
            return row + 1;
        }

        private void ReadEmployeeSummary(Worksheet worksheet)
        {
            var values = ReadDetailValues(worksheet);

            _employee.EmployeeNo = GetInt(values, "Employee #");
            _employee.FirstName = GetString(values, "First Name");
            _employee.LastName = GetString(values, "Last Name");
            _employee.FatherName = GetString(values, "Father Name");
            _employee.MotherFullName = GetString(values, "Mother Full Name");
            _employee.DateOfBirth = GetDate(values, "Date Of Birth");
            _employee.PlaceOfBirth = GetString(values, "Place Of Birth");
            _employee.Nationality = GetString(values, "Nationality");
            _employee.IdCardOrPassportNo = GetString(values, "ID / Passport");
            _employee.FamilyStatus = GetString(values, "Family Status");
            _employee.EmployeeType = CreateEmployeeType(GetString(values, "Employee Type"));
            _employee.ActualPosition = GetString(values, "Position");
            _employee.Specialization = GetString(values, "Specialization");
            _employee.HiredDate = GetDate(values, "Hired Date") ?? _employee.HiredDate;
            _employee.WorkingDate = GetDate(values, "Working Date");
            _employee.YearsOfExperience = GetInt(values, "Years Of Experience");
            _employee.LeftWork = GetBoolean(values, "Left Work");
            _employee.DocumentLink = GetString(values, "Document Link");

            EnsureNestedInfo();
            _employee.ContactInfo.Email = GetString(values, "Email");
            _employee.ContactInfo.LocalMobileNo = GetString(values, "Local Mobile");
            _employee.ContactInfo.AbroadMobileNo = GetString(values, "Abroad Mobile");
            _employee.ContactInfo.LocalFixPhone = GetString(values, "Local Fix Phone");
            _employee.EmergencyContact.ContactName = GetString(values, "Emergency Contact");
            _employee.EmergencyContact.MobileNo = GetString(values, "Emergency Mobile");
            _employee.Cnss.RegistrationNo = GetString(values, "CNSS Registration No");
            _employee.Cnss.RegistrationDate = GetDate(values, "CNSS Registration Date");
            _employee.Cnss.NoOfChildren = GetInt(values, "CNSS Children");
            _employee.Cnss.NoOfBeneficiary = GetInt(values, "CNSS Beneficiaries");
            _employee.Cnss.SpouseRegistration = GetBoolean(values, "Spouse Registration");
            _employee.Cnss.CnssLeaveDate = GetDate(values, "CNSS Leave Date");
            _employee.Syndicat.SyndicatNo = GetString(values, "Syndicat No");
            _employee.Syndicat.RegistrationDate = GetDate(values, "Syndicat Registration Date");
            _employee.Syndicat.RegistrationPlace = GetString(values, "Syndicat Registration Place");
            _employee.WorkingPermit.RegistrationNo = GetString(values, "Working Permit No");
            _employee.WorkingPermit.PermitDate = GetDate(values, "Working Permit Date");
            _employee.Financial.FinancialNo = GetString(values, "Financial No");
        }

        private List<EducationInfo> ReadEducation(Worksheet worksheet)
        {
            var education = new List<EducationInfo>();
            var usedRange = worksheet.GetUsedRange();
            if (usedRange == null)
                return education;

            for (int row = 2; row <= usedRange.BottomRowIndex; row++)
            {
                var item = new EducationInfo
                {
                    Degree = ReportSpreadsheetHelper.GetCellText(worksheet, row, 0),
                    Specialization = ReportSpreadsheetHelper.GetCellText(worksheet, row, 1),
                    SchoolOrUniversity = ReportSpreadsheetHelper.GetCellText(worksheet, row, 2),
                    Place = ReportSpreadsheetHelper.GetCellText(worksheet, row, 3),
                    GraduationYear = GetNullableInt(worksheet, row, 4)
                };

                if (string.IsNullOrWhiteSpace(item.Degree)
                    && string.IsNullOrWhiteSpace(item.Specialization)
                    && string.IsNullOrWhiteSpace(item.SchoolOrUniversity)
                    && string.IsNullOrWhiteSpace(item.Place)
                    && !item.GraduationYear.HasValue)
                {
                    continue;
                }

                education.Add(item);
            }

            return education;
        }

        private Dictionary<string, string> ReadDetailValues(Worksheet worksheet)
        {
            var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var usedRange = worksheet.GetUsedRange();
            if (usedRange == null)
                return values;

            for (int row = 0; row <= usedRange.BottomRowIndex; row++)
            {
                var key = ReportSpreadsheetHelper.GetCellText(worksheet, row, 0);
                var value = ReportSpreadsheetHelper.GetCellText(worksheet, row, 1);
                if (!string.IsNullOrWhiteSpace(key) && !values.ContainsKey(key))
                    values.Add(key, value);
            }

            return values;
        }

        private IEnumerable<FieldDescriptor> GetIdentityFields()
        {
            yield return new FieldDescriptor("Employee.EmployeeNo", "Employee #", "Identity", typeof(int), null, () => _employee.EmployeeNo);
            yield return new FieldDescriptor("Employee.FirstName", "First Name", "Identity", typeof(string), null, () => _employee.FirstName);
            yield return new FieldDescriptor("Employee.LastName", "Last Name", "Identity", typeof(string), null, () => _employee.LastName);
            yield return new FieldDescriptor("Employee.FullName", "Full Name", "Identity", typeof(string), null, () => _employee.FullName);
            yield return new FieldDescriptor("Employee.FatherName", "Father Name", "Identity", typeof(string), null, () => _employee.FatherName);
            yield return new FieldDescriptor("Employee.MotherFullName", "Mother Full Name", "Identity", typeof(string), null, () => _employee.MotherFullName);
            yield return new FieldDescriptor("Employee.BloodType", "Blood Type", "Identity", typeof(string), null, () => _employee.BloodType);
            yield return new FieldDescriptor("Employee.DateOfBirth", "Date Of Birth", "Identity", typeof(DateTime?), "d", () => _employee.DateOfBirth);
            yield return new FieldDescriptor("Employee.PlaceOfBirth", "Place Of Birth", "Identity", typeof(string), null, () => _employee.PlaceOfBirth);
            yield return new FieldDescriptor("Employee.Nationality", "Nationality", "Identity", typeof(string), null, () => _employee.Nationality);
            yield return new FieldDescriptor("Employee.IdCardOrPassportNo", "ID / Passport", "Identity", typeof(string), null, () => _employee.IdCardOrPassportNo);
            yield return new FieldDescriptor("Employee.FamilyStatus", "Family Status", "Identity", typeof(string), null, () => _employee.FamilyStatus);
        }

        private IEnumerable<FieldDescriptor> GetContactFields()
        {
            yield return new FieldDescriptor("Employee.Contact.Email", "Email", "Contact", typeof(string), null, () => _employee.ContactInfo?.Email);
            yield return new FieldDescriptor("Employee.Contact.LocalMobileNo", "Local Mobile", "Contact", typeof(string), null, () => _employee.ContactInfo?.LocalMobileNo);
            yield return new FieldDescriptor("Employee.Contact.AbroadMobileNo", "Abroad Mobile", "Contact", typeof(string), null, () => _employee.ContactInfo?.AbroadMobileNo);
            yield return new FieldDescriptor("Employee.Contact.LocalFixPhone", "Local Fix Phone", "Contact", typeof(string), null, () => _employee.ContactInfo?.LocalFixPhone);
            yield return new FieldDescriptor("Employee.Emergency.ContactName", "Emergency Contact", "Contact", typeof(string), null, () => _employee.EmergencyContact?.ContactName);
            yield return new FieldDescriptor("Employee.Emergency.MobileNo", "Emergency Mobile", "Contact", typeof(string), null, () => _employee.EmergencyContact?.MobileNo);
            yield return new FieldDescriptor("Employee.Address", "Address", "Contact", typeof(string), null, () => _employee.Address);
        }

        private IEnumerable<FieldDescriptor> GetAdministrativeFields()
        {
            yield return new FieldDescriptor("Employee.Cnss.RegistrationNo", "CNSS Registration No", "Registration", typeof(string), null, () => _employee.Cnss?.RegistrationNo);
            yield return new FieldDescriptor("Employee.Cnss.RegistrationDate", "CNSS Registration Date", "Registration", typeof(DateTime?), "d", () => _employee.Cnss?.RegistrationDate);
            yield return new FieldDescriptor("Employee.Cnss.NoOfChildren", "CNSS Children", "Registration", typeof(int), null, () => _employee.Cnss?.NoOfChildren ?? 0);
            yield return new FieldDescriptor("Employee.Cnss.NoOfBeneficiary", "CNSS Beneficiaries", "Registration", typeof(int), null, () => _employee.Cnss?.NoOfBeneficiary ?? 0);
            yield return new FieldDescriptor("Employee.Cnss.SpouseRegistration", "Spouse Registration", "Registration", typeof(bool), null, () => _employee.Cnss != null && _employee.Cnss.SpouseRegistration);
            yield return new FieldDescriptor("Employee.Cnss.CnssLeaveDate", "CNSS Leave Date", "Registration", typeof(DateTime?), "d", () => _employee.Cnss?.CnssLeaveDate);
            yield return new FieldDescriptor("Employee.Syndicat.SyndicatNo", "Syndicat No", "Registration", typeof(string), null, () => _employee.Syndicat?.SyndicatNo);
            yield return new FieldDescriptor("Employee.Syndicat.RegistrationDate", "Syndicat Registration Date", "Registration", typeof(DateTime?), "d", () => _employee.Syndicat?.RegistrationDate);
            yield return new FieldDescriptor("Employee.Syndicat.RegistrationPlace", "Syndicat Registration Place", "Registration", typeof(string), null, () => _employee.Syndicat?.RegistrationPlace);
            yield return new FieldDescriptor("Employee.WorkingPermit.RegistrationNo", "Working Permit No", "Registration", typeof(string), null, () => _employee.WorkingPermit?.RegistrationNo);
            yield return new FieldDescriptor("Employee.WorkingPermit.PermitDate", "Working Permit Date", "Registration", typeof(DateTime?), "d", () => _employee.WorkingPermit?.PermitDate);
            yield return new FieldDescriptor("Employee.Financial.FinancialNo", "Financial No", "Registration", typeof(string), null, () => _employee.Financial?.FinancialNo);
        }

        private IEnumerable<FieldDescriptor> GetWorkFields()
        {
            yield return new FieldDescriptor("Employee.EmployeeType", "Employee Type", "Employment", typeof(string), null, () => GetEmployeeTypeName());
            yield return new FieldDescriptor("Employee.ActualPosition", "Position", "Employment", typeof(string), null, () => _employee.ActualPosition);
            yield return new FieldDescriptor("Employee.Specialization", "Specialization", "Employment", typeof(string), null, () => _employee.Specialization);
            yield return new FieldDescriptor("Employee.HiredDate", "Hired Date", "Employment", typeof(DateTime), "d", () => _employee.HiredDate);
            yield return new FieldDescriptor("Employee.WorkingDate", "Working Date", "Employment", typeof(DateTime?), "d", () => _employee.WorkingDate);
            yield return new FieldDescriptor("Employee.YearsOfExperience", "Years Of Experience", "Employment", typeof(int), null, () => _employee.YearsOfExperience);
            yield return new FieldDescriptor("Employee.LeftWork", "Left Work", "Employment", typeof(bool), null, () => _employee.LeftWork);
            yield return new FieldDescriptor("Employee.DocumentLink", "Document Link", "Employment", typeof(string), null, () => _employee.DocumentLink);
            yield return new FieldDescriptor("Employee.WorkExperience.WorkingPosition", "Working Position", "Employment", typeof(string), null, () => _employee.WorkExperience?.WorkingPosition);
            yield return new FieldDescriptor("Employee.WorkExperience.WorkLocation", "Work Location", "Employment", typeof(string), null, () => _employee.WorkExperience?.WorkLocation);
            yield return new FieldDescriptor("Employee.WorkExperience.TotalYearsOfExperience", "Total Years Experience", "Employment", typeof(int), null, () => _employee.WorkExperience?.TotalYearsOfExperience ?? 0);
            yield return new FieldDescriptor("Employee.WorkExperience.YearsAtSpectrum", "Years At Spectrum", "Employment", typeof(int), null, () => _employee.WorkExperience?.YearsAtSpectrum ?? 0);
        }

        private IEnumerable<FieldDescriptor> GetEducationFields(List<EducationInfo> education, int count)
        {
            yield return new FieldDescriptor("Employee.Education.Degree", "Degree", "Education", typeof(string), null, null, i => i < count ? education[i].Degree : null, count);
            yield return new FieldDescriptor("Employee.Education.Specialization", "Education Specialization", "Education", typeof(string), null, null, i => i < count ? education[i].Specialization : null, count);
            yield return new FieldDescriptor("Employee.Education.SchoolOrUniversity", "School / University", "Education", typeof(string), null, null, i => i < count ? education[i].SchoolOrUniversity : null, count);
            yield return new FieldDescriptor("Employee.Education.Place", "Education Place", "Education", typeof(string), null, null, i => i < count ? education[i].Place : null, count);
            yield return new FieldDescriptor("Employee.Education.GraduationYear", "Graduation Year", "Education", typeof(int?), null, null, i => i < count ? education[i].GraduationYear : null, count);
        }

        private void EnsureNestedInfo()
        {
            if (_employee.ContactInfo == null)
                _employee.ContactInfo = new EmployeeContactInfo();
            if (_employee.EmergencyContact == null)
                _employee.EmergencyContact = new EmergencyContactInfo();
            if (_employee.Cnss == null)
                _employee.Cnss = new CnssInfo();
            if (_employee.Syndicat == null)
                _employee.Syndicat = new SyndicatInfo();
            if (_employee.WorkingPermit == null)
                _employee.WorkingPermit = new WorkingPermitInfo();
            if (_employee.Financial == null)
                _employee.Financial = new FinancialInfo();
        }

        private static BindingList<EducationInfo> CreateEducationBindingList(List<EducationInfo> education)
        {
            var bindingList = new BindingList<EducationInfo>(education ?? new List<EducationInfo>());
            bindingList.AllowNew = true;
            bindingList.AllowEdit = true;
            bindingList.AllowRemove = true;
            return bindingList;
        }

        private string GetEmployeeTypeName()
        {
            return _employee.EmployeeType?.TypeName ?? string.Empty;
        }

        private static EmployeeTypeModel CreateEmployeeType(string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName))
                return null;

            return new EmployeeTypeModel { TypeName = typeName.Trim() };
        }

        private static string FormatDate(DateTime? value)
        {
            return value.HasValue ? value.Value.ToString("yyyy-MM-dd") : string.Empty;
        }

        private static string GetString(Dictionary<string, string> values, string key)
        {
            string value;
            return values.TryGetValue(key, out value) ? value : string.Empty;
        }

        private static DateTime? GetDate(Dictionary<string, string> values, string key)
        {
            DateTime value;
            return DateTime.TryParse(GetString(values, key), out value) ? value : (DateTime?)null;
        }

        private static int GetInt(Dictionary<string, string> values, string key)
        {
            int value;
            return int.TryParse(GetString(values, key), out value) ? value : 0;
        }

        private static bool GetBoolean(Dictionary<string, string> values, string key)
        {
            var value = GetString(values, key);
            return string.Equals(value, "yes", StringComparison.OrdinalIgnoreCase)
                || string.Equals(value, "true", StringComparison.OrdinalIgnoreCase)
                || string.Equals(value, "1", StringComparison.OrdinalIgnoreCase);
        }

        private static int? GetNullableInt(Worksheet worksheet, int row, int column)
        {
            var text = ReportSpreadsheetHelper.GetCellText(worksheet, row, column);
            if (string.IsNullOrWhiteSpace(text))
                return null;

            int value;
            return int.TryParse(text, out value) ? value : (int?)null;
        }
    }
}
