using System;
using System.Windows.Forms;
using BusinessLogic.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Presentation.ApiClient;
using Contracts;

namespace Presentation
{
    public partial class CambioContrasenaForm : Form
    {
        private readonly ApiClient.ApiClient _apiClient;
        private readonly IServiceProvider _serviceProvider;
        private string _username = string.Empty;

        public CambioContrasenaForm(ApiClient.ApiClient apiClient, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _apiClient = apiClient;
            _serviceProvider = serviceProvider;
            btnCambiar.Click += BtnCambiar_Click;
        }

        public void Initialize(string username)
        {
            _username = username;
        }

        private async void BtnCambiar_Click(object? sender, EventArgs? e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtActual.Text) ||
                    string.IsNullOrWhiteSpace(txtNueva.Text) ||
                    string.IsNullOrWhiteSpace(txtRepetir.Text))
                {
                    MessageBox.Show("Por favor, complete todos los campos.", "Error");
                    return;
                }

                string actual = txtActual.Text;
                string nueva = txtNueva.Text;
                string repetir = txtRepetir.Text;

                if (nueva != repetir)
                {
                    MessageBox.Show("Las contraseñas nuevas no coinciden.", "Error");
                    return;
                }

                var request = new ChangePasswordRequest
                {
                    Username = _username,
                    OldPassword = actual,
                    NewPassword = nueva
                };
                await _apiClient.ChangePasswordAsync(request);
                MessageBox.Show("Contraseña cambiada correctamente.", "Info");

                var userQuestions = await _apiClient.GetUserSecurityQuestionsAsync(_username);
                if (userQuestions == null || userQuestions.Count == 0)
                {
                    using (var preguntasForm = _serviceProvider.GetRequiredService<PreguntasSeguridadForm>())
                    {
                        preguntasForm.Initialize(_username);
                        preguntasForm.ShowDialog();
                    }
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inesperado: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void iconPictureBox5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}