using Implant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Implant.Commands
{
    public abstract class Command
    {
        public abstract string Name { get; }

        public abstract AgentTaskResult Execute(string taskId , string agentId , string arguments);
    }
}
