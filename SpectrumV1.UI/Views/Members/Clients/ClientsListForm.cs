using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using SpectrumV1.Models.Members.Clients;
using SpectrumV1.Utilities.Interfaces;

namespace SpectrumV1.Views.Members.Clients
{
	public partial class ClientsListForm : RibbonForm, IFormWithRibbon
	{



		//Init permission variables
		private bool _canAdd;
		private bool _canEdit;
		private bool _canDelete;
		private bool _canPrint;
		private bool _isAdmin;
		private bool _isProtected;
		private bool _isLimitedView;

		#region Implementation of IFormWithRibbon

		public RibbonControl MainRibbon => rcCustomersList;
		public RibbonPage DefaultPage => rpCustomersList;


		#endregion

		public ClientsListForm()
		{
			InitializeComponent();
		}

		private void btnRefresh_ItemClick(object sender, ItemClickEventArgs e)
		{
			var newForm = new ClientEditForm(new ClientModel());
			newForm.ShowDialog();
		}
	}
}