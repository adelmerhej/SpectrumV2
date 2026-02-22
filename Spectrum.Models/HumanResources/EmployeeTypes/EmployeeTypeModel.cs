using MongoDB.Bson.Serialization.Attributes;
using Spectrum.Models;
using System;

namespace SpectrumV1.Models.HumanResources.EmployeeTypes
{
    public class EmployeeTypeModel : EntityObject, ICloneable
    {
        [BsonElement("TypeName")]
        public string TypeName { get; set; }

        #region Implementation of ICloneable

        /// <summary>Creates a new object that is a copy of the current instance.</summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone()
        {
            var recordModel = (EmployeeTypeModel)MemberwiseClone();
            return recordModel;
        }

        #endregion
    }
}
