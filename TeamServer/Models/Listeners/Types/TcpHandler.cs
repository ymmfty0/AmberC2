using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TeamServer.Data;
using TeamServer.Hubs;
using TeamServer.Interfaces.AgentTasks;
using TeamServer.Models.Agents;

namespace TeamServer.Models.Listeners.Types
{
    public class TcpHandler : IHandler
    {

        private TcpListener tcpListener;

        private CancellationTokenSource _tokenSource = new CancellationTokenSource();

        private List<ClientInfo> connectedClients = new List<ClientInfo>();

        private readonly DataContext _context;

        private readonly IHubContext<AmberHub> _hubContext;
        private readonly IAgentTasktQueueService _agentTasktQueueService;

        public string Id { get; set; }

        public TcpHandler(DataContext context, IHubContext<AmberHub> hubContext , IAgentTasktQueueService agentTasktQueueService, string id)
        {
            _context = context;
            _hubContext = hubContext;
            _agentTasktQueueService = agentTasktQueueService;
            Id = id;
        }

        public async Task Start(string BindHost, int BindPort)
        {

            Thread listenerThread = new Thread(() => StartTcpListener(BindHost, BindPort)); ;
            listenerThread.IsBackground = true;
            listenerThread.Start();
        }

        public async Task StartTcpListener(string BindHost, int BindPort)
        {

            IPAddress address = IPAddress.TryParse("0.0.0.0", out address) ? address : IPAddress.Any;
            tcpListener = new TcpListener(address, BindPort);
           
            tcpListener.Start();

            Task.Run(async () => await AcceptClients());

        }

        private async Task AcceptClients()
        {
            while (!_tokenSource.IsCancellationRequested)
            {
                if (tcpListener.Pending())
                {
                    TcpClient client = await tcpListener.AcceptTcpClientAsync();
                    string message = ReceiveMessage(client);
                    Agent agentMetadata = await CheckAgentMetada(message);
                    if(agentMetadata is null)
                        continue;
                    connectedClients.Add(new ClientInfo { Id = agentMetadata.Id, Client = client });

                }
                foreach (var connectedClient in connectedClients)
                {
                    var task = _agentTasktQueueService.DequeueAgentTask(connectedClient.Id);
                    if(task == null)
                    {
                        continue;
                    }
                    SendMessage(connectedClients.Find(item => item.Id == connectedClient.Id).Client, EncodeTask(task));
                    string message = ReciveTaskResult(connectedClients.Find(item => item.Id == connectedClient.Id).Client);
                    string resultTask = await CheckAgentTaskResult(message);
                    if (resultTask == "")
                    {
                        continue;
                    }

                }

            }
        }

        private async Task<string> CheckAgentTaskResult(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                AgentTaskResult taskResult = ExtractTaskResult(message);
                if (taskResult is null)
                    return "";

                await _hubContext.Clients.All.SendAsync("AgentTaskResult", taskResult.Id);

                await _context.AgentTasksResult.AddAsync(taskResult);
                await _context.SaveChangesAsync();
            }
            return "";
        } 
        private async Task<Agent> CheckAgentMetada(string message)
        {
            Agent agentMetadata = ExtractMetadata(message);
            if (agentMetadata is null)
                return null;

            var agentById = await _context.Agents.FindAsync(agentMetadata.Id);
            if (agentById is null)
            {
                await _hubContext.Clients.All.SendAsync("NewAgent", agentMetadata.Id);

                _context.Agents.Add(agentMetadata);
                await _context.SaveChangesAsync();
            }

            return agentMetadata;

        }
        private string ReceiveMessage(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            StringBuilder messageBuilder = new StringBuilder();
            int bytesRead = 0;

            // Принять сообщение сразу после подключения клиента
            bytesRead = stream.Read(buffer, 0, buffer.Length);
            string initialMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            messageBuilder.Append(initialMessage);

            while (stream.DataAvailable)
            {
                bytesRead = stream.Read(buffer, 0, buffer.Length);
                string messagePart = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                messageBuilder.Append(messagePart);
            }

            string message = messageBuilder.ToString();

            // Сброс буфера сетевого потока
            stream.Flush();

            return message;
        }

        private string ReciveTaskResult(TcpClient client)
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

        private Agent ExtractMetadata(string messgae)
        {
            var json = Encoding.UTF8.GetString(Convert.FromBase64String(messgae));

            Agent agent = JsonConvert.DeserializeObject<Agent>(json);
            return agent;
        }
        private AgentTaskResult ExtractTaskResult(string messgae)
        {
            var json = Encoding.UTF8.GetString(Convert.FromBase64String(messgae));

            AgentTaskResult agentTaskResult = JsonConvert.DeserializeObject<AgentTaskResult>(json);
            return agentTaskResult;
        }

        private string EncodeTask(AgentTask task)
        {
            string json = JsonConvert.SerializeObject(task);

            byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
            string base64Encoded = Convert.ToBase64String(jsonBytes);

            return base64Encoded;
        }

        public void Stop()
        {
            _tokenSource.Cancel();
            tcpListener.Stop();
        }

    }
}

class ClientInfo
{
    public string Id { get; set; }

    public TcpClient Client { get; set; }
};