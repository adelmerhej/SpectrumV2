using MongoDB.Bson.Serialization.Attributes;
using Spectrum.Models;
using System;

namespace Spectrum.Models.Accounting.Journals
{
    public class JournalDetailModel : EntityObject, ICloneable
    {
        [BsonElement("JvNo")]
        public string JvNo { get; set; }
        public int Line { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string CostCenter { get; set; }
        public string FlowType { get; set; }
        public DateTime ValueDate { get; set; }
        public string Currency { get; set; }
        public decimal Rate { get; set; }
        public string Description { get; set; }
        public string DbCr { get; set; }
        public decimal Amount { get; set; }
        public decimal LAmount { get; set; }
        public decimal FAmount { get; set; }
        public bool Posted { get; set; }
        public string DocumentRef { get; set; }

        #region Implementation of ICloneable

        /// <summary>Creates a new object that is a copy of the current instance.</summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone()
        {
            var newModel = (JournalDetailModel)MemberwiseClone();
            return newModel;
        }

        #endregion
    }
}
