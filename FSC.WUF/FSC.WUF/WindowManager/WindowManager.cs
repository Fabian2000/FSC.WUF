using System;
using System.Threading.Tasks;
using System.Windows;

namespace FSC.WUF
{
    public sealed partial class WindowManager
    {
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

        public bool UseInternalPopupManager { get; set; } = false;

        public static WindowManager Create(Action<WindowManager> run)
        {
            return Create(run, new Size(1080, 720));
        }

        public static WindowManager Create(Action<WindowManager> run, Size size)
        {
            var windowManager = new WindowManager();

            windowManager._window = new WindowTemplate(run, windowManager, () => { windowManager.EventManager(); });

            windowManager.Height = size.Height;
            windowManager.Width = size.Width;

            windowManager.Titlebar = new WindowTitlebar();

            windowManager._window.Show();

            return windowManager;
        }

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
