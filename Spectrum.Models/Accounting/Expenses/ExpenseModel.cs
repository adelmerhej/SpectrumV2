using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Spectrum.Models.Accounting.Invoices;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Spectrum.Models.Accounting.Expenses
{
    public class ExpenseModel : EntityObject, ICloneable
    {
        public string ExpenseNo { get; set; }
        public string Reference { get; set; }
        public string ProjectId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string MemberName { get; set; }
        public string Subject { get; set; }
        public string Currency { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Rate { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal? TotalAmount { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal? TotalLAmount { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal? TotalFAmount { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal? TotalVat { get; set; }

        public bool Posted { get; set; }

        public bool Paid { get; set; }
        public bool Approved { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal? PaidAmount { get; set; }
        public string Bank { get; set; }

        [BsonElement("DocumentLink")]
        public string DocumentLink { get; set; }

        public List<InvoiceDetailModel> InvoiceDetails { get; set; } = new List<InvoiceDetailModel>();
        public List<InvoiceActivityEntryModel> ActivityTimeline { get; set; } = new List<InvoiceActivityEntryModel>();

        #region Implementation of ICloneable

        /// <summary>Creates a new object that is a copy of the current instance.</summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone()
        {
            var invoiceModel = (InvoiceModel)MemberwiseClone();
            invoiceModel.InvoiceDetails = InvoiceDetails?
                .Select(detail => (detail.Clone() as InvoiceDetailModel) ?? new InvoiceDetailModel())
                .ToList() ?? new List<InvoiceDetailModel>();
            invoiceModel.ActivityTimeline = ActivityTimeline?
                .Select(entry => (InvoiceActivityEntryModel)entry.Clone())
                .ToList() ?? new List<InvoiceActivityEntryModel>();
            return invoiceModel;
        }

        #endregion
    }

    public class InvoiceActivityEntryModel : ICloneable
    {
        public string Title { get; set; }
        public string Description { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime Timestamp { get; set; }

        public string PerformedBy { get; set; }
        public int AccentColorArgb { get; set; }
        public int Sequence { get; set; }
        public bool DateOnly { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
