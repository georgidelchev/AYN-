using System.Collections.Generic;

using AYN.Web.ViewModels.Ads;

namespace AYN.Web.ViewModels.Users
{
    public class UserWishlistViewModel : PagingViewModel
    {
        public IEnumerable<WishlistAdsViewModel> AdsWishlist { get; set; }
    }
}
