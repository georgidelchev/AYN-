using System.Collections.Generic;

namespace AYN.Web.ViewModels.Users
{
    public class ListUserAdsViewModel
    {
        public IEnumerable<GetAdViewModel> Ads { get; set; }
    }
}
