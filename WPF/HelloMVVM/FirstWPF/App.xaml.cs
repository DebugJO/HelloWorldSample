using FirstWPF.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Windows;
using FirstWPF.Services;

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
            services.AddSingleton<MainView>();
            services.AddSingleton<IDateTimeServices, DateTimeServices>();
            Current.GetType().Assembly.GetTypes().Where(type => type.IsClass).Where(type => type.Name.EndsWith("ViewModel")).ToList().ForEach(vmType => services.AddSingleton(vmType));
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();
            _host.Services.GetRequiredService<MainView>().Show();
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