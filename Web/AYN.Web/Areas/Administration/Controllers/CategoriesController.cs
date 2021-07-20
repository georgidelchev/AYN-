using System;
using System.Linq;
using System.Threading.Tasks;

using AYN.Services.Data.Interfaces;
using AYN.Web.ViewModels.Administration.Categories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.Areas.Administration.Controllers
{
    public class CategoriesController : AdministrationController
    {
        private readonly ICategoriesService categoriesService;
        private readonly IWebHostEnvironment environment;

        public CategoriesController(
            ICategoriesService categoriesService,
            IWebHostEnvironment environment)
        {
            this.categoriesService = categoriesService;
            this.environment = environment;
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
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            try
            {
                await this.categoriesService.CreateAsync(input);
            }
            catch (InvalidOperationException ioe)
            {
                this.ModelState.AddModelError(string.Empty, ioe.Message);

                return this.View(input);
            }

            return this.Redirect("/");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var viewModel = await this.categoriesService
                .GetByIdAsync<EditCategoryInputModel>(id);

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditCategoryInputModel input, int id)
        {
            var wwwrootPath = this.environment
                .WebRootPath;

            await this.categoriesService.UpdateAsync(input, id, wwwrootPath);

            return this.Redirect("/Administration/Categories/All");
        }

        [HttpGet]
        public IActionResult AddSubCategory()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSubCategory(AddSubCategoryInputModel input, int id)
        {
            await this.categoriesService.AddSubCategoryAsync(input, id);
            return this.Redirect("/Administration/Categories/All");
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
