using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Spectrum.Models.Members.Funders
{
    public class FunderModel : EntityObject, ICloneable
    {
        [BsonElement("Name")]
        public string Name { get; set; }      // Name of the Funder

        [BsonElement("Email")]
        public string Email { get; set; }     // Contact person at the organization

        [BsonElement("Type")]
        public string Type { get; set; }      // Funder type (e.g., Gov, Corporate)

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
