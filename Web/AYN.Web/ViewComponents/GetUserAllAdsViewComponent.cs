using System.Threading.Tasks;

using AYN.Services.Data.Interfaces;
using AYN.Web.ViewModels.Ads;
using AYN.Web.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.ViewComponents;

public class GetUserAllAdsViewComponent : ViewComponent
{
    private readonly IAdsService adsService;

    public GetUserAllAdsViewComponent(
        IAdsService adsService)
    {
        this.adsService = adsService;
    }

    public async Task<IViewComponentResult> InvokeAsync(string userId)
    {
        var viewModel = new ListUserAdsViewModel
        {
            Ads = await this.adsService.GetUserAllAds<GetAdViewModel>(userId),
        };

        return this.View(viewModel);
    }
}
