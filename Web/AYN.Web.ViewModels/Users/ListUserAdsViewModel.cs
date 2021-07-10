using System.Collections.Generic;

using AYN.Web.ViewModels.Ads;

namespace AYN.Web.ViewModels.Users
{
    public class ListUserAdsViewModel
    {
        public IEnumerable<GetAdViewModel> Ads { get; set; }
    }
}
