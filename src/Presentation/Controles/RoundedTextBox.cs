using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Presentation.Theme;

namespace Presentation.Controles
{
    public class RoundedTextBox : TextBox
    {
        // Fields
        private int _borderRadius = 15;
        private Color _borderColor = ThemeColors.Border;
        private Color _focusBorderColor = ThemeColors.Primary;
        private Color _currentBorderColor;

        // Properties
        [Category("Appearance")]
        public int BorderRadius
        {
            get => _borderRadius;
            set { _borderRadius = value; Invalidate(); }
        }

        [Category("Appearance")]
        public Color BorderColor
        {
            get => _borderColor;
            set { _borderColor = value; if (!Focused) _currentBorderColor = value; Invalidate(); }
        }

        [Category("Appearance")]
        public Color FocusBorderColor
        {
            get => _focusBorderColor;
            set { _focusBorderColor = value; if (Focused) _currentBorderColor = value; Invalidate(); }
        }

        // Propiedades para el manejo de contraseñas
        public new char PasswordChar
        {
            get { return base.PasswordChar; }
            set { base.PasswordChar = value; Invalidate(); }
        }

        public new bool UseSystemPasswordChar
        {
            get { return base.UseSystemPasswordChar; }
            set { base.UseSystemPasswordChar = value; Invalidate(); }
        }


        public RoundedTextBox()
        {
            BorderStyle = BorderStyle.None;
            Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            BackColor = ThemeColors.Surface;
            ForeColor = ThemeColors.TextPrimary;
            Padding = new Padding(10, 8, 10, 8);

            _currentBorderColor = _borderColor;

            // Events
            Enter += (s, e) => { _currentBorderColor = _focusBorderColor; this.Invalidate(); };
            Leave += (s, e) => { _currentBorderColor = _borderColor; this.Invalidate(); };
        }

        private GraphicsPath GetRoundedPath(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            int diameter = radius * 2;

            if (diameter > Math.Min(rect.Width, rect.Height))
            {
                diameter = Math.Min(rect.Width, rect.Height);
            }

            if (diameter > 0)
            {
                path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
                path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
                path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
                path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
                path.CloseFigure();
            }
            else
            {
                path.AddRectangle(rect);
            }
            return path;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Parent != null)
            {
                e.Graphics.Clear(Parent.BackColor);
            }
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            using (var path = GetRoundedPath(rect, _borderRadius))
            {
                // Fill the background
                using (var brush = new SolidBrush(this.BackColor))
                {
                    e.Graphics.FillPath(brush, path);
                }

                // Draw the border
                using (var pen = new Pen(_currentBorderColor, 2))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }

            // Draw the text
            string textToRender = this.Text;
            if (this.UseSystemPasswordChar)
            {
                textToRender = new string('●', this.Text.Length);
            }
            else if (this.PasswordChar != '\0')
            {
                textToRender = new string(this.PasswordChar, this.Text.Length);
            }

            TextRenderer.DrawText(e.Graphics, textToRender, Font, ClientRectangle, ForeColor, TextFormatFlags.TextBoxControl);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            // We are hooking into the WM_PAINT message to ensure our custom border is drawn
            // on top of the base control's painting. This is a common technique for customizing
            // controls that don't fully support owner drawing.
            if (m.Msg == 0x000F) // WM_PAINT
            {
                // Create a new Graphics object from the Handle and call OnPaint
                // This ensures we are drawing on the correct device context.
                using (var g = Graphics.FromHwnd(this.Handle))
                {
                    OnPaint(new PaintEventArgs(g, this.ClientRectangle));
                }
            }
        }
    }
}