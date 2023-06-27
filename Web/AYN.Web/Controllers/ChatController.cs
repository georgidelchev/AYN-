using System.Threading.Tasks;

using AYN.Services.Data.Interfaces;
using AYN.Web.Infrastructure.Extensions;
using AYN.Web.ViewModels.Chat;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.Controllers;

public class ChatController : BaseController
{
    private readonly IUsersService usersService;
    private readonly IMessagesService messagesService;
    private readonly IEmojisService emojisService;

    public ChatController(
        IUsersService usersService,
        IMessagesService messagesService,
        IEmojisService emojisService)
    {
        this.usersService = usersService;
        this.messagesService = messagesService;
        this.emojisService = emojisService;
    }

    [HttpGet]
    public IActionResult All()
        => this.View();

    [HttpGet]
    public IActionResult NewMessage()
        => this.View();

    [HttpPost]
    public async Task<IActionResult> NewMessage(ChatSendMessageInputModel input)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(input);
        }

        var receiverId = this.usersService.GetIdByUsername(input.Receiver);

        if (receiverId == null)
        {
            this.ModelState.AddModelError(string.Empty, "This user is not existing.");
            return this.View(input);
        }

        await this.messagesService.CreateAsync(input.Message, this.User.GetId(), receiverId);
        return this.RedirectToAction(nameof(this.With), new { id = receiverId });
    }

    [HttpGet]
    public async Task<IActionResult> With(string id)
        => this.View(new ChatWithUserViewModel
        {
            User = await this.usersService.GetByIdAsync<ChatUserViewModel>(id),
            Messages = await this.messagesService.GetAllWithUserAsync<ChatMessagesWithUserViewModel>(this.User.GetId(), id),
            Emojis = await this.emojisService.GetAll(),
        });

    [HttpGet]
    public IActionResult UnreadMessagesCount(string userId)
        => this.Json(this.messagesService.GetUnreadMessagesCount(userId));
}
