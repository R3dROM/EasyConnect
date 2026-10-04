using EasyConnect.Events;
using EasyConnect.Services;
using System.ComponentModel;

namespace EasyConnect.Presentation
{
    public class NetworkPresentation: INotifyPropertyChanged
    {
        public NetworkPresentation(NetworkService _networkService)
        {
            _networkService.SubscribeConsumer(OnNetworkChange);
        }
        private void OnNetworkChange(NetworkChange networkChange)
        {
            switch (networkChange.type)
            {
                case NetworkChangeType.ServerIp:
                    ServerIp = networkChange.toChange;
                    break;
                case NetworkChangeType.Bundle:
                    Bundle = networkChange.toChange;
                    break;
                default:
                    break;
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private string _serverIp = "";
        public string ServerIp
        {
            get => _serverIp;
            set
            {
                if (_serverIp != value)
                {
                    _serverIp = value;
                    OnPropertyChanged(nameof(ServerIp));
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
                    OnPropertyChanged(nameof(Bundle));
                }
            }
        }
    }
}
