namespace EasyConnect.Models
{
    [Serializable]
    public class StartUpPaths(string caddyPath, string manifestPath)
    {
        public string CaddyPath { get; set; } = caddyPath;
        //public string DeployPath { get; set; } = deployPath;
        public string ManifestPath { get; set; } = manifestPath;
    }
}
