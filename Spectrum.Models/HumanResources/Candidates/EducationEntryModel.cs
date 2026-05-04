using MongoDB.Bson.Serialization.Attributes;

namespace SpectrumV1.Models.HumanResources.Candidates
{
    public class EducationEntryModel
    {
        [BsonElement("Degree")]
        public string Degree { get; set; }
        public string Institution { get; set; }
        public string Year { get; set; }
        public string Specialization { get; set; }
    }
}
