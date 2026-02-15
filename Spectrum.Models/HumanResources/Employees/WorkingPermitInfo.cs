using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Spectrum.Models.HumanResources.Employees
{
    [BsonIgnoreExtraElements]
    public class WorkingPermitInfo
    {
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        [BsonElement("PermitDate")]
        public DateTime? PermitDate { get; set; }
    }
}
