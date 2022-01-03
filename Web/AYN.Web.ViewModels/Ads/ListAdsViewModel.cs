using System.Collections.Generic;

namespace AYN.Web.ViewModels.Ads;

public class ListAdsViewModel
{
    public IEnumerable<GetAdsViewModel> Ads { get; set; }
}
