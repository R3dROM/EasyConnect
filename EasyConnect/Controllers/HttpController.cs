using EasyConnect.Models;
using EasyConnect.Services;
using EasyConnect.State;
using Makaretu.Dns;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace EasyConnect.Controllers
{
    public class HttpController
        (NetworkState networkState)
    {
        private readonly NetworkState _NetworkState = networkState;
        private ServiceDiscovery? sd;
        private ServiceProfile serviceProfile = new(
                "Easyconnect Server",
                "_easyconnect._tcp",
                8000
                );

        private HttpListener? _HttpListener;

        public async Task<DeviceCommandResult> StartServerListener()
        {
            try
            {
                _ = StartListener();
                return new DeviceCommandResult
                { 
                    Ip = _NetworkState.MyIpAddress?.ToString() ?? "",
                    ExitCode = 0,
                    Output = "HTTP listener/Handler Service Ready"
                };
            }
            catch (Exception)
            {
                return new DeviceCommandResult
                {
                    Ip = _NetworkState.MyIpAddress?.ToString() ?? "",
                    ExitCode = -1,
                    Output = "HTTP listener/Handler Service Fail"
                };
            }
        }
        public async Task<Manifest?> GetManifestFromServer()
        {
            var ipServer = _NetworkState.MyIpAddress?.ToString() ?? "";
            var portServer = _NetworkState.ServerPort;
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
                var ipServer = _NetworkState.MyIpAddress?.ToString() ?? "";
                var portServer = _NetworkState.ServerPort;
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
        private async Task<DeviceCommandResult> StartListener()
        {
            if (_HttpListener != null && _HttpListener.IsListening)
                return new DeviceCommandResult
                {
                    Ip = "127.0.0.1",
                    ExitCode = 0,
                    Output = "Server Already Listening"
                };
            _HttpListener = new HttpListener();
            _HttpListener.Prefixes.Add("http://127.0.0.1:7777/");
            _HttpListener.Start();

            SemaphoreSlim sempahore = new(10);
            while (true)
            {
                var context = await _HttpListener.GetContextAsync();

                await sempahore.WaitAsync();
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await HandleRequest(context);
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        sempahore.Release();
                    }
                });
            }
        }
        private async Task HandleRequest(HttpListenerContext context)
        {
            //try
            //{
            //    if (context.Request.HttpMethod == "POST")
            //    {
            //        using var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding);
            //        string json = await reader.ReadToEndAsync();
            //        var jsonToMessage = JsonSerializer.Deserialize<DeploymentInformation>(json, _jsonSerializerOptions);
            //        Debug.WriteLine(json);
            //        _DeviceManager.AddDeviceFromMessageInfo(jsonToMessage);
            //    }
            //    else if (context.Request.HttpMethod == "PUT")
            //    {
            //        using var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding);
            //        string json = await reader.ReadToEndAsync();
            //        var basePath = context.Request.Headers["X-Base-Path"];
            //        var urlPath = context.Request.Url!.LocalPath;
            //        Debug.WriteLine($"{basePath}/{urlPath}");
            //        var fullPath = Path.Combine(
            //            basePath!,
            //            urlPath.TrimStart('/').Replace("/", "\\")
            //            );

            //        fullPath = Path.GetFullPath(fullPath);

            //        await File.WriteAllTextAsync(fullPath, json);
            //    }
            //    else
            //    {
            //        context.Response.StatusCode = 405;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    context.Response.StatusCode = 500;
            //    throw new Exception($"ERROR", ex);
            //}
            //finally
            //{
            //    context.Response.Close();
            //}
        }
        public async Task InitializemDnsService()
        {
            Debug.WriteLine("ZEROCONF");

            serviceProfile.AddProperty("ipAddress", _NetworkState.MyIpAddress?.ToString());
            serviceProfile.AddProperty("downloadPort", _NetworkState.ServerPort);
            serviceProfile.AddProperty("websocketPort", _NetworkState.WebSocketPort);
            Debug.WriteLine($"ipAddress: {_NetworkState.MyIpAddress?.ToString()}");
            sd = new ServiceDiscovery();

            sd.Advertise(serviceProfile);
        }
        public async Task ShutDownService()
        {
            sd?.Unadvertise(serviceProfile);
            sd?.Dispose();
        }
    }
}
