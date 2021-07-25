using System.Threading.Tasks;

using AYN.Services.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.Areas.Administration.Controllers
{
    public class SubCategoriesController : AdministrationController
    {
        private readonly ISubCategoriesService subCategoriesService;

        public SubCategoriesController(
            ISubCategoriesService subCategoriesService)
        {
            this.subCategoriesService = subCategoriesService;
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (this.subCategoriesService.IsSubCategoryExisting(id))
            {
                await this.subCategoriesService.DeleteAsync(id);
            }

            return this.Redirect("/Administration/Categories/All");
        }
    }
}
