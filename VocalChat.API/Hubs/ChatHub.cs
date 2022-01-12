using Microsoft.AspNetCore.SignalR;
using VocalChat.Shared.Models;

namespace VocalChat.API.Hubs
{
    public class ChatHub : Hub
    {
        public async Task DeclareArrival(UserHubModel user)
        {
            await Clients.All.SendAsync("NewConnection", user);
        }

        public override Task OnConnectedAsync()
        {
            Clients.Caller.SendAsync("Connected", Context.ConnectionId);
            return base.OnConnectedAsync();
        }
    }
}
