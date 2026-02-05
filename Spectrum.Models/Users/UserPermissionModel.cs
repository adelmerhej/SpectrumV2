using System;

namespace Spectrum.Models.Users
{
	public class UserPermissionModel : EntityObject, ICloneable
	{
		public int UserPermissionId { get; set; }
		public string ControlName { get; set; }
		public bool Value { get; set; }

		#region Implementation of ICloneable

		public virtual object Clone()
		{
			var newModel = (UserPermissionModel)MemberwiseClone();
			return newModel;
		}

		#endregion
	}
}
