using Presentation.Controles;
using Presentation.Theme;
using System.Drawing;

// src/Presentation/PreguntasSeguridadForm.Designer.cs
namespace Presentation
{
    partial class PreguntasSeguridadForm
    {
        private System.ComponentModel.IContainer components = null;

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
            btnGuardar = new RoundedButton();
            flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            panel1 = new System.Windows.Forms.Panel();
            iconPictureBox5 = new FontAwesome.Sharp.IconPictureBox();
            iconPictureBox3 = new FontAwesome.Sharp.IconPictureBox();
            label1 = new System.Windows.Forms.Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox3).BeginInit();
            SuspendLayout();
            // 
            // btnGuardar
            // 
            btnGuardar.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnGuardar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(702, 421);
            btnGuardar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(97, 44);
            btnGuardar.TabIndex = 4;
            btnGuardar.Text = "Guardar";
            // 
            // flowLayoutPanel
            // 
            flowLayoutPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            flowLayoutPanel.BackColor = Color.Transparent;
            flowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            flowLayoutPanel.ForeColor = SystemColors.ControlLightLight;
            flowLayoutPanel.Location = new Point(0, 102);
            flowLayoutPanel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            flowLayoutPanel.Name = "flowLayoutPanel";
            flowLayoutPanel.Size = new Size(799, 311);
            flowLayoutPanel.TabIndex = 5;
            flowLayoutPanel.Paint += flowLayoutPanel_Paint;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(43, 47, 49);
            panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel1.Controls.Add(iconPictureBox5);
            panel1.Controls.Add(iconPictureBox3);
            panel1.Controls.Add(label1);
            panel1.Dock = System.Windows.Forms.DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(799, 63);
            panel1.TabIndex = 6;
            // 
            // iconPictureBox5
            // 
            iconPictureBox5.BackColor = Color.FromArgb(43, 47, 49);
            iconPictureBox5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            iconPictureBox5.Cursor = System.Windows.Forms.Cursors.Hand;
            iconPictureBox5.ForeColor = SystemColors.ControlLightLight;
            iconPictureBox5.IconChar = FontAwesome.Sharp.IconChar.X;
            iconPictureBox5.IconColor = SystemColors.ControlLightLight;
            iconPictureBox5.IconFont = FontAwesome.Sharp.IconFont.Regular;
            iconPictureBox5.IconSize = 55;
            iconPictureBox5.Location = new Point(744, -1);
            iconPictureBox5.Name = "iconPictureBox5";
            iconPictureBox5.Size = new Size(55, 63);
            iconPictureBox5.TabIndex = 2;
            iconPictureBox5.TabStop = false;
            iconPictureBox5.Click += iconPictureBox5_Click;
            // 
            // iconPictureBox3
            // 
            iconPictureBox3.BackColor = Color.FromArgb(43, 47, 49);
            iconPictureBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            iconPictureBox3.ForeColor = SystemColors.ControlLightLight;
            iconPictureBox3.IconChar = FontAwesome.Sharp.IconChar.Paw;
            iconPictureBox3.IconColor = SystemColors.ControlLightLight;
            iconPictureBox3.IconFont = FontAwesome.Sharp.IconFont.Auto;
            iconPictureBox3.IconSize = 62;
            iconPictureBox3.Location = new Point(-1, -1);
            iconPictureBox3.Name = "iconPictureBox3";
            iconPictureBox3.Size = new Size(62, 63);
            iconPictureBox3.TabIndex = 1;
            iconPictureBox3.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ControlLightLight;
            label1.Location = new Point(256, 18);
            label1.Name = "label1";
            label1.Size = new Size(269, 31);
            label1.TabIndex = 0;
            label1.Text = "Preguntas de Seguridad";
            // 
            // PreguntasSeguridadForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = Color.FromArgb(43, 47, 49);
            BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            ClientSize = new Size(799, 478);
            Controls.Add(panel1);
            Controls.Add(flowLayoutPanel);
            Controls.Add(btnGuardar);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name = "PreguntasSeguridadForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Preguntas de Seguridad";
            Load += PreguntasSeguridadForm_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox5).EndInit();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox3).EndInit();
            ResumeLayout(false);

        }

        #endregion
        private RoundedButton btnGuardar;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
        private System.Windows.Forms.Panel panel1;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox5;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox3;
        private System.Windows.Forms.Label label1;
    }
}
