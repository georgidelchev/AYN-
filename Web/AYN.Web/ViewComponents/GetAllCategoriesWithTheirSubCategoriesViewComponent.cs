using System.Collections.Generic;
using System.Threading.Tasks;

using AYN.Services.Data.Interfaces;
using AYN.Web.ViewModels.Categories;
using AYN.Web.ViewModels.SubCategories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AYN.Web.ViewComponents;

public class GetAllCategoriesWithTheirSubCategoriesViewComponent : ViewComponent
{
    private readonly ICategoriesService categoriesService;
    private readonly ISubCategoriesService subCategoriesService;

    public GetAllCategoriesWithTheirSubCategoriesViewComponent(
        ICategoriesService categoriesService,
        ISubCategoriesService subCategoriesService)
    {
        this.categoriesService = categoriesService;
        this.subCategoriesService = subCategoriesService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var viewModel = new ListAllCategoriesViewModel
        {
            AllCategoriesWithAllSubCategories = new Dictionary<CategoryViewModel, List<SubCategoryViewModel>>(),
        };

        var categories = this.categoriesService
            .GetAll<CategoryViewModel>();

        foreach (var category in categories)
        {
            var subCategories = await this.subCategoriesService
                .GetAllByCategoryId<SubCategoryViewModel>(category.Id)
                .ToListAsync();

            var categoryViewModel = new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
                ImageUrl = category.ImageUrl,
            };

            viewModel.AllCategoriesWithAllSubCategories
                .Add(categoryViewModel, subCategories);
        }

        return this.View(viewModel);
    }
}
