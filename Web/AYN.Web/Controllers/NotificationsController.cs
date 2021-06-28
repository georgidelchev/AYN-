using System.Security.Claims;
using System.Threading.Tasks;
using AYN.Services.Data;
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

            var viewModel = 1;

            return this.View(viewModel);
        }

        [HttpGet]
        public IActionResult Count(string userId)
        {
            var data = this.notificationsService.GetCount(userId);

            return this.Json(data);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> MarkAsRead(string notificationId)
        {
            return this.Redirect($"/Notifications/All");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> MarkAllAsRead(string userId)
        {
            return this.Redirect($"/Notifications/All");
        }
    }
}
