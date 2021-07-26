using System.Collections.Generic;

namespace AYN.Web.ViewModels.Users
{
    public class ListFolloweesViewModel : PagingViewModel
    {
        public IEnumerable<FolloweeViewModel> Followees { get; set; }
    }
}
