using System.Collections.Generic;

namespace AYN.Web.ViewModels.Posts
{
    public class ListUserPostsViewModel
    {
        public IEnumerable<GetUserPostsViewModel> UserPosts { get; set; }
    }
}
