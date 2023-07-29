using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamServer.Data;
using TeamServer.Models.Agents;

namespace TeamServer.Controllers.Agents
{
    [Route("/api/agent/task/result/")]
    [ApiController]
    public class AgentTaskResultController : ControllerBase
    {
        private readonly DataContext _context;
        public AgentTaskResultController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<AgentTaskResult>>> GetResults()
        {
            return Ok(await _context.AgentTasksResult.ToArrayAsync());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<List<AgentTaskResult>>> GetResults(string id)
        {
            return Ok(await _context.AgentTasksResult.FindAsync(id));
        }

    }
}
