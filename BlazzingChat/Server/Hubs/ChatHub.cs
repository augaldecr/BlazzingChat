using BlazzingChat.Shared.Models;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace BlazzingChat.Server.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(Message message)
        {
            //await Clients.All.SendAsync("ReceiveMessage", message);
            var users = new string[] { message.ToUserId, message.FromUserId };
            await Clients.Users(users).SendAsync("ReceiveMessage", message);
        }
    }
}
