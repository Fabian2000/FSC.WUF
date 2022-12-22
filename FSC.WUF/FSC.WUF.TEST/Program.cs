using System.Windows;
using System.Windows.Media;

namespace FSC.WUF.TEST
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var window = WindowManager.Create((WindowManager window) => Run(window), new Size(300, 300));

            Application application = new Application();
            application.Run();
        }

        static Action<WindowManager> Run = (WindowManager window) =>
        {
            var html = new Html();
            html.Load("index.html");

            window.ShowIcon = Visibility.Collapsed;
            window.ResizeMode = ResizeMode.CanResize;
            window.Background = new SolidColorBrush(Color.FromRgb(33, 37, 41));
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            window.Titlebar = new WindowTitlebar
            {
                Background = new SolidColorBrush(Color.FromRgb(33, 37, 41)),
                Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255))
            };

            window.Load(html);

            window.OnLoaded += async (s, e) =>
            {
                await window.AddEventListener(".btn-primary", "click", async () =>
                {
                    var input = await window.GetElement("input").Value();
                    input = input.Trim('"');

                    if (string.IsNullOrWhiteSpace(input))
                    {
                        MessageBox.Show("Uff, gib erstmal einen Namen ein, ok? ... :/");
                        return;
                    }
                    MessageBox.Show("Hallo " + input);
                });

                await window.AddEventListener(".btn-secondary", "click", () =>
                {
                    MessageBox.Show("Ne, klick wo anders hin ...");
                });
            };
        };
    }
}

