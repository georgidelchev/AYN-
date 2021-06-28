using System.Collections.Generic;

namespace AYN.Web.ViewModels.Ads
{
    public class ListAdsViewModel
    {
        public IEnumerable<GetAdsViewModel> RecentAds { get; set; }
    }
}
