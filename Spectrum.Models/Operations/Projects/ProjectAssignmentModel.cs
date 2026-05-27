using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Spectrum.Models.Operations.Projects
{
	public class ProjectAssignmentModel : EntityObject, ICloneable
	{
		[BsonElement("EngineerId")]
		[BsonRepresentation(BsonType.ObjectId)]
		public string EngineerId { get; set; }
		public string EngineerName { get; set; }
		public DateTime AssignmentDate { get; set; }
		public DateTime? UnassignmentDate { get; set; }

		[BsonRepresentation(BsonType.Decimal128)]
		public decimal HourlyRateAtAssignment { get; set; }

		[BsonRepresentation(BsonType.Decimal128)]
		public decimal EstimatedHours { get; set; }

		[BsonIgnore]
		public bool IsCurrent => UnassignmentDate == null;

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
