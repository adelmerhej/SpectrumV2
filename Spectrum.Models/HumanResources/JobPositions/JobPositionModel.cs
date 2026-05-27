using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Spectrum.Models.HumanResources.JobPositions
{
	public class JobPositionModel : EntityObject, ICloneable
	{
		[BsonElement("PositionName")]
		public string PositionName { get; set; }

        #region Implementation of ICloneable

        /// <summary>Creates a new object that is a copy of the current instance.</summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion
    }
}
