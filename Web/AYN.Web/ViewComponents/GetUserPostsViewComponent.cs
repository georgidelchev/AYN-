using System.Linq;
using System.Threading.Tasks;

using AYN.Services.Data;
using AYN.Web.ViewModels.Posts;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.ViewComponents
{
    public class GetUserPostsViewComponent : ViewComponent
    {
        private readonly IPostsService postsService;

        public GetUserPostsViewComponent(
            IPostsService postsService)
        {
            this.postsService = postsService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string userId, int id = 1)
        {
            var posts = await this.postsService.GetUserAllPostsAsync<GetUserPostsViewModel>(userId);

            var viewModel = new ListUserPostsViewModel()
            {
                UserPosts = posts.Skip((id - 1) * 12).Take(12),
                Count = this.postsService.GetCount(userId),
                ItemsPerPage = 12,
                PageNumber = id,
            };

            return this.View(viewModel);
        }
    }
}
