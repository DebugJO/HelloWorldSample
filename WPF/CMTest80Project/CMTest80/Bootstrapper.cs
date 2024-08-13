using Caliburn.Micro;
using CMTest80.Helpers;
using CMTest80.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace CMTest80;

public class Bootstrapper : BootstrapperBase
{
    private readonly SimpleContainer mContainer;

    public Bootstrapper()
    {
        mContainer = new SimpleContainer();
        Initialize();
    }

    protected override void Configure()
    {
        mContainer.Instance(mContainer);
        mContainer.Singleton<IWindowManager, WindowManager>();
        mContainer.Singleton<IEventAggregator, EventAggregator>();

        GetType().Assembly.GetTypes().Where(type => type.IsClass).Where(type => type.Name.EndsWith("Service")).ToList()
            .ForEach(vmType => mContainer.RegisterSingleton(vmType.GetInterfaces().First(), nameof(vmType), vmType));

        GetType().Assembly.GetTypes().Where(type => type.IsClass).Where(type => type.Name.EndsWith("ViewModel")).ToList()
            .ForEach(vmType => mContainer.RegisterSingleton(vmType, nameof(vmType), vmType));

        GetType().Assembly.GetTypes().Where(type => type.IsClass).Where(type => type.Name.EndsWith("View")).ToList()
            .ForEach(vmType => mContainer.RegisterSingleton(vmType, nameof(vmType), vmType));
    }

    protected override object GetInstance(Type service, string key)
    {
        return mContainer.GetInstance(service, key);
    }

    protected override IEnumerable<object> GetAllInstances(Type service)
    {
        return mContainer.GetAllInstances(service);
    }

    protected override void BuildUp(object instance)
    {
        mContainer.BuildUp(instance);
    }

    protected override async void OnStartup(object sender, StartupEventArgs e)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        AppStartup.AppStart().AWait(AppStartup.Completed, AppStartup.Error);
        await DisplayRootViewForAsync(typeof(ShellViewModel));
    }

    protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        LogHelper.Logger.Fatal("#################### Global Exception : " + e.Exception.Message);
        e.Handled = true;
    }
}

/*
 부팅순서
    App
    Bootstrapper.Configure
    Bootstrapper.Bootstrapper
    Bootstrapper.GetInstance
    ShellViewModel
    Bootstrapper.GetAllInstances
    ShellView
    Bootstrapper.OnStartup
*/