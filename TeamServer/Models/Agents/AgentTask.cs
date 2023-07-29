namespace TeamServer.Models.Agents
{
    public class AgentTask
    {

        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        public string AgentId { get; set; }

        public string Command { get; set; }
        public string Arguments { get; set; }

        public bool Status { get; set; } = false;


    }
}
