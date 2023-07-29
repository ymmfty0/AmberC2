using TeamServer.Interfaces.AgentTasks;
using TeamServer.Models.Agents;

namespace TeamServer.Services.AgentTasks
{
    public class AgentTasktQueueService : IAgentTasktQueueService
    {
        private Queue<AgentTask> queue;

        public AgentTasktQueueService()
        {
            queue = new Queue<AgentTask>();
        }
        public void AddToQueue(AgentTask task)
        {
            queue.Enqueue(task);
        }

        public AgentTask DequeueAgentTask(string agentId)
        {
            AgentTask foundTask = null;
            foreach(AgentTask task in queue)
            {
                if (task.AgentId == agentId && !task.Status)
                {
                    foundTask = task; break;
                }
            }

            if(foundTask != null)
            {
                queue.Dequeue();
            }

            return foundTask;
        }

    }
}
