using System.Threading.Tasks;

using AYN.Services.Data.Interfaces;
using AYN.Web.ViewModels.Administration.SubCategories;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.Areas.Administration.Controllers;

public class SubCategoriesController : AdministrationController
{
    private readonly ISubCategoriesService subCategoriesService;

    public SubCategoriesController(
        ISubCategoriesService subCategoriesService)
    {
        this.subCategoriesService = subCategoriesService;
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var viewModel = await this.subCategoriesService.GetByIdAsync<EditSubCategoryInputModel>(id);
        return this.View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditSubCategoryInputModel input)
    {
        await this.subCategoriesService.EditAsync(input);
        return this.Redirect("/Administration/Categories/All");
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

    [HttpGet]
    public async Task<IActionResult> UnDelete(int id)
    {
        await this.subCategoriesService.UnDeleteAsync(id);

        return this.Redirect("/Administration/Categories/All");
    }
}
