using MongoDB.Bson.Serialization.Attributes;
using System;

namespace SpectrumV1.Models.Common.Countries
{
	public class CityModel : EntityObject, ICloneable
	{
		[BsonElement("CityName")]
		public string CityName { get; set; }
		public string CityCode { get; set; }
		public string Country { get; set; }
		public string Province { get; set; }
		public string District { get; set; }
		public string Region { get; set; }
		public string Continent { get; set; }
		public int Population { get; set; }
		public bool IsCapital { get; set; }

		#region Implementation of ICloneable

		/// <summary>Creates a new object that is a copy of the current instance.</summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public object Clone()
		{
			var recordModel = (CityModel)MemberwiseClone();
			return recordModel;
		}

		#endregion
	}
}
