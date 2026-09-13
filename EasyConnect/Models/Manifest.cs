namespace EasyConnect.Models
{
    [Serializable]
    public class Manifest
    {
        public required string Version {  get; set; }
        public required string Bundle { get; set; }
        public required List<Files> Files { get; set; }
        public required List<Configs> NetConfigs { get; set; }
    }

    public class Files
    {
        public required string Path { get; set; }
        public required string Sha256 { get; set; }
        public required long Size { get; set; }
    }
    public class Configs
    {
        public required string Path { get; set; }
        public required string DeviceId { get; set; }
        public required string SerialNumber { get; set; }
        public required string Sha256 { get; set; }
        public required long Size { get; set; }
    }
}
