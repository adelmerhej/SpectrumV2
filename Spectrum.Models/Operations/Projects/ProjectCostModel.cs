using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Spectrum.Models.Operations.Projects
{
	public class ProjectCostModel : EntityObject, ICloneable
	{
		[BsonRepresentation(BsonType.ObjectId)]
		public string CostId { get; set; } = ObjectId.GenerateNewId().ToString();

		public CostType CostType { get; set; }

		public string Description { get; set; }

		[BsonRepresentation(BsonType.Decimal128)]
		public decimal Amount { get; set; }

		public DateTime CostDate { get; set; }

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
