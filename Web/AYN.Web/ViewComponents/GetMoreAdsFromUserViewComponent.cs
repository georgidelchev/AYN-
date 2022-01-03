using System.Security.Claims;
using System.Threading.Tasks;

using AYN.Services.Data.Interfaces;
using AYN.Web.ViewModels.Ads;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.ViewComponents;

public class GetMoreAdsFromUserViewComponent : ViewComponent
{
    private readonly IAdsService adsService;

    public GetMoreAdsFromUserViewComponent(
        IAdsService adsService)
    {
        this.adsService = adsService;
    }

    public async Task<IViewComponentResult> InvokeAsync(string townName, int categoryId, int subCategoryId, string currentAdId)
    {
        var userId = this.UserClaimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var viewModel = new ListMoreAdsByUserViewModel()
        {
            Ads = await this.adsService.GetMoreFromUserAds<MoreAdsByUserViewModel>(townName, categoryId, subCategoryId, userId, currentAdId),
        };

        return this.View(viewModel);
    }
}
