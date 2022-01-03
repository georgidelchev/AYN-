using System.Collections.Generic;

using AYN.Web.ViewModels.Categories;
using AYN.Web.ViewModels.SubCategories;

namespace AYN.Web.ViewModels.Ads;

public class ListGetFromSearchViewModel : PagingViewModel
{
    public IEnumerable<GetAdsViewModel> AllFromSearch { get; set; }

    public Dictionary<CategoryViewModel, List<SubCategoryViewModel>> AllCategoriesWithAllSubCategories { get; set; }

    public int TotalResults { get; set; }
}
