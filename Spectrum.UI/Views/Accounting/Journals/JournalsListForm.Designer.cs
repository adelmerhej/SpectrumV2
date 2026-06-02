namespace Spectrum.Views.Accounting.Journals
{
    partial class JournalsListForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JournalsListForm));
            this.gvJournalDetails = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colDetailId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetailJournalId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetailLine = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetailChartId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repCharts = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colDetailAccountName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repChartList = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colDetailValueDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repDateFormat = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.colDetailCurrencyId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repCurrencies = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colDetailRate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetailDescription = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetailDbCr = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetailAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetailLAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetailFAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetailPosted = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CheckBoxFormat = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.colDetailDocumentRef = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetailJobNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetailDepartmentId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repDepartments = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colDetailCostCenterId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repCostCenters = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repFlowTypes = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colDetailNotes = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetailWorkingYear = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetailLocked = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcJournals = new DevExpress.XtraGrid.GridControl();
            this.bsJournals = new System.Windows.Forms.BindingSource(this.components);
            this.gvJournals = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colJvNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colJournalTypeId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repJournalTypes = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colJournalDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colReference = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCurrencyId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIsPosted = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNotes = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colWorkingYear = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIsProtected = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLocked = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rcJournals = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.btnNew = new DevExpress.XtraBars.BarButtonItem();
            this.btnDelete = new DevExpress.XtraBars.BarButtonItem();
            this.btnEdit = new DevExpress.XtraBars.BarButtonItem();
            this.btnPrint = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem11 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem12 = new DevExpress.XtraBars.BarButtonItem();
            this.btnRefresh = new DevExpress.XtraBars.BarButtonItem();
            this.btnClose = new DevExpress.XtraBars.BarButtonItem();
            this.btnPrintFilter = new DevExpress.XtraBars.BarButtonItem();
            this.btnResetGridStyle = new DevExpress.XtraBars.BarButtonItem();
            this.btnPrintStatement = new DevExpress.XtraBars.BarButtonItem();
            this.dtFrom = new DevExpress.XtraBars.BarEditItem();
            this.dtTo = new DevExpress.XtraBars.BarEditItem();
            this.cboWorkingYear = new DevExpress.XtraBars.BarEditItem();
            this.repWorkingYear = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.rpJournals = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup6 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup8 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.rpgDepartments = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup11 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.rpViewSettings = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.repWorkingYears = new DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit();
            this.repositoryItemSearchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.ribbonStatusBar1 = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.mainLayout = new DevExpress.XtraLayout.LayoutControl();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.gvJournalDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repCharts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repChartList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repDateFormat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repDateFormat.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repCurrencies)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CheckBoxFormat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repDepartments)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repCostCenters)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repFlowTypes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcJournals)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsJournals)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvJournals)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repJournalTypes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rcJournals)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repWorkingYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repWorkingYears)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainLayout)).BeginInit();
            this.mainLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // gvJournalDetails
            // 
            this.gvJournalDetails.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colDetailId,
            this.colDetailJournalId,
            this.colDetailLine,
            this.colDetailChartId,
            this.colDetailAccountName,
            this.colDetailValueDate,
            this.colDetailCurrencyId,
            this.colDetailRate,
            this.colDetailDescription,
            this.colDetailDbCr,
            this.colDetailAmount,
            this.colDetailLAmount,
            this.colDetailFAmount,
            this.colDetailPosted,
            this.colDetailDocumentRef,
            this.colDetailJobNo,
            this.colDetailDepartmentId,
            this.colDetailCostCenterId,
            this.gridColumn6,
            this.colDetailNotes,
            this.colDetailWorkingYear,
            this.colDetailLocked});
            this.gvJournalDetails.GridControl = this.gcJournals;
            this.gvJournalDetails.Name = "gvJournalDetails";
            this.gvJournalDetails.OptionsBehavior.Editable = false;
            this.gvJournalDetails.OptionsBehavior.ReadOnly = true;
            this.gvJournalDetails.OptionsView.ColumnAutoWidth = false;
            this.gvJournalDetails.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            // 
            // colDetailId
            // 
            this.colDetailId.Caption = "Id";
            this.colDetailId.FieldName = "Id";
            this.colDetailId.MinWidth = 25;
            this.colDetailId.Name = "colDetailId";
            this.colDetailId.Width = 57;
            // 
            // colDetailJournalId
            // 
            this.colDetailJournalId.Caption = "JournalId";
            this.colDetailJournalId.FieldName = "JournalId";
            this.colDetailJournalId.MinWidth = 25;
            this.colDetailJournalId.Name = "colDetailJournalId";
            this.colDetailJournalId.Width = 72;
            // 
            // colDetailLine
            // 
            this.colDetailLine.Caption = "Line";
            this.colDetailLine.FieldName = "Line";
            this.colDetailLine.MinWidth = 25;
            this.colDetailLine.Name = "colDetailLine";
            this.colDetailLine.Visible = true;
            this.colDetailLine.VisibleIndex = 0;
            this.colDetailLine.Width = 58;
            // 
            // colDetailChartId
            // 
            this.colDetailChartId.Caption = "Account";
            this.colDetailChartId.ColumnEdit = this.repCharts;
            this.colDetailChartId.FieldName = "ChartId";
            this.colDetailChartId.MinWidth = 25;
            this.colDetailChartId.Name = "colDetailChartId";
            this.colDetailChartId.Visible = true;
            this.colDetailChartId.VisibleIndex = 1;
            this.colDetailChartId.Width = 168;
            // 
            // repCharts
            // 
            this.repCharts.AutoHeight = false;
            this.repCharts.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repCharts.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Id", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("AccountNumber", "AccountNumber", 60, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default)});
            this.repCharts.DisplayMember = "AccountNumber";
            this.repCharts.Name = "repCharts";
            this.repCharts.NullText = "";
            this.repCharts.ValueMember = "Id";
            // 
            // colDetailAccountName
            // 
            this.colDetailAccountName.Caption = "Account Name";
            this.colDetailAccountName.ColumnEdit = this.repChartList;
            this.colDetailAccountName.FieldName = "ChartId";
            this.colDetailAccountName.MinWidth = 25;
            this.colDetailAccountName.Name = "colDetailAccountName";
            this.colDetailAccountName.Visible = true;
            this.colDetailAccountName.VisibleIndex = 2;
            this.colDetailAccountName.Width = 304;
            // 
            // repChartList
            // 
            this.repChartList.AutoHeight = false;
            this.repChartList.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repChartList.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Id", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("AccountName", "Name", 60, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default)});
            this.repChartList.DisplayMember = "AccountName";
            this.repChartList.Name = "repChartList";
            this.repChartList.NullText = "";
            this.repChartList.ValueMember = "Id";
            // 
            // colDetailValueDate
            // 
            this.colDetailValueDate.Caption = "Value Date";
            this.colDetailValueDate.ColumnEdit = this.repDateFormat;
            this.colDetailValueDate.FieldName = "ValueDate";
            this.colDetailValueDate.MinWidth = 25;
            this.colDetailValueDate.Name = "colDetailValueDate";
            this.colDetailValueDate.Visible = true;
            this.colDetailValueDate.VisibleIndex = 3;
            this.colDetailValueDate.Width = 104;
            // 
            // repDateFormat
            // 
            this.repDateFormat.AutoHeight = false;
            this.repDateFormat.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repDateFormat.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repDateFormat.CalendarTimeProperties.MaskSettings.Set("mask", "dd/MM/yyyy");
            this.repDateFormat.CalendarTimeProperties.UseMaskAsDisplayFormat = true;
            this.repDateFormat.MaskSettings.Set("mask", "dd/MM/yyyy");
            this.repDateFormat.Name = "repDateFormat";
            this.repDateFormat.UseMaskAsDisplayFormat = true;
            // 
            // colDetailCurrencyId
            // 
            this.colDetailCurrencyId.Caption = "Currency";
            this.colDetailCurrencyId.ColumnEdit = this.repCurrencies;
            this.colDetailCurrencyId.FieldName = "CurrencyId";
            this.colDetailCurrencyId.MinWidth = 25;
            this.colDetailCurrencyId.Name = "colDetailCurrencyId";
            this.colDetailCurrencyId.Visible = true;
            this.colDetailCurrencyId.VisibleIndex = 4;
            this.colDetailCurrencyId.Width = 71;
            // 
            // repCurrencies
            // 
            this.repCurrencies.AutoHeight = false;
            this.repCurrencies.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repCurrencies.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Id", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CurrencyCode", "Currency", 60, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default)});
            this.repCurrencies.DisplayMember = "CurrencyCode";
            this.repCurrencies.Name = "repCurrencies";
            this.repCurrencies.NullText = "";
            this.repCurrencies.ValueMember = "Id";
            // 
            // colDetailRate
            // 
            this.colDetailRate.Caption = "Rate";
            this.colDetailRate.DisplayFormat.FormatString = "{0:n}";
            this.colDetailRate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colDetailRate.FieldName = "Rate";
            this.colDetailRate.MinWidth = 25;
            this.colDetailRate.Name = "colDetailRate";
            this.colDetailRate.Visible = true;
            this.colDetailRate.VisibleIndex = 5;
            this.colDetailRate.Width = 55;
            // 
            // colDetailDescription
            // 
            this.colDetailDescription.Caption = "Description";
            this.colDetailDescription.FieldName = "Description";
            this.colDetailDescription.MinWidth = 25;
            this.colDetailDescription.Name = "colDetailDescription";
            this.colDetailDescription.Visible = true;
            this.colDetailDescription.VisibleIndex = 6;
            this.colDetailDescription.Width = 308;
            // 
            // colDetailDbCr
            // 
            this.colDetailDbCr.Caption = "D/C";
            this.colDetailDbCr.FieldName = "DbCr";
            this.colDetailDbCr.MinWidth = 25;
            this.colDetailDbCr.Name = "colDetailDbCr";
            this.colDetailDbCr.Visible = true;
            this.colDetailDbCr.VisibleIndex = 7;
            this.colDetailDbCr.Width = 54;
            // 
            // colDetailAmount
            // 
            this.colDetailAmount.Caption = "Amount";
            this.colDetailAmount.DisplayFormat.FormatString = "{0:n}";
            this.colDetailAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colDetailAmount.FieldName = "Amount";
            this.colDetailAmount.MinWidth = 25;
            this.colDetailAmount.Name = "colDetailAmount";
            this.colDetailAmount.Visible = true;
            this.colDetailAmount.VisibleIndex = 8;
            this.colDetailAmount.Width = 122;
            // 
            // colDetailLAmount
            // 
            this.colDetailLAmount.Caption = "Amount LL";
            this.colDetailLAmount.DisplayFormat.FormatString = "{0:n}";
            this.colDetailLAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colDetailLAmount.FieldName = "LAmount";
            this.colDetailLAmount.MinWidth = 25;
            this.colDetailLAmount.Name = "colDetailLAmount";
            this.colDetailLAmount.Visible = true;
            this.colDetailLAmount.VisibleIndex = 9;
            this.colDetailLAmount.Width = 115;
            // 
            // colDetailFAmount
            // 
            this.colDetailFAmount.Caption = "Amount USD";
            this.colDetailFAmount.DisplayFormat.FormatString = "{0:n}";
            this.colDetailFAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colDetailFAmount.FieldName = "FAmount";
            this.colDetailFAmount.MinWidth = 25;
            this.colDetailFAmount.Name = "colDetailFAmount";
            this.colDetailFAmount.Visible = true;
            this.colDetailFAmount.VisibleIndex = 10;
            this.colDetailFAmount.Width = 127;
            // 
            // colDetailPosted
            // 
            this.colDetailPosted.Caption = "Posted";
            this.colDetailPosted.ColumnEdit = this.CheckBoxFormat;
            this.colDetailPosted.FieldName = "Posted";
            this.colDetailPosted.MinWidth = 25;
            this.colDetailPosted.Name = "colDetailPosted";
            this.colDetailPosted.Visible = true;
            this.colDetailPosted.VisibleIndex = 11;
            this.colDetailPosted.Width = 63;
            // 
            // CheckBoxFormat
            // 
            this.CheckBoxFormat.AutoHeight = false;
            this.CheckBoxFormat.Name = "CheckBoxFormat";
            // 
            // colDetailDocumentRef
            // 
            this.colDetailDocumentRef.Caption = "Document#";
            this.colDetailDocumentRef.FieldName = "DocumentRef";
            this.colDetailDocumentRef.MinWidth = 25;
            this.colDetailDocumentRef.Name = "colDetailDocumentRef";
            this.colDetailDocumentRef.Visible = true;
            this.colDetailDocumentRef.VisibleIndex = 12;
            this.colDetailDocumentRef.Width = 88;
            // 
            // colDetailJobNo
            // 
            this.colDetailJobNo.Caption = "Job No";
            this.colDetailJobNo.FieldName = "JobNo";
            this.colDetailJobNo.MinWidth = 25;
            this.colDetailJobNo.Name = "colDetailJobNo";
            this.colDetailJobNo.Visible = true;
            this.colDetailJobNo.VisibleIndex = 13;
            this.colDetailJobNo.Width = 99;
            // 
            // colDetailDepartmentId
            // 
            this.colDetailDepartmentId.Caption = "Department";
            this.colDetailDepartmentId.ColumnEdit = this.repDepartments;
            this.colDetailDepartmentId.FieldName = "DepartmentId";
            this.colDetailDepartmentId.MinWidth = 25;
            this.colDetailDepartmentId.Name = "colDetailDepartmentId";
            this.colDetailDepartmentId.Visible = true;
            this.colDetailDepartmentId.VisibleIndex = 14;
            this.colDetailDepartmentId.Width = 159;
            // 
            // repDepartments
            // 
            this.repDepartments.AutoHeight = false;
            this.repDepartments.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repDepartments.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Id", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("DepartmentName", "Name", 60, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default)});
            this.repDepartments.DisplayMember = "DepartmentName";
            this.repDepartments.Name = "repDepartments";
            this.repDepartments.NullText = "";
            this.repDepartments.ValueMember = "Id";
            // 
            // colDetailCostCenterId
            // 
            this.colDetailCostCenterId.Caption = "Cost Center";
            this.colDetailCostCenterId.ColumnEdit = this.repCostCenters;
            this.colDetailCostCenterId.FieldName = "CostCenterId";
            this.colDetailCostCenterId.MinWidth = 25;
            this.colDetailCostCenterId.Name = "colDetailCostCenterId";
            this.colDetailCostCenterId.Visible = true;
            this.colDetailCostCenterId.VisibleIndex = 15;
            this.colDetailCostCenterId.Width = 101;
            // 
            // repCostCenters
            // 
            this.repCostCenters.AutoHeight = false;
            this.repCostCenters.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repCostCenters.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Id", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Name", "Name", 60, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default)});
            this.repCostCenters.DisplayMember = "Name";
            this.repCostCenters.Name = "repCostCenters";
            this.repCostCenters.NullText = "";
            this.repCostCenters.ValueMember = "Id";
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "Flow Type";
            this.gridColumn6.ColumnEdit = this.repFlowTypes;
            this.gridColumn6.FieldName = "FlowTypeId";
            this.gridColumn6.MinWidth = 25;
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 16;
            this.gridColumn6.Width = 93;
            // 
            // repFlowTypes
            // 
            this.repFlowTypes.AutoHeight = false;
            this.repFlowTypes.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repFlowTypes.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Id", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Code", "Code", 60, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default)});
            this.repFlowTypes.DisplayMember = "Code";
            this.repFlowTypes.Name = "repFlowTypes";
            this.repFlowTypes.NullText = "";
            this.repFlowTypes.ValueMember = "Id";
            // 
            // colDetailNotes
            // 
            this.colDetailNotes.Caption = "Notes";
            this.colDetailNotes.FieldName = "Notes";
            this.colDetailNotes.MinWidth = 25;
            this.colDetailNotes.Name = "colDetailNotes";
            this.colDetailNotes.Visible = true;
            this.colDetailNotes.VisibleIndex = 17;
            this.colDetailNotes.Width = 154;
            // 
            // colDetailWorkingYear
            // 
            this.colDetailWorkingYear.Caption = "Working Year";
            this.colDetailWorkingYear.FieldName = "WorkingYear";
            this.colDetailWorkingYear.MinWidth = 25;
            this.colDetailWorkingYear.Name = "colDetailWorkingYear";
            this.colDetailWorkingYear.Visible = true;
            this.colDetailWorkingYear.VisibleIndex = 18;
            this.colDetailWorkingYear.Width = 169;
            // 
            // colDetailLocked
            // 
            this.colDetailLocked.Caption = "Locked";
            this.colDetailLocked.ColumnEdit = this.CheckBoxFormat;
            this.colDetailLocked.FieldName = "Locked";
            this.colDetailLocked.MinWidth = 25;
            this.colDetailLocked.Name = "colDetailLocked";
            this.colDetailLocked.Visible = true;
            this.colDetailLocked.VisibleIndex = 19;
            this.colDetailLocked.Width = 246;
            // 
            // gcJournals
            // 
            this.gcJournals.DataSource = this.bsJournals;
            this.gcJournals.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcJournals.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcJournals.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcJournals.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcJournals.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcJournals.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            gridLevelNode1.LevelTemplate = this.gvJournalDetails;
            gridLevelNode1.RelationName = "JournalDetails";
            this.gcJournals.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gcJournals.Location = new System.Drawing.Point(14, 14);
            this.gcJournals.MainView = this.gvJournals;
            this.gcJournals.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcJournals.Name = "gcJournals";
            this.gcJournals.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repJournalTypes,
            this.repDateFormat,
            this.repCurrencies,
            this.CheckBoxFormat,
            this.repCharts,
            this.repDepartments,
            this.repCostCenters,
            this.repFlowTypes,
            this.repChartList});
            this.gcJournals.Size = new System.Drawing.Size(1351, 511);
            this.gcJournals.TabIndex = 5;
            this.gcJournals.UseEmbeddedNavigator = true;
            this.gcJournals.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvJournals,
            this.gvJournalDetails});
            // 
            // bsJournals
            // 
            this.bsJournals.DataSource = typeof(Spectrum.Models.Accounting.Journals.JournalModel);
            // 
            // gvJournals
            // 
            this.gvJournals.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colId,
            this.colJvNo,
            this.colJournalTypeId,
            this.colJournalDate,
            this.colReference,
            this.colCurrencyId,
            this.colRate,
            this.colIsPosted,
            this.colNotes,
            this.colWorkingYear,
            this.colIsProtected,
            this.colLocked});
            this.gvJournals.DetailHeight = 458;
            this.gvJournals.GridControl = this.gcJournals;
            this.gvJournals.GroupSummary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Count, "Name", null, "")});
            this.gvJournals.Name = "gvJournals";
            this.gvJournals.OptionsBehavior.Editable = false;
            this.gvJournals.OptionsBehavior.ReadOnly = true;
            this.gvJournals.OptionsFind.AlwaysVisible = true;
            this.gvJournals.OptionsPrint.AutoWidth = false;
            this.gvJournals.OptionsPrint.PrintHorzLines = false;
            this.gvJournals.OptionsPrint.PrintVertLines = false;
            this.gvJournals.OptionsView.ColumnAutoWidth = false;
            this.gvJournals.OptionsView.ShowGroupedColumns = true;
            this.gvJournals.OptionsView.ShowGroupPanel = false;
            this.gvJournals.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gvJournals_RowCellStyle);
            this.gvJournals.DoubleClick += new System.EventHandler(this.gvJournals_DoubleClick);
            // 
            // colId
            // 
            this.colId.FieldName = "Id";
            this.colId.MinWidth = 25;
            this.colId.Name = "colId";
            this.colId.Width = 94;
            // 
            // colJvNo
            // 
            this.colJvNo.FieldName = "JvNo";
            this.colJvNo.MinWidth = 25;
            this.colJvNo.Name = "colJvNo";
            this.colJvNo.Visible = true;
            this.colJvNo.VisibleIndex = 0;
            this.colJvNo.Width = 94;
            // 
            // colJournalTypeId
            // 
            this.colJournalTypeId.Caption = "JV Type";
            this.colJournalTypeId.ColumnEdit = this.repJournalTypes;
            this.colJournalTypeId.FieldName = "JournalTypeId";
            this.colJournalTypeId.MinWidth = 25;
            this.colJournalTypeId.Name = "colJournalTypeId";
            this.colJournalTypeId.Visible = true;
            this.colJournalTypeId.VisibleIndex = 1;
            this.colJournalTypeId.Width = 108;
            // 
            // repJournalTypes
            // 
            this.repJournalTypes.AutoHeight = false;
            this.repJournalTypes.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repJournalTypes.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Id", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Code", "Code", 60, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default)});
            this.repJournalTypes.DisplayMember = "Code";
            this.repJournalTypes.Name = "repJournalTypes";
            this.repJournalTypes.NullText = "";
            this.repJournalTypes.ValueMember = "Id";
            // 
            // colJournalDate
            // 
            this.colJournalDate.Caption = "Date";
            this.colJournalDate.ColumnEdit = this.repDateFormat;
            this.colJournalDate.FieldName = "JournalDate";
            this.colJournalDate.MinWidth = 25;
            this.colJournalDate.Name = "colJournalDate";
            this.colJournalDate.Visible = true;
            this.colJournalDate.VisibleIndex = 2;
            this.colJournalDate.Width = 123;
            // 
            // colReference
            // 
            this.colReference.FieldName = "Reference";
            this.colReference.MinWidth = 25;
            this.colReference.Name = "colReference";
            this.colReference.Visible = true;
            this.colReference.VisibleIndex = 3;
            this.colReference.Width = 132;
            // 
            // colCurrencyId
            // 
            this.colCurrencyId.Caption = "Currency";
            this.colCurrencyId.ColumnEdit = this.repCurrencies;
            this.colCurrencyId.FieldName = "CurrencyId";
            this.colCurrencyId.MinWidth = 25;
            this.colCurrencyId.Name = "colCurrencyId";
            this.colCurrencyId.Visible = true;
            this.colCurrencyId.VisibleIndex = 4;
            this.colCurrencyId.Width = 101;
            // 
            // colRate
            // 
            this.colRate.DisplayFormat.FormatString = "{0:n}";
            this.colRate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colRate.FieldName = "Rate";
            this.colRate.MinWidth = 25;
            this.colRate.Name = "colRate";
            this.colRate.Visible = true;
            this.colRate.VisibleIndex = 5;
            this.colRate.Width = 94;
            // 
            // colIsPosted
            // 
            this.colIsPosted.Caption = "Posted";
            this.colIsPosted.ColumnEdit = this.CheckBoxFormat;
            this.colIsPosted.FieldName = "IsPosted";
            this.colIsPosted.MinWidth = 25;
            this.colIsPosted.Name = "colIsPosted";
            this.colIsPosted.Visible = true;
            this.colIsPosted.VisibleIndex = 6;
            this.colIsPosted.Width = 94;
            // 
            // colNotes
            // 
            this.colNotes.FieldName = "Notes";
            this.colNotes.MinWidth = 25;
            this.colNotes.Name = "colNotes";
            this.colNotes.Visible = true;
            this.colNotes.VisibleIndex = 7;
            this.colNotes.Width = 182;
            // 
            // colWorkingYear
            // 
            this.colWorkingYear.FieldName = "WorkingYear";
            this.colWorkingYear.MinWidth = 25;
            this.colWorkingYear.Name = "colWorkingYear";
            this.colWorkingYear.Visible = true;
            this.colWorkingYear.VisibleIndex = 8;
            this.colWorkingYear.Width = 112;
            // 
            // colIsProtected
            // 
            this.colIsProtected.Caption = "Protected";
            this.colIsProtected.ColumnEdit = this.CheckBoxFormat;
            this.colIsProtected.FieldName = "IsProtected";
            this.colIsProtected.MinWidth = 25;
            this.colIsProtected.Name = "colIsProtected";
            this.colIsProtected.Visible = true;
            this.colIsProtected.VisibleIndex = 9;
            this.colIsProtected.Width = 94;
            // 
            // colLocked
            // 
            this.colLocked.ColumnEdit = this.CheckBoxFormat;
            this.colLocked.FieldName = "Locked";
            this.colLocked.MinWidth = 25;
            this.colLocked.Name = "colLocked";
            this.colLocked.Visible = true;
            this.colLocked.VisibleIndex = 10;
            this.colLocked.Width = 94;
            // 
            // rcJournals
            // 
            this.rcJournals.EmptyAreaImageOptions.ImagePadding = new System.Windows.Forms.Padding(35, 39, 35, 39);
            this.rcJournals.ExpandCollapseItem.Id = 0;
            this.rcJournals.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.rcJournals.ExpandCollapseItem,
            this.btnNew,
            this.btnDelete,
            this.btnEdit,
            this.btnPrint,
            this.barButtonItem11,
            this.barButtonItem12,
            this.btnRefresh,
            this.btnClose,
            this.btnPrintFilter,
            this.btnResetGridStyle,
            this.btnPrintStatement,
            this.dtFrom,
            this.dtTo,
            this.cboWorkingYear});
            this.rcJournals.Location = new System.Drawing.Point(0, 0);
            this.rcJournals.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rcJournals.MaxItemId = 67;
            this.rcJournals.MdiMergeStyle = DevExpress.XtraBars.Ribbon.RibbonMdiMergeStyle.Always;
            this.rcJournals.Name = "rcJournals";
            this.rcJournals.OptionsMenuMinWidth = 385;
            this.rcJournals.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.rpJournals,
            this.rpViewSettings});
            this.rcJournals.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repWorkingYears,
            this.repWorkingYear});
            this.rcJournals.Size = new System.Drawing.Size(1379, 193);
            this.rcJournals.StatusBar = this.ribbonStatusBar1;
            // 
            // btnNew
            // 
            this.btnNew.Caption = "Add New";
            this.btnNew.Enabled = false;
            this.btnNew.Id = 1;
            this.btnNew.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.False;
            this.btnNew.ImageOptions.ImageUri.Uri = "resource://DevExpress.DevAV.Resources.NewCustomer.svg";
            this.btnNew.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnNew.ImageOptions.SvgImage")));
            this.btnNew.Name = "btnNew";
            this.btnNew.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnNew_ItemClick);
            // 
            // btnDelete
            // 
            this.btnDelete.Caption = "Delete";
            this.btnDelete.Enabled = false;
            this.btnDelete.Id = 3;
            this.btnDelete.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.False;
            this.btnDelete.ImageOptions.ImageUri.Uri = "resource://DevExpress.DevAV.Resources.Delete.svg";
            this.btnDelete.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnDelete.ImageOptions.SvgImage")));
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnDelete_ItemClick);
            // 
            // btnEdit
            // 
            this.btnEdit.Caption = "Edit";
            this.btnEdit.Enabled = false;
            this.btnEdit.Id = 13;
            this.btnEdit.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.False;
            this.btnEdit.ImageOptions.ImageUri.Uri = "resource://DevExpress.DevAV.Resources.Edit.svg";
            this.btnEdit.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnEdit.ImageOptions.SvgImage")));
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnEdit_ItemClick);
            // 
            // btnPrint
            // 
            this.btnPrint.Caption = "Print View";
            this.btnPrint.Enabled = false;
            this.btnPrint.Id = 16;
            this.btnPrint.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.False;
            this.btnPrint.ImageOptions.ImageUri.Uri = "resource://DevExpress.DevAV.Resources.SalesAnalysis.svg";
            this.btnPrint.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnPrint.ImageOptions.SvgImage")));
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnPrint_ItemClick);
            // 
            // barButtonItem11
            // 
            this.barButtonItem11.Caption = "View Settings";
            this.barButtonItem11.Id = 22;
            this.barButtonItem11.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.False;
            this.barButtonItem11.ImageOptions.ImageUri.Uri = "resource://DevExpress.DevAV.Resources.ViewSetting.svg";
            this.barButtonItem11.Name = "barButtonItem11";
            // 
            // barButtonItem12
            // 
            this.barButtonItem12.Caption = "Reset View";
            this.barButtonItem12.Id = 23;
            this.barButtonItem12.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.False;
            this.barButtonItem12.ImageOptions.ImageUri.Uri = "resource://DevExpress.DevAV.Resources.ResetView.svg";
            this.barButtonItem12.Name = "barButtonItem12";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Caption = "Refresh";
            this.btnRefresh.Id = 37;
            this.btnRefresh.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnRefresh.ImageOptions.SvgImage")));
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnRefresh_ItemClick);
            // 
            // btnClose
            // 
            this.btnClose.Caption = "Close";
            this.btnClose.Id = 38;
            this.btnClose.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnClose.ImageOptions.SvgImage")));
            this.btnClose.Name = "btnClose";
            this.btnClose.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnClose_ItemClick);
            // 
            // btnPrintFilter
            // 
            this.btnPrintFilter.Caption = "Print Filter";
            this.btnPrintFilter.Enabled = false;
            this.btnPrintFilter.Id = 58;
            this.btnPrintFilter.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnPrintFilter.ImageOptions.SvgImage")));
            this.btnPrintFilter.Name = "btnPrintFilter";
            this.btnPrintFilter.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnPrintFilter_ItemClick);
            // 
            // btnResetGridStyle
            // 
            this.btnResetGridStyle.Caption = "Reset Grid Style";
            this.btnResetGridStyle.Id = 61;
            this.btnResetGridStyle.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnResetGridStyle.ImageOptions.SvgImage")));
            this.btnResetGridStyle.Name = "btnResetGridStyle";
            this.btnResetGridStyle.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnResetGridStyle_ItemClick);
            // 
            // btnPrintStatement
            // 
            this.btnPrintStatement.Caption = "Print Statement";
            this.btnPrintStatement.Id = 62;
            this.btnPrintStatement.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnPrintStatement.ImageOptions.SvgImage")));
            this.btnPrintStatement.Name = "btnPrintStatement";
            this.btnPrintStatement.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnPrintStatement_ItemClick);
            // 
            // dtFrom
            // 
            this.dtFrom.Caption = "Date From";
            this.dtFrom.CaptionToEditorIndent = 10;
            this.dtFrom.Edit = null;
            this.dtFrom.EditWidth = 120;
            this.dtFrom.Id = 63;
            this.dtFrom.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("dtFrom.ImageOptions.SvgImage")));
            this.dtFrom.Name = "dtFrom";
            // 
            // dtTo
            // 
            this.dtTo.Caption = "Date To";
            this.dtTo.CaptionToEditorIndent = 22;
            this.dtTo.Edit = null;
            this.dtTo.EditWidth = 120;
            this.dtTo.Id = 64;
            this.dtTo.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("dtTo.ImageOptions.SvgImage")));
            this.dtTo.Name = "dtTo";
            // 
            // cboWorkingYear
            // 
            this.cboWorkingYear.Caption = "Working Year       ";
            this.cboWorkingYear.CaptionAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.cboWorkingYear.Edit = this.repWorkingYear;
            this.cboWorkingYear.EditWidth = 100;
            this.cboWorkingYear.Id = 66;
            this.cboWorkingYear.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("cboWorkingYear.ImageOptions.SvgImage")));
            this.cboWorkingYear.Name = "cboWorkingYear";
            this.cboWorkingYear.EditValueChanged += new System.EventHandler(this.cboWorkingYear_EditValueChanged);
            // 
            // repWorkingYear
            // 
            this.repWorkingYear.AutoHeight = false;
            this.repWorkingYear.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repWorkingYear.Name = "repWorkingYear";
            this.repWorkingYear.NullText = "";
            // 
            // rpJournals
            // 
            this.rpJournals.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1,
            this.ribbonPageGroup6,
            this.ribbonPageGroup8,
            this.rpgDepartments,
            this.ribbonPageGroup11,
            this.ribbonPageGroup2});
            this.rpJournals.Name = "rpJournals";
            this.rpJournals.Text = "JOURNALS";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.AllowTextClipping = false;
            this.ribbonPageGroup1.CaptionButtonVisible = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonPageGroup1.ItemLinks.Add(this.btnNew);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "New";
            // 
            // ribbonPageGroup6
            // 
            this.ribbonPageGroup6.AllowTextClipping = false;
            this.ribbonPageGroup6.CaptionButtonVisible = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonPageGroup6.ItemLinks.Add(this.btnEdit);
            this.ribbonPageGroup6.ItemLinks.Add(this.btnRefresh);
            this.ribbonPageGroup6.ItemLinks.Add(this.btnPrint);
            this.ribbonPageGroup6.ItemLinks.Add(this.btnPrintFilter, true);
            this.ribbonPageGroup6.Name = "ribbonPageGroup6";
            this.ribbonPageGroup6.Text = "Actions";
            // 
            // ribbonPageGroup8
            // 
            this.ribbonPageGroup8.AllowTextClipping = false;
            this.ribbonPageGroup8.CaptionButtonVisible = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonPageGroup8.ItemLinks.Add(this.btnDelete);
            this.ribbonPageGroup8.Name = "ribbonPageGroup8";
            this.ribbonPageGroup8.Text = "Delete";
            // 
            // rpgDepartments
            // 
            this.rpgDepartments.AllowTextClipping = false;
            this.rpgDepartments.CaptionButtonVisible = DevExpress.Utils.DefaultBoolean.False;
            this.rpgDepartments.ItemLinks.Add(this.cboWorkingYear);
            this.rpgDepartments.Name = "rpgDepartments";
            this.rpgDepartments.Text = "Current Period";
            // 
            // ribbonPageGroup11
            // 
            this.ribbonPageGroup11.Alignment = DevExpress.XtraBars.Ribbon.RibbonPageGroupAlignment.Far;
            this.ribbonPageGroup11.ItemLinks.Add(this.btnClose);
            this.ribbonPageGroup11.Name = "ribbonPageGroup11";
            this.ribbonPageGroup11.Text = "Close View";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.dtFrom);
            this.ribbonPageGroup2.ItemLinks.Add(this.dtTo);
            this.ribbonPageGroup2.ItemLinks.Add(this.btnPrintStatement, true);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "ribbonPageGroup2";
            // 
            // rpViewSettings
            // 
            this.rpViewSettings.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup3});
            this.rpViewSettings.Name = "rpViewSettings";
            this.rpViewSettings.Text = "VIEW SETTINGS";
            // 
            // ribbonPageGroup3
            // 
            this.ribbonPageGroup3.ItemLinks.Add(this.btnResetGridStyle);
            this.ribbonPageGroup3.Name = "ribbonPageGroup3";
            this.ribbonPageGroup3.Text = "Grid Settings";
            // 
            // repWorkingYears
            // 
            this.repWorkingYears.AutoHeight = false;
            this.repWorkingYears.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repWorkingYears.Name = "repWorkingYears";
            this.repWorkingYears.NullText = "";
            this.repWorkingYears.PopupView = this.repositoryItemSearchLookUpEdit1View;
            // 
            // repositoryItemSearchLookUpEdit1View
            // 
            this.repositoryItemSearchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.repositoryItemSearchLookUpEdit1View.Name = "repositoryItemSearchLookUpEdit1View";
            this.repositoryItemSearchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.repositoryItemSearchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // ribbonStatusBar1
            // 
            this.ribbonStatusBar1.Location = new System.Drawing.Point(0, 732);
            this.ribbonStatusBar1.Name = "ribbonStatusBar1";
            this.ribbonStatusBar1.Ribbon = this.rcJournals;
            this.ribbonStatusBar1.Size = new System.Drawing.Size(1379, 30);
            // 
            // mainLayout
            // 
            this.mainLayout.Controls.Add(this.gcJournals);
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.Location = new System.Drawing.Point(0, 193);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.Root = this.Root;
            this.mainLayout.Size = new System.Drawing.Size(1379, 539);
            this.mainLayout.TabIndex = 15;
            this.mainLayout.Text = "layoutControl1";
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(1379, 539);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.gcJournals;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1355, 515);
            this.layoutControlItem1.TextVisible = false;
            // 
            // JournalsListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1379, 762);
            this.Controls.Add(this.mainLayout);
            this.Controls.Add(this.rcJournals);
            this.Controls.Add(this.ribbonStatusBar1);
            this.Name = "JournalsListForm";
            this.Ribbon = this.rcJournals;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.StatusBar = this.ribbonStatusBar1;
            this.Text = "Journals List";
            ((System.ComponentModel.ISupportInitialize)(this.gvJournalDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repCharts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repChartList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repDateFormat.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repDateFormat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repCurrencies)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CheckBoxFormat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repDepartments)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repCostCenters)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repFlowTypes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcJournals)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsJournals)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvJournals)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repJournalTypes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rcJournals)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repWorkingYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repWorkingYears)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainLayout)).EndInit();
            this.mainLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public DevExpress.XtraBars.Ribbon.RibbonControl rcJournals;
        private DevExpress.XtraBars.BarButtonItem btnNew;
        private DevExpress.XtraBars.BarButtonItem btnDelete;
        private DevExpress.XtraBars.BarButtonItem btnEdit;
        private DevExpress.XtraBars.BarButtonItem btnPrint;
        private DevExpress.XtraBars.BarButtonItem barButtonItem11;
        private DevExpress.XtraBars.BarButtonItem barButtonItem12;
        private DevExpress.XtraBars.BarButtonItem btnRefresh;
        private DevExpress.XtraBars.BarButtonItem btnClose;
        private DevExpress.XtraBars.BarButtonItem btnPrintFilter;
        private DevExpress.XtraBars.BarButtonItem btnResetGridStyle;
        private DevExpress.XtraBars.BarButtonItem btnPrintStatement;
        private DevExpress.XtraBars.BarEditItem dtFrom;
        private DevExpress.XtraBars.BarEditItem dtTo;
        private DevExpress.XtraBars.Ribbon.RibbonPage rpJournals;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup6;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup8;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rpgDepartments;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup11;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.Ribbon.RibbonPage rpViewSettings;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar1;
        private DevExpress.XtraLayout.LayoutControl mainLayout;
        private DevExpress.XtraGrid.GridControl gcJournals;
        private DevExpress.XtraGrid.Views.Grid.GridView gvJournalDetails;
        private DevExpress.XtraGrid.Columns.GridColumn colDetailId;
        private DevExpress.XtraGrid.Columns.GridColumn colDetailJournalId;
        private DevExpress.XtraGrid.Columns.GridColumn colDetailLine;
        private DevExpress.XtraGrid.Columns.GridColumn colDetailChartId;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repCharts;
        private DevExpress.XtraGrid.Columns.GridColumn colDetailAccountName;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repChartList;
        private DevExpress.XtraGrid.Columns.GridColumn colDetailValueDate;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repDateFormat;
        private DevExpress.XtraGrid.Columns.GridColumn colDetailCurrencyId;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repCurrencies;
        private DevExpress.XtraGrid.Columns.GridColumn colDetailRate;
        private DevExpress.XtraGrid.Columns.GridColumn colDetailDescription;
        private DevExpress.XtraGrid.Columns.GridColumn colDetailDbCr;
        private DevExpress.XtraGrid.Columns.GridColumn colDetailAmount;
        private DevExpress.XtraGrid.Columns.GridColumn colDetailLAmount;
        private DevExpress.XtraGrid.Columns.GridColumn colDetailFAmount;
        private DevExpress.XtraGrid.Columns.GridColumn colDetailPosted;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit CheckBoxFormat;
        private DevExpress.XtraGrid.Columns.GridColumn colDetailDocumentRef;
        private DevExpress.XtraGrid.Columns.GridColumn colDetailJobNo;
        private DevExpress.XtraGrid.Columns.GridColumn colDetailDepartmentId;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repDepartments;
        private DevExpress.XtraGrid.Columns.GridColumn colDetailCostCenterId;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repCostCenters;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repFlowTypes;
        private DevExpress.XtraGrid.Columns.GridColumn colDetailNotes;
        private DevExpress.XtraGrid.Columns.GridColumn colDetailWorkingYear;
        private DevExpress.XtraGrid.Columns.GridColumn colDetailLocked;
        private DevExpress.XtraGrid.Views.Grid.GridView gvJournals;
        private DevExpress.XtraGrid.Columns.GridColumn colId;
        private DevExpress.XtraGrid.Columns.GridColumn colJvNo;
        private DevExpress.XtraGrid.Columns.GridColumn colJournalTypeId;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repJournalTypes;
        private DevExpress.XtraGrid.Columns.GridColumn colJournalDate;
        private DevExpress.XtraGrid.Columns.GridColumn colReference;
        private DevExpress.XtraGrid.Columns.GridColumn colCurrencyId;
        private DevExpress.XtraGrid.Columns.GridColumn colRate;
        private DevExpress.XtraGrid.Columns.GridColumn colIsPosted;
        private DevExpress.XtraGrid.Columns.GridColumn colNotes;
        private DevExpress.XtraGrid.Columns.GridColumn colWorkingYear;
        private DevExpress.XtraGrid.Columns.GridColumn colIsProtected;
        private DevExpress.XtraGrid.Columns.GridColumn colLocked;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit repWorkingYears;
        private DevExpress.XtraGrid.Views.Grid.GridView repositoryItemSearchLookUpEdit1View;
        private System.Windows.Forms.BindingSource bsJournals;
        private DevExpress.XtraBars.BarEditItem cboWorkingYear;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repWorkingYear;
    }
}