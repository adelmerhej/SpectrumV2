using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace SpectrumV1.Models.Operations.Projects
{
	public class ProjectPaymentModel : EntityObject, ICloneable
	{
		[BsonRepresentation(BsonType.ObjectId)]
		public string PaymentId { get; set; } = ObjectId.GenerateNewId().ToString();

		[BsonRepresentation(BsonType.Decimal128)]
		public decimal Amount { get; set; }

		public DateTime PaymentDate { get; set; }

		public PaymentMethod PaymentMethod { get; set; }

		public string ReferenceNumber { get; set; }

		#region Implementation of ICloneable

		/// <summary>Creates a new object that is a copy of the current instance.</summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public object Clone()
		{
			var recordModel = (ProjectPaymentModel)MemberwiseClone();
			return recordModel;
		}

		#endregion
	}
}
