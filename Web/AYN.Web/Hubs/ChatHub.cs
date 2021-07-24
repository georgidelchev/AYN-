using System;
using System.Security.Claims;
using System.Threading.Tasks;

using AYN.Services.Data.Interfaces;
using AYN.Web.ViewModels.Chat;
using Microsoft.AspNetCore.SignalR;

namespace AYN.Web.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IUsersService usersService;
        private readonly IMessagesService messagesService;

        public ChatHub(
            IUsersService usersService,
            IMessagesService messagesService)
        {
            this.usersService = usersService;
            this.messagesService = messagesService;
        }

        public async Task SendMessage(string message, string receiverId)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            var authorId = this.Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await this.usersService.GetByIdAsync<ChatUserViewModel>(authorId);

            await this.messagesService.CreateAsync(message, authorId, receiverId);

            await this.Clients.All.SendAsync(
                "ReceiveMessage",
                new ChatMessagesWithUserViewModel
                {
                    SenderId = authorId,
                    SenderUserName = user.UserName,
                    SenderAvatarImageUrl = user.AvatarImageUrl,
                    Content = message,
                    CreatedOn = DateTime.Now,
                });
        }
    }
}
