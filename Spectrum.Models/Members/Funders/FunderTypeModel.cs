using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Spectrum.Models.Members.Funders
{
    public class FunderTypeModel : EntityObject, ICloneable
    {
        [BsonElement("Type")]
        public string Type { get; set; }      // Type of the Funder

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
