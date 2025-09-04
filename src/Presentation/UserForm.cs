using System;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using Presentation.ApiClient;

namespace Presentation
{
    public partial class UserForm : Form
    {
        private readonly ApiClient.ApiClient _apiClient;
        private readonly IServiceProvider _serviceProvider;
        private string _username = string.Empty;

        public UserForm(
            ApiClient.ApiClient apiClient,
            IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _apiClient = apiClient;
            _serviceProvider = serviceProvider;

            btnCambiarContrasena.Click += BtnCambiarContrasena_Click;
            btnCambiarPreguntas.Click += BtnCambiarPreguntas_Click;
            btnMiPerfil.Click += BtnMiPerfil_Click;
        }

        public void Initialize(string username)
        {
            _username = username;
        }

        private void BtnCambiarContrasena_Click(object? sender, EventArgs e)
        {
            using (var form = _serviceProvider.GetRequiredService<CambioContrasenaForm>())
            {
                form.Initialize(_username);
                form.ShowDialog();
            }
        }

        private void BtnCambiarPreguntas_Click(object? sender, EventArgs e)
        {
            using (var form = _serviceProvider.GetRequiredService<PreguntasSeguridadForm>())
            {
                form.Initialize(_username);
                form.ShowDialog();
            }
        }

        private async void BtnMiPerfil_Click(object? sender, EventArgs e)
        {
            var user = await _apiClient.GetUserByUsernameAsync(_username);
            if (user != null)
            {
                var persona = await _apiClient.GetPersonaAsync(user.IdPersona);
                if (persona != null)
                {
                    using (var form = _serviceProvider.GetRequiredService<ProfileForm>())
                    {
                        form.Initialize(user, persona);
                        form.ShowDialog();
                    }
                }
                else
                {
                    MessageBox.Show("No se encontraron los datos de la persona.", "Error");
                }
            }
            else
            {
                MessageBox.Show("No se encontró el usuario.", "Error");
            }
        }

        private void iconPictureBox5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
