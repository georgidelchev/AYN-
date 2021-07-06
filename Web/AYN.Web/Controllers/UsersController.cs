﻿using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using AYN.Services.Data;
using AYN.Services.Data.Interfaces;
using AYN.Web.ViewModels.Ads;
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
        private readonly IWishlistsService wishlistsService;

        public UsersController(
            IUsersService usersService,
            ITownsService townsService,
            IWebHostEnvironment environment,
            IWishlistsService wishlistsService)
        {
            this.usersService = usersService;
            this.townsService = townsService;
            this.environment = environment;
            this.wishlistsService = wishlistsService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile(string id, int pagedId = 1)
        {
            var viewModel = await this.usersService
                .GetProfileDetails<GetUserProfileBaseDetailsViewModel>(id);

            viewModel.PagingId = pagedId;

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
        public async Task<IActionResult> EditGeneralInfo(EditUserViewModel model)
        {
            if (!this.usersService.IsUserExisting(model.EditUserGeneralInfoViewModel.Id))
            {
                return this.NotFound();
            }

            await this.usersService.EditAsync(model, this.environment.WebRootPath);

            return this.Redirect($"/Users/Profile?id={this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value}");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Wishlist(int id = 1)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var wishlist = await this.wishlistsService.Wishlist<WishlistAdsViewModel>(userId);
            var viewModel = new UserWishlistViewModel()
            {
                AdsWishlist = wishlist.Skip((id - 1) * 12).Take(12),
                Count = this.wishlistsService.Count(userId),
                ItemsPerPage = 12,
                PageNumber = id,
            };

            return this.View(viewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> AddToWishlist(string adId)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await this.wishlistsService.AddAsync(adId, userId);
            return this.Redirect($"/Ads/Details?id={adId}");
        }
    }
}
