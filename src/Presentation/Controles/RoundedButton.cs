using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Presentation.Theme;

namespace Presentation.Controles
{
    public class RoundedButton : Button
    {
        // Fields
        private int _borderRadius = 15;
        private Color _baseColor = ThemeColors.Primary;
        private Color _hoverColor = ThemeColors.PrimaryHover;

        private Color _currentColor;
        private Timer _animationTimer;
        private bool _hovering = false;

        // Properties for customization in the designer
        [Category("Appearance")]
        [Description("The radius of the button's corners.")]
        public int BorderRadius
        {
            get => _borderRadius;
            set { _borderRadius = value; Invalidate(); }
        }

        [Category("Appearance")]
        [Description("The base color of the button.")]
        public Color BaseColor
        {
            get => _baseColor;
            set { _baseColor = value; if (!_hovering) { _currentColor = value; } Invalidate(); }
        }

        [Category("Appearance")]
        [Description("The color of the button when hovered.")]
        public Color HoverColor
        {
            get => _hoverColor;
            set { _hoverColor = value; if (_hovering) { _currentColor = value; } Invalidate(); }
        }

        public RoundedButton()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            ForeColor = ThemeColors.TextPrimary;
            Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            _currentColor = _baseColor;

            // Animation setup
            _animationTimer = new Timer { Interval = 15 };
            _animationTimer.Tick += AnimationTick;

            MouseEnter += (s, e) => { _hovering = true; _animationTimer.Start(); };
            MouseLeave += (s, e) => { _hovering = false; _animationTimer.Start(); };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var rect = ClientRectangle;

            int diameter = _borderRadius * 2;

            // To prevent ugly artifacts if the button is smaller than the corner radius
            if (diameter > Math.Min(this.Width, this.Height))
            {
                diameter = Math.Min(this.Width, this.Height);
            }

            using (var path = new GraphicsPath())
            {
                if (diameter > 0)
                {
                    path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
                    path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
                    path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
                    path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
                    path.CloseFigure();
                }
                else // If diameter is 0, draw a rectangle
                {
                    path.AddRectangle(rect);
                }

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                using (var brush = new SolidBrush(_currentColor))
                {
                    e.Graphics.FillPath(brush, path);
                }

                TextRenderer.DrawText(e.Graphics, Text, Font, rect, ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
        }

        private void AnimationTick(object? sender, EventArgs e)
        {
            var targetColor = _hovering ? _hoverColor : _baseColor;

            if (_currentColor == targetColor)
            {
                _animationTimer.Stop();
                return;
            }

            int r = Animate(_currentColor.R, targetColor.R);
            int g = Animate(_currentColor.G, targetColor.G);
            int b = Animate(_currentColor.B, targetColor.B);

            _currentColor = Color.FromArgb(r, g, b);
            Invalidate();
        }

        private int Animate(int current, int target)
        {
            const int step = 20;
            if (Math.Abs(current - target) < step)
            {
                return target;
            }
            return current < target ? current + step : current - step;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_animationTimer != null)
                {
                    _animationTimer.Stop();
                    _animationTimer.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
}