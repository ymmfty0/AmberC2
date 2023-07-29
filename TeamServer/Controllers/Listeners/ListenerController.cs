using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using TeamServer.Data;
using TeamServer.Hubs;
using TeamServer.Models.Listeners;
using TeamServer.Models.Listeners.Types;
using TeamServer.Services.Listeners;

namespace TeamServer.Controllers.Listeners
{

    [Route("/api/listeners/")]
    [ApiController]
    public class ListenerController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IListenerService _listenerService;
        private readonly IHubContext<AmberHub> _hubContext;
        private readonly ListenerFormatService _listenerFormatService;
        public ListenerController(DataContext context, IListenerService listenerService, IHubContext<AmberHub> hubContext, ListenerFormatService listenerFormatService)
        {
            _context = context;
            _listenerService = listenerService;
            _hubContext = hubContext;
            _listenerFormatService = listenerFormatService;
        }
        [HttpPost]
        public async Task<ActionResult<List<Listener>>> AddListener(Listener listener)
        {

            var listenerType = await _context.ListenerTypes.FindAsync(listener.ListenerTypeId);

            if (listenerType is null)
                return BadRequest();

            var hanlder = _listenerFormatService.GetListenerType(listenerType.Name, listener.Id);

            _listenerService.AddListener(hanlder);
            hanlder.Start(listener.BindHost, listener.BindPort);

            _context.Listeners.Add(listener);
            await _context.SaveChangesAsync();


            return Ok(listener);
        }

        [HttpGet]
        public async Task<ActionResult<List<Listener>>> GetAllListener()
        {
            return Ok(await _context.Listeners.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Listener>>> GetListenerById(string id)
        {
            var listener = await _context.Listeners.FindAsync(id);
            if (listener is null) return NotFound();

            return Ok(listener);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Listener>>> RemoveListenerById(string id)
        {
            var listener = await _context.Listeners.FindAsync(id);
            if (listener is null) return NotFound();

            var listenerService = _listenerService.GetListener(id);
            if (listenerService is null) return NotFound();

            listenerService.Stop();
            
            _listenerService.RemoveListener(listenerService);
            _context.Listeners.Remove(listener);

            await _context.SaveChangesAsync();
            return Ok("Deleted");
        }
    }
}
