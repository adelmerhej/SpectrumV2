namespace Spectrum.Models.Administration.Connections
{
	public class ConnectionModel
	{
		public string Name { get; set; } 
		public string DatabaseType { get; set; }
		public string DatabaseHost { get; set; }
		public int DatabasePort { get; set; }
		public string DatabaseName { get; set; }
		public string DatabaseUser { get; set; }
		public string DatabasePassword { get; set; }

		public string DatabaseConnectionString { get; set; }
		public string AiModel { get; set; }
		public string ProjectsDocumentsFolder { get; set; }
		public string EmployeesDocumentsFolder { get; set; }
		public string EngineersDocumentsFolder { get; set; }

		// SENSITIVE FIELDS: Stored Encrypted
		public string EncryptedPassword { get; set; }
		public string EncryptedConnectionString { get; set; }
		public string EncryptedAiApikey { get; set; }

	}
}
