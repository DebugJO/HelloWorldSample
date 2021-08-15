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
            ProgressBarExam.SetState(UIHelperExtension.Color.Yellow, 0);
        }

        private async void ButtonStart_Click(object sender, EventArgs e)
        {
            if (cts != null)
            {
                (sender as Button).RunUIThread(b => b.Text = "동작중...");
                return;
            }
            
            cts = new CancellationTokenSource();

            var progress = new Progress<int>(value =>
           {
                ProgressBarExam.RunUIThread(c => c.Value = value);
                TextBoxBlock.RunUIThread(t => t.Text = value.ToString());       
                
               // [1] 
               //ProgressBarExam.Value = value;
               //TextBoxBlock.Text = $"{value}";

               // [2] 
               //if (ProgressBarExam.InvokeRequired)
               //{
               //    ProgressBarExam.Invoke(new Action(() => { ProgressBarExam.Value = value; })); // '() =>' 또는 'delegate'
               //}
               //else
               //{
               //    ProgressBarExam.Value = value;
               //}

               // [3] 
               //TextBoxBlock.RunUIThread(c => c.Text = $"{value}");
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
                (sender as Button).RunUIThread(t => t.Text = "Start");            
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
        
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);
        public enum Color { None, Green, Red, Yellow }

        public static void SetState(this ProgressBar pBar, Color newColor, int newValue)
        {
            if (pBar.Value == pBar.Minimum)
            {
                SendMessage(pBar.Handle, 1040, (IntPtr)(int)Color.Green, IntPtr.Zero);
            }
            pBar.Value = newValue;
            SendMessage(pBar.Handle, 1040, (IntPtr)(int)Color.Green, IntPtr.Zero);
            SendMessage(pBar.Handle, 1040, (IntPtr)(int)newColor, IntPtr.Zero);
        }
    }
}
