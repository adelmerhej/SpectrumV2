using Spectrum.Models;
using Spectrum.Models.Users;
using System;

namespace Spectrum.Utilities
{
	internal class LogInfoRepository
	{
		#region Update user activity log
		public void CreateLogInfo<T>(T obj) where T : EntityObject
		{
			obj.CreatedBy = CurrentUser.UserName;
			obj.CreatedAt = DateTime.UtcNow;
			obj.Company = CurrentUser.Company;
			obj.Branch = CurrentUser.Branch;
			obj.WorkingYear = DateTime.Now.Year;
		}

		public void UpdateLogInfo<T>(T obj) where T : EntityObject
		{
			obj.LastModifiedBy = CurrentUser.UserName;
			obj.LastModifiedDate = DateTime.UtcNow;
		}

		#endregion
	}
}
