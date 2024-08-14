using Caliburn.Micro;
using CMTest80.Views;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;

namespace CMTest80.Helpers;

public static partial class WindowHelper
{
    [LibraryImport("USER32.DLL", EntryPoint = "SendMessageW", StringMarshalling = StringMarshalling.Utf16)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
    [return: MarshalAs(UnmanagedType.SysInt)]
    private static partial IntPtr SendMessageW(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

    [LibraryImport("USER32.DLL", EntryPoint = "FindWindowW", StringMarshalling = StringMarshalling.Utf16)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
    [return: MarshalAs(UnmanagedType.SysInt)]
    private static partial IntPtr FindWindowW(string? lpClassName, string? lpWindowName);

    [LibraryImport("USER32.DLL", EntryPoint = "SetForegroundWindow", StringMarshalling = StringMarshalling.Utf16)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool SetForegroundWindow(IntPtr hWnd);

    public static void BringToFront()
    {
        IntPtr handle = FindWindowW(null, IoC.Get<ShellView>().Title);

        if (handle == IntPtr.Zero)
        {
            return;
        }

        _ = SetForegroundWindow(handle);
    }

    public static void StartWindowCenter(Window window)
    {
        Rect workArea = SystemParameters.WorkArea;
        window.Left = (workArea.Width - window.Width) / 2 + workArea.Left;
        window.Top = (workArea.Height - window.Height) / 2 + workArea.Top;
    }

    public static void StartWindowFullSize(Window window, uint width = 1024, uint height = 768)
    {
        int _width = Convert.ToInt32(SystemParameters.PrimaryScreenWidth);
        int _height = Convert.ToInt32(SystemParameters.PrimaryScreenHeight);

        if (_width != width || _height != height)
        {
            window.Topmost = false;
            window.ResizeMode = ResizeMode.CanResize;
            window.WindowStyle = WindowStyle.SingleBorderWindow;
            window.WindowState = WindowState.Normal;
        }
        else
        {
            window.Topmost = true;
            window.ResizeMode = ResizeMode.NoResize;
            window.WindowStyle = WindowStyle.None;
            window.WindowState = WindowState.Maximized;
        }
    }

    public static IntPtr SendMessage(string sendString)
    {
        IntPtr lParam = Marshal.StringToHGlobalAuto(sendString);
        IntPtr handle = FindWindowW(null, IoC.Get<ShellView>().Title);
        return handle == IntPtr.Zero ? handle : SendMessageW(handle, 0x0400, IntPtr.Zero, lParam);
    }

    public static string GetWindowsVersion()
    {
        Version version = Environment.OSVersion.Version;

        switch (version.Major)
        {
            case 10:
                return version.Build >= 20000 ? "Windows11" : "Windows10";
            case 6:
                switch (version.Minor)
                {
                    case 3:
                        return "Windows8.1";
                    case 2:
                        return "Windows8";
                    case 1:
                        return "Windows7";
                    case 0:
                        return "WindowsVista";
                }

                break;
            case 5:
                return "WindowsXP";
        }

        return "NotDetected";
    }
}