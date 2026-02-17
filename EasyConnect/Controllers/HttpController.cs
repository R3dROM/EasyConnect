using EasyConnect.Models;
using EasyConnect.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyConnect.Controllers
{
    public class HttpController
    {
        private readonly AdbService _AdbService;
        private WindowVariables _WindowVariables;
        private HttpListener _HttpListener;

        public HttpController(AdbService adbService, WindowVariables windowVariables)
        {
            _AdbService = adbService;
            _WindowVariables = windowVariables;
        }


        public async void StartServerConnection(ListBox listBox, Label label)
        {
            try
            {
                listBox.Items.Clear();
                var fileNames = await GetFileNameFromServer();
                label.Text = File.ReadAllText("C:\\Users\\Univrse\\EXPERIENCE\\DEPLOY\\bundleID.txt");
                _WindowVariables.SetBundleId(label.Text);
                foreach (var file in fileNames)
                {
                    listBox.Items.Add(file.Remove(0, 2));
                }
                StartServerListener();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
        private async Task<List<string>> GetFileNameFromServer()
        {
            var files = new List<string>();
            var ipServer = _WindowVariables.GetServerIp();
            var portServer = _WindowVariables.GetServerPort();
            string url = $"http://{ipServer}:{portServer}/";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var code = await client.GetAsync(url);
                    Debug.WriteLine(code.Content.ToString());
                    string html = await client.GetStringAsync(url);

                    // Extrae href="archivo"
                    Regex regex = new Regex("href=\"([^\"]+)\"");
                    MatchCollection matches = regex.Matches(html);

                    foreach (Match match in matches)
                    {
                        string name = match.Groups[1].Value;
                        // Ignorar navegación
                        if (name == "../" || name.EndsWith("/") || name.EndsWith("com") || name.EndsWith("asc") || name.EndsWith("desc"))
                            continue;
                        //Debug.WriteLine(name);
                        files.Add(name);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            return files;
        }

        private async void StartServerListener()
        {
            if (_HttpListener.IsListening)
                return;
            _HttpListener = new HttpListener();
            _HttpListener.Prefixes.Add("http://127.0.0.1:7777/");
            _HttpListener.Start();

            Debug.WriteLine($"ESCUCHANDO EN EL PUERTO");

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
                        var report = JsonSerializer.Deserialize<DeviceReport>(body);

                        Debug.WriteLine($"Device IP: {report.deviceId}");
                        Debug.WriteLine($"Download Status: {report.downloadStatus}");
                        Debug.WriteLine($"Install Status: {report.installStatus}");

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
