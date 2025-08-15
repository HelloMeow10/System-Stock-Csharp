using System;
using System.Windows.Forms;
using Presentation.Theme;

namespace Presentation
{
    public partial class frmNotification : Form
    {
        public frmNotification()
        {
            InitializeComponent();
            this.BackColor = ThemeColors.Danger;
        }

        public void SetMessage(string message)
        {
            lblMessage.Text = message;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
