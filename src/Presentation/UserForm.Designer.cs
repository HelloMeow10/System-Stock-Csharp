using Presentation.Controles;
using Presentation.Theme;
using FontAwesome.Sharp;

namespace Presentation
{
    partial class UserForm
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
            btnCambiarPreguntas = new RoundedButton();
            btnCambiarContrasena = new RoundedButton();
            btnMiPerfil = new RoundedButton();
            iconPictureBox1 = new IconPictureBox();
            iconPictureBox2 = new IconPictureBox();
            iconPictureBox4 = new IconPictureBox();
            panel1 = new System.Windows.Forms.Panel();
            iconPictureBox5 = new IconPictureBox();
            iconPictureBox3 = new IconPictureBox();
            label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox4).BeginInit();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox3).BeginInit();
            SuspendLayout();
            // 
            // btnCambiarPreguntas
            // 
            btnCambiarPreguntas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnCambiarPreguntas.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnCambiarPreguntas.ForeColor = System.Drawing.Color.White;
            btnCambiarPreguntas.Location = new System.Drawing.Point(60, 189);
            btnCambiarPreguntas.Name = "btnCambiarPreguntas";
            btnCambiarPreguntas.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            btnCambiarPreguntas.Size = new System.Drawing.Size(220, 45);
            btnCambiarPreguntas.TabIndex = 1;
            btnCambiarPreguntas.Text = "Cambiar Preguntas";
            btnCambiarPreguntas.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCambiarContrasena
            // 
            btnCambiarContrasena.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnCambiarContrasena.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnCambiarContrasena.ForeColor = System.Drawing.Color.White;
            btnCambiarContrasena.Location = new System.Drawing.Point(60, 95);
            btnCambiarContrasena.Name = "btnCambiarContrasena";
            btnCambiarContrasena.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            btnCambiarContrasena.Size = new System.Drawing.Size(220, 45);
            btnCambiarContrasena.TabIndex = 0;
            btnCambiarContrasena.Text = "Cambiar Contraseña";
            btnCambiarContrasena.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnMiPerfil
            //
            btnMiPerfil.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnMiPerfil.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnMiPerfil.ForeColor = System.Drawing.Color.White;
            btnMiPerfil.Location = new System.Drawing.Point(60, 142);
            btnMiPerfil.Name = "btnMiPerfil";
            btnMiPerfil.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            btnMiPerfil.Size = new System.Drawing.Size(220, 45);
            btnMiPerfil.TabIndex = 5;
            btnMiPerfil.Text = "Mi Perfil";
            btnMiPerfil.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // iconPictureBox1
            // 
            iconPictureBox1.BackColor = System.Drawing.Color.Transparent;
            iconPictureBox1.ForeColor = System.Drawing.SystemColors.ControlText;
            iconPictureBox1.IconChar = IconChar.Lock;
            iconPictureBox1.IconColor = System.Drawing.SystemColors.ControlText;
            iconPictureBox1.IconFont = IconFont.Auto;
            iconPictureBox1.IconSize = 35;
            iconPictureBox1.Location = new System.Drawing.Point(286, 95);
            iconPictureBox1.Name = "iconPictureBox1";
            iconPictureBox1.Size = new System.Drawing.Size(35, 35);
            iconPictureBox1.TabIndex = 2;
            iconPictureBox1.TabStop = false;
            // 
            // iconPictureBox2
            // 
            iconPictureBox2.BackColor = System.Drawing.Color.Transparent;
            iconPictureBox2.ForeColor = System.Drawing.SystemColors.ControlText;
            iconPictureBox2.IconChar = IconChar.CircleQuestion;
            iconPictureBox2.IconColor = System.Drawing.SystemColors.ControlText;
            iconPictureBox2.IconFont = IconFont.Auto;
            iconPictureBox2.IconSize = 35;
            iconPictureBox2.Location = new System.Drawing.Point(286, 189);
            iconPictureBox2.Name = "iconPictureBox2";
            iconPictureBox2.Size = new System.Drawing.Size(35, 35);
            iconPictureBox2.TabIndex = 3;
            iconPictureBox2.TabStop = false;
            // 
            // iconPictureBox4
            //
            iconPictureBox4.BackColor = System.Drawing.Color.Transparent;
            iconPictureBox4.ForeColor = System.Drawing.SystemColors.ControlText;
            iconPictureBox4.IconChar = IconChar.User;
            iconPictureBox4.IconColor = System.Drawing.SystemColors.ControlText;
            iconPictureBox4.IconFont = IconFont.Auto;
            iconPictureBox4.IconSize = 35;
            iconPictureBox4.Location = new System.Drawing.Point(286, 142);
            iconPictureBox4.Name = "iconPictureBox4";
            iconPictureBox4.Size = new System.Drawing.Size(35, 35);
            iconPictureBox4.TabIndex = 6;
            iconPictureBox4.TabStop = false;
            //
            // panel1
            // 
            panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel1.Controls.Add(iconPictureBox5);
            panel1.Controls.Add(iconPictureBox3);
            panel1.Controls.Add(label1);
            panel1.Dock = System.Windows.Forms.DockStyle.Top;
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(383, 58);
            panel1.TabIndex = 4;
            // 
            // iconPictureBox5
            // 
            iconPictureBox5.BackColor = System.Drawing.Color.FromArgb(43, 47, 49);
            iconPictureBox5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            iconPictureBox5.Cursor = System.Windows.Forms.Cursors.Hand;
            iconPictureBox5.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            iconPictureBox5.IconChar = IconChar.X;
            iconPictureBox5.IconColor = System.Drawing.SystemColors.ControlLightLight;
            iconPictureBox5.IconFont = IconFont.Regular;
            iconPictureBox5.IconSize = 55;
            iconPictureBox5.Location = new System.Drawing.Point(327, -1);
            iconPictureBox5.Name = "iconPictureBox5";
            iconPictureBox5.Size = new System.Drawing.Size(55, 58);
            iconPictureBox5.TabIndex = 3;
            iconPictureBox5.TabStop = false;
            iconPictureBox5.Click += iconPictureBox5_Click;
            // 
            // iconPictureBox3
            // 
            iconPictureBox3.BackColor = System.Drawing.Color.FromArgb(43, 47, 49);
            iconPictureBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            iconPictureBox3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            iconPictureBox3.IconChar = IconChar.Paw;
            iconPictureBox3.IconColor = System.Drawing.SystemColors.ControlLightLight;
            iconPictureBox3.IconFont = IconFont.Auto;
            iconPictureBox3.IconSize = 62;
            iconPictureBox3.Location = new System.Drawing.Point(-1, -5);
            iconPictureBox3.Name = "iconPictureBox3";
            iconPictureBox3.Size = new System.Drawing.Size(62, 62);
            iconPictureBox3.TabIndex = 2;
            iconPictureBox3.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            label1.Location = new System.Drawing.Point(158, 21);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(54, 28);
            label1.TabIndex = 0;
            label1.Text = "User";
            // 
            // UserForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            BackColor = System.Drawing.Color.FromArgb(43, 47, 49);
            BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            ClientSize = new System.Drawing.Size(383, 294);
            Controls.Add(iconPictureBox4);
            Controls.Add(btnMiPerfil);
            Controls.Add(panel1);
            Controls.Add(iconPictureBox2);
            Controls.Add(iconPictureBox1);
            Controls.Add(btnCambiarPreguntas);
            Controls.Add(btnCambiarContrasena);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Name = "UserForm";
            ShowIcon = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Seguridad";
            ((System.ComponentModel.ISupportInitialize)iconPictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox4).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox5).EndInit();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox3).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private RoundedButton btnCambiarPreguntas;
        private RoundedButton btnCambiarContrasena;
        private RoundedButton btnMiPerfil;
        private IconPictureBox iconPictureBox1;
        private IconPictureBox iconPictureBox2;
        private IconPictureBox iconPictureBox4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private IconPictureBox iconPictureBox3;
        private IconPictureBox iconPictureBox5;
    }
}