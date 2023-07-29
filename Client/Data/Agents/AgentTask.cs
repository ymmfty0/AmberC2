namespace Client.Data.Agents
{
    public class AgentTask
    {
        public string Id { get; set; } 

        public string AgentId { get; set; }

        public string Command { get; set; }
        public string Arguments { get; set; }

        public bool Status { get; set; }
    }
}
