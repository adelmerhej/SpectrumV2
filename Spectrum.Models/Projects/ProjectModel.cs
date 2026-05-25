using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Spectrum.Models.Projects
{
    [BsonIgnoreExtraElements]
	public class ProjectModel : EntityObject, ICloneable
	{
        // ==========================================================
        // 1. Project Information (References and Core Data)
        // ==========================================================

        // References: Storing IDs for related collections (Area, Engineer, Client, etc.).
        // Use string for MongoDB ObjectId references.

        [BsonElement("ProjectType ")]                                 // "Project Type "
        public string ProjectType { get; set; }

        [BsonElement("Reference ")]                                 // "Project Reference "
		public string Reference { get; set; }

		[BsonElement("ProjectName")]                               // "Project Name"
		public string ProjectName { get; set; }

		[BsonElement("TentativeReference ")]                      // "Tentative Project Reference "
		public string TentativeReference { get; set; }

		[BsonElement("ContractReferenceId")]                       // Optional Reference to the Quotation document
		[BsonRepresentation(BsonType.ObjectId)]
		public ObjectId? QuotationReferenceId { get; set; }

		[BsonElement("Contract")]                                   // "Quotation Reference"
		public string Contract { get; set; }

		public string JointVenture { get; set; }

		[BsonElement("ClientId")]                                   // Optional ref to Clients collection
		[BsonRepresentation(BsonType.ObjectId)]
		public ObjectId? ClientId { get; set; }

		[BsonElement("ClientName")]                                 // denormalized client name
		public string ClientName { get; set; }


		[BsonElement("ContactId")]                                   // Optional ref to Clients Contact Collection
		[BsonRepresentation(BsonType.ObjectId)]
		public ObjectId? ContactId { get; set; }
		[BsonElement("ClientContact")]                               // denormalized client Contact
		public string ClientContact { get; set; }


		// Personnel / Ownership
		//------------------------------------------------
		[BsonElement("EngineerInCharge")]                           // "Engineer in-charge"
		public string EngineerInCharge { get; set; }

		[BsonElement("EngineerId")]                                 // optional ref to Engineers collection
		[BsonRepresentation(BsonType.ObjectId)]
		public ObjectId? EngineerId { get; set; }

		// Personnel / Username
		//------------------------------------------------
		[BsonElement("Username")]                           // "The user who created this record is responsible for assigning tasks."
		public string Username { get; set; }

		[BsonElement("UserId")]                                 // optional ref to first User in User collection
		[BsonRepresentation(BsonType.ObjectId)]
		public ObjectId? UserId { get; set; }

		[BsonIgnore]
		public string ClientIdValue
		{
			get { return ClientId?.ToString(); }
			set { ClientId = ParseObjectId(value); }
		}

		[BsonIgnore]
		public string EngineerIdValue
		{
			get { return EngineerId?.ToString(); }
			set { EngineerId = ParseObjectId(value); }
		}

		[BsonIgnore]
		public string UserIdValue
		{
			get { return UserId?.ToString(); }
			set { UserId = ParseObjectId(value); }
		}

		// Location
		//------------------------------------------------
		[BsonElement("Location")]                                   // Location
		public LocationInfoModel Location { get; set; } = new LocationInfoModel();

		[BsonElement("Area")]
		[BsonIgnoreIfNull]
		private string LegacyArea
		{
			get { return null; }
			set
			{
				if (string.IsNullOrWhiteSpace(value)) return;
				Location = Location ?? new LocationInfoModel();
				if (string.IsNullOrWhiteSpace(Location.Area))
				{
					Location.Area = value;
				}
			}
		}

		[BsonElement("Country")]
		[BsonIgnoreIfNull]
		private string LegacyCountry
		{
			get { return null; }
			set
			{
				if (string.IsNullOrWhiteSpace(value)) return;
				Location = Location ?? new LocationInfoModel();
				if (string.IsNullOrWhiteSpace(Location.Country))
				{
					Location.Country = value;
				}
			}
		}

		[BsonElement("City")]
		[BsonIgnoreIfNull]
		private string LegacyCity
		{
			get { return null; }
			set
			{
				if (string.IsNullOrWhiteSpace(value)) return;
				Location = Location ?? new LocationInfoModel();
				if (string.IsNullOrWhiteSpace(Location.City))
				{
					Location.City = value;
				}
			}
		}

       // Dates & status
		//------------------------------------------------

		[BsonElement("YearOfIssuance")]
		[BsonRepresentation(BsonType.Int32)]
		public int? YearOfIssuance { get; set; }                    // "Year of Issuance"

		public DateTime? ProjectDate { get; set; }                  // "ProjectDate Date"

        public DateTime? IssuanceDate { get; set; }                  // "IssuanceDate Date"

        public DateTime? ExpiryDate { get; set; }                    // "Expiry Date"

		[BsonRepresentation(BsonType.String)]
		public ProjectStatus Status { get; set; }                   // "Status"

		// Repeating financial adjustments (addendums) -> dynamic list
		//------------------------------------------------
		[BsonElement("Addendums")]
		public List<AddendumModel> Addendums { get; set; } = new List<AddendumModel>();


		// Invoicing related (if present in CSV)
		//------------------------------------------------
		[BsonElement("Invoices")]
		public List<InvoiceModel> Invoices { get; set; } = new List<InvoiceModel>();

		[BsonElement("ProjectHandovers")]
		public List<ProjectHandoverModel> ProjectHandovers { get; set; } = new List<ProjectHandoverModel>();


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
            return MemberwiseClone();
        }

        #endregion

		private static ObjectId? ParseObjectId(string value)
		{
			if (string.IsNullOrWhiteSpace(value)) return null;
			ObjectId objectId;
			return ObjectId.TryParse(value, out objectId) ? objectId : (ObjectId?)null;
		}
    }

	public class ProjectHandoverModel
	{
		[BsonElement("_id")]
		public string _id { get; set; }

		[BsonElement("HandingOver")]
		public string HandingOver { get; set; }

		[BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
		[BsonElement("HandingOverDate")]
		public DateTime? HandingOverDate { get; set; }
	}
}
