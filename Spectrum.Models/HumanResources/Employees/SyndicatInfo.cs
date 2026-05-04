using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Spectrum.Models.HumanResources.Employees
{
    [BsonIgnoreExtraElements]
    public class SyndicatInfo
    {
        [BsonElement("SyndicatNo")]
        public string SyndicatNo { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        [BsonElement("RegistrationDate")]
        public DateTime? RegistrationDate { get; set; }

        [BsonElement("RegistrationPlace")]
        public string RegistrationPlace { get; set; }
    }
}
