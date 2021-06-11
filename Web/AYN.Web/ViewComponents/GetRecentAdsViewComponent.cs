﻿using System.Threading.Tasks;

using AYN.Services.Data;
using AYN.Web.ViewModels.Ads;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.ViewComponents
{
    public class GetRecentAdsViewComponent : ViewComponent
    {
        private readonly IAdsService adsService;

        public GetRecentAdsViewComponent(IAdsService adsService)
        {
            this.adsService = adsService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var viewModel = new ListRecentAdsViewModel()
            {
                RecentAds = this.adsService.GetRecent12Ads<GetRecentAdsViewModel>(),
            };

            return this.View(viewModel);
        }
    }
}