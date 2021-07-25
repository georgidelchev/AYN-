using System.Security.Claims;
using System.Threading.Tasks;

using AYN.Services.Data.Interfaces;
using AYN.Web.ViewModels.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.Controllers
{
    [Authorize]
    public class ChatController : Controller
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
        {
            return this.View();
        }

        [HttpGet]
        public IActionResult NewMessage()
        {
            return this.View();
        }

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

            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await this.messagesService.CreateAsync(input.Message, userId, receiverId);

            return this.RedirectToAction(nameof(this.With), new { id = receiverId });
        }

        [HttpGet]
        public async Task<IActionResult> With(string id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var viewModel = new ChatWithUserViewModel
            {
                User = await this.usersService.GetByIdAsync<ChatUserViewModel>(id),
                Messages = await this.messagesService.GetAllWithUserAsync<ChatMessagesWithUserViewModel>(userId, id),
                Emojis = await this.emojisService.GetAll(),
            };

            return this.View(viewModel);
        }

        [HttpGet]
        public IActionResult UnreadMessagesCount(string userId)
        {
            var data = this.messagesService.GetUnreadMessagesCount(userId);
            return this.Json(data);
        }
    }
}
