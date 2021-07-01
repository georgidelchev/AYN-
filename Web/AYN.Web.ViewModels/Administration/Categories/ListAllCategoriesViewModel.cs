using System.Collections.Generic;

namespace AYN.Web.ViewModels.Administration.Categories
{
    public class ListAllCategoriesViewModel : PagingViewModel
    {
        public IEnumerable<GetAllCategoriesViewModel> AllCategories { get; set; }
    }
}
