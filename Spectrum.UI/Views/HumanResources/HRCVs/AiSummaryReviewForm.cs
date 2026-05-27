using DevExpress.XtraEditors;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Spectrum.Views.HumanResources.HRCVs
{
    public class AiSummaryReviewForm : XtraForm
    {
        private readonly RichTextBox _richTextBox;
        private readonly SimpleButton _btnApply;
        private readonly SimpleButton _btnClose;

        public AiSummaryReviewForm(string rtfContent, string plainTextFallback)
        {
            Text = "AI Summary Review";
            StartPosition = FormStartPosition.CenterParent;
            Width = 1000;
            Height = 700;

            _richTextBox = new RichTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                DetectUrls = true,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 10f)
            };

            if (!string.IsNullOrWhiteSpace(rtfContent))
            {
                _richTextBox.Rtf = rtfContent;
            }
            else
            {
                _richTextBox.Text = plainTextFallback ?? string.Empty;
            }

            _btnApply = new SimpleButton
            {
                Text = "Apply",
                Dock = DockStyle.Right,
                Width = 120
            };
            _btnApply.Click += BtnApply_Click;

            _btnClose = new SimpleButton
            {
                Text = "Close",
                Dock = DockStyle.Right,
                Width = 120
            };
            _btnClose.Click += BtnClose_Click;

            var panelBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 48,
                Padding = new Padding(10)
            };
            panelBottom.Controls.Add(_btnClose);
            panelBottom.Controls.Add(_btnApply);

            Controls.Add(_richTextBox);
            Controls.Add(panelBottom);
        }

        private void BtnApply_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
