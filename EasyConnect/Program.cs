using EasyConnect.Controllers;
using EasyConnect.Handler;
using EasyConnect.Legacy;
using EasyConnect.Managers;
using EasyConnect.Presentation;
using EasyConnect.Services;
using EasyConnect.State;
using Microsoft.Extensions.DependencyInjection;

namespace EasyConnect
{
    internal static class Program
    {
        public static IServiceProvider? ServiceProvider { get; private set; }
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            var services = new ServiceCollection();
            services.AddSingleton<NetworkPresentation>();
            services.AddSingleton<NetworkManager>();
            services.AddSingleton<NetworkService>();

            services.AddSingleton<WebSocketManager>();
            services.AddSingleton<WebSocketService>();

            services.AddSingleton<ConfigurationService>();
            services.AddSingleton<ConsoleService>();

            services.AddSingleton<DeviceManager>();
            services.AddSingleton<DeviceService>();

            services.AddSingleton<DeploymentService>();
            services.AddSingleton<ConnectionService>();

            services.AddSingleton<DeployController>();
            services.AddSingleton<ServerController>();
            services.AddSingleton<AllDevicesPresentation>();

            services.AddSingleton<JobTrackerManager>();
            services.AddSingleton<JobTrackerService>();
            services.AddSingleton<JobTrackerHandler>();

            services.AddSingleton<DiscoveryService>();
            services.AddSingleton<UiHandler>();

            services.AddSingleton<LauncherController>();

            services.AddSingleton<EasyLinkView>();
            services.AddSingleton<LauncherView>();

            ServiceProvider = services.BuildServiceProvider();

            var initializer = ServiceProvider.GetService<LauncherView>();
            var mainForm = ServiceProvider.GetService<EasyLinkView>();
            if(mainForm != null && initializer != null)
            {
                Application.Run(initializer);
            }
        }
    }
}
