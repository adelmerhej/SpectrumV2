using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace SpectrumV1.Models.Projects
{
	public class ProjectModel : EntityObject, ICloneable
	{
		// ==========================================================
		// 1. Project Information (References and Core Data)
		// ==========================================================

		// References: Storing IDs for related collections (Area, Engineer, Client, etc.).
		// Use string for MongoDB ObjectId references.

		[BsonElement("Area")]
		public string Area { get; set; }                            // "Area"

		[BsonElement("ProjectName")]                               // "Project Name"
		public string ProjectName { get; set; }

		[BsonElement("QuotationReferenceId")]                       // Optional Reference to the Quotation document
		[BsonRepresentation(BsonType.ObjectId)]
		public ObjectId? QuotationReferenceId { get; set; }

		[BsonElement("Reference ")]                                 // "Quotation Reference "
		public string Reference { get; set; }

		[BsonElement("ClientId")]                                   // Optional ref to Clients collection
		[BsonRepresentation(BsonType.ObjectId)]
		public ObjectId? ClientId { get; set; }

		[BsonElement("ClientName ")]                                // denormalized client name
		public string ClientName { get; set; }


		// Personnel / Ownership
		//------------------------------------------------
		[BsonElement("EngineerInCharge ")]                          // "Engineer in-charge"
		public string EngineerInCharge { get; set; }

		[BsonElement("EngineerId")]                                 // optional ref to Engineers collection
		[BsonRepresentation(BsonType.ObjectId)]
		public ObjectId? EngineerId { get; set; }


		// Location
		//------------------------------------------------
		[BsonElement("Location ")]                                  // Location
		public LocationInfoModel Location { get; set; }


		// Dates & status
		//------------------------------------------------

		[BsonElement("YearOfIssuance ")]
		[BsonRepresentation(BsonType.Int32)]
		public int? YearOfIssuance { get; set; }                    // "Year of Issuance"

		public DateTime? IssuanceDate { get; set; }                  // "Issuance Date"

		public DateTime? ExpiryDate { get; set; }                    // "Expiry Date"

		[BsonRepresentation(BsonType.String)]
		public ProjectStatus Status { get; set; }                   // "Status"

		// Description & free text   
		//------------------------------------------------
		[BsonElement("Description")]
		public string Description { get; set; }


		// Contract & financials (single, canonical contract)
		//------------------------------------------------
		[BsonElement("Contract")]
		public ContractInfoModel Contract { get; set; }


		// Repeating financial adjustments (addendums) -> dynamic list
		//------------------------------------------------
		[BsonElement("Addendums")]
		public List<AddendumModel> Addendums { get; set; } = new List<AddendumModel>();


		// Invoicing related (if present in CSV)
		//------------------------------------------------
		[BsonElement("Invoices")]
		public List<InvoiceModel> Invoices { get; set; } = new List<InvoiceModel>();


		// Warranties / bank / misc
		//------------------------------------------------
		[BsonElement("Warranty")]
		public WarrantyInfoModel Warranty { get; set; }

		[BsonElement("Bank")]
		public string Bank { get; set; }

		// Audit / provenance
		//------------------------------------------------
		[BsonElement("SourceFile")] 
		public string SourceFile { get; set; }            // store original CSV filename/row id if needed


		// ==========================================================
		// 2. Embedded Sub-Documents
		// ==========================================================

		// EMBEDDED: Groups all document links and dates specific to this project
		[BsonElement("Documents")]
		public ProjectDocumentsModel Documents { get; set; } = new ProjectDocumentsModel();

		// EMBEDDED: Groups all contract, financial, and warranty details
		[BsonElement("ContractDetails")]
		public ContractDetailModel ContractDetails { get; set; } = new ContractDetailModel();


		#region Implementation of ICloneable

		public object Clone()
		{
			var recordModel = (ProjectModel)MemberwiseClone();
			return recordModel;
		}

		#endregion
	}
}
