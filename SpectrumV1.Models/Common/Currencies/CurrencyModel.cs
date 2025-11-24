using MongoDB.Bson.Serialization.Attributes;
using System;


namespace SpectrumV1.Models.Common.Currencies
{
	public class CurrencyModel : EntityObject, ICloneable
	{
		[BsonElement("CurrencyName")]
		public string CurrencyName { get; set; }
		public string CurrencyCode { get; set; }
		public string CurrencySymbol { get; set; }

		#region Implementation of ICloneable

		/// <summary>Creates a new object that is a copy of the current instance.</summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public object Clone()
		{
			var recordModel = (CurrencyModel)MemberwiseClone();
			return recordModel;
		}

		#endregion
	}
}
