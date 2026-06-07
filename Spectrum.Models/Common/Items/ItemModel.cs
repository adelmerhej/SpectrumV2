using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Spectrum.Models.Common.Items
{
    public class ItemModel : EntityObject, ICloneable
    {

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("Service")]
        public string Service { get; set; }

        #region Implementation of ICloneable

        /// <summary>Creates a new object that is a copy of the current instance.</summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone()
        {
            var recordModel = (ItemModel)MemberwiseClone();
            return recordModel;
        }

        #endregion
    }
}
