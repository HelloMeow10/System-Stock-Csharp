using System;
using System.Windows.Forms;
using Contracts;
using Presentation.ApiClient;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Exceptions;

namespace Presentation
{
    public partial class TwoFactorAuthForm : Form
    {
        private readonly ApiClient.ApiClient _apiClient;
        private readonly IServiceProvider _serviceProvider;
        public string Username { get; set; } = string.Empty;

        public TwoFactorAuthForm(ApiClient.ApiClient apiClient, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _apiClient = apiClient;
            _serviceProvider = serviceProvider;
            btnVerificar.Click += BtnVerificar_Click;
        }

        private async void BtnVerificar_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                MessageBox.Show("Por favor, ingrese el código de verificación.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var request = new Validate2faRequest
                {
                    Username = this.Username,
                    Code = txtCodigo.Text.Trim()
                };
                var loginResponse = await _apiClient.Validate2faAsync(request);

                this.DialogResult = DialogResult.OK;
                this.Hide();

                var nextUsername = loginResponse.Username ?? this.Username;
                try
                {
                    var currentUser = await _apiClient.GetCurrentUserAsync();
                    if (currentUser != null && currentUser.CambioContrasenaObligatorio)
                    {
                        using (var changeForm = _serviceProvider.GetRequiredService<CambioContrasenaForm>())
                        {
                            changeForm.Initialize(nextUsername);
                            var dlg = changeForm.ShowDialog();
                            if (dlg != DialogResult.OK)
                            {
                                // User cancelled or failed to change password; close and return to login
                                this.Close();
                                return;
                            }
                        }
                    }
                }
                catch
                {
                    // If cannot fetch current user, continue
                }

                var userRole = loginResponse.Rol ?? string.Empty;
                if (userRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    ShowDashboard(_serviceProvider.GetRequiredService<AdminForm>(), nextUsername);
                }
                else
                {
                    ShowDashboard(_serviceProvider.GetRequiredService<UserForm>(), nextUsername);
                }

                this.Close();
            }
            catch (ApiException apiEx)
            {
                MessageBox.Show($"Error de verificación: {apiEx.Message}", "Error de API", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error inesperado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowDashboard(Form dashboard, string username)
        {
            if (dashboard is AdminForm adminForm)
            {
                adminForm.Initialize(username);
            }
            else if (dashboard is UserForm userForm)
            {
                userForm.Initialize(username);
            }

            dashboard.FormClosed += (s, args) =>
            {
                // When the dashboard closes, the application should exit or show the login form.
                // For simplicity, we can let the application exit if the login form is not visible.
                Application.Exit();
            };
            dashboard.Show();
        }

        private void iconPictureBox5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
