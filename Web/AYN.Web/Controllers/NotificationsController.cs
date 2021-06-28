using System.Security.Claims;
using System.Threading.Tasks;
using AYN.Services.Data;
using AYN.Web.ViewModels.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly INotificationsService notificationsService;

        public NotificationsController(
            INotificationsService notificationsService)
        {
            this.notificationsService = notificationsService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> All()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var viewModel = new ListAllNotificationsViewModel()
            {
                Notifications = await this.notificationsService.GetAll<GetAllNotificationsForUserViewModel>(userId),
            };

            return this.View(viewModel);
        }

        [HttpGet]
        public IActionResult Count(string userId)
        {
            var data = this.notificationsService.GetCount(userId);

            return this.Json(data);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> MarkAsRead(string id)
        {
            await this.notificationsService.MarkAsRead(id);

            return this.Redirect("/Notifications/All");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await this.notificationsService.MarkAllAsRead(userId);

            return this.Redirect("/Notifications/All");
        }
    }
}
