using Presentation.Controles;
using Presentation.Theme;

namespace Presentation
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.Label lblContrasena;

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
            lblUsuario = new System.Windows.Forms.Label();
            txtUsuario = new RoundedTextBox();
            lblContrasena = new System.Windows.Forms.Label();
            txtContrasena = new RoundedTextBox();
            chkMostrarContrasena = new System.Windows.Forms.CheckBox();
            btnLogin = new RoundedButton();
            btnRecuperarContrasena = new RoundedButton();
            iconPictureBox2 = new FontAwesome.Sharp.IconPictureBox();
            iconPictureBox1 = new FontAwesome.Sharp.IconPictureBox();
            panel1 = new System.Windows.Forms.Panel();
            iconPictureBox5 = new FontAwesome.Sharp.IconPictureBox();
            iconPictureBox3 = new FontAwesome.Sharp.IconPictureBox();
            label1 = new System.Windows.Forms.Label();
            iconPictureBox4 = new FontAwesome.Sharp.IconPictureBox();
            tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox1).BeginInit();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox4).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.BackColor = System.Drawing.Color.FromArgb(43, 47, 58);
            tableLayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel.ColumnCount = 2;
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel.Controls.Add(lblUsuario, 0, 0);
            tableLayoutPanel.Controls.Add(txtUsuario, 1, 0);
            tableLayoutPanel.Controls.Add(lblContrasena, 0, 1);
            tableLayoutPanel.Controls.Add(txtContrasena, 1, 1);
            tableLayoutPanel.Controls.Add(chkMostrarContrasena, 1, 2);
            tableLayoutPanel.Controls.Add(btnLogin, 1, 3);
            tableLayoutPanel.Controls.Add(btnRecuperarContrasena, 1, 4);
            tableLayoutPanel.Controls.Add(iconPictureBox2, 0, 4);
            tableLayoutPanel.Controls.Add(iconPictureBox1, 0, 2);
            tableLayoutPanel.Location = new System.Drawing.Point(257, 85);
            tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.Padding = new System.Windows.Forms.Padding(10);
            tableLayoutPanel.RowCount = 5;
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanel.Size = new System.Drawing.Size(534, 338);
            tableLayoutPanel.TabIndex = 0;
            tableLayoutPanel.Paint += tableLayoutPanel_Paint;
            // 
            // lblUsuario
            // 
            lblUsuario.Dock = System.Windows.Forms.DockStyle.Fill;
            lblUsuario.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblUsuario.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            lblUsuario.Location = new System.Drawing.Point(14, 11);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new System.Drawing.Size(249, 35);
            lblUsuario.TabIndex = 0;
            lblUsuario.Text = "Usuario:";
            lblUsuario.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtUsuario
            // 
            txtUsuario.BackColor = System.Drawing.Color.FromArgb(40, 40, 56);
            txtUsuario.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtUsuario.Dock = System.Windows.Forms.DockStyle.Fill;
            txtUsuario.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtUsuario.ForeColor = System.Drawing.Color.White;
            txtUsuario.Location = new System.Drawing.Point(270, 14);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new System.Drawing.Size(250, 23);
            txtUsuario.TabIndex = 1;
            // 
            // lblContrasena
            // 
            lblContrasena.Dock = System.Windows.Forms.DockStyle.Fill;
            lblContrasena.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblContrasena.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            lblContrasena.Location = new System.Drawing.Point(14, 47);
            lblContrasena.Name = "lblContrasena";
            lblContrasena.Size = new System.Drawing.Size(249, 35);
            lblContrasena.TabIndex = 2;
            lblContrasena.Text = "Contraseña:";
            lblContrasena.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtContrasena
            // 
            txtContrasena.BackColor = System.Drawing.Color.FromArgb(40, 40, 56);
            txtContrasena.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtContrasena.Dock = System.Windows.Forms.DockStyle.Fill;
            txtContrasena.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtContrasena.ForeColor = System.Drawing.Color.White;
            txtContrasena.Location = new System.Drawing.Point(270, 50);
            txtContrasena.Name = "txtContrasena";
            txtContrasena.PasswordChar = '●';
            txtContrasena.Size = new System.Drawing.Size(250, 23);
            txtContrasena.TabIndex = 3;
            // 
            // chkMostrarContrasena
            // 
            chkMostrarContrasena.AutoSize = true;
            chkMostrarContrasena.Font = new System.Drawing.Font("Segoe UI", 9F);
            chkMostrarContrasena.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            chkMostrarContrasena.Location = new System.Drawing.Point(270, 86);
            chkMostrarContrasena.Name = "chkMostrarContrasena";
            chkMostrarContrasena.Size = new System.Drawing.Size(158, 24);
            chkMostrarContrasena.TabIndex = 6;
            chkMostrarContrasena.Text = "Mostrar contraseña";
            chkMostrarContrasena.UseVisualStyleBackColor = true;
            chkMostrarContrasena.CheckedChanged += ChkMostrarContrasena_CheckedChanged;
            // 
            // btnLogin
            // 
            btnLogin.Dock = System.Windows.Forms.DockStyle.Right;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnLogin.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnLogin.ForeColor = System.Drawing.Color.White;
            btnLogin.Location = new System.Drawing.Point(400, 117);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new System.Drawing.Size(120, 39);
            btnLogin.TabIndex = 4;
            btnLogin.Text = "Iniciar sesión";
            // 
            // btnRecuperarContrasena
            // 
            btnRecuperarContrasena.Dock = System.Windows.Forms.DockStyle.Fill;
            btnRecuperarContrasena.FlatAppearance.BorderSize = 0;
            btnRecuperarContrasena.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnRecuperarContrasena.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            btnRecuperarContrasena.ForeColor = System.Drawing.Color.White;
            btnRecuperarContrasena.Location = new System.Drawing.Point(270, 163);
            btnRecuperarContrasena.Name = "btnRecuperarContrasena";
            btnRecuperarContrasena.Size = new System.Drawing.Size(250, 161);
            btnRecuperarContrasena.TabIndex = 5;
            btnRecuperarContrasena.Text = "¿Olvidaste tu contraseña?";
            btnRecuperarContrasena.UseVisualStyleBackColor = false;
            // 
            // iconPictureBox2
            // 
            iconPictureBox2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            iconPictureBox2.BackColor = System.Drawing.Color.FromArgb(43, 47, 58);
            iconPictureBox2.ForeColor = System.Drawing.SystemColors.ControlText;
            iconPictureBox2.IconChar = FontAwesome.Sharp.IconChar.Skyatlas;
            iconPictureBox2.IconColor = System.Drawing.SystemColors.ControlText;
            iconPictureBox2.IconFont = FontAwesome.Sharp.IconFont.Auto;
            iconPictureBox2.IconSize = 161;
            iconPictureBox2.Location = new System.Drawing.Point(14, 163);
            iconPictureBox2.Name = "iconPictureBox2";
            iconPictureBox2.Size = new System.Drawing.Size(249, 161);
            iconPictureBox2.TabIndex = 8;
            iconPictureBox2.TabStop = false;
            // 
            // iconPictureBox1
            // 
            iconPictureBox1.BackColor = System.Drawing.Color.FromArgb(43, 47, 58);
            iconPictureBox1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            iconPictureBox1.IconChar = FontAwesome.Sharp.IconChar.FaceLaughBeam;
            iconPictureBox1.IconColor = System.Drawing.SystemColors.ControlLightLight;
            iconPictureBox1.IconFont = FontAwesome.Sharp.IconFont.Regular;
            iconPictureBox1.IconSize = 24;
            iconPictureBox1.Location = new System.Drawing.Point(14, 86);
            iconPictureBox1.Name = "iconPictureBox1";
            iconPictureBox1.Size = new System.Drawing.Size(249, 24);
            iconPictureBox1.TabIndex = 7;
            iconPictureBox1.TabStop = false;
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
            panel1.TabIndex = 1;
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
            label1.Size = new System.Drawing.Size(308, 31);
            label1.TabIndex = 0;
            label1.Text = "Inicia sesión para continuar";
            // 
            // iconPictureBox4
            // 
            iconPictureBox4.BackColor = System.Drawing.Color.FromArgb(43, 47, 58);
            iconPictureBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            iconPictureBox4.ForeColor = System.Drawing.SystemColors.ControlText;
            iconPictureBox4.IconChar = FontAwesome.Sharp.IconChar.User;
            iconPictureBox4.IconColor = System.Drawing.SystemColors.ControlText;
            iconPictureBox4.IconFont = FontAwesome.Sharp.IconFont.Auto;
            iconPictureBox4.IconSize = 185;
            iconPictureBox4.Location = new System.Drawing.Point(0, 99);
            iconPictureBox4.Name = "iconPictureBox4";
            iconPictureBox4.Size = new System.Drawing.Size(185, 324);
            iconPictureBox4.TabIndex = 2;
            iconPictureBox4.TabStop = false;
            // 
            // LoginForm
            // 
            AcceptButton = btnLogin;
            BackColor = System.Drawing.Color.FromArgb(43, 47, 58);
            ClientSize = new System.Drawing.Size(800, 422);
            Controls.Add(iconPictureBox4);
            Controls.Add(panel1);
            Controls.Add(tableLayoutPanel);
            Font = new System.Drawing.Font("Segoe UI", 10F);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LoginForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Iniciar Sesión";
            tableLayoutPanel.ResumeLayout(false);
            tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox1).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox5).EndInit();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox4).EndInit();
            ResumeLayout(false);
        }
        private RoundedTextBox txtUsuario;
        private RoundedTextBox txtContrasena;
        private System.Windows.Forms.CheckBox chkMostrarContrasena;
        private RoundedButton btnLogin;
        private RoundedButton btnRecuperarContrasena;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox1;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox3;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox4;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox5;
    }
}