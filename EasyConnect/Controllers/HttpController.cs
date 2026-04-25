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
        NetworkService networkService
        )
    {
        private readonly NetworkService _NetworkServices = networkService;


        private HttpListener? _HttpListener;

        public async Task<DeviceCommandResult> StartServerListener()
        {
            try
            {
                _ = StartListener();
                return new DeviceCommandResult
                { 
                    Ip = _NetworkServices.serverIp,
                    ExitCode = 0,
                    Output = "HTTP listener/Handler Service Ready"
                };
            }
            catch (Exception)
            {
                return new DeviceCommandResult
                {
                    Ip = _NetworkServices.serverIp,
                    ExitCode = -1,
                    Output = "HTTP listener/Handler Service Fail"
                };
                throw;
            }
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
        private async Task StartListener()
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
                if (context.Request.HttpMethod == "PUT")
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
                context.Response.StatusCode = 500;
                throw new Exception($"ERROR",ex);
            }
            finally
            {
                context.Response.Close();
            }
        }
    }
}
