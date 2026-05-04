using MongoDB.Bson.Serialization.Attributes;
using Spectrum.Models;
using System;

namespace SpectrumV1.Models.Accounting.FlowType
{
    public class FlowTypeModel : EntityObject, ICloneable
    {
        [BsonElement("FlowTypeName")]
        public string FlowTypeName { get; set; }

        #region Implementation of ICloneable

        /// <summary>Creates a new object that is a copy of the current instance.</summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone()
        {
            var recordModel = (FlowTypeModel)MemberwiseClone();
            return recordModel;
        }

        #endregion
    }
}
