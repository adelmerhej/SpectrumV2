using MongoDB.Bson.Serialization.Attributes;
using Spectrum.Models;
using Spectrum.Models.Common.Countries;
using System;

namespace SpectrumV1.Models.HumanResources.BloodTypes
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
            var recordModel = (ContinentModel)MemberwiseClone();
            return recordModel;
        }

        #endregion
    }
}
