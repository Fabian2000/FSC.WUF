using System.Windows.Media;

namespace FSC.WUF
{
    public sealed partial class WindowTitlebar
    {
        public WindowTitlebar()
        {

        }

        public bool Enabled { get; init; } = true;

        public Brush Background { get; init; } = new SolidColorBrush(Color.FromRgb(243, 243, 243));

        public Brush Foreground { get; init; } = new SolidColorBrush(Color.FromRgb(0, 0, 0));

        public bool DisableMaximize { get; init; } = false;
    }
}