using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using AYN.Services.Data.Interfaces;
using AYN.Web.ViewModels.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.Controllers;

public class NotificationsController : BaseController
{
    private readonly INotificationsService notificationsService;

    public NotificationsController(
        INotificationsService notificationsService)
    {
        this.notificationsService = notificationsService;
    }

    [HttpGet]
    public async Task<IActionResult> All(int id = 1)
    {
        var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var notifications = await this.notificationsService
            .GetAll<GetAllNotificationsForUserViewModel>(userId);

        var viewModel = new ListAllNotificationsViewModel()
        {
            Notifications = notifications.Skip((id - 1) * 12).Take(12),
            Count = this.notificationsService.GetCount(userId),
            ItemsPerPage = 12,
            PageNumber = id,
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
    public async Task<IActionResult> MarkAsRead(string id)
    {
        await this.notificationsService.MarkAsRead(id);

        return this.Redirect("/Notifications/All");
    }

    [HttpPost]
    public async Task<IActionResult> MarkAllAsRead()
    {
        var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        await this.notificationsService.MarkAllAsRead(userId);

        return this.Redirect("/Notifications/All");
    }
}
