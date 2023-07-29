namespace TeamServer.Models.Listeners
{
    public class Listener
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string BindHost { get; set; } = "0.0.0.0";
        public int BindPort { get; set; }

        public string ListenerTypeId { get; set;  }
    }

}
