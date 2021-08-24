using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFApp
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            ButtonSearch.Click += ButtonClickTest;
            ButtonOther.Click += ButtonClickProgress;

            RadioButton1.MouseDown += RadioMouseDown;
            RadioButton2.MouseDown += RadioMouseDown;

            TextSample obj = new()
            {
                Text1 = "홍길동",
                Text2 = "홍길서",
                Text3 = "홍길남"
            };

            DataContext = obj;
        }

        private void RadioMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                ((RadioButton)FindName("Radio" + ((Border)sender).Name.Last())).IsChecked = true;
            }
        }

        private static async void ButtonClickTest(object sender, RoutedEventArgs e)
        {
            try
            {
                ((Button)sender).IsEnabled = false;
                await Task.Delay(1000);
            }
            finally
            {
                ((Button)sender).IsEnabled = true;
            }
        }

        private async void ButtonClickProgress(object sender, RoutedEventArgs e)
        {
            try
            {
                ((Button)sender).IsEnabled = false;

                for (var i = 0; i <= 100; i++)
                {
                    PbStatus.Value = i;
                    await Task.Delay(50);
                }
            }
            finally
            {
                ((Button)sender).IsEnabled = true;
            }
        }
    }

    public class TextSample
    {
        public string Text1 { get; set; }
        public string Text2 { get; set; }
        public string Text3 { get; set; }
    }
}