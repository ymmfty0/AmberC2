using Client.Data.Agents;
using Client.Data.Implants;
using Client.Services.Api;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;

namespace Client.Services
{
	public class ApiService
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

        public async Task<List<Implant>> GetGeneratedImplants(string apiHost)
        {
            using (var httpClient = new HttpClient())
            {
                return await httpClient.GetFromJsonAsync<List<Implant>>($"http://{apiHost}{ApiList.GET_IMPLANTS}");
            }
        }

        public async Task<List<string>> GetImplantCommands(string apiHost)
        {
            using (var httpClient = new HttpClient())
            {
                return await httpClient.GetFromJsonAsync<List<string>>($"http://{apiHost}{ApiList.GET_IMPLANTS}/commands");
            }
        }

        public async Task<byte[]> DonwloadImplant(string apiHost, string implantId)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"http://{apiHost}{ApiList.GET_IMPLANTS}/{implantId}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsByteArrayAsync();
                }
                else
                {
                    throw new Exception($"Failed to download file. Status code: {response.StatusCode}");
                }
            }
        }

        public async Task GenerateImplant(string apiHost, string listenerId , List<string> commandsToRemove)
        {
            using (var httpClient = new HttpClient())
            {
                await httpClient.PostAsync($"http://{apiHost}{ApiList.GET_IMPLANTS}/generate/{listenerId}", new StringContent(JsonConvert.SerializeObject(commandsToRemove), Encoding.UTF8, "application/json"));
            }
        }

    }
}
