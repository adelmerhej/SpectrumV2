using DevExpress.XtraEditors;
using SpectrumV1.Models.Accounting.Charts;
using System;

namespace SpectrumV1.Views.Accounting.Charts
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