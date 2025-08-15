// src/Presentation/frmNotification.Designer.cs
namespace Presentation
{
    partial class frmNotification
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Button btnOk;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblMessage = new System.Windows.Forms.Label();
            btnOk = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // lblMessage
            // 
            lblMessage.BackColor = System.Drawing.Color.Transparent;
            lblMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            lblMessage.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            lblMessage.Location = new System.Drawing.Point(0, 0);
            lblMessage.Name = "lblMessage";
            lblMessage.Padding = new System.Windows.Forms.Padding(20);
            lblMessage.Size = new System.Drawing.Size(579, 307);
            lblMessage.TabIndex = 0;
            lblMessage.Text = "Message";
            lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnOk
            // 
            btnOk.BackColor = System.Drawing.Color.FromArgb(43, 47, 50);
            btnOk.Cursor = System.Windows.Forms.Cursors.Hand;
            btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnOk.ForeColor = System.Drawing.Color.White;
            btnOk.Location = new System.Drawing.Point(239, 218);
            btnOk.Name = "btnOk";
            btnOk.Size = new System.Drawing.Size(100, 30);
            btnOk.TabIndex = 1;
            btnOk.Text = "OK";
            btnOk.UseVisualStyleBackColor = false;
            btnOk.Click += btnOk_Click;
            // 
            // frmNotification
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(579, 307);
            Controls.Add(btnOk);
            Controls.Add(lblMessage);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Name = "frmNotification";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "frmNotification";
            ResumeLayout(false);
        }
    }
}
