using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Spectrum.Models.Members.Clients
{
	public class ContactModel : EntityObject, ICloneable
	{
		[BsonElement("ClientId")]
		[BsonRepresentation(BsonType.ObjectId)]
		public string ClientId { get; set; }

        [BsonElement("ClientName")]
        public string ClientName { get; set; }

		[BsonElement("ContactName")]
		public string ContactName { get; set; }

		public string Title { get; set; }
		public string JobPosition { get; set; }
		public string Email { get; set; }
		public string PhoneNumber1 { get; set; }
		public string PhoneNumber2 { get; set; }
		public string MobileNumber { get; set; }
		public bool IsPrimary { get; set; }

		#region Implementation of ICloneable

		/// <summary>Creates a new object that is a copy of the current instance.</summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public object Clone()
		{
			var recordModel = (ContactModel)MemberwiseClone();
			return recordModel;
		}

		#endregion
	}
}
