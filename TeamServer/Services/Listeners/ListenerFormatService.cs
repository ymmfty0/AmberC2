using Microsoft.AspNetCore.SignalR;
using TeamServer.Data;
using TeamServer.Hubs;
using TeamServer.Interfaces.AgentTasks;
using TeamServer.Models.Listeners;
using TeamServer.Models.Listeners.Types;

namespace TeamServer.Services.Listeners
{
    public class ListenerFormatService
    {

        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly DataContext _context;
        private readonly IHubContext<AmberHub> _hubContext;
        private readonly IAgentTasktQueueService _agentTasktQueueService;
        public ListenerFormatService(IServiceScopeFactory serviceScopeFactory) 
        {
            _serviceScopeFactory = serviceScopeFactory;
            var scope = _serviceScopeFactory.CreateScope();

            _context = scope.ServiceProvider.GetRequiredService<DataContext>();
            _hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<AmberHub>>();
            _agentTasktQueueService = scope.ServiceProvider.GetRequiredService<IAgentTasktQueueService>();
        }
        public IHandler GetListenerType( string name ,string id )
        {
            return name switch
            {
                "HTTP" => new HttpHandler(id),
                "TCP" => new TcpHandler(_context, _hubContext,_agentTasktQueueService, id),
                _ => throw new ArgumentOutOfRangeException(name, name, null)
            } ;
                
        }
    }
}
