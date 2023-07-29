using Implant.Models;
using System.Threading.Tasks;

namespace Implant.CommunicationModule
{
    public abstract class CommunactionModule
    {

        public abstract Task SendRequest(AgentMetadata agentMetadata);

        public abstract string Type { get; }

        public abstract string BindHost { get;  }
        public abstract string BindPort { get;  }
    }
}
