using MongoDB.Bson.Serialization.Attributes;
using Spectrum.Models;
using System;

namespace SpectrumV1.Models.Common.Roles
{
    public class RoleModel : EntityObject, ICloneable
    {
        [BsonElement("RoleName")]
        public string RoleName { get; set; }

        #region Implementation of ICloneable

        /// <summary>Creates a new object that is a copy of the current instance.</summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone()
        {
            var recordModel = (RoleModel)MemberwiseClone();
            return recordModel;
        }

        #endregion
    }
}
