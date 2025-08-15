using System;
using System.Windows.Forms;
using BusinessLogic.Services;
using BusinessLogic.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace Presentation
{
    public partial class CambioContrasenaForm : Form
    {
        private readonly IPasswordService _passwordService;
        private readonly ISecurityQuestionService _securityQuestionService;
        private readonly IServiceProvider _serviceProvider;
        private string _username = string.Empty;

        public CambioContrasenaForm(IPasswordService passwordService, ISecurityQuestionService securityQuestionService, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _passwordService = passwordService;
            _securityQuestionService = securityQuestionService;
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

                await _passwordService.CambiarContrasenaAsync(_username, nueva, actual);
                MessageBox.Show("Contraseña cambiada correctamente.", "Info");

                var userQuestions = await _securityQuestionService.GetPreguntasDeUsuarioAsync(_username);
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
            catch (ValidationException ex)
            {
                using (var errorForm = _serviceProvider.GetRequiredService<frmNotification>())
                {
                    errorForm.SetMessage(ex.Message);
                    errorForm.ShowDialog();
                }
            }
            catch (BusinessLogicException ex)
            {
                var errorMessage = new StringBuilder(ex.Message);
                if (ex.InnerException != null)
                {
                    errorMessage.AppendLine();
                    errorMessage.AppendLine($"Detalles: {ex.InnerException.Message}");
                }
                MessageBox.Show(errorMessage.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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