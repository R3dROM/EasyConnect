using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyConnect.Models
{
    public class Manifest
    {
        public string version {  get; set; }
        public string bundleID { get; set; }
        public List<Files> files {  get; set; }
    }

    public class Files
    {
        public string path { get; set; }
        public string sha256 { get; set; }
        public long size { get; set; }
    }
}
