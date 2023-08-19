using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FSC.WUF
{
    /// <summary>
    /// Interaktionslogik für WindowTitlebarTemplate.xaml
    /// </summary>
    internal partial class WindowTitlebarTemplate : UserControl
    {
        public WindowTitlebarTemplate()
        {
            InitializeComponent();
        }

        public new Brush Background
        {
            get => base.Background;
            set
            {
                TitleGrid.Background = value;
                TitleBarTitle.Background = value;
            }
        }

        public new Brush Foreground
        {
            get => base.Foreground;
            set
            {
                TitleBarTitle.Foreground = value;
                Button[] buttons = new Button[] { UnpinButton, PinButton, MinimizeButton, RestoreButton, MaximizeButton, CloseButton };
                for (var i = 0; i < buttons.Length; i++)
                {
                    buttons[i].Foreground = value;
                }
            }
        }

        private void Titlebar_Loaded(object sender, RoutedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            Window parentWindow = Window.GetWindow(this.Parent);
            TitleBarTitle.Text = parentWindow.Title;
            TitleBarIcon.Source = parentWindow.Icon;
            parentWindow.StateChanged += WindowTitlebarTemplate_StateChanged!;
            WindowTitlebarTemplate_StateChanged(null!, null!);

            if (parentWindow.Topmost)
            {
                PinButton.Visibility = Visibility.Collapsed;
                UnpinButton.Visibility = Visibility.Visible;
            }
            else
            {
                PinButton.Visibility = Visibility.Visible;
                UnpinButton.Visibility = Visibility.Collapsed;
            }

            if (parentWindow.ResizeMode == ResizeMode.NoResize)
            {
                MaximizeButton.Visibility = Visibility.Collapsed;
            }
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        // Minimize
        private void CommandBinding_Executed_Minimize(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(Window.GetWindow(this.Parent));
        }

        // Maximize
        private void CommandBinding_Executed_Maximize(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(Window.GetWindow(this.Parent));
        }

        // Restore
        private void CommandBinding_Executed_Restore(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(Window.GetWindow(this.Parent));
        }

        // Close
        private void CommandBinding_Executed_Close(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow(Window.GetWindow(this.Parent));
        }

        // Unpin
        private void UnpinButton_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this.Parent);
            parentWindow.Topmost = false;

            PinButton.Visibility = Visibility.Visible;
            UnpinButton.Visibility = Visibility.Collapsed;
        }

        // Pin
        private void PinButton_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this.Parent);
            parentWindow.Topmost = true;

            PinButton.Visibility = Visibility.Collapsed;
            UnpinButton.Visibility = Visibility.Visible;
        }

        // State change
        private void WindowTitlebarTemplate_StateChanged(object sender, EventArgs e)
        {
            Window parentWindow = Window.GetWindow(this.Parent);

            if (RestoreButton.Visibility != Visibility.Visible && MaximizeButton.Visibility != Visibility.Visible)
            {
                return;
            }

            if (parentWindow.WindowState == WindowState.Maximized)
            {
                parentWindow.Padding = new Thickness(0);
                parentWindow.BorderThickness = new Thickness(8);
                RestoreButton.Visibility = Visibility.Visible;
                MaximizeButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                parentWindow.Padding = new Thickness(2);
                parentWindow.BorderThickness = new Thickness(1);
                RestoreButton.Visibility = Visibility.Collapsed;
                MaximizeButton.Visibility = Visibility.Visible;
            }
        }
    }
}
