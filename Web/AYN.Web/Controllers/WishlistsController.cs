using AYN.Services.Data.Interfaces;
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
        public IActionResult Count(string userId)
        {
            var data = this.wishlistsService.Count(userId);

            return this.Json(data);
        }
    }
}
