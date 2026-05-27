using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Spectrum.Models.Projects.Serializers;
using System;
using System.Collections.Generic;

namespace Spectrum.Models.Projects
{
	// Embedded document for contract, financial, and warranty terms
	[BsonIgnoreExtraElements]
	public class ContractDetailModel
	{// ==========================================================
	 // Contract References & Core Info
	 // ==========================================================

		// Reference to Services lookup collection
		[BsonElement("ServiceId")]
		[BsonSerializer(typeof(SafeObjectIdSerializer))]
		public string ServiceId { get; set; }

		// Reference to Type lookup collection
		[BsonElement("TypeId")]
		[BsonSerializer(typeof(SafeObjectIdSerializer))]
		public string TypeId { get; set; }

		// Reference to Sponsor/Funder collection
		[BsonElement("SponsorId")]
		[BsonSerializer(typeof(SafeObjectIdSerializer))]
		public string SponsorId { get; set; }


		[BsonElement("ContractNumber")]
		public string ContractNumber { get; set; }

		[BsonElement("ContractFileLink")]
		public string ContractFileLink { get; set; }

		[BsonElement("SignatureDate")]
		public DateTime? SignatureDate { get; set; }

		[BsonElement("ContractPeriod")]
		public string ContractPeriod { get; set; }

		[BsonElement("ExtensionDetails")]
		public string ExtensionDetails { get; set; }

		[BsonElement("ActualCompletionDate")]
		public DateTime? ActualCompletionDate { get; set; }

		[BsonElement("ClientContactEmail")]
		public string ClientContactEmail { get; set; }

		[BsonElement("CurrencyCode")]
		public string CurrencyCode { get; set; }

		[BsonElement("ServicesProvided")]
		public List<string> ServicesProvided { get; set; } = new List<string>();

		[BsonElement("ServiceTypes")]
		public List<string> ServiceTypes { get; set; } = new List<string>();


		// ==========================================================
		// Financial Amounts
		// IMPORTANT: Use Decimal128 for precise currency storage in MongoDB
		// ==========================================================

		[BsonRepresentation(BsonType.Decimal128)]
		[BsonElement("DesignFee")]
		public decimal? DesignFee { get; set; }

		[BsonRepresentation(BsonType.Decimal128)]
		[BsonElement("SupervisionFee")]
		public decimal? SupervisionFee { get; set; }

		[BsonRepresentation(BsonType.Decimal128)]
		[BsonElement("InitialContractAmount")]
		public decimal? InitialContractAmount { get; set; }

		[BsonRepresentation(BsonType.Decimal128)]
		[BsonElement("RetentionPercentage")]
		public decimal? RetentionPercentage { get; set; }

		[BsonRepresentation(BsonType.Decimal128)]
		[BsonElement("InitialVatAmount")]
		public decimal? InitialVatAmount { get; set; }

		[BsonRepresentation(BsonType.Decimal128)]
		[BsonElement("InitialTtcAmount")]
		public decimal? InitialTtcAmount { get; set; }


		// ==========================================================
		// Warranty / Guarantee
		// ==========================================================

		[BsonElement("WarrantyReference")]
		public string WarrantyReference { get; set; }

		[BsonRepresentation(BsonType.Decimal128)]
		[BsonElement("WarrantyAmount")]
		public decimal? WarrantyAmount { get; set; }

		[BsonElement("WarrantyBank")]
		public string WarrantyBank { get; set; }

		[BsonElement("WarrantyIssuanceDate")]
		public DateTime? WarrantyIssuanceDate { get; set; }

		[BsonElement("WarrantyExpiryDate")]
		public DateTime? WarrantyExpiryDate { get; set; }

		[BsonElement("WarrantyStatus")]
		public string WarrantyStatus { get; set; }

	}
}
