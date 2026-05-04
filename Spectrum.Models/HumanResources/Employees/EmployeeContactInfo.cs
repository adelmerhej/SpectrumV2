using MongoDB.Bson.Serialization.Attributes;

namespace Spectrum.Models.HumanResources.Employees
{
    [BsonIgnoreExtraElements]
    public class EmployeeContactInfo
    {
        [BsonElement("LocalFixPhone")]
        public string LocalFixPhone { get; set; }

        [BsonElement("LocalMobileNo")]
        public string LocalMobileNo { get; set; }

        [BsonElement("AbroadMobileNo")]
        public string AbroadMobileNo { get; set; }

        [BsonElement("Email")]
        public string Email { get; set; }
    }
}
