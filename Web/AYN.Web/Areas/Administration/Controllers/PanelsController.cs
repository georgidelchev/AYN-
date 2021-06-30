using AYN.Services.Data.Interfaces;
using AYN.Web.Areas.Administration.ViewModels.Panels;

using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.Areas.Administration.Controllers
{
    public class PanelsController : AdministrationController
    {
        private readonly IUsersService usersService;
        private readonly IAdsService adsService;
        private readonly IReportsService reportsService;
        private readonly ICategoriesService categoriesService;

        public PanelsController(
            IUsersService usersService,
            IAdsService adsService,
            IReportsService reportsService,
            ICategoriesService categoriesService)
        {
            this.usersService = usersService;
            this.adsService = adsService;
            this.reportsService = reportsService;
            this.categoriesService = categoriesService;
        }

        public IActionResult Index()
        {
            var viewModel = new IndexViewModel()
            {
                UsersCounts = this.usersService.GetCounts(),
                AdsCount = this.adsService.GetCounts(),
                ReportsCount = this.reportsService.GetCounts(),
                CategoriesCount = this.categoriesService.GetCounts(),
            };

            return this.View(viewModel);
        }
    }
}
