using MongoDB.Bson.Serialization.Attributes;

namespace Spectrum.Models.HumanResources.Employees
{
    [BsonIgnoreExtraElements]
    public class FinancialInfo
    {
        [BsonElement("FinancialNo")]
        public string FinancialNo { get; set; }
    }
}
