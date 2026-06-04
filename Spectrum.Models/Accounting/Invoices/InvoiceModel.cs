using System;
using System.Collections.Generic;
using System.Linq;

namespace Spectrum.Models.Accounting.Invoices
{
    public class InvoiceModel : EntityObject, ICloneable
    {
        public string InvoiceNo { get; set; }
        public string Reference { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string MemberName { get; set; }
        public string Subject { get; set; }
        public string Currency { get; set; }
        public decimal Rate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalLAmount { get; set; }
        public decimal TotalFAmount { get; set; }
        public bool Posted { get; set; }
        public List<InvoiceDetailModel> InvoiceDetails { get; set; } = new List<InvoiceDetailModel>();

        #region Implementation of ICloneable

        /// <summary>Creates a new object that is a copy of the current instance.</summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone()
        {
            var invoiceModel = (InvoiceModel)MemberwiseClone();
            invoiceModel.InvoiceDetails = InvoiceDetails?
                .Select(detail => (detail.Clone() as InvoiceDetailModel) ?? new InvoiceDetailModel())
                .ToList() ?? new List<InvoiceDetailModel>();
            return invoiceModel;
        }

        #endregion
    }
}
