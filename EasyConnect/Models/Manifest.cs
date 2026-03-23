using System.Collections.Generic;

namespace EasyConnect.Models
{
    public class Manifest
    {
        public required string version {  get; set; }
        public required string bundle { get; set; }
        public required List<Files> files {  get; set; }
    }

    public class Files
    {
        public required string path { get; set; }
        public required string sha256 { get; set; }
        public required long size { get; set; }
    }
}
