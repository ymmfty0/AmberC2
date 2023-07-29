using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Implant.Models
{
    public class AgentTaskResult
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string AgentId { get; set; }
        public string Result { get; set; }

        public string TaskId { get; set; }
    }
}
