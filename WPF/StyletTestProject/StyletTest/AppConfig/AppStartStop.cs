﻿using StyletTest.Helpers;
using StyletTest.ViewModels;
using StyletTest.Views;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace StyletTest.AppConfig;

public static class AppStartStop
{
    public static async Task Start()
    {
        Process[] p = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName.ToUpper());

        if (p.Length > 1)
        {
            LogHelper.Logger.Info("##### 프로그램 중복 실행 시도- 방지 - 프로그램 최상위로 가져오기 #####");
            Application.Current.Shutdown();
            WindowHelper.BringToFront();
            return;
        }

        LogHelper.Logger.Info("***** 프로그램 서비스 : Start(1) *****");
        await Task.Delay(50); // 시작작성
        LogHelper.Logger.Info("***** 프로그램 서비스 : Start(2) *****");
    }

    public static async Task Stop()
    {
        try
        {
            LogHelper.Logger.Info("===== 프로그램 서비스 : Stop(1) =====");
            await Task.Delay(50); // 종료작성
            LogHelper.Logger.Info("===== 프로그램 서비스 : Stop(2) =====");

            IoC.Get<ShellViewModel>().Items.Clear();

            for (int windowsCount = Application.Current.Windows.Count - 1; windowsCount > 0; windowsCount--)
            {
                Application.Current.Windows[windowsCount]?.Close();
            }

            LogHelper.Logger.Info("===== 프로그램 종료 : Stop =====");
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
        LogHelper.Logger.Info("***** 프로그램 시작 : Completed *****");
    }

    public static void Error(Exception ex)
    {
        LogHelper.Logger.Error("***** 프로그램 시작 : ERROR : " + ex.Message);
    }

    public static async void OnClosing(object? sender, CancelEventArgs e)
    {
        e.Cancel = true;
        await Stop();
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
    }
}