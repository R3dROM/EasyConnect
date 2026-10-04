using EasyConnect.Events;
using EasyConnect.State;
using System.Net;
using System.Reactive.Subjects;

namespace EasyConnect.Managers
{
    public class NetworkManager
    {
        private readonly NetworkState _networkState = new();

        private readonly Subject<NetworkChange> _networkChange = new();
        public IObservable<NetworkChange> NetworkChange
        => _networkChange;

        public void SetExperienceManagerIp(string ip)
            => _networkState.ExperienceServerIp = ip;
        public string GetExperienceManagerIp()
            => _networkState.ExperienceServerIp;

        public void SetDeployPath(string path)
            => _networkState.DeployPath = path;
        public string GetDeployPath()
            => _networkState.DeployPath;

        public void SetBundle(string bundle)
        {
            if (_networkState.Bundle == bundle) return;
            _networkState.Bundle = bundle;
            _networkChange.OnNext(new NetworkChange(NetworkChangeType.Bundle, GetBundle()));
        }
        public string GetBundle()
            => _networkState.Bundle;

        public void SetFolderBundle(string folderBundle)
        {
            string path = Path.GetFileName(folderBundle.TrimEnd(Path.DirectorySeparatorChar));
            _networkState.FolderBundle = path;
        }
        public string GetFolderBundle()
            => _networkState.FolderBundle;

        public void SetScriptsPath(string path)
            => _networkState.ScriptsPath = path;
        public string GetScriptsPath()
            => _networkState.ScriptsPath;
        public void SetManifestScript(string manifestScript)
            => _networkState.ManifestScriptPath = manifestScript;
        public string GetManifestScript()
            => _networkState.ManifestScriptPath;
        public void SetBundleScript(string bundleScript)
            => _networkState.BundleScriptPath = bundleScript;
        public string GetBundleScript()
            => _networkState.BundleScriptPath;

        public void SetCaddy(string path)
            => _networkState.CaddyPath = path;
        public string GetCaddy()
            => _networkState.CaddyPath;
        public string GetCaddyExe()
            => _networkState.CaddyExe;
        public string GetCaddyFile()
            => _networkState.CaddyFile;

        public void SetMyIpAddress(IPAddress ipAddress)
        {
            if (_networkState.MyIpAddress == ipAddress) return;
            _networkState.MyIpAddress = ipAddress;
            _networkChange.OnNext(new NetworkChange(NetworkChangeType.ServerIp, GetMyIpAddress()));
        }
        public string GetMyIpAddress()
            => _networkState.MyIpAddress?.ToString() ?? "NO";

        public void SetWebSocketPort(string port)
            => _networkState.WebSocketPort = port;
        public string GetWebSocketPort()
            => _networkState.WebSocketPort;

        public void SetServerPort(string port)
            => _networkState.ServerPort = port;
        public string GetServerPort()
            => _networkState.ServerPort;
    }
}
