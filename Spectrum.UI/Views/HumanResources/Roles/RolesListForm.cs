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

namespace Spectrum.Views.HumanResources.Roles
{
    public partial class RolesListForm : RibbonForm, IFormWithRibbon
    {



        #region Implementation of IFormWithRibbon

        public RibbonControl MainRibbon => rcRolesList;
        public RibbonPage DefaultPage => rpRolesList;


        #endregion

        public RolesListForm()
        {
            InitializeComponent();
        }

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
    }
}