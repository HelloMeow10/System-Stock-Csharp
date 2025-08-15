using Presentation.Controles;
using Presentation.Theme;

namespace Presentation
{
    partial class frmError
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblMessage;
        private RoundedTextBox txtMessage;
        private System.Windows.Forms.Label lblExceptionType;
        private RoundedTextBox txtExceptionType;
        private System.Windows.Forms.Label lblStackTrace;
        private RoundedTextBox txtStackTrace;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            lblMessage = new System.Windows.Forms.Label();
            txtMessage = new RoundedTextBox();
            lblExceptionType = new System.Windows.Forms.Label();
            txtExceptionType = new RoundedTextBox();
            lblStackTrace = new System.Windows.Forms.Label();
            txtStackTrace = new RoundedTextBox();
            SuspendLayout();
            // 
            // lblMessage
            // 
            lblMessage.AutoSize = true;
            lblMessage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblMessage.ForeColor = System.Drawing.SystemColors.Control;
            lblMessage.Location = new System.Drawing.Point(16, 23);
            lblMessage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblMessage.Name = "lblMessage";
            lblMessage.Size = new System.Drawing.Size(74, 20);
            lblMessage.TabIndex = 0;
            lblMessage.Text = "Message:";
            // 
            // txtMessage
            // 
            txtMessage.BackColor = System.Drawing.Color.FromArgb(40, 40, 56);
            txtMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtMessage.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtMessage.ForeColor = System.Drawing.Color.White;
            txtMessage.Location = new System.Drawing.Point(147, 18);
            txtMessage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            txtMessage.Multiline = true;
            txtMessage.Name = "txtMessage";
            txtMessage.ReadOnly = true;
            txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtMessage.Size = new System.Drawing.Size(483, 92);
            txtMessage.TabIndex = 1;
            // 
            // lblExceptionType
            // 
            lblExceptionType.AutoSize = true;
            lblExceptionType.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblExceptionType.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            lblExceptionType.Location = new System.Drawing.Point(16, 125);
            lblExceptionType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblExceptionType.Name = "lblExceptionType";
            lblExceptionType.Size = new System.Drawing.Size(118, 20);
            lblExceptionType.TabIndex = 2;
            lblExceptionType.Text = "Exception Type:";
            // 
            // txtExceptionType
            // 
            txtExceptionType.BackColor = System.Drawing.Color.FromArgb(40, 40, 56);
            txtExceptionType.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtExceptionType.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtExceptionType.ForeColor = System.Drawing.Color.White;
            txtExceptionType.Location = new System.Drawing.Point(147, 120);
            txtExceptionType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            txtExceptionType.Name = "txtExceptionType";
            txtExceptionType.ReadOnly = true;
            txtExceptionType.Size = new System.Drawing.Size(483, 23);
            txtExceptionType.TabIndex = 3;
            // 
            // lblStackTrace
            // 
            lblStackTrace.AutoSize = true;
            lblStackTrace.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblStackTrace.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            lblStackTrace.Location = new System.Drawing.Point(16, 160);
            lblStackTrace.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblStackTrace.Name = "lblStackTrace";
            lblStackTrace.Size = new System.Drawing.Size(91, 20);
            lblStackTrace.TabIndex = 4;
            lblStackTrace.Text = "Stack Trace:";
            // 
            // txtStackTrace
            // 
            txtStackTrace.BackColor = System.Drawing.Color.FromArgb(40, 40, 56);
            txtStackTrace.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtStackTrace.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtStackTrace.ForeColor = System.Drawing.Color.White;
            txtStackTrace.Location = new System.Drawing.Point(13, 202);
            txtStackTrace.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            txtStackTrace.Multiline = true;
            txtStackTrace.Name = "txtStackTrace";
            txtStackTrace.ReadOnly = true;
            txtStackTrace.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtStackTrace.Size = new System.Drawing.Size(556, 178);
            txtStackTrace.TabIndex = 5;
            // 
            // frmError
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(43, 47, 58);
            ClientSize = new System.Drawing.Size(645, 394);
            Controls.Add(txtStackTrace);
            Controls.Add(lblStackTrace);
            Controls.Add(txtExceptionType);
            Controls.Add(lblExceptionType);
            Controls.Add(txtMessage);
            Controls.Add(lblMessage);
            ForeColor = System.Drawing.SystemColors.ControlText;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            Name = "frmError";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Error Details";
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion
    }
}
