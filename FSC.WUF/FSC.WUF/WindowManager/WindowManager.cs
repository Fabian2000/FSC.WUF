using System;
using System.Threading.Tasks;
using System.Windows;

namespace FSC.WUF
{
    /// <summary>
    /// The WindowManager is the manager of the whole window that will show up and all its actions
    /// </summary>
    public sealed partial class WindowManager
    {
        /// <summary>
        /// Sets the options for the titlebar. It is not possible to set the properties by themself. On every change, this property wants a new instance
        /// </summary>
        public WindowTitlebar Titlebar
        {
            get => new WindowTitlebar
            {
                Background = _window!.titlebar.Background,
                Foreground = _window!.titlebar.Foreground,
                Enabled = _window!.titlebar.Visibility is Visibility.Visible,
                DisableMaximize = !_window!.titlebar.MaximizeButton.IsEnabled
            };
            set
            {
                var windowTitlebar = new WindowTitlebar();
                _window!.titlebar.Background = value.Background ?? windowTitlebar.Background;
                _window!.titlebar.Foreground = value.Foreground ?? windowTitlebar.Foreground;
                _window!.titlebar.Visibility = value.Enabled ? Visibility.Visible : Visibility.Collapsed;
                _window!.titlebar.MaximizeButton.IsEnabled = !value.DisableMaximize;
            }
        }

        /// <summary>
        /// If true, the popup event will use an internal browser window (edge popup) to open external links
        /// </summary>
        public bool UseInternalPopupManager { get; set; } = false;

        /// <summary>
        /// Creates a new window
        /// </summary>
        /// <param name="run">If the window shows up, this action will get called</param>
        /// <param name="size">Defines the size of the window on startup</param>
        /// <param name="visibility">Defines the visibility of the window on startup. This feature probably doesn't hide the window from beginning. Didn't found out why, probably a bug from WPF. The only method that worked didn't supported the windows 11 rounding</param>
        /// <param name="windowStartupLocation">Sets the startup location</param>
        /// <returns></returns>
        public static WindowManager Create
        (
            Action<WindowManager> run, 
            Size size = new Size(), 
            Visibility visibility = Visibility.Visible, 
            WindowStartupLocation windowStartupLocation = WindowStartupLocation.CenterScreen
        )
        {
            var windowManager = new WindowManager();

            windowManager._window = new WindowTemplate(run, windowManager, () => { windowManager.EventManager(); });

            if (size.IsEmpty)
            {
                size = new Size(1080, 720);
            }

            windowManager.Height = size.Height;
            windowManager.Width = size.Width;
            windowManager.Visibility = visibility;
            windowManager.WindowStartupLocation = windowStartupLocation;

            windowManager.Titlebar = new WindowTitlebar();

            windowManager._window.Show();

            return windowManager;
        }

        /// <summary>
        /// Loads the main html file into the view. This file has to be less than 2MB. To build up the whole page, use InnerHtml, Append or Prepend, after the main html file is loaded successfully
        /// </summary>
        /// <param name="html"></param>
        /// <exception cref="Exception">Invalid html exception</exception>
        public void Load(Html html)
        {
            if (!html.IsValid())
            {
                throw new Exception("Invalid Html");
            }

            _window!.webView.NavigateToString(html.resource);
        }
    }
}
