using System.Threading.Tasks;

using AYN.Common;
using AYN.Services.Data;
using AYN.Web.ViewModels.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.Controllers
{
    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public class CategoriesController : Controller
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

            var wwwrootPath = this.environment
                .WebRootPath;

            await this.categoriesService.CreateAsync(input, wwwrootPath);

            return this.Redirect("/");
        }
    }
}
