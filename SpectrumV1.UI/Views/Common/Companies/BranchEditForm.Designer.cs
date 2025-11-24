namespace SpectrumV1.Views.Common.Companies
{
	partial class BranchEditForm
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
			this.cboCompanies = new DevExpress.XtraEditors.SearchLookUpEdit();
			this.bsBranch = new System.Windows.Forms.BindingSource(this.components);
			this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
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
			this.grpBranchInfo = new DevExpress.XtraLayout.LayoutControlGroup();
			this.lblName = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
			this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
			this.emptySpaceItem4 = new DevExpress.XtraLayout.EmptySpaceItem();
			this.lblCompany = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlGroup4 = new DevExpress.XtraLayout.LayoutControlGroup();
			this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
			this.chkIsDefault = new DevExpress.XtraEditors.CheckEdit();
			this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
			((System.ComponentModel.ISupportInitialize)(this.mainLayout)).BeginInit();
			this.mainLayout.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.cboCompanies.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.bsBranch)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.chkActive.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtNotes.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.grpBranchInfo)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.lblName)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.lblCompany)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.chkIsDefault.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
			this.SuspendLayout();
			// 
			// mainLayout
			// 
			this.mainLayout.Controls.Add(this.chkIsDefault);
			this.mainLayout.Controls.Add(this.cboCompanies);
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
			this.mainLayout.Size = new System.Drawing.Size(598, 402);
			this.mainLayout.TabIndex = 10;
			this.mainLayout.Text = "layoutControl1";
			// 
			// cboCompanies
			// 
			this.cboCompanies.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsBranch, "Company", true));
			this.cboCompanies.Location = new System.Drawing.Point(96, 80);
			this.cboCompanies.Name = "cboCompanies";
			this.cboCompanies.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.cboCompanies.Properties.DisplayMember = "CompanyName";
			this.cboCompanies.Properties.NullText = "";
			this.cboCompanies.Properties.PopupView = this.gridView1;
			this.cboCompanies.Properties.ShowAddNewButton = true;
			this.cboCompanies.Properties.ValueMember = "CompanyName";
			this.cboCompanies.Size = new System.Drawing.Size(474, 22);
			this.cboCompanies.StyleController = this.mainLayout;
			this.cboCompanies.TabIndex = 15;
			// 
			// bsBranch
			// 
			this.bsBranch.DataSource = typeof(SpectrumV1.Models.Common.Companies.BranchModel);
			// 
			// gridView1
			// 
			this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2});
			this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
			this.gridView1.Name = "gridView1";
			this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
			this.gridView1.OptionsView.ShowGroupPanel = false;
			// 
			// gridColumn1
			// 
			this.gridColumn1.Caption = "Id";
			this.gridColumn1.FieldName = "_id";
			this.gridColumn1.Name = "gridColumn1";
			this.gridColumn1.Width = 213;
			// 
			// gridColumn2
			// 
			this.gridColumn2.Caption = "Company Name";
			this.gridColumn2.FieldName = "CompanyName";
			this.gridColumn2.Name = "gridColumn2";
			this.gridColumn2.Visible = true;
			this.gridColumn2.VisibleIndex = 0;
			this.gridColumn2.Width = 823;
			// 
			// chkActive
			// 
			this.chkActive.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsBranch, "Active", true));
			this.chkActive.Location = new System.Drawing.Point(28, 144);
			this.chkActive.Name = "chkActive";
			this.chkActive.Properties.Caption = "Active";
			this.chkActive.Size = new System.Drawing.Size(376, 24);
			this.chkActive.StyleController = this.mainLayout;
			this.chkActive.TabIndex = 13;
			// 
			// txtNotes
			// 
			this.txtNotes.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsBranch, "Notes", true));
			this.txtNotes.Location = new System.Drawing.Point(28, 245);
			this.txtNotes.Name = "txtNotes";
			this.txtNotes.Size = new System.Drawing.Size(542, 82);
			this.txtNotes.StyleController = this.mainLayout;
			this.txtNotes.TabIndex = 11;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(463, 361);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(121, 27);
			this.btnCancel.StyleController = this.mainLayout;
			this.btnCancel.TabIndex = 8;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnSave
			// 
			this.btnSave.Enabled = false;
			this.btnSave.Location = new System.Drawing.Point(353, 361);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(106, 27);
			this.btnSave.StyleController = this.mainLayout;
			this.btnSave.TabIndex = 7;
			this.btnSave.Text = "Save";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// txtName
			// 
			this.txtName.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsBranch, "BranchName", true));
			this.txtName.Location = new System.Drawing.Point(96, 54);
			this.txtName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(474, 22);
			this.txtName.StyleController = this.mainLayout;
			this.txtName.TabIndex = 5;
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
            this.grpBranchInfo,
            this.layoutControlGroup4});
			this.layoutControlGroup1.Name = "Root";
			this.layoutControlGroup1.Size = new System.Drawing.Size(598, 402);
			this.layoutControlGroup1.TextVisible = false;
			// 
			// emptySpaceItem1
			// 
			this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
			this.emptySpaceItem1.Location = new System.Drawing.Point(0, 331);
			this.emptySpaceItem1.Name = "emptySpaceItem1";
			this.emptySpaceItem1.Size = new System.Drawing.Size(574, 16);
			// 
			// emptySpaceItem2
			// 
			this.emptySpaceItem2.CustomizationFormText = "emptySpaceItem2";
			this.emptySpaceItem2.Location = new System.Drawing.Point(0, 347);
			this.emptySpaceItem2.Name = "emptySpaceItem2";
			this.emptySpaceItem2.Size = new System.Drawing.Size(339, 31);
			// 
			// layoutControlItem4
			// 
			this.layoutControlItem4.Control = this.btnSave;
			this.layoutControlItem4.Location = new System.Drawing.Point(339, 347);
			this.layoutControlItem4.Name = "layoutControlItem4";
			this.layoutControlItem4.Size = new System.Drawing.Size(110, 31);
			this.layoutControlItem4.TextVisible = false;
			// 
			// layoutControlItem5
			// 
			this.layoutControlItem5.Control = this.btnCancel;
			this.layoutControlItem5.Location = new System.Drawing.Point(449, 347);
			this.layoutControlItem5.Name = "layoutControlItem5";
			this.layoutControlItem5.Size = new System.Drawing.Size(125, 31);
			this.layoutControlItem5.TextVisible = false;
			// 
			// grpBranchInfo
			// 
			this.grpBranchInfo.GroupStyle = DevExpress.Utils.GroupStyle.Light;
			this.grpBranchInfo.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lblName,
            this.layoutControlItem1,
            this.emptySpaceItem3,
            this.emptySpaceItem4,
            this.lblCompany,
            this.layoutControlItem2});
			this.grpBranchInfo.Location = new System.Drawing.Point(0, 0);
			this.grpBranchInfo.Name = "grpBranchInfo";
			this.grpBranchInfo.Size = new System.Drawing.Size(574, 191);
			this.grpBranchInfo.Text = "Branch Info";
			// 
			// lblName
			// 
			this.lblName.Control = this.txtName;
			this.lblName.Location = new System.Drawing.Point(0, 0);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(546, 26);
			this.lblName.Text = "Name";
			this.lblName.TextSize = new System.Drawing.Size(53, 16);
			// 
			// layoutControlItem1
			// 
			this.layoutControlItem1.Control = this.chkActive;
			this.layoutControlItem1.Location = new System.Drawing.Point(0, 90);
			this.layoutControlItem1.Name = "layoutControlItem1";
			this.layoutControlItem1.Size = new System.Drawing.Size(380, 28);
			this.layoutControlItem1.TextVisible = false;
			// 
			// emptySpaceItem3
			// 
			this.emptySpaceItem3.Location = new System.Drawing.Point(0, 118);
			this.emptySpaceItem3.Name = "emptySpaceItem3";
			this.emptySpaceItem3.Size = new System.Drawing.Size(546, 19);
			// 
			// emptySpaceItem4
			// 
			this.emptySpaceItem4.Location = new System.Drawing.Point(0, 52);
			this.emptySpaceItem4.Name = "emptySpaceItem4";
			this.emptySpaceItem4.Size = new System.Drawing.Size(546, 38);
			// 
			// lblCompany
			// 
			this.lblCompany.Control = this.cboCompanies;
			this.lblCompany.Location = new System.Drawing.Point(0, 26);
			this.lblCompany.Name = "lblCompany";
			this.lblCompany.Size = new System.Drawing.Size(546, 26);
			this.lblCompany.Text = "Company";
			this.lblCompany.TextSize = new System.Drawing.Size(53, 16);
			// 
			// layoutControlGroup4
			// 
			this.layoutControlGroup4.GroupStyle = DevExpress.Utils.GroupStyle.Light;
			this.layoutControlGroup4.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem6});
			this.layoutControlGroup4.Location = new System.Drawing.Point(0, 191);
			this.layoutControlGroup4.Name = "layoutControlGroup4";
			this.layoutControlGroup4.Size = new System.Drawing.Size(574, 140);
			this.layoutControlGroup4.Text = "Notes";
			// 
			// layoutControlItem6
			// 
			this.layoutControlItem6.Control = this.txtNotes;
			this.layoutControlItem6.Location = new System.Drawing.Point(0, 0);
			this.layoutControlItem6.Name = "layoutControlItem6";
			this.layoutControlItem6.Size = new System.Drawing.Size(546, 86);
			this.layoutControlItem6.TextVisible = false;
			// 
			// chkIsDefault
			// 
			this.chkIsDefault.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsBranch, "IsDefault", true));
			this.chkIsDefault.Location = new System.Drawing.Point(408, 144);
			this.chkIsDefault.Name = "chkIsDefault";
			this.chkIsDefault.Properties.Caption = "Is Default";
			this.chkIsDefault.Size = new System.Drawing.Size(162, 24);
			this.chkIsDefault.StyleController = this.mainLayout;
			this.chkIsDefault.TabIndex = 16;
			// 
			// layoutControlItem2
			// 
			this.layoutControlItem2.Control = this.chkIsDefault;
			this.layoutControlItem2.Location = new System.Drawing.Point(380, 90);
			this.layoutControlItem2.Name = "layoutControlItem2";
			this.layoutControlItem2.Size = new System.Drawing.Size(166, 28);
			this.layoutControlItem2.TextVisible = false;
			// 
			// BranchEditForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(598, 402);
			this.Controls.Add(this.mainLayout);
			this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Shadow;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "BranchEditForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Edit Branch";
			((System.ComponentModel.ISupportInitialize)(this.mainLayout)).EndInit();
			this.mainLayout.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.cboCompanies.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.bsBranch)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.chkActive.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtNotes.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.grpBranchInfo)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.lblName)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.lblCompany)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.chkIsDefault.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraLayout.LayoutControl mainLayout;
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
		private DevExpress.XtraLayout.LayoutControlGroup grpBranchInfo;
		private DevExpress.XtraLayout.LayoutControlItem lblName;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
		private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup4;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
		private System.Windows.Forms.BindingSource bsBranch;
		private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem3;
		private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem4;
		private DevExpress.XtraEditors.SearchLookUpEdit cboCompanies;
		private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
		private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
		private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
		private DevExpress.XtraLayout.LayoutControlItem lblCompany;
		private DevExpress.XtraEditors.CheckEdit chkIsDefault;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
	}
}