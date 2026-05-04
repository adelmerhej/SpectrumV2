using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Spectrum.Models.Projects
{
	public class LocationInfoModel : EntityObject, ICloneable
	{
      [BsonElement("RawLocation")]
		public string RawLocation { get; set; }           // original "Location" field

		[BsonElement("Area")]
		public string Area { get; set; }                  // "Area"

		[BsonElement("Country")]
		public string Country { get; set; }               // "Country"

		[BsonElement("City")]
		public string City { get; set; }                  // parsed from "Location" if available

		#region Implementation of ICloneable

		/// <summary>Creates a new object that is a copy of the current instance.</summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public object Clone()
		{
			var recordModel = (LocationInfoModel)MemberwiseClone();
			return recordModel;
		}

		#endregion
	}
}
