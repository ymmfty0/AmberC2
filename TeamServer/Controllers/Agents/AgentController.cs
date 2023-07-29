using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamServer.Data;
using TeamServer.Models.Agents;
using TeamServer.Models.Listeners;

namespace TeamServer.Controllers.Agents
{

    [Route("/api/agents")]
    [ApiController]
    public class AgentController : ControllerBase
    {
        private readonly DataContext _context;
        public AgentController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Agent>>> GetAgents()
        {
            return Ok(await _context.Agents.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAgent(string id)
        {

            var agent = await _context.Agents.FindAsync(id);
            if (agent is null) return NotFound();

            _context.Agents.Remove(agent);
            await _context.SaveChangesAsync();
            return Ok("Deleted");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Agent>> GetAgent(string id)
        {
            var agent = await _context.Agents.FindAsync(id);
            if (agent is null) return NotFound();

            return Ok(agent);
        }


    }
}
