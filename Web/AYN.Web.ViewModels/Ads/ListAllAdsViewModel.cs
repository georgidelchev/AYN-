using System.Collections.Generic;

namespace AYN.Web.ViewModels.Ads
{
    public class ListAllAdsViewModel : PagingViewModel
    {
        public IEnumerable<GetRecentAdsViewModel> AllAds { get; set; }
    }
}
