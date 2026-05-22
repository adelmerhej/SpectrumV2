using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace Spectrum.Reports.Interfaces
{
    public interface IHasId
    {
        long Id { get; }
    }

    public interface IWorksheetRequirements
    {
        IEnumerable<string> RequiredWorksheetNames();
    }

    public class GridColumnDescriptor
    {
        public string FieldName { get; set; }
        public string Caption { get; set; }
        public bool ReadOnly { get; set; }
        public int Width { get; set; }
        public string FormatString { get; set; }

        public GridColumnDescriptor() { }

        public GridColumnDescriptor(string fieldName, string caption, bool readOnly = false, int width = 100, string formatString = null)
        {
            FieldName = fieldName;
            Caption = caption;
            ReadOnly = readOnly;
            Width = width;
            FormatString = formatString;
        }
    }

    public class QuickActionDescriptor
    {
        public string Name { get; set; }
        public string Caption { get; set; }
        public string IconName { get; set; }
        public Func<bool> CanExecute { get; set; }
        public Action Execute { get; set; }

        public QuickActionDescriptor() { }

        public QuickActionDescriptor(string name, string caption, Action execute, Func<bool> canExecute = null, string iconName = null)
        {
            Name = name;
            Caption = caption;
            Execute = execute;
            CanExecute = canExecute;
            IconName = iconName;
        }
    }

    public class FieldDescriptor
    {
        public string Key { get; set; }
        public string Caption { get; set; }
        public string Category { get; set; }
        public Type ValueType { get; set; }
        public string FormatString { get; set; }
        public Func<object> GetValue { get; set; }
        public Func<int, object> GetRowValue { get; set; }
        public int RowCount { get; set; }
        public Func<int, object> GetSelectedValue { get; set; }
        public Func<int, int, object> GetSelectedRowValue { get; set; }
        public Func<int, int> GetSelectedRowCount { get; set; }

        public bool IsRowLevel { get { return GetRowValue != null; } }
        public bool HasSelectedRecordValues { get { return GetSelectedValue != null || GetSelectedRowValue != null; } }

        public FieldDescriptor() { }

        public FieldDescriptor(string key, string caption, string category,
            Type valueType = null, string formatString = null,
            Func<object> getValue = null,
            Func<int, object> getRowValue = null, int rowCount = 0,
            Func<int, object> getSelectedValue = null,
            Func<int, int, object> getSelectedRowValue = null,
            Func<int, int> getSelectedRowCount = null)
        {
            Key = key;
            Caption = caption;
            Category = category;
            ValueType = valueType;
            FormatString = formatString;
            GetValue = getValue;
            GetRowValue = getRowValue;
            RowCount = rowCount;
            GetSelectedValue = getSelectedValue;
            GetSelectedRowValue = getSelectedRowValue;
            GetSelectedRowCount = getSelectedRowCount;
        }
    }

    public class ReportRecordDescriptor
    {
        public string Key { get; set; }
        public string Caption { get; set; }
        public object Model { get; set; }

        public override string ToString()
        {
            return Caption ?? Key ?? base.ToString();
        }
    }

    public interface IMultiRecordReportAdapter
    {
        IList<ReportRecordDescriptor> GetAvailableRecords();
        IList<ReportRecordDescriptor> GetSelectedRecords();
        ReportRecordDescriptor GetActiveRecord();
        void UpdateRecordSelection(IList<string> selectedKeys, string activeKey);
    }

    public interface IReportAdapter : IDisposable
    {
        string Title { get; }
        object Model { get; }
        IList<GridColumnDescriptor> GetGridColumns();
        IBindingList GetGridDataSource();
        Stream GetSpreadsheetStream(out string format);
        IList<QuickActionDescriptor> GetQuickActions();
        void Recalculate();
        IList<KeyValuePair<string, string>> GetSummaryFields();
        string ExportToCsvString();
        byte[] ExportToXlsxBytes();
        XtraReport BuildReport();
        void ApplySpreadsheetChanges(DevExpress.Spreadsheet.IWorkbook workbook);
        IList<FieldDescriptor> GetFieldDescriptors();
    }
}
