using MongoDB.Bson.Serialization.Attributes;

namespace Spectrum.Models.HumanResources.Employees
{
    [BsonIgnoreExtraElements]
    public class EmergencyContactInfo
    {
        [BsonElement("ContactName")]
        public string ContactName { get; set; }

        [BsonElement("MobileNo")]
        public string MobileNo { get; set; }
    }
}
