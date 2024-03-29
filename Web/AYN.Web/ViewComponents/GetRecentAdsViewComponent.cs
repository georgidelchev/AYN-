﻿using System.Threading.Tasks;

using AYN.Services.Data.Interfaces;
using AYN.Web.ViewModels.Ads;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.ViewComponents;

public class GetRecentAdsViewComponent : ViewComponent
{
    private readonly IAdsService adsService;

    public GetRecentAdsViewComponent(IAdsService adsService)
    {
        this.adsService = adsService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var viewModel = new ListAdsViewModel
        {
            Ads = await this.adsService
                .GetRecent12AdsAsync<GetAdsViewModel>(),
        };

        return this.View(viewModel);
    }
}
