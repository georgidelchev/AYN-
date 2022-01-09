using System.Threading.Tasks;

using AYN.Services.Data.Interfaces;
using AYN.Web.ViewModels.Ads;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.ViewComponents;

public class GetRecentlyPromotedAdsViewComponent : ViewComponent
{
    private readonly IAdsService adsService;

    public GetRecentlyPromotedAdsViewComponent(
        IAdsService adsService)
    {
        this.adsService = adsService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var viewModel = new ListAdsViewModel
        {
            Ads = await this.adsService.GetRecent12PromotedAdsAsync<GetAdsViewModel>(),
        };

        return this.View(viewModel);
    }
}
