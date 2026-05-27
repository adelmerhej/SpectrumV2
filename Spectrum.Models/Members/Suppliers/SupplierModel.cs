using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Spectrum.Models.Members.Suppliers
{
    public class SupplierModel : EntityObject, ICloneable
    {
        [BsonElement("SupplierInitial")]
        public string SupplierInitial { get; set; }

        [BsonElement("SupplierName")]
        public string SupplierName { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
