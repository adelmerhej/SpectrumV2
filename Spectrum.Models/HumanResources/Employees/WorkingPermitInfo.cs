using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Spectrum.Models.HumanResources.Employees
{
    [BsonIgnoreExtraElements]
    public class WorkingPermitInfo
    {
        [BsonElement("RegistrationNo")]
        public string RegistrationNo { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        [BsonElement("PermitDate")]
        public DateTime? PermitDate { get; set; }
    }
}
