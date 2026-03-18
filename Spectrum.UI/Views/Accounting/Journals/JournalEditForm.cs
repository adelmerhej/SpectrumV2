using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using SpectrumV1.Models.Accounting.Journals;
using System;

namespace Spectrum.Views.Accounting.Journals
{


    public partial class JournalEditForm : RibbonForm
    {


        public EventHandler SendUpdatedJournal;

        public JournalEditForm(JournalModel model)
        {
            InitializeComponent();
        }


        #region Button Click Events

        private void btnNew_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void btnSaveAs_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void btnRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void btnSave_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void btnSaveAndClose_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void btnPrint_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void chkShowProtected_CheckedChanged(object sender, ItemClickEventArgs e)
        {

        }

        private void btnPrintStatement_ItemClick(object sender, ItemClickEventArgs e)
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