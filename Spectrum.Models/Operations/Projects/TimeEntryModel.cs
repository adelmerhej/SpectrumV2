using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Spectrum.Models.Operations.Projects
{
	public class TimeEntryModel : EntityObject, ICloneable
	{
		[BsonRepresentation(BsonType.ObjectId)]
		public string TimeEntryId { get; set; } = ObjectId.GenerateNewId().ToString();

		[BsonRepresentation(BsonType.ObjectId)]
		public string EngineerId { get; set; }

		public string EngineerName { get; set; }

		public DateTime WorkDate { get; set; }

		[BsonRepresentation(BsonType.Decimal128)]
		public decimal HoursWorked { get; set; }

		public string Description { get; set; }

		[BsonRepresentation(BsonType.ObjectId)]
		public string AssignmentId { get; set; }

		public BillingStatus BillingStatus { get; set; } = BillingStatus.Unbilled;

		#region Implementation of ICloneable

		/// <summary>Creates a new object that is a copy of the current instance.</summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public object Clone()
		{
			var recordModel = (TimeEntryModel)MemberwiseClone();
			return recordModel;
		}

		#endregion
	}
}
