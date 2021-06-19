using System.Security.Claims;
using System.Threading.Tasks;

using AYN.Services.Data;
using AYN.Web.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var viewModel = await this.usersService.GetProfileDetails<GetUserProfileDetailsViewModel>(userId);

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Follow(string id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await this.usersService.Follow(userId, id);

            var viewModel = await this.usersService.GetProfileDetails<GetUserProfileDetailsViewModel>(userId);

            return this.RedirectToAction(nameof(this.Profile), viewModel);
        }
    }
}
