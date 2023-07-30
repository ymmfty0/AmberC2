namespace Client.Data.Implants
{
    public class Implant
    {
        public string Id { get; set; } 
        public string ListenerId { get; set; }
        public byte[] FileData { get; set; }
        public string FileName { get; set; }

    }
}
