using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.Transactions.Statements
{
	public partial class StatementOfAccountForm : XtraForm
	{
		private const string _formName = "StatementOfAccount";
		private int _formId;
		private bool _resetMenu;


		public StatementOfAccountForm()
		{
			InitializeComponent();
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void btnPrint_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}