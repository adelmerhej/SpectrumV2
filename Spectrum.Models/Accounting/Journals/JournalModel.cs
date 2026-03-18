using MongoDB.Bson.Serialization.Attributes;
using Spectrum.Models;
using System;

namespace SpectrumV1.Models.Accounting.Journals
{
    public class JournalModel : EntityObject, ICloneable
    {
        [BsonElement("JvNo")]
        public string JvNo { get; set; }
        public string JournalType { get; set; }
        public DateTime JournalDate { get; set; }
        public string Reference { get; set; }
        public string Currency { get; set; }
        public decimal Rate { get; set; }
        public bool IsPosted { get; set; }
        public bool IsCloned { get; set; }

        #region Implementation of ICloneable

        /// <summary>Creates a new object that is a copy of the current instance.</summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone()
        {
            var recordModel = (JournalModel)MemberwiseClone();
            return recordModel;
        }

        #endregion
    }
}
