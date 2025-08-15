using System;
using System.Windows.Forms;
using BusinessLogic.Services;
using BusinessLogic.Models;

namespace Presentation
{
    public partial class TwoFactorAuthForm : Form
    {
        private readonly IAuthenticationService _authService;
        private string _username = string.Empty;

        public AuthenticationResult? AuthResult { get; private set; }

        public TwoFactorAuthForm(IAuthenticationService authService)
        {
            InitializeComponent();
            _authService = authService;
            btnVerificar.Click += BtnVerificar_Click;
        }

        public void Initialize(string username)
        {
            _username = username;
        }

        private async void BtnVerificar_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                MessageBox.Show("Por favor, ingrese el código de verificación.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            AuthResult = await _authService.Validate2faAsync(_username, txtCodigo.Text.Trim());

            if (AuthResult.Success)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(AuthResult.ErrorMessage ?? "Error desconocido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void iconPictureBox5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
