using MongoDB.Bson.Serialization.Attributes;
using Spectrum.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Spectrum.Models.Accounting.Journals
{
    public class 
        JournalModel : EntityObject, ICloneable
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
        public List<JournalDetailModel> JournalDetails { get; set; } = new List<JournalDetailModel>();

        #region Implementation of ICloneable

        /// <summary>Creates a new object that is a copy of the current instance.</summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone()
        {
            var recordModel = (JournalModel)MemberwiseClone();
            recordModel.JournalDetails = JournalDetails?
                .Select(detail => (detail.Clone() as JournalDetailModel) ?? new JournalDetailModel())
                .ToList() ?? new List<JournalDetailModel>();
            return recordModel;
        }

        #endregion
    }
}
