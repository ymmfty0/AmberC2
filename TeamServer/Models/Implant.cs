namespace TeamServer.Models
{
    public class Implant
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ListenerId { get; set; }
        public byte[] FileData { get; set; }
        public string FileName { get; set; }

    }
}
