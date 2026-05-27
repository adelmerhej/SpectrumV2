using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Spectrum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectrum.Models.Members.Suppliers
{
    public class SupplierContactModel : EntityObject, ICloneable
    {
        [BsonElement("SupplierId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string SupplierId { get; set; }

        [BsonElement("SupplierName")]
        public string SupplierName { get; set; }

        [BsonElement("ContactName")]
        public string ContactName { get; set; }

        public string Title { get; set; }
        public string JobPosition { get; set; }
        public string Email { get; set; }
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        public string MobileNumber { get; set; }
        public bool IsPrimary { get; set; }

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
