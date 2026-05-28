using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Runtime.Serialization;

namespace Spectrum.Models.Operations.Projects.Settings.ProjectTypes
{
    [BsonIgnoreExtraElements]
    public class ProjectTypeModel : EntityObject, ICloneable
    {
        [BsonElement("Type")]
        public string Type { get; set; }

        [BsonElement("Sector")]
        public string Sector { get; set; }

        [BsonElement("Description")]
        public string Description { get; set; }

        [BsonExtraElements]
        public BsonDocument ExtraElements { get; set; }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            if (!string.IsNullOrWhiteSpace(Type) || ExtraElements == null) return;

            if (ExtraElements.Contains("Name") && ExtraElements["Name"].IsString)
            {
                Type = ExtraElements["Name"].AsString;
            }
        }

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
