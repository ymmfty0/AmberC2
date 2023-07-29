using Client.Data.Agents;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

namespace Client.Hubs.Agents
{
    public class AmberHub
    {

        public Func<string, Task> NewAgent { get; set; }


        private HubConnection _connection;

        public async Task Connect(string server)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl($"http://{server}/AmberHub")
                .WithAutomaticReconnect()
                .Build();

            await _connection.StartAsync();

            _connection.On<string>("NewAgent", OnNewAgent);
        }

        private void OnNewAgent(string agent) => NewAgent?.Invoke(agent);
    }
}