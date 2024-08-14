using Caliburn.Micro;
using CMTest80.Helpers;
using CMTest80.ViewModels;
using CMTest80.Views;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace CMTest80;

public static class AppStartup
{
    public static event EventHandler<string>? OnReceiveMessage;

    public static async Task AppStart()
    {
        Process[] p = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName.ToUpper());

        if (p.Length > 1)
        {
            LogHelper.Logger.Info("##### 프로그램 중복 실행 시도- 방지 - 프로그램 최상위로 가져오기 #####");
            Application.Current.Shutdown();
            WindowHelper.BringToFront();
            return;
        }

        IoC.Get<ShellView>().Closing += OnClosing;
        IoC.Get<ShellView>().SourceInitialized += OnSourceInitialized;

        LogHelper.Logger.Info("***** 프로그램 : AppStart *****");
        WindowHelper.StartWindowCenter(IoC.Get<ShellView>());
        WindowHelper.StartWindowFullSize(IoC.Get<ShellView>());
        await Task.Delay(50);
        // 서비스 시작 ...
    }

    public static async Task AppStop()
    {
        try
        {
            // 서비스 종료 ...
            await Task.Delay(50);
            //IoC.Get<FormWidgetView>().Close();
            IoC.Get<ShellViewModel>().Items.Clear();
            
            for (int windowsCount = Application.Current.Windows.Count - 1; windowsCount > 0; windowsCount--)
            {
                Application.Current.Windows[windowsCount]?.Close();
            }
            
            LogHelper.Logger.Info("***** 프로그램 : AppStop *****");
            LogHelper.ShutdownLogManager();
            Environment.Exit(0);
        }
        catch
        {
            Environment.Exit(0);
        }
    }

    public static void Completed()
    {
        LogHelper.Logger.Info("***** 프로그램 : AppStart Completed *****");
    }

    public static void Error(Exception ex)
    {
        LogHelper.Logger.Error($"***** 프로그램 : AppStart ERROR : {ex.Message}");
    }

    public static async void OnClosing(object? sender, CancelEventArgs e)
    {
        e.Cancel = true;
        await AppStop();
    }

    public static void OnSourceInitialized(object? sender, EventArgs e)
    {
        IntPtr windowHandle = new WindowInteropHelper(IoC.Get<ShellView>()).Handle;
        HwndSource? hwndSource = HwndSource.FromHwnd(windowHandle);
        hwndSource?.AddHook(WndProc);
    }

    private static IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        try
        {
            switch (msg)
            {
                case 0x0011: // WM_QUERYENDSESSION
                    LogHelper.Logger.Info("====== Windows 종료에 따른 프로그램 강제 종료 =====");
                    IoC.Get<ShellView>().Close();
                    handled = true;
                    break;
                case 0x0400: // WM_USER
                    ReceiveMessage(Marshal.PtrToStringAuto(lParam) ?? "");
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

    private static void ReceiveMessage(string receiveString)
    {
        LogHelper.Logger.Debug($"ReceiveMessage : WM_USER : {receiveString}");
        OnReceiveMessage?.Invoke(IoC.Get<ShellView>(), receiveString);
    }
}