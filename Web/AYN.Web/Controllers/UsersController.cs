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
        public IActionResult Profile(string id)
        {
            var viewModel = this.usersService.GetProfileDetails<GetUserProfileDetailsViewModel>(id);

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Follow(string id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (this.usersService.IsAlreadyFollower(userId, id))
            {
                this.TempData["Message"] = "Already following this user!";
                return this.Redirect($"/Users/Profile?id={id}");
            }

            if (userId == id)
            {
                this.TempData["Message"] = "You cannot follow yourself";
                return this.Redirect($"/Users/Profile?id={id}");
            }

            await this.usersService.Follow(userId, id);

            this.TempData["Message"] = "Successfully followed!";
            return this.Redirect($"/Users/Profile?id={id}");
        }
    }
}
