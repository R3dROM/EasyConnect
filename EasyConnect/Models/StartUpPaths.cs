using System;
using System.Collections.Generic;
using System.Text;

namespace EasyConnect.Models
{
    [Serializable]
    public class StartUpPaths
    {
        public string CaddyPath { get; set; } = string.Empty;
        public string DeployPath {  get; set; } = string.Empty;
        public string DeviceListPath {  get; set; } = string.Empty;
        public string ManifestPath {  get; set; } = string.Empty;

        public StartUpPaths(string caddyPath, string deployPath, string deviceListPath, string manifestPath)
        {
            CaddyPath = caddyPath;
            DeployPath = deployPath;
            DeviceListPath = deviceListPath;
            ManifestPath = manifestPath;
        }
    }
}
