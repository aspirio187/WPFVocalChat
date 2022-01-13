using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VocalChat.Shared.Models;

namespace VocalChat.WPF.Services
{
    public class SignalRChatService
    {
        private readonly HubConnection _connection;

        public event Action<UserHubModel>? NewUserArrived;
        public event Action<string> Connected;

        public SignalRChatService(HubConnection connection)
        {
            _connection = connection;

            _connection.On<string>("Connected", (id) => Connected?.Invoke(id));

            _connection.On<UserHubModel>("NewConnection", (user) =>
            {
                Debug.WriteLine(user.Username);
                NewUserArrived?.Invoke(user);
            });
        }

        public async Task Connect()
        {
            await _connection.StartAsync();
        }

        public async Task DeclareArrival(UserHubModel user)
        {
            await _connection.SendAsync("DeclareArrival", user);
        }
    }
}
