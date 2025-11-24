using MongoDB.Bson.Serialization.Attributes;
using System;

namespace SpectrumV1.Models.Common.Countries
{
	public class DistrictModel : EntityObject, ICloneable
	{
		[BsonElement("DistrictName")]
		public string DistrictName { get; set; }
		public string Province { get; set; }
		public string Country { get; set; }
		public string Region { get; set; }
		public string Continent { get; set; }

		#region Implementation of ICloneable

		/// <summary>Creates a new object that is a copy of the current instance.</summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public object Clone()
		{
			var recordModel = (DistrictModel)MemberwiseClone();
			return recordModel;
		}

		#endregion
	}
}
