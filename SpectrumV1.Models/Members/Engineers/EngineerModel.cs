using MongoDB.Bson.Serialization.Attributes;
using System;

namespace SpectrumV1.Models.HumanResources.Engineers
{
	public class EngineerModel : EntityObject, ICloneable
	{
		[BsonElement("EngineerName")]
		public string EngineerName { get; set; }
		public string Specialization { get; set; }
		public string Address { get; set; }
		public string MobileNumber { get; set; }
		public string PhoneNumber { get; set; }
		public string Email { get; set; }
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
