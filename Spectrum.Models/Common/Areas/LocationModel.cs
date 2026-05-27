using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Spectrum.Models.Common.Areas
{
	public class LocationModel : EntityObject, ICloneable
	{
       [BsonElement("LocationCode")]
		[BsonIgnoreIfNull]
		public string LocationCode { get; set; }

		[BsonElement("LocationName")]
		public string LocationName { get; set; }

		#region Implementation of ICloneable

		/// <summary>Creates a new object that is a copy of the current instance.</summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public object Clone()
		{
			var recordModel = (LocationModel)MemberwiseClone();
			return recordModel;
		}

		#endregion
	}
}
