using System;
using System.Collections.Generic;
using System.Text;

namespace EasyConnect.Models.Communication.Message
{
    public enum MessageType
    {
        Register,
        Deployment,
        Battery,
        Heartbeat,
        Acknowledge,
        Update
    }
}
