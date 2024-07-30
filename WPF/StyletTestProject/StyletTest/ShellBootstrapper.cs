using Stylet;
using StyletTest.AppConfig;
using StyletTest.Helpers;
using StyletTest.ViewModels;
using StyletTest.Views;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace StyletTest;

public class ShellBootstrapper : Bootstrapper<ShellViewModel>
{
    protected override void OnStart()
    {
        LogHelper.Logger.Info("***** 프로그램 시작 : OnStart *****");
    }

    protected override void ConfigureIoC(StyletIoC.IStyletIoCBuilder builder)
    {
        base.ConfigureIoC(builder);

        GetType().Assembly.GetTypes().Where(type => type.IsClass).Where(type => type.Name.EndsWith("Service")).ToList()
            .ForEach(vmType => builder.Bind(vmType.GetInterfaces().First()).To(vmType).InSingletonScope());

        GetType().Assembly.GetTypes().Where(type => type.IsClass).Where(type => type.Name.EndsWith("ViewModel")).ToList()
            .ForEach(vmType => builder.Bind(vmType).ToSelf().InSingletonScope());

        GetType().Assembly.GetTypes().Where(type => type.IsClass).Where(type => type.Name.EndsWith("View")).ToList()
            .ForEach(vmType => builder.Bind(vmType).ToSelf().InSingletonScope());

        //builder.Bind<IWindowManager>().To<WindowManager>().InSingletonScope();
    }

    protected override void Configure()
    {
        base.Configure();
        IoC.GetInstance = Container.Get;
        IoC.GetAllInstances = Container.GetAll;
        IoC.BuildUp = Container.BuildUp;

        IoC.Get<ShellView>().InitializeComponent();
        AppSettings.Start().AWait(AppSettings.Completed, AppSettings.Error);
    }

    protected override void OnLaunch()
    {
        LogHelper.Logger.Info("***** 프로그램 시작 : OnLaunch *****");
    }

    protected override void OnExit(ExitEventArgs e)
    {
        LogHelper.Logger.Info($"===== 프로그램 종료 : OnExit : {e.ApplicationExitCode} =====");
    }

    protected override void OnUnhandledException(DispatcherUnhandledExceptionEventArgs e)
    {
        LogHelper.Logger.Fatal("#################### Global Exception : " + e.Exception.Message);
        e.Handled = true;
    }
}