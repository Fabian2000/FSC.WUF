using Microsoft.Web.WebView2.Core;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace FSC.WUF
{
    public partial class WindowManager
    {
        internal bool isWebViewReady = false;

        internal async Task<string> ExecuteScript(string script)
        {
            return await _window!.webView.ExecuteScriptAsync(script);
        }

        internal void EventManager()
        {
            var webview = _window!.webView!;

            webview.NavigationCompleted += Webview_NavigationCompleted;
            webview.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
            webview.CoreWebView2.ContextMenuRequested += CoreWebView2_ContextMenuRequested;
            webview.CoreWebView2.DocumentTitleChanged += CoreWebView2_DocumentTitleChanged;
            webview.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;
        }

        private void CoreWebView2_WebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            var info = e.TryGetWebMessageAsString();

            if (info is not null)
            {
                var message = info.Split(':');
                
                switch (message[0])
                {
                    case "event":
                        _events[Convert.ToInt32(message[1])](_window.GetElement(new Guid(message[2])));
                        break;
                }
            }
        }

        private void CoreWebView2_DocumentTitleChanged(object? sender, object e)
        {
            var title = _window!.webView!.CoreWebView2.DocumentTitle;
            var source = _window!.webView!.CoreWebView2.Source;
            
            if (title == source)
            {
                return;
            }

            if (title.StartsWith("data:", System.StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            Title = title;
        }

        private void CoreWebView2_ContextMenuRequested(object? sender, CoreWebView2ContextMenuRequestedEventArgs e)
        {
            e.MenuItems.Clear();
        }

        private void CoreWebView2_NewWindowRequested(object? sender, CoreWebView2NewWindowRequestedEventArgs e)
        {
            e.Handled = !UseInternalPopupManager;

            OnPopup?.Invoke(this, new WindowManagerLinkEventArgs(e.Uri));
        }

        private async void Webview_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            isWebViewReady = true;

            var bootstrapCss = new Css();
            bootstrapCss.Load("bootstrap.css");
            bootstrapCss.BuildDataUrl("css");

            var bootstrapJs = new Js();
            bootstrapJs.Load("bootstrap.bundle.js");
            bootstrapJs.BuildDataUrl("js");

            var fscWufApiJs = new Js();
            fscWufApiJs.Load("fsc.wuf.api.js");
            fscWufApiJs.BuildDataUrl("js");

            var importBootstrapCss = $@"
                let link = document.createElement('link');
                link.setAttribute('rel', 'stylesheet');
                link.setAttribute('href', '{bootstrapCss.dataURL}');
                document.head.appendChild(link);"
                .ReplaceLineEndings("");

            var importBootstrapJs = $@"
                let script = document.createElement('script');
                script.setAttribute('src', '{bootstrapJs.dataURL}');
                document.head.appendChild(script);"
                .ReplaceLineEndings("");

            var importFscWufApi = $@"
                script = document.createElement('script');
                script.setAttribute('src', '{fscWufApiJs.dataURL}');
                document.head.appendChild(script);"
                .ReplaceLineEndings("");

            var stringForEasierDebug = await ExecuteScript(importBootstrapCss);
            stringForEasierDebug = await ExecuteScript(importBootstrapJs);
            stringForEasierDebug = await ExecuteScript(importFscWufApi);

            OnLoaded?.Invoke(this, new EventArgs());
        }
    }
}
