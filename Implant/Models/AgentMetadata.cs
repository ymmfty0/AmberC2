using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Implant.Models
{
    public class AgentMetadata
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Hostname { get; set; }
        public string Username { get; set; }
        public string ProcessName { get; set; }
        public int ProcessId { get; set; }
        public string Integrity { get; set; }
        public string Architecture { get; set; }
    }
}
