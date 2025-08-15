using Presentation.Controles;
using Presentation.Theme;

namespace Presentation
{
    partial class TwoFactorAuthForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblInfo;
        private RoundedTextBox txtCodigo;
        private RoundedButton btnVerificar;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;

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
            tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            lblInfo = new System.Windows.Forms.Label();
            btnVerificar = new RoundedButton();
            txtCodigo = new RoundedTextBox();
            panel1 = new System.Windows.Forms.Panel();
            iconPictureBox5 = new FontAwesome.Sharp.IconPictureBox();
            iconPictureBox3 = new FontAwesome.Sharp.IconPictureBox();
            label1 = new System.Windows.Forms.Label();
            tableLayoutPanel.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox3).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.BackColor = System.Drawing.Color.Transparent;
            tableLayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel.ColumnCount = 1;
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel.Controls.Add(lblInfo, 0, 0);
            tableLayoutPanel.Controls.Add(btnVerificar, 0, 2);
            tableLayoutPanel.Controls.Add(txtCodigo, 0, 1);
            tableLayoutPanel.Location = new System.Drawing.Point(201, 116);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.Padding = new System.Windows.Forms.Padding(20);
            tableLayoutPanel.RowCount = 3;
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel.Size = new System.Drawing.Size(421, 182);
            tableLayoutPanel.TabIndex = 0;
            // 
            // lblInfo
            // 
            lblInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            lblInfo.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            lblInfo.Location = new System.Drawing.Point(24, 21);
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new System.Drawing.Size(373, 50);
            lblInfo.TabIndex = 0;
            lblInfo.Text = "Se ha enviado un código a su correo. Por favor, ingréselo a continuación.";
            lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnVerificar
            // 
            btnVerificar.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnVerificar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnVerificar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnVerificar.ForeColor = System.Drawing.Color.White;
            btnVerificar.Location = new System.Drawing.Point(297, 116);
            btnVerificar.Name = "btnVerificar";
            btnVerificar.Size = new System.Drawing.Size(100, 35);
            btnVerificar.TabIndex = 2;
            btnVerificar.Text = "Verificar";
            // 
            // txtCodigo
            // 
            txtCodigo.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtCodigo.BackColor = System.Drawing.Color.FromArgb(40, 40, 56);
            txtCodigo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtCodigo.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtCodigo.ForeColor = System.Drawing.Color.White;
            txtCodigo.Location = new System.Drawing.Point(24, 80);
            txtCodigo.Name = "txtCodigo";
            txtCodigo.Size = new System.Drawing.Size(373, 23);
            txtCodigo.TabIndex = 1;
            // 
            // panel1
            // 
            panel1.BackColor = System.Drawing.Color.FromArgb(43, 47, 49);
            panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel1.Controls.Add(iconPictureBox5);
            panel1.Controls.Add(iconPictureBox3);
            panel1.Controls.Add(label1);
            panel1.Dock = System.Windows.Forms.DockStyle.Top;
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(800, 63);
            panel1.TabIndex = 4;
            // 
            // iconPictureBox5
            // 
            iconPictureBox5.BackColor = System.Drawing.Color.FromArgb(43, 47, 49);
            iconPictureBox5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            iconPictureBox5.Cursor = System.Windows.Forms.Cursors.Hand;
            iconPictureBox5.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            iconPictureBox5.IconChar = FontAwesome.Sharp.IconChar.X;
            iconPictureBox5.IconColor = System.Drawing.SystemColors.ControlLightLight;
            iconPictureBox5.IconFont = FontAwesome.Sharp.IconFont.Regular;
            iconPictureBox5.IconSize = 55;
            iconPictureBox5.Location = new System.Drawing.Point(744, -1);
            iconPictureBox5.Name = "iconPictureBox5";
            iconPictureBox5.Size = new System.Drawing.Size(55, 63);
            iconPictureBox5.TabIndex = 2;
            iconPictureBox5.TabStop = false;
            iconPictureBox5.Click += iconPictureBox5_Click;
            // 
            // iconPictureBox3
            // 
            iconPictureBox3.BackColor = System.Drawing.Color.FromArgb(43, 47, 49);
            iconPictureBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            iconPictureBox3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            iconPictureBox3.IconChar = FontAwesome.Sharp.IconChar.Paw;
            iconPictureBox3.IconColor = System.Drawing.SystemColors.ControlLightLight;
            iconPictureBox3.IconFont = FontAwesome.Sharp.IconFont.Auto;
            iconPictureBox3.IconSize = 62;
            iconPictureBox3.Location = new System.Drawing.Point(-1, -1);
            iconPictureBox3.Name = "iconPictureBox3";
            iconPictureBox3.Size = new System.Drawing.Size(62, 63);
            iconPictureBox3.TabIndex = 1;
            iconPictureBox3.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            label1.Location = new System.Drawing.Point(256, 18);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(306, 31);
            label1.TabIndex = 0;
            label1.Text = "Autenticación de dos pasos";
            // 
            // TwoFactorAuthForm
            // 
            AcceptButton = btnVerificar;
            BackColor = System.Drawing.Color.FromArgb(43, 47, 49);
            ClientSize = new System.Drawing.Size(800, 356);
            Controls.Add(panel1);
            Controls.Add(tableLayoutPanel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "TwoFactorAuthForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Verificación de Dos Factores";
            tableLayoutPanel.ResumeLayout(false);
            tableLayoutPanel.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox5).EndInit();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox3).EndInit();
            ResumeLayout(false);
        }
        private System.Windows.Forms.Panel panel1;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox5;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox3;
        private System.Windows.Forms.Label label1;
    }
}
