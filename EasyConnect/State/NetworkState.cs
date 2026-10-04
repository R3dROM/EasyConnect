using System.Net;

namespace EasyConnect.State
{
    public class NetworkState
    {
        private string _scriptsPaths = string.Empty;
        public string ScriptsPath
        {
            get => _scriptsPaths;
            set
            {
                _scriptsPaths = value;
                _manifestScriptPath = Path.Combine(_scriptsPaths, "generate-manifest.ps1");
                _bundleScriptPath = Path.Combine(_scriptsPaths, "generate-bundleId.ps1");
            }
        }
        private string _bundleScriptPath = string.Empty;
        public string BundleScriptPath
        {
            get => _bundleScriptPath;
            set
            {
                if (_bundleScriptPath != value)
                {
                    _bundleScriptPath = value;
                }
            }
        }
        private string _manifestScriptPath = string.Empty;
        public string ManifestScriptPath
        {
            get => _manifestScriptPath;
            set
            {
                if (_manifestScriptPath != value)
                {
                    _manifestScriptPath = value;
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
                }
            }
        }
        private string _experienceServerIp = string.Empty;
        public string ExperienceServerIp
        {
            get => _experienceServerIp;
            set
            {
                if (_experienceServerIp != value)
                {
                    _experienceServerIp = value;
                }
            }
        }
    }
}
