using AYN.Services.Data;
using AYN.Web.ViewModels.Ads;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.Controllers
{
    public class AdsController : Controller
    {
        private readonly ICategoriesService categoriesService;
        private readonly ISubCategoriesService subCategoriesService;
        private readonly ITownsService townsService;
        private readonly IAddressesService addressesService;

        public AdsController(
            ICategoriesService categoriesService,
            ISubCategoriesService subCategoriesService,
            ITownsService townsService)
        {
            this.categoriesService = categoriesService;
            this.subCategoriesService = subCategoriesService;
            this.townsService = townsService;
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
        public IActionResult Create(CreateAdInputModel input)
        {
            return this.Redirect("/");
        }
    }
}
