using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CancelTokenExam
{
    public partial class FormMain : Form
    {
        // Reference : Brian Lagunas, "https://www.youtube.com/watch?v=TKc5A3exKBQ"

        private CancellationTokenSource cts = null;

        public FormMain()
        {
            InitializeComponent();
        }

        private async void ButtonStart_Click(object sender, EventArgs e)
        {
            cts = new CancellationTokenSource();

            var progress = new Progress<int>(value =>
           {
               // [1] 사용
               //ProgressBarExam.Value = value;
               //TextBoxBlock.Text = $"{value}";

               // [2] 미사용
               if (ProgressBarExam.InvokeRequired)
               {
                   ProgressBarExam.Invoke(new Action(() => { ProgressBarExam.Value = value; })); // '() =>' 또는 'delegate'
               }
               else
               {
                   ProgressBarExam.Value = value;
               }

               // [3] 미사용 : 확장메서드 테스트
               TextBoxBlock.RunUIThread(c => c.Text = $"{value}");
           });

            try
            {
                await Task.Run(() => LoopNumbers(100, progress, cts.Token));
                //TextBoxBlock.Text = "Completed";
                //ProgressBarExam.Value = 100;
            }
            catch (OperationCanceledException ex)
            {
                TextBoxBlock.Text = "Canceled : " + ex.Message;
            }
            catch (Exception ex)
            {
                TextBoxBlock.Text = "Error : " + ex.Message;
            }
            finally
            {
                cts.Dispose();
                cts = null;
            }
        }

        private static void LoopNumbers(int count, IProgress<int> progress, CancellationToken token)
        {
            for (int i = 0; i <= count; i++)
            {
                Thread.Sleep(100);
                var percentComplete = (i * 100) / count;
                progress.Report(percentComplete);

                if (token.IsCancellationRequested)
                {
                    //break;
                    token.ThrowIfCancellationRequested();
                }
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            if (cts != null)
                cts.Cancel();
        }
    }

    // 확장메서드
    public static class UIHelperExtension
    {
        public delegate void UIThreadDelegate<T>(T obj) where T : ISynchronizeInvoke;

        public static void RunUIThread<T>(this T obj, UIThreadDelegate<T> action) where T : ISynchronizeInvoke
        {
            if (obj.InvokeRequired)
            {
                obj.Invoke(action, new object[] { obj });
            }
            else
            {
                action(obj);
            }
        }
    }
}
