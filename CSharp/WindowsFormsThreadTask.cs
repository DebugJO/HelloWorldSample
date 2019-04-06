using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsTask
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();

            ButtonTest.Click += async (s, e) =>
            {
                /*
                ButtonTest.Invoke(new Action(delegate {
                    ButtonTest.Text = "실행 중...";
                }));
                */

                TextBoxTest.Text = "";
                TextBoxTest2.Text = "";
                ButtonTest.Text = "실행 중...";
                ButtonTest.Enabled = false;

                TextBoxTest.Text = await DoWork("가나닭 ");

                TextBoxTest2.Text = "실행 완료";
                ButtonTest.Text = "조회";
                ButtonTest.Enabled = true;
            };
        }

        private string PrintMesage(string name)
        {
            Thread.Sleep(5000);
            return "안녕하세요 : " + name;
        }

        private async Task<string> DoWork(string name)
        {
            return await Task.Run(() => PrintMesage(name));
        }

        private int SlowFunc(int a, int b)
        {
            Thread.Sleep(5000);
            return a + b;
        }

        private async void Button2_Click(object sender, EventArgs e)
        {
            Button2.Enabled = false;
            Button2.Text = "조회 중...";

            int result = await Task.Run(() => SlowFunc(2, 3));

            TextBoxTest2.Text = result.ToString();

            Button2.Enabled = true;
            Button2.Text = "조회";
        }
    }
}
