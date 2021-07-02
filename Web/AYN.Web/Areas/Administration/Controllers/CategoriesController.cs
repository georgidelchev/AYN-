using System;
using System.Linq;
using System.Threading.Tasks;

using AYN.Services.Data.Interfaces;
using AYN.Web.ViewModels.Administration.Categories;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.Areas.Administration.Controllers
{
    public class CategoriesController : AdministrationController
    {
        private readonly ICategoriesService categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        [HttpGet]
        public async Task<IActionResult> All(int id = 1)
        {
            var categories = await this.categoriesService
                .GetAllWithDeletedAsync<GetAllCategoriesViewModel>();

            var viewModel = new ListAllCategoriesViewModel()
            {
                AllCategories = categories.Skip((id - 1) * 12).Take(12),
                Count = this.categoriesService.GetTotalCount(),
                ItemsPerPage = 12,
                PageNumber = id,
            };

            return this.View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await this.categoriesService.DeleteAsync(id);
            return this.Redirect("/Administration/Categories/All");
        }

        [HttpGet]
        public async Task<IActionResult> UnDelete(int id)
        {
            await this.categoriesService.UnDeleteAsync(id);
            return this.Redirect("/Administration/Categories/All");
        }
    }
}
