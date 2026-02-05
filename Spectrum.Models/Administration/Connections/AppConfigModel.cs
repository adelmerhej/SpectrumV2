using System.Collections.Generic;

namespace Spectrum.Models.Administration.Connections
{
	public class AppConfigModel
	{
		public Dictionary<string, ConnectionModel> Connections { get; set; } = new Dictionary<string, ConnectionModel>();
	}
}
