using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using AYN.Services.Data;
using AYN.Web.ViewModels.Ads;
using AYN.Web.ViewModels.Categories;
using AYN.Web.ViewModels.SubCategories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AYN.Web.Controllers
{
    public class AdsController : Controller
    {
        private readonly ICategoriesService categoriesService;
        private readonly ITownsService townsService;
        private readonly IAdsService adsService;
        private readonly IWebHostEnvironment environment;
        private readonly ISubCategoriesService subCategoriesService;

        public AdsController(
            ICategoriesService categoriesService,
            ITownsService townsService,
            IAdsService adsService,
            IWebHostEnvironment environment,
            ISubCategoriesService subCategoriesService)
        {
            this.categoriesService = categoriesService;
            this.townsService = townsService;
            this.adsService = adsService;
            this.environment = environment;
            this.subCategoriesService = subCategoriesService;
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

        [HttpGet]
        public async Task<IActionResult> GetFromSearch(string search, string town, string orderBy = "priceDesc", int id = 1)
        {
            var ads = this.adsService.GetFromSearch<GetRecentAdsViewModel>(search, orderBy, town);

            var itemsPerPage = 12;
            var viewModel = new ListGetFromSearchViewModel()
            {
                Count = ads.Count(),
                ItemsPerPage = itemsPerPage,
                AllFromSearch = ads.Skip((id - 1) * itemsPerPage).Take(itemsPerPage),
                PageNumber = id,
                OrderBy = orderBy,
                Town = town,
                Search = search,
                TotalResults = ads.Count(),
                AllCategoriesWithAllSubCategories = new Dictionary<CategoryViewModel, List<SubCategoryViewModel>>(),
            };
            var categories = this.categoriesService
                .GetAll<CategoryViewModel>();

            foreach (var category in categories)
            {
                var subCategories = await this.subCategoriesService
                    .GetAllByCategoryId<SubCategoryViewModel>(category.Id)
                    .ToListAsync();

                var categoryViewModel = new CategoryViewModel()
                {
                    Id = category.Id,
                    Name = category.Name,
                    PictureExtension = category.PictureExtension,
                };

                viewModel.AllCategoriesWithAllSubCategories
                    .Add(categoryViewModel, subCategories);
            }

            return this.View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetByCategory(string category, int id = 1)
        {
            return this.RedirectToAction(nameof(this.GetFromSearch), new { search = "a" });
        }
    }
}
