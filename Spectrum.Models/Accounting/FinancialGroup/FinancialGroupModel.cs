using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Spectrum.Models.Accounting.FinancialGroup
{
    public class FinancialGroupModel : EntityObject, ICloneable
    {
        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("Purchase")]
        public string Purchase { get; set; }

        [BsonElement("PurchaseDiscount")]
        public string PurchaseDiscount { get; set; }

        [BsonElement("Sales")]
        public string Sales { get; set; }

        [BsonElement("SalesDiscount")]
        public string SalesDiscount { get; set; }   

        [BsonElement("VATPurchase")]
        public string VATPurchase { get; set; }

        [BsonElement("VATSales")]
        public string VATSales { get; set; }

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
