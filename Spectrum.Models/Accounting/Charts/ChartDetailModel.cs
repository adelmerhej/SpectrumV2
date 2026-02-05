using System;

namespace Spectrum.Models.Accounting.Charts
{
	public class ChartDetailModel : EntityObject, ICloneable
	{
		public string AccountNumber { get; set; }
		public string AccountName { get; set; }
		public string ParentId { get; set; }
		public string CostCenterId { get; set; }
		public string CurrencyId { get; set; }
		public double AmountOpening { get; set; }
		public double AmountDebit { get; set; }
		public double AmountCredit { get; set; }
		public double LAmountOpening { get; set; }
		public double LAmountDebit { get; set; }
		public double LAmountCredit { get; set; }
		public double FAmountOpening { get; set; }
		public double FAmountDebit { get; set; }
		public double FAmountCredit { get; set; }

		#region Implementation of ICloneable

		/// <summary>Creates a new object that is a copy of the current instance.</summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public object Clone()
		{
			var newModel = (ChartDetailModel)MemberwiseClone();
			return newModel;
		}

		#endregion
	}
}
