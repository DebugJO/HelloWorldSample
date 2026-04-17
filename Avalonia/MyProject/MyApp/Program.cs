using Avalonia;
using Avalonia.Controls;
using Avalonia.Logging;
using MyApp.WindowHelper;
using MyAppLib.Helpers;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace MyApp;

internal sealed partial class Program
{
    private static Mutex? _mutex;
    private const int SW_RESTORE = 9;

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool SetForegroundWindow(IntPtr hWnd);

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool IsIconic(IntPtr hWnd);

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [STAThread]
    public static void Main(string[] args)
    {
        const string mutexName = @"Global\MyApp-devsight-2026 -001";
        _mutex = new Mutex(true, mutexName, out bool isNewInstance);

        if (!isNewInstance)
        {
            LogHelper.Warn("프로그램 중복실행 ...");
            BringExistingInstanceToForeground();
            return;
        }

        try
        {
            LogHelper.Debug("Desktop : Start(1/3) ...");
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }
        catch (Exception ex)
        {
            LogHelper.Fatal($"최상위 UI 스레드 에러 : {ex.Message}");
        }
        finally
        {
            _mutex.ReleaseMutex();
            _mutex.Dispose();
        }
    }

    private static void BringExistingInstanceToForeground()
    {
        Process current = Process.GetCurrentProcess();

        foreach (Process process in Process.GetProcessesByName(current.ProcessName))
        {
            if (process.Id == current.Id)
            {
                continue;
            }

            IntPtr handle = process.MainWindowHandle;

            if (handle != IntPtr.Zero)
            {
                if (IsIconic(handle))
                {
                    _ = ShowWindow(handle, SW_RESTORE);
                }

                _ = SetForegroundWindow(handle);
                LogHelper.Warn("프로그램 Foreground ...");
            }

            break;
        }
    }

    public static AppBuilder BuildAvaloniaApp() => AppBuilder.Configure<App>()
        .UsePlatformDetect()
        .WithInterFont()
        .With(new Win32PlatformOptions
        {
            OverlayPopups = true,
            CompositionMode = [Win32CompositionMode.WinUIComposition, Win32CompositionMode.DirectComposition],
            // ShouldRenderOnUIThread = true
        })
        // .LogToTrace();
        .AfterSetup(_ =>
        {
            if (Design.IsDesignMode)
            {
                return;
            }

            Logger.Sink = new CustomLogSink(LogEventLevel.Warning, "MY_APP");
            LogHelper.Debug("Desktop : Start(2/3) ...");
        });
}

/*
   dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
 */

// [Window, MacOS, Linux(Desktop/DRM)]
//
// internal sealed class Program
// {
//     [STAThread]
//     public static void Main(string[] args)
//     {
//         AppBuilder builder = BuildAvaloniaApp();
//
//         if (OperatingSystem.IsLinux() && IsHeadless())
//         {
//             builder.StartLinuxDrm(args, card: null, scaling: 1.0);
//         }
//         else
//         {
//             builder.StartWithClassicDesktopLifetime(args);
//         }
//     }
//
//     public static AppBuilder BuildAvaloniaApp()
//         => AppBuilder.Configure<App>()
//             .UsePlatformDetect()
//             .WithInterFont()
//             .WithPlatformOptions()
//             .LogToTrace();
//
//     private static bool IsHeadless()
//     {
//         return string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DISPLAY")) &&
//                string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WAYLAND_DISPLAY"));
//     }
// }
//
// internal static class AppBuilderExtensions
// {
//     public static AppBuilder WithPlatformOptions(this AppBuilder builder)
//     {
//         if (OperatingSystem.IsWindows())
//         {
//             return builder.With(new Win32PlatformOptions
//             {
//                 OverlayPopups = true,
//                 CompositionMode = [Win32CompositionMode.WinUIComposition, Win32CompositionMode.DirectComposition],
//                 // ShouldRenderOnUIThread = true
//             });
//         }
//
//         if (OperatingSystem.IsLinux())
//         {
//             return builder.With(new X11PlatformOptions
//             {
//                 OverlayPopups = true
//             });
//         }
//
//         if (OperatingSystem.IsMacOS())
//         {
//             return builder.With(new AvaloniaNativePlatformOptions
//             {
//                 OverlayPopups = true
//             });
//         }
//
//         return builder;
//     }
// }
//

// [Window, MacOS, Linux(Desktop/DRM)]
//
// App.axml.cs
// MainWindowViewModel vm = new();
//
// MainWindow mainWindow = new()
// {
//     DataContext = vm
// };
//
// switch (ApplicationLifetime)
// {
//     case IClassicDesktopStyleApplicationLifetime desktop:
//         DisableAvaloniaDataAnnotationValidation();
//         desktop.MainWindow = mainWindow;
//         break;
//     case ISingleViewApplicationLifetime singleView:
//     {
//         Control? content = mainWindow.Content as Control;
//         mainWindow.Content = null;
//         singleView.MainView = content;
//         break;
//     }
// }