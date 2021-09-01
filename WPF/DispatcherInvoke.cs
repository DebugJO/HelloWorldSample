using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void ButtonHello_Click(object sender, RoutedEventArgs e)
        {
            Thread th1 = new(TestError);
            th1.Start();

            Thread th2 = new(TestInvoke);
            th2.Start();

            string[] parms = { "Hellow World", "가나닭" };
            Thread th3 = new(TestInvokeParms);
            th3.Start(parms);


            await Task.Run(async () =>
            {
                Thread th2 = new(TestInvoke);
                th2.Start();

                await Task.Delay(3000);

                string[] parms = { "Hellow World", "가나닭" };
                Thread th3 = new(TestInvokeParms);
                th3.Start(parms);
            });

            await TestAsync();
        }

        private void TestError()
        {
            TextBoxHello.Text = "Hello World";
        }

        private void TestInvoke()
        {
            Dispatcher?.Invoke(() => { TextBoxHello.Text = "Hello World"; });
        }

        private void TestInvokeParms(object str)
        {
            string[] data = str as string[];
            Dispatcher.Invoke(() => { TextBoxHello.Text = data?[0] + (char)0x20 + data?[1]; });
        }

        private async Task TestAsync()
        {
            await Task.Delay(3000);
            TextBoxHello.Text = "Hello World Task";
        }

        public string GetName()
        {
            return typeof(Person).GetProperty("Name").GetDisplayName();
        }
    }
}
