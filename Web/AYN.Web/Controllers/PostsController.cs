using System.Security.Claims;
using System.Threading.Tasks;

using AYN.Services.Data;
using AYN.Web.ViewModels.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.Controllers
{
    public class PostsController : Controller
    {
        private readonly IPostsService postsService;

        public PostsController(
            IPostsService postsService)
        {
            this.postsService = postsService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(string title, string content)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await this.postsService.CreateAsync(title, content, userId);
            return this.Redirect($"/Users/Profile?id={userId}");
        }
    }
}
