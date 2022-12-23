using Microsoft.Web.WebView2.Core;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace FSC.WUF
{
    /// <summary>
    /// Interaktionslogik für WindowTemplate.xaml
    /// </summary>
    internal partial class WindowTemplate : Window
    {
        private Action<WindowManager> _run;
        private WindowManager _windowManager;
        private Action _events;
        internal WindowTemplate(Action<WindowManager> run, WindowManager windowManager, Action events)
        {
            InitializeComponent();
            _run = run;
            _windowManager = windowManager;
            _events = events;
        }

        public new string Title
        {
            get
            {
                return base.Title;
            }
            set
            {
                base.Title = value;
                titlebar.TitleBarTitle.Text = base.Title;
            }
        }

        public Visibility ShowTitle
        {
            get
            {
                return titlebar.TitleBarTitle.Visibility;
            }
            set
            {
                titlebar.TitleBarTitle.Visibility = value;
            }
        }

        public new ImageSource Icon
        {
            get
            {
                return base.Icon;
            }
            set
            {
                base.Icon = value;
                titlebar.TitleBarIcon.Source = base.Icon;
            }
        }

        public Visibility ShowIcon
        {
            get
            {
                return titlebar.TitleBarIcon.Visibility;
            }
            set
            {
                titlebar.TitleBarIcon.Visibility = value;
                titlebar.TitleBarTitle.Margin = new Thickness(value == Visibility.Collapsed ? 10 : 4, 3, 0, 0);
            }
        }

        private async void DockWrapper_Loaded(object sender, RoutedEventArgs e)
        {
            DockWrapper.Loaded -= DockWrapper_Loaded;
            _ = await InitializeAsync();

            _events();
            _run(_windowManager);
        }

        internal async Task<bool> InitializeAsync()
        {
            var path = Path.Combine(Path.GetTempPath(), "FSC.WUF");
            var coreWebEnv = await CoreWebView2Environment.CreateAsync(userDataFolder: path);

            await webView.EnsureCoreWebView2Async(coreWebEnv);

            webView.Source = new Uri("about:blank");
            webView.CoreWebView2.Settings.AreDevToolsEnabled = false;
#if DEBUG
            webView.CoreWebView2.Settings.AreDevToolsEnabled = true;
#endif
            webView.CoreWebView2.Settings.IsZoomControlEnabled = false;
            webView.CoreWebView2.Settings.IsPinchZoomEnabled = false;
            webView.CoreWebView2.Settings.IsBuiltInErrorPageEnabled = false;
            webView.CoreWebView2.Settings.IsGeneralAutofillEnabled = false;
            webView.CoreWebView2.Settings.IsPasswordAutosaveEnabled = false;
            webView.CoreWebView2.Settings.IsSwipeNavigationEnabled = false;
            webView.CoreWebView2.Settings.AreBrowserAcceleratorKeysEnabled = false;
            webView.CoreWebView2.Settings.IsStatusBarEnabled = false;
#if DEBUG
            webView.CoreWebView2.Settings.AreBrowserAcceleratorKeysEnabled = true;
#endif

            return true;
        }
    }
}
