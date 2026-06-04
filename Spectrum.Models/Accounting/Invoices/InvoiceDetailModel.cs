using System;

namespace Spectrum.Models.Accounting.Invoices
{
    public class InvoiceDetailModel : EntityObject, ICloneable
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
        public decimal VATRate { get; set; }
        public decimal VATValue { get; set; }

        #region Implementation of ICloneable

        /// <summary>Creates a new object that is a copy of the current instance.</summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion
    }
}
