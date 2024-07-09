using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StyletTest.Helpers;

public static partial class WindowHelper
{
    [LibraryImport("USER32.DLL", EntryPoint = "FindWindowW", StringMarshalling = StringMarshalling.Utf16)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
    [return: MarshalAs(UnmanagedType.SysInt)]
    private static partial IntPtr FindWindowW(string? lpClassName, string? lpWindowName);

    [LibraryImport("USER32.DLL", EntryPoint = "SetForegroundWindow", StringMarshalling = StringMarshalling.Utf16)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool SetForegroundWindow(IntPtr hWnd);

    public static void BringToFront(string windowTitle)
    {
        IntPtr handle = FindWindowW(null, windowTitle);

        if (handle == IntPtr.Zero)
        {
            return;
        }

        _ = SetForegroundWindow(handle);
    }
}