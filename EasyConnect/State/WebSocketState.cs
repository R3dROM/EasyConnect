using System;
using System.Collections.Generic;
using System.Text;

namespace EasyConnect.State
{
    public class WebSocketState
    {
        private string _serverIp = "";
        public string ServerIp
        {
            get => _serverIp;
            set
            {
                if (_serverIp != value)
                    _serverIp = value;
            }
        }
        private string _webSocketPort = "";
        public string WebSocketPort
        {
            get => _webSocketPort;
            set
            {
                if (value != _webSocketPort)
                    _webSocketPort = value;
            }
        }
    }
}
