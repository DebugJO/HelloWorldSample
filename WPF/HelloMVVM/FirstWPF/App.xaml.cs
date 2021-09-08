using FirstWPF.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Windows;

namespace FirstWPF
{
    public partial class App
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder().ConfigureServices((context, services) =>
            {
                context.HostingEnvironment.ApplicationName = "FirstWPF";
                ConfigureServices(services);
            }).Build();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            Current.GetType().Assembly.GetTypes().Where(type => type.IsClass).Where(type => type.Name.EndsWith("Services")).ToList()
                .ForEach(vmType => services.AddSingleton(vmType.GetInterfaces().First(), vmType));

            Current.GetType().Assembly.GetTypes().Where(type => type.IsClass).Where(type => type.Name.EndsWith("ViewModel")).ToList()
                .ForEach(vmType =>
                {
                    services.AddSingleton(vmType);
                    services.BuildServiceProvider().GetRequiredService(vmType);
                });

            Current.GetType().Assembly.GetTypes().Where(type => type.IsClass).Where(type => type.Name.EndsWith("View")).ToList()
                .ForEach(vmType =>
                {
                    services.AddSingleton(vmType);
                    services.BuildServiceProvider().GetRequiredService(vmType);
                });
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();
            Current.Windows.OfType<MainView>().First().Show();
            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync();
            }

            base.OnExit(e);
        }
    }
}