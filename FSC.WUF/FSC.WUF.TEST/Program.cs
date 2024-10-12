using System.Reflection;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FSC.WUF.TEST
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var window = WindowManager.Create((WindowManager window) => Run(window), new Size(600, 500));

            Application application = new Application();
            application.Run();
        }

        static Action<WindowManager> Run = (WindowManager window) =>
        {
            var html = new Html();
            html.Load("index.html");
            //html.ForEachBinding(new People());

            window.ShowIcon = Visibility.Collapsed;
            window.ResizeMode = ResizeMode.CanResize;
            window.Background = new SolidColorBrush(Color.FromRgb(33, 37, 41));
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            window.Titlebar = new WindowTitlebar
            {
                Background = new SolidColorBrush(Color.FromRgb(33, 37, 41)),
                Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255))
            };

            ImageSource? icon = GetIconFromResource("FSC.WUF.TEST.icon.ico");
            if (icon is not null)
            {
                window.Icon = icon;
            }

            window.Load(html);

            window.OnLoaded += async (s, e) =>
            {
                var canvas = window.GetElement("canvas");
                var context = canvas.GetContext(ContextType.Context2D);

                // Height of each stripe (integer)
                int stripeHeight = 83;

                // Black (top stripe)
                await context.SetFillStyle("#000000");  // Black
                await context.FillRect(0, 0, 250, stripeHeight);

                // Red (middle stripe)
                await context.SetFillStyle("#FF0000");  // Red
                await context.FillRect(0, stripeHeight, 250, stripeHeight);

                // Gold (bottom stripe, fills the remaining space)
                await context.SetFillStyle("#FFCC00");  // Gold
                await context.FillRect(0, stripeHeight * 2, 250, 250 - stripeHeight * 2);

                // Text "FSC.WUF" in the center
                await context.SetFont("bold 30px Arial");
                await context.SetTextAlign(TextAlign.Center);
                await context.SetTextBaseline(TextBaseline.Middle);
                await context.SetFillStyle("#000000");  // Black text
                await context.FillText("FSC.WUF", 250 / 2, 250 / 2);

                // Count test for fixing count method
                //var countElements = await window.GetElement("body").Count();
                /*var test = window.GetElement("body");
                await test.Test();*/

                //var people = new List<Person>();

                //var person1 = new Person
                //{
                //    Id = 0,
                //    FirstName = "Jack🤔",
                //    LastName = "Exampleman",
                //    Age = 30,
                //};

                //var person2 = new Person
                //{
                //    Id = 1,
                //    FirstName = "Timmy",
                //    LastName = "Lalala",
                //    Age = 16,
                //};

                //var person3 = new Person
                //{
                //    Id = 2,
                //    FirstName = "Cindy",
                //    LastName = "Stone",
                //    Age = 25,
                //};

                //people.Add(person1);
                //people.Add(person2);
                //people.Add(person3);

                //var html = new Html();
                //html.Load("table_row.html");
                //html.Bind(people);

                //await window.GetElement(".people").Append(html);
            };
        };

        static ImageSource? GetIconFromResource(string ico)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream(ico);

            if (stream is not null)
            {
                var icon = new System.Drawing.Icon(stream);

                var bitmapSource = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

                return bitmapSource;
            }

            return null;
        }
    }
}

