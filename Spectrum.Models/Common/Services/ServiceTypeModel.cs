using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Spectrum.Models.Common.Services
{
	public class ServiceTypeModel : EntityObject, ICloneable
	{
		[BsonElement("ServiceType")]
		public string ServiceType { get; set; }
		public string ServiceCode { get; set; }

		#region Implementation of ICloneable

		/// <summary>Creates a new object that is a copy of the current instance.</summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public object Clone()
		{
			var recordModel = (ServiceTypeModel)MemberwiseClone();
			return recordModel;
		}

		#endregion
	}
}
