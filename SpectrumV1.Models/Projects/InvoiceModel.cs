using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace SpectrumV1.Models.Projects
{
	public class InvoiceModel : EntityObject, ICloneable
	{
		public string ProjectName { get; set; }
		public string InvoiceNumber { get; set; }
		public DateTime? InvoiceDate { get; set; }

		[BsonRepresentation(BsonType.Decimal128)]
		public decimal? Amount { get; set; }

		[BsonRepresentation(BsonType.Decimal128)]
		public decimal? VAT { get; set; }
		public bool Paid { get; set; }

		[BsonRepresentation(BsonType.Decimal128)]
		public decimal? PaidAmount { get; set; }
		public string Bank { get; set; }


		#region Implementation of ICloneable

		/// <summary>Creates a new object that is a copy of the current instance.</summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public object Clone()
		{
			var recordModel = (InvoiceModel)MemberwiseClone();
			return recordModel;
		}

		#endregion
	}
}
