using MongoDB.Bson.Serialization.Attributes;
using SpectrumV1.Models.Common.Companies;
using System;

namespace SpectrumV1.Models.Common.Areas
{
	public class AreaModel : EntityObject, ICloneable
	{
		[BsonElement("AreaName")]
		public string AreaName { get; set; }
		public string AreaCode { get; set; }


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
