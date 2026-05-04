using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Spectrum.Models.Operations.Projects
{
	public class ProjectModel : EntityObject, ICloneable
	{
		[BsonElement("ProjectName")]
		public string ProjectName { get; set; }                             // "Bridge Construction"
		public string ClientName { get; set; }                              // "Acme Corp"
		public string Description { get; set; }                             //"Design and build suspension bridge"
		public ProjectStatus Status { get; set; } = ProjectStatus.Active;      // active, completed, on_hold, cancelled

		[BsonRepresentation(BsonType.Decimal128)]
		public decimal TotalBudget { get; set; }                             //250,000.00 USD
		public string Email { get; set; }                                   // contact email for project manager or client
		public DateTime StartedDate { get; set; }
		public DateTime Deadline { get; set; }
		public DateTime ActualEndDate { get; set; }

		//Embedded Documents
		public List<ProjectAssignmentModel> Assignments { get; set; } = new List<ProjectAssignmentModel>();
		public List<TimeEntryModel> TimeEntries { get; set; } = new List<TimeEntryModel>();
		public List<ProjectCostModel> Costs { get; set; } = new List<ProjectCostModel>();
		public List<ProjectPaymentModel> Payments { get; set; } = new List<ProjectPaymentModel>();
		public FinancialSummaryModel FinancialSummary { get; set; } = new FinancialSummaryModel();

		#region Implementation of ICloneable

		/// <summary>Creates a new object that is a copy of the current instance.</summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public object Clone()
		{
			var recordModel = (ProjectModel)MemberwiseClone();
			return recordModel;
		}

		#endregion
	}
}
