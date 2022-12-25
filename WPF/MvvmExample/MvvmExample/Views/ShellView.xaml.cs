using Caliburn.Micro;
using MvvmExample.Helpers;
using MvvmExample.ViewModels;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace MvvmExample.Views;

public partial class ShellView : Window
{
    public ShellView()
    {
        InitializeComponent();
        StartShellView().AWait(CompletedCallback, ErrorCallback);
    }

    private void CompletedCallback()
    {
        WindowHelper.InfoMessage("프로그램 시작", " 완료");
        LogHelper.Logger.Info("***** 프로그램 시작 : 완료 *****");
    }

    private void ErrorCallback(Exception ex)
    {
        LogHelper.Logger.Error("프로그램 시작 오류 : " + ex.Message);
        WindowHelper.InfoMessage("프로그램 시작 오류", ex.Message);
    }

    public async Task StartShellView()
    {
        Process[] p = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName.ToUpper());
        if (p.Length > 1)
        {
            LogHelper.Logger.Info("##### 프로그램 중복 실행 시도 - 프로그램 최상위로 가져오기 #####");
            Close();
            WindowHelper.BringToFront(Title);
            return;
        }

        WindowHelper.StartWindowCenter(this);
        //WindowHelper.WindowFullScreen(this);

        // 여기에 시작 로직 추가

        LogHelper.Logger.Info("***** 프로그램 시작 : 준비 중.......... *****");
        await Task.Delay(100);
    }

    public static async Task StopShellView()
    {
        // 여기에 종료 로직 추가
        await Task.Run(async () =>
        {
            LogHelper.Logger.Info("====== 프로그램 종료 =====");
            await Task.Delay(100);
        });
    }

    protected override async void OnClosing(CancelEventArgs e)
    {
        try
        {
            await StopShellView();

            IoC.Get<ShellViewModel>().Items.Clear();
            LogHelper.ShutdownLogManager();

            for (var windowsCount = Application.Current.Windows.Count - 1; windowsCount > 0; windowsCount--)
            {
                Application.Current.Windows[windowsCount]?.Close();
            }

            DataContext = null;
            Application.Current.Shutdown();
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error("ShellView Closing Exception : " + ex.Message);
            Application.Current.Shutdown();
        }

        base.OnClosing(e);
    }

    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);
        HwndSource? source = PresentationSource.FromVisual(this) as HwndSource;
        source?.AddHook(WndProc);
    }

    private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        try
        {
            switch (msg)
            {
                case 0x0011: // WM_QUERYENDSESSION
                    LogHelper.Logger.Info("====== Windows 종료에 따른 프로그램 강제 종료 =====");
                    Close();
                    handled = true;
                    break;
            }

            return IntPtr.Zero;
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error("WndProc Error : " + ex.Message);
            return IntPtr.Zero;
        }
    }
}
