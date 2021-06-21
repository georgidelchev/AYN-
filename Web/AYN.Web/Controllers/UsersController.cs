using System.Security.Claims;
using System.Threading.Tasks;

using AYN.Services.Data;
using AYN.Web.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;
        private readonly ITownsService townsService;
        private readonly IWebHostEnvironment environment;

        public UsersController(
            IUsersService usersService,
            ITownsService townsService,
            IWebHostEnvironment environment)
        {
            this.usersService = usersService;
            this.townsService = townsService;
            this.environment = environment;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Profile(string id)
        {
            var viewModel = this.usersService.GetProfileDetails<GetUserProfileDetailsViewModel>(id);

            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Follow(string id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (this.usersService.IsFollower(userId, id))
            {
                this.TempData["Message"] = "Already following this user!";
                return this.Redirect($"/Users/Profile?id={id}");
            }

            if (userId == id)
            {
                this.TempData["Message"] = "You cannot follow yourself!";
                return this.Redirect($"/Users/Profile?id={id}");
            }

            await this.usersService.Follow(userId, id);

            this.TempData["Message"] = "Successfully followed!";
            return this.Redirect($"/Users/Profile?id={id}");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Unfollow(string id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!this.usersService.IsFollower(userId, id))
            {
                this.TempData["Message"] = "You're not following this user!";
                return this.Redirect($"/Users/Profile?id={id}");
            }

            if (userId == id)
            {
                this.TempData["Message"] = "You cannot unfollow yourself!";
                return this.Redirect($"/Users/Profile?id={id}");
            }

            await this.usersService.Unfollow(userId, id);

            this.TempData["Message"] = "Successfully unfollowed!";
            return this.Redirect($"/Users/Profile?id={id}");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditGeneralInfo(string id)
        {
            var viewModel = new EditUserViewModel()
            {
                EditUserGeneralInfoViewModel = await this.usersService.GetByIdAsync<EditUserGeneralInfoViewModel>(id),
                Towns = await this.townsService.GetAllAsKeyValuePairsAsync(),
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditGeneralInfo(EditUserViewModel model, string id)
        {
            if (!this.usersService.IsUserExisting(model.EditUserGeneralInfoViewModel.Id))
            {
                return this.NotFound();
            }

            await this.usersService.EditAsync(model, this.environment.WebRootPath);

            return this.Redirect($"/Users/Profile?id={id}");
        }
    }
}
