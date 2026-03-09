using System;

namespace Spectrum.Models.Accounting.Charts
{
	public class ChartModel : EntityObject, ICloneable
	{
		public int ParentId { get; set; }
		public string Number { get; set; }
		public string Serial { get; set; }
		public string AccountName { get; set; }
		public string AccountType { get; set; }
		public double LAmountOpening { get; set; }
		public double LAmountDebit { get; set; }
		public double LAmountCredit { get; set; }
		public double FAmountOpening { get; set; }
		public double FAmountDebit { get; set; }
		public double FAmountCredit { get; set; }


		public string AccountNumber
		{
			get { return $"{Number} {Serial}"; }
		}


		#region Implementation of ICloneable

		/// <summary>Creates a new object that is a copy of the current instance.</summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public object Clone()
		{
			var newModel = (ChartModel)MemberwiseClone();
			return newModel;
		}

		#endregion
	}
}
