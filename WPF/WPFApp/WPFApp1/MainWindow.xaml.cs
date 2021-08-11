using System.Windows;
using System.Windows.Input;

namespace WPFApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            //MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            //Left = SystemParameters.WorkArea.Left;
            //Top = SystemParameters.WorkArea.Top;
            //Height = SystemParameters.WorkArea.Height;
            //Width = SystemParameters.WorkArea.Width;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ButtonWindowstate_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState != WindowState.Maximized)
            {
                WindowState = WindowState.Maximized;
                BorderThickness = new Thickness(8, 8, 8, 8);
            }
            else
            {
                WindowState = WindowState.Normal;
                BorderThickness = new Thickness(0, 0, 0, 0);
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (WindowState != WindowState.Maximized)
                {
                    WindowState = WindowState.Maximized;
                    BorderThickness = new Thickness(8, 8, 8, 8);
                }
                else
                {
                    WindowState = WindowState.Normal;
                    BorderThickness = new Thickness(0, 0, 0, 0);
                }
            }
        }
    }
}