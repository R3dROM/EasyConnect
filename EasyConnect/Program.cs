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

            ServiceProvider = services.BuildServiceProvider();

            var mainForm = ServiceProvider.GetService<WINDOW>();
            if(mainForm != null)
                Application.Run(mainForm);
        }
    }
}
