namespace Spectrum.Views.Accounting.CashRegister
{
    partial class CashRegisterEditForm
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
            this.mainLayout = new DevExpress.XtraLayout.LayoutControl();
            this.txtAccountName = new DevExpress.XtraEditors.TextEdit();
            this.bsCashRegister = new System.Windows.Forms.BindingSource(this.components);
            this.cboCharts = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.searchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.chkIsDefault = new DevExpress.XtraEditors.CheckEdit();
            this.chkActive = new DevExpress.XtraEditors.CheckEdit();
            this.txtNotes = new DevExpress.XtraEditors.MemoEdit();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.txtName = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.grpInfo = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lblName = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem4 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem5 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblCharts = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblAccountName = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup4 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.mainLayout)).BeginInit();
            this.mainLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtAccountName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsCashRegister)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboCharts.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsDefault.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkActive.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNotes.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCharts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAccountName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            this.SuspendLayout();
            // 
            // mainLayout
            // 
            this.mainLayout.Controls.Add(this.txtAccountName);
            this.mainLayout.Controls.Add(this.cboCharts);
            this.mainLayout.Controls.Add(this.chkIsDefault);
            this.mainLayout.Controls.Add(this.chkActive);
            this.mainLayout.Controls.Add(this.txtNotes);
            this.mainLayout.Controls.Add(this.btnCancel);
            this.mainLayout.Controls.Add(this.btnSave);
            this.mainLayout.Controls.Add(this.txtName);
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.Location = new System.Drawing.Point(0, 0);
            this.mainLayout.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(738, 385, 450, 350);
            this.mainLayout.Root = this.layoutControlGroup1;
            this.mainLayout.Size = new System.Drawing.Size(548, 383);
            this.mainLayout.TabIndex = 13;
            this.mainLayout.Text = "layoutControl1";
            // 
            // txtAccountName
            // 
            this.txtAccountName.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsCashRegister, "AccountName", true));
            this.txtAccountName.Location = new System.Drawing.Point(131, 106);
            this.txtAccountName.Name = "txtAccountName";
            this.txtAccountName.Properties.ReadOnly = true;
            this.txtAccountName.Size = new System.Drawing.Size(389, 22);
            this.txtAccountName.StyleController = this.mainLayout;
            this.txtAccountName.TabIndex = 8;
            // 
            // bsCashRegister
            // 
            this.bsCashRegister.DataSource = typeof(Spectrum.Models.Accounting.CashRegister.CashRegisterModel);
            // 
            // cboCharts
            // 
            this.cboCharts.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsCashRegister, "AccountNumber", true));
            this.cboCharts.Location = new System.Drawing.Point(131, 80);
            this.cboCharts.Name = "cboCharts";
            this.cboCharts.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboCharts.Properties.DisplayMember = "AccountNumber";
            this.cboCharts.Properties.NullText = "";
            this.cboCharts.Properties.PopupView = this.searchLookUpEdit1View;
            this.cboCharts.Properties.ShowAddNewButton = true;
            this.cboCharts.Properties.ValueMember = "AccountNumber";
            this.cboCharts.Size = new System.Drawing.Size(389, 22);
            this.cboCharts.StyleController = this.mainLayout;
            this.cboCharts.TabIndex = 7;
            this.cboCharts.AddNewValue += new DevExpress.XtraEditors.Controls.AddNewValueEventHandler(this.cboCharts_AddNewValue);
            this.cboCharts.EditValueChanged += new System.EventHandler(this.cboCharts_EditValueChanged);
            // 
            // searchLookUpEdit1View
            // 
            this.searchLookUpEdit1View.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3});
            this.searchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.searchLookUpEdit1View.Name = "searchLookUpEdit1View";
            this.searchLookUpEdit1View.OptionsFind.Condition = DevExpress.Data.Filtering.FilterCondition.StartsWith;
            this.searchLookUpEdit1View.OptionsFind.SearchInPreview = true;
            this.searchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.searchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "Id";
            this.gridColumn1.FieldName = "_id";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Width = 216;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "Account Number";
            this.gridColumn2.FieldName = "AccountNumber";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 0;
            this.gridColumn2.Width = 215;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "Account Name";
            this.gridColumn3.FieldName = "AccountName";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 1;
            this.gridColumn3.Width = 920;
            // 
            // chkIsDefault
            // 
            this.chkIsDefault.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsCashRegister, "IsDefault", true));
            this.chkIsDefault.Location = new System.Drawing.Point(399, 142);
            this.chkIsDefault.Name = "chkIsDefault";
            this.chkIsDefault.Properties.Caption = "Is Default";
            this.chkIsDefault.Size = new System.Drawing.Size(121, 24);
            this.chkIsDefault.StyleController = this.mainLayout;
            this.chkIsDefault.TabIndex = 6;
            // 
            // chkActive
            // 
            this.chkActive.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsCashRegister, "Active", true));
            this.chkActive.Location = new System.Drawing.Point(28, 142);
            this.chkActive.Name = "chkActive";
            this.chkActive.Properties.Caption = "Active";
            this.chkActive.Size = new System.Drawing.Size(244, 24);
            this.chkActive.StyleController = this.mainLayout;
            this.chkActive.TabIndex = 3;
            // 
            // txtNotes
            // 
            this.txtNotes.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsCashRegister, "Notes", true));
            this.txtNotes.Location = new System.Drawing.Point(28, 224);
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(492, 78);
            this.txtNotes.StyleController = this.mainLayout;
            this.txtNotes.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(424, 342);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(110, 27);
            this.btnCancel.StyleController = this.mainLayout;
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(323, 342);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(97, 27);
            this.btnSave.StyleController = this.mainLayout;
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtName
            // 
            this.txtName.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsCashRegister, "Name", true));
            this.txtName.Location = new System.Drawing.Point(131, 54);
            this.txtName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(389, 22);
            this.txtName.StyleController = this.mainLayout;
            this.txtName.TabIndex = 1;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "Root";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.emptySpaceItem1,
            this.emptySpaceItem2,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.grpInfo,
            this.layoutControlGroup4});
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Size = new System.Drawing.Size(548, 383);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 306);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(524, 22);
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.CustomizationFormText = "emptySpaceItem2";
            this.emptySpaceItem2.Location = new System.Drawing.Point(0, 328);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(309, 31);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.btnSave;
            this.layoutControlItem4.Location = new System.Drawing.Point(309, 328);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(101, 31);
            this.layoutControlItem4.TextVisible = false;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.btnCancel;
            this.layoutControlItem5.Location = new System.Drawing.Point(410, 328);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(114, 31);
            this.layoutControlItem5.TextVisible = false;
            // 
            // grpInfo
            // 
            this.grpInfo.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.grpInfo.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lblName,
            this.emptySpaceItem4,
            this.emptySpaceItem5,
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.lblCharts,
            this.lblAccountName});
            this.grpInfo.Location = new System.Drawing.Point(0, 0);
            this.grpInfo.Name = "grpInfo";
            this.grpInfo.Size = new System.Drawing.Size(524, 170);
            this.grpInfo.Text = "Cash Register Info";
            // 
            // lblName
            // 
            this.lblName.Control = this.txtName;
            this.lblName.Location = new System.Drawing.Point(0, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(496, 26);
            this.lblName.Text = "Name";
            this.lblName.TextSize = new System.Drawing.Size(88, 16);
            // 
            // emptySpaceItem4
            // 
            this.emptySpaceItem4.Location = new System.Drawing.Point(248, 88);
            this.emptySpaceItem4.Name = "emptySpaceItem4";
            this.emptySpaceItem4.Size = new System.Drawing.Size(123, 28);
            // 
            // emptySpaceItem5
            // 
            this.emptySpaceItem5.Location = new System.Drawing.Point(0, 78);
            this.emptySpaceItem5.Name = "emptySpaceItem5";
            this.emptySpaceItem5.Size = new System.Drawing.Size(496, 10);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.chkActive;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 88);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(248, 28);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.chkIsDefault;
            this.layoutControlItem2.Location = new System.Drawing.Point(371, 88);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(125, 28);
            this.layoutControlItem2.TextVisible = false;
            // 
            // lblCharts
            // 
            this.lblCharts.Control = this.cboCharts;
            this.lblCharts.Location = new System.Drawing.Point(0, 26);
            this.lblCharts.Name = "lblCharts";
            this.lblCharts.Size = new System.Drawing.Size(496, 26);
            this.lblCharts.Text = "Account Ledger";
            this.lblCharts.TextSize = new System.Drawing.Size(88, 16);
            // 
            // lblAccountName
            // 
            this.lblAccountName.Control = this.txtAccountName;
            this.lblAccountName.Location = new System.Drawing.Point(0, 52);
            this.lblAccountName.Name = "lblAccountName";
            this.lblAccountName.Size = new System.Drawing.Size(496, 26);
            this.lblAccountName.Text = "Account Name";
            this.lblAccountName.TextSize = new System.Drawing.Size(88, 16);
            // 
            // layoutControlGroup4
            // 
            this.layoutControlGroup4.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.layoutControlGroup4.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem6});
            this.layoutControlGroup4.Location = new System.Drawing.Point(0, 170);
            this.layoutControlGroup4.Name = "layoutControlGroup4";
            this.layoutControlGroup4.Size = new System.Drawing.Size(524, 136);
            this.layoutControlGroup4.Text = "Notes";
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.txtNotes;
            this.layoutControlItem6.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(496, 82);
            this.layoutControlItem6.TextVisible = false;
            // 
            // CashRegisterEditForm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(548, 383);
            this.Controls.Add(this.mainLayout);
            this.MaximizeBox = false;
            this.Name = "CashRegisterEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit Cash Register";
            ((System.ComponentModel.ISupportInitialize)(this.mainLayout)).EndInit();
            this.mainLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtAccountName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsCashRegister)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboCharts.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsDefault.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkActive.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNotes.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCharts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAccountName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl mainLayout;
        private DevExpress.XtraEditors.CheckEdit chkIsDefault;
        private DevExpress.XtraEditors.CheckEdit chkActive;
        private DevExpress.XtraEditors.MemoEdit txtNotes;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.TextEdit txtName;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlGroup grpInfo;
        private DevExpress.XtraLayout.LayoutControlItem lblName;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem4;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraEditors.SearchLookUpEdit cboCharts;
        private DevExpress.XtraGrid.Views.Grid.GridView searchLookUpEdit1View;
        private DevExpress.XtraLayout.LayoutControlItem lblCharts;
        private System.Windows.Forms.BindingSource bsCashRegister;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraEditors.TextEdit txtAccountName;
        private DevExpress.XtraLayout.LayoutControlItem lblAccountName;
    }
}