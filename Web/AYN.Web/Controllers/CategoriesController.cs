using AYN.Services.Data.Interfaces;
using AYN.Web.ViewModels.SubCategories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ISubCategoriesService subCategoriesService;

        public CategoriesController(
            ISubCategoriesService subCategoriesService)
        {
            this.subCategoriesService = subCategoriesService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetSubCategories(int id)
        {
            var sc = this.subCategoriesService
                .GetAllByCategoryId<SubCategoryViewModel>(id);

            return this.Json(sc);
        }
    }
}
