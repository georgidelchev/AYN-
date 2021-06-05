using System.Collections.Generic;

using AYN.Web.ViewModels.SubCategories;

namespace AYN.Web.ViewModels.Categories
{
    public class ListAllCategoriesViewModel
    {
        public Dictionary<CategoryViewModel, List<SubCategoryViewModel>> AllCategoriesWithAllSubCategories { get; set; }
    }
}
