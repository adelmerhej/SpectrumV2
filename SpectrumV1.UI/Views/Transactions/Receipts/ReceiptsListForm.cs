using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using SpectrumV1.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpectrumV1.Views.Transactions.Receipts
{
	public partial class ReceiptsListForm : RibbonForm, IFormWithRibbon
	{



		#region Implementation of IFormWithRibbon

		public RibbonControl MainRibbon => rcReceipts;
		public RibbonPage DefaultPage => rpReceipts;


		#endregion


		public ReceiptsListForm()
		{
			InitializeComponent();
		}


		#region Buttons Event

		private void btnNew_ItemClick(object sender, ItemClickEventArgs e)
		{
			ReceiptEditForm receiptEditForm = new ReceiptEditForm();
			receiptEditForm.ShowDialog();
		}

		private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
		{

		}

		private void btnRefresh_ItemClick(object sender, ItemClickEventArgs e)
		{

		}

		private void btnPrint_ItemClick(object sender, ItemClickEventArgs e)
		{

		}

		private void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
		{

		}

		private void btnClose_ItemClick(object sender, ItemClickEventArgs e)
		{
			Close();
		}

		private void btnResetGridStyle_ItemClick(object sender, ItemClickEventArgs e)
		{

		}

		#endregion

	}
}