using System;
using System.Windows.Forms;
using Contracts;
using Presentation.ApiClient;

namespace Presentation
{
    public partial class TwoFactorAuthForm : Form
    {
        private readonly ApiClient.ApiClient _apiClient;
        private string _username = string.Empty;

        public LoginResponse? AuthResult { get; private set; }

        public TwoFactorAuthForm(ApiClient.ApiClient apiClient)
        {
            InitializeComponent();
            _apiClient = apiClient;
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

            try
            {
                var request = new Validate2faRequest
                {
                    Username = _username,
                    Code = txtCodigo.Text.Trim()
                };
                AuthResult = await _apiClient.Validate2faAsync(request);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void iconPictureBox5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
