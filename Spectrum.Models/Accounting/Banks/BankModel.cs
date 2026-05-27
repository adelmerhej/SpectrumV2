using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Spectrum.Models.Accounting.Banks
{
    public class BankModel : EntityObject, ICloneable
    {
        [BsonElement("BankName")]
        public string BankName { get; set; }

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
