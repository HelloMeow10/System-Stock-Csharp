using System;
using System.Windows.Forms;
using Contracts;
using Microsoft.Extensions.DependencyInjection;
using Presentation.ApiClient;

namespace Presentation
{
    public partial class LoginForm : Form
    {
        private readonly ApiClient.ApiClient _apiClient;
        private readonly IServiceProvider _serviceProvider;

        public LoginForm(ApiClient.ApiClient apiClient, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _apiClient = apiClient;
            _serviceProvider = serviceProvider;
            btnLogin.Click += BtnLogin_Click;
            btnRecuperarContrasena.Click += BtnRecuperarContrasena_Click;
        }

        private async void BtnLogin_Click(object? sender, EventArgs? e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) ||
                string.IsNullOrWhiteSpace(txtContrasena.Text))
            {
                MessageBox.Show("Por favor, ingrese usuario y contraseña.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string username = txtUsuario.Text.Trim();
            string password = txtContrasena.Text.Trim();

            try
            {
                var loginRequest = new LoginRequest { Username = username, Password = password };
                var loginResponse = await _apiClient.LoginAsync(loginRequest);

                if (loginResponse.Requires2fa)
                {
                    this.Hide();
                    using (var twoFaForm = _serviceProvider.GetRequiredService<TwoFactorAuthForm>())
                    {
                        twoFaForm.Username = username; // Pass username to the 2FA form
                        if (twoFaForm.ShowDialog() == DialogResult.OK)
                        {
                            // After successful 2FA, the 2FA form will handle showing the dashboard.
                            // We just need to make sure the login form stays hidden.
                            this.DialogResult = DialogResult.OK;
                        }
                        else
                        {
                            // If 2FA is cancelled or fails, show the login form again.
                            this.Show();
                        }
                    }
                }
                else
                {
                    // No 2FA required, proceed to show dashboard
                    if (loginResponse.Rol.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                    {
                        ShowDashboard(_serviceProvider.GetRequiredService<AdminForm>(), loginResponse.Username);
                    }
                    else
                    {
                        ShowDashboard(_serviceProvider.GetRequiredService<UserForm>(), loginResponse.Username);
                    }
                }
            }
            catch (ApiException apiEx)
            {
                MessageBox.Show($"Error de autenticación: {apiEx.Message}", "Error de API", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error inesperado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowDashboard(Form dashboard, string username)
        {
            this.Hide();
            if (dashboard is AdminForm adminForm)
            {
                adminForm.Initialize(username);
            }
            else if (dashboard is UserForm userForm)
            {
                userForm.Initialize(username);
            }

            dashboard.FormClosed += (s, args) => {
                this.txtContrasena.Clear();
                this.txtUsuario.Clear();
                this.Show();
            };
            dashboard.Show();
        }

        private void BtnRecuperarContrasena_Click(object? sender, EventArgs e)
        {
            using (var form = _serviceProvider.GetRequiredService<RecuperarContrasenaForm>())
            {
                form.ShowDialog();
            }
        }

        private void ChkMostrarContrasena_CheckedChanged(object? sender, EventArgs e)
        {
            if (chkMostrarContrasena.Checked)
            {
                txtContrasena.PasswordChar = '\0'; // Show password
            }
            else
            {
                txtContrasena.PasswordChar = '●'; // Hide password
            }
        }

        private void tableLayoutPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void iconPictureBox5_Click(object sender, EventArgs e)
        {
         this.Close(); // Close the form when the icon is clicked   
        }
    }
}