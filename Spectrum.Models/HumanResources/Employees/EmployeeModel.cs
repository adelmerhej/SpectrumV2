using MongoDB.Bson.Serialization.Attributes;
using Spectrum.Utilities.Enums;
using SpectrumV1.Models.HumanResources.EmployeeTypes;
using SpectrumV1.Models.Projects.Serializers;
using System;
using System.Collections.Generic;

namespace Spectrum.Models.HumanResources.Employees
{
    [BsonIgnoreExtraElements]
    public class EmployeeModel : EntityObject, ICloneable
    {
        [BsonElement("EmployeeNo")]
        public int EmployeeNo { get; set; }

        [BsonElement("FirstName")]
        public string FirstName { get; set; }

        [BsonElement("LastName")]
        public string LastName { get; set; }

        [BsonElement("Specialization")]
        public string Specialization { get; set; }

        [BsonElement("FatherName")]
        public string FatherName { get; set; }

        [BsonElement("MotherFullName")]
        public string MotherFullName { get; set; }

        [BsonElement("BloodType")]
        public string BloodType { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        [BsonElement("DateOfBirth")]
        public DateTime? DateOfBirth { get; set; }

        [BsonElement("PlaceOfBirth")]
        public string PlaceOfBirth { get; set; }

        [BsonElement("ActualPosition")]
        public string ActualPosition { get; set; }

        [BsonElement("Address")]
        public string Address { get; set; }

        [BsonElement("RecordRegister")]
        public string RecordRegister { get; set; }

        [BsonElement("Nationality")]
        public string Nationality { get; set; }

        [BsonElement("IdCardOrPassportNo")]
        public string IdCardOrPassportNo { get; set; }

        [BsonElement("FamilyStatus")]
        public string FamilyStatus { get; set; }

        [BsonElement("EmployeeType")]
        public string EmployeeType { get; set; }

        [BsonElement("EnumEmployeeType")]
        public EnumEmployeeType EnumEmployeeType { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        [BsonElement("HiredDate")]
        public DateTime HiredDate { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        [BsonElement("WorkingDate")]
        public DateTime? WorkingDate { get; set; }

        [BsonElement("YearsOfExperience")]
        [BsonSerializer(typeof(SafeInt32Serializer))]
        public int YearsOfExperience { get; set; }

        [BsonElement("LeftWork")]
        public bool LeftWork { get; set; }

        [BsonElement("ContactInfo")]
        public EmployeeContactInfo ContactInfo { get; set; }

        [BsonElement("EmergencyContact")]
        public EmergencyContactInfo EmergencyContact { get; set; }

        [BsonElement("Cnss")]
        public CnssInfo Cnss { get; set; }

        [BsonElement("Syndicat")]
        public SyndicatInfo Syndicat { get; set; }

        [BsonElement("WorkingPermit")]
        public WorkingPermitInfo WorkingPermit { get; set; }

        [BsonElement("Financial")]
        public FinancialInfo Financial { get; set; }

        [BsonElement("Education")]
        public List<EducationInfo> Education { get; set; } = new List<EducationInfo>();

        [BsonElement("WorkExperience")]
        public WorkExperienceInfo WorkExperience { get; set; }

        [BsonElement("DocumentLink")]
        public string DocumentLink { get; set; }


        public string FullName
        {
            get { return $"{FirstName} {LastName}"; }
        }

        #region Implementation of ICloneable

        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion
    }
}
