using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Spectrum.Models.Common.Services
{
    public class ServiceModel : EntityObject, ICloneable
    {
        [BsonElement("ServiceName")]
        public string ServiceName { get; set; }

        [BsonElement("ServiceCode")]
        public string ServiceCode { get; set; }

        #region Implementation of ICloneable

        public object Clone()
        {
            var recordModel = (ServiceModel)MemberwiseClone();
            return recordModel;
        }

        #endregion
    }
}
