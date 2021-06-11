using System.Collections.Generic;

namespace AYN.Web.ViewModels.Ads
{
    public class ListGetFromSearchViewModel : PagingViewModel
    {
        public IEnumerable<GetRecentAdsViewModel> AllFromSearch { get; set; }

        public int TotalResults { get; set; }
    }
}
