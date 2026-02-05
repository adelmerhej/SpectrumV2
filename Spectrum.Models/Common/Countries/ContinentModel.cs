using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Spectrum.Models.Common.Countries
{
	public class ContinentModel : EntityObject, ICloneable
	{
		[BsonElement("ContinentName")]
		public string ContinentName { get; set; }

		#region Implementation of ICloneable

		/// <summary>Creates a new object that is a copy of the current instance.</summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public object Clone()
		{
			var recordModel = (ContinentModel)MemberwiseClone();
			return recordModel;
		}

		#endregion
	}
}
