using TeamServer.Models.Agents;

namespace TeamServer.Interfaces.AgentTasks
{
    public interface IAgentTasktQueueService
    {
        public void AddToQueue(AgentTask task);
        public AgentTask DequeueAgentTask(string agentId);

    }
}
