using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace SpectrumV1.Models.Projects
{
	public class AddendumModel : EntityObject, ICloneable
	{
		// An addendum is stored as a subdocument in a list, one record per addendum
		public int Sequence { get; set; }                 // 1,2,3... useful if CSV had Add 1, Add 2, etc.
		public string Title { get; set; }                 // optional descriptive title
		public DateTime? BODDate { get; set; }            // "Adde1 BOD Date", etc.

		[BsonRepresentation(BsonType.Decimal128)]
		public decimal? Amount { get; set; }              // "Add 1 Amount"

		[BsonRepresentation(BsonType.Decimal128)]
		public decimal? VAT { get; set; }                 // "VAT-1"

		[BsonRepresentation(BsonType.Decimal128)]
		public decimal? Retention { get; set; }           // "Retention .1"

		[BsonRepresentation(BsonType.Decimal128)]
		public decimal? TTC { get; set; }                 // "TTC-1"

		public string Reference { get; set; }
		public DateTime? EffectiveDate { get; set; }


		#region Implementation of ICloneable

		/// <summary>Creates a new object that is a copy of the current instance.</summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public object Clone()
		{
			var recordModel = (AddendumModel)MemberwiseClone();
			return recordModel;
		}

		#endregion
	}
}
