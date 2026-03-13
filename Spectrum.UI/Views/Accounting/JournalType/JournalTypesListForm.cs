using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
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

namespace Spectrum.Views.Accounting.JournalType
{
    public partial class JournalTypesListForm : RibbonForm, IFormWithRibbon
    {



        #region Implementation of IFormWithRibbon

        public RibbonControl MainRibbon => rcJournalTypes;
        public RibbonPage DefaultPage => rpJournalTypes;


        #endregion


        public JournalTypesListForm()
        {
            InitializeComponent();
        }
    }
}