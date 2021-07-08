using System.Threading.Tasks;

using AYN.Services.Data.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace AYN.Web.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatConversationsService chatConversationsService;

        public ChatHub(IChatConversationsService chatConversationsService)
        {
            this.chatConversationsService = chatConversationsService;
        }

        public async Task SendMessage(string senderId, string receiverId, string text)
        {

        }
    }
}
