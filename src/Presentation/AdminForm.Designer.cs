using Presentation.Controles;
using Presentation.Theme;

namespace Presentation
{
    partial class AdminForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel navigationPanel;
        private System.Windows.Forms.Panel contentPanel;
        private RoundedButton btnNavigatePersonas;
        private RoundedButton btnNavigateUsuarios;
        private RoundedButton btnNavigateGestion;
        private RoundedButton btnNavigateConfiguracion;
        private RoundedButton btnNavigateGestionPersonas;
        private RoundedButton btnMiPerfil;
        private FontAwesome.Sharp.IconPictureBox iconMiPerfil;

        // Controles para "Añadir Persona"
        private System.Windows.Forms.Panel panelPersonas;
        private System.Windows.Forms.TableLayoutPanel personaLayout;
        private System.Windows.Forms.Label lblLegajo, lblNombre, lblApellido, lblTipoDoc, lblNumDoc, lblFechaNacimiento, lblCuil, lblProvincia, lblPartido, lblLocalidad, lblCalle, lblAltura, lblGenero, lblCorreo, lblCelular, lblFechaIngreso;
        private RoundedTextBox txtLegajo, txtNombre, txtApellido, txtNumDoc, txtCuil, txtCalle, txtAltura, txtCorreo, txtCelular;
        private System.Windows.Forms.ComboBox cbxTipoDoc, cbxProvincia, cbxPartido, cbxLocalidad, cbxGenero;
        private System.Windows.Forms.DateTimePicker dtpFechaIngreso, dtpFechaNacimiento;
        private RoundedButton btnGuardarPersona;

        // Controles para "Crear Usuario"
        private System.Windows.Forms.Panel panelUsuarios;
        private System.Windows.Forms.TableLayoutPanel usuarioLayout;
        private System.Windows.Forms.Label lblPersona, lblUsuario, lblPassword, lblRolUsuario;
        private System.Windows.Forms.ComboBox cbxPersona, cbxRolUsuario;
        private RoundedTextBox txtUsuario, txtPassword;
        private RoundedButton btnCrearUsuario;

        // Controles para "Configuracion"
        private System.Windows.Forms.Panel panelConfiguracion;
        private System.Windows.Forms.TableLayoutPanel configuracionLayout;
        private System.Windows.Forms.Label lblMinCaracteres, lblCantPreguntas;
        private System.Windows.Forms.CheckBox chkMayusculasMinusculas;
        private System.Windows.Forms.CheckBox chkNumeros;
        private System.Windows.Forms.CheckBox chkCaracteresEspeciales;
        private System.Windows.Forms.CheckBox chkDobleFactor;
        private System.Windows.Forms.CheckBox chkNoRepetirContrasenas;
        private System.Windows.Forms.CheckBox chkVerificarDatosPersonales;
        private RoundedTextBox txtMinCaracteres;
        private RoundedTextBox txtCantPreguntas;
        private RoundedButton btnGuardarConfig;

        // Controles para "Gestion de Usuarios"
        private System.Windows.Forms.Panel panelGestionUsuarios;
        private System.Windows.Forms.TableLayoutPanel gestionUsuariosLayout;
        private System.Windows.Forms.Label lblBuscarUsuario;
        private RoundedTextBox txtBuscarUsuario;
        private DoubleBufferedDataGridView dgvUsuarios;
        private System.Windows.Forms.Label lblFechaExpiracionGestion;
        private System.Windows.Forms.DateTimePicker dtpFechaExpiracionGestion;
        private RoundedButton btnRefrescarUsuarios, btnGuardarCambios, btnEliminarUsuario;

        // Controles para "Gestion de Personas"
        private System.Windows.Forms.Panel panelGestionPersonas;
        private System.Windows.Forms.TableLayoutPanel gestionPersonasLayout;
        private System.Windows.Forms.Label lblBuscarPersona;
        private RoundedTextBox txtBuscarPersona;
        private DoubleBufferedDataGridView dgvPersonas;
        private RoundedButton btnRefrescarPersonas;
        private System.Windows.Forms.FlowLayoutPanel personasButtonPanel;
        private RoundedButton btnEliminarPersona;
        private RoundedButton btnGuardarCambiosPersona;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            navigationPanel = new System.Windows.Forms.Panel();
            btnNavigatePersonas = new RoundedButton();
            btnNavigateUsuarios = new RoundedButton();
            btnNavigateGestion = new RoundedButton();
            btnNavigateConfiguracion = new RoundedButton();
            btnNavigateGestionPersonas = new RoundedButton();
            btnMiPerfil = new RoundedButton();
            iconMiPerfil = new FontAwesome.Sharp.IconPictureBox();
            contentPanel = new System.Windows.Forms.Panel();
            panelPersonas = new System.Windows.Forms.Panel();
            personaLayout = new System.Windows.Forms.TableLayoutPanel();
            lblLegajo = new System.Windows.Forms.Label();
            txtLegajo = new RoundedTextBox();
            lblNombre = new System.Windows.Forms.Label();
            txtNombre = new RoundedTextBox();
            lblApellido = new System.Windows.Forms.Label();
            txtApellido = new RoundedTextBox();
            lblTipoDoc = new System.Windows.Forms.Label();
            cbxTipoDoc = new System.Windows.Forms.ComboBox();
            lblNumDoc = new System.Windows.Forms.Label();
            txtNumDoc = new RoundedTextBox();
            lblFechaNacimiento = new System.Windows.Forms.Label();
            dtpFechaNacimiento = new System.Windows.Forms.DateTimePicker();
            lblCuil = new System.Windows.Forms.Label();
            txtCuil = new RoundedTextBox();
            lblProvincia = new System.Windows.Forms.Label();
            cbxProvincia = new System.Windows.Forms.ComboBox();
            lblPartido = new System.Windows.Forms.Label();
            cbxPartido = new System.Windows.Forms.ComboBox();
            lblLocalidad = new System.Windows.Forms.Label();
            cbxLocalidad = new System.Windows.Forms.ComboBox();
            lblCalle = new System.Windows.Forms.Label();
            txtCalle = new RoundedTextBox();
            lblAltura = new System.Windows.Forms.Label();
            txtAltura = new RoundedTextBox();
            lblGenero = new System.Windows.Forms.Label();
            cbxGenero = new System.Windows.Forms.ComboBox();
            lblCorreo = new System.Windows.Forms.Label();
            txtCorreo = new RoundedTextBox();
            lblCelular = new System.Windows.Forms.Label();
            txtCelular = new RoundedTextBox();
            lblFechaIngreso = new System.Windows.Forms.Label();
            dtpFechaIngreso = new System.Windows.Forms.DateTimePicker();
            btnGuardarPersona = new RoundedButton();
            panelUsuarios = new System.Windows.Forms.Panel();
            usuarioLayout = new System.Windows.Forms.TableLayoutPanel();
            lblPersona = new System.Windows.Forms.Label();
            cbxPersona = new System.Windows.Forms.ComboBox();
            lblUsuario = new System.Windows.Forms.Label();
            txtUsuario = new RoundedTextBox();
            lblPassword = new System.Windows.Forms.Label();
            txtPassword = new RoundedTextBox();
            lblRolUsuario = new System.Windows.Forms.Label();
            cbxRolUsuario = new System.Windows.Forms.ComboBox();
            btnCrearUsuario = new RoundedButton();
            panelGestionUsuarios = new System.Windows.Forms.Panel();
            gestionUsuariosLayout = new System.Windows.Forms.TableLayoutPanel();
            lblBuscarUsuario = new System.Windows.Forms.Label();
            txtBuscarUsuario = new RoundedTextBox();
            dgvUsuarios = new DoubleBufferedDataGridView();
            lblFechaExpiracionGestion = new System.Windows.Forms.Label();
            dtpFechaExpiracionGestion = new System.Windows.Forms.DateTimePicker();
            buttonPanel = new System.Windows.Forms.FlowLayoutPanel();
            btnEliminarUsuario = new RoundedButton();
            btnGuardarCambios = new RoundedButton();
            btnRefrescarUsuarios = new RoundedButton();
            panelConfiguracion = new System.Windows.Forms.Panel();
            configuracionLayout = new System.Windows.Forms.TableLayoutPanel();
            chkMayusculasMinusculas = new System.Windows.Forms.CheckBox();
            chkNumeros = new System.Windows.Forms.CheckBox();
            chkCaracteresEspeciales = new System.Windows.Forms.CheckBox();
            chkDobleFactor = new System.Windows.Forms.CheckBox();
            chkNoRepetirContrasenas = new System.Windows.Forms.CheckBox();
            chkVerificarDatosPersonales = new System.Windows.Forms.CheckBox();
            lblMinCaracteres = new System.Windows.Forms.Label();
            txtMinCaracteres = new RoundedTextBox();
            lblCantPreguntas = new System.Windows.Forms.Label();
            txtCantPreguntas = new RoundedTextBox();
            btnGuardarConfig = new RoundedButton();
            iconPictureBox1 = new FontAwesome.Sharp.IconPictureBox();
            navigationPanel.SuspendLayout();
            contentPanel.SuspendLayout();
            panelPersonas.SuspendLayout();
            personaLayout.SuspendLayout();
            panelUsuarios.SuspendLayout();
            usuarioLayout.SuspendLayout();
            panelGestionUsuarios.SuspendLayout();
            gestionUsuariosLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).BeginInit();
            buttonPanel.SuspendLayout();
            panelGestionPersonas = new System.Windows.Forms.Panel();
            gestionPersonasLayout = new System.Windows.Forms.TableLayoutPanel();
            lblBuscarPersona = new System.Windows.Forms.Label();
            txtBuscarPersona = new RoundedTextBox();
            dgvPersonas = new DoubleBufferedDataGridView();
            personasButtonPanel = new System.Windows.Forms.FlowLayoutPanel();
            btnEliminarPersona = new RoundedButton();
            btnGuardarCambiosPersona = new RoundedButton();
            btnRefrescarPersonas = new RoundedButton();
            panelGestionPersonas.SuspendLayout();
            gestionPersonasLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPersonas).BeginInit();
            personasButtonPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconMiPerfil).BeginInit();
            panelConfiguracion.SuspendLayout();
            configuracionLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox1).BeginInit();
            SuspendLayout();
            // 
            // navigationPanel
            // 
            navigationPanel.BackColor = System.Drawing.Color.FromArgb(43, 47, 49);
            navigationPanel.Controls.Add(btnNavigatePersonas);
            navigationPanel.Controls.Add(btnNavigateUsuarios);
            navigationPanel.Controls.Add(btnNavigateGestion);
            navigationPanel.Controls.Add(btnNavigateGestionPersonas);
            navigationPanel.Controls.Add(btnNavigateConfiguracion);
            navigationPanel.Controls.Add(btnMiPerfil);
            navigationPanel.Controls.Add(iconMiPerfil);
            navigationPanel.Dock = System.Windows.Forms.DockStyle.Left;
            navigationPanel.Location = new System.Drawing.Point(0, 0);
            navigationPanel.Name = "navigationPanel";
            navigationPanel.Size = new System.Drawing.Size(200, 600);
            navigationPanel.TabIndex = 1;
            // 
            // btnNavigatePersonas
            // 
            btnNavigatePersonas.Dock = System.Windows.Forms.DockStyle.Top;
            btnNavigatePersonas.FlatAppearance.BorderSize = 0;
            btnNavigatePersonas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnNavigatePersonas.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            btnNavigatePersonas.ForeColor = System.Drawing.Color.White;
            btnNavigatePersonas.Location = new System.Drawing.Point(0, 150);
            btnNavigatePersonas.Name = "btnNavigatePersonas";
            btnNavigatePersonas.Size = new System.Drawing.Size(200, 50);
            btnNavigatePersonas.TabIndex = 0;
            btnNavigatePersonas.Text = "Añadir Persona";
            btnNavigatePersonas.UseVisualStyleBackColor = true;
            // 
            // btnNavigateUsuarios
            // 
            btnNavigateUsuarios.Dock = System.Windows.Forms.DockStyle.Top;
            btnNavigateUsuarios.FlatAppearance.BorderSize = 0;
            btnNavigateUsuarios.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnNavigateUsuarios.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            btnNavigateUsuarios.ForeColor = System.Drawing.Color.White;
            btnNavigateUsuarios.Location = new System.Drawing.Point(0, 100);
            btnNavigateUsuarios.Name = "btnNavigateUsuarios";
            btnNavigateUsuarios.Size = new System.Drawing.Size(200, 50);
            btnNavigateUsuarios.TabIndex = 1;
            btnNavigateUsuarios.Text = "Crear Usuario";
            btnNavigateUsuarios.UseVisualStyleBackColor = true;
            // 
            // btnNavigateGestion
            // 
            btnNavigateGestion.Dock = System.Windows.Forms.DockStyle.Top;
            btnNavigateGestion.FlatAppearance.BorderSize = 0;
            btnNavigateGestion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnNavigateGestion.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            btnNavigateGestion.ForeColor = System.Drawing.Color.White;
            btnNavigateGestion.Location = new System.Drawing.Point(0, 50);
            btnNavigateGestion.Name = "btnNavigateGestion";
            btnNavigateGestion.Size = new System.Drawing.Size(200, 50);
            btnNavigateGestion.TabIndex = 2;
            btnNavigateGestion.Text = "Gestión de Usuarios";
            btnNavigateGestion.UseVisualStyleBackColor = true;
            // 
            // btnNavigateGestionPersonas
            //
            btnNavigateGestionPersonas.Dock = System.Windows.Forms.DockStyle.Top;
            btnNavigateGestionPersonas.FlatAppearance.BorderSize = 0;
            btnNavigateGestionPersonas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnNavigateGestionPersonas.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            btnNavigateGestionPersonas.ForeColor = System.Drawing.Color.White;
            btnNavigateGestionPersonas.Location = new System.Drawing.Point(0, 200);
            btnNavigateGestionPersonas.Name = "btnNavigateGestionPersonas";
            btnNavigateGestionPersonas.Size = new System.Drawing.Size(200, 50);
            btnNavigateGestionPersonas.TabIndex = 4;
            btnNavigateGestionPersonas.Text = "Gestión de Personas";
            btnNavigateGestionPersonas.UseVisualStyleBackColor = true;
            //
            // btnNavigateConfiguracion
            // 
            btnNavigateConfiguracion.Dock = System.Windows.Forms.DockStyle.Top;
            btnNavigateConfiguracion.FlatAppearance.BorderSize = 0;
            btnNavigateConfiguracion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnNavigateConfiguracion.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            btnNavigateConfiguracion.ForeColor = System.Drawing.Color.White;
            btnNavigateConfiguracion.Location = new System.Drawing.Point(0, 0);
            btnNavigateConfiguracion.Name = "btnNavigateConfiguracion";
            btnNavigateConfiguracion.Size = new System.Drawing.Size(200, 50);
            btnNavigateConfiguracion.TabIndex = 3;
            btnNavigateConfiguracion.Text = "Configuración";
            btnNavigateConfiguracion.UseVisualStyleBackColor = true;
            // 
            // btnMiPerfil
            //
            btnMiPerfil.Dock = System.Windows.Forms.DockStyle.Bottom;
            btnMiPerfil.FlatAppearance.BorderSize = 0;
            btnMiPerfil.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnMiPerfil.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            btnMiPerfil.ForeColor = System.Drawing.Color.White;
            btnMiPerfil.Location = new System.Drawing.Point(0, 550);
            btnMiPerfil.Name = "btnMiPerfil";
            btnMiPerfil.Size = new System.Drawing.Size(200, 50);
            btnMiPerfil.TabIndex = 5;
            btnMiPerfil.Text = "Mi Perfil";
            btnMiPerfil.UseVisualStyleBackColor = true;
            //
            // iconMiPerfil
            //
            iconMiPerfil.BackColor = System.Drawing.Color.FromArgb(43, 47, 49);
            iconMiPerfil.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            iconMiPerfil.IconChar = FontAwesome.Sharp.IconChar.UserCircle;
            iconMiPerfil.IconColor = System.Drawing.SystemColors.ControlLightLight;
            iconMiPerfil.IconFont = FontAwesome.Sharp.IconFont.Auto;
            iconMiPerfil.IconSize = 35;
            iconMiPerfil.Location = new System.Drawing.Point(160, 560);
            iconMiPerfil.Name = "iconMiPerfil";
            iconMiPerfil.Size = new System.Drawing.Size(35, 35);
            iconMiPerfil.TabIndex = 6;
            iconMiPerfil.TabStop = false;
            //
            // contentPanel
            // 
            contentPanel.Controls.Add(panelPersonas);
            contentPanel.Controls.Add(panelUsuarios);
            contentPanel.Controls.Add(panelGestionUsuarios);
            contentPanel.Controls.Add(panelGestionPersonas);
            contentPanel.Controls.Add(panelConfiguracion);
            contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            contentPanel.Location = new System.Drawing.Point(200, 0);
            contentPanel.Name = "contentPanel";
            contentPanel.Size = new System.Drawing.Size(623, 600);
            contentPanel.TabIndex = 0;
            // 
            // panelPersonas
            // 
            panelPersonas.BackColor = System.Drawing.Color.FromArgb(43, 47, 49);
            panelPersonas.Controls.Add(iconPictureBox1);
            panelPersonas.Controls.Add(personaLayout);
            panelPersonas.Dock = System.Windows.Forms.DockStyle.Fill;
            panelPersonas.Location = new System.Drawing.Point(0, 0);
            panelPersonas.Name = "panelPersonas";
            panelPersonas.Size = new System.Drawing.Size(623, 600);
            panelPersonas.TabIndex = 0;
            // 
            // personaLayout
            // 
            personaLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            personaLayout.BackColor = System.Drawing.Color.Transparent;
            personaLayout.ColumnCount = 2;
            personaLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            personaLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            personaLayout.Controls.Add(lblLegajo, 0, 0);
            personaLayout.Controls.Add(txtLegajo, 1, 0);
            personaLayout.Controls.Add(lblNombre, 0, 1);
            personaLayout.Controls.Add(txtNombre, 1, 1);
            personaLayout.Controls.Add(lblApellido, 0, 2);
            personaLayout.Controls.Add(txtApellido, 1, 2);
            personaLayout.Controls.Add(lblTipoDoc, 0, 3);
            personaLayout.Controls.Add(cbxTipoDoc, 1, 3);
            personaLayout.Controls.Add(lblNumDoc, 0, 4);
            personaLayout.Controls.Add(txtNumDoc, 1, 4);
            personaLayout.Controls.Add(lblFechaNacimiento, 0, 5);
            personaLayout.Controls.Add(dtpFechaNacimiento, 1, 5);
            personaLayout.Controls.Add(lblCuil, 0, 6);
            personaLayout.Controls.Add(txtCuil, 1, 6);
            personaLayout.Controls.Add(lblProvincia, 0, 7);
            personaLayout.Controls.Add(cbxProvincia, 1, 7);
            personaLayout.Controls.Add(lblPartido, 0, 8);
            personaLayout.Controls.Add(cbxPartido, 1, 8);
            personaLayout.Controls.Add(lblLocalidad, 0, 9);
            personaLayout.Controls.Add(cbxLocalidad, 1, 9);
            personaLayout.Controls.Add(lblCalle, 0, 10);
            personaLayout.Controls.Add(txtCalle, 1, 10);
            personaLayout.Controls.Add(lblAltura, 0, 11);
            personaLayout.Controls.Add(txtAltura, 1, 11);
            personaLayout.Controls.Add(lblGenero, 0, 12);
            personaLayout.Controls.Add(cbxGenero, 1, 12);
            personaLayout.Controls.Add(lblCorreo, 0, 13);
            personaLayout.Controls.Add(txtCorreo, 1, 13);
            personaLayout.Controls.Add(lblCelular, 0, 14);
            personaLayout.Controls.Add(txtCelular, 1, 14);
            personaLayout.Controls.Add(lblFechaIngreso, 0, 15);
            personaLayout.Controls.Add(dtpFechaIngreso, 1, 15);
            personaLayout.Controls.Add(btnGuardarPersona, 1, 16);
            personaLayout.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            personaLayout.Location = new System.Drawing.Point(0, 0);
            personaLayout.Name = "personaLayout";
            personaLayout.RowCount = 17;
            personaLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            personaLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            personaLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            personaLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            personaLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            personaLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            personaLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            personaLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            personaLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            personaLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            personaLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            personaLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            personaLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            personaLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            personaLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            personaLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            personaLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            personaLayout.Size = new System.Drawing.Size(492, 597);
            personaLayout.TabIndex = 0;
            // 
            // lblLegajo
            // 
            lblLegajo.Anchor = System.Windows.Forms.AnchorStyles.Right;
            lblLegajo.AutoSize = true;
            lblLegajo.Location = new System.Drawing.Point(105, 3);
            lblLegajo.Name = "lblLegajo";
            lblLegajo.Size = new System.Drawing.Size(64, 23);
            lblLegajo.TabIndex = 0;
            lblLegajo.Text = "Legajo:";
            // 
            // txtLegajo
            // 
            txtLegajo.BackColor = System.Drawing.Color.FromArgb(40, 40, 56);
            txtLegajo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtLegajo.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtLegajo.ForeColor = System.Drawing.Color.White;
            txtLegajo.Location = new System.Drawing.Point(175, 3);
            txtLegajo.Name = "txtLegajo";
            txtLegajo.Size = new System.Drawing.Size(100, 23);
            txtLegajo.TabIndex = 1;
            // 
            // lblNombre
            // 
            lblNombre.Anchor = System.Windows.Forms.AnchorStyles.Right;
            lblNombre.AutoSize = true;
            lblNombre.Location = new System.Drawing.Point(92, 33);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new System.Drawing.Size(77, 23);
            lblNombre.TabIndex = 2;
            lblNombre.Text = "Nombre:";
            // 
            // txtNombre
            // 
            txtNombre.BackColor = System.Drawing.Color.FromArgb(40, 40, 56);
            txtNombre.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtNombre.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtNombre.ForeColor = System.Drawing.Color.White;
            txtNombre.Location = new System.Drawing.Point(175, 33);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new System.Drawing.Size(100, 23);
            txtNombre.TabIndex = 3;
            // 
            // lblApellido
            // 
            lblApellido.Anchor = System.Windows.Forms.AnchorStyles.Right;
            lblApellido.AutoSize = true;
            lblApellido.Location = new System.Drawing.Point(93, 63);
            lblApellido.Name = "lblApellido";
            lblApellido.Size = new System.Drawing.Size(76, 23);
            lblApellido.TabIndex = 4;
            lblApellido.Text = "Apellido:";
            // 
            // txtApellido
            // 
            txtApellido.BackColor = System.Drawing.Color.FromArgb(40, 40, 56);
            txtApellido.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtApellido.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtApellido.ForeColor = System.Drawing.Color.White;
            txtApellido.Location = new System.Drawing.Point(175, 63);
            txtApellido.Name = "txtApellido";
            txtApellido.Size = new System.Drawing.Size(100, 23);
            txtApellido.TabIndex = 5;
            // 
            // lblTipoDoc
            // 
            lblTipoDoc.Anchor = System.Windows.Forms.AnchorStyles.Right;
            lblTipoDoc.AutoSize = true;
            lblTipoDoc.Location = new System.Drawing.Point(27, 93);
            lblTipoDoc.Name = "lblTipoDoc";
            lblTipoDoc.Size = new System.Drawing.Size(142, 23);
            lblTipoDoc.TabIndex = 6;
            lblTipoDoc.Text = "Tipo Documento:";
            // 
            // cbxTipoDoc
            // 
            cbxTipoDoc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            cbxTipoDoc.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            cbxTipoDoc.Location = new System.Drawing.Point(175, 93);
            cbxTipoDoc.Name = "cbxTipoDoc";
            cbxTipoDoc.Size = new System.Drawing.Size(121, 31);
            cbxTipoDoc.TabIndex = 7;
            // 
            // lblNumDoc
            // 
            lblNumDoc.Anchor = System.Windows.Forms.AnchorStyles.Right;
            lblNumDoc.AutoSize = true;
            lblNumDoc.Location = new System.Drawing.Point(27, 123);
            lblNumDoc.Name = "lblNumDoc";
            lblNumDoc.Size = new System.Drawing.Size(142, 23);
            lblNumDoc.TabIndex = 8;
            lblNumDoc.Text = "Nro. Documento:";
            // 
            // txtNumDoc
            // 
            txtNumDoc.BackColor = System.Drawing.Color.FromArgb(40, 40, 56);
            txtNumDoc.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtNumDoc.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtNumDoc.ForeColor = System.Drawing.Color.White;
            txtNumDoc.Location = new System.Drawing.Point(175, 123);
            txtNumDoc.Name = "txtNumDoc";
            txtNumDoc.Size = new System.Drawing.Size(100, 23);
            txtNumDoc.TabIndex = 9;
            // 
            // lblFechaNacimiento
            // 
            lblFechaNacimiento.Anchor = System.Windows.Forms.AnchorStyles.Right;
            lblFechaNacimiento.AutoSize = true;
            lblFechaNacimiento.Location = new System.Drawing.Point(67, 150);
            lblFechaNacimiento.Name = "lblFechaNacimiento";
            lblFechaNacimiento.Size = new System.Drawing.Size(102, 30);
            lblFechaNacimiento.TabIndex = 10;
            lblFechaNacimiento.Text = "Fecha de Nacimiento:";
            // 
            // dtpFechaNacimiento
            // 
            dtpFechaNacimiento.Location = new System.Drawing.Point(175, 153);
            dtpFechaNacimiento.Name = "dtpFechaNacimiento";
            dtpFechaNacimiento.Size = new System.Drawing.Size(200, 30);
            dtpFechaNacimiento.TabIndex = 11;
            // 
            // lblCuil
            // 
            lblCuil.Anchor = System.Windows.Forms.AnchorStyles.Right;
            lblCuil.AutoSize = true;
            lblCuil.Location = new System.Drawing.Point(119, 183);
            lblCuil.Name = "lblCuil";
            lblCuil.Size = new System.Drawing.Size(50, 23);
            lblCuil.TabIndex = 10;
            lblCuil.Text = "CUIL:";
            // 
            // txtCuil
            // 
            txtCuil.BackColor = System.Drawing.Color.FromArgb(40, 40, 56);
            txtCuil.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtCuil.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtCuil.ForeColor = System.Drawing.Color.White;
            txtCuil.Location = new System.Drawing.Point(175, 183);
            txtCuil.Name = "txtCuil";
            txtCuil.Size = new System.Drawing.Size(100, 23);
            txtCuil.TabIndex = 11;
            // 
            // lblProvincia
            // 
            lblProvincia.Anchor = System.Windows.Forms.AnchorStyles.Right;
            lblProvincia.AutoSize = true;
            lblProvincia.Location = new System.Drawing.Point(86, 213);
            lblProvincia.Name = "lblProvincia";
            lblProvincia.Size = new System.Drawing.Size(83, 23);
            lblProvincia.TabIndex = 16;
            lblProvincia.Text = "Provincia:";
            // 
            // cbxProvincia
            // 
            cbxProvincia.Location = new System.Drawing.Point(175, 213);
            cbxProvincia.Name = "cbxProvincia";
            cbxProvincia.Size = new System.Drawing.Size(121, 31);
            cbxProvincia.TabIndex = 17;
            // 
            // lblPartido
            // 
            lblPartido.Anchor = System.Windows.Forms.AnchorStyles.Right;
            lblPartido.AutoSize = true;
            lblPartido.Location = new System.Drawing.Point(101, 243);
            lblPartido.Name = "lblPartido";
            lblPartido.Size = new System.Drawing.Size(68, 23);
            lblPartido.TabIndex = 16;
            lblPartido.Text = "Partido:";
            // 
            // cbxPartido
            // 
            cbxPartido.Location = new System.Drawing.Point(175, 243);
            cbxPartido.Name = "cbxPartido";
            cbxPartido.Size = new System.Drawing.Size(121, 31);
            cbxPartido.TabIndex = 17;
            // 
            // lblLocalidad
            // 
            lblLocalidad.Anchor = System.Windows.Forms.AnchorStyles.Right;
            lblLocalidad.AutoSize = true;
            lblLocalidad.Location = new System.Drawing.Point(83, 273);
            lblLocalidad.Name = "lblLocalidad";
            lblLocalidad.Size = new System.Drawing.Size(86, 23);
            lblLocalidad.TabIndex = 16;
            lblLocalidad.Text = "Localidad:";
            // 
            // cbxLocalidad
            // 
            cbxLocalidad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            cbxLocalidad.Location = new System.Drawing.Point(175, 273);
            cbxLocalidad.Name = "cbxLocalidad";
            cbxLocalidad.Size = new System.Drawing.Size(121, 31);
            cbxLocalidad.TabIndex = 17;
            // 
            // lblCalle
            // 
            lblCalle.Anchor = System.Windows.Forms.AnchorStyles.Right;
            lblCalle.AutoSize = true;
            lblCalle.Location = new System.Drawing.Point(118, 303);
            lblCalle.Name = "lblCalle";
            lblCalle.Size = new System.Drawing.Size(51, 23);
            lblCalle.TabIndex = 12;
            lblCalle.Text = "Calle:";
            // 
            // txtCalle
            // 
            txtCalle.BackColor = System.Drawing.Color.FromArgb(40, 40, 56);
            txtCalle.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtCalle.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtCalle.ForeColor = System.Drawing.Color.White;
            txtCalle.Location = new System.Drawing.Point(175, 303);
            txtCalle.Name = "txtCalle";
            txtCalle.Size = new System.Drawing.Size(100, 23);
            txtCalle.TabIndex = 13;
            // 
            // lblAltura
            // 
            lblAltura.Anchor = System.Windows.Forms.AnchorStyles.Right;
            lblAltura.AutoSize = true;
            lblAltura.Location = new System.Drawing.Point(109, 333);
            lblAltura.Name = "lblAltura";
            lblAltura.Size = new System.Drawing.Size(60, 23);
            lblAltura.TabIndex = 14;
            lblAltura.Text = "Altura:";
            // 
            // txtAltura
            // 
            txtAltura.BackColor = System.Drawing.Color.FromArgb(40, 40, 56);
            txtAltura.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtAltura.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtAltura.ForeColor = System.Drawing.Color.White;
            txtAltura.Location = new System.Drawing.Point(175, 333);
            txtAltura.Name = "txtAltura";
            txtAltura.Size = new System.Drawing.Size(100, 23);
            txtAltura.TabIndex = 15;
            // 
            // lblGenero
            // 
            lblGenero.Anchor = System.Windows.Forms.AnchorStyles.Right;
            lblGenero.AutoSize = true;
            lblGenero.Location = new System.Drawing.Point(99, 363);
            lblGenero.Name = "lblGenero";
            lblGenero.Size = new System.Drawing.Size(70, 23);
            lblGenero.TabIndex = 18;
            lblGenero.Text = "Género:";
            // 
            // cbxGenero
            // 
            cbxGenero.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            cbxGenero.Location = new System.Drawing.Point(175, 363);
            cbxGenero.Name = "cbxGenero";
            cbxGenero.Size = new System.Drawing.Size(121, 31);
            cbxGenero.TabIndex = 19;
            // 
            // lblCorreo
            // 
            lblCorreo.Anchor = System.Windows.Forms.AnchorStyles.Right;
            lblCorreo.AutoSize = true;
            lblCorreo.Location = new System.Drawing.Point(14, 393);
            lblCorreo.Name = "lblCorreo";
            lblCorreo.Size = new System.Drawing.Size(155, 23);
            lblCorreo.TabIndex = 20;
            lblCorreo.Text = "Correo Electrónico:";
            // 
            // txtCorreo
            // 
            txtCorreo.BackColor = System.Drawing.Color.FromArgb(40, 40, 56);
            txtCorreo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtCorreo.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtCorreo.ForeColor = System.Drawing.Color.White;
            txtCorreo.Location = new System.Drawing.Point(175, 393);
            txtCorreo.Name = "txtCorreo";
            txtCorreo.Size = new System.Drawing.Size(100, 23);
            txtCorreo.TabIndex = 21;
            // 
            // lblCelular
            // 
            lblCelular.Anchor = System.Windows.Forms.AnchorStyles.Right;
            lblCelular.AutoSize = true;
            lblCelular.Location = new System.Drawing.Point(102, 423);
            lblCelular.Name = "lblCelular";
            lblCelular.Size = new System.Drawing.Size(67, 23);
            lblCelular.TabIndex = 16;
            lblCelular.Text = "Celular:";
            // 
            // txtCelular
            // 
            txtCelular.BackColor = System.Drawing.Color.FromArgb(40, 40, 56);
            txtCelular.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtCelular.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtCelular.ForeColor = System.Drawing.Color.White;
            txtCelular.Location = new System.Drawing.Point(175, 423);
            txtCelular.Name = "txtCelular";
            txtCelular.Size = new System.Drawing.Size(121, 23);
            txtCelular.TabIndex = 17;
            // 
            // lblFechaIngreso
            // 
            lblFechaIngreso.Anchor = System.Windows.Forms.AnchorStyles.Right;
            lblFechaIngreso.AutoSize = true;
            lblFechaIngreso.Location = new System.Drawing.Point(25, 453);
            lblFechaIngreso.Name = "lblFechaIngreso";
            lblFechaIngreso.Size = new System.Drawing.Size(144, 23);
            lblFechaIngreso.TabIndex = 22;
            lblFechaIngreso.Text = "Fecha de Ingreso:";
            // 
            // dtpFechaIngreso
            // 
            dtpFechaIngreso.Location = new System.Drawing.Point(175, 453);
            dtpFechaIngreso.Name = "dtpFechaIngreso";
            dtpFechaIngreso.Size = new System.Drawing.Size(200, 30);
            dtpFechaIngreso.TabIndex = 23;
            // 
            // btnGuardarPersona
            // 
            btnGuardarPersona.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnGuardarPersona.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnGuardarPersona.ForeColor = System.Drawing.Color.White;
            btnGuardarPersona.Location = new System.Drawing.Point(175, 483);
            btnGuardarPersona.Name = "btnGuardarPersona";
            btnGuardarPersona.Size = new System.Drawing.Size(150, 34);
            btnGuardarPersona.TabIndex = 22;
            btnGuardarPersona.Text = "Guardar Persona";
            // 
            // panelUsuarios
            // 
            panelUsuarios.Controls.Add(usuarioLayout);
            panelUsuarios.Dock = System.Windows.Forms.DockStyle.Fill;
            panelUsuarios.Location = new System.Drawing.Point(0, 0);
            panelUsuarios.Name = "panelUsuarios";
            panelUsuarios.Size = new System.Drawing.Size(623, 600);
            panelUsuarios.TabIndex = 1;
            panelUsuarios.Visible = false;
            // 
            // usuarioLayout
            // 
            usuarioLayout.BackColor = System.Drawing.Color.Transparent;
            usuarioLayout.ColumnCount = 2;
            usuarioLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            usuarioLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            usuarioLayout.Controls.Add(lblPersona, 0, 0);
            usuarioLayout.Controls.Add(cbxPersona, 1, 0);
            usuarioLayout.Controls.Add(lblUsuario, 0, 1);
            usuarioLayout.Controls.Add(txtUsuario, 1, 1);
            usuarioLayout.Controls.Add(lblPassword, 0, 2);
            usuarioLayout.Controls.Add(txtPassword, 1, 2);
            usuarioLayout.Controls.Add(lblRolUsuario, 0, 3);
            usuarioLayout.Controls.Add(cbxRolUsuario, 1, 3);
            usuarioLayout.Controls.Add(btnCrearUsuario, 1, 4);
            usuarioLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            usuarioLayout.Location = new System.Drawing.Point(0, 0);
            usuarioLayout.Name = "usuarioLayout";
            usuarioLayout.RowCount = 6;
            usuarioLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            usuarioLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            usuarioLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            usuarioLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            usuarioLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            usuarioLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            usuarioLayout.Size = new System.Drawing.Size(623, 600);
            usuarioLayout.TabIndex = 0;
            // 
            // lblPersona
            // 
            lblPersona.Anchor = System.Windows.Forms.AnchorStyles.Right;
            lblPersona.AutoSize = true;
            lblPersona.Location = new System.Drawing.Point(141, 3);
            lblPersona.Name = "lblPersona";
            lblPersona.Size = new System.Drawing.Size(74, 23);
            lblPersona.TabIndex = 0;
            lblPersona.Text = "Persona:";
            // 
            // cbxPersona
            // 
            cbxPersona.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            cbxPersona.Location = new System.Drawing.Point(221, 3);
            cbxPersona.Name = "cbxPersona";
            cbxPersona.Size = new System.Drawing.Size(121, 31);
            cbxPersona.TabIndex = 1;
            // 
            // lblUsuario
            // 
            lblUsuario.Anchor = System.Windows.Forms.AnchorStyles.Right;
            lblUsuario.AutoSize = true;
            lblUsuario.Location = new System.Drawing.Point(143, 33);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new System.Drawing.Size(72, 23);
            lblUsuario.TabIndex = 2;
            lblUsuario.Text = "Usuario:";
            // 
            // txtUsuario
            // 
            txtUsuario.BackColor = System.Drawing.Color.FromArgb(40, 40, 56);
            txtUsuario.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtUsuario.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtUsuario.ForeColor = System.Drawing.Color.White;
            txtUsuario.Location = new System.Drawing.Point(221, 33);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new System.Drawing.Size(100, 23);
            txtUsuario.TabIndex = 3;
            // 
            // lblPassword
            // 
            lblPassword.Anchor = System.Windows.Forms.AnchorStyles.Right;
            lblPassword.AutoSize = true;
            lblPassword.Location = new System.Drawing.Point(114, 63);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new System.Drawing.Size(101, 23);
            lblPassword.TabIndex = 4;
            lblPassword.Text = "Contraseña:";
            // 
            // txtPassword
            // 
            txtPassword.BackColor = System.Drawing.Color.FromArgb(40, 40, 56);
            txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtPassword.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtPassword.ForeColor = System.Drawing.Color.White;
            txtPassword.Location = new System.Drawing.Point(221, 63);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new System.Drawing.Size(100, 23);
            txtPassword.TabIndex = 5;
            // 
            // lblRolUsuario
            // 
            lblRolUsuario.Anchor = System.Windows.Forms.AnchorStyles.Right;
            lblRolUsuario.AutoSize = true;
            lblRolUsuario.Location = new System.Drawing.Point(177, 93);
            lblRolUsuario.Name = "lblRolUsuario";
            lblRolUsuario.Size = new System.Drawing.Size(38, 23);
            lblRolUsuario.TabIndex = 6;
            lblRolUsuario.Text = "Rol:";
            // 
            // cbxRolUsuario
            // 
            cbxRolUsuario.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            cbxRolUsuario.Location = new System.Drawing.Point(221, 93);
            cbxRolUsuario.Name = "cbxRolUsuario";
            cbxRolUsuario.Size = new System.Drawing.Size(121, 31);
            cbxRolUsuario.TabIndex = 7;
            // 
            // btnCrearUsuario
            // 
            btnCrearUsuario.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnCrearUsuario.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnCrearUsuario.ForeColor = System.Drawing.Color.White;
            btnCrearUsuario.Location = new System.Drawing.Point(221, 123);
            btnCrearUsuario.Name = "btnCrearUsuario";
            btnCrearUsuario.Size = new System.Drawing.Size(150, 24);
            btnCrearUsuario.TabIndex = 8;
            btnCrearUsuario.Text = "Crear Usuario";
            // 
            // panelGestionUsuarios
            // 
            panelGestionUsuarios.Controls.Add(gestionUsuariosLayout);
            panelGestionUsuarios.Dock = System.Windows.Forms.DockStyle.Fill;
            panelGestionUsuarios.Location = new System.Drawing.Point(0, 0);
            panelGestionUsuarios.Name = "panelGestionUsuarios";
            panelGestionUsuarios.Size = new System.Drawing.Size(623, 600);
            panelGestionUsuarios.TabIndex = 2;
            panelGestionUsuarios.Visible = false;
            // 
            // gestionUsuariosLayout
            // 
            gestionUsuariosLayout.BackColor = System.Drawing.Color.Transparent;
            gestionUsuariosLayout.ColumnCount = 3;
            gestionUsuariosLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            gestionUsuariosLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            gestionUsuariosLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 220F));
            gestionUsuariosLayout.Controls.Add(lblBuscarUsuario, 0, 0);
            gestionUsuariosLayout.Controls.Add(txtBuscarUsuario, 1, 0);
            gestionUsuariosLayout.Controls.Add(dgvUsuarios, 0, 1);
            gestionUsuariosLayout.Controls.Add(lblFechaExpiracionGestion, 0, 2);
            gestionUsuariosLayout.Controls.Add(dtpFechaExpiracionGestion, 1, 2);
            gestionUsuariosLayout.Controls.Add(buttonPanel, 0, 3);
            gestionUsuariosLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            gestionUsuariosLayout.Location = new System.Drawing.Point(0, 0);
            gestionUsuariosLayout.Name = "gestionUsuariosLayout";
            gestionUsuariosLayout.RowCount = 4;
            gestionUsuariosLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            gestionUsuariosLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            gestionUsuariosLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            gestionUsuariosLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            gestionUsuariosLayout.Size = new System.Drawing.Size(623, 600);
            gestionUsuariosLayout.TabIndex = 0;
            // 
            //
            // lblBuscarUsuario
            //
            lblBuscarUsuario.Anchor = System.Windows.Forms.AnchorStyles.Right;
            lblBuscarUsuario.AutoSize = true;
            lblBuscarUsuario.ForeColor = System.Drawing.Color.White;
            lblBuscarUsuario.Location = new System.Drawing.Point(38, 8);
            lblBuscarUsuario.Name = "lblBuscarUsuario";
            lblBuscarUsuario.Size = new System.Drawing.Size(59, 23);
            lblBuscarUsuario.TabIndex = 4;
            lblBuscarUsuario.Text = "Buscar:";
            //
            // txtBuscarUsuario
            //
            txtBuscarUsuario.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtBuscarUsuario.BackColor = System.Drawing.SystemColors.Window;
            txtBuscarUsuario.BorderStyle = System.Windows.Forms.BorderStyle.None;
            gestionUsuariosLayout.SetColumnSpan(txtBuscarUsuario, 2);
            txtBuscarUsuario.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtBuscarUsuario.ForeColor = System.Drawing.SystemColors.ControlText;
            txtBuscarUsuario.Location = new System.Drawing.Point(103, 8);
            txtBuscarUsuario.Name = "txtBuscarUsuario";
            txtBuscarUsuario.Size = new System.Drawing.Size(517, 23);
            txtBuscarUsuario.TabIndex = 5;
            //
            // dgvUsuarios
            // 
            dgvUsuarios.BorderStyle = System.Windows.Forms.BorderStyle.None;
            gestionUsuariosLayout.SetColumnSpan(dgvUsuarios, 3);
            dgvUsuarios.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dgvUsuarios.ColumnHeadersHeight = 29;
            gestionUsuariosLayout.SetColumnSpan(dgvUsuarios, 3);
            dgvUsuarios.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvUsuarios.EnableHeadersVisualStyles = false;
            dgvUsuarios.Location = new System.Drawing.Point(3, 3);
            dgvUsuarios.Name = "dgvUsuarios";
            dgvUsuarios.RowHeadersWidth = 51;
            dgvUsuarios.Size = new System.Drawing.Size(617, 421);
            dgvUsuarios.TabIndex = 0;
            // 
            // lblFechaExpiracionGestion
            // 
            lblFechaExpiracionGestion.Anchor = System.Windows.Forms.AnchorStyles.Right;
            lblFechaExpiracionGestion.AutoSize = true;
            lblFechaExpiracionGestion.Location = new System.Drawing.Point(63, 430);
            lblFechaExpiracionGestion.Name = "lblFechaExpiracionGestion";
            lblFechaExpiracionGestion.Size = new System.Drawing.Size(141, 23);
            lblFechaExpiracionGestion.TabIndex = 2;
            lblFechaExpiracionGestion.Text = "Fecha Expiración:";
            // 
            // dtpFechaExpiracionGestion
            // 
            dtpFechaExpiracionGestion.Anchor = System.Windows.Forms.AnchorStyles.Left;
            dtpFechaExpiracionGestion.Location = new System.Drawing.Point(210, 430);
            dtpFechaExpiracionGestion.Name = "dtpFechaExpiracionGestion";
            dtpFechaExpiracionGestion.Size = new System.Drawing.Size(200, 30);
            dtpFechaExpiracionGestion.TabIndex = 3;
            // 
            // buttonPanel
            // 
            buttonPanel.BackColor = System.Drawing.Color.Transparent;
            gestionUsuariosLayout.SetColumnSpan(buttonPanel, 3);
            buttonPanel.Controls.Add(btnEliminarUsuario);
            buttonPanel.Controls.Add(btnGuardarCambios);
            buttonPanel.Controls.Add(btnRefrescarUsuarios);
            buttonPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonPanel.Location = new System.Drawing.Point(3, 460);
            buttonPanel.Name = "buttonPanel";
            buttonPanel.Size = new System.Drawing.Size(617, 137);
            buttonPanel.TabIndex = 1;
            // 
            // btnEliminarUsuario
            // 
            btnEliminarUsuario.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnEliminarUsuario.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnEliminarUsuario.ForeColor = System.Drawing.Color.White;
            btnEliminarUsuario.Location = new System.Drawing.Point(3, 3);
            btnEliminarUsuario.Name = "btnEliminarUsuario";
            btnEliminarUsuario.Size = new System.Drawing.Size(120, 35);
            btnEliminarUsuario.TabIndex = 0;
            btnEliminarUsuario.Text = "Eliminar Usuario";
            // 
            // btnGuardarCambios
            // 
            btnGuardarCambios.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnGuardarCambios.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnGuardarCambios.ForeColor = System.Drawing.Color.White;
            btnGuardarCambios.Location = new System.Drawing.Point(129, 3);
            btnGuardarCambios.Name = "btnGuardarCambios";
            btnGuardarCambios.Size = new System.Drawing.Size(120, 35);
            btnGuardarCambios.TabIndex = 1;
            btnGuardarCambios.Text = "Guardar Cambios";
            // 
            // btnRefrescarUsuarios
            // 
            btnRefrescarUsuarios.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnRefrescarUsuarios.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnRefrescarUsuarios.ForeColor = System.Drawing.Color.White;
            btnRefrescarUsuarios.Location = new System.Drawing.Point(255, 3);
            btnRefrescarUsuarios.Name = "btnRefrescarUsuarios";
            btnRefrescarUsuarios.Size = new System.Drawing.Size(120, 35);
            btnRefrescarUsuarios.TabIndex = 2;
            btnRefrescarUsuarios.Text = "Refrescar";
            // 
            // panelGestionPersonas
            //
            panelGestionPersonas.Controls.Add(gestionPersonasLayout);
            panelGestionPersonas.Dock = System.Windows.Forms.DockStyle.Fill;
            panelGestionPersonas.Location = new System.Drawing.Point(0, 0);
            panelGestionPersonas.Name = "panelGestionPersonas";
            panelGestionPersonas.Size = new System.Drawing.Size(623, 600);
            panelGestionPersonas.TabIndex = 4;
            panelGestionPersonas.Visible = false;
            //
            // gestionPersonasLayout
            //
            gestionPersonasLayout.BackColor = System.Drawing.Color.Transparent;
            gestionPersonasLayout.ColumnCount = 3;
            gestionPersonasLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            gestionPersonasLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            gestionPersonasLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 220F));
            gestionPersonasLayout.Controls.Add(lblBuscarPersona, 0, 0);
            gestionPersonasLayout.Controls.Add(txtBuscarPersona, 1, 0);
            gestionPersonasLayout.Controls.Add(dgvPersonas, 0, 1);
            gestionPersonasLayout.Controls.Add(personasButtonPanel, 0, 2);
            gestionPersonasLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            gestionPersonasLayout.Location = new System.Drawing.Point(0, 0);
            gestionPersonasLayout.Name = "gestionPersonasLayout";
            gestionPersonasLayout.RowCount = 3;
            gestionPersonasLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            gestionPersonasLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            gestionPersonasLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            gestionPersonasLayout.Size = new System.Drawing.Size(623, 600);
            gestionPersonasLayout.TabIndex = 0;
            //
            //
            // lblBuscarPersona
            //
            lblBuscarPersona.Anchor = System.Windows.Forms.AnchorStyles.Right;
            lblBuscarPersona.AutoSize = true;
            lblBuscarPersona.ForeColor = System.Drawing.Color.White;
            lblBuscarPersona.Location = new System.Drawing.Point(38, 8);
            lblBuscarPersona.Name = "lblBuscarPersona";
            lblBuscarPersona.Size = new System.Drawing.Size(59, 23);
            lblBuscarPersona.TabIndex = 6;
            lblBuscarPersona.Text = "Buscar:";
            //
            // txtBuscarPersona
            //
            txtBuscarPersona.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtBuscarPersona.BackColor = System.Drawing.SystemColors.Window;
            txtBuscarPersona.BorderStyle = System.Windows.Forms.BorderStyle.None;
            gestionPersonasLayout.SetColumnSpan(txtBuscarPersona, 2);
            txtBuscarPersona.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtBuscarPersona.ForeColor = System.Drawing.SystemColors.ControlText;
            txtBuscarPersona.Location = new System.Drawing.Point(103, 8);
            txtBuscarPersona.Name = "txtBuscarPersona";
            txtBuscarPersona.Size = new System.Drawing.Size(517, 23);
            txtBuscarPersona.TabIndex = 7;
            //
            // dgvPersonas
            //
            dgvPersonas.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgvPersonas.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            gestionPersonasLayout.SetColumnSpan(dgvPersonas, 3);
            dgvPersonas.ColumnHeadersHeight = 29;
            dgvPersonas.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvPersonas.EnableHeadersVisualStyles = false;
            dgvPersonas.Location = new System.Drawing.Point(3, 43);
            dgvPersonas.Name = "dgvPersonas";
            dgvPersonas.RowHeadersWidth = 51;
            dgvPersonas.Size = new System.Drawing.Size(617, 504);
            dgvPersonas.TabIndex = 0;
            //
            // personasButtonPanel
            //
            personasButtonPanel.BackColor = System.Drawing.Color.Transparent;
            gestionPersonasLayout.SetColumnSpan(personasButtonPanel, 3);
            personasButtonPanel.Controls.Add(btnEliminarPersona);
            personasButtonPanel.Controls.Add(btnGuardarCambiosPersona);
            personasButtonPanel.Controls.Add(btnRefrescarPersonas);
            personasButtonPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            personasButtonPanel.Location = new System.Drawing.Point(3, 553);
            personasButtonPanel.Name = "personasButtonPanel";
            personasButtonPanel.Size = new System.Drawing.Size(617, 44);
            personasButtonPanel.TabIndex = 1;
            //
            // btnEliminarPersona
            //
            btnEliminarPersona.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnEliminarPersona.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnEliminarPersona.ForeColor = System.Drawing.Color.White;
            btnEliminarPersona.Location = new System.Drawing.Point(3, 3);
            btnEliminarPersona.Name = "btnEliminarPersona";
            btnEliminarPersona.Size = new System.Drawing.Size(140, 35);
            btnEliminarPersona.TabIndex = 0;
            btnEliminarPersona.Text = "Eliminar Persona";
            //
            // btnGuardarCambiosPersona
            //
            btnGuardarCambiosPersona.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnGuardarCambiosPersona.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnGuardarCambiosPersona.ForeColor = System.Drawing.Color.White;
            btnGuardarCambiosPersona.Location = new System.Drawing.Point(149, 3);
            btnGuardarCambiosPersona.Name = "btnGuardarCambiosPersona";
            btnGuardarCambiosPersona.Size = new System.Drawing.Size(140, 35);
            btnGuardarCambiosPersona.TabIndex = 1;
            btnGuardarCambiosPersona.Text = "Guardar Cambios";
            //
            // btnRefrescarPersonas
            //
            btnRefrescarPersonas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnRefrescarPersonas.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnRefrescarPersonas.ForeColor = System.Drawing.Color.White;
            btnRefrescarPersonas.Location = new System.Drawing.Point(295, 3);
            btnRefrescarPersonas.Name = "btnRefrescarPersonas";
            btnRefrescarPersonas.Size = new System.Drawing.Size(120, 35);
            btnRefrescarPersonas.TabIndex = 2;
            btnRefrescarPersonas.Text = "Refrescar";
            //
            // panelConfiguracion
            // 
            panelConfiguracion.Controls.Add(configuracionLayout);
            panelConfiguracion.Dock = System.Windows.Forms.DockStyle.Fill;
            panelConfiguracion.Location = new System.Drawing.Point(0, 0);
            panelConfiguracion.Name = "panelConfiguracion";
            panelConfiguracion.Size = new System.Drawing.Size(623, 600);
            panelConfiguracion.TabIndex = 3;
            panelConfiguracion.Visible = false;
            // 
            // configuracionLayout
            // 
            configuracionLayout.BackColor = System.Drawing.Color.Transparent;
            configuracionLayout.ColumnCount = 2;
            configuracionLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            configuracionLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            configuracionLayout.Controls.Add(chkMayusculasMinusculas, 0, 0);
            configuracionLayout.Controls.Add(chkNumeros, 0, 1);
            configuracionLayout.Controls.Add(chkCaracteresEspeciales, 0, 2);
            configuracionLayout.Controls.Add(chkDobleFactor, 0, 3);
            configuracionLayout.Controls.Add(chkNoRepetirContrasenas, 0, 4);
            configuracionLayout.Controls.Add(chkVerificarDatosPersonales, 0, 5);
            configuracionLayout.Controls.Add(lblMinCaracteres, 0, 6);
            configuracionLayout.Controls.Add(txtMinCaracteres, 1, 6);
            configuracionLayout.Controls.Add(lblCantPreguntas, 0, 7);
            configuracionLayout.Controls.Add(txtCantPreguntas, 1, 7);
            configuracionLayout.Controls.Add(btnGuardarConfig, 1, 8);
            configuracionLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            configuracionLayout.Location = new System.Drawing.Point(0, 0);
            configuracionLayout.Name = "configuracionLayout";
            configuracionLayout.RowCount = 9;
            configuracionLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            configuracionLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            configuracionLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            configuracionLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            configuracionLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            configuracionLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            configuracionLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            configuracionLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            configuracionLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            configuracionLayout.Size = new System.Drawing.Size(623, 600);
            configuracionLayout.TabIndex = 0;
            // 
            // chkMayusculasMinusculas
            // 
            chkMayusculasMinusculas.Location = new System.Drawing.Point(3, 3);
            chkMayusculasMinusculas.Name = "chkMayusculasMinusculas";
            chkMayusculasMinusculas.Size = new System.Drawing.Size(271, 24);
            chkMayusculasMinusculas.TabIndex = 0;
            chkMayusculasMinusculas.Text = "Combinar mayúsculas y minúsculas";
            // 
            // chkNumeros
            // 
            chkNumeros.Location = new System.Drawing.Point(3, 33);
            chkNumeros.Name = "chkNumeros";
            chkNumeros.Size = new System.Drawing.Size(271, 24);
            chkNumeros.TabIndex = 1;
            chkNumeros.Text = "Requerir números";
            // 
            // chkCaracteresEspeciales
            // 
            chkCaracteresEspeciales.Location = new System.Drawing.Point(3, 63);
            chkCaracteresEspeciales.Name = "chkCaracteresEspeciales";
            chkCaracteresEspeciales.Size = new System.Drawing.Size(271, 24);
            chkCaracteresEspeciales.TabIndex = 2;
            chkCaracteresEspeciales.Text = "Requerir caracteres especiales";
            // 
            // chkDobleFactor
            // 
            chkDobleFactor.Location = new System.Drawing.Point(3, 93);
            chkDobleFactor.Name = "chkDobleFactor";
            chkDobleFactor.Size = new System.Drawing.Size(271, 24);
            chkDobleFactor.TabIndex = 3;
            chkDobleFactor.Text = "Usar doble factor";
            // 
            // chkNoRepetirContrasenas
            // 
            chkNoRepetirContrasenas.Location = new System.Drawing.Point(3, 123);
            chkNoRepetirContrasenas.Name = "chkNoRepetirContrasenas";
            chkNoRepetirContrasenas.Size = new System.Drawing.Size(271, 24);
            chkNoRepetirContrasenas.TabIndex = 4;
            chkNoRepetirContrasenas.Text = "No repetir contraseñas anteriores";
            // 
            // chkVerificarDatosPersonales
            // 
            chkVerificarDatosPersonales.Location = new System.Drawing.Point(3, 153);
            chkVerificarDatosPersonales.Name = "chkVerificarDatosPersonales";
            chkVerificarDatosPersonales.Size = new System.Drawing.Size(271, 24);
            chkVerificarDatosPersonales.TabIndex = 5;
            chkVerificarDatosPersonales.Text = "Verificar datos personales";
            // 
            // lblMinCaracteres
            // 
            lblMinCaracteres.Anchor = System.Windows.Forms.AnchorStyles.Right;
            lblMinCaracteres.AutoSize = true;
            lblMinCaracteres.Location = new System.Drawing.Point(255, 183);
            lblMinCaracteres.Name = "lblMinCaracteres";
            lblMinCaracteres.Size = new System.Drawing.Size(178, 23);
            lblMinCaracteres.TabIndex = 6;
            lblMinCaracteres.Text = "Mínimo de caracteres:";
            // 
            // txtMinCaracteres
            // 
            txtMinCaracteres.BackColor = System.Drawing.Color.FromArgb(40, 40, 56);
            txtMinCaracteres.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtMinCaracteres.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtMinCaracteres.ForeColor = System.Drawing.Color.White;
            txtMinCaracteres.Location = new System.Drawing.Point(439, 183);
            txtMinCaracteres.Name = "txtMinCaracteres";
            txtMinCaracteres.Size = new System.Drawing.Size(100, 23);
            txtMinCaracteres.TabIndex = 7;
            // 
            // lblCantPreguntas
            // 
            lblCantPreguntas.Anchor = System.Windows.Forms.AnchorStyles.Right;
            lblCantPreguntas.AutoSize = true;
            lblCantPreguntas.Location = new System.Drawing.Point(140, 213);
            lblCantPreguntas.Name = "lblCantPreguntas";
            lblCantPreguntas.Size = new System.Drawing.Size(293, 23);
            lblCantPreguntas.TabIndex = 8;
            lblCantPreguntas.Text = "Cantidad de preguntas de seguridad:";
            // 
            // txtCantPreguntas
            // 
            txtCantPreguntas.BackColor = System.Drawing.Color.FromArgb(40, 40, 56);
            txtCantPreguntas.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtCantPreguntas.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtCantPreguntas.ForeColor = System.Drawing.Color.White;
            txtCantPreguntas.Location = new System.Drawing.Point(439, 213);
            txtCantPreguntas.Name = "txtCantPreguntas";
            txtCantPreguntas.Size = new System.Drawing.Size(100, 23);
            txtCantPreguntas.TabIndex = 9;
            // 
            // btnGuardarConfig
            // 
            btnGuardarConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnGuardarConfig.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnGuardarConfig.ForeColor = System.Drawing.Color.White;
            btnGuardarConfig.Location = new System.Drawing.Point(439, 243);
            btnGuardarConfig.Name = "btnGuardarConfig";
            btnGuardarConfig.Size = new System.Drawing.Size(120, 35);
            btnGuardarConfig.TabIndex = 10;
            btnGuardarConfig.Text = "Guardar";
            // 
            // iconPictureBox1
            // 
            iconPictureBox1.BackColor = System.Drawing.Color.FromArgb(43, 47, 49);
            iconPictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            iconPictureBox1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            iconPictureBox1.IconChar = FontAwesome.Sharp.IconChar.ClipboardQuestion;
            iconPictureBox1.IconColor = System.Drawing.SystemColors.ControlLightLight;
            iconPictureBox1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            iconPictureBox1.IconSize = 125;
            iconPictureBox1.Location = new System.Drawing.Point(498, 3);
            iconPictureBox1.Name = "iconPictureBox1";
            iconPictureBox1.Size = new System.Drawing.Size(125, 174);
            iconPictureBox1.TabIndex = 1;
            iconPictureBox1.TabStop = false;
            // 
            // AdminForm
            // 
            ClientSize = new System.Drawing.Size(823, 600);
            Controls.Add(contentPanel);
            Controls.Add(navigationPanel);
            Font = new System.Drawing.Font("Segoe UI", 10F);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AdminForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Panel de Administración";
            navigationPanel.ResumeLayout(false);
            contentPanel.ResumeLayout(false);
            panelPersonas.ResumeLayout(false);
            personaLayout.ResumeLayout(false);
            personaLayout.PerformLayout();
            panelUsuarios.ResumeLayout(false);
            usuarioLayout.ResumeLayout(false);
            usuarioLayout.PerformLayout();
            panelGestionUsuarios.ResumeLayout(false);
            gestionUsuariosLayout.ResumeLayout(false);
            gestionUsuariosLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).EndInit();
            buttonPanel.ResumeLayout(false);
            panelGestionPersonas.ResumeLayout(false);
            gestionPersonasLayout.ResumeLayout(false);
            personasButtonPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvPersonas).EndInit();
            panelConfiguracion.ResumeLayout(false);
            configuracionLayout.ResumeLayout(false);
            configuracionLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)iconMiPerfil).EndInit();
            ResumeLayout(false);
        }
        private System.Windows.Forms.FlowLayoutPanel buttonPanel;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox1;
    }
}