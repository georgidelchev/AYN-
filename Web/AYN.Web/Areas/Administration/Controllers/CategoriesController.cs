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
        public IActionResult All(int id = 1)
        {
            var categories = this.categoriesService.GetAll<GetAllCategoriesViewModel>().ToList();

            var viewModel = new ListAllCategoriesViewModel()
            {
                AllCategories = categories.Skip((id - 1) * 12).Take(12),
                Count = this.categoriesService.GetCounts().Item1,
                ItemsPerPage = 12,
                PageNumber = id,
            };

            return this.View(viewModel);
        }
    }
}
