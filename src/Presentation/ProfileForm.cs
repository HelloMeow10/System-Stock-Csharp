using System;
using System.Windows.Forms;
using Contracts;

namespace Presentation
{
    public partial class ProfileForm : Form
    {
        public ProfileForm()
        {
            InitializeComponent();
        }

        public void Initialize(UserDto user, PersonaDto persona)
        {
            LoadProfileData(user, persona);
        }

        private void LoadProfileData(UserDto user, PersonaDto persona)
        {
            // User data
            lblUsername.Text = user.Username;
            lblRol.Text = user.Rol ?? "N/A";

            // Persona data
            lblNombre.Text = persona.Nombre;
            lblApellido.Text = persona.Apellido;
            lblLegajo.Text = persona.Legajo.ToString();
            lblTipoDoc.Text = persona.TipoDocNombre ?? "N/A";
            lblNumDoc.Text = persona.NumDoc;
            lblFechaNacimiento.Text = persona.FechaNacimiento?.ToString("dd/MM/yyyy") ?? "N/A";
            lblCuil.Text = persona.Cuil;
            lblLocalidad.Text = persona.LocalidadNombre ?? "N/A";
            lblCalle.Text = persona.Calle;
            lblAltura.Text = persona.Altura;
            lblGenero.Text = persona.GeneroNombre ?? "N/A";
            lblCorreo.Text = persona.Correo;
            lblCelular.Text = persona.Celular;
            lblFechaIngreso.Text = persona.FechaIngreso.ToString("dd/MM/yyyy");
        }
    }
}
