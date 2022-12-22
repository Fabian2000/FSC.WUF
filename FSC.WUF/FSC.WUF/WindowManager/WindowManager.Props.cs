using System.Windows;
using System.Windows.Media;
using System.Windows.Shell;

namespace FSC.WUF
{
    public sealed partial class WindowManager
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public double ActualHeight
        {
            get => _window!.ActualHeight;
        }

        public double ActualWidth
        {
            get => _window!.ActualWidth;
        }
     
        public Brush Background
        {
            get => _window!.Background;
            set => _window!.Background = value;
        }
     
        public bool? DialogResult
        {
            get => _window!.DialogResult;
            set => _window!.DialogResult = value;
        }
     
        public double Height
        {
            get => _window!.Height;
            set => _window!.Height = value;
        }
     
        public ImageSource Icon
        {
            get => _window!.Icon;
            set => _window!.Icon = value;
        }

        public bool IsActive
        {
            get => _window!.IsActive;
        }
     
        public bool IsInitialized
        {
            get => _window!.IsInitialized;
        }
     
        public bool IsLoaded
        {
            get => _window!.IsLoaded;
        }
     
        public bool IsMouseOver
        {
            get => _window!.IsMouseOver;
        }
     
        public bool IsSealed
        {
            get => _window!.IsSealed;
        }
     
        public bool IsVisible
        {
            get => _window!.IsVisible;
        }

        public double Left
        {
            get => _window!.Left;
            set => _window!.Left = value;
        }

        public double MaxHeight
        {
            get => _window!.MaxHeight;
            set => _window!.MaxHeight = value;
        }

        public double MaxWidth
        {
            get => _window!.MaxWidth;
            set => _window!.MaxWidth = value;
        }

        public double MinHeight
        {
            get => _window!.MinHeight;
            set => _window!.MinHeight = value;
        }

        public double MinWidth
        {
            get => _window!.MinWidth;
            set => _window!.MinWidth = value;
        }

        public double Opacity
        {
            get => _window!.Opacity;
            set => _window!.Opacity = value;
        }

        public ResizeMode ResizeMode
        {
            get => _window!.ResizeMode;
            set => _window!.ResizeMode = value;
        }

        public Rect RestoreBounds
        {
            get => _window!.RestoreBounds;
        }

        public bool ShowActivated
        {
            get => _window!.ShowActivated;
            set => _window!.ShowActivated = value;
        }

        public Visibility ShowIcon
        {
            get => _window!.ShowIcon;
            set => _window!.ShowIcon = value;
        }

        public bool ShowInTaskbar
        {
            get => _window!.ShowInTaskbar;
            set => _window!.ShowInTaskbar = value;
        }

        public TaskbarItemInfo TaskbarItemInfo
        {
            get => _window!.TaskbarItemInfo;
            set => _window!.TaskbarItemInfo = value;
        }

        public string Title
        {
            get => _window!.Title;
            set => _window!.Title = value;
        }

        public double Top
        {
            get => _window!.Top;
            set => _window!.Top = value;
        }

        public bool Topmost
        {
            get => _window!.Topmost;
            set => _window!.Topmost = value;
        }

        public Visibility Visibility
        {
            get => _window!.Visibility;
            set => _window!.Visibility = value;
        }

        public double Width
        {
            get => _window!.Width;
            set => _window!.Width = value;
        }

        public WindowStartupLocation WindowStartupLocation
        {
            get => _window!.WindowStartupLocation;
            set => _window!.WindowStartupLocation = value;
        }

        public WindowState WindowState
        {
            get => _window!.WindowState;
            set => _window!.WindowState = value;
        }
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }
}
