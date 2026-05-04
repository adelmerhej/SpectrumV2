using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using Spectrum.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Spectrum.Views.Transactions.Invoices
{
	public partial class MyInvoiceForm : RibbonForm, IFormWithRibbon
	{
		private List<SampleInvoiceItem> _invoiceItems;

		#region Implementation of IFormWithRibbon

		public RibbonControl MainRibbon => rcMyInvoice;
		public RibbonPage DefaultPage => rpMyInvoice;

		#endregion

		public MyInvoiceForm()
		{
			InitializeComponent();
			LoadSampleData();
		}

		private void LoadSampleData()
		{
			_invoiceItems = new List<SampleInvoiceItem>
			{
				new SampleInvoiceItem { Id = 1, InvoiceNumber = "INV-001", CustomerName = "Acme Corp", Description = "Consulting Services", Quantity = 10, UnitPrice = 150.00m, InvoiceDate = DateTime.Now.AddDays(-30) },
				new SampleInvoiceItem { Id = 2, InvoiceNumber = "INV-002", CustomerName = "Tech Solutions", Description = "Software Development", Quantity = 40, UnitPrice = 125.00m, InvoiceDate = DateTime.Now.AddDays(-25) },
				new SampleInvoiceItem { Id = 3, InvoiceNumber = "INV-003", CustomerName = "Global Industries", Description = "Project Management", Quantity = 20, UnitPrice = 200.00m, InvoiceDate = DateTime.Now.AddDays(-20) },
				new SampleInvoiceItem { Id = 4, InvoiceNumber = "INV-004", CustomerName = "Prime Services", Description = "Training Session", Quantity = 5, UnitPrice = 500.00m, InvoiceDate = DateTime.Now.AddDays(-15) },
				new SampleInvoiceItem { Id = 5, InvoiceNumber = "INV-005", CustomerName = "Delta Corp", Description = "System Integration", Quantity = 15, UnitPrice = 300.00m, InvoiceDate = DateTime.Now.AddDays(-10) }
			};

			gcMyInvoice.DataSource = _invoiceItems;
		}

		#region Button Event Handlers

		private void btnRefresh_ItemClick(object sender, ItemClickEventArgs e)
		{
			LoadSampleData();
		}

		private void btnClose_ItemClick(object sender, ItemClickEventArgs e)
		{
			Close();
		}

		#endregion
	}

	public class SampleInvoiceItem
	{
		public int Id { get; set; }
		public string InvoiceNumber { get; set; }
		public string CustomerName { get; set; }
		public string Description { get; set; }
		public int Quantity { get; set; }
		public decimal UnitPrice { get; set; }
		public decimal TotalAmount => Quantity * UnitPrice;
		public DateTime InvoiceDate { get; set; }
	}
}
