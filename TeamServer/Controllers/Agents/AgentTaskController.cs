using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamServer.Data;
using TeamServer.Interfaces.AgentTasks;
using TeamServer.Models.Agents;

namespace TeamServer.Controllers.Agents
{
    [Route("/api/agents/task")]
    [ApiController]
    public class AgentTaskController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IAgentTasktQueueService _agentTasktQueueService;
        public AgentTaskController(DataContext context, IAgentTasktQueueService agentTasktQueueService)
        {
            _context = context;
            _agentTasktQueueService = agentTasktQueueService;
        }

        [HttpPost]
        public async Task<ActionResult<AgentTask>> CreateAgentTask(AgentTask agentTask)
        {
            _context.AgentTasks.Add(agentTask);
            await _context.SaveChangesAsync();
            _agentTasktQueueService.AddToQueue(agentTask);
            return agentTask;
        }

        [HttpGet]
        public async Task<ActionResult<List<AgentTask>>> GetAgetTasks()
        {
            return Ok(await _context.AgentTasks.ToArrayAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<AgentTask>>> GetAgetTasks(string id)
        {
            return Ok(await _context.AgentTasks.ToArrayAsync());
        }
    }
}
