# FSC.WUF
 FSC - Web UI Framework - A simple, but great way to design modern applications for windows in C#

## What is WUF?
WUF is short and means Web UI Framework. This framework is based on WPF and the webview2. This library makes it possible to use HTML and CSS instead of XAML. It is important to not forget, that this is not a website. Yes, you load html and css, but javascript is blocked. Instead of JavaScript, there is a JS - C# bridge with a lot of methods to use. It works really well.

## Easy style - modern style
The library includes bootrap 5 by default. So if you don't have any experience in CSS, you are able to use all bootstrap features.

## Custom CSS support
Yes, custom CSS is supported. It can be used by appending a css resource to the head.

## Loading one site at once
No, this doesn't work. It is different to a normal website. For example the input tags and others that work like that, may not end without a slash.
- <input> wrong.
- <input /> right.
As it is visible, the library has some special rules. Same as loading the "page". You are not able to load one big HTML file. There is a limit of 2 MB. To load bigger content, make an index.html as your default site and load with innerHtml, append or prepend the content into the body or into other elements.

## Reloading? Bye bye
Yes, it's true. What ever happens, this library will not reload the view. Everything works dynamic with C#.
You need a table? Add one per html resource. You want to add a table row? Sure, add it per html resource.
It is very easy to use.

## The program does not work and will there be Html and Css files next to the exe and dll builds?
No, don't worry. You need to set all files that you want to use as an embedded resource. After compiling, they will not be visible from the outside.
There is almost no visible difference to a normal program.

## How to start a project?
It works with every .Net6 and .Net7 project.
The recommend way is the following:
1. Start a .Net6|7 console project
2. Go to the project properties and change the application type from console to windows application
3. Start writing the code ...

## Code example C#
```cs
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace FSC.WUF.TEST
{
    internal class Program
    {
        [STAThread] // <- This is important
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
                        MessageBox.Show("Enter a name please ...");
                        return;
                    }
                    MessageBox.Show("Hello " + input);
                });

                await window.AddEventListener(".btn-secondary", "click", () =>
                {
                    MessageBox.Show("Please use login ...");
                });

                var css = new Css();
                css.Load("test.css");
                await window.GetElement("head").Append(css);

                window.OnPopup += (s, e) => Process.Start(new ProcessStartInfo(e?.Link) { UseShellExecute = true });
            };
        };
    }
}
```

## Code example Html
```html
<!DOCTYPE html>
<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
    <head>
        <meta charset="utf-8" />
        <title></title>
    </head>
    <body class="bg-dark">
        <div class="container">
            <h1 class="text-white h1 mt-3">Login</h1>
            <p class="text text-white">
                Please login to use our services
            </p>
            <!--<img class="img-fluid" src="res://test.png" />--> <!-- Resources can load with a res:// link. To be more exact, it could be res://namespace.test.png -->
            <!--<a class="link" href="test" target="_blank">Test</a>-->
            <form>
                <div class="input-group flex-nowrap my-2">
                    <span class="input-group-text" id="addon-wrapping">@</span>
                    <input type="text" class="form-control" placeholder="Username" aria-label="Username" aria-describedby="addon-wrapping"/>
                </div>
                <div class="input-group flex-nowrap my-2">
                    <span class="input-group-text" id="addon-wrapping">*</span>
                    <input type="password" disabled="disabled" class="form-control" placeholder="Password" aria-label="Username" aria-describedby="addon-wrapping"/>
                </div>
                <div class="w-100">
                    <button id="unique-btn" class="btn btn-primary float-end">Login</button>
                    <button class="btn btn-secondary mx-2 float-end">Register</button>
                </div>
                <a href="https://google.com">Click here &gt;Google&lt;</a>
            </form>
        </div>
    </body>
</html>
```

## Code example Css
```css
h1 {
    background-image: url("res://test.png"); // Even here you can set resources per res url
    background-size: auto 100%;
}
```
