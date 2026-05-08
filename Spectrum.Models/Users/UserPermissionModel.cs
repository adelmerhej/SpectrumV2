using System;

namespace Spectrum.Models.Users
{
	public class UserPermissionModel : EntityObject, ICloneable
	{
		public int UserPermissionId { get; set; }
		public string ControlName { get; set; }
		public bool Value { get; set; }

        #region Implementation of ICloneable

        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion
    }
}
