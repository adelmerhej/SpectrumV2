using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Spectrum.Models.Projects
{
	public class ProjectModel : EntityObject, ICloneable
	{
		// ==========================================================
		// 1. Project Information (References and Core Data)
		// ==========================================================

		// References: Storing IDs for related collections (Area, Engineer, Client, etc.).
		// Use string for MongoDB ObjectId references.

		[BsonElement("Reference ")]                                 // "Project Reference "
		public string Reference { get; set; }

		[BsonElement("ProjectName")]                               // "Project Name"
		public string ProjectName { get; set; }

		[BsonElement("TentativeReference ")]                                 // "Tentative Project Reference "
		public string TentativeReference { get; set; }

		[BsonElement("ContractReferenceId")]                       // Optional Reference to the Quotation document
		[BsonRepresentation(BsonType.ObjectId)]
		public ObjectId? QuotationReferenceId { get; set; }

		[BsonElement("Contract ")]                                 // "Quotation Reference "
		public string Contract { get; set; }

		public string JointVenture { get; set; }

		[BsonElement("ClientId")]                                   // Optional ref to Clients collection
		[BsonRepresentation(BsonType.ObjectId)]
		public ObjectId? ClientId { get; set; }

		[BsonElement("ClientName ")]                                // denormalized client name
		public string ClientName { get; set; }


		[BsonElement("ContactId")]                                   // Optional ref to Clients Contact Collection
		[BsonRepresentation(BsonType.ObjectId)]
		public ObjectId? ContactId { get; set; }
		[BsonElement("ClientContact ")]                                // denormalized client Contact
		public string ClientContact { get; set; }


		[BsonElement("FundedById")]                                   // Optional ref to Funded By Collection
		[BsonRepresentation(BsonType.ObjectId)]
		public ObjectId? FundedById { get; set; }
		[BsonElement("FundedBy ")]                                // denormalized Funded By
		public string FundedBy { get; set; }


		// Personnel / Ownership
		//------------------------------------------------
		[BsonElement("EngineerInCharge ")]                          // "Engineer in-charge"
		public string EngineerInCharge { get; set; }

		[BsonElement("EngineerId")]                                 // optional ref to Engineers collection
		[BsonRepresentation(BsonType.ObjectId)]
		public ObjectId? EngineerId { get; set; }

		// Personnel / Username
		//------------------------------------------------
		[BsonElement("Username ")]                          // "The user who created this record is responsible for assigning tasks."
		public string Username { get; set; }

		[BsonElement("UserId")]                                 // optional ref to first User in User collection
		[BsonRepresentation(BsonType.ObjectId)]
		public ObjectId? UserId { get; set; }

		// Location
		//------------------------------------------------
		[BsonElement("Location ")]                                  // Location
		public LocationInfoModel Location { get; set; }

		[BsonElement("Area")]
		public string Area { get; set; }                            // "Area"

		// Dates & status
		//------------------------------------------------

		[BsonElement("YearOfIssuance ")]
		[BsonRepresentation(BsonType.Int32)]
		public int? YearOfIssuance { get; set; }                    // "Year of Issuance"

		public DateTime? ProjectDate { get; set; }                  // "ProjectDate Date"
		public DateTime? SignatureDate { get; set; }                  // "Signature Date"


		public DateTime? ExpiryDate { get; set; }                    // "Expiry Date"

		[BsonRepresentation(BsonType.String)]
		public ProjectStatus Status { get; set; }                   // "Status"

		// Contract Link   
		//------------------------------------------------
		[BsonElement("ContractLink")]
		public string ContractLink { get; set; }


		// Services (Design, Supervision...)
		//------------------------------------------------
		[BsonElement("Services")]
		public ContractInfoModel Services { get; set; }


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
