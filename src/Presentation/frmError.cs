using System;
using System.Windows.Forms;

namespace Presentation
{
    public partial class frmError : Form
    {
        public frmError()
        {
            InitializeComponent();
        }

        public void SetErrorDetails(string userMessage, string errorId, Exception ex)
        {
            // --- User-Friendly Part ---
            txtMessage.Text = userMessage;

            lblExceptionType.Text = "Error ID:";
            txtExceptionType.Text = errorId;

            // --- Technical Details Part (for developers) ---
            // In a real app, this might be in a collapsible panel.
            // For now, we'll just put the technical info in the stack trace box.
            string technicalDetails = $"Exception Type: {ex.GetType().FullName}\n\n" +
                                      $"Message: {ex.Message}\n\n" +
                                      $"Stack Trace:\n{ex.StackTrace}";
            txtStackTrace.Text = technicalDetails;

            // Optional: Make the stack trace less prominent initially.
            // this.Height = 180; // Start smaller
            // btnShowDetails.Click += (s,e) => { this.Height = 430; };
        }
    }
}
