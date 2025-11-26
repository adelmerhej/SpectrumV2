using MongoDB.Bson.Serialization.Attributes;
using System;

namespace SpectrumV1.Models.Members.Clients
{
	public class ClientModel : EntityObject, ICloneable
	{
		[BsonElement("ClientName")]
		public string ClientName { get; set; }
		public string ContactPerson { get; set; }
		public string Address { get; set; }
		public string Email { get; set; }
		public string PhoneNumber1 { get; set; }
		public string PhoneNumber2 { get; set; }
		public string PhoneNumber3 { get; set; }
		public string FaxNumber { get; set; }
		public string Website { get; set; }
		public string PoBox { get; set; }
		public string MofNo { get; set; }
		public string Activity { get; set; }
		public string Country { get; set; }
		public string City { get; set; }

		#region Implementation of ICloneable

		/// <summary>Creates a new object that is a copy of the current instance.</summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public object Clone()
		{
			var recordModel = (ClientModel)MemberwiseClone();
			return recordModel;
		}

		#endregion
	}
}
