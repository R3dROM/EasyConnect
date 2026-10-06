using EasyConnect.Models.Action;
using EasyConnect.Services;
using System.Diagnostics;

namespace EasyConnect.Controllers
{
    public class ServerController
        (DiscoveryService _discoveryService,
        NetworkService _networkService)
    {
        private readonly DiscoveryService _discoveryService = _discoveryService;
        private readonly NetworkService _networkService = _networkService;

        public async Task<ActionResult> StartServer()
        {
            try
            {
                await _networkService.StartNetwork();
                await _discoveryService.InitializemDnsService();
                //_ = StartListener();
                return new ActionResult
                { 
                    Ip = "127.0.0.1",
                    ExitCode = 0,
                    Output = "Server Ready"
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"START SERVER EXCEPTION {ex}");
                return new ActionResult
                {
                    Ip = "127.0.0.1",
                    ExitCode = -1,
                    Output = "Server Fail"
                };
            }
        }
        //private async Task<DeviceCommandResult> StartListener()
        //{
        //    if (_HttpListener != null && _HttpListener.IsListening)
        //        return new DeviceCommandResult
        //        {
        //            Ip = "127.0.0.1",
        //            ExitCode = 0,
        //            Output = "Server Already Listening"
        //        };
        //    _HttpListener = new HttpListener();
        //    _HttpListener.Prefixes.Add("http://127.0.0.1:7777/");
        //    _HttpListener.Start();

        //    SemaphoreSlim sempahore = new(10);
        //    while (true)
        //    {
        //        var context = await _HttpListener.GetContextAsync();

        //        await sempahore.WaitAsync();
        //        _ = Task.Run(async () =>
        //        {
        //            try
        //            {
        //                await HandleRequest(context);
        //            }
        //            catch
        //            {
        //                throw;
        //            }
        //            finally
        //            {
        //                sempahore.Release();
        //            }
        //        });
        //    }
        //}
        //private async Task HandleRequest(HttpListenerContext context)
        //{
        //    //try
        //    //{
        //    //    if (context.Request.HttpMethod == "POST")
        //    //    {
        //    //        using var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding);
        //    //        string json = await reader.ReadToEndAsync();
        //    //        var jsonToMessage = JsonSerializer.Deserialize<DeploymentInformation>(json, _jsonSerializerOptions);
        //    //        Debug.WriteLine(json);
        //    //        _DeviceManager.AddDeviceFromMessageInfo(jsonToMessage);
        //    //    }
        //    //    else if (context.Request.HttpMethod == "PUT")
        //    //    {
        //    //        using var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding);
        //    //        string json = await reader.ReadToEndAsync();
        //    //        var basePath = context.Request.Headers["X-Base-Path"];
        //    //        var urlPath = context.Request.Url!.LocalPath;
        //    //        Debug.WriteLine($"{basePath}/{urlPath}");
        //    //        var fullPath = Path.Combine(
        //    //            basePath!,
        //    //            urlPath.TrimStart('/').Replace("/", "\\")
        //    //            );

        //    //        fullPath = Path.GetFullPath(fullPath);

        //    //        await File.WriteAllTextAsync(fullPath, json);
        //    //    }
        //    //    else
        //    //    {
        //    //        context.Response.StatusCode = 405;
        //    //    }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    context.Response.StatusCode = 500;
        //    //    throw new Exception($"ERROR", ex);
        //    //}
        //    //finally
        //    //{
        //    //    context.Response.Close();
        //    //}
        //}
        
        public async Task StopServer()
        {
            await _networkService.StopServerConnection();
            await _discoveryService.ShutDown();
        }
    }
}
