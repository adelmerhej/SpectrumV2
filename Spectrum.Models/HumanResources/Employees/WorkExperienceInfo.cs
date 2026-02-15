using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Spectrum.Models.HumanResources.Employees
{
    [BsonIgnoreExtraElements]
    public class WorkExperienceInfo
    {
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        [BsonElement("OfficialWorkingDay")]
        public DateTime? OfficialWorkingDay { get; set; }

        [BsonElement("WorkingPosition")]
        public string WorkingPosition { get; set; }

        [BsonElement("TotalYearsOfExperience")]
        public int TotalYearsOfExperience { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        [BsonElement("SpectrumWorkingDate")]
        public DateTime? SpectrumWorkingDate { get; set; }

        [BsonElement("YearsAtSpectrum")]
        public int YearsAtSpectrum { get; set; }

        [BsonElement("WorkLocation")]
        public string WorkLocation { get; set; }
    }
}
