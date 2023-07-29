using Client.Data.Agents;
using Client.Services.Api;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

namespace Client.Services
{
	public class AgentService
	{

		public async Task<List<Agent>> GetAgents(string apiHost)
		{
			using (var httpClient = new HttpClient())
			{
				return await httpClient.GetFromJsonAsync<List<Agent>>($"http://{apiHost}{ApiList.GET_AGENTS}");
			}
		}

        public async Task<List<AgentTask>> GetAgentsTasks(string apiHost)
        {
            using (var httpClient = new HttpClient())
            {
                return await httpClient.GetFromJsonAsync<List<AgentTask>>($"http://{apiHost}{ApiList.GET_AGENT_TASKS}");
            }
        }

        public async Task SendAgentTask(string apiHost, AgentTask agentTask)
        {
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.PostAsync($"http://{apiHost}{ApiList.GET_AGENT_TASKS}", new StringContent(JsonConvert.SerializeObject(agentTask), Encoding.UTF8, "application/json"));
            }
        }

        public async Task<AgentTaskResult> GetAgentTaskResult(string apiHost, string id)
        {
            using (var httpClient = new HttpClient())
            {
                return await httpClient.GetFromJsonAsync<AgentTaskResult>($"http://{apiHost}{ApiList.GET_AGENT_TASK_RESULT_BY_ID}{id}");
            }
        }
        public async Task<List<AgentTaskResult>> GetAgentTaskResults(string apiHost)
        {
            using (var httpClient = new HttpClient())
            {
                return await httpClient.GetFromJsonAsync<List<AgentTaskResult>>($"http://{apiHost}{ApiList.GET_AGENT_TASK_RESULT_BY_ID}");
            }
        }



        public async Task<Agent> GetAgentsById(string apiHost, string id)
		{
			using (var httpClient = new HttpClient())
			{
                return await httpClient.GetFromJsonAsync<Agent>($"http://{apiHost}{ApiList.GET_AGENT_BY_ID}{id}");
            }
        }

		public async Task<bool> DeleteDataAsync(string id)
		{
			using (var httpClient = new HttpClient())
			{
				var response = await httpClient.DeleteAsync($"http://localhost:7252/api/agents{id}");
				return response.IsSuccessStatusCode;

			}
		}

	}
}
