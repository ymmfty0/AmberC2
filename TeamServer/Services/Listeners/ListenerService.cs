using System.Xml.Linq;
using TeamServer.Models.Listeners;

namespace TeamServer.Services.Listeners
{

    public class ListenerService : IListenerService
    {
        private readonly List<IHandler> _listeners = new();

        public void AddListener(IHandler handler)
        {
            _listeners.Add(handler);
        }

        public IHandler GetListener(string id)
        {
            return GetListeners().FirstOrDefault(l => l.Id.Equals(id));
        }

        public IEnumerable<IHandler> GetListeners()
        {
            return _listeners;
        }

        public void RemoveListener(IHandler handler)
        {
            _listeners.Remove(handler);
        }
    }
    public interface IListenerService
    {
        void AddListener(IHandler handler);
        IEnumerable<IHandler> GetListeners();
        IHandler GetListener(string id);
        void RemoveListener(IHandler handler);
    }
}
