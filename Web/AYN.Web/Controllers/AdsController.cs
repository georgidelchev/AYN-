using System.Security.Claims;
using System.Threading.Tasks;

using AYN.Services.Data;
using AYN.Web.ViewModels.Ads;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.Controllers
{
    public class AdsController : Controller
    {
        private readonly ICategoriesService categoriesService;
        private readonly ITownsService townsService;
        private readonly IAdsService adsService;
        private readonly IWebHostEnvironment environment;

        public AdsController(
            ICategoriesService categoriesService,
            ITownsService townsService,
            IAdsService adsService,
            IWebHostEnvironment environment)
        {
            this.categoriesService = categoriesService;
            this.townsService = townsService;
            this.adsService = adsService;
            this.environment = environment;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new CreateAdInputModel()
            {
                Categories = this.categoriesService.GetAllAsKeyValuePairs(),
                Towns = this.townsService.GetAllAsKeyValuePairs(),
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAdInputModel input)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var wwwrootPath = this.environment
                .WebRootPath;

            await this.adsService.CreateAsync(input, userId, wwwrootPath);

            return this.Redirect("/");
        }

        [HttpGet]
        public IActionResult All(int id = 1)
        {
            var viewModel = new ListAllAdsViewModel()
            {
                Count = this.adsService.GetCount(),
                AllAds = this.adsService.GetAll<GetRecentAdsViewModel>(id, 12),
                ItemsPerPage = 12,
                PageNumber = id,
            };

            return this.View(viewModel);
        }

        public IActionResult GetFromSearch(string search, string orderBy = "dateDesc", int id = 1)
        {
            return this.View();
        }
    }
}
