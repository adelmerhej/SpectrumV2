namespace Spectrum.Views.Common.Services
{
	partial class ServiceTypeEditForm
	{
		private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
				components.Dispose();
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.bsServiceType = new System.Windows.Forms.BindingSource(this.components);
			this.txtType = new DevExpress.XtraEditors.TextEdit();
			this.txtCode = new DevExpress.XtraEditors.TextEdit();
			this.btnSave = new DevExpress.XtraEditors.SimpleButton();
			this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
			this.lblType = new DevExpress.XtraEditors.LabelControl();
			this.lblCode = new DevExpress.XtraEditors.LabelControl();
			((System.ComponentModel.ISupportInitialize)(this.bsServiceType)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtType.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtCode.Properties)).BeginInit();
			this.SuspendLayout();
			// 
			// bsServiceType
			// 
			this.bsServiceType.DataSource = typeof(Spectrum.Models.Common.Services.ServiceTypeModel);
			// 
			// txtType
			// 
			this.txtType.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsServiceType, "ServiceType", true));
			this.txtType.Location = new System.Drawing.Point(120, 18);
			this.txtType.Name = "txtType";
			this.txtType.Size = new System.Drawing.Size(260, 22);
			this.txtType.TabIndex = 0;
			// 
			// txtCode
			// 
			this.txtCode.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsServiceType, "ServiceCode", true));
			this.txtCode.Location = new System.Drawing.Point(120, 52);
			this.txtCode.Name = "txtCode";
			this.txtCode.Size = new System.Drawing.Size(260, 22);
			this.txtCode.TabIndex = 1;
			// 
			// btnSave
			// 
			this.btnSave.Location = new System.Drawing.Point(224, 90);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 27);
			this.btnSave.TabIndex = 2;
			this.btnSave.Text = "Save";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(305, 90);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 27);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// lblType
			// 
			this.lblType.Location = new System.Drawing.Point(18, 21);
			this.lblType.Name = "lblType";
			this.lblType.Size = new System.Drawing.Size(72, 16);
			this.lblType.TabIndex = 4;
			this.lblType.Text = "Service Type";
			// 
			// lblCode
			// 
			this.lblCode.Location = new System.Drawing.Point(18, 55);
			this.lblCode.Name = "lblCode";
			this.lblCode.Size = new System.Drawing.Size(30, 16);
			this.lblCode.TabIndex = 5;
			this.lblCode.Text = "Code";
			// 
			// ServiceTypeEditForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(402, 133);
			this.Controls.Add(this.lblCode);
			this.Controls.Add(this.lblType);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.txtCode);
			this.Controls.Add(this.txtType);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ServiceTypeEditForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Service Type";
			((System.ComponentModel.ISupportInitialize)(this.bsServiceType)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtType.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtCode.Properties)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.BindingSource bsServiceType;
		private DevExpress.XtraEditors.TextEdit txtType;
		private DevExpress.XtraEditors.TextEdit txtCode;
		private DevExpress.XtraEditors.SimpleButton btnSave;
		private DevExpress.XtraEditors.SimpleButton btnCancel;
		private DevExpress.XtraEditors.LabelControl lblType;
		private DevExpress.XtraEditors.LabelControl lblCode;
	}
}
