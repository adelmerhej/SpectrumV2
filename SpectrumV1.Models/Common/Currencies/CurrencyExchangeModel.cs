using MongoDB.Bson.Serialization.Attributes;
using System;

namespace SpectrumV1.Models.Common.Currencies
{
	public class CurrencyExchangeModel : EntityObject, ICloneable
	{
		[BsonElement("Currency")]
		public string Currency { get; set; }
		public DateTime ExchangeDate { get; set; }
		public decimal Rate { get; set; }
		public decimal HighValue { get; set; }


		#region Implementation of ICloneable

		/// <summary>Creates a new object that is a copy of the current instance.</summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public object Clone()
		{
			var recordModel = (CurrencyExchangeModel)MemberwiseClone();
			return recordModel;
		}

		#endregion
	}
}
