using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Spectrum.Models.Members.Engineers
{
	public class EngineerModel : EntityObject, ICloneable
	{
		[BsonElement("EngineerName")]
		public string EngineerName { get; set; }
		public string Specialization { get; set; }
		public string Status { get; set; }
		public string Address { get; set; }
		public string Email { get; set; }
		public string PhoneNumber1 { get; set; }
		public string PhoneNumber2 { get; set; }
		public string PhoneNumber3 { get; set; }
		public string FaxNumber { get; set; }
		public string Website { get; set; }
		public string Activity { get; set; }
		public string Country { get; set; }
		public string City { get; set; }
		public DateTime HiredDate { get; set; }

		#region Implementation of ICloneable

		/// <summary>Creates a new object that is a copy of the current instance.</summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public object Clone()
		{
			var recordModel = (EngineerModel)MemberwiseClone();
			return recordModel;
		}

		#endregion
	}
}
