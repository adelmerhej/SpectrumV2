using MongoDB.Bson.Serialization.Attributes;
using Spectrum.Models;
using System;

namespace Spectrum.Models.HumanResources.BloodTypes
{
    public class BloodTypeModel : EntityObject, ICloneable
    {
        [BsonElement("BloodTypeName")]
        public string BloodTypeName { get; set; }

        #region Implementation of ICloneable

        /// <summary>Creates a new object that is a copy of the current instance.</summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone()
        {
            var recordModel = (BloodTypeModel)MemberwiseClone();
            return recordModel;
        }

        #endregion
    }
}
