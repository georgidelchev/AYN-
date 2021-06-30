using AYN.Services.Data.Interfaces;
using AYN.Web.ViewModels.Administration.Dashboard;

using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.Areas.Administration.Controllers
{
    public class PanelsController : AdministrationController
    {
        private readonly IUsersService usersService;
        private readonly IAdsService adsService;
        private readonly IReportsService reportsService;

        public PanelsController(
            IUsersService usersService,
            IAdsService adsService,
            IReportsService reportsService)
        {
            this.usersService = usersService;
            this.adsService = adsService;
            this.reportsService = reportsService;
        }

        public IActionResult Index()
        {
            var viewModel = new IndexViewModel()
            {
            };

            return this.View(viewModel);
        }
    }
}
