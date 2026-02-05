using DevExpress.DashboardCommon.Native;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using Spectrum.Models.Accounting.Charts;
using Spectrum.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.Accounting.Charts
{
	public partial class ChartsListForm : RibbonForm, IFormWithRibbon
	{
		private ChartModel _chartModel = new ChartModel();
		private IList<ChartModel> _charts = new List<ChartModel>();



		#region Implementation of IFormWithRibbon

		public RibbonControl MainRibbon => rcCharts;
		public RibbonPage DefaultPage => rpCharts;


		#endregion

		public ChartsListForm()
		{
			InitializeComponent();
		}

		private void InitializeBindings()
		{

		}

		private void WireUpBindings()
		{

		}

		private void btnClose_ItemClick(object sender, ItemClickEventArgs e)
		{
			Close();	
		}

		private void btnNew_ItemClick(object sender, ItemClickEventArgs e)
		{
			ChartEditForm frm = new ChartEditForm(new ChartModel());
			frm.SendUpdatedChartAccount += RcvUpdatedChartAccount;
			frm.ShowDialog();
		}

		private void RcvUpdatedChartAccount(object sender, EventArgs e)
		{
			if (sender == null) return;
			_chartModel = sender as ChartModel;

			if (_chartModel != null && (_chartModel.LastModifiedDate == null || _chartModel.Deleted))
			{
				InitializeBindings();
				WireUpBindings();
			}
			else
			{
				chartTreeList.Update();
			}
		}
	}
}