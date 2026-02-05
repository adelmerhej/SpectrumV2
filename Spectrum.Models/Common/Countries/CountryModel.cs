using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Spectrum.Models.Common.Countries
{
	public class CountryModel : EntityObject, ICloneable
	{
		[BsonElement("CountryName")]
		public string CountryName { get; set; }
		public string CountryCode { get; set; }
		public string Region { get; set; }
		public string Continent { get; set; }

		#region Implementation of ICloneable

		/// <summary>Creates a new object that is a copy of the current instance.</summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public object Clone()
		{
			var recordModel = (CountryModel)MemberwiseClone();
			return recordModel;
		}

		#endregion
	}
}
