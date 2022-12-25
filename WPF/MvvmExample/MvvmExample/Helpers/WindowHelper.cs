using Caliburn.Micro;
using MvvmExample.ViewModels;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Shell;

namespace MvvmExample.Helpers;

public static class WindowHelper
{
    private static bool mIsFull = false;

    [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
    private static extern void ReleaseCapture();

    [DllImport("user32.DLL", EntryPoint = "SendMessage")]
    private static extern void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

    [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
    private static extern IntPtr FindWindow(string? lpClassName, string lpWindowName);

    [DllImport("USER32.DLL")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    public static void DragMoveWindow()
    {
        ReleaseCapture();
        SendMessage(Process.GetCurrentProcess().MainWindowHandle, 0x112, 0xf012, 0);
    }

    public static void BringToFront(string title)
    {
        var handle = FindWindow(null, title);

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

    public static void MaxMinChangeScreen(Window window)
    {
        if (window.WindowState != WindowState.Maximized)
        {
            WindowMax(window);
        }
        else
        {
            WindowNormal(window);
        }
    }

    private static WindowChrome GetChrome()
    {
        Thickness t = SystemParameters.WindowResizeBorderThickness;

        WindowChrome customChrome = new()
        {
            GlassFrameThickness = new Thickness(0),
            CornerRadius = new CornerRadius(0),
            CaptionHeight = 0,
            NonClientFrameEdges = NonClientFrameEdges.None,
            ResizeBorderThickness = new Thickness(t.Left, t.Top, t.Right, t.Bottom)
        };
        return customChrome;
    }

    private static void UndoFullscreen(Window window)
    {
        WindowChrome.SetWindowChrome(window, GetChrome());
        window.WindowStyle = WindowStyle.SingleBorderWindow;
        window.ResizeMode = ResizeMode.CanResize;
    }

    public static void WindowNormal(Window window)
    {
        UndoFullscreen(window);
        window.WindowState = WindowState.Normal;
        mIsFull = false;
    }

    public static void WindowMin(Window window)
    {
        UndoFullscreen(window);
        window.WindowState = WindowState.Minimized;
        mIsFull = false;
    }

    public static void WindowMax(Window window)
    {
        UndoFullscreen(window);
        window.WindowState = WindowState.Maximized;
        mIsFull = false;
    }

    public static void WindowFullScreen(Window window)
    {
        if (!mIsFull)
        {
            window.WindowState = WindowState.Normal;
        }

        WindowChrome.SetWindowChrome(window, new()
        {
            GlassFrameThickness = new Thickness(-1),
            CornerRadius = new CornerRadius(0),
            CaptionHeight = 0,
            NonClientFrameEdges = NonClientFrameEdges.None,
            ResizeBorderThickness = new Thickness(0),
        });

        window.ResizeMode = ResizeMode.NoResize;
        window.WindowStyle = WindowStyle.None;
        window.WindowState = WindowState.Maximized;
        window.BorderThickness = new Thickness(0);
        mIsFull = true;
    }

    public static void InfoMessage(string? title = null, string? message = null)
    {
        string result = title == null ? string.Empty : $"[ {DateTime.Now:yyyy-MM-dd hh:mm:ss} ] " + title +
                (string.IsNullOrWhiteSpace(message) ? "" : Environment.NewLine + "⦁ " + message.RemoveNewline().RemoveDoubleSpace());

        if (string.IsNullOrWhiteSpace(result)) { return; }

        if (result.Length > 1024)
        {
            IoC.Get<ShellViewModel>().InfoMessage = result[..1024];
        }
        else
        {
            IoC.Get<ShellViewModel>().InfoMessage = result;
        }
    }
}
