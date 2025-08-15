using System.Drawing;

namespace Presentation.Theme
{
    public static class ThemeColors
    {
        // A modern, dark color palette
        public static Color FormBackground { get; } = Color.FromArgb(26, 26, 46);
        public static Color Surface { get; } = Color.FromArgb(31, 41, 55);

        public static Color Primary { get; } = Color.FromArgb(59, 130, 246);
        public static Color PrimaryHover { get; } = Color.FromArgb(96, 165, 250);

        public static Color TextPrimary { get; } = Color.FromArgb(243, 244, 246);
        public static Color TextSecondary { get; } = Color.FromArgb(156, 163, 175);

        public static Color Danger { get; } = Color.FromArgb(239, 68, 68);
        public static Color Success { get; } = Color.FromArgb(16, 185, 129);

        public static Color Border { get; } = Color.FromArgb(75, 85, 99);
    }
}
