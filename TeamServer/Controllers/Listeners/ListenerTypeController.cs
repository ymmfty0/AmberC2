using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamServer.Data;
using TeamServer.Models.Listeners;

namespace TeamServer.Controllers.Listeners
{
    [Route("/api/listeners/type")]
    [ApiController]
    public class ListenerTypeController : ControllerBase
    {
        private readonly DataContext _context;

        public ListenerTypeController(DataContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<ActionResult<List<ListenerType>>> AddListenerType(ListenerType listenerType)
        {

            var listener = new ListenerType
            {
                Name = listenerType.Name.ToUpper()
            };

            _context.ListenerTypes.Add(listener);
            await _context.SaveChangesAsync();


            return Ok(listenerType);
        }

        [HttpGet]
        public async Task<ActionResult<List<ListenerType>>> GetAllListenerTypes()
        {
            return Ok(await _context.ListenerTypes.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<ListenerType>>> GetListenerTypeById(string id)
        {
            var listenerType = await _context.ListenerTypes.FindAsync(id);
            if (listenerType is null) return NotFound();

            return Ok(listenerType);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<ListenerType>>> RemoveListenerTypeById(string id)
        {
            var listenerType = await _context.ListenerTypes.FindAsync(id);
            if (listenerType is null) return NotFound();

            _context.ListenerTypes.Remove(listenerType);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
