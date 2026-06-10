using System;

namespace Spectrum.Views.Transactions.Invoices.Classes
{
    internal sealed class InvoiceHeaderSnapshot
    {
        public DateTime InvoiceDate { get; set; }
        public string Currency { get; set; }
        public string Subject { get; set; }
        public string MemberName { get; set; }
        public string ProjectReference { get; set; }
        public decimal Rate { get; set; }
    }
}
