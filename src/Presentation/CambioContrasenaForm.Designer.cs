using Presentation.Controles;
using Presentation.Theme;

namespace Presentation
{
    partial class CambioContrasenaForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblActual;
        private System.Windows.Forms.Label lblNueva;
        private System.Windows.Forms.Label lblRepetir;
        private RoundedTextBox txtActual;
        private RoundedTextBox txtNueva;
        private RoundedTextBox txtRepetir;
        private RoundedButton btnCambiar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblActual = new System.Windows.Forms.Label();
            lblNueva = new System.Windows.Forms.Label();
            lblRepetir = new System.Windows.Forms.Label();
            txtActual = new RoundedTextBox();
            txtNueva = new RoundedTextBox();
            txtRepetir = new RoundedTextBox();
            btnCambiar = new RoundedButton();
            panel1 = new System.Windows.Forms.Panel();
            iconPictureBox1 = new FontAwesome.Sharp.IconPictureBox();
            panel2 = new System.Windows.Forms.Panel();
            iconPictureBox5 = new FontAwesome.Sharp.IconPictureBox();
            iconPictureBox3 = new FontAwesome.Sharp.IconPictureBox();
            label1 = new System.Windows.Forms.Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox1).BeginInit();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox3).BeginInit();
            SuspendLayout();
            // 
            // lblActual
            // 
            lblActual.AutoSize = true;
            lblActual.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblActual.Location = new System.Drawing.Point(204, 24);
            lblActual.Name = "lblActual";
            lblActual.Size = new System.Drawing.Size(138, 20);
            lblActual.TabIndex = 0;
            lblActual.Text = "Contraseña actual:";
            // 
            // lblNueva
            // 
            lblNueva.AutoSize = true;
            lblNueva.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblNueva.Location = new System.Drawing.Point(204, 69);
            lblNueva.Name = "lblNueva";
            lblNueva.Size = new System.Drawing.Size(139, 20);
            lblNueva.TabIndex = 2;
            lblNueva.Text = "Nueva contraseña:";
            // 
            // lblRepetir
            // 
            lblRepetir.AutoSize = true;
            lblRepetir.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblRepetir.Location = new System.Drawing.Point(204, 106);
            lblRepetir.Name = "lblRepetir";
            lblRepetir.Size = new System.Drawing.Size(145, 20);
            lblRepetir.TabIndex = 4;
            lblRepetir.Text = "Repetir contraseña:";
            // 
            // txtActual
            // 
            txtActual.BackColor = System.Drawing.Color.FromArgb(40, 40, 56);
            txtActual.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtActual.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtActual.ForeColor = System.Drawing.Color.White;
            txtActual.Location = new System.Drawing.Point(525, 21);
            txtActual.Name = "txtActual";
            txtActual.PasswordChar = '●';
            txtActual.Size = new System.Drawing.Size(200, 23);
            txtActual.TabIndex = 1;
            // 
            // txtNueva
            // 
            txtNueva.BackColor = System.Drawing.Color.FromArgb(40, 40, 56);
            txtNueva.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtNueva.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtNueva.ForeColor = System.Drawing.Color.White;
            txtNueva.Location = new System.Drawing.Point(525, 66);
            txtNueva.Name = "txtNueva";
            txtNueva.PasswordChar = '●';
            txtNueva.Size = new System.Drawing.Size(200, 23);
            txtNueva.TabIndex = 3;
            // 
            // txtRepetir
            // 
            txtRepetir.BackColor = System.Drawing.Color.FromArgb(40, 40, 56);
            txtRepetir.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtRepetir.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtRepetir.ForeColor = System.Drawing.Color.White;
            txtRepetir.Location = new System.Drawing.Point(525, 103);
            txtRepetir.Name = "txtRepetir";
            txtRepetir.PasswordChar = '●';
            txtRepetir.Size = new System.Drawing.Size(200, 23);
            txtRepetir.TabIndex = 5;
            // 
            // btnCambiar
            // 
            btnCambiar.Cursor = System.Windows.Forms.Cursors.Hand;
            btnCambiar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnCambiar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnCambiar.ForeColor = System.Drawing.Color.White;
            btnCambiar.Location = new System.Drawing.Point(657, 152);
            btnCambiar.Name = "btnCambiar";
            btnCambiar.Size = new System.Drawing.Size(100, 35);
            btnCambiar.TabIndex = 6;
            btnCambiar.Text = "Cambiar";
            // 
            // panel1
            // 
            panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel1.Controls.Add(iconPictureBox1);
            panel1.Controls.Add(txtRepetir);
            panel1.Controls.Add(txtNueva);
            panel1.Controls.Add(btnCambiar);
            panel1.Controls.Add(txtActual);
            panel1.Controls.Add(lblRepetir);
            panel1.Controls.Add(lblActual);
            panel1.Controls.Add(lblNueva);
            panel1.Location = new System.Drawing.Point(2, 116);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(798, 212);
            panel1.TabIndex = 8;
            // 
            // iconPictureBox1
            // 
            iconPictureBox1.BackColor = System.Drawing.Color.FromArgb(43, 47, 58);
            iconPictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            iconPictureBox1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            iconPictureBox1.IconChar = FontAwesome.Sharp.IconChar.Question;
            iconPictureBox1.IconColor = System.Drawing.SystemColors.ControlLightLight;
            iconPictureBox1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            iconPictureBox1.IconSize = 122;
            iconPictureBox1.Location = new System.Drawing.Point(0, 0);
            iconPictureBox1.Name = "iconPictureBox1";
            iconPictureBox1.Size = new System.Drawing.Size(122, 212);
            iconPictureBox1.TabIndex = 7;
            iconPictureBox1.TabStop = false;
            // 
            // panel2
            // 
            panel2.BackColor = System.Drawing.Color.FromArgb(43, 47, 49);
            panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel2.Controls.Add(iconPictureBox5);
            panel2.Controls.Add(iconPictureBox3);
            panel2.Controls.Add(label1);
            panel2.Dock = System.Windows.Forms.DockStyle.Top;
            panel2.Location = new System.Drawing.Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(800, 64);
            panel2.TabIndex = 9;
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
            label1.Size = new System.Drawing.Size(313, 31);
            label1.TabIndex = 0;
            label1.Text = "Recuperación de contraseña";
            // 
            // CambioContrasenaForm
            // 
            BackColor = System.Drawing.Color.FromArgb(43, 47, 58);
            ClientSize = new System.Drawing.Size(800, 330);
            Controls.Add(panel2);
            Controls.Add(panel1);
            ForeColor = System.Drawing.SystemColors.ControlLightLight;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CambioContrasenaForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Cambiar Contraseña";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox1).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox5).EndInit();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox3).EndInit();
            ResumeLayout(false);
        }
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox5;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox3;
        private System.Windows.Forms.Label label1;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox1;
    }
}