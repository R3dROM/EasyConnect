using EasyConnect.Models;
using EasyConnect.Services;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.Json;

namespace EasyConnect.Controllers
{
    public class HttpController
        (
        AdbService adbService, 
        NetworkService networkService, 
        InfoController infoController, 
        ConsoleService _consoleService
        )
    {
        public event Func<object, EventArgs, string, List<Files>, Task>? OpenServerEvent;
        private readonly AdbService _AdbService = adbService;
        private readonly InfoController _InfoController = infoController;
        private readonly NetworkService _NetworkServices = networkService;
        private readonly string caddy = @"C:\Users\UNIVRSE_Santiago\TOOLS\Caddy\caddy_2.10.2_windows_amd64\caddy.exe";
        private readonly string scriptsPath = @"C:\scripts\generate-manifest.ps1";

        private HttpListener? _HttpListener;

        protected virtual async Task OnOpenServerEvent(string bundle, List<Files> files)
        {
            if (OpenServerEvent == null) return;

            var handlers = OpenServerEvent.GetInvocationList()
                                           .Cast<Func<object, EventArgs, string, List<Files>, Task>>();

            foreach (var handler in handlers)
            {
                await handler(this, EventArgs.Empty, bundle, files);
            }
        }
        public async Task StartServerConnection()
        {
            _ = _consoleService.RunCommandAsync(caddy, $"start --config \"C:\\Users\\UNIVRSE_Santiago\\TOOLS\\Caddy\\caddy_2.10.2_windows_amd64\\Caddyfile");
            _ = StartServerListener();
        }
        public async Task StopServerConnection()
        {
            await _consoleService.RunCommandAsync(caddy, $"stop");
        }
        public async Task<Manifest?> GetManifestFromServer()
        {
            var ipServer = _NetworkServices.serverIp;
            var portServer = _NetworkServices.serverPort;
            string url = $"http://{ipServer}:{portServer}/manifest.json";

            using HttpClient client = new();
            try
            {
                var code = await client.GetAsync(url);
                if (code.IsSuccessStatusCode)
                {
                    var json = await client.GetStringAsync(url);

                    var files = JsonSerializer.Deserialize<Manifest>(json);
                    if (files != null)
                        return files;
                }
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
        public async Task PUTConfigToServer(string json, string path)
        {
            using HttpClient client = new();
            try
            {
                var ipServer = _NetworkServices.serverIp;
                var portServer = _NetworkServices.serverPort;
                string url = $"http://{ipServer}:{portServer}/upload";
                var dest = Path.Combine(url, path).Replace("\\", "/");
                using var content = new StringContent(json, new System.Text.UTF8Encoding(false), "application/json");
                HttpResponseMessage response = await client.PutAsync(dest, content);

                string responseBody = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Respuest exitosa: ");
                }
                else
                {
                    Debug.WriteLine($"Error: {response.StatusCode}");
                }
                Debug.WriteLine(responseBody);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR EN EL PUT: {ex.Message}");
                return;
            }
        }
        private async Task StartServerListener()
        {
            if (_HttpListener != null && _HttpListener.IsListening)
                return;
            _HttpListener = new HttpListener();
            _HttpListener.Prefixes.Add("http://127.0.0.1:7777/");
            _HttpListener.Start();

            Debug.WriteLine($"ESCUCHANDO EN EL PUERTO 7777");
            JsonSerializerOptions options = new()
            {
                WriteIndented = true
            };
            SemaphoreSlim sempahore = new(10);
            while (true)
            {
                var context = await _HttpListener.GetContextAsync();

                await sempahore.WaitAsync();
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await HandleRequest(context, options);
                    }
                    finally
                    {
                        sempahore.Release();
                    }
                });
            }
        }
        private async Task HandleRequest(HttpListenerContext context, JsonSerializerOptions options)
        {
            try
            {
                if (context.Request.HttpMethod == "POST")
                {
                    using var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding);
                    string body = await reader.ReadToEndAsync();

                    Debug.WriteLine("JSON recibido:");
                    Debug.WriteLine(body);

                    // Parsear JSON
                    var deviceReport = JsonSerializer.Deserialize<DeviceReport>(body);
                    //_AdbService.UpdateDevice(deviceReport);

                    context.Response.StatusCode = 200;
                }
                else if (context.Request.HttpMethod == "PUT")
                {
                    using var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding);
                    string json = await reader.ReadToEndAsync();
                    var basePath = context.Request.Headers["X-Base-Path"];
                    var urlPath = context.Request.Url!.LocalPath;
                    Debug.WriteLine($"{basePath}/{urlPath}");
                    var fullPath = Path.Combine(
                        basePath!,
                        urlPath.TrimStart('/').Replace("/", "\\")
                        );

                    fullPath = Path.GetFullPath(fullPath);

                    await File.WriteAllTextAsync(fullPath, json);
                }
                else
                {
                    context.Response.StatusCode = 405;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                context.Response.StatusCode = 500;
            }
            finally
            {
                context.Response.Close();
            }
        }
    }
}
