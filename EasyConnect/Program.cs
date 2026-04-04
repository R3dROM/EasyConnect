using EasyConnect.Controllers;
using EasyConnect.Services;
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
            services.AddSingleton<AppInitializer>();
            services.AddSingleton<WINDOW>();
            services.AddSingleton<NetworkingConfiguration>();
            services.AddSingleton<NetworkConfigurationService>();

            ServiceProvider = services.BuildServiceProvider();

            var mainForm = ServiceProvider.GetService<WINDOW>();
            var netConfigForm = ServiceProvider.GetService<NetworkingConfiguration>();
            if(mainForm != null && netConfigForm != null)
            {
                Application.Run(mainForm);
            }
        }
    }
}
