using System.Security.Claims;
using System.Threading.Tasks;

using AYN.Services.Data;
using AYN.Services.Data.Interfaces;
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

            return this.Redirect($"/Users/Profile/{userId}");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> All()
        {
            var vm = await this.postsService.GetUserAllPostsAsync<GetUserPostsViewModel>(this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            return this.View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var viewModel = await this.postsService.GetById<EditPostInputModel>(id);

            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(EditPostInputModel input)
        {
            await this.postsService.EditAsync(input);

            return this.Redirect($"/Users/Profile?id={this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value}");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            await this.postsService.DeleteAsync(id);

            return this.Redirect($"/Users/Profile?id={this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value}");
        }
    }
}
