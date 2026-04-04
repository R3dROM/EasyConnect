using System;
using System.Collections.Generic;
using System.Text;

namespace EasyConnect.Models
{
    [Serializable]
    public class NetConfig()
    {
        public required string version { get; set; }
        public required List<Configs> configs { get; set; }
    }
}
