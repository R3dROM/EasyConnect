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
    public class HttpController(AdbService adbService, NetworkService networkService, InfoController infoController)
    {
        public event Func<object, EventArgs, string, List<Files>, Task>? OpenServerEvent;
        private readonly AdbService _AdbService = adbService;
        private readonly InfoController _InfoController = infoController;
        private readonly NetworkService _NetworkServices = networkService;
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

        public async Task<Manifest?> StartServerConnection()
        {
            _ = StartServerListener();
            var fileNames = await GetFileNameFromServer();
            if (fileNames != null)
            {
                _NetworkServices.bundle = fileNames.bundle;
                await OnOpenServerEvent(_NetworkServices.bundle, fileNames.files);
                return fileNames;
            }
            return null;
        }
        private async Task<Manifest?> GetFileNameFromServer()
        {
            var ipServer = _NetworkServices.serverIp;
            var portServer = _NetworkServices.serverPort;
            string url = $"http://{ipServer}:{portServer}/manifest.json";

            using HttpClient client = new();
            try
            {
                var code = await client.GetAsync(url);
                var json = await client.GetStringAsync(url);

                var files = JsonSerializer.Deserialize<Manifest>(json);
                return files;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
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
                await _HttpListener.GetContextAsync();
                //await Task.Run(() => HandleRequest(context));
            }
        }
        //private async Task HandleRequest(HttpListenerContext context)
        //{
        //    try
        //    {
        //        if (context.Request.HttpMethod == "POST")
        //        {
        //            using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
        //            {
        //                string body = await reader.ReadToEndAsync();

        //                Debug.WriteLine("JSON recibido:");
        //                Debug.WriteLine(body);

        //                // Parsear JSON
        //                var deviceReport = JsonSerializer.Deserialize<DeviceReport>(body);
        //                _AdbService.UpdateDevice(deviceReport);

        //                context.Response.StatusCode = 200;
        //            }
        //        }
        //        else
        //        {
        //            context.Response.StatusCode = 405;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex);
        //        context.Response.StatusCode = 500;
        //    }
        //    finally
        //    {
        //        context.Response.Close();
        //    }
        //}
    }
}
