using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Spectrum.Models.Accounting.CashRegister
{
    public class CashRegisterModel : EntityObject, ICloneable
    {
        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("AccountNumber")]
        public string AccountNumber { get; set; }

        [BsonElement("AccountName")]
        public string AccountName { get; set; }

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
