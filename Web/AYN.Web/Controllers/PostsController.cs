using System.Security.Claims;
using System.Threading.Tasks;

using AYN.Services.Data.Interfaces;
using AYN.Web.Infrastructure.Extensions;
using AYN.Web.ViewModels.Posts;
using Microsoft.AspNetCore.Mvc;

using static AYN.Common.AttributeConstraints;

namespace AYN.Web.Controllers;

public class PostsController : BaseController
{
    private readonly IPostsService postsService;

    public PostsController(
        IPostsService postsService)
    {
        this.postsService = postsService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(string title, string content)
    {
        var userId = this.User.GetId();

        if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content) ||
            title.Length > PostTitleMaxLength || title.Length < PostTitleMinLength ||
            content.Length > PostContentMaxLength || content.Length < PostContentMinLength)
        {
            return this.Redirect($"/Users/Profile/{userId}");
        }

        await this.postsService.CreateAsync(title, content, userId);

        return this.Redirect($"/Users/Profile/{userId}");
    }

    [HttpGet]
    public async Task<IActionResult> All()
    {
        var vm = await this.postsService.GetUserAllPostsAsync<GetUserPostsViewModel>(this.User.GetId());

        return this.View();
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var viewModel = await this.postsService.GetById<EditPostInputModel>(id);

        return this.View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditPostInputModel input)
    {
        await this.postsService.EditAsync(input);

        return this.Redirect($"/Users/Profile?id={this.User.GetId()}");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await this.postsService.DeleteAsync(id);

        return this.Redirect($"/Users/Profile?id={this.User.GetId()}");
    }
}
