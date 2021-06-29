using System.Linq;
using System.Threading.Tasks;

using AYN.Services.Data.Interfaces;
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

        public async Task<IViewComponentResult> InvokeAsync(string userId, int id)
        {
            var posts = await this.postsService.GetUserAllPostsAsync<GetUserPostsViewModel>(userId);

            var viewModel = new ListUserPostsViewModel()
            {
                UserPosts = posts.Skip((id - 1) * 6).Take(6),
                Count = this.postsService.GetCount(userId),
                ItemsPerPage = 6,
                PageNumber = id,
                PagedId = id,
            };

            return this.View(viewModel);
        }
    }
}
