using DevExpress.Spreadsheet;
using DevExpress.XtraSpreadsheet;
using Spectrum.Reports.Interfaces;

namespace Spectrum.Reports.Editors
{
    public interface ISpreadsheetReportEditorAdapter
    {
        string Name { get; }
        bool CanHandle(IReportAdapter reportAdapter);
        void Initialize(SpreadsheetControl spreadsheetControl, IReportAdapter reportAdapter);
        bool TryAddRecord();
        bool TryRemoveSelectedRecord();
        void ApplyA4Layout(Worksheet worksheet);
    }
}
