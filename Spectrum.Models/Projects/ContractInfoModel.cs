using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Spectrum.Models.Projects
{
	public class ContractInfoModel : EntityObject, ICloneable
	{
		public string ContractReference { get; set; }     // optional

		[BsonRepresentation(BsonType.Decimal128)]
		public decimal? InitialContractAmount { get; set; } // "Initial Contract Amount"
		public string Currency { get; set; }              // if present

		[BsonRepresentation(BsonType.Decimal128)]
		public decimal? Retention { get; set; }           // "Retention"

		[BsonRepresentation(BsonType.Decimal128)]
		public decimal? VAT { get; set; }                 // "VAT"

		[BsonRepresentation(BsonType.Decimal128)]
		public decimal? TTC { get; set; }

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
