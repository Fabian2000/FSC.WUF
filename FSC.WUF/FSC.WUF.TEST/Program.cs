using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

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
                var people = new List<Person>();

                var person1 = new Person {
                    Id = 0,
                    FirstName = "Jack",
                    LastName = "Exampleman",
                    Age = 30,
                };

                var person2 = new Person {
                    Id = 1,
                    FirstName = "Timmy",
                    LastName = "Lalala",
                    Age = 16,
                };

                var person3 = new Person {
                    Id = 2,
                    FirstName = "Cindy",
                    LastName = "Stone",
                    Age = 25,
                };

                people.Add(person1);
                people.Add(person2);
                people.Add(person3);

                var html = new Html();
                html.Load("table_row.html");
                html.Bind(people);

                await window.GetElement(".people").Append(html);
            };
        };
    }
}

