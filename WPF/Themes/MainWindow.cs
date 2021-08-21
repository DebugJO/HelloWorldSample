using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WPFApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ButtonSearch.Click += async (s, e) => // Sender, EventArgs
            {
                if (s is not Button)
                {
                    _ = MessageBox.Show(e.RoutedEvent.ToString());
                    return;
                }

                try
                {
                    (s as Button).IsEnabled = false;
                    await Task.Delay(3000);
                }
                finally
                {
                    (s as Button).IsEnabled = true;
                }

            };
        }


    }

    public class MyButton : Button
    {
        public static readonly DependencyProperty p1 = DependencyProperty.Register(nameof(Icon), typeof(string), typeof(MyButton),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public string Icon
        {
            get => (string)GetValue(p1);
            set => SetValue(p1, value);
        }

        public static readonly DependencyProperty p2 = DependencyProperty.Register(nameof(Radius), typeof(string), typeof(MyButton),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public string Radius
        {
            get => (string)GetValue(p2);
            set => SetValue(p2, value);
        }
    }
}
