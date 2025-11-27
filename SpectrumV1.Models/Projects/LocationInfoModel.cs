using System;

namespace SpectrumV1.Models.Projects
{
	public class LocationInfoModel : EntityObject, ICloneable
	{
		public string RawLocation { get; set; }           // original "Location" field
		public string Country { get; set; }               // "Country"
		public string City { get; set; }                  // parsed from "Location" if available

		#region Implementation of ICloneable

		/// <summary>Creates a new object that is a copy of the current instance.</summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public object Clone()
		{
			var recordModel = (LocationInfoModel)MemberwiseClone();
			return recordModel;
		}

		#endregion
	}
}
