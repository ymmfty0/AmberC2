using Microsoft.EntityFrameworkCore;
using TeamServer.Data;
using TeamServer.Models.Listeners;

namespace TeamServer.Services.Listeners
{
    public class StartupListenersService : IHostedService
    {
       
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly DataContext _context;
        private readonly IListenerService _listenerService;
        private readonly ListenerFormatService _listenerFormatService;
        public StartupListenersService(IServiceScopeFactory serviceScopeFactory, ListenerFormatService listenerFormatService)
        {
            _serviceScopeFactory = serviceScopeFactory;
            var scope = _serviceScopeFactory.CreateScope(); 
            _listenerService = scope.ServiceProvider.GetRequiredService<IListenerService>(); 
            _context = scope.ServiceProvider.GetRequiredService<DataContext>();
            _listenerFormatService = listenerFormatService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            List<Listener> listeners = await _context.Listeners.ToListAsync();
            if (listeners is null) return;

            foreach (var listener in listeners)
            {
                var listenerType = await _context.ListenerTypes.FindAsync(listener.ListenerTypeId);
                var hanlder = _listenerFormatService.GetListenerType(listenerType.Name, listener.Id);

                _listenerService.AddListener(hanlder);
                hanlder.Start(listener.BindHost, listener.BindPort);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
