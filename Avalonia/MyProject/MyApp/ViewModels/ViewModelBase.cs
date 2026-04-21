using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using MyApp.Messages;
using MyAppLib.Helpers;
using System;
using System.Threading.Tasks;

namespace MyApp.ViewModels;

public class ViewModelBase : ObservableObject
{
    protected void Send<T>(T message) where T : class
    {
        WeakReferenceMessenger.Default.Send(message);
    }

    protected void SendState(string message)
    {
        WeakReferenceMessenger.Default.Send(new StatusMessageRegister(message));
    }
    
    // Register가 아래 예제처럼 비동일 때 사용
    protected async Task<bool> SendAsync<T>(T message) where T : class
    {
        Action action = () => WeakReferenceMessenger.Default.Send(message);
        return await action.RunAsync(ex => LogHelper.Error($"메시지 전송실패 : {ex.Message}"));
    }
}

// private void RegisterStatusMessage()
// {
//     WeakReferenceMessenger.Default.Register<StatusMessageRegister>(
//         this, (_, m) => { ProcessStatusUpdateAsync(m.text).Await(); }
//     );
// }
//
// private async Task ProcessStatusUpdateAsync(string text)
// {
//     await Task.Delay(10);
//
//     Dispatcher.UIThread.Post(() => 
//     {
//         UpdateStatus(text);
//     });
// }

/*
// 큐를 던져놓고 즉시 리턴 : 로그,상태 등 UI 흐름에 영향을 주시 않을 때  
Post(Action action, DispatcherPriority priority)
// await 가능, 완료될 때까지 기다리는 Task반환
InvokeAsync(Func<Task> action, DispatcherPriority priority)
var result = await Dispatcher.UIThread.InvokeAsync(() => MyDialog.Show());
// 화면을 다시 그리도록 강제
RequestRender()
// 우선순위(DispatcherPriority)
MaxValue   가장 높은 우선순위. 즉시 처리해야 함.
Render	   화면 갱신(Rendering)과 같은 수준. 부드러운 애니메이션에 중요.
Normal	   (기본값) 일반적인 작업 처리 수준.
Input	    마우스 클릭이나 키보드 입력보다 낮은 수준.
Background  UI가 한가할 때 처리. 무거운 작업 후순위 밀기용.
MinValue	가장 낮음. 유휴 상태(Idle)일 때만 실행.
*/