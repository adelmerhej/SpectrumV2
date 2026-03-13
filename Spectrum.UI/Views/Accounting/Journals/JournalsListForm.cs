using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.Accounting.Journals
{
    public partial class JournalsListForm : RibbonForm
    {





        #region Implementation of IFormWithRibbon

        public RibbonControl MainRibbon => rcJournals;
        public RibbonPage DefaultPage => rpJournals;


        #endregion


        public JournalsListForm()
        {
            InitializeComponent();
        }
    }
}