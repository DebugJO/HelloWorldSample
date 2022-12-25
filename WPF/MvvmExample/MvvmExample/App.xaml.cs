using MvvmExample.Helpers;
using System.Windows;
using System.Windows.Threading;

namespace MvvmExample;

public partial class App : Application
{
    public App()
    {
        Dispatcher.UnhandledException += Dispatcher_UnhandledException;
    }

    private void Dispatcher_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        WindowHelper.InfoMessage("프로그램 동작 오류(Global Exception)", "Log(AppLogs폴더) 확인 필요");
        LogHelper.Logger.Fatal("#################### Global Exception : " + e.Exception.Message);
        e.Handled = true;
    }
}
