using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using AYN.Services.Data.Interfaces;
using AYN.Web.ViewModels.Ads;
using AYN.Web.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.Controllers
{
    public class WishlistsController : Controller
    {
        private readonly IWishlistsService wishlistsService;

        public WishlistsController(
            IWishlistsService wishlistsService)
        {
            this.wishlistsService = wishlistsService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Favorites(int id = 1)
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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> RemoveFromWishlist(string id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await this.wishlistsService.RemoveAsync(id, userId);
            return this.Redirect($"/Wishlists/Favorites");
        }

        [HttpGet]
        public IActionResult Count(string userId)
        {
            var data = this.wishlistsService.Count(userId);

            return this.Json(data);
        }
    }
}
