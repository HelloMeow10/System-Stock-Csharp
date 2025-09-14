using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Contracts;
using Presentation.Controles;
using Presentation.Theme;
using Presentation.ApiClient;

namespace Presentation
{
    public partial class RecuperarContrasenaForm : Form
    {
        private readonly ApiClient.ApiClient _apiClient;
        private List<PreguntaSeguridadDto> _preguntasUsuario;

        public RecuperarContrasenaForm(ApiClient.ApiClient apiClient)
        {
            InitializeComponent();
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            _preguntasUsuario = new List<PreguntaSeguridadDto>();

            // Wire up events
            btnContinuar.Click += BtnContinuar_Click;
            btnRecuperar.Click += BtnRecuperar_Click;
        }

        private async void BtnContinuar_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtUsuario.Text))
                {
                    MessageBox.Show("Por favor, ingrese un nombre de usuario.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var usuario = txtUsuario.Text.Trim();
                _preguntasUsuario = await _apiClient.GetUserSecurityQuestionsAsync(usuario);

                if (_preguntasUsuario != null && _preguntasUsuario.Count > 0)
                {
                    MostrarPreguntas(_preguntasUsuario);
                    grpPreguntas.Visible = true;
                    btnRecuperar.Visible = true;
                    grpUsuario.Enabled = false; // Prevent user from changing username
                }
                else
                {
                    MessageBox.Show("El usuario no existe o no tiene preguntas de seguridad configuradas.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LimpiarPreguntas();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Se produjo un error al cargar las preguntas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LimpiarPreguntas();
            }
        }

        private void MostrarPreguntas(List<PreguntaSeguridadDto> preguntas)
        {
            pnlPreguntas.Controls.Clear(); // Limpiar el panel
            pnlPreguntas.SuspendLayout(); // Suspender el layout para mejorar el rendimiento

            try
            {
                foreach (var pregunta in preguntas)
                {
                    var label = new Label
                    {
                        Text = pregunta.Pregunta,
                        Font = new Font("Segoe UI", 9.75F, FontStyle.Regular),
                        AutoSize = true,
                        Margin = new Padding(3, 5, 3, 3),
                        ForeColor = ThemeColors.TextPrimary
                    };

                    var textBox = new RoundedTextBox
                    {
                        Width = pnlPreguntas.Width - 25, // Ajustar al ancho del panel
                        Tag = pregunta.IdPregunta, // Guardar el ID de la pregunta
                        Margin = new Padding(3, 3, 3, 10)
                    };

                    pnlPreguntas.Controls.Add(label);
                    pnlPreguntas.Controls.Add(textBox);
                }
            }
            finally
            {
                pnlPreguntas.ResumeLayout(true);
            }
        }

        private void LimpiarPreguntas()
        {
            _preguntasUsuario.Clear();
            pnlPreguntas.Controls.Clear();
            grpPreguntas.Visible = false;
            btnRecuperar.Visible = false;
            grpUsuario.Enabled = true;
        }

        private async void BtnRecuperar_Click(object? sender, EventArgs e)
        {
            var respuestas = new Dictionary<int, string>();
            foreach (Control control in pnlPreguntas.Controls)
            {
                if (control is RoundedTextBox textBox)
                {
                    if (string.IsNullOrWhiteSpace(textBox.Text))
                    {
                        MessageBox.Show("Por favor, responda todas las preguntas de seguridad.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (textBox.Tag is int idPregunta)
                    {
                        respuestas.Add(idPregunta, textBox.Text.Trim());
                    }
                }
            }

            if (respuestas.Count != _preguntasUsuario.Count)
            {
                MessageBox.Show("No se pudieron recolectar todas las respuestas. Intente de nuevo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnRecuperar.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            try
            {
                string usuario = txtUsuario.Text.Trim();
                var request = new RecoverPasswordRequest
                {
                    Username = usuario,
                    Answers = respuestas
                };
                await _apiClient.RecoverPasswordAsync(request);

                MessageBox.Show("Si las respuestas proporcionadas son correctas, se ha enviado una nueva contraseña a su dirección de correo electrónico.", "Recuperación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Se produjo un error inesperado durante la recuperación: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnRecuperar.Enabled = true;
                this.Cursor = Cursors.Default;
            }
        }

        private void iconPictureBox5_Click(object sender, EventArgs e)
        {
            this.Close();   
        }
    }
}
