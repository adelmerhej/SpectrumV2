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

namespace SpectrumV1.Views.Members.Engineers
{
	public partial class EngineersListForm : RibbonForm, IFormWithRibbon
	{



		#region Implementation of IFormWithRibbon

		public RibbonControl MainRibbon => rcEngineers;
		public RibbonPage DefaultPage => rpEngineers;


		#endregion

		public EngineersListForm()
		{
			InitializeComponent();
		}
	}
}