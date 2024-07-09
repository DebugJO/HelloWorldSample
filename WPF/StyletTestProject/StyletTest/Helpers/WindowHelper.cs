using StyletTest.AppConfig;
using StyletTest.ViewModels;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

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
}