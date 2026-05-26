using MongoDB.Bson.Serialization.Attributes;
using Spectrum.Models;
using System;
using System.Collections.Generic;

namespace Spectrum.Models.HumanResources.Candidates
{
    public class CandidateModel : EntityObject, ICloneable
    {
        [BsonElement("FirstName")]
        public string FirstName { get; set; }
        [BsonElement("LastName")]
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Nationality { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Position { get; set; }
        public string YearsOfExperience { get; set; }
        public string Skills { get; set; }
        public string Summary { get; set; }

        // Phase 2 pre-processing artifacts
        public string RawExtractedText { get; set; }
        public string PreprocessedJson { get; set; }

        public List<EducationEntryModel> Education { get; set; } = new List<EducationEntryModel>();
        public List<WorkExperienceModel> History { get; set; } = new List<WorkExperienceModel>();
        public double Confidence { get; set; } // 0.0 to 1.0
        public string RawInsights { get; set; }
        public string DocumentLink { get; set; }

        #region Implementation of ICloneable

        /// <summary>Creates a new object that is a copy of the current instance.</summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone()
        {
            var recordModel = (CandidateModel)MemberwiseClone();
            return recordModel;
        }

        #endregion
    }
}
