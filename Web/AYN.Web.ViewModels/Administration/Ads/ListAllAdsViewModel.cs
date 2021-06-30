using System.Collections.Generic;

namespace AYN.Web.ViewModels.Administration.Ads
{
    public class ListAllAdsViewModel : PagingViewModel
    {
        public IEnumerable<GetAllAdsViewModel> AllAds { get; set; }
    }
}
