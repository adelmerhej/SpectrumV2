using DevExpress.XtraEditors;
using System;
using System.Drawing;

namespace Spectrum.Views.Common
{
	public partial class FeatureUnderDevelopmentForm : XtraForm
	{
		public FeatureUnderDevelopmentForm()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
			this.lblMessage = new DevExpress.XtraEditors.LabelControl();
			this.lblDescription = new DevExpress.XtraEditors.LabelControl();
			this.btnOk = new DevExpress.XtraEditors.SimpleButton();
			((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureEdit1
			// 
			this.pictureEdit1.Location = new System.Drawing.Point(30, 30);
			this.pictureEdit1.Name = "pictureEdit1";
			this.pictureEdit1.Properties.AllowFocused = false;
			this.pictureEdit1.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
			this.pictureEdit1.Properties.Appearance.Options.UseBackColor = true;
			this.pictureEdit1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
			this.pictureEdit1.Properties.ShowMenu = false;
			this.pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
			this.pictureEdit1.Size = new System.Drawing.Size(64, 64);
			this.pictureEdit1.TabIndex = 0;
			this.pictureEdit1.EditValue = SystemIcons.Information.ToBitmap();
			// 
			// lblMessage
			// 
			this.lblMessage.Appearance.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
			this.lblMessage.Appearance.Options.UseFont = true;
			this.lblMessage.Appearance.Options.UseTextOptions = true;
			this.lblMessage.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
			this.lblMessage.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.lblMessage.Location = new System.Drawing.Point(110, 40);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Size = new System.Drawing.Size(330, 25);
			this.lblMessage.TabIndex = 1;
			this.lblMessage.Text = "Feature Under Development";
			// 
			// lblDescription
			// 
			this.lblDescription.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lblDescription.Appearance.Options.UseFont = true;
			this.lblDescription.Appearance.Options.UseTextOptions = true;
			this.lblDescription.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
			this.lblDescription.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.lblDescription.Location = new System.Drawing.Point(110, 71);
			this.lblDescription.Name = "lblDescription";
			this.lblDescription.Size = new System.Drawing.Size(330, 40);
			this.lblDescription.TabIndex = 2;
			this.lblDescription.Text = "This feature will be implemented soon.\r\nThank you for your patience.";
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(340, 130);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(100, 30);
			this.btnOk.TabIndex = 3;
			this.btnOk.Text = "OK";
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// FeatureUnderDevelopmentForm
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(460, 175);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.lblDescription);
			this.Controls.Add(this.lblMessage);
			this.Controls.Add(this.pictureEdit1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.IconOptions.ShowIcon = false;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FeatureUnderDevelopmentForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Information";
			((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
			this.ResumeLayout(false);

		}

		private DevExpress.XtraEditors.PictureEdit pictureEdit1;
		private DevExpress.XtraEditors.LabelControl lblMessage;
		private DevExpress.XtraEditors.LabelControl lblDescription;
		private DevExpress.XtraEditors.SimpleButton btnOk;

		private void btnOk_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
