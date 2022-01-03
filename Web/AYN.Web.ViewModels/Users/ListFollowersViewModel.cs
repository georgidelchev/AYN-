using System.Collections.Generic;

namespace AYN.Web.ViewModels.Users;

public class ListFollowersViewModel : PagingViewModel
{
    public IEnumerable<FollowerViewModel> Followers { get; set; }
}
