using System;
using System.Runtime.Serialization;

namespace Implant.Models
{
    public class AgentTask
    {
        [DataMember(Name = "id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [DataMember(Name = "agentId")]
        public string AgentId { get; set; }

        [DataMember(Name = "command")]
        public string Command { get; set; }

        [DataMember(Name = "arguments")]
        public string Arguments { get; set; }

        public bool Status { get; set; } = false;

    }
}
