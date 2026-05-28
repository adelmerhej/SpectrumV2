using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Spectrum.Models.Common.Services
{
	public class ServiceModel : EntityObject, ICloneable
	{
		[BsonElement("ServiceName")]
		public string ServiceName { get; set; }

		#region Implementation of ICloneable

		/// <summary>Creates a new object that is a copy of the current instance.</summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public object Clone()
		{
			var recordModel = (ServiceModel)MemberwiseClone();
			return recordModel;
		}

		#endregion
	}
}
