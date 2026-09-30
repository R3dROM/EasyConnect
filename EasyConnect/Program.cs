using EasyConnect.Controllers;
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
            services.AddSingleton<NetworkState>();
            services.AddSingleton<NetworkPresentation>();
            services.AddSingleton<WebSocketManager>();
            services.AddSingleton<ConsoleService>();
            services.AddSingleton<DeviceManager>();
            services.AddSingleton<DeploymentService>();
            services.AddSingleton<ConnectionService>();
            services.AddSingleton<InfoController>();
            services.AddSingleton<AdbService>();
            services.AddSingleton<NetworkService>();
            services.AddSingleton<WebSocketService>();
            services.AddSingleton<DeployController>();
            services.AddSingleton<HttpController>();
            services.AddSingleton<AppManager>();
            services.AddSingleton<DevicePresentation>();
            services.AddSingleton<WINDOW>();
            services.AddSingleton<JobTrackerService>();
            services.AddSingleton<Initializer>();
            services.AddSingleton<NetworkConfigurationService>();

            ServiceProvider = services.BuildServiceProvider();

            var initializer = ServiceProvider.GetService<Initializer>();
            var mainForm = ServiceProvider.GetService<WINDOW>();
            if(mainForm != null && initializer != null)
            {
                Application.Run(initializer);
            }
        }
    }
}
