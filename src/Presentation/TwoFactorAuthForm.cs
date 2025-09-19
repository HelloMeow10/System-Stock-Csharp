using System;
using System.Windows.Forms;
using Contracts;
using Presentation.ApiClient;

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

                if (loginResponse.Rol.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    ShowDashboard(_serviceProvider.GetRequiredService<AdminForm>(), loginResponse.Username);
                }
                else
                {
                    ShowDashboard(_serviceProvider.GetRequiredService<UserForm>(), loginResponse.Username);
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
