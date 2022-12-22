using System.Windows.Media;

namespace FSC.WUF
{
    /// <summary>
    /// The titlebar of the window (WindowManager.Create(...))
    /// </summary>
    public sealed partial class WindowTitlebar
    {
    /// <summary>
    /// The titlebar of the window (WindowManager.Create(...))
    /// </summary>
        public WindowTitlebar()
        {

        }

        /// <summary>
        /// If true, the titlebar will show up
        /// </summary>
        public bool Enabled { get; init; } = true;

        /// <summary>
        /// Changes the background color of the titlebar
        /// </summary>
        public Brush Background { get; init; } = new SolidColorBrush(Color.FromRgb(243, 243, 243));

        /// <summary>
        /// Changes the foreground (text) color of the titlebar
        /// </summary>
        public Brush Foreground { get; init; } = new SolidColorBrush(Color.FromRgb(0, 0, 0));

        /// <summary>
        /// Disables the maximize functionality
        /// </summary>
        public bool DisableMaximize { get; init; } = false;
    }
}