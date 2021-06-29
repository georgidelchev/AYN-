using System.Threading.Tasks;

using AYN.Services.Data;
using AYN.Web.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.ViewComponents
{
    public class GetUserRecentAdsViewComponent : ViewComponent
    {
        private readonly IAdsService adsService;

        public GetUserRecentAdsViewComponent(
            IAdsService adsService)
        {
            this.adsService = adsService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string userId)
        {
            var viewModel = new ListUserAdsViewModel()
            {
                Ads = await this.adsService.GetUserRecentAds<GetAdViewModel>(userId),
            };

            return this.View(viewModel);
        }
    }
}
