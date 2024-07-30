using StyletTest.AppConfig;
using StyletTest.ViewModels;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;

namespace StyletTest.Helpers;

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
        IntPtr handle = FindWindowW(null, IoC.Get<ShellViewModel>().DisplayName);

        if (handle == IntPtr.Zero)
        {
            return;
        }

        _ = SetForegroundWindow(handle);
    }

    public static IntPtr SendMessage(string sendString)
    {
        IntPtr lParam = Marshal.StringToHGlobalAuto(sendString);
        IntPtr handle = FindWindowW(null, IoC.Get<ShellViewModel>().DisplayName);
        return handle == IntPtr.Zero ? handle : SendMessageW(handle, 0x0400, IntPtr.Zero, lParam);
    }

    public static void StartWindowCenter(Window window)
    {
        Rect workArea = SystemParameters.WorkArea;
        window.Left = (workArea.Width - window.Width) / 2 + workArea.Left;
        window.Top = (workArea.Height - window.Height) / 2 + workArea.Top;
        //window.Left = SystemParameters.PrimaryScreenWidth / 2 - (window.ActualWidth == 0 ? 800 : window.ActualWidth) / 2;
        //window.Top = SystemParameters.PrimaryScreenHeight / 2 - (window.ActualHeight == 0 ? 450 : window.ActualWidth) / 2;
    }
}