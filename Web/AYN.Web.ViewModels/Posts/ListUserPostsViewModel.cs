using System.Collections.Generic;

namespace AYN.Web.ViewModels.Posts;

public class ListUserPostsViewModel : PagingViewModel
{
    public IEnumerable<GetUserPostsViewModel> UserPosts { get; set; }
}
