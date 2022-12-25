using Caliburn.Micro;
using MvvmExample.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace MvvmExample;

public class Bootstrapper : BootstrapperBase
{
    private readonly SimpleContainer mSimpleContainer;

    public Bootstrapper()
    {
        mSimpleContainer = new SimpleContainer();
        Initialize();
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    protected override void Configure()
    {
        mSimpleContainer.Singleton<IWindowManager, WindowManager>();

        GetType().Assembly.GetTypes().Where(type => type.IsClass).Where(type => type.Name.EndsWith("Service")).ToList()
            .ForEach(vmType => mSimpleContainer.RegisterSingleton(vmType.GetInterfaces().First(), nameof(vmType), vmType));

        GetType().Assembly.GetTypes().Where(type => type.IsClass).Where(type => type.Name.EndsWith("ViewModel", StringComparison.CurrentCulture)).ToList()
            .ForEach(vmType => mSimpleContainer.RegisterSingleton(vmType, nameof(vmType), vmType));

        GetType().Assembly.GetTypes().Where(type => type.IsClass).Where(type => type.Name.EndsWith("View", StringComparison.CurrentCulture)).ToList()
            .ForEach(vmType => mSimpleContainer.RegisterSingleton(vmType, nameof(vmType), vmType));
    }

    protected override async void OnStartup(object sender, StartupEventArgs e)
    {
        await DisplayRootViewForAsync(typeof(ShellViewModel)).ConfigureAwait(false);
    }

    protected override object GetInstance(Type service, string key)
    {
        return mSimpleContainer.GetInstance(service, key);
    }

    protected override IEnumerable<object> GetAllInstances(Type service)
    {
        return mSimpleContainer.GetAllInstances(service);
    }

    protected override void BuildUp(object instance)
    {
        mSimpleContainer.BuildUp(instance);
    }
}
