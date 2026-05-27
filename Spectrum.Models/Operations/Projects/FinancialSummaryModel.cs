using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Spectrum.Models.Operations.Projects
{
	public class FinancialSummaryModel : EntityObject, ICloneable
	{
		[BsonRepresentation(BsonType.Decimal128)]
		public decimal TotalLaborCosts { get; set; }

		[BsonRepresentation(BsonType.Decimal128)]
		public decimal TotalMaterialCosts { get; set; }

		[BsonRepresentation(BsonType.Decimal128)]
		public decimal TotalCosts { get; set; }

		[BsonRepresentation(BsonType.Decimal128)]
		public decimal TotalPaymentsReceived { get; set; }

		[BsonRepresentation(BsonType.Decimal128)]
		public decimal EstimatedProfit { get; set; }

		[BsonRepresentation(BsonType.Decimal128)]
		public decimal BalanceDue { get; set; }

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
