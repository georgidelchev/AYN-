using System.Collections.Generic;

namespace AYN.Web.ViewModels.Ads
{
    public class ListRecentAdsViewModel
    {
        public IEnumerable<GetRecentAdsViewModel> RecentAds { get; set; }
    }
}
