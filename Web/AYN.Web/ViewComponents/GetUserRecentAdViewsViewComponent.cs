using System.Threading.Tasks;

using AYN.Services.Data.Interfaces;
using AYN.Web.ViewModels.Ads;
using AYN.Web.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.ViewComponents;

public class GetUserRecentAdViewsViewComponent : ViewComponent
{
    private readonly IUserLatestAdViewsService userLatestAdViewsService;

    public GetUserRecentAdViewsViewComponent(
        IUserLatestAdViewsService userLatestAdViewsService)
    {
        this.userLatestAdViewsService = userLatestAdViewsService;
    }

    public async Task<IViewComponentResult> InvokeAsync(string userId)
    {
        var viewModel = new ListUserAdsViewModel
        {
            Ads = await this.userLatestAdViewsService.GetUserLatestAdViews<GetAdViewModel>(userId),
        };

        return this.View(viewModel);
    }
}
