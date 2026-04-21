using System;
using System.Threading;
using System.Threading.Tasks;

namespace MyAppLib.Helpers;

public static class TaskExtensions
{
    extension(Task task)
    {
        // task.Await(done, err, 10)
        public void Await(Action? done, Action<Exception>? err, int timeoutSeconds)
        {
            task.Await(done, err, timeoutSeconds, CancellationToken.None);
        }

        // task.Await(done, err, _cts.Token)
        public void Await(Action? done, Action<Exception>? err, CancellationToken token)
        {
            task.Await(done, err, 0, token);
        }

        //  task.Await(done, err);
        public void Await(Action? done = null, Action<Exception>? err = null)
        {
            task.Await(done, err, 0, CancellationToken.None);
        }

        // task.Await(done, err, 10, _cts.Token)
        private async void Await(Action? completedCallback, Action<Exception>? errorCallback,
            int timeoutSeconds, CancellationToken token)
        {
            try
            {
                if (timeoutSeconds > 0)
                {
                    await task.WaitAsync(TimeSpan.FromSeconds(timeoutSeconds), token).ConfigureAwait(false);
                }
                else
                {
                    await task.WaitAsync(token).ConfigureAwait(false);
                }

                completedCallback?.Invoke();
            }
            catch (Exception ex)
            {
                errorCallback?.Invoke(ex);
            }
        }
    }

    extension<TResult>(Task<TResult> task)
    {
        // Test().Await(res => Console.WriteLine(res), ex => Console.WriteLine(ex), 5);
        public void Await(Action<TResult>? done, Action<Exception>? err, int timeoutSeconds)
        {
            task.Await(done, err, timeoutSeconds, CancellationToken.None);
        }

        // Test().Await(res => Console.WriteLine(res), ex => Console.WriteLine(ex), _cts.Token);
        public void Await(Action<TResult>? done, Action<Exception>? err, CancellationToken token)
        {
            task.Await(done, err, 0, token);
        }

        // Test().Await(res => Console.WriteLine(res));
        public void Await(Action<TResult>? done = null, Action<Exception>? err = null)
        {
            task.Await(done, err, 0, CancellationToken.None);
        }

        // Test().Await(res => Console.WriteLine(res), ex => Console.WriteLine(ex), 5, _cts.Token);
        private async void Await(Action<TResult>? completedCallback, Action<Exception>? errorCallback,
            int timeoutSeconds, CancellationToken token)
        {
            try
            {
                TResult result;

                if (timeoutSeconds > 0)
                {
                    result = await task.WaitAsync(TimeSpan.FromSeconds(timeoutSeconds), token).ConfigureAwait(false);
                }
                else
                {
                    result = await task.WaitAsync(token).ConfigureAwait(false);
                }

                completedCallback?.Invoke(result);
            }
            catch (Exception ex)
            {
                errorCallback?.Invoke(ex);
            }
        }
    }
}

//
//  string targetData = "";
//  동기 함수 내부
//  Test().Await(
//      result => {
//          targetData = result; 
//          UpdateUI(targetData); // 데이터가 도착한 시점에 실행
//      },
//      ex => HandleError(ex)
//  );

// private readonly CancellationTokenSource _cts;
//public void DoSomethingSync()
//{
//  _cts?.Cancel();
//  _cts = new CancellationTokenSource();
//  _cts.CancelAfter(TimeSpan.FromSeconds(5));
//           
//  Test().Await(
//     () => Console.WriteLine("성공했습니다!"),
//     ex => Console.WriteLine(ex is OperationCanceledException ? "취소됨" : $"에러: {ex.Message}"),
//     0,
//     _cts.Token
//  );
//}

// Test().Await(
//     res => {
//         // 성공했을 때만 실행되는 구역
//         this.myData = res;
//         this.NotifySuccess(); 
//     },
//     ex => {
//         // 실패했을 때만 실행되는 구역
//         this.myData = null;
//         this.NotifyError(ex);
//     },
//     5
// );

// // 1. 상태를 관리하는 변수들
// private string _data = string.Empty;
// private bool _isError = false;
//
// // 2. 상태 업데이트 전용 함수 (함수를 통해 업데이트)
// private void UpdateStatus(string result, Exception? ex = null)
// {
//     if (ex != null)
//     {
//         _isError = true;
//         _data = $"에러 발생: {ex.Message}";
//     }
//     else
//     {
//         _isError = false;
//         _data = result;
//     }
//     
//     // UI 업데이트 로직이나 로그 출력 등을 여기서 통합 관리
//     Console.WriteLine($"[상태 변경] 데이터: {_data}, 에러여부: {_isError}");
// }
//
// // 3. 실행부
// public void RunTest()
// {
//     Test().Await(
//         res => UpdateStatus(res),          // 성공 시: 결과 전달
//         ex => UpdateStatus(string.Empty, ex), // 실패 시: 예외 전달
//         5                                  // 5초 타임아웃
//     );
// }

// Test().Await(
//     res => 
//     {
//         // Avalonia의 Dispatcher를 사용하여 UI 스레드에서 실행
//         Dispatcher.UIThread.Post(() => 
//         {
//             if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
//             {
//                 var win = desktop.MainWindow;
//                 if (win != null)
//                 {
//                     win.Title = $"결과: {res}";
//                     // 여기서 결과에 따라 함수 호출하여 업데이트
//                     UpdateMyUI(res); 
//                 }
//             }
//         });
//     },
//     ex => 
//     {
//         Dispatcher.UIThread.Post(() => 
//         {
//             // 에러 시 처리
//             ShowErrorDialog(ex.Message);
//         });
//     },
//     5 // 5초 타임아웃
// );