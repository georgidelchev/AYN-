using System;
using System.Threading.Tasks;

using AYN.Services.Data.Interfaces;
using AYN.Web.ViewModels.Categories;
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

        public async Task<IActionResult> AddSubCategory(AddSubCategoryViewModel input, int id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            try
            {
                await this.categoriesService.AddSubCategoryAsync(input, id);
            }
            catch (InvalidOperationException ioe)
            {
                this.ModelState.AddModelError(string.Empty, ioe.Message);

                return this.View(input);
            }

            return this.Redirect("/");
        }
    }
}
