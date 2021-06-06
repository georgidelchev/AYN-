using AYN.Common;
using AYN.Web.ViewModels.SubCategories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.Controllers
{
    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public class SubCategoriesController : Controller
    {
        [HttpGet]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreateSubCategoryInputModel input)
        {
            return this.Redirect("/");
        }
    }
}
