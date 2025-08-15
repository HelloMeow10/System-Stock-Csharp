using Presentation.Controles;
using Presentation.Theme;

namespace Presentation
{
    partial class RecuperarContrasenaForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.GroupBox grpUsuario;
        private System.Windows.Forms.Label lblUsuario;
        private RoundedTextBox txtUsuario;
        private RoundedButton btnContinuar;
        private System.Windows.Forms.GroupBox grpPreguntas;
        private System.Windows.Forms.FlowLayoutPanel pnlPreguntas;
        private RoundedButton btnRecuperar;

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
            grpUsuario = new System.Windows.Forms.GroupBox();
            lblUsuario = new System.Windows.Forms.Label();
            txtUsuario = new RoundedTextBox();
            btnContinuar = new RoundedButton();
            grpPreguntas = new System.Windows.Forms.GroupBox();
            pnlPreguntas = new System.Windows.Forms.FlowLayoutPanel();
            btnRecuperar = new RoundedButton();
            panel1 = new System.Windows.Forms.Panel();
            iconPictureBox5 = new FontAwesome.Sharp.IconPictureBox();
            iconPictureBox3 = new FontAwesome.Sharp.IconPictureBox();
            label1 = new System.Windows.Forms.Label();
            iconPictureBox1 = new FontAwesome.Sharp.IconPictureBox();
            grpUsuario.SuspendLayout();
            grpPreguntas.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox1).BeginInit();
            SuspendLayout();
            // 
            // grpUsuario
            // 
            grpUsuario.Controls.Add(lblUsuario);
            grpUsuario.Controls.Add(txtUsuario);
            grpUsuario.Controls.Add(btnContinuar);
            grpUsuario.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            grpUsuario.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            grpUsuario.Location = new System.Drawing.Point(12, 117);
            grpUsuario.Name = "grpUsuario";
            grpUsuario.Padding = new System.Windows.Forms.Padding(10);
            grpUsuario.Size = new System.Drawing.Size(460, 80);
            grpUsuario.TabIndex = 0;
            grpUsuario.TabStop = false;
            grpUsuario.Text = "Paso 1: Ingrese su usuario";
            // 
            // lblUsuario
            // 
            lblUsuario.AutoSize = true;
            lblUsuario.Location = new System.Drawing.Point(13, 38);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new System.Drawing.Size(62, 20);
            lblUsuario.TabIndex = 0;
            lblUsuario.Text = "Usuario:";
            // 
            // txtUsuario
            // 
            txtUsuario.BackColor = System.Drawing.Color.FromArgb(40, 40, 56);
            txtUsuario.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtUsuario.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtUsuario.ForeColor = System.Drawing.Color.White;
            txtUsuario.Location = new System.Drawing.Point(79, 33);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new System.Drawing.Size(250, 23);
            txtUsuario.TabIndex = 1;
            // 
            // btnContinuar
            // 
            btnContinuar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnContinuar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnContinuar.ForeColor = System.Drawing.Color.White;
            btnContinuar.Location = new System.Drawing.Point(335, 33);
            btnContinuar.Name = "btnContinuar";
            btnContinuar.Size = new System.Drawing.Size(110, 30);
            btnContinuar.TabIndex = 2;
            btnContinuar.Text = "Continuar";
            // 
            // grpPreguntas
            // 
            grpPreguntas.Controls.Add(pnlPreguntas);
            grpPreguntas.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            grpPreguntas.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            grpPreguntas.Location = new System.Drawing.Point(12, 227);
            grpPreguntas.Name = "grpPreguntas";
            grpPreguntas.Padding = new System.Windows.Forms.Padding(10);
            grpPreguntas.Size = new System.Drawing.Size(460, 170);
            grpPreguntas.TabIndex = 1;
            grpPreguntas.TabStop = false;
            grpPreguntas.Text = "Paso 2: Responda sus preguntas de seguridad";
            grpPreguntas.Visible = false;
            // 
            // pnlPreguntas
            // 
            pnlPreguntas.AutoScroll = true;
            pnlPreguntas.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlPreguntas.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            pnlPreguntas.Location = new System.Drawing.Point(10, 30);
            pnlPreguntas.Name = "pnlPreguntas";
            pnlPreguntas.Size = new System.Drawing.Size(440, 130);
            pnlPreguntas.TabIndex = 0;
            pnlPreguntas.WrapContents = false;
            // 
            // btnRecuperar
            // 
            btnRecuperar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnRecuperar.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            btnRecuperar.ForeColor = System.Drawing.Color.White;
            btnRecuperar.Location = new System.Drawing.Point(282, 429);
            btnRecuperar.Name = "btnRecuperar";
            btnRecuperar.Size = new System.Drawing.Size(190, 35);
            btnRecuperar.TabIndex = 2;
            btnRecuperar.Text = "Recuperar Contraseña";
            btnRecuperar.Visible = false;
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
            panel1.TabIndex = 3;
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
            label1.Size = new System.Drawing.Size(244, 31);
            label1.TabIndex = 0;
            label1.Text = "Recuperar contraseña";
            // 
            // iconPictureBox1
            // 
            iconPictureBox1.BackColor = System.Drawing.Color.FromArgb(43, 47, 49);
            iconPictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            iconPictureBox1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            iconPictureBox1.IconChar = FontAwesome.Sharp.IconChar.ShieldCat;
            iconPictureBox1.IconColor = System.Drawing.SystemColors.ControlLightLight;
            iconPictureBox1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            iconPictureBox1.IconSize = 184;
            iconPictureBox1.Location = new System.Drawing.Point(612, 117);
            iconPictureBox1.Name = "iconPictureBox1";
            iconPictureBox1.Size = new System.Drawing.Size(188, 184);
            iconPictureBox1.TabIndex = 4;
            iconPictureBox1.TabStop = false;
            // 
            // RecuperarContrasenaForm
            // 
            BackColor = System.Drawing.Color.FromArgb(43, 47, 49);
            ClientSize = new System.Drawing.Size(800, 491);
            Controls.Add(iconPictureBox1);
            Controls.Add(panel1);
            Controls.Add(btnRecuperar);
            Controls.Add(grpPreguntas);
            Controls.Add(grpUsuario);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "RecuperarContrasenaForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Recuperar Contraseña";
            grpUsuario.ResumeLayout(false);
            grpUsuario.PerformLayout();
            grpPreguntas.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox5).EndInit();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox1).EndInit();
            ResumeLayout(false);
        }
        private System.Windows.Forms.Panel panel1;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox5;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox3;
        private System.Windows.Forms.Label label1;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox1;
    }
}
