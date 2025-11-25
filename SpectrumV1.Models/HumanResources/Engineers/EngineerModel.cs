using MongoDB.Bson.Serialization.Attributes;
using System;

namespace SpectrumV1.Models.HumanResources.Engineers
{
	public class EngineerModel : EntityObject, ICloneable
	{
		[BsonElement("Name")]
		public string Name { get; set; }

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
