using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Utilities.Interfaces;
using SpectrumV1.DataLayers.HumanResources.Candidates;
using SpectrumV1.Models.HumanResources.Candidates;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Views.HumanResources.HRCVs
{
    public partial class TestForm : RibbonForm, IFormWithRibbon
    {
        private bool _resetMenu;
        private HrCvEditForm _hrCvEditForm;

        private CandidateModel _candidateModel = new CandidateModel();
        private IList<CandidateModel> _candidates = new List<CandidateModel>();

        private readonly CandidateRepository _candidateRepository = new CandidateRepository(DatabaseFactory.ProfilePrimary);

        //Init permissionvariables
        private bool _canAdd = true;
        private bool _canEdit = true;
        private bool _canDelete = true;
        private bool _canPrint = true;
        private bool _isAdmin = true;
        private bool _isProtected = true;

        #region Implementation of IFormWithRibbon

        public RibbonControl MainRibbon => rpTestList;
        public RibbonPage DefaultPage => rcTestList;


        #endregion

        public TestForm()
        {
            InitializeComponent();

            StartLoading();
        }

        private void StartLoading()
        {
            InitializeBindings();
            WireUpBindings();
            ApplyDefaults();
            ApplyPermissions();
        }

        private void InitializeBindings()
        {

        }

        private void WireUpBindings()
        {

        }


        private void ApplyDefaults()
        {

        }   

        private void ApplyPermissions()
        {
            btnNew.Enabled = _canAdd;
            btnSave.Enabled = _canEdit;
            btnSaveAndClose.Enabled = _canEdit;
            btnDelete.Enabled = _canDelete;
            btnPrint.Enabled = _canPrint;
        }

        #region button click events
        private void btnNew_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void btnSave_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void btnSaveAndClose_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void btnRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void btnPrint_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void btnClose_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }

        #endregion

    }
}