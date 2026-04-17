using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using MyApp.ViewModels;
using MyApp.Views;
using MyAppLib.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MyApp;

public class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;

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

        ServiceCollection serviceCollection = new();
        ConfigureServices(serviceCollection);
        Services = serviceCollection.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            LogHelper.Debug("Desktop : Start(3/3) ...");
            DisableAvaloniaDataAnnotationValidation();

            MainView view = Services.GetRequiredService<MainView>();
            MainViewModel viewModel = Services.GetRequiredService<MainViewModel>();
            view.DataContext = viewModel;
            desktop.MainWindow = view;

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
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        List<Assembly> assemblies = GetAllAssemblies();
        LogHelper.Debug($"AppService : Total Assemblies found : {assemblies.Count}");
        List<Type?> allTypes = assemblies.SelectMany(a =>
        {
            try
            {
                Type[] types = a.GetTypes();
                LogHelper.Debug($"AppService : Assembly {a.GetName().Name} has {types.Length} types.");
                return types;
            }
            catch (ReflectionTypeLoadException ex)
            {
                foreach (Exception? le in ex.LoaderExceptions)
                {
                    LogHelper.Error($"AppService : Type Load Error in {a.GetName().Name}: {le?.Message}");
                }

                return ex.Types.Where(t => t != null);
            }
            catch (Exception ex)
            {
                LogHelper.Error($"AppService : Error getting types from {a.GetName().Name} : {ex.Message}");
                return Type.EmptyTypes;
            }
        }).ToList();

        LogHelper.Debug($"AppService : Total types collected: {allTypes.Count}");

        foreach (Type? type in allTypes)
        {
            bool isClass = type is { IsClass: true, IsAbstract: false };

            if (!isClass)
            {
                continue;
            }

            if (type != null && type.Name.EndsWith("Service"))
            {
                Type? interfaceType = type.GetInterfaces().FirstOrDefault();

                if (interfaceType == null)
                {
                    continue;
                }

                services.AddSingleton(interfaceType, type);
                LogHelper.Debug($"AppService : Service Registered: {type.Name}");
            }
            else if (type != null && (type.Name.EndsWith("ViewModel") || type.Name.EndsWith("View")))
            {
                services.AddSingleton(type);
                LogHelper.Debug($"AppService : UI Registered: {type.Name}");
            }
        }
    }

    private List<Assembly> GetAllAssemblies()
    {
        string path = AppContext.BaseDirectory;
        Assembly executingAssembly = Assembly.GetExecutingAssembly();
        string mainName = executingAssembly.GetName().Name ?? string.Empty;
        // 제외 파일
        string[] excludeKeywords = ["Aot", "Native", "OtherDll", "Test"];
        List<Assembly> assemblies = [executingAssembly];
        // 프로젝트 이름 기준으로 등록
        string searchPattern = $"{mainName}*.dll";
        string[] dllFiles = Directory.GetFiles(path, searchPattern);

        foreach (string dll in dllFiles)
        {
            if (string.Equals(Path.GetFullPath(dll), Path.GetFullPath(executingAssembly.Location), StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (excludeKeywords.Any(keyword => dll.Contains(keyword, StringComparison.OrdinalIgnoreCase)))
            {
                continue;
            }

            try
            {
                assemblies.Add(Assembly.LoadFrom(dll));
                LogHelper.Debug($"AppService : Load Assembly : {dll}");
            }
            catch (Exception ex)
            {
                LogHelper.Error($"AppService : Failed to load assembly {dll}: {ex.Message}");
            }
        }

        return assemblies.Distinct().ToList();
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