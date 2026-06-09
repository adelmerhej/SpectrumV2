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

		// Legacy flat AI settings (kept for compatibility)
		public string AiModel { get; set; }
		public string EncryptedAiApikey { get; set; }

		// Phase 1: grouped AI settings
		public AiSettingsModel AiSettings { get; set; } = new AiSettingsModel();

		public string ProjectsDocumentsFolder { get; set; }
		public string EmployeesDocumentsFolder { get; set; }
		public string EngineersDocumentsFolder { get; set; }
        public string InvoicesDocumentsFolder { get; set; }
		public string InvoicePostingInvoiceType { get; set; }
		public string InvoicePostingJournalType { get; set; }
		public string InvoicePostingCustomerType { get; set; }
		public string InvoicePostingCustomerDbCr { get; set; }
		public string InvoicePostingVatType { get; set; }
		public string InvoicePostingVatAccountType { get; set; }
		public string InvoicePostingVatAccountNumber { get; set; }

        // SENSITIVE FIELDS: Stored Encrypted
        public string EncryptedPassword { get; set; }
		public string EncryptedConnectionString { get; set; }
	}

	public class AiSettingsModel
	{
		public string Provider { get; set; }
		public string OpenAiModel { get; set; }
		public string DeepSeekModel { get; set; }

		// Stored encrypted on disk, decrypted in runtime model (same pattern as legacy EncryptedAiApikey)
		public string EncryptedOpenAiApiKey { get; set; }
		public string EncryptedDeepSeekApiKey { get; set; }
	}
}
