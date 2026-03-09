using MongoDB.Bson.Serialization.Attributes;
using Spectrum.Models;
using System;

namespace SpectrumV1.Models.Accounting.Banks
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
            var recordModel = (BankModel)MemberwiseClone();
            return recordModel;
        }

        #endregion
    }
}
