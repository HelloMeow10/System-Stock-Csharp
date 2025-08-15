using System;
using System.Windows.Forms;
using BusinessLogic.Services;
using BusinessLogic.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation
{
    public partial class LoginForm : Form
    {
        private readonly IAuthenticationService _authService;
        private readonly IServiceProvider _serviceProvider;

        public LoginForm(IAuthenticationService authService, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _authService = authService;
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

            var authResult = await _authService.AuthenticateAsync(username, password);

            if (authResult.Requires2fa)
            {
                using (var twoFactorForm = _serviceProvider.GetRequiredService<TwoFactorAuthForm>())
                {
                    twoFactorForm.Initialize(username);
                    if (twoFactorForm.ShowDialog() == DialogResult.OK)
                    {
                        authResult = twoFactorForm.AuthResult;
                    }
                    else
                    {
                        return; // 2FA cancelled or failed
                    }
                }
            }

            if (authResult == null || !authResult.Success)
            {
                MessageBox.Show(authResult?.ErrorMessage ?? "Ocurrió un error desconocido.", "Error de Autenticación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (authResult.User == null)
            {
                 MessageBox.Show("Ocurrió un error inesperado al obtener los datos del usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 return;
            }

            switch (authResult.NextAction)
            {
                case PostLoginAction.ChangePassword:
                    HandleChangePassword(authResult);
                    break;
                case PostLoginAction.ShowAdminDashboard:
                    ShowDashboard(_serviceProvider.GetRequiredService<AdminForm>(), authResult.User.Username);
                    break;
                case PostLoginAction.ShowUserDashboard:
                    ShowDashboard(_serviceProvider.GetRequiredService<UserForm>(), authResult.User.Username);
                    break;
            }
        }

        private void HandleChangePassword(AuthenticationResult authResult)
        {
            using (var form = _serviceProvider.GetRequiredService<CambioContrasenaForm>())
            {
                form.Initialize(authResult.User.Username);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // After successful password change, show the appropriate dashboard
                    if (authResult.User.Rol.Equals("Administrador", StringComparison.OrdinalIgnoreCase))
                    {
                        ShowDashboard(_serviceProvider.GetRequiredService<AdminForm>(), authResult.User.Username);
                    }
                    else
                    {
                        ShowDashboard(_serviceProvider.GetRequiredService<UserForm>(), authResult.User.Username);
                    }
                }
                else
                {
                    // If password change is cancelled, show login form again
                    this.Show();
                }
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