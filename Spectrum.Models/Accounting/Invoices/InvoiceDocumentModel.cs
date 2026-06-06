using System;

namespace Spectrum.Models.Accounting.Invoices
{
    public class InvoiceDocumentModel
    {
        public string DocumentName { get; set; }
        public DateTime DocumentDate { get; set; }
        public DateTime StreamedDate { get; set; }
        public byte[] DocumentContent { get; set; }
        public string OriginPath { get; set; }
    }
}
