namespace Spectrum.Views.Transactions.Invoices.Classes
{
    internal sealed class InvoiceDetailSnapshot
    {
        public int LineNo { get; set; }
        public string ItemId { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public decimal LAmount { get; set; }
        public decimal FAmount { get; set; }
        public bool VAT { get; set; }
        public decimal VATAmount { get; set; }
        public decimal VATLAmount { get; set; }
        public decimal VATFAmount { get; set; }
        public string Notes { get; set; }
    }
}
