using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using AYN.Services.Data.Interfaces;
using AYN.Web.ViewModels.Chat;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.ViewComponents;

[ViewComponent(Name = "ChatConversations")]
public class ChatConversationsViewComponent : ViewComponent
{
    private readonly IMessagesService messagesService;

    public ChatConversationsViewComponent(IMessagesService messagesService)
        => this.messagesService = messagesService;

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var currentUserId = this.UserClaimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var conversations = await this.messagesService.GetAllAsync<ChatConversationsViewModel>(currentUserId);

        var chatConversationsAsArray = conversations as ChatConversationsViewModel[] ?? conversations.ToArray();

        foreach (var user in chatConversationsAsArray)
        {
            user.LastMessage = await this.messagesService.GetLastMessageAsync(currentUserId, user.Id);
            user.LastMessageActivity = await this.messagesService.GetLastActivityAsync(currentUserId, user.Id);
            user.IsRead = this.messagesService.IsAllMessagesRead(user.Id, currentUserId);
        }

        var viewModel = new ListChatConversationsViewModel()
        {
            AllChats = chatConversationsAsArray,
        };

        return this.View(viewModel);
    }
}
