// https://www.youtube.com/watch?v=O1Tx-k4Vao0
using System;
using System.Threading.Tasks;
using System.Windows;

namespace YieldReturnAsync
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void DoSomething_Click(object sender, RoutedEventArgs e)
        {
            DoSomething().AWait(Completed, HandleError);
        }

        private void Completed()
        {
            TextBlockExam.Text = "Completed";
        }

        private void HandleError(Exception ex)
        {
            TextBlockExam.Text = ex.Message;
        }

        private async Task DoSomething()
        {
            await Task.Delay(3000);
            TextBlockExam.Text = "Changed in task";
            // throw new Exception("Thrown in task");
        }
    }

    public static class TaskExtensions
    {
        public static async void AWait(this Task task, Action completedCallback, Action<Exception> errorCallback)
        {
            try
            {
                await task;
                completedCallback?.Invoke();
            }
            catch (Exception ex)
            {
                errorCallback?.Invoke(ex);
            }
        }
    }
}
