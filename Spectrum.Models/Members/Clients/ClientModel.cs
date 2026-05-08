using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Spectrum.Models.Members.Clients
{
	public class ClientModel : EntityObject, ICloneable
	{
		[BsonElement("ClientInitial")]
		public string ClientInitial { get; set; }

		[BsonElement("ClientName")]
		public string ClientName { get; set; }
			
		[BsonElement("Contacts")]
		public List<ContactModel> Contacts { get; set; } = new List<ContactModel>();
		

		public string Email { get; set; }
		public string PhoneNumberFirst1 { get; set; }
		public string PhoneNumberFirst2 { get; set; }
		public string PhoneNumberFirst3 { get; set; }
		public string FaxNumberFirst { get; set; }
		public string PhoneNumberSecond1 { get; set; }
		public string PhoneNumberSecond2 { get; set; }
		public string PhoneNumberSecond3 { get; set; }
		public string FaxNumberSecond { get; set; }
		public string Website { get; set; }
		public string PoBox1 { get; set; }
		public string PoBox2 { get; set; }
		public string MofNo { get; set; }
		public string Activity { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string Location1 { get; set; }
		public string Location2 { get; set; }
		public string Country1 { get; set; }
		public string Country2 { get; set; }
		public string City1 { get; set; }
		public string City2 { get; set; }

        #region Implementation of ICloneable

        /// <summary>Creates a new object that is a copy of the current instance.</summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion
    }
}
