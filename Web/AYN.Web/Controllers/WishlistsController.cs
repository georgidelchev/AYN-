using System.Linq;
using System.Threading.Tasks;

using AYN.Services.Data.Interfaces;
using AYN.Web.Infrastructure.Extensions;
using AYN.Web.ViewModels.Ads;
using AYN.Web.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.Controllers;

public class WishlistsController : BaseController
{
    private readonly IWishlistsService wishlistsService;
    private readonly IAdsService adsService;

    public WishlistsController(
        IWishlistsService wishlistsService,
        IAdsService adsService)
    {
        this.wishlistsService = wishlistsService;
        this.adsService = adsService;
    }

    [HttpGet]
    public async Task<IActionResult> Favorites(int id = 1)
    {
        var userId = this.User.GetId();

        var wishlist = await this.wishlistsService.Wishlist<WishlistAdsViewModel>(userId);
        var viewModel = new UserWishlistViewModel
        {
            AdsWishlist = wishlist.Skip((id - 1) * 12).Take(12),
            Count = this.wishlistsService.Count(userId),
            ItemsPerPage = 12,
            PageNumber = id,
        };

        return this.View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> AddToWishlist(string adId, string redirectUrl = "/Wishlists/Favorites")
    {
        var userId = this.User.GetId();
        if (!this.adsService.IsAdExisting(adId))
        {
            return this.Redirect("/");
        }

        if (this.adsService.IsUserOwnsGivenAd(userId, adId))
        {
            return this.Redirect(redirectUrl);
        }

        await this.wishlistsService.AddAsync(adId, userId);
        return this.Redirect($"/Ads/Details?id={adId}");
    }

    [HttpGet]
    public async Task<IActionResult> RemoveFromWishlist(string id, string redirectUrl = "/Wishlists/Favorites")
    {
        var userId = this.User.GetId();

        if (!this.wishlistsService.IsUserHaveGivenAdInHisWishlist(id, userId))
        {
            return this.Redirect("/Wishlists/Favorites");
        }

        await this.wishlistsService.RemoveAsync(id, userId);
        return this.Redirect(redirectUrl);
    }

    [HttpGet]
    public IActionResult Count(string userId)
        => this.Json(this.wishlistsService.Count(userId));
}
