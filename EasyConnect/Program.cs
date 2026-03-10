using EasyConnect.Controllers;
using EasyConnect.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace EasyConnect
{
    internal static class Program
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var services = new ServiceCollection();
            services.AddSingleton<InfoController>();
            services.AddSingleton<AdbService>();
            services.AddSingleton<ConsoleService>();
            services.AddSingleton<NetworkService>();
            services.AddSingleton<WebSocketService>();
            services.AddSingleton<DeployController>();
            services.AddSingleton<HttpController>();
            services.AddSingleton<AppInitializer>();
            services.AddSingleton<WINDOW>();

            ServiceProvider = services.BuildServiceProvider();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var mainForm = ServiceProvider.GetService<WINDOW>();
            Application.Run(mainForm);
        }
    }
}
