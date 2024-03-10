using System.Threading.Tasks;

using AYN.Services.Data.Interfaces;
using AYN.Web.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.ViewComponents;

public class GetHomePageStatisticsViewComponent : ViewComponent
{
    private readonly IAdsService adsService;
    private readonly IUsersService usersService;

    public GetHomePageStatisticsViewComponent(
        IAdsService adsService,
        IUsersService usersService)
    {
        this.adsService = adsService;
        this.usersService = usersService;
    }

    public Task<IViewComponentResult> InvokeAsync()
    {
        var viewModel = new IndexPageStatisticsViewModel
        {
            AdsCount = this.adsService.GetCounts().Item2,
            UsersCount = this.usersService.GetCounts().Item3,
        };

        return Task.FromResult<IViewComponentResult>(this.View(viewModel));
    }
}
