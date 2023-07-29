using Microsoft.AspNetCore.SignalR;

namespace TeamServer.Hubs
{
    public class AmberHub : Hub
    {
        public async Task SendNewAgent(string newAgent)
        {
            await Clients.All.SendAsync("NewAgent", newAgent);
        }
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine("Client connected");
        }
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
    }


}
