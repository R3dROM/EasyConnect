using EasyConnect.Models;
using System.Net;
using System.Reactive.Subjects;

namespace EasyConnect.State
{
    public class NetworkState
    {
        private readonly Subject<NetworkChange> _networkChange = new();
        public IObservable<NetworkChange> NetworkChange
        => _networkChange;

        private Manifest? _manifest = null;
        public Manifest? Manifest
        {
            get => _manifest;
            set
            {
                if (_manifest != value)
                {
                    _manifest = value;
                }
            }
        }
        private string _manifestScriptsPath = string.Empty;
        public string ManifestScriptsPath
        {
            get => _manifestScriptsPath;
            set
            {
                if (_manifestScriptsPath != value)
                {
                    _manifestScriptsPath = value;
                }
            }
        }
        private string _deployPath = string.Empty;
        public string DeployPath
        {
            get => _deployPath;
            set
            {
                if (_deployPath != value)
                {
                    _deployPath = value;
                }
            }
        }
        private string _caddyExe = string.Empty;
        public string CaddyExe
        {
            get => _caddyExe;
        }
        private string _caddyFile = string.Empty;
        public string CaddyFile
        {
            get => _caddyFile;
        }
        private string _caddyPath = string.Empty;
        public string CaddyPath
        {
            get => _caddyPath;
            set
            {
                if (_caddyPath != value)
                {
                    _caddyPath = value;
                    _caddyExe = Path.Combine(_caddyPath, "caddy.exe");
                    _caddyFile = Path.Combine(_caddyPath, "CaddyFile");
                }
            }
        }

        private IPAddress? _myIpAddress;
        public IPAddress? MyIpAddress
        {
            get => _myIpAddress;
            set
            {
                if (_myIpAddress != value)
                {
                    _myIpAddress = value;
                    _networkChange.OnNext(new NetworkChange(NetworkChangeType.ServerIp, MyIpAddress?.ToString() ?? ""));
                }
            }
        }
        private string _serverPort = "8000";
        public string ServerPort
        {
            get => _serverPort;
            set
            {
                if (_serverPort != value)
                {
                    _serverPort = value;
                }
            }
        }
        private string _webSocketPort = "8181";
        public string WebSocketPort
        {
            get => _webSocketPort;
            set
            {
                if (_webSocketPort != value)
                {
                    _webSocketPort = value;
                }
            }
        }
        private string _folderBundle = "";
        public string FolderBundle
        {
            get => _folderBundle;
            set
            {
                if (value != _folderBundle)
                {
                    _folderBundle = value;
                }
            }
        }
        private string _bundle = "";
        public string Bundle
        {
            get => _bundle;
            set
            {
                if (value != _bundle)
                {
                    _bundle = value;
                    _networkChange.OnNext(new NetworkChange(NetworkChangeType.Bundle, Bundle));
                }
            }
        }
        private string _apkName = string.Empty;
        public string ApkName
        {
            get => _apkName;
            set
            {
                if (value != _apkName)
                {
                    var tmp = value;
                    tmp = tmp.Substring(4);
                    _apkName = tmp;
                }
            }
        }
    }
}
