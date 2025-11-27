using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace SpectrumV1.Models.Projects
{
	public class WarrantyInfoModel : EntityObject, ICloneable
	{
		public string WarrantyRef { get; set; }           // "Warranty Ref"


		[BsonRepresentation(BsonType.Decimal128)]
		public decimal? WarrantyAmount { get; set; }      // "Warranty Amount"


		#region Implementation of ICloneable

		/// <summary>Creates a new object that is a copy of the current instance.</summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public object Clone()
		{
			var recordModel = (WarrantyInfoModel)MemberwiseClone();
			return recordModel;
		}

		#endregion
	}
}
