using DevExpress.XtraEditors;
using Spectrum.Models.Accounting.Charts;
using System;

namespace Spectrum.Views.Accounting.Charts
{
	public partial class ChartEditForm : XtraForm
	{


		public EventHandler SendUpdatedChartAccount;

		public ChartEditForm(ChartModel model)
		{
			InitializeComponent();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}