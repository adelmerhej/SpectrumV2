using MongoDB.Bson.Serialization.Attributes;
using SpectrumV1.Models.Common.Companies;
using System;

namespace SpectrumV1.Models.Common.Countries
{
	public class RegionModel : EntityObject, ICloneable
	{
		[BsonElement("RegionName")]
		public string RegionName { get; set; }
		public string Continent { get; set; }

		#region Implementation of ICloneable

		/// <summary>Creates a new object that is a copy of the current instance.</summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public object Clone()
		{
			var recordModel = (BranchModel)MemberwiseClone();
			return recordModel;
		}

		#endregion
	}
}
