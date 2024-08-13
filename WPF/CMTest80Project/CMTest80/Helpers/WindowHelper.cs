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

    public static IntPtr SendMessage(string sendString)
    {
        IntPtr lParam = Marshal.StringToHGlobalAuto(sendString);
        IntPtr handle = FindWindowW(null, IoC.Get<ShellView>().Title);
        return handle == IntPtr.Zero ? handle : SendMessageW(handle, 0x0400, IntPtr.Zero, lParam);
    }
}