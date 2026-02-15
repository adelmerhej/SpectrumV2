using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Spectrum.Models.HumanResources.Employees
{
    [BsonIgnoreExtraElements]
    public class CnssInfo
    {
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        [BsonElement("RegistrationDate")]
        public DateTime? RegistrationDate { get; set; }

        [BsonElement("RegistrationNo")]
        public string RegistrationNo { get; set; }

        [BsonElement("NoOfChildren")]
        public int NoOfChildren { get; set; }

        [BsonElement("NoOfBeneficiary")]
        public int NoOfBeneficiary { get; set; }

        [BsonElement("SpouseRegistration")]
        public bool SpouseRegistration { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        [BsonElement("CnssLeaveDate")]
        public DateTime? CnssLeaveDate { get; set; }
    }
}
