using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Win32;
using MyApp.ViewModels;
using MyApp.Views;
using MyAppLib.Helpers;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        AppDomain.CurrentDomain.UnhandledException += (_, args) =>
        {
            if (args.ExceptionObject is Exception ex)
            {
#if DEBUG
                LogHelper.Fatal($"전역에러(Crash) : {ex.Message}\n{ex.StackTrace}");
#else
                LogHelper.Fatal($"전역에러(UI) : {ex.Message}");
#endif
            }

            LogHelper.Shutdown();
        };

        TaskScheduler.UnobservedTaskException += (_, args) =>
        {
            LogHelper.Fatal($"전역에러(Async) : {args.Exception.Message}");
            args.SetObserved();
        };

        if (Design.IsDesignMode)
        {
            base.OnFrameworkInitializationCompleted();
            return;
        }

        string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string lockFile = Path.Combine(localAppData, "MyApp_2026_001_shutdown.lock");

        if (OperatingSystem.IsWindows())
        {
            SystemEvents.SessionEnding += (_, _) =>
            {
                try
                {
                    LogHelper.Info("====== 시스템 종료 신호 수신: 즉시 종료 ======");

                    if (File.Exists(lockFile))
                    {
                        File.Delete(lockFile);
                    }

                    LogHelper.Shutdown();
                    Environment.Exit(0);
                }
                catch
                {
                    Environment.Exit(0);
                }
            };
        }

        try
        {
            if (File.Exists(lockFile))
            {
                string lastRunTime = File.ReadAllText(lockFile);
                LogHelper.Fatal($"[비정상 종료 감지] 파일 발견됨 : {lastRunTime}");
                File.Delete(lockFile);
                LogHelper.Debug("[비정상 종료 감지] 이전 lock 파일 삭제 완료.");
            }

            File.WriteAllText(lockFile, DateTime.Now.ToString(
                "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)
            );
        }
        catch (Exception ex)
        {
            LogHelper.Error($"Lock 파일 처리 중 에러 : {ex.Message}");
        }

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            LogHelper.Debug("Desktop : Start(3) ...");
            DisableAvaloniaDataAnnotationValidation();

            desktop.Exit += (_, _) =>
            {
                try
                {
                    if (File.Exists(lockFile))
                    {
                        File.Delete(lockFile);
                    }
                }
                catch
                {
                    // ignored
                }

                LogHelper.Debug("Desktop : Complete : 프로그램 종료");
                LogHelper.Shutdown();
            };

            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
        }

        // [Window, MacOS, Linux(Desktop/DRM)]
        //
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

        base.OnFrameworkInitializationCompleted();
    }

    private static void DisableAvaloniaDataAnnotationValidation()
    {
        DataAnnotationsValidationPlugin[] dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        foreach (DataAnnotationsValidationPlugin plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}