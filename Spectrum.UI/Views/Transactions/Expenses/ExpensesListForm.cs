using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using Spectrum.Utilities.Interfaces;

namespace Spectrum.Views.Transactions.Expenses
{
	public partial class ExpensesListForm : RibbonForm, IFormWithRibbon
	{


		#region Implementation of IFormWithRibbon

		public RibbonControl MainRibbon => rcExpenses;
		public RibbonPage DefaultPage => rpExpenses;


		#endregion

		public ExpensesListForm()
		{
			InitializeComponent();
		}



		#region Buttons Event Handlers

		private void btnNew_ItemClick(object sender, ItemClickEventArgs e)
		{

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