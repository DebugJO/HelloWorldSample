using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia.Threading;
using MyAppLib.Helpers;
using System;
using System.Runtime.InteropServices;

namespace MyApp.Views;

public partial class MainView : Window
{
    public MainView()
    {
        InitializeComponent();
    }
}

/*
private delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

private static partial class Win32Native
{
    [LibraryImport("user32.dll", EntryPoint = "SetWindowLongPtrW")]
    public static partial IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    [LibraryImport("user32.dll", EntryPoint = "SetWindowLongW")]
    public static partial IntPtr SetWindowLong32(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    [LibraryImport("user32.dll")]
    public static partial IntPtr CallWindowProcW(IntPtr lpPrevWndFunc, IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
}

private const int GWL_WNDPROC = -4;
private WndProcDelegate? _newWndProc;
private IntPtr _oldWndProc = IntPtr.Zero;

protected override void OnOpened(EventArgs e)
{
    base.OnOpened(e);

    if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    {
        return;
    }

    IPlatformHandle? platformHandle = TryGetPlatformHandle();

    if (platformHandle == null)
    {
        return;
    }

    IntPtr hWnd = platformHandle.Handle;

    if (hWnd == IntPtr.Zero)
    {
        return;
    }

    _newWndProc = MyWndProc;
    IntPtr newWndProcPtr = Marshal.GetFunctionPointerForDelegate(_newWndProc);

    _oldWndProc = IntPtr.Size == 8
        ? Win32Native.SetWindowLongPtr64(hWnd, GWL_WNDPROC, newWndProcPtr)
        : Win32Native.SetWindowLong32(hWnd, GWL_WNDPROC, newWndProcPtr);
}

private IntPtr MyWndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
{
    const uint WM_QUERYENDSESSION = 0x0011;
    const uint WM_ENDSESSION = 0x0016;

    if (msg is not (WM_QUERYENDSESSION or WM_ENDSESSION))
    {
        return Win32Native.CallWindowProcW(_oldWndProc, hWnd, msg, wParam, lParam);
    }

    LogHelper.Info("====== Windows 종료에 따른 프로그램 강제 종료 =====");
    LogHelper.Flush();

    Dispatcher.UIThread.Post(Close);
    return new IntPtr(1);
}
*/