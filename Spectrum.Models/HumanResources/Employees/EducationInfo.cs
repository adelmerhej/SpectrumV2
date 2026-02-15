using MongoDB.Bson.Serialization.Attributes;

namespace Spectrum.Models.HumanResources.Employees
{
    [BsonIgnoreExtraElements]
    public class EducationInfo
    {
        [BsonElement("Degree")]
        public string Degree { get; set; }

        [BsonElement("Specialization")]
        public string Specialization { get; set; }

        [BsonElement("SchoolOrUniversity")]
        public string SchoolOrUniversity { get; set; }

        [BsonElement("Place")]
        public string Place { get; set; }

        [BsonElement("GraduationYear")]
        public int? GraduationYear { get; set; }
    }
}
