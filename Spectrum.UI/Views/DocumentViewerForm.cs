using DevExpress.XtraPrinting.Preview;

namespace Spectrum.Views.Reports
{
    public partial class DocumentViewerForm : DevExpress.XtraEditors.XtraForm
    {
        public DocumentViewer Viewer => documentViewer1;
        public DocumentViewerForm()
        {
            InitializeComponent();
        }
    }
}