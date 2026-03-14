using EasyConnect.Models;
using EasyConnect.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace EasyConnect.Controllers
{
    public class HttpController
    {
        public event Func<object, EventArgs, string, List<Files>, Task> OpenServerEvent;
        private readonly AdbService _AdbService;
        private readonly InfoController _InfoController;
        private readonly NetworkService _NetworkServices;
        private HttpListener _HttpListener;

        public HttpController(AdbService adbService, NetworkService networkService, InfoController infoController)
        {
            _AdbService = adbService;
            _NetworkServices = networkService;
            _InfoController = infoController;
        }
        protected virtual async Task OnOpenServerEvent(string bundle, List<Files> files)
        {
            if (OpenServerEvent == null) return;

            var handlers = OpenServerEvent.GetInvocationList()
                                           .Cast<Func<object, EventArgs,string, List<Files>, Task>>();

            foreach (var handler in handlers)
            {
                await handler(this, EventArgs.Empty, bundle, files);
            }
        }

        public async Task<Manifest> StartServerConnection()
        {
            _ = StartServerListener();
            var fileNames = await GetFileNameFromServer();
            _NetworkServices.SetBundleId(fileNames.bundle);
            await OnOpenServerEvent(_NetworkServices.GetBundleId(), fileNames.files);
            return fileNames;
        }
        private async Task<Manifest> GetFileNameFromServer()
        {
            Manifest files = new Manifest();
            var ipServer = _NetworkServices.GetServerIp();
            var portServer = _NetworkServices.GetServerPort();
            string url = $"http://{ipServer}:{portServer}/manifest.json";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var code = await client.GetAsync(url);
                    var json = await client.GetStringAsync(url);

                    files = JsonSerializer.Deserialize<Manifest>(json);
                    return files;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return null;
                }
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

            while (true)
            {
                var context = await _HttpListener.GetContextAsync();
                await Task.Run(() => HandleRequest(context));
            }
        }
        private async Task HandleRequest(HttpListenerContext context)
        {
            try
            {
                if (context.Request.HttpMethod == "POST")
                {
                    using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
                    {
                        string body = await reader.ReadToEndAsync();

                        Debug.WriteLine("JSON recibido:");
                        Debug.WriteLine(body);

                        // Parsear JSON
                        var deviceReport = JsonSerializer.Deserialize<DeviceReport>(body);
                        _AdbService.UpdateDevice(deviceReport);

                        context.Response.StatusCode = 200;
                    }
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
