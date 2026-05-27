namespace Spectrum.Views.Main
{
    partial class ApiKeySettingForm
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
            this.mainLayout = new DevExpress.XtraLayout.LayoutControl();
            this.cboProvider = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cboModel = new DevExpress.XtraEditors.ComboBoxEdit();
            this.txtAPIKey = new DevExpress.XtraEditors.MemoEdit();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.txtProjectsFolder = new DevExpress.XtraEditors.ButtonEdit();
            this.txtEmployeesFolder = new DevExpress.XtraEditors.ButtonEdit();
            this.txtEngineersFolder = new DevExpress.XtraEditors.ButtonEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.grpInfo = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lblProvider = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblOpenAIModel = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblProjectsFolder = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblEmployeesFolder = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblEngineersFolder = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup4 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.mainLayout)).BeginInit();
            this.mainLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboProvider.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboModel.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAPIKey.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProjectsFolder.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmployeesFolder.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEngineersFolder.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblOpenAIModel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblProjectsFolder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblEmployeesFolder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblEngineersFolder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            this.SuspendLayout();
            // 
            // mainLayout
            // 
            this.mainLayout.Controls.Add(this.cboProvider);
            this.mainLayout.Controls.Add(this.cboModel);
            this.mainLayout.Controls.Add(this.txtAPIKey);
            this.mainLayout.Controls.Add(this.btnCancel);
            this.mainLayout.Controls.Add(this.btnSave);
            this.mainLayout.Controls.Add(this.txtProjectsFolder);
            this.mainLayout.Controls.Add(this.txtEmployeesFolder);
            this.mainLayout.Controls.Add(this.txtEngineersFolder);
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.Location = new System.Drawing.Point(0, 0);
            this.mainLayout.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(738, 385, 450, 350);
            this.mainLayout.Root = this.layoutControlGroup1;
            this.mainLayout.Size = new System.Drawing.Size(734, 396);
            this.mainLayout.TabIndex = 11;
            this.mainLayout.Text = "layoutControl1";
            // 
            // cboProvider
            // 
            this.cboProvider.Location = new System.Drawing.Point(171, 54);
            this.cboProvider.Name = "cboProvider";
            this.cboProvider.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboProvider.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cboProvider.Size = new System.Drawing.Size(535, 22);
            this.cboProvider.StyleController = this.mainLayout;
            this.cboProvider.TabIndex = 1;
            this.cboProvider.SelectedIndexChanged += new System.EventHandler(this.cboProvider_SelectedIndexChanged);
            // 
            // cboModel
            // 
            this.cboModel.Location = new System.Drawing.Point(171, 80);
            this.cboModel.Name = "cboModel";
            this.cboModel.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboModel.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cboModel.Size = new System.Drawing.Size(535, 22);
            this.cboModel.StyleController = this.mainLayout;
            this.cboModel.TabIndex = 2;
            // 
            // txtAPIKey
            // 
            this.txtAPIKey.Location = new System.Drawing.Point(28, 184);
            this.txtAPIKey.Name = "txtAPIKey";
            this.txtAPIKey.Size = new System.Drawing.Size(678, 121);
            this.txtAPIKey.StyleController = this.mainLayout;
            this.txtAPIKey.TabIndex = 6;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(570, 355);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(150, 27);
            this.btnCancel.StyleController = this.mainLayout;
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(433, 355);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(133, 27);
            this.btnSave.StyleController = this.mainLayout;
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtProjectsFolder
            // 
            this.txtProjectsFolder.Location = new System.Drawing.Point(171, 106);
            this.txtProjectsFolder.Name = "txtProjectsFolder";
            this.txtProjectsFolder.Properties.Buttons.Clear();
            this.txtProjectsFolder.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtProjectsFolder.Properties.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.btnBrowseProjects_Click);
            this.txtProjectsFolder.Size = new System.Drawing.Size(535, 22);
            this.txtProjectsFolder.StyleController = this.mainLayout;
            this.txtProjectsFolder.TabIndex = 3;
            // 
            // txtEmployeesFolder
            // 
            this.txtEmployeesFolder.Location = new System.Drawing.Point(171, 132);
            this.txtEmployeesFolder.Name = "txtEmployeesFolder";
            this.txtEmployeesFolder.Properties.Buttons.Clear();
            this.txtEmployeesFolder.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtEmployeesFolder.Properties.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.btnBrowseEmployees_Click);
            this.txtEmployeesFolder.Size = new System.Drawing.Size(535, 22);
            this.txtEmployeesFolder.StyleController = this.mainLayout;
            this.txtEmployeesFolder.TabIndex = 4;
            // 
            // txtEngineersFolder
            // 
            this.txtEngineersFolder.Location = new System.Drawing.Point(171, 158);
            this.txtEngineersFolder.Name = "txtEngineersFolder";
            this.txtEngineersFolder.Properties.Buttons.Clear();
            this.txtEngineersFolder.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtEngineersFolder.Properties.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.btnBrowseEngineers_Click);
            this.txtEngineersFolder.Size = new System.Drawing.Size(535, 22);
            this.txtEngineersFolder.StyleController = this.mainLayout;
            this.txtEngineersFolder.TabIndex = 5;
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
            this.layoutControlGroup1.Size = new System.Drawing.Size(734, 396);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 309);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(710, 32);
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.CustomizationFormText = "emptySpaceItem2";
            this.emptySpaceItem2.Location = new System.Drawing.Point(0, 341);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(419, 31);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.btnSave;
            this.layoutControlItem4.Location = new System.Drawing.Point(419, 341);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(137, 31);
            this.layoutControlItem4.TextVisible = false;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.btnCancel;
            this.layoutControlItem5.Location = new System.Drawing.Point(556, 341);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(154, 31);
            this.layoutControlItem5.TextVisible = false;
            // 
            // grpInfo
            // 
            this.grpInfo.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.grpInfo.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lblProvider,
            this.lblOpenAIModel,
            this.lblProjectsFolder,
            this.lblEmployeesFolder,
            this.lblEngineersFolder});
            this.grpInfo.Location = new System.Drawing.Point(0, 0);
            this.grpInfo.Name = "grpInfo";
            this.grpInfo.Size = new System.Drawing.Size(710, 130);
            this.grpInfo.Text = "Settings";
            // 
            // lblProvider
            // 
            this.lblProvider.Control = this.cboProvider;
            this.lblProvider.Location = new System.Drawing.Point(0, 0);
            this.lblProvider.Name = "lblProvider";
            this.lblProvider.Size = new System.Drawing.Size(682, 26);
            this.lblProvider.Text = "AI Provider";
            this.lblProvider.TextSize = new System.Drawing.Size(131, 16);
            // 
            // lblOpenAIModel
            // 
            this.lblOpenAIModel.Control = this.cboModel;
            this.lblOpenAIModel.Location = new System.Drawing.Point(0, 26);
            this.lblOpenAIModel.Name = "lblOpenAIModel";
            this.lblOpenAIModel.Size = new System.Drawing.Size(682, 26);
            this.lblOpenAIModel.Text = "AI Model";
            this.lblOpenAIModel.TextSize = new System.Drawing.Size(131, 16);
            // 
            // lblProjectsFolder
            // 
            this.lblProjectsFolder.Control = this.txtProjectsFolder;
            this.lblProjectsFolder.Location = new System.Drawing.Point(0, 52);
            this.lblProjectsFolder.Name = "lblProjectsFolder";
            this.lblProjectsFolder.Size = new System.Drawing.Size(682, 26);
            this.lblProjectsFolder.Text = "Projects Folder";
            this.lblProjectsFolder.TextSize = new System.Drawing.Size(131, 16);
            // 
            // lblEmployeesFolder
            // 
            this.lblEmployeesFolder.Control = this.txtEmployeesFolder;
            this.lblEmployeesFolder.Location = new System.Drawing.Point(0, 78);
            this.lblEmployeesFolder.Name = "lblEmployeesFolder";
            this.lblEmployeesFolder.Size = new System.Drawing.Size(682, 26);
            this.lblEmployeesFolder.Text = "Employees Folder";
            this.lblEmployeesFolder.TextSize = new System.Drawing.Size(131, 16);
            // 
            // lblEngineersFolder
            // 
            this.lblEngineersFolder.Control = this.txtEngineersFolder;
            this.lblEngineersFolder.Location = new System.Drawing.Point(0, 104);
            this.lblEngineersFolder.Name = "lblEngineersFolder";
            this.lblEngineersFolder.Size = new System.Drawing.Size(682, 26);
            this.lblEngineersFolder.Text = "Engineers Folder";
            this.lblEngineersFolder.TextSize = new System.Drawing.Size(131, 16);
            // 
            // layoutControlGroup4
            // 
            this.layoutControlGroup4.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.layoutControlGroup4.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem6});
            this.layoutControlGroup4.Location = new System.Drawing.Point(0, 130);
            this.layoutControlGroup4.Name = "layoutControlGroup4";
            this.layoutControlGroup4.Size = new System.Drawing.Size(710, 179);
            this.layoutControlGroup4.Text = "API Key";
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.txtAPIKey;
            this.layoutControlItem6.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(682, 125);
            this.layoutControlItem6.TextVisible = false;
            // 
            // ApiKeySettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 396);
            this.Controls.Add(this.mainLayout);
            this.MaximizeBox = false;
            this.Name = "ApiKeySettingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Api Key Setting";
            ((System.ComponentModel.ISupportInitialize)(this.mainLayout)).EndInit();
            this.mainLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cboProvider.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboModel.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAPIKey.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProjectsFolder.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmployeesFolder.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEngineersFolder.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblOpenAIModel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblProjectsFolder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblEmployeesFolder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblEngineersFolder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl mainLayout;
        private DevExpress.XtraEditors.ComboBoxEdit cboProvider;
        private DevExpress.XtraEditors.ComboBoxEdit cboModel;
        private DevExpress.XtraEditors.MemoEdit txtAPIKey;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.ButtonEdit txtProjectsFolder;
        private DevExpress.XtraEditors.ButtonEdit txtEmployeesFolder;
        private DevExpress.XtraEditors.ButtonEdit txtEngineersFolder;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlGroup grpInfo;
        private DevExpress.XtraLayout.LayoutControlItem lblProvider;
        private DevExpress.XtraLayout.LayoutControlItem lblOpenAIModel;
        private DevExpress.XtraLayout.LayoutControlItem lblProjectsFolder;
        private DevExpress.XtraLayout.LayoutControlItem lblEmployeesFolder;
        private DevExpress.XtraLayout.LayoutControlItem lblEngineersFolder;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
    }
}