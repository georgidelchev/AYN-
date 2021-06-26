using System.Collections.Generic;

namespace AYN.Web.ViewModels.Users
{
    public class ListFollowersViewModel
    {
        public IEnumerable<GetFollowersViewModel> Followers { get; set; }
    }
}
