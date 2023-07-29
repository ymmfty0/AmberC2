namespace TeamServer.Models.Agents
{
    public class AgentTaskResult
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Result { get; set; }
        public string AgentId { get; set;  }

        public string TaskId { get; set; }
    }
}
