using Spectrum.Models;
using System;

namespace SpectrumV1.Models.Accounting.JournalType
{
    public class JournalTypeModel : EntityObject, ICloneable
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int ChartId { get; set; }
        public string SpecialReport { get; set; }



        #region Implementation of ICloneable

        /// <summary>Creates a new object that is a copy of the current instance.</summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
