using Stylet;
using StyletTest.AppConfig;
using StyletTest.Helpers;
using StyletTest.ViewModels;
using StyletTest.Views;
using System.Linq;
using System.Windows.Threading;

namespace StyletTest;

public class ShellBootstrapper : Bootstrapper<ShellViewModel>
{
    protected override void OnStart()
    {
        LogHelper.Logger.Info("***** 프로그램 시작 : 준비 중 .....  *****");
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
    }

    protected override void Configure()
    {
        // OnStart -> ConfigureIoC -> Configure(*) -> OnLaunch
        base.Configure();
        IoC.GetInstance = Container.Get;
        IoC.GetAllInstances = Container.GetAll;
        IoC.BuildUp = Container.BuildUp;

        AppStartStop.Start().AWait(AppStartStop.Completed, AppStartStop.Error);
        IoC.Get<ShellView>().Closing += AppStartStop.OnClosing;
        IoC.Get<ShellView>().SourceInitialized += AppStartStop.OnSourceInitialized;
    }

    protected override void OnLaunch()
    {
        LogHelper.Logger.Info("***** 프로그램 시작(OnLaunch) : OK *****");
    }

    //protected override void OnExit(ExitEventArgs e)
    //{
    //    LogHelper.Logger.Info("===== 프로그램 종료(OnExit) =====");
    //    Environment.Exit(0);
    //}

    protected override void OnUnhandledException(DispatcherUnhandledExceptionEventArgs e)
    {
        LogHelper.Logger.Fatal("#################### Global Exception : " + e.Exception.Message);
        e.Handled = true;
    }
}