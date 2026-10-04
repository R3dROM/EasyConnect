namespace EasyConnect.Models
{
    [Serializable]
    public class StartUpPaths(string caddyPath, string scriptsPath)
    {
        public string CaddyPath { get; set; } = caddyPath;
        public string ScriptsPath { get; set; } = scriptsPath;
    }
}
