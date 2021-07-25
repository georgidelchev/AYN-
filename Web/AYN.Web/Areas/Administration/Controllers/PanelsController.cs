using AYN.Services.Data.Interfaces;
using AYN.Web.ViewModels.Administration.Panels;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.Areas.Administration.Controllers
{
    public class PanelsController : AdministrationController
    {
        private readonly IUsersService usersService;
        private readonly IAdsService adsService;
        private readonly IReportsService reportsService;
        private readonly ICategoriesService categoriesService;
        private readonly IEmojisService emojisService;

        public PanelsController(
            IUsersService usersService,
            IAdsService adsService,
            IReportsService reportsService,
            ICategoriesService categoriesService,
            IEmojisService emojisService)
        {
            this.usersService = usersService;
            this.adsService = adsService;
            this.reportsService = reportsService;
            this.categoriesService = categoriesService;
            this.emojisService = emojisService;
        }

        public IActionResult Index()
        {
            var viewModel = new IndexViewModel()
            {
                UsersCounts = this.usersService.GetCounts(),
                AdsCount = this.adsService.GetCounts(),
                ReportsCount = this.reportsService.GetCounts(),
                CategoriesCount = this.categoriesService.GetCounts(),
                EmojisCount = this.emojisService.Count(),
            };

            return this.View(viewModel);
        }
    }
}
