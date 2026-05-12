using MongoDB.Bson.Serialization.Attributes;
using Spectrum.Models;
using System;

namespace Spectrum.Models.HumanResources.Employees.EmployeeStatus
{
    public class StatusModel : EntityObject, ICloneable
    {
        [BsonElement("Status")]
        public string Status { get; set; }

        #region Implementation of ICloneable

        /// <summary>Creates a new object that is a copy of the current instance.</summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone()
        {
            var recordModel = (StatusModel)MemberwiseClone();
            return recordModel;
        }

        #endregion
    }
}
