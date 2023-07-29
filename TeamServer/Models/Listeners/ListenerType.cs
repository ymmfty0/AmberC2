namespace TeamServer.Models.Listeners
{
    public class ListenerType
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }
    }
}
