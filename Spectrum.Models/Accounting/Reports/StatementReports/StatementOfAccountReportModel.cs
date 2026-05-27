using System;

namespace Spectrum.Models.Accounting.Reports.StatementReports
{
    public class StatementOfAccountReportModel
    {
        public int JvNo { get; set; }
        public string JournalType { get; set; }
        public DateTime JournalDate { get; set; }
        public string Reference { get; set; }
        public string Currency { get; set; }
        public decimal Rate { get; set; }
        public bool IsPosted { get; set; }
        public string Notes { get; set; }
        public int WorkingYear { get; set; }

        //details
        public int Line { get; set; }
        public string Number { get; set; }
        public string Serial { get; set; }
        public string AccountName { get; set; }
        public string CostCenter { get; set; }
        public DateTime? ValueDate { get; set; }
        public string CurrencyDetail { get; set; }
        public decimal RateDetail { get; set; }
        public string Description { get; set; }
        public string DbCr { get; set; }
        public decimal Amount { get; set; }
        public decimal LAmount { get; set; }
        public decimal FAmount { get; set; }
        public string DocumentRef { get; set; }

        //Additional
        public string JobNo { get; set; }
        public string Department { get; set; }
        public string AccountNumber
        {
            get { return $"{Number} {Serial}"; }
        }
    }
}
